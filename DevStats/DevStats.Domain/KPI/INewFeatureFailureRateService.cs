using System.Collections.Generic;

namespace DevStats.Domain.KPI
{
    public interface INewFeatureFailureRateService
    {
        Dictionary<string, string> GetDevelopers();

        NewFeatureFailureRate GetQualityKpi(string developer);

        Dictionary<string, NewFeatureFailureRate> GetQualityKpi();
    }
}