using Newtonsoft.Json;

namespace DevStats.Domain.Bitbucket.Models.Webhook
{
    public class Commit
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }
    }
}
