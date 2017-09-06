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