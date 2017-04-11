using DevStats.Domain.Jira.JsonModels.Create;

namespace DevStats.Domain.Jira.Logging
{
    public interface IJiraLogRepository
    {
        void LogIncomingHook(string issueId, string displayIssueId, string content);

        void LogTaskCreateEvent(string issueId, string displayIssueId, SubtaskType taskType, bool successful, string content);
    }
}