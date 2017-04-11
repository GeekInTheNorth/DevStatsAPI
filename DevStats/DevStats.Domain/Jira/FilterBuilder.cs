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

        public FilterBuilder WithAProject(JiraProject project)
        {
            var projectName = string.Empty;

            switch (project)
            {
                case JiraProject.CascadeHR:
                    projectName = "CHR";
                    break;
                case JiraProject.CascadePayroll:
                    projectName = "CPR";
                    break;
                case JiraProject.CascadeGo:
                    projectName = "OCT";
                    break;
                default:
                    projectName = string.Empty;
                    break;
            }

            if (!string.IsNullOrWhiteSpace(projectName) && projects != null && !projects.Contains(projectName))
                projects.Add(projectName);

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
            var filter = string.Empty;

            if (projects.Any())
            {
                filter += string.Format("project in ({0})", string.Join(",", projects));
            }

            if (issueTypes != JiraIssueType.All)
            {
                if (!string.IsNullOrWhiteSpace(filter))
                    filter += " AND ";

                filter += issueTypes == JiraIssueType.Stories ? "issuetype in standardIssueTypes()" : "issuetype in subTaskIssueTypes()";
            }

            if (issueStates != JiraState.All)
            {
                if (!string.IsNullOrWhiteSpace(filter))
                    filter += " AND ";

                filter += issueStates == JiraState.Todo ? "status = \"To Do\"" : "status = \"Done\"";
            }

            if (updatedItemsOnly)
            {
                if (!string.IsNullOrWhiteSpace(filter))
                    filter += " AND ";

                filter += "updatedDate >= startOfDay() and updatedDate<endOfDay()";
            }

            return filter;
        }
    }

    public enum JiraProject
    {
        CascadeHR,
        CascadePayroll,
        CascadeGo
    }

    public enum JiraIssueType
    {
        All,
        Stories,
        Subtasks
    }

    public enum JiraState
    {
        All,
        Todo,
        Done
    }
}