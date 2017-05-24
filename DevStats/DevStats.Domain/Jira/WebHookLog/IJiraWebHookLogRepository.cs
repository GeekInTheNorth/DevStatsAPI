namespace DevStats.Domain.Jira.WebHookLog
{
    public interface IJiraWebHookLogRepository
    {
        void Save(string user_id, string user_key, string content);
    }
}