using Newtonsoft.Json;

namespace DevStats.Domain.Bitbucket.Models.Webhook
{
    public class Repository : BaseIdentity
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("owner")]
        public User Owner { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }

        [JsonProperty("is_private")]
        public bool IsPrivate { get; set; }
    }
}
