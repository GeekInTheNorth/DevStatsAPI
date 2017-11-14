using System;
using System.Collections.Generic;

namespace DevStats.Domain.Jira
{
    public interface IJiraService
    {
        void ProcessSubTaskUpdate(string issueId, string displayIssueId);

        void ProcessStoryCreate(string jiraId);

        void ProcessStoryUpdate(string jiraId);

        IEnumerable<JiraAudit> GetJiraAudit(DateTime from, DateTime to);

        IEnumerable<JiraStateSummary> GetStateSummaries(string requestData);

        IEnumerable<JiraDefectSummary> GetDefects();

        void Delete(string jiraId);
    }
}