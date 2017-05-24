namespace DevStats.Domain.Jira.WebHookLog
{
    public interface IJiraLogRepository
    {
        void Save(string issueId, string displayIssueId, string sourceDomain, string content);
    }
}