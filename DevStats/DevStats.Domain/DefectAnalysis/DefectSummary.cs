using System.Collections.Generic;

namespace DevStats.Domain.DefectAnalysis
{
    public class DefectSummary
    {
        public string Module { get; set; }

        public Dictionary<string, int> MonthlyBreakdown { get; set; }

        public DefectSummary(string module)
        {
            Module = module;
            MonthlyBreakdown = new Dictionary<string, int>();
        }
    }
}
