using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DevStats.Domain.Jira;
using DevStats.Domain.Jira.JsonModels;
using DevStats.Domain.Jira.Logging;
using DevStats.Domain.SystemProperties;

namespace DevStats.Domain.Sprints
{
    public class SprintPlannerService : ISprintPlannerService
    {
        private readonly IJiraLogRepository loggingRepository;
        private readonly IJiraSender jiraSender;
        private readonly ISystemPropertyRepository systemPropertyRepository;
        private const string JiraIssuePath = @"{0}/rest/api/2/issue/{1}";
        private const string IssueSearchPath = @"{0}/rest/api/2/search?jql={1}&fields=parent,timetracking,summary,issuetype,status,subtasks,customfield_13701,customfield_13709&maxResults=500";
        private const string GetSprintItemsPath = @"{0}/rest/agile/1.0/sprint/{1}/issue?maxResults=500";
        private const string MoveToBacklogPath = @"{0}/rest/agile/1.0/backlog/issue";
        private const string MoveToSprintPath = @"{0}/rest/agile/1.0/sprint/{1}/issue";
        private const string UpdateSprintPath = @"{0}/rest/agile/1.0/sprint/{1}";

        public SprintPlannerService(IJiraLogRepository loggingRepository, IJiraSender jiraSender, ISystemPropertyRepository systemPropertyRepository)
        {
            if (loggingRepository == null) throw new ArgumentNullException(nameof(loggingRepository));
            if (jiraSender == null) throw new ArgumentNullException(nameof(jiraSender));
            if (systemPropertyRepository == null) throw new ArgumentNullException(nameof(systemPropertyRepository));

            this.loggingRepository = loggingRepository;
            this.jiraSender = jiraSender;
            this.systemPropertyRepository = systemPropertyRepository;
        }

        public IEnumerable<SprintInformation> GetSprints()
        {
            var projects = GetJiraProjects();
            var sprintInfos = new List<SprintInformation>();

            try
            {
                var boards = new List<NameField>();
                foreach (var project in projects)
                {
                    var boardUrl = string.Format("{0}/rest/agile/1.0/board?projectLocation={1}", GetApiRoot(), project);
                    var projectBoards = jiraSender.Get<NameCollection>(boardUrl).Names;

                    boards.AddRange(projectBoards.Where(x => !boards.Any(y => y.Id == x.Id)));
                }

                foreach (var board in boards)
                {
                    var sprintUrl = string.Format("{0}/rest/agile/1.0/board/{1}/sprint?state=active,future", GetApiRoot(), board.Id);
                    var sprints = jiraSender.Get<NameCollection>(sprintUrl).Names;

                    sprintInfos.AddRange(sprints.Select(x => new SprintInformation(board, x)));
                }
            }
            catch (Exception ex)
            {
                loggingRepository.Log("n/a", "n/a", "Sprint Planning: Get Sprints", ex.Message, false);
                throw new Exception("Failed to get sprints", ex);
            }

            return sprintInfos.OrderBy(x => x.SprintName);
        }

        public IEnumerable<SprintStory> GetSprintItems(int boardId, int sprintId)
        {
            var apiRoot = GetApiRoot();
            var url = string.Format("{0}/rest/agile/1.0/board/{1}/sprint/{2}/issue?maxResults=500", apiRoot, boardId, sprintId);

            try
            {
                var sprintItems = jiraSender.Get<JiraIssues>(url).Issues;
                var stories = sprintItems.Where(x => !x.Fields.Issuetype.Subtask);
                var tasks = sprintItems.Where(x => x.Fields.Issuetype.Subtask);

                return stories.Select(x => new SprintStory(x, tasks, apiRoot)).OrderBy(x => x.Key);
            }
            catch (Exception ex)
            {
                loggingRepository.Log("n/a", "n/a", "Sprint Planning: Get Sprint Items", ex.Message, false);
                throw new Exception("Failed to get sprint items", ex);
            }
        }

