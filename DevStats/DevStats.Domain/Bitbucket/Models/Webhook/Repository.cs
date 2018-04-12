using Newtonsoft.Json;

namespace DevStats.Domain.Bitbucket.Models.Webhook
{
    public class Repository : BaseIdentity
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        public User Owner { get; set; }

        public Project Project { get; set; }
    }
}
