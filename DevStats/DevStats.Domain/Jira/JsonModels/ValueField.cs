using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class ValueField
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        public override string ToString()
        {
            return Value ?? string.Empty;
        }
    }
}