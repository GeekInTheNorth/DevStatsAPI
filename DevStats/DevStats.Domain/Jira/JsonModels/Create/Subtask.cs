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
}
