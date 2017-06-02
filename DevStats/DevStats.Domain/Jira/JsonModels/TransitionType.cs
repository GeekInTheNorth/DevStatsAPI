using System.Collections.Generic;
using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class TransitionType
    {
        [JsonProperty("name")]
        public string IssueType { get; set; }

        public List<TransitionTypeStatus> Statuses { get; set; }
    }

    public class TransitionTypeStatus
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int TransitionId { get; set; }
    }
}