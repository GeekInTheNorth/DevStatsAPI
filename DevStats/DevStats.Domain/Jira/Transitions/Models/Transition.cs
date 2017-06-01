using System.Collections.Generic;
using Newtonsoft.Json;

namespace DevStats.Domain.Jira.Transitions.Models
{
    public class Transition
    {
        [JsonProperty("name")]
        public string IssueType { get; set; }

        public List<Status> Statuses { get; set; }
    }

    public class Status
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int TransitionId { get; set; }
    }
}