        public IEnumerable<SprintStory> GetRefinedItems(string owningTeam, int currentSprint)
        {
            try
            {
                var apiRoot = GetApiRoot();
                var projects = GetJiraProjects();
                var jql = "project in ({0}) AND issuetype in (Bug, Story, Task) AND \"Cascade Team\" = {1} AND Status = \"To Do\" AND Refinement in (\"Dev to Task Out\", \"QA to Task Out\", Ready) AND (Sprint = NULL OR Sprint != {2})";
                jql = string.Format(jql, string.Join(",", projects), owningTeam, currentSprint);

                var url = string.Format(IssueSearchPath, apiRoot, WebUtility.UrlEncode(jql));
                var stories = jiraSender.Get<JiraIssues>(url).Issues;

                if (!stories.Any()) return new List<SprintStory>();

                var storyKeys = stories.Select(x => x.Key).ToArray();
                jql = string.Format("parent in ({0}) AND \"Task Type\" Not IN (\"PO Review\", Merge)", string.Join(",", storyKeys));
                url = string.Format(IssueSearchPath, apiRoot, WebUtility.UrlEncode(jql));

                var tasks = jiraSender.Get<JiraIssues>(url).Issues;

                return stories.Select(x => new SprintStory(x, tasks, apiRoot)).OrderBy(x => x.Key);
            }
            catch(Exception ex)
            {
                loggingRepository.Log("n/a", "n/a", "Sprint Planning: Get Refined Items", ex.Message, false);
                throw new Exception("Failed to get backlog items", ex);
            }
        }

        public void SetSprintContents(int boardId, int sprintId, string teamName, IEnumerable<string> storyKeys)
        {
            var url = string.Format(GetSprintItemsPath, GetApiRoot(), sprintId);
            var existingStoryKeys = jiraSender.Get<JiraIssues>(url).Issues.Where(x => !x.Fields.Issuetype.Subtask).Select(x => x.Key);

            try
            {
                var storiesToRemove = existingStoryKeys.Where(x => !storyKeys.Contains(x));
                var storiesToAdd = storyKeys.Where(x => !existingStoryKeys.Contains(x));

                RemoveFromSprint(storiesToRemove);
                AddToSprint(sprintId, storiesToAdd);
                SetTeam(teamName, storyKeys);
            }
            catch (Exception ex)
            {
                loggingRepository.Log("n/a", "n/a", "Sprint Planning: Commit Sprint", ex.Message, false);
                throw;
            }
        }

        private void RemoveFromSprint(IEnumerable<string> keys)
        {
            if (keys == null || !keys.Any())
                return;

            var backlogUrl = string.Format(MoveToBacklogPath, GetApiRoot());
            var result = jiraSender.Post(backlogUrl, new StoryKeys(keys));

            loggingRepository.Log("n/a", "n/a", "Sprint Planning: Assign Stories to Backlog", result.Response, result.WasSuccessful);
        }

        private void AddToSprint(int sprintId, IEnumerable<string> keys)
        {
            if (keys == null || !keys.Any())
                return;

            var sprintUrl = string.Format(MoveToSprintPath, GetApiRoot(), sprintId);
            var result = jiraSender.Post(sprintUrl, new StoryKeys(keys));

            loggingRepository.Log("n/a", "n/a", "Sprint Planning: Assign Stories to Sprint", result.Response, result.WasSuccessful);
        }

        private void SetTeam(string teamName, IEnumerable<string> keys)
        {
            if (string.IsNullOrWhiteSpace(teamName)) return;

            var json = "{ \"update\" : { \"customfield_13700\" : [{\"set\" : {\"value\" : \"@@FieldValue@@\"} }] }}";
            json = json.Replace("@@FieldValue@@", teamName);

            foreach (var key in keys)
            {
                var url = string.Format(JiraIssuePath, GetApiRoot(), key);
                var action = string.Format("Sprint Planning: Update Cascade Team on Story {0}", key);

                var putResult = jiraSender.Put(url, json);
                loggingRepository.Log(key, key, action, putResult.Response, putResult.WasSuccessful);
            }
        }

        private string GetApiRoot()
        {
            return systemPropertyRepository.GetNonNullValue(SystemPropertyName.JiraApiRoot);
        }

        public IEnumerable<string> GetJiraProjects()
        {
            var allowedProjects = systemPropertyRepository.GetNonNullValue(SystemPropertyName.JiraProjects);

            if (string.IsNullOrWhiteSpace(allowedProjects)) return new List<string>();

            return allowedProjects.Split(',');
        }
    }
}