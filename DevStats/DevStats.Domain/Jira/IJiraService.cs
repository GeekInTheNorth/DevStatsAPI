using System;
using System.Collections.Generic;

namespace DevStats.Domain.Jira
{
    public interface IJiraService
    {
        void CreateSubTasks(string issueId, string displayIssueId);

        void ProcessSubTaskUpdate(string issueId, string displayIssueId);

        void ProcessStoryUpdate(string issueId, string displayIssueId);

        IEnumerable<JiraAudit> GetJiraAudit(DateTime from, DateTime to);

        IEnumerable<JiraStateSummary> GetStateSummaries(string requestData);

        IEnumerable<JiraDefectSummary> GetDefects();
    }
}