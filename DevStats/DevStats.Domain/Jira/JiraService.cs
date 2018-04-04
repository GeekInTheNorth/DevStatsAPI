using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using DevStats.Domain.DefectAnalysis;
using DevStats.Domain.Jira.JsonModels;
using DevStats.Domain.Jira.Logging;
using DevStats.Domain.SystemProperties;

namespace DevStats.Domain.Jira
{
    public class JiraService : IJiraService
    {
        private readonly IJiraLogRepository loggingRepository;
        private readonly IJiraSender jiraSender;
        private readonly IWorkLogRepository workLogRepository;
        private readonly IDefectRepository defectRepository;
        private readonly IDefectScoringRepository defectScoringRepository;
        private readonly IJiraIdValidator idValidator;
        private readonly ISystemPropertyRepository systemPropertyRepository;
        private const string JiraIssuePath = @"{0}/rest/api/2/issue/{1}";
        private const string JiraAssigneePath = @"{0}/rest/api/2/issue/{1}/assignee";
        private const string JiraIssuePathWithFields = @"{0}/rest/api/2/issue/{1}?fields={2}";
        private const string JiraCreatePath = @"{0}/rest/api/2/issue/";
        private const string JiraTransitionPath = @"{0}/rest/api/latest/issue/{1}/transitions";
        private const string CoreIssueIdRegex = "({0})[-][0-9]{{1,6}}";
        private const string JiraIssueSearchPath = @"{0}/rest/api/2/search?jql={1}&validateQuery=Warn";
        private const string JiraUserGroupSearchPath = @"{0}/rest/api/2/group?groupname={1}&expand=users";
        private const string VersionNumberRegEx = "[0-9]{1,2}[.]{1}[0-9]{1,3}";

        public JiraService(
            IJiraLogRepository loggingRepository,
            IJiraSender jiraSender,
            IWorkLogRepository workLogRepository,
            IDefectRepository defectRepository,
            IDefectScoringRepository defectScoringRepository,
            IJiraIdValidator idValidator,
            ISystemPropertyRepository systemPropertyRepository)
        {
            if (loggingRepository == null) throw new ArgumentNullException(nameof(loggingRepository));
            if (jiraSender == null) throw new ArgumentNullException(nameof(jiraSender));
            if (workLogRepository == null) throw new ArgumentNullException(nameof(workLogRepository));
            if (defectRepository == null) throw new ArgumentNullException(nameof(defectRepository));
            if (defectScoringRepository == null) throw new ArgumentNullException(nameof(defectScoringRepository));
            if (idValidator == null) throw new ArgumentNullException(nameof(idValidator));
            if (systemPropertyRepository == null) throw new ArgumentNullException(nameof(systemPropertyRepository));

            this.loggingRepository = loggingRepository;
            this.jiraSender = jiraSender;
            this.workLogRepository = workLogRepository;
            this.defectRepository = defectRepository;
            this.defectScoringRepository = defectScoringRepository;
            this.idValidator = idValidator;
            this.systemPropertyRepository = systemPropertyRepository;
        }

        public void ProcessStoryCreate(string jiraId)
        {
            if (!idValidator.Validate(jiraId))
            {
                var message = "Invalid Jira Id Provided.";
                loggingRepository.Log(jiraId, "Process Story Create", message, false);

                throw new ArgumentException(message);
            }

            try
            {
                var storyUrl = string.Format(JiraIssuePath, GetApiRoot(), jiraId);
                var story = jiraSender.Get<Issue>(storyUrl);
                var taskSummaries = story.Fields.Subtasks ?? new Issue[] { };
                var tasks = new Issue[] { };

                if (taskSummaries.Any())
                {
                    var taskSearch = string.Format("issueKey in ({0})", string.Join(",", taskSummaries.Select(x => x.Key)));
                    var taskUrl = string.Format(JiraIssueSearchPath, GetApiRoot(), HttpUtility.JavaScriptStringEncode(taskSearch));
                    tasks = jiraSender.Get<JiraIssues>(taskUrl).Issues ?? new Issue[] { };
                }

                var hasPOTask = tasks.Any(x => x.Fields.TaskType != null && x.Fields.TaskType.Value == "PO Review");
                var hasMergeTask = tasks.Any(x => x.Fields.TaskType != null && x.Fields.TaskType.Value == "Merge");

                if (!hasPOTask) CreateProductOwnerTask(story.Id, story.Key);
                if (!hasMergeTask) CreateMergeTask(story.Id, story.Key);
            }
            catch (Exception ex)
            {
                loggingRepository.Log(jiraId, "Process Create Sub-Tasks", ex.Message, false);
            }
        }

