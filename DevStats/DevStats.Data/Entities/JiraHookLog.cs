using System;

namespace DevStats.Data.Entities
{
    public class JiraHookLog
    {
        public int ID { get; set; }

        public string UserIdentity { get; set; }

        public string UserKey { get; set; }

        public string Body { get; set; }

        public DateTime Triggered { get; set; }
    }
}