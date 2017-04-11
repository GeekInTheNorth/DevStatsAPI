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

        public Defect ToDefect()
        {
            var project = (Fields.Project == null || Fields.Project.Key == null) ? "UNKNOWN" : Fields.Project.Key;
            var module = string.Empty;

            switch (module)
            {
                case "OCT":
                    module = Fields.OctopusModule.Value;
                    break;
                case "CPR":
                    module = Fields.PayrollModule.Value;
                    break;
                case "CHR":
                    module = Fields.HRModule.Value;
                    break;
            }

            return new Defect(Id, module, DefectType.External.ToString(), Fields.Created, Fields.Resolutiondate);
        }
    }
}