using Newtonsoft.Json;

namespace DevStats.Domain.Bitbucket.Models.Webhook
{
    public class PullRequestTarget
    {
        [JsonProperty("branch")]
        public Branch Branch { get; set; }

        [JsonProperty("commit")]
        public Commit Commit { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }
    }
}
