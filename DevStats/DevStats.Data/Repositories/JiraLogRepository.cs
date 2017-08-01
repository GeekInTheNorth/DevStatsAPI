using System;
using System.Collections.Generic;
using System.Linq;
using DevStats.Data.Entities;
using DevStats.Domain.Jira;
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

        public void LogIncomingHook(JiraHook hook, string issueId, string displayIssueId, string content)
        {
            var message = "Incoming Web Hook";
            if (hook != JiraHook.Unknown)
                message = string.Format("{0}: {1}", message, GetHookName(hook));

            Log(issueId, displayIssueId, message, content, true);
        }

        public IEnumerable<JiraAudit> Get(DateTime fromDate, DateTime toDate)
        {
            return Context.JiraLogs
                          .Where(x => x.Triggered >= fromDate && x.Triggered <= toDate)
                          .AsEnumerable()
                          .Select(x => new JiraAudit(x.IssueKey, x.Action, x.Content, x.Success, x.Triggered));
        }

        private string GetHookName(JiraHook hook)
        {
            switch (hook)
            {
                case JiraHook.StoryCreated:
                    return "Story Created";
                case JiraHook.StoryUpdate:
                    return "Story Updated";
                case JiraHook.StoryCompleted:
                    return "Story Completed";
                case JiraHook.SubtaskUpdate:
                    return "Subtask Updated";
                default:
                    return string.Empty;
            }
        }
    }
}