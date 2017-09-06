using System.Collections.Generic;

namespace DevStats.Domain.DeveloperKpi
{
    public interface IDeveloperKpiService
    {
        Dictionary<string, string> GetDevelopers();

        DeveloperQualityKPI GetQualityKpi(string developer);
    }
}