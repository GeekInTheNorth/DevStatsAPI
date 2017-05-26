using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels.Create
{
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
}
