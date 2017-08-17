using System.Collections.Generic;
using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class NameCollection
    {
        [JsonProperty("values")]
        public List<NameField> Names { get; set; }
    }
}