using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class NameField
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        public override string ToString()
        {
            return Name ?? string.Empty;
        }
    }
}