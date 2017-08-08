using System.Collections.Generic;
using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class Worklogs
    {
        public List<WorkLog> Logs { get; set; }

        public Worklogs()
        {
            Logs = new List<WorkLog>();
        }
    }

    public class WorkLog
    {
        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("timeSpent")]
        public string TimeSpent { get; set; }

        [JsonProperty("timeSpentSeconds")]
        public int TimeSpentSeconds { get; set; }

        public WorkLogIssue Issue { get; set; }

        [JsonProperty("worker")]
        public string Worker { get; set; }
    }

    public class WorkLogIssue
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }
}