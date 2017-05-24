using System;
using DevStats.Data.Entities;
using DevStats.Domain.Jira.WebHookLog;

namespace DevStats.Data.Repositories
{
    public class JiraLogRepository : BaseRepository, IJiraLogRepository
    {
        public void Save(string issueId, string displayIssueId, string sourceDomain, string content)
        {
            var log = new JiraLog
            {
                IssueIdentity = issueId,
                IssueKey = displayIssueId,
                SourceDomain = sourceDomain,
                Triggered = DateTime.Now
            };

            Context.JiraLogs.Add(log);
            Context.SaveChanges();
        }
    }
}