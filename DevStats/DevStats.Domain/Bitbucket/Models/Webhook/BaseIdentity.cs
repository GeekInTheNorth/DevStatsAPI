using System;
using Newtonsoft.Json;

namespace DevStats.Domain.Bitbucket.Models.Webhook
{
    public class BaseIdentity
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uuid")]
        public Guid Identity { get; set; }
    }
}
