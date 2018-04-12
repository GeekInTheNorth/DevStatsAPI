using Newtonsoft.Json;

namespace DevStats.Domain.Bitbucket.Models.Webhook
{
    public class User : BaseIdentity
    {
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("display_name")]
        public string DisplayUserName { get; set; }
    }
}