        public void ProcessStoryUpdate(string jiraId)
        {
            if (!idValidator.Validate(jiraId))
            {
                var message = "Invalid Jira Id Provided.";
                loggingRepository.Log(jiraId, "Process Story Update", message, false);

                throw new ArgumentException(message);
            }

            try
            {
                var storyUrl = string.Format(JiraIssuePath, GetApiRoot(), jiraId);
                var story = jiraSender.Get<Issue>(storyUrl);
                var taskSummaries = story.Fields.Subtasks ?? new Issue[] { };
                var tasks = new Issue[] { };

                if (taskSummaries.Any())
                {
                    var taskSearch = string.Format("issueKey in ({0})", string.Join(",", taskSummaries.Select(x => x.Key)));
                    var taskUrl = "{0}/rest/api/2/search?jql={1}&fields=parent,timetracking,summary,issuetype,status,subtasks,resolutiondate,customfield_13701,customfield_13709,assignee,customfield_13700,timeoriginalestimate";
                    taskUrl = string.Format(taskUrl, GetApiRoot(), HttpUtility.JavaScriptStringEncode(taskSearch));

                    tasks = jiraSender.Get<JiraIssues>(taskUrl).Issues ?? new Issue[] { };
                }

                CopyTeamFromStoryToTask(story, tasks);
                CopyVersionFromStoryToTask(story, tasks);
                ProcessWorkLogs(story, tasks);
                UpdateDefectAnalysis(story);
                UpdateBugFromSubtasks(story, tasks);
                UpdateReleaseOriginated(story);
                UpdateAssigneeOnParent(story, tasks);
            }
            catch (Exception ex)
            {
                loggingRepository.Log(jiraId, "Process Story Update", string.Format("Unexpected Error: {0}", ex.Message), false);
            }
        }

        public void ProcessSubtaskUpdate(string jiraId)
        {
            if (!idValidator.Validate(jiraId))
            {
                var message = "Invalid Jira Id Provided.";
                loggingRepository.Log(jiraId, "Process Story Update", message, false);

                throw new ArgumentException(message);
            }

            try
            {
                var taskUrl = string.Format(JiraIssuePath, GetApiRoot(), jiraId);
                var task = jiraSender.Get<Issue>(taskUrl);

                if (task.Fields.Parent == null)
                {
                    loggingRepository.Log(jiraId, "Process Sub-Task Update", "No parent to update", false);
                    return;
                }

                if (task.Fields.Parent.Fields.Status.Name.Equals("In Progress") || !task.Fields.Status.Name.Equals("In Progress"))
                {
                    loggingRepository.Log(jiraId, "Process Sub-Task Update", "No updates to apply to parent", true);
                    return;
                }

                var transistionUrl = string.Format(JiraTransitionPath, GetApiRoot(), task.Fields.Parent.Key);
                var transitions = jiraSender.Get<IssueTransitions>(transistionUrl);

                if (transitions == null || transitions.Transitions == null || !transitions.Transitions.Any(x => x.Name == task.Fields.Status.Name))
                {
                    loggingRepository.Log(jiraId, "Process Sub-Task Update", "No known transitions for updating the parent", false);
                    return;
                }

                var transition = transitions.Transitions.First(x => x.Name == task.Fields.Status.Name);
                var json = "{ \"update\": { \"comment\": [{ \"add\": { \"body\": \"@@COMMENT@@\" } } ] }, \"transition\": { \"id\": \"@@ID@@\" } }";
                var comment = string.Format("Moved to {0} to match subtask: {1}", task.Fields.Status.Name, jiraId);
                json = json.Replace("@@ID@@", transition.Id.ToString())
                           .Replace("@@COMMENT@@", HttpUtility.JavaScriptStringEncode(comment));

                var postResult = jiraSender.Post(transistionUrl, json);
                loggingRepository.Log(jiraId, "Process Sub-Task Update: Update Parent", postResult.Response, postResult.WasSuccessful);
            }
            catch (Exception ex)
            {
                loggingRepository.Log(jiraId, "Process Sub-Task Update", string.Format("Unexpected Error: {0}", ex.Message), false);
            }
        }

