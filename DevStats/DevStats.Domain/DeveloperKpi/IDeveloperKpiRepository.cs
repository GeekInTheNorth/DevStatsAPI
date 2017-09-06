using System.Collections.Generic;

namespace DevStats.Domain.DeveloperKpi
{
    public interface IDeveloperKpiRepository
    {
        IEnumerable<string> GetDevelopers();

        IEnumerable<StoryBreakdown> GetQualityApi(string developer);
    }
}
