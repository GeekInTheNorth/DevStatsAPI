using System.Collections.Generic;

namespace DevStats.Domain.SiteStats
{
    public interface ISiteStatisticRepository
    {
        IEnumerable<UsageStatistic> GetUsageStatistics();
    }
}
