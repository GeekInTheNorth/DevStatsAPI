using System.Collections.Generic;

namespace DevStats.Domain.KPI
{
    public interface INewFeaturePassRateRepository
    {
        IEnumerable<string> GetDevelopers();

        IEnumerable<NewFeaturePassRateStory> GetQualityApi(string developer);
    }
}
