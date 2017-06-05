using System.Collections.Generic;
using DevStats.Domain.Jira;

namespace DevStats.Models.Jira
{
    public class StateCheckerModel
    {
        public List<JiraStateSummary> Summaries { get; set; }
    }
}