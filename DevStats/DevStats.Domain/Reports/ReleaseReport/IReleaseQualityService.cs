using System.Collections.Generic;

namespace DevStats.Domain.Reports.ReleaseReport
{
    public interface IReleaseQualityService
    {
        ReleaseQuality Get(int id);

        ReleaseQuality Get(string product, string version);

        IEnumerable<ReleaseQuality> Get();
    }
}