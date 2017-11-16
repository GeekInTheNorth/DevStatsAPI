using System.Collections.Generic;

namespace DevStats.Domain.KPI
{
    public interface IActualsVsEstimatesService
    {
        Dictionary<string, string> GetTeamMembers();

        ActualsVsEstimateSummary Get(string userName);
    }
}