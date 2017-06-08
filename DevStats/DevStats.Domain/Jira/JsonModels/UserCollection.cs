using System.Collections.Generic;
using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class UserCollection
    {
        [JsonProperty("items")]
        public List<User> Users { get; set; }
    }
}