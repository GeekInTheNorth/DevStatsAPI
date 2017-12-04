using System.Collections.Generic;

namespace DevStats.Models.ReleaseQuality
{
    public class ReleaseQualityModel
    {
        public bool IsAdmin { get; set; }

        public List<Domain.Reports.ReleaseReport.ReleaseQuality> Releases { get; set; }
    }
}