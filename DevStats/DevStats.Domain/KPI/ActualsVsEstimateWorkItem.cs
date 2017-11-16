using System;

namespace DevStats.Domain.KPI
{
    public class ActualsVsEstimateWorkItem
    {
        public string Key { get; private set; }

        public string Description { get; private set; }

        public string Complexity { get; private set; }

        public int EstimateInSeconds { get; private set; }

        public int ActualInSeconds { get; private set; }

        public DateTime Completed { get; private set; }

        public decimal Ratio { get; private set; }

        public ActualsVsEstimateWorkItem(string key, string description, string complexity, int estimateInSeconds, int actualInSeconds, DateTime lastWorkedOn)
        {
            Key = key;
            Description = description;
            Complexity = complexity;
            EstimateInSeconds = estimateInSeconds;
            ActualInSeconds = actualInSeconds;
            Completed = lastWorkedOn;

            Ratio = estimateInSeconds == 0 ? 1M : Math.Round((decimal)actualInSeconds / estimateInSeconds, 2);
        }
    }
}