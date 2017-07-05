using System;
using System.Collections.Generic;
using DevStats.Domain.Jira.JsonModels.Create;

namespace DevStats.Domain.Jira.Logging
{
    public interface IJiraLogRepository
    {
        void Log(string issueId, string displayIssueId, string action, string content, bool wasSuccessful);

        void LogIncomingHook(string issueId, string displayIssueId, string content);

        void LogIncomingHook(JiraHook hook, string issueId, string displayIssueId, string content);

        void LogTaskCreateEvent(string issueId, string displayIssueId, SubtaskType taskType, bool successful, string content);

        IEnumerable<JiraAudit> Get(DateTime from, DateTime to);
    }
}