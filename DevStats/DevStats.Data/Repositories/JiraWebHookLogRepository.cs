using System;
using DevStats.Data.Entities;
using DevStats.Domain.Jira.WebHookLog;

namespace DevStats.Data.Repositories
{
    public class JiraWebHookLogRepository : BaseRepository, IJiraWebHookLogRepository
    {
        public void Save(string user_id, string user_key, string content)
        {
            var log = new JiraHookLog
            {
                UserIdentity = user_id,
                UserKey = user_key,
                Body = content,
                Triggered = DateTime.Now
            };

            Context.JiraHookLogs.Add(log);
            Context.SaveChanges();
        }
    }
}