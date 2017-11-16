using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.KPI;

namespace DevStats.Models.KPI
{
    public class ActualsVsEstimatesModel
    {
        public Dictionary<string, string> TeamMembers { get; set; }

        public bool IsAdmin { get; set; }

        public string SelectedTeamMember { get; set; }

        public ActualsVsEstimateSummary Summary { get; set; }

        public ActualsVsEstimatesModel(Dictionary<string, string> teamMembers, string userName, bool isAdmin) : this(teamMembers, userName, userName, isAdmin)
        {
        }

        public ActualsVsEstimatesModel(Dictionary<string, string> teamMembers, string selectedTeamMember, string userName, bool isAdmin)
        {
            TeamMembers = teamMembers.Where(x => x.Key == userName || isAdmin).ToDictionary(x => x.Key, y => y.Value);
            IsAdmin = isAdmin;

            if (TeamMembers.Any(x => x.Key == selectedTeamMember))
                SelectedTeamMember = selectedTeamMember;
            else if (TeamMembers.Any())
                SelectedTeamMember = TeamMembers.First().Key;
        }
    }
}