namespace DevStats.Domain.KPI
{
    public class NewFeatureFailureRateTask
    {
        public string Activity { get; set; }

        public string Owner { get; set; }

        public int TotalTimeInSeconds { get; set; }

        public NewFeatureFailureRateTask(string owner, string activity, int totalTimeInSeconds)
        {
            Owner = owner;
            Activity = activity;
            TotalTimeInSeconds = totalTimeInSeconds;
        }
    }
}
