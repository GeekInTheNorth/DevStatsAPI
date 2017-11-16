using System;
using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.KPI;

namespace DevStats.Data.Repositories
{
    public class ActualsVsEstimatesRepository : BaseRepository, IActualsVsEstimatesRepository
    {
        public ActualsVsEstimateSummary Get(string userName)
        {
            var earliestItem = DateTime.Today.AddYears(-1);
            var unwantedActivities = new List<string> { "Merge", "PO Review" };

            var tasks = Context.WorkLogTasks
                               .Where(x => x.Owner == userName && x.LastWorkedOn.HasValue && x.LastWorkedOn >= earliestItem && !unwantedActivities.Contains(x.Activity))
                               .ToList()
                               .Select(x => new ActualsVsEstimateWorkItem(x.TaskKey, x.Description, x.Complexity, x.EstimateInSeconds, x.ActualTimeInSeconds, x.LastWorkedOn.Value));

            return new ActualsVsEstimateSummary(tasks);
        }
    }
}