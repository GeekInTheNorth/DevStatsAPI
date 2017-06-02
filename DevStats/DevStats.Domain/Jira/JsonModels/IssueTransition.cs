using System.Collections.Generic;
using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class IssueTransitions
    {
        [JsonProperty("transitions")]
        public List<IssueTransition> Transitions { get; set; }
    }

    public class IssueTransition
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}