        public void ProcessImpactAnalysisModelUpdate(string jiraId)
        {
            if (!idValidator.Validate(jiraId))
            {
                var message = "Invalid Jira Id Provided.";
                loggingRepository.Log(jiraId, "Process Story Update", message, false);

                throw new ArgumentException(message);
            }

            try
            {
                var fields = "customfield_15716,customfield_15717,customfield_15718,customfield_15705";
                var bugUrl = string.Format(JiraIssuePathWithFields, GetApiRoot(), jiraId, fields);
                var bug = jiraSender.Get<Issue>(bugUrl);
                var action = string.Format("Process Bug Update: Update IAM Score on Bug {0}", bug.Key);

                var score = GetScore(defectScoringRepository.GetUserImpactScores(), bug.Fields.DefectScaleOfUserImpact);
                score += GetScore(defectScoringRepository.GetFunctionalImpactScores(), bug.Fields.DefectScaleOfFunctionalImpact);
                score += GetScore(defectScoringRepository.GetWorkAroundScores(), bug.Fields.DefectWorkaround);

                if (!bug.Fields.DefectScore.HasValue || score != bug.Fields.DefectScore.Value)
                {
                    var json = "{ \"update\" : { \"customfield_15705\" : [{\"set\" : @@FieldValue@@ }] }}";
                    json = json.Replace("@@FieldValue@@", score.ToString("F2"));

                    var url = string.Format(JiraIssuePath, GetApiRoot(), bug.Key);
                    var putResult = jiraSender.Put(url, json);

                    loggingRepository.Log(bug.Id, bug.Key, action, putResult.Response, putResult.WasSuccessful);
                }
                else
                    loggingRepository.Log(bug.Id, bug.Key, action, "No updates to apply.", true);
            }
            catch (Exception ex)
            {
                loggingRepository.Log(jiraId, "Process Bug Update", string.Format("Unexpected Error: {0}", ex.Message), false);
            }
        }

        public IEnumerable<JiraAudit> GetJiraAudit(DateTime from, DateTime to)
        {
            return loggingRepository.Get(from, to);
        }

        public IEnumerable<JiraStateSummary> GetStateSummaries(string requestData)
        {
            var projects = GetJiraProjects();
            var regExStr = string.Format(CoreIssueIdRegex, string.Join("|", projects));
            var matches = Regex.Matches(requestData, regExStr);

            if (matches.Count == 0) return new List<JiraStateSummary>();

            var jql = string.Format("issueKey in ({0})", string.Join(",", matches.OfType<Match>().Select(x => x.Value)));
            var url = string.Format(JiraIssueSearchPath, GetApiRoot(), WebUtility.UrlEncode(jql));

            var response = jiraSender.Get<JiraIssues>(url);

            return (from issue in response.Issues
                    select new JiraStateSummary(issue));
        }

        public IEnumerable<JiraDefectSummary> GetDefects()
        {
            var supportUserNames = GetUserNames(GetServiceDeskGroup());
            var jql = FilterBuilder.Create()
                                   .WithProjects(GetJiraProjects())
                                   .WithIssueTypesOf(JiraIssueType.Bugs)
                                   .WithIssueStatesOf(JiraState.UnResolved)
                                   .Build();

            var url = string.Format(JiraIssueSearchPath, GetApiRoot(), WebUtility.UrlEncode(jql));
            var response = jiraSender.Get<JiraIssues>(url);

            return (from issue in response.Issues
                    select new JiraDefectSummary(issue, supportUserNames));
        }

