using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels.Create
{
    public class ValueField
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        public ValueField(string value)
        {
            Value = value;
        }
    }
}
