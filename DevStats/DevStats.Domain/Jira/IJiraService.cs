using System;
using System.Collections.Generic;
using DevStats.Domain.Jira.JsonModels;

namespace DevStats.Domain.Jira
{
    public interface IJiraService
    {
        void CreateSubTasks(string issueId, string displayIssueId);

        void ProcessSubTaskUpdate(string issueId, string displayIssueId);

        void ProcessStoryUpdate(string jiraId);

        IEnumerable<JiraAudit> GetJiraAudit(DateTime from, DateTime to);

        IEnumerable<JiraStateSummary> GetStateSummaries(string requestData);

        IEnumerable<JiraDefectSummary> GetDefects();

        Issue GetIssue(string issueId);

        void Delete(string jiraId);
    }
}