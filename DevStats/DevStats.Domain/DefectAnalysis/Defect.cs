using System;

namespace DevStats.Domain.DefectAnalysis
{
    public class Defect
    {
        public string DefectId { get; set; }

        public string Module { get; set; }

        public DefectType Type { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Closed { get; set; }

        public Defect(string defectId, string module, string type, DateTime created, DateTime? closed)
        {
            DefectId = defectId;
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