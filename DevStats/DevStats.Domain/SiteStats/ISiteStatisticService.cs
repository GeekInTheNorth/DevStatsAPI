using System.Collections.Generic;

namespace DevStats.Domain.SiteStats
{
    public interface ISiteStatisticService
    {
        IEnumerable<UsageStatistic> GetUsageStatistics();
    }
}
