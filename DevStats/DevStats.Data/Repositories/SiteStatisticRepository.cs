using System;
using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.SiteStats;

namespace DevStats.Data.Repositories
{
    public class SiteStatisticRepository : BaseRepository, ISiteStatisticRepository
    {
        public IEnumerable<UsageStatistic> GetUsageStatistics()
        {
            var today = DateTime.Today;
            var sevenDay = today.AddDays(-6);
            var thirtyDay = today.AddDays(-30);

            return new List<UsageStatistic>
            {
                GetApiLogStatistic(today, sevenDay, thirtyDay),
                GetJiraLogStatistic(today, sevenDay, thirtyDay),
                GetBuildLogStatistic(today, sevenDay, thirtyDay)
            };
        }

        private UsageStatistic GetApiLogStatistic(DateTime today, DateTime sevenDay, DateTime thirtyDay)
        {
            var todayCount = Context.ApiLogs.Where(x => x.Triggered >= today && x.Success).Count();
            var sevenDayCount = Context.ApiLogs.Where(x => x.Triggered >= sevenDay && x.Success).Count();
            var thirtyDayCount = Context.ApiLogs.Where(x => x.Triggered >= thirtyDay && x.Success).Count();

            return new UsageStatistic
            {
                StatisticType = "API Calls",
                UsagesToday = todayCount,
                UsagesLastSevenDays = sevenDayCount,
                UsagesLastThirtyDays = thirtyDayCount
            };
        }

        private UsageStatistic GetJiraLogStatistic(DateTime today, DateTime sevenDay, DateTime thirtyDay)
        {
            var todayCount = Context.JiraLogs.Where(x => x.Triggered >= today && x.Success).Count();
            var sevenDayCount = Context.JiraLogs.Where(x => x.Triggered >= sevenDay && x.Success).Count();
            var thirtyDayCount = Context.JiraLogs.Where(x => x.Triggered >= thirtyDay && x.Success).Count();

            return new UsageStatistic
            {
                StatisticType = "Jira Administrations",
                UsagesToday = todayCount,
                UsagesLastSevenDays = sevenDayCount,
                UsagesLastThirtyDays = thirtyDayCount
            };
        }

        private UsageStatistic GetBuildLogStatistic(DateTime today, DateTime sevenDay, DateTime thirtyDay)
        {
            var todayCount = Context.BuildStatusLogs.Where(x => x.StatusDate >= today && x.BitbucketUpdated).Count();
            var sevenDayCount = Context.BuildStatusLogs.Where(x => x.StatusDate >= sevenDay && x.BitbucketUpdated).Count();
            var thirtyDayCount = Context.BuildStatusLogs.Where(x => x.StatusDate >= thirtyDay && x.BitbucketUpdated).Count();

            return new UsageStatistic
            {
                StatisticType = "Build Status Updates",
                UsagesToday = todayCount,
                UsagesLastSevenDays = sevenDayCount,
                UsagesLastThirtyDays = thirtyDayCount
            };
        }
    }
}
