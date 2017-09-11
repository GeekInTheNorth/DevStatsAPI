using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.DeveloperKpi;

namespace DevStats.Models.DeveloperKPI
{
    public class QualityKpiModel
    {
        public Dictionary<string, string> Developers { get; set; }

        public bool IsAdmin { get; set; }

        public string SelectedDeveloper { get; set; }

        public DeveloperQualityKPI Quality { get; set; }

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

        public QualityKpiModel(Dictionary<string, string> developers, string userName, bool isAdmin) : this(developers, userName, isAdmin, userName)
        {
        }

        public QualityKpiModel(Dictionary<string, string> developers, string userName, bool isAdmin, string selectedDeveloper)
        {
            Developers = developers.Where(x => x.Key == userName || isAdmin).ToDictionary(x => x.Key, y => y.Value);
            IsAdmin = isAdmin;

            if (Developers.Any(x => x.Key == selectedDeveloper))
                SelectedDeveloper = selectedDeveloper;
            else if (Developers.Any())
                SelectedDeveloper = Developers.First().Key;
        }
    }
}