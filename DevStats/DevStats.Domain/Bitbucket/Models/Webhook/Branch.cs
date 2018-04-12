using Newtonsoft.Json;

namespace DevStats.Domain.Bitbucket.Models.Webhook
{
    public class Branch
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
