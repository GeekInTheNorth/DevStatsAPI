using System.Collections.Generic;
using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class UserGroup
    {
        [JsonProperty("users")]
        public UserCollection Users { get; set; }
    }
}