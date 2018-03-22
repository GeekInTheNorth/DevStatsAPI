using System;
using System.Collections.Generic;

namespace DevStats.Domain.SiteStats
{
    public class SiteStatisticService : ISiteStatisticService
    {
        private readonly ISiteStatisticRepository repository;

        public SiteStatisticService(ISiteStatisticRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            this.repository = repository;
        }

        public IEnumerable<UsageStatistic> GetUsageStatistics()
        {
            return repository.GetUsageStatistics();
        }
    }
}
