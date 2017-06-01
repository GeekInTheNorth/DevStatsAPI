using System;
using System.Collections.Generic;
using System.Configuration;
using DevStats.Domain.Jira.JsonModels.Create;
using DevStats.Domain.Jira.Logging;

namespace DevStats.Domain.Jira
{
    public class JiraService: IJiraService
    {
        private readonly IJiraConvertor convertor;
        private readonly IJiraLogRepository loggingRepository;
        private readonly IJiraSender jiraSender;
        private const string JiraCreateTaskPath = @"{0}/rest/api/2/issue/";
        private string apiRoot;

        public JiraService(IJiraConvertor convertor, IJiraLogRepository loggingRepository, IJiraSender jiraSender)
        {
            if (convertor == null) throw new ArgumentNullException(nameof(convertor));
            if (loggingRepository == null) throw new ArgumentNullException(nameof(loggingRepository));
            if (jiraSender == null) throw new ArgumentNullException(nameof(jiraSender));

            this.convertor = convertor;
            this.loggingRepository = loggingRepository;
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