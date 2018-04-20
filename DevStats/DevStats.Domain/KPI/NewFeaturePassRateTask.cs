namespace DevStats.Domain.KPI
{
    public class NewFeaturePassRateTask
    {
        public string Activity { get; set; }

        public string Owner { get; set; }

        public int TotalTimeInSeconds { get; set; }

        public NewFeaturePassRateTask(string owner, string activity, int totalTimeInSeconds)
        {
            Owner = owner;
            Activity = activity;
            TotalTimeInSeconds = totalTimeInSeconds;
        }
    }
}
