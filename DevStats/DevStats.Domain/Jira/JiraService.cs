using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using DevStats.Domain.Jira.JsonModels;
using DevStats.Domain.Jira.JsonModels.WebHook;
using DevStats.Domain.Jira.Logging;

namespace DevStats.Domain.Jira
{
    public class JiraService: IJiraService
    {
        private readonly IJiraConvertor convertor;
        private readonly IJiraLogRepository loggingRepository;
        private readonly IJiraSender jiraSender;
        private readonly IProjectsRepository projectsRepository;
        private readonly IWorkLogRepository workLogRepository;
        private const string JiraIssuePath = @"{0}/rest/api/2/issue/{1}";
        private const string JiraCreatePath = @"{0}/rest/api/2/issue/";
        private const string JiraTransitionPath = @"{0}/rest/api/latest/issue/{1}/transitions";
        private const string CoreIssueIdRegex = "({0})[-][0-9]{{1,6}}";
        private const string JiraIssueSearchPath = @"{0}/rest/api/2/search?jql={1}";
        private const string JiraUserGroupSearchPath = @"{0}/rest/api/2/group?groupname={1}&expand=users";
        private const string TempoSearchPath = @"{0}/rest/tempo-timesheets/4/worklogs/search";

        public JiraService(
            IJiraConvertor convertor, 
            IJiraLogRepository loggingRepository,
            IJiraSender jiraSender,
            IProjectsRepository projectsRepository,
            IWorkLogRepository workLogRepository)
        {
            if (convertor == null) throw new ArgumentNullException(nameof(convertor));
            if (loggingRepository == null) throw new ArgumentNullException(nameof(loggingRepository));
            if (jiraSender == null) throw new ArgumentNullException(nameof(jiraSender));
            if (projectsRepository == null) throw new ArgumentNullException(nameof(projectsRepository));
            if (workLogRepository == null) throw new ArgumentNullException(nameof(workLogRepository));

            this.convertor = convertor;
            this.loggingRepository = loggingRepository;
            this.jiraSender = jiraSender;
            this.projectsRepository = projectsRepository;
            this.workLogRepository = workLogRepository;
        }

        public void CreateSubTasks(string issueId, string displayIssueId, string content)
        {
            loggingRepository.LogIncomingHook(JiraHook.StoryCreated, issueId, displayIssueId, content);

            CreateProductOwnerTask(issueId, displayIssueId);
            CreateMergeTask(issueId, displayIssueId);
        }

        public void ProcessSubTaskUpdate(string issueId, string displayIssueId, string content)
        {
            loggingRepository.LogIncomingHook(JiraHook.SubtaskUpdate, issueId, displayIssueId, content);

            if (string.IsNullOrWhiteSpace(content))
            {
                loggingRepository.Log(issueId, displayIssueId, "Process Sub-Task Update", "No issue content recieved from Jira", false);
                return;
            }

            try
            {
                var webHookData = convertor.Deserialize<WebHookData>(content);
                var task = webHookData.Issue;
                var taskState = task.Fields.Status.Name;
                var parentId = task.Fields.Parent.Key;
                var parentType = task.Fields.Parent.Fields.Issuetype.Name;
                var parentState = task.Fields.Parent.Fields.Status.Name;
                var project = task.Fields.Project.Key;
                var url = string.Format(JiraTransitionPath, GetApiRoot(), parentId);

                if (!taskState.Equals("In Progress") || parentState.Equals("In Progress"))
                {
                    loggingRepository.Log(issueId, displayIssueId, "Process Sub-Task Update: Update Parent", "No updates to apply", true);
                    return;
                }

                var transitions = jiraSender.Get<IssueTransitions>(url);
                
                if (transitions == null || transitions.Transitions == null || !transitions.Transitions.Any(x => x.Name == taskState))
                {
                    loggingRepository.Log(issueId, displayIssueId, "Process Sub-Task Update: Update Parent", "No known transitions", false);
                    return;
                }

                var transition = transitions.Transitions.First(x => x.Name == taskState);

                var json = "{ \"update\": { \"comment\": [{ \"add\": { \"body\": \"@@COMMENT@@\" } } ] }, \"transition\": { \"id\": \"@@ID@@\" } }";
                var comment = string.Format("Moved to {0} to match subtask: {1}", taskState, displayIssueId);
                json = json.Replace("@@ID@@", transition.Id.ToString())
                           .Replace("@@COMMENT@@", HttpUtility.JavaScriptStringEncode(comment));

                var postResult = jiraSender.Post(url, json);
                loggingRepository.Log(issueId, displayIssueId, "Process Sub-Task Update: Update Parent", postResult.Response, postResult.WasSuccessful);
            }
            catch (Exception ex)
            {
                loggingRepository.Log(issueId, displayIssueId, "Process Sub-Task Update: Update Parent", string.Format("Unexpected Error: {0}", ex.Message), false);
            }
        }

