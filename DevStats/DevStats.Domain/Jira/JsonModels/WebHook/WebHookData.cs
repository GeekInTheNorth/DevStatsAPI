using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels.WebHook
{
    public class WebHookData
    {
        [JsonProperty("webhookEvent")]
        public string Event { get; set; }

        [JsonProperty("issue_event_type_name")]
        public string EventName { get; set; }

        [JsonProperty("user")]
        public User UpdatedBy { get; set; }

        [JsonProperty("issue")]
        public Issue Issue { get; set; }
    }
}