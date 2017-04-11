using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class JiraIssues
    {
        [JsonProperty("expand")]
        public string Expand { get; set; }

        [JsonProperty("startAt")]
        public int StartAt { get; set; }

        [JsonProperty("maxResults")]
        public int MaxResults { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("issues")]
        public Issue[] Issues { get; set; }
    }
}