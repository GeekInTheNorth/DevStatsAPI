using Newtonsoft.Json;

namespace DevStats.Domain.Aha.JsonModels
{
    public class WorkFlowStatus
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}