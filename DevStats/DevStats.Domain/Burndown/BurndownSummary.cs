using System.Collections.Generic;
using System.Linq;

namespace DevStats.Domain.Burndown
{
    public class BurndownSummary
    {
        public string Sprint
        {
            get
            {
                if (Days == null || !Days.Any())
                    return "Unknown";

                return Days.First().Sprint;
            }
        }

        public List<BurndownDay> Days { get; set; }
    }
}