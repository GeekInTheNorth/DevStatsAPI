using System;
using System.Collections.Generic;
using DevStats.Domain.Jira;

namespace DevStats.Models.Jira
{
    public class AuditModel
    {
        public DateTime FilterFrom { get; set; }

        public DateTime FilterTo { get; set; }

        public List<JiraAudit> AuditItems { get; set; }
    }
}