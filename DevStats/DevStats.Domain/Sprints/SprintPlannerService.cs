using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using DevStats.Domain.Jira;
using DevStats.Domain.Jira.JsonModels;
using DevStats.Domain.Jira.Logging;

namespace DevStats.Domain.Sprints
{
    public class SprintPlannerService : ISprintPlannerService
    {
        private readonly IJiraLogRepository loggingRepository;
        private readonly IJiraSender jiraSender;
        private readonly IProjectsRepository projectsRepository;
        private const string JiraIssueSearchPath = @"{0}/rest/api/2/search?jql={1}&fields=parent,timetracking,summary,issuetype,status,subtasks,customfield_13701,customfield_13709";

        public SprintPlannerService(IJiraLogRepository loggingRepository, IJiraSender jiraSender, IProjectsRepository projectsRepository)
        {
            if (loggingRepository == null) throw new ArgumentNullException(nameof(loggingRepository));
            if (jiraSender == null) throw new ArgumentNullException(nameof(jiraSender));
            if (projectsRepository == null) throw new ArgumentNullException(nameof(projectsRepository));

            this.loggingRepository = loggingRepository;
            this.jiraSender = jiraSender;
            this.projectsRepository = projectsRepository;
        }

        public IEnumerable<SprintInformation> GetSprints()
        {
            var projects = projectsRepository.Get();
            var sprintInfos = new List<SprintInformation>();

            try
            {
                var boards = new List<NameField>();
                foreach (var project in projects)
                {
                    var boardUrl = string.Format("{0}/rest/agile/1.0/board?projectKeyOrId={1}", GetApiRoot(), project);
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
                throw ex;
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
                throw ex;
            }
        }

        public IEnumerable<SprintStory> GetRefinedItems(string owningTeam, int currentSprint)
        {
            try
            {
                var apiRoot = GetApiRoot();
                var projects = projectsRepository.Get();
                var jql = "project in ({0}) AND issuetype in standardIssueTypes() AND \"Cascade Team\" = {1} AND Status = \"To Do\"AND (Sprint = NULL OR Sprint != {2})";
                jql = string.Format(jql, string.Join(",", projects), owningTeam, currentSprint);

                var url = string.Format(JiraIssueSearchPath, apiRoot, WebUtility.UrlEncode(jql));
                var stories = jiraSender.Get<JiraIssues>(url).Issues;

                if (!stories.Any()) return new List<SprintStory>();

                var storyKeys = stories.Select(x => x.Key).ToArray();
                jql = string.Format("parent in ({0})", string.Join(",", storyKeys));
                url = string.Format(JiraIssueSearchPath, apiRoot, WebUtility.UrlEncode(jql));

                var tasks = jiraSender.Get<JiraIssues>(url).Issues;

                return stories.Select(x => new SprintStory(x, tasks, apiRoot)).OrderBy(x => x.Key);
            }
            catch(Exception ex)
            {
                loggingRepository.Log("n/a", "n/a", "Sprint Planning: Get Refined Items", ex.Message, false);
                throw ex;
            }
        }

        private string GetApiRoot()
        {
            return ConfigurationManager.AppSettings.Get("JiraApiRoot") ?? string.Empty;
        }       
    }
}