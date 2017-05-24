using System;

namespace DevStats.Data.Entities
{
    public class JiraLog
    {
        public int ID { get; set; }

        public string IssueIdentity { get; set; }

        public string IssueKey { get; set; }

        public string SourceDomain { get; set; }

        public string Content { get; set; }

        public DateTime Triggered { get; set; }
    }
}