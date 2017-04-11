using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class Progress
    {
        [JsonProperty("progress")]
        public int IndividualProgress { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("percent")]
        public int? Percent { get; set; }
    }
}