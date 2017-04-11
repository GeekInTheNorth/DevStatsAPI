using System;
using DevStats.Data.Entities;
using DevStats.Domain.Jira.JsonModels.Create;
using DevStats.Domain.Jira.Logging;

namespace DevStats.Data.Repositories
{
    public class JiraLogRepository : BaseRepository, IJiraLogRepository
    {
        public void LogIncomingHook(string issueId, string displayIssueId, string content)
        {
            var log = new JiraLog
            {
                IssueIdentity = issueId,
                IssueKey = displayIssueId,
                Content = content,
                Triggered = DateTime.Now,
                Action = "Incoming Web Hook",
                Success = true
            };

            Context.JiraLogs.Add(log);
            Context.SaveChanges();
        }

        public void LogTaskCreateEvent(string issueId, string displayIssueId, SubtaskType taskType, bool wasSuccessful, string content)
        {
            var log = new JiraLog
            {
                IssueIdentity = issueId,
                IssueKey = displayIssueId,
                Content = content,
                Triggered = DateTime.Now,
                Action = string.Format("Create {0} Task", taskType),
                Success = wasSuccessful
            };

            Context.JiraLogs.Add(log);
            Context.SaveChanges();
        }
    }
}