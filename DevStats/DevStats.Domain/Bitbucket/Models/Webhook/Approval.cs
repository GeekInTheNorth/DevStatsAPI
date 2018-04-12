using System;
using Newtonsoft.Json;

namespace DevStats.Domain.Bitbucket.Models.Webhook
{
    public class Approval
    {
        [JsonProperty("date")]
        public DateTime Approved { get; set; }

        [JsonProperty("user")]
        public User ApprovedBy { get; set; }
    }
}
