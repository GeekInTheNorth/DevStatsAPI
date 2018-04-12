using System;
using System.Collections.Generic;
using DevStats.Domain.Jira;

namespace DevStats.Models.Jira
{
    public class AuditModel
    {
        public DateTime FilterFrom { get; set; }

        public DateTime FilterTo { get; set; }

        public List<AuditModelItem> AuditItems { get; set; }
    }

    public class AuditModelItem
    {
        public string JiraId { get; private set; }

        public string Action { get; private set; }

        public string Content { get; private set; }

        public bool IsJson { get; private set; }

        public bool WasSuccessful { get; private set; }

        public string WasSuccessfulString
        {
            get { return WasSuccessful ? "Yes" : "No"; }
        }

        public DateTime AuditDate { get; private set; }

        public AuditModelItem(JiraAudit auditItem)
        {
            JiraId = auditItem.JiraId;
            Action = auditItem.Action;
            WasSuccessful = auditItem.WasSuccessful;
            AuditDate = auditItem.AuditDate;
            IsJson = !string.IsNullOrWhiteSpace(auditItem.Content) && auditItem.Content.StartsWith("{");
            Content = auditItem.Content;
        }
    }
}