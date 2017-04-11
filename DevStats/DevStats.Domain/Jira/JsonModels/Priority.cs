using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class Priority
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("iconUrl")]
        public string IconUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}