using Newtonsoft.Json;

namespace DevStats.Domain.Aha.JsonModels
{
    public class IntegrationField
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("service_name")]
        public string Service { get; set; }
    }
}