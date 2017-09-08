using System;
using System.Collections.Generic;
using System.Linq;

namespace DevStats.Domain.DeveloperKpi
{
    public class DeveloperQualityKPI
    {
        public decimal TotalProportionalWork { get; private set; }

        public decimal TotalProportionalRework { get; private set; }

        public decimal TotalReworkProportion
        {
            get { return GetProportionOfTime(TotalProportionalWork, TotalProportionalRework); }
        }

        public bool IsOnTrack
        {
            get { return TotalReworkProportion < 0.125M; }
        }

        public List<StoryBreakdown> Stories { get; private set; }

        public DeveloperQualityKPI(IEnumerable<StoryBreakdown> stories)
        {
            Stories = stories != null ? stories.ToList() : new List<StoryBreakdown>();
            TotalProportionalWork = Math.Round(stories.Sum(x => x.TotalDuration * x.DeveloperProportion), 2);
            TotalProportionalRework = Math.Round(stories.Sum(x => x.ReworkDuration * x.ReworkProportion), 2);
        }

        private decimal GetProportionOfTime(decimal totalTime, decimal proportionalTime)
        {
            if (totalTime <= 0) return 0;

            return Math.Round(proportionalTime / totalTime, 4);
        }
    }
}