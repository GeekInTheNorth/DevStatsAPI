using Newtonsoft.Json;

namespace DevStats.Domain.Bitbucket.Models.Webhook
{
    public class Payload
    {
        [JsonProperty("actor")]
        public User Actor { get; set; }

        [JsonProperty("pullrequest")]
        public PullRequest PullRequest { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("approval")]
        public Approval Approval { get; set; }
    }
}
