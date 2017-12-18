using System;
using System.Collections.Generic;
using System.Linq;

namespace DevStats.Domain.KPI
{
    public class NewFeatureFailureRate
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

        public List<NewFeatureFailureRateStory> Stories { get; private set; }

        public NewFeatureFailureRate(IEnumerable<NewFeatureFailureRateStory> stories)
        {
            Stories = stories != null ? stories.ToList() : new List<NewFeatureFailureRateStory>();
            TotalProportionalWork = Math.Round(stories.Sum(x => x.TotalDuration * x.DeveloperProportion), 2);
            TotalProportionalRework = Math.Round(stories.Sum(x => x.ReworkDuration * x.DeveloperProportion), 2);
        }

        private decimal GetProportionOfTime(decimal totalTime, decimal proportionalTime)
        {
            if (totalTime <= 0) return 0;

            return Math.Round(proportionalTime / totalTime, 4);
        }
    }
}