using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class TimeTracking
    {
        [JsonProperty("originalEstimateSeconds")]
        public int EstimateInSeconds { get; set; }

        [JsonProperty("remainingEstimateSeconds")]
        public int TimeRemainingInSeconds { get; set; }

        [JsonProperty("timeSpentSeconds")]
        public int TimeSpentInSeconds { get; set; }
    }
}