using System;
using System.Collections.Generic;
using System.Linq;

namespace DevStats.Domain.KPI
{
    public class NewFeaturePassRateStory
    {
        public string Key { get; private set; }

        public string Product { get; private set; }

        public string Description { get; private set; }

        public string Release { get; private set; }

        public DateTime? LastWorkedOn { get; private set; }

        public int TotalDuration { get; private set; }

        public int ReworkDuration { get; private set; }

        public decimal ReworkProportion
        {
            get { return GetProportionOfTime(TotalDuration, ReworkDuration); }
        }

        public bool IsOnTrack
        {
            get { return ReworkProportion < 0.1M; }
        }

        public int TotalPlannedDevelopment { get; private set; }

        public int TotalPlannedDevelopmentByDeveloper { get; private set; }

        public decimal DeveloperProportion
        {
            get { return GetProportionOfTime(TotalPlannedDevelopment, TotalPlannedDevelopmentByDeveloper); }
        }

        public NewFeaturePassRateStory(string storyKey, string storyDescription, string release, DateTime? lastWorkedOn , IEnumerable<NewFeaturePassRateTask> tasks, string developer)
        {
            Key = storyKey;
            Description = storyDescription;
            Release = release;
            LastWorkedOn = lastWorkedOn;
            TotalDuration = tasks.Sum(x => x.TotalTimeInSeconds);
            ReworkDuration = tasks.Where(x => x.Activity == "Rework").Sum(x => x.TotalTimeInSeconds);

            if (storyKey.StartsWith("CHR")) Product = "Cascade HR";
            if (storyKey.StartsWith("CPR")) Product = "Cascade Payroll";
            if (storyKey.StartsWith("OCT")) Product = "Cascade Go";

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