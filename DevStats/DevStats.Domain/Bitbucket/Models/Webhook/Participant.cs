using System;
using Newtonsoft.Json;

namespace DevStats.Domain.Bitbucket.Models.Webhook
{
    public class Participant
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("approved")]
        public bool Approved { get; set; }

        [JsonProperty("participated_on")]
        public DateTime? Participated { get; set; }
    }
}