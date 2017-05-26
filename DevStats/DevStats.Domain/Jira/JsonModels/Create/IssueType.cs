using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels.Create
{
    public class IssueType
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public IssueType(int id)
        {
            Id = id.ToString();
        }
    }
}
