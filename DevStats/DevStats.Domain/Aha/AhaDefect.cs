using System;
using DevStats.Domain.DefectAnalysis;

namespace DevStats.Domain.Aha
{
    public class AhaDefect
    {
        public string JiraId { get; set; }

        public string AhaId { get; set; }

        public string Product { get; set; }

        public string Module { get; set; }

        public DefectType Type { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Closed { get; set; }

        public AhaDefect(string jiraId, string ahaId, string module, DefectType type, DateTime created, DateTime? closed)
        {
            JiraId = jiraId;
            AhaId = ahaId;
            Module = string.IsNullOrWhiteSpace(module) ? "Unknown" : module;
            Created = created;
            Closed = closed;
            Type = type;

            if (AhaId.StartsWith("PR-"))
                Product = "Payroll";
            else if (AhaId.StartsWith("HR-"))
                Product = "Cascade";
            else if (AhaId.StartsWith("IHR-"))
                Product = "Octopus";
        }
    }
}
