﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DevStats.Domain.DeveloperKpi
{
    public class StoryBreakdown
    {
        public string Key { get; private set; }

        public string Description { get; private set; }

        public int TotalDuration { get; private set; }

        public int ReworkDuration { get; private set; }

        public decimal ReworkProportion
        {
            get { return GetProportionOfTime(TotalDuration, ReworkDuration); }
        }

        public bool IsOnTrack
        {
            get { return ReworkProportion < 12.5M; }
        }

        public int TotalPlannedDevelopment { get; private set; }

        public int TotalPlannedDevelopmentByDeveloper { get; private set; }

        public decimal DeveloperProportion
        {
            get { return GetProportionOfTime(TotalPlannedDevelopment, TotalPlannedDevelopmentByDeveloper); }
        }

        public StoryBreakdown(string storyKey, string storyDescription, IEnumerable<StoryTask> tasks, string developer)
        {
            Key = storyKey;
            Description = storyDescription;
            TotalDuration = tasks.Sum(x => x.TotalTimeInSeconds);
            ReworkDuration = tasks.Where(x => x.Activity == "Rework").Sum(x => x.TotalTimeInSeconds);

            TotalPlannedDevelopment = tasks.Where(x => x.Activity == "Dev").Sum(x => x.TotalTimeInSeconds);
            TotalPlannedDevelopmentByDeveloper = tasks.Where(x => x.Activity == "Dev" && x.Owner.Equals(developer, StringComparison.CurrentCultureIgnoreCase)).Sum(x => x.TotalTimeInSeconds);
        }

        private decimal GetProportionOfTime(decimal totalTime, decimal proportionalTime)
        {
            if (totalTime <= 0) return 0;

            return Math.Round(proportionalTime / totalTime, 4);
        }
    }
}