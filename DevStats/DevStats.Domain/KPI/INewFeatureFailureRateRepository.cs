using System.Collections.Generic;

namespace DevStats.Domain.KPI
{
    public interface INewFeatureFailureRateRepository
    {
        IEnumerable<string> GetDevelopers();

        IEnumerable<NewFeatureFailureRateStory> GetQualityApi(string developer);
    }
}
