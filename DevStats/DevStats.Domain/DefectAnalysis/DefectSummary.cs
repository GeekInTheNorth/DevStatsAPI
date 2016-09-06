using System;
using System.Collections.Generic;

namespace DevStats.Domain.DefectAnalysis
{
    public class DefectSummary
    {
        public string MonthString { get; set; }

        public Dictionary<string, int> ModuleBreakdown { get; set; }
    }
}
