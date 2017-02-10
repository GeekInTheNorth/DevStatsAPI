using System;

namespace DevStats.Domain.Sprints
{
    public class Sprint
    {
        public string Name { get; private set; }

        public string Pod { get; private set; }

        public DateTime StartDate { get; private set; }

        public int DurationDays { get; private set; }

        public int PlannedEffort { get; set; }

        public Sprint(string name, string pod, DateTime startDate, int durationDays, int plannedEffort)
        {
            Name = name;
            Pod = pod;
            StartDate = startDate;
            DurationDays = durationDays;
            PlannedEffort = plannedEffort;
        }
    }
}