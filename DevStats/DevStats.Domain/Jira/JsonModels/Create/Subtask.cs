using System;
using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels.Create
{
    public class Subtask
    {
        [JsonProperty("fields")]
        public Fields Fields { get; set; }

        public Subtask(string parentId, SubtaskType subtaskType)
        {
            Fields = new Fields(parentId, subtaskType);
        }
    }

    public class Fields
    {
        [JsonProperty("parent")]
        public SimpleField Parent { get; set; }

        [JsonProperty("customfield_13701")]
        public TaskType TaskType { get; set; }

        [JsonProperty("issuetype")]
        public IssueType IssueType { get; set; }

        [JsonProperty("project")]
        public SimpleField Project { get; set; }

        [JsonProperty("timetracking")]
        public TimeTracking TimeTracking { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        public Fields(string parentId, SubtaskType subtaskType)
        {
            Project = new SimpleField(GetProject(parentId));
            Parent = new SimpleField(parentId);
            Summary = GetSummary(subtaskType);
            IssueType = new IssueType(5);
            TimeTracking = subtaskType == SubtaskType.Merge ? new TimeTracking(1800) : new TimeTracking(900);
            TaskType = new TaskType(subtaskType);
        }

        private string GetSummary(SubtaskType subtaskType)
        {
            switch (subtaskType)
            {
                case SubtaskType.Merge:
                    return "Merge to Develop";
                case SubtaskType.POReview:
                    return "Product Owner Review";
                default:
                    throw new ArgumentException("Invalid subtaskType");
            }
        }

        private string GetProject(string parentId)
        {
            if (parentId.StartsWith("CPR", StringComparison.CurrentCultureIgnoreCase))
                return "CPR";

            if (parentId.StartsWith("CHR", StringComparison.CurrentCultureIgnoreCase))
                return "CHR";

            if (parentId.StartsWith("OCT", StringComparison.CurrentCultureIgnoreCase))
                return "OCT";

            throw new ArgumentException("Invalid parentId");
        }
    }

    public class SimpleField
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        public SimpleField(string key)
        {
            Key = key;
        }
    }

    public class IssueType
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public IssueType(int id)
        {
            Id = id.ToString();
        }
    }

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

    public class TimeTracking
    {
        [JsonProperty("originalEstimate")]
        public string Estimate { get; set; }

        [JsonProperty("remainingEstimate")]
        public string Remaining { get; set; }

        public TimeTracking(int estimateMinutes)
        {
            Estimate = string.Format("{0}m", estimateMinutes);
            Remaining = string.Format("{0}m", estimateMinutes);
        }
    }

    public enum SubtaskType
    {
        Merge,
        POReview
    }
}
