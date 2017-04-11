using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class Parent
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }
}