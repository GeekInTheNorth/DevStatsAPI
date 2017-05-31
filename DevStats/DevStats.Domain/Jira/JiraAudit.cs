using System;

namespace DevStats.Domain.Jira
{
    public class JiraAudit
    {
        public string JiraId { get; private set; }

        public string Action { get; private set; }

        public string Content { get; private set; }

        public bool WasSuccessful { get; private set; }

        public DateTime AuditDate { get; private set; }

        public JiraAudit(string jiraId, string action, string content, bool wasSuccessful, DateTime auditDate)
        {
            JiraId = jiraId;
            Action = action;
            Content = content;
            WasSuccessful = wasSuccessful;
            AuditDate = auditDate;
        }
    }
}