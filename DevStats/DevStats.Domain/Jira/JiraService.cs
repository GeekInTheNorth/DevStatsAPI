using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using DevStats.Domain.Jira.JsonModels;
using DevStats.Domain.Jira.JsonModels.Create;
using DevStats.Domain.Jira.JsonModels.WebHook;
using DevStats.Domain.Jira.Logging;
using DevStats.Domain.Jira.Transitions;

namespace DevStats.Domain.Jira
{
    public class JiraService: IJiraService
    {
        private readonly IJiraConvertor convertor;
        private readonly IJiraLogRepository loggingRepository;
        private readonly IJiraTransitionRepository transitionRepository;
        private readonly IJiraSender jiraSender;
        private const string JiraCreateTaskPath = @"{0}/rest/api/2/issue/";
        private const string JiraTransitionPath = @"{0}/rest/api/latest/issue/{1}/transitions";
        private string apiRoot;

        public JiraService(
            IJiraConvertor convertor, 
            IJiraLogRepository loggingRepository,
            IJiraTransitionRepository transitionRepository,
            IJiraSender jiraSender)
        {
            if (convertor == null) throw new ArgumentNullException(nameof(convertor));
            if (loggingRepository == null) throw new ArgumentNullException(nameof(loggingRepository));
            if (transitionRepository == null) throw new ArgumentNullException(nameof(transitionRepository));
            if (jiraSender == null) throw new ArgumentNullException(nameof(jiraSender));

            this.convertor = convertor;
            this.loggingRepository = loggingRepository;
            this.transitionRepository = transitionRepository;
            this.jiraSender = jiraSender;
        }

        public void CreateSubTasks(string issueId, string displayIssueId, string content)
        {
            loggingRepository.LogIncomingHook(issueId, displayIssueId, content);

            CreateSubTask(issueId, displayIssueId, SubtaskType.Merge);
            CreateSubTask(issueId, displayIssueId, SubtaskType.POReview);
        }

        public void ProcessSubTaskUpdate(string issueId, string displayIssueId, string content)
        {
            loggingRepository.LogIncomingHook(issueId, displayIssueId, content);

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

        public IEnumerable<JiraAudit> GetJiraAudit(DateTime from, DateTime to)
        {
            return loggingRepository.Get(from, to);
        }

        private void CreateSubTask(string issueId, string displayIssueId, SubtaskType taskType)
        {
            var task = new Subtask(displayIssueId, taskType);
            var url = string.Format(JiraCreateTaskPath, GetApiRoot());
            var postResult = jiraSender.Post(url, task);

            loggingRepository.LogTaskCreateEvent(issueId, displayIssueId, taskType, postResult.WasSuccessful, postResult.Response);
        }

        private string GetApiRoot()
        {
            if (string.IsNullOrWhiteSpace(apiRoot))
            {
                apiRoot = ConfigurationManager.AppSettings.Get("JiraApiRoot") ?? string.Empty;
            }

            return apiRoot;
        }
    }
}