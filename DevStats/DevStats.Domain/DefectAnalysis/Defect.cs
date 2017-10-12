using System;

namespace DevStats.Domain.DefectAnalysis
{
    public class Defect
    {
        public string JiraId { get; set; }

        public string AhaId { get; set; }

        public string Module { get; set; }

        public DefectType Type { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Closed { get; set; }

        public Defect(string jiraId, string ahaId, string module, string type, DateTime created, DateTime? closed)
        {
            JiraId = jiraId;
            AhaId = ahaId;
            Module = string.IsNullOrWhiteSpace(module) ? "Unknown" : module;
            Created = created;
            Closed = closed;

            DefectType defectType;
            if (!Enum.TryParse(type, out defectType))
                defectType = DefectType.Unknown;

            Type = defectType;
        }

        public bool WasClosedBy(DateTime theDate)
        {
            return Closed.HasValue && Closed.Value <= theDate;
        }
    }
}