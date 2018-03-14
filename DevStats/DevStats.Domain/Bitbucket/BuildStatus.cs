using Newtonsoft.Json;

namespace DevStats.Domain.Bitbucket
{
    public class BuildStatus
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
