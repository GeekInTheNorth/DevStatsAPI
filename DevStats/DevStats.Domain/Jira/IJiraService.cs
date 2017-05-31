using System;
using System.Collections.Generic;

namespace DevStats.Domain.Jira
{
    public interface IJiraService
    {
        void ProcessUpdatedItems();

        void CreateSubTasks(string issueId, string displayIssueId, string sourceDomain, string content);

        IEnumerable<JiraAudit> GetJiraAudit(DateTime from, DateTime to);
    }
}