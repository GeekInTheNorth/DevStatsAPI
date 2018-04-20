using System.Collections.Generic;

namespace DevStats.Domain.KPI
{
    public interface INewFeaturePassRateService
    {
        Dictionary<string, string> GetDevelopers();

        NewFeaturePassRate GetQualityKpi(string developer);

        Dictionary<string, NewFeaturePassRate> GetQualityKpi();
    }
}