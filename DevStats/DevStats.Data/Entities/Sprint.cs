using System;

namespace DevStats.Data.Entities
{
    public class Sprint
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Pod { get; set; }

        public DateTime StartDate { get; set; }

        public int DurationDays { get; set; }

        public int PlannedEffort { get; set; }
    }
}