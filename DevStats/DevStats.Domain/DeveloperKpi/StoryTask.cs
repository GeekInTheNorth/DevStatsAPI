namespace DevStats.Domain.DeveloperKpi
{
    public class StoryTask
    {
        public string Activity { get; set; }

        public string Owner { get; set; }

        public int TotalTimeInSeconds { get; set; }

        public StoryTask(string owner, string activity, int totalTimeInSeconds)
        {
            Owner = owner;
            Activity = activity;
            TotalTimeInSeconds = totalTimeInSeconds;
        }
    }
}
