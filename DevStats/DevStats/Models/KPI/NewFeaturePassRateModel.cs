using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.KPI;

namespace DevStats.Models.KPI
{
    public class NewFeaturePassRateModel
    {
        public Dictionary<string, string> Developers { get; set; }

        public bool IsAdmin { get; set; }

        public string SelectedDeveloper { get; set; }

        public string JiraRoot { get; set; }

        public NewFeaturePassRate Quality { get; set; }

        public int TotalDevTaskDuration
        {
            get
            {
                if (Quality == null || Quality.Stories == null || !Quality.Stories.Any())
                    return 0;

                return Quality.Stories.Sum(x => x.TotalPlannedDevelopment);
            }
        }

        public int TotalDevTaskDurationByDeveloper
        {
            get
            {
                if (Quality == null || Quality.Stories == null || !Quality.Stories.Any())
                    return 0;

                return Quality.Stories.Sum(x => x.TotalPlannedDevelopmentByDeveloper);
            }
        }

        public int TotalDuration
        {
            get
            {
                if (Quality == null || Quality.Stories == null || !Quality.Stories.Any())
                    return 0;

                return Quality.Stories.Sum(x => x.TotalDuration);
            }
        }

        public int TotalReworkDuration
        {
            get
            {
                if (Quality == null || Quality.Stories == null || !Quality.Stories.Any())
                    return 0;

                return Quality.Stories.Sum(x => x.ReworkDuration);
            }
        }

        public NewFeaturePassRateModel(Dictionary<string, string> developers, string userName, bool isAdmin, string jiraRoot) : this(developers, userName, isAdmin, jiraRoot, userName)
        {
        }

        public NewFeaturePassRateModel(Dictionary<string, string> developers, string userName, bool isAdmin, string jiraRoot, string selectedDeveloper)
        {
            Developers = developers.Where(x => x.Key == userName || isAdmin).ToDictionary(x => x.Key, y => y.Value);
            IsAdmin = isAdmin;
            JiraRoot = jiraRoot;

            if (Developers.Any(x => x.Key == selectedDeveloper))
                SelectedDeveloper = selectedDeveloper;
            else if (Developers.Any())
                SelectedDeveloper = Developers.First().Key;
        }
    }
}