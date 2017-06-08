using System.Collections.Generic;
using System.Linq;

namespace DevStats.Domain.Jira
{
    public class FilterBuilder
    {
        private List<string> projects;
        private JiraIssueType issueTypes;
        private JiraState issueStates;
        private bool updatedItemsOnly;

        public FilterBuilder()
        {
            projects = new List<string>();
            issueTypes = JiraIssueType.All;
            updatedItemsOnly = false;
        }

        public static FilterBuilder Create()
        {
            return new FilterBuilder();
        }

        public FilterBuilder WithProjects(IEnumerable<string> projects)
        {
            this.projects = projects.Distinct().ToList();

            return this;
        }

        public FilterBuilder WithIssueTypesOf(JiraIssueType issueTypes)
        {
            this.issueTypes = issueTypes;

            return this;
        }

        public FilterBuilder WithIssueStatesOf(JiraState issueStates)
        {
            this.issueStates = issueStates;

            return this;
        }

        public FilterBuilder WithItemsUpdatedToday()
        {
            updatedItemsOnly = true;

            return this;
        }

        public string Build()
        {
            var filters = new List<string>();

            if (projects.Any())
                filters.Add(string.Format("project in ({0})", string.Join(",", projects)));

            switch (issueTypes)
            {
                case JiraIssueType.Tasks:
                    filters.Add("issuetype in standardIssueTypes()");
                    break;
                case JiraIssueType.Subtasks:
                    filters.Add("issuetype in subTaskIssueTypes()");
                    break;
                case JiraIssueType.Bugs:
                    filters.Add("issuetype = \"Bug\"");
                    break;
            }

            switch (issueStates)
            {
                case JiraState.Todo:
                    filters.Add("status = \"To Do\"");
                    break;
                case JiraState.InProgress:
                    filters.Add("status = \"In Progress\"");
                    break;
                case JiraState.Done:
                    filters.Add("status = \"Done\"");
                    break;
                case JiraState.UnResolved:
                    filters.Add("status in (\"To Do\",\"In Progress\")");
                    break;
            }

            if (updatedItemsOnly)
                filters.Add("updatedDate >= startOfDay() and updatedDate<endOfDay()");

            return string.Join(" AND ", filters);
        }
    }

    public enum JiraIssueType
    {
        All,
        Bugs,
        Tasks,
        Subtasks
    }

    public enum JiraState
    {
        All,
        Todo,
        InProgress,
        Done,
        UnResolved
    }
}