        public void UpdateDefectAnalysis(Issue story)
        {
            var action = string.Format("Process Story Update: Update defect analysis for {0}", story.Key);

            try
            {
                var defects = new List<JiraDefect> { new JiraDefect(story) };

                defectRepository.Save(defects);

                loggingRepository.Log(story.Id, story.Key, action, string.Empty, true);
            }
            catch (Exception ex)
            {
                loggingRepository.Log(story.Id, story.Key, action, ex.Message, false);
            }
        }

        public void Delete(string jiraId)
        {
            var action = string.Format("Process Delete: Update defect analysis for {0}", jiraId);

            if (!idValidator.Validate(jiraId))
            {
                var message = "Invalid Jira Id Provided.";
                loggingRepository.Log(jiraId, jiraId, action, message, false);

                throw new ArgumentException(message);
            }

            try
            {
                defectRepository.Delete(jiraId);
                loggingRepository.Log(jiraId, jiraId, action, string.Empty, true);
            }
            catch (Exception ex)
            {
                loggingRepository.Log(jiraId, jiraId, action, ex.Message, false);
            }
        }

        private IEnumerable<string> GetUserNames(string groupName)
        {
            var url = string.Format(JiraUserGroupSearchPath, GetApiRoot(), WebUtility.UrlEncode(groupName));
            var response = jiraSender.Get<UserGroup>(url);

            return response.Users.Users.Select(x => x.Name);
        }

        private void CreateProductOwnerTask(string issueId, string displayIssueId)
        {
            var project = displayIssueId.Split('-')[0];
            var buffer = JsonFiles.CreateSubTask;
            var json = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            json = json.Replace("@@ID@@", displayIssueId)
                       .Replace("@@TASKID@@", "11508")
                       .Replace("@@TASKNAME@@", "PO Review")
                       .Replace("@@PROJECT@@", project)
                       .Replace("@@DESCRIPTION@@", "Product Owner Review");

            var url = string.Format(JiraCreatePath, GetApiRoot());
            var postResult = jiraSender.Post(url, json);

            loggingRepository.Log(issueId, displayIssueId, "Create Product Owner Review Task", postResult.Response, postResult.WasSuccessful);
        }

        private void CreateMergeTask(string issueId, string displayIssueId)
        {
            var project = displayIssueId.Split('-')[0];
            var buffer = JsonFiles.CreateSubTask;
            var json = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            json = json.Replace("@@ID@@", displayIssueId)
                       .Replace("@@TASKID@@", "11510")
                       .Replace("@@TASKNAME@@", "Merge")
                       .Replace("@@PROJECT@@", project)
                       .Replace("@@DESCRIPTION@@", "Merge to Master");

            var url = string.Format(JiraCreatePath, GetApiRoot());
            var postResult = jiraSender.Post(url, json);

            loggingRepository.Log(issueId, displayIssueId, "Create Merge Task", postResult.Response, postResult.WasSuccessful);
        }

        private void CopyTeamFromStoryToTask(Issue story, IEnumerable<Issue> tasks)
        {
            if (tasks == null || !tasks.Any())
            {
                loggingRepository.Log(story.Id, story.Key, "Process Story Update: Update Cascade Team on Sub-Tasks", "No Sub-Tasks to update", true);
                return;
            }

            foreach (var task in tasks)
            {
                var storyTeam = story.Fields.CascadeTeam != null ? story.Fields.CascadeTeam.Value : string.Empty;
                var taskTeam = task.Fields.CascadeTeam != null ? task.Fields.CascadeTeam.Value : string.Empty;

                if (storyTeam != taskTeam)
                {
                    var json = "{ \"update\" : { \"@@fieldName@@\" : [{\"set\" : {\"value\" : \"@@FieldValue@@\"} }] }}";
                    json = json.Replace("@@fieldName@@", "customfield_13700")
                               .Replace("@@FieldValue@@", storyTeam);

                    var url = string.Format(JiraIssuePath, GetApiRoot(), task.Key);
                    var action = string.Format("Process Story Update: Update Cascade Team on Sub-Task {0}", task.Key);
                    var putResult = jiraSender.Put(url, json);

                    loggingRepository.Log(task.Id, task.Key, action, putResult.Response, putResult.WasSuccessful);
                }
            }
        }

