using Newtonsoft.Json;

namespace DevStats.Domain.Bitbucket
{
    public class Commit
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
