using Newtonsoft.Json;

namespace DevStats.Domain.Aha.JsonModels
{
    public class CustomField
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}