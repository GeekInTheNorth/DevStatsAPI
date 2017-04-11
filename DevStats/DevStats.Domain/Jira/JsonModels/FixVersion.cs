using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class FixVersion
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("archived")]
        public bool Archived { get; set; }

        [JsonProperty("released")]
        public bool Released { get; set; }

        [JsonProperty("releaseDate")]
        public DateTime? ReleaseDate { get; set; }
    }
}