        private void CopyVersionFromStoryToTask(Issue story, IEnumerable<Issue> tasks)
        {
            if (tasks == null || !tasks.Any())
            {
                loggingRepository.Log(story.Id, story.Key, "Process Story Update: Update Fix Version on Sub-Tasks", "No Sub-Tasks to update", true);
                return;
            }

            var regEx = new Regex("[0-9][.][0-9]{1,2}");
            var storyVersions = story.Fields.FixVersions != null ? story.Fields.FixVersions.Where(x => regEx.IsMatch(x.Name)).Select(x => x.Name) : Enumerable.Empty<string>();

            if (!storyVersions.Any())
            {
                loggingRepository.Log(story.Id, story.Key, "Process Story Update: Update Fix Version on Sub-Tasks", "No version to copy", true);
                return;
            }

            var versions = storyVersions.Select(x => "{ \"name\": \"" + x + "\" }");
            var json = "{ \"update\" : { \"fixVersions\" : [ { \"set\": [";
            json += string.Join(",", versions);
            json += "] } ] } }";

            foreach (var task in tasks)
            {
                var url = string.Format(JiraIssuePath, GetApiRoot(), task.Key);
                var action = string.Format("Process Story Update: Update Fix Version on Sub-Task {0}", task.Key);
                var putResult = jiraSender.Put(url, json);

                loggingRepository.Log(task.Id, task.Key, action, putResult.Response, putResult.WasSuccessful);
            }
        }

        private void ProcessWorkLogs(Issue story, IEnumerable<Issue> tasks)
        {
            if (story.Fields.Status.Name != "Done") return;

            // Worklogs may also be saved against stories
            var tasksToCheck = tasks == null ? new List<Issue>() : tasks.ToList();
            tasksToCheck.Add(story);

            var action = "Process Story Completion: Record Work Logs";

            try
            {
                var storyEffort = new StoryEffort(story, tasksToCheck);
                workLogRepository.Save(storyEffort);
                loggingRepository.Log(story.Id, story.Key, action, string.Empty, true);
            }
            catch (Exception ex)
            {
                loggingRepository.Log(story.Id, story.Key, action, ex.Message, false);
            }
        }

        private void UpdateBugFromSubtasks(Issue story, IEnumerable<Issue> tasks)
        {
            if (story.Fields.Status.Name != "Done" || story.Fields.Issuetype.Name != "Bug") return;

            if (tasks == null || !tasks.Any())
            {
                loggingRepository.Log(story.Id, story.Key, "Process Story Update: Update Bug From Sub-Tasks", "No Sub-Tasks to process", true);
                return;
            }

            var json = "{ \"update\" : { \"customfield_15500\" : [{\"set\" : null }] }}";
            if (tasks.Any(x => x.Fields.TaskType.Value == "Rework"))
                json = "{ \"update\" : { \"customfield_15500\" : [{\"set\" : [ { \"value\": \"Yes\" } ] }] }}";

            var url = string.Format(JiraIssuePath, GetApiRoot(), story.Key);
            var action = string.Format("Process Story Update: Set 'Testing Defect Returned' for {0}", story.Key);
            var putResult = jiraSender.Put(url, json);

            loggingRepository.Log(story.Id, story.Key, action, putResult.Response, putResult.WasSuccessful);
        }

