using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class Issue
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("fields")]
        public Fields Fields { get; set; }
    }
}