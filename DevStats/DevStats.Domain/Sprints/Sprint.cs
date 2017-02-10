using System;

namespace DevStats.Domain.Sprints
{
    public class Sprint
    {
        public string Name { get; set; }

        public string Pod { get; set; }

        public DateTime StartDate { get; set; }

        public int DurationDays { get; set; }

        public int PlannedEffort { get; set; }
    }
}