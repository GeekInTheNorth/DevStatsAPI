using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class TempoSearchParameters
    {
        [JsonProperty("from")]
        public DateTime DateFrom { get; private set; }

        [JsonProperty("to")]
        public DateTime DateTo { get; private set; }

        [JsonProperty("taskId")]
        public List<String> TaskIds { get; private set; }

        public TempoSearchParameters(IEnumerable<Issue> issues)
        {
            if (issues == null) throw new ArgumentNullException("issues");

            DateTo = DateTime.Today;
            DateFrom = DateTo.AddYears(-1);
            TaskIds = issues.Select(x => x.Id).ToList();
        }
    }
}