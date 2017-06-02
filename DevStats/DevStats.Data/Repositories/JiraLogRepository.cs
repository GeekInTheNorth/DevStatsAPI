using System;
using System.Collections.Generic;
using System.Linq;
using DevStats.Data.Entities;
using DevStats.Domain.Jira;
using DevStats.Domain.Jira.JsonModels.Create;
using DevStats.Domain.Jira.Logging;

namespace DevStats.Data.Repositories
{
    public class JiraLogRepository : BaseRepository, IJiraLogRepository
    {
        public void Log(string issueId, string displayIssueId, string action, string content, bool wasSuccessful)
        {
            var log = new JiraLog
            {
                IssueIdentity = issueId,
                IssueKey = displayIssueId,
                Content = content,
                Triggered = DateTime.Now,
                Action = action,
                Success = wasSuccessful
            };

            Context.JiraLogs.Add(log);
            Context.SaveChanges();
        }

        public void LogIncomingHook(string issueId, string displayIssueId, string content)
        {
            Log(issueId, displayIssueId, "Incoming Web Hook", content, true);
        }

        public void LogTaskCreateEvent(string issueId, string displayIssueId, SubtaskType taskType, bool wasSuccessful, string content)
        {
            var action = string.Format("Create {0} Task", taskType);

            Log(issueId, displayIssueId, action, content, wasSuccessful);
        }

        public IEnumerable<JiraAudit> Get(DateTime fromDate, DateTime toDate)
        {
            return Context.JiraLogs
                          .Where(x => x.Triggered >= fromDate && x.Triggered <= toDate)
                          .AsEnumerable()
                          .Select(x => new JiraAudit(x.IssueKey, x.Action, x.Content, x.Success, x.Triggered));
        }
    }
}