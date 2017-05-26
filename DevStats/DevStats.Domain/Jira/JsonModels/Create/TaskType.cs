using System;
using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels.Create
{
    public class TaskType
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        public TaskType(SubtaskType subtaskType)
        {
            switch (subtaskType)
            {
                case SubtaskType.Merge:
                    Id = "11510";
                    Value = "Merge";
                    break;
                case SubtaskType.POReview:
                    Id = "11508";
                    Value = "PO Review";
                    break;
                default:
                    throw new ArgumentException("Invalid subtaskType");
            }
        }
    }
}
