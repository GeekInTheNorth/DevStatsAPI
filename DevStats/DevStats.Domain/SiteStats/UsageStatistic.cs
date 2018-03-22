namespace DevStats.Domain.SiteStats
{
    public class UsageStatistic
    {
        public string StatisticType { get; set; }

        public int UsagesToday { get; set; }

        public int UsagesLastSevenDays { get; set; }

        public int UsagesLastThirtyDays { get; set; }
    }
}
