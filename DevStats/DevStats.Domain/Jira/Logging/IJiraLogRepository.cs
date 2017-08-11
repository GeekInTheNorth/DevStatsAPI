using System;
using System.Collections.Generic;

namespace DevStats.Domain.Jira.Logging
{
    public interface IJiraLogRepository
    {
        void Log(string issueId, string displayIssueId, string action, string content, bool wasSuccessful);

        void LogIncomingHook(JiraHook hook, string issueId, string displayIssueId);

        IEnumerable<JiraAudit> Get(DateTime from, DateTime to);
    }
}