        private void UpdateAssigneeOnParent(Issue story, IEnumerable<Issue> tasks)
        {
            if (tasks == null || !tasks.Any())
            {
                loggingRepository.Log(story.Id, story.Key, "Process Story Update: Update Assignee on Parent Task", "No Sub-Tasks to process", true);
                return;
            }

            var newAssignee = tasks.Where(x => x.Fields.TaskType.Value == "Dev" && x.Fields.Assignee != null && x.Fields.Timeoriginalestimate.HasValue)
                                   .GroupBy(x => x.Fields.Assignee.Name)
                                   .Select(x => new { Developer = x.Key, Hours = x.Sum(y => y.Fields.Timeoriginalestimate ?? 0) })
                                   .OrderByDescending(x => x.Hours)
                                   .Select(x => x.Developer).FirstOrDefault();

            var oldAssignee = story.Fields.Assignee == null ? "UNKNOWN" : story.Fields.Assignee.Name;

            if (!string.IsNullOrWhiteSpace(newAssignee) && !newAssignee.Equals(oldAssignee, StringComparison.CurrentCultureIgnoreCase))
            {
                var url = string.Format(JiraAssigneePath, GetApiRoot(), story.Key);
                var json = "{ \"name\" : \"@@name@@\" }";
                json = json.Replace("@@name@@", newAssignee);

                var action = string.Format("Process Story Update: Set 'Assignee' for {0}", story.Key);
                var putResult = jiraSender.Put(url, json);

                loggingRepository.Log(story.Id, story.Key, action, putResult.Response, putResult.WasSuccessful);
            }
        }

        private void UpdateReleaseOriginated(Issue story)
        {
            if (story.Fields.Issuetype.Name != "Bug") return;

            var action = string.Format("Process Story Update: Set 'Release Originated' for {0}", story.Key);

            try
            {
                var affectedVersions = story.Fields.AffectedVersions ?? Enumerable.Empty<FixVersion>();
                var regEx = new Regex(VersionNumberRegEx);
                var earliestAffected = affectedVersions.Where(x => regEx.IsMatch(x.Name))
                                                       .Select(x => new { MatchName = regEx.Match(x.Name).Value, ReleaseName = x.Name, ReleaseId = x.Id })
                                                       .OrderBy(x => x.MatchName)
                                                       .FirstOrDefault();

                // Can't find a valid affected version
                if (earliestAffected == null) return;

                if (story.Fields.ReleaseOriginated == null || !story.Fields.ReleaseOriginated.Name.Equals(earliestAffected.ReleaseName, StringComparison.CurrentCultureIgnoreCase))
                {
                    var json = "{ \"update\" : { \"customfield_15725\" : [{\"set\" : { \"id\": \"@@VERSION@@\" } }] }}";
                    json = json.Replace("@@VERSION@@", earliestAffected.ReleaseId);

                    var url = string.Format(JiraIssuePath, GetApiRoot(), story.Key);
                    
                    var postResult = jiraSender.Put(url, json);
                    loggingRepository.Log(story.Key, action, postResult.Response, postResult.WasSuccessful);
                }
            }
            catch(Exception ex)
            {
                loggingRepository.Log(story.Key, action, string.Format("Unexpected Error: {0}", ex.Message), false);
            }
        }

        private string GetApiRoot()
        {
            return systemPropertyRepository.GetNonNullValue(SystemPropertyName.JiraApiRoot);
        }

        private string GetServiceDeskGroup()
        {
            return systemPropertyRepository.GetNonNullValue(SystemPropertyName.JiraServiceDeskGroup);
        }

        private decimal GetScore(Dictionary<string, decimal> scores, ValueField field)
        {
            if (field == null || string.IsNullOrWhiteSpace(field.Value))
                return 0;

            return GetScore(scores, field.Value);
        }

        private decimal GetScore(Dictionary<string, decimal> scores, string option)
        {
            if (scores == null || !scores.Any() || string.IsNullOrWhiteSpace(option))
                return 0;

            return scores.ContainsKey(option) ? scores[option] : 0;
        }

        public IEnumerable<string> GetJiraProjects()
        {
            var allowedProjects = systemPropertyRepository.GetNonNullValue(SystemPropertyName.JiraProjects);

            if (string.IsNullOrWhiteSpace(allowedProjects)) return new List<string>();

            return allowedProjects.Split(',');
        }
    }
}