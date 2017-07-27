using System.Collections.Generic;
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

        [JsonProperty("changelog")]
        public ChangeLog Changes {get;set;}
    }

    public class ChangeLog
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("items")]
        public List<ChangeLogField> Items { get; set; }

        public ChangeLog()
        {
            Items = new List<ChangeLogField>();
        }
    }

    public class ChangeLogField
    {
        [JsonProperty("field")]
        public string DisplayName { get; set; }

        [JsonProperty("fieldId")]
        public string Name { get; set; }

        [JsonProperty("fromString")]
        public string OldValue { get; set; }

        [JsonProperty("toString")]
        public string NewValue { get; set; }
    }
}