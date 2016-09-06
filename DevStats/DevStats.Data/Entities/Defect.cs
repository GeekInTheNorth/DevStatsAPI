using System;

namespace DevStats.Data.Entities
{
    public class Defect
    {
        public int ID { get; set; }

        public string DefectId { get; set; }

        public string Module { get; set; }

        public string Type { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Closed { get; set; }
    }
}