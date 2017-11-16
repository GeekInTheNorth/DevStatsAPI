using System;
using System.Collections.Generic;
using System.Linq;

namespace DevStats.Domain.KPI
{
    public class ActualsVsEstimateSummary
    {
        public decimal Rolling30Days { get; private set; }

        public decimal Rolling90Days { get; private set; }

        public decimal Rolling365Days { get; private set; }

        public List<ActualsVsEstimateWorkItem> WorkItems { get; private set; }

        public ActualsVsEstimateSummary(IEnumerable<ActualsVsEstimateWorkItem> workItems)
        {
            WorkItems = workItems.OrderByDescending(x => x.Completed).ThenBy(x => x.Key).ToList();

            var today = DateTime.Today;
            Rolling30Days = GetRollingAmount(workItems, today, 30);
            Rolling90Days = GetRollingAmount(workItems, today, 90);
            Rolling365Days = GetRollingAmount(workItems, today, 365);
        }

        private decimal GetRollingAmount(IEnumerable<ActualsVsEstimateWorkItem> workItems, DateTime today, int days)
        {
            var oldestCompletion = today.AddDays(-1 * days);
            var sumOfActuals = workItems.Where(x => x.Completed >= oldestCompletion).Sum(x => x.ActualInSeconds);
            var sumofEstimates = workItems.Where(x => x.Completed >= oldestCompletion).Sum(x => x.EstimateInSeconds);

            return sumofEstimates == 0 ? 1 : Math.Round((decimal)sumOfActuals / sumofEstimates, 2);
        }
    }
}