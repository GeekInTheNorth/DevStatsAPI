using System;

namespace DevStats.Models.DefectTracking
{
    public class DefectModel
    {
        public string DefectId { get; set; }

        public string Module { get; set; }

        public string Type { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Closed { get; set; }
    }
}
