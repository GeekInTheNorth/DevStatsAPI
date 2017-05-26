using System;
using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels.Create
{
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
            TimeTracking = subtaskType == SubtaskType.Merge ? new TimeTracking(30) : new TimeTracking(15);
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
}
