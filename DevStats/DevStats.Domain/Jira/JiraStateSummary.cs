using DevStats.Domain.Jira.JsonModels;

namespace DevStats.Domain.Jira
{
    public class JiraStateSummary
    {
        public string Id { get; private set; }

        public string Type { get; private set; }

        public string Title { get; private set; }

        public string State { get; private set; }

        public JiraStateSummary(Issue issue)
        {
            Id = issue.Key;
            Type = issue.Fields.Issuetype.Name;
            Title = issue.Fields.Summary;
            State = issue.Fields.Status.Name;
        }
    }
}