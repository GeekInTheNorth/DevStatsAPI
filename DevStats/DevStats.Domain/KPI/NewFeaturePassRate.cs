using System;
using System.Collections.Generic;
using System.Linq;

namespace DevStats.Domain.KPI
{
    public class NewFeaturePassRate
    {
        public decimal TotalProportionalWork { get; private set; }

        public decimal TotalProportionalRework { get; private set; }

        public decimal TotalProportionalNonRework { get; private set; }

        public decimal TotalReworkProportion
        {
            get { return GetProportionOfTime(TotalProportionalWork, TotalProportionalRework); }
        }

        public decimal PassRate
        {
            get { return GetProportionOfTime(TotalProportionalWork, TotalProportionalNonRework); }
        }

        public bool IsOnTrack
        {
            get { return TotalReworkProportion < 0.1M; }
        }

        public List<NewFeaturePassRateStory> Stories { get; private set; }

        public NewFeaturePassRate(IEnumerable<NewFeaturePassRateStory> stories)
        {
            Stories = stories != null ? stories.ToList() : new List<NewFeaturePassRateStory>();
            TotalProportionalWork = Math.Round(stories.Sum(x => x.TotalDuration * x.DeveloperProportion), 2);
            TotalProportionalRework = Math.Round(stories.Sum(x => x.ReworkDuration * x.DeveloperProportion), 2);
            TotalProportionalNonRework = Math.Round(stories.Sum(x => (x.TotalDuration - x.ReworkDuration) * x.DeveloperProportion), 2);
        }

        private decimal GetProportionOfTime(decimal totalTime, decimal proportionalTime)
        {
            if (totalTime <= 0) return 0;

            return Math.Round(proportionalTime / totalTime, 4);
        }
    }
}