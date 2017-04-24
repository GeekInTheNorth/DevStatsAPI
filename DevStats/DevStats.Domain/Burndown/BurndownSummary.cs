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

        public BurndownSummary(IEnumerable<BurndownDay> days)
        {
            if (days == null)
                days = new List<BurndownDay>();

            Days = days.OrderBy(x => x.Date).ToList();

            if (Days.Any()) FillMissingDays();
        }

        private void FillMissingDays()
        {
            var firstDate = Days.Min(x => x.Date);
            var lastDate = Days.Max(x => x.Date);
            var workingDate = firstDate;
            var daysToAdd = new List<BurndownDay>();

            while (workingDate < lastDate)
            {
                if (!Days.Any(x => x.Date == workingDate))
                {
                    var dayToCopy = Days.OrderBy(x => x.Date).LastOrDefault(x => x.Date < workingDate);

                    if (dayToCopy != null)
                    {
                        daysToAdd.Add(new BurndownDay
                        {
                            Sprint = dayToCopy.Sprint,
                            Date = workingDate,
                            WorkRemaining = dayToCopy.WorkRemaining
                        });
                    }
                }

                workingDate = workingDate.AddDays(1);
            }

            if (daysToAdd.Any())
                Days = Days.Union(daysToAdd).OrderBy(x => x.Date).ToList();
        }
    }
}