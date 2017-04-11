namespace DevStats.Domain.Jira
{
    public interface IJiraService
    {
        void ProcessUpdatedItems();

        void CreateSubTasks(string issueId, string displayIssueId, string sourceDomain, string content);
    }
}