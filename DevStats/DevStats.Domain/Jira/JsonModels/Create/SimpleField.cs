using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels.Create
{
    public class SimpleField
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        public SimpleField(string key)
        {
            Key = key;
        }
    }
}
