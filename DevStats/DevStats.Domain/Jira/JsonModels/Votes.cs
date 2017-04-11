using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class Votes
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("votes")]
        public int NumberOfVotes { get; set; }

        [JsonProperty("hasVoted")]
        public bool HasVoted { get; set; }
    }
}