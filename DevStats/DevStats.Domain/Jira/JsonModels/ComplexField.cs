using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class ComplexField
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        public override string ToString()
        {
            return Value ?? string.Empty;
        }
    }
}