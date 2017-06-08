using System;
using DevStats.Domain.DefectAnalysis;
using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class Issue
    {
        [JsonProperty("expand")]
        public string Expand { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("fields")]
        public Fields Fields { get; set; }
    }
}