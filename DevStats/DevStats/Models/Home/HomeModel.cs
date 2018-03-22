using System.Collections.Generic;
using DevStats.Domain.SiteStats;

namespace DevStats.Models.Home
{
    public class HomeModel
    {
        public List<UsageStatistic> Statistics { get; set; }
    }
}