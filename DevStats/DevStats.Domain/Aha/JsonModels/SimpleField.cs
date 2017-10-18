using Newtonsoft.Json;

namespace DevStats.Domain.Aha.JsonModels
{
    public class SimpleField
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}