        public void ProcessStoryUpdate(string issueId, string displayIssueId, string content)
        {
            loggingRepository.LogIncomingHook(JiraHook.StoryUpdate, issueId, displayIssueId, content);

            if (string.IsNullOrWhiteSpace(content))
            {
                loggingRepository.Log(issueId, displayIssueId, "Process Story Update", "No issue content recieved from Jira", false);
                return;
            }

            try
            {
                var webHookData = convertor.Deserialize<WebHookData>(content);
                var task = webHookData.Issue;
                var changes = webHookData.Changes ?? new ChangeLog();
                var subtasks = task.Fields.Subtasks;
                var fieldsToUpdate = new List<string> { "customfield_13700" };

                if (subtasks == null || !subtasks.Any())
                {
                    loggingRepository.Log(issueId, displayIssueId, "Process Story Update", "No Sub-Tasks to update", true);
                    return;
                }

                var matchingFields = changes.Items.Where(x => fieldsToUpdate.Contains(x.Name, StringComparer.OrdinalIgnoreCase));
                if (matchingFields == null || !matchingFields.Any())
                {
                    loggingRepository.Log(issueId, displayIssueId, "Process Story Update", "No changes to targetted fields", true);
                    return;
                }

                foreach (var matchingField in matchingFields)
                {
                    foreach (var subtask in subtasks)
                    {
                        var json = "{ \"update\" : { \"@@fieldName@@\" : [{\"set\" : {\"value\" : \"@@FieldValue@@\"} }] }}";
                        json = json.Replace("@@fieldName@@", matchingField.Name)
                                   .Replace("@@FieldValue@@", matchingField.NewValue);
                        var url = string.Format(JiraIssuePath, GetApiRoot(), subtask.Key);
                        var putResult = jiraSender.Put(url, json);
                        var action = string.Format("Process Story Update: Update {0} on Sub-Task", matchingField.DisplayName);
                        loggingRepository.Log(subtask.Id, subtask.Key, action, putResult.Response, putResult.WasSuccessful);
                    }
                }
            }
            catch (Exception ex)
            {
                loggingRepository.Log(issueId, displayIssueId, "Process Story Update: Update Sub-Tasks", string.Format("Unexpected Error: {0}", ex.Message), false);
            }
        }

        public void ProcessStoryCompletion(string issueId, string displayIssueId, string content)
        {
            loggingRepository.LogIncomingHook(JiraHook.StoryCompleted, issueId, displayIssueId, content);

            try
            {
                var storyUrl = string.Format(JiraIssuePath, GetApiRoot(), displayIssueId);
                var story = jiraSender.Get<Issue>(storyUrl);
                var taskSummaries = story.Fields.Subtasks ?? new Issue[] { };

                var taskSearch = string.Format("issueKey in ({0})", string.Join(",", taskSummaries.Select(x => x.Key)));
                var taskUrl = string.Format(JiraIssueSearchPath, GetApiRoot(), HttpUtility.JavaScriptStringEncode(taskSearch));
                var tasks = jiraSender.Get<JiraIssues>(taskUrl).Issues ?? new Issue[] { };

                var tempoParams = new TempoSearchParameters(taskSummaries);
                var url = string.Format(TempoSearchPath, GetApiRoot());
                var tempoResult = jiraSender.Post(url, tempoParams);
                var workLogs = convertor.Deserialize<List<WorkLog>>(tempoResult.Response);

                var storyEffort = new StoryEffort(story, tasks, workLogs);

                workLogRepository.Save(storyEffort);
                loggingRepository.Log(issueId, displayIssueId, "Process Story Completion: Record Work Logs", string.Empty, true);
            }
            catch (Exception ex)
            {
                loggingRepository.Log(issueId, displayIssueId, "Process Story Completion: Record Work Logs", string.Format("Unexpected Error: {0}", ex.Message), false);
            }
        }

        public IEnumerable<JiraAudit> GetJiraAudit(DateTime from, DateTime to)
        {
            return loggingRepository.Get(from, to);
        }

        public IEnumerable<JiraStateSummary> GetStateSummaries(string requestData)
        {
            var projects = projectsRepository.Get();
            var regExStr = string.Format(CoreIssueIdRegex, string.Join("|", projects));
            var matches = Regex.Matches(requestData, regExStr);

            if (matches.Count == 0) return new List<JiraStateSummary>();

            var jql = string.Format("issueKey in ({0})", string.Join(",", matches.OfType<Match>().Select(x => x.Value)));
            var url = string.Format(JiraIssueSearchPath, GetApiRoot(), WebUtility.UrlEncode(jql));

            var response = jiraSender.Get<JiraIssues>(url);

            return (from issue in response.Issues
                    select new JiraStateSummary(issue));
        }

        public IEnumerable<JiraDefect> GetDefects()
        {
            var supportUserNames = GetUserNames(GetServiceDeskGroup());
            var jql = FilterBuilder.Create()
                                   .WithProjects(projectsRepository.Get())
                                   .WithIssueTypesOf(JiraIssueType.Bugs)
                                   .WithIssueStatesOf(JiraState.UnResolved)
                                   .Build();

            var url = string.Format(JiraIssueSearchPath, GetApiRoot(), WebUtility.UrlEncode(jql));
            var response = jiraSender.Get<JiraIssues>(url);

            return (from issue in response.Issues
                    select new JiraDefect(issue, supportUserNames));
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
                       .Replace("@@DESCRIPTION@@", "Merge to Develop");

            var url = string.Format(JiraCreatePath, GetApiRoot());
            var postResult = jiraSender.Post(url, json);

            loggingRepository.Log(issueId, displayIssueId, "Create Merge Task", postResult.Response, postResult.WasSuccessful);
        }

        private string GetApiRoot()
        {
            return ConfigurationManager.AppSettings.Get("JiraApiRoot") ?? string.Empty;
        }

        private string GetServiceDeskGroup()
        {
            return ConfigurationManager.AppSettings.Get("JiraServiceDeskGroup") ?? string.Empty;
        }
    }
}