using System;
using System.Linq;
using DevStats.Domain.Sprints;

namespace DevStats.Data.Repositories
{
    public class SprintRepository : BaseRepository, ISprintRepository
    {
        public Sprint GetSprint(string podName)
        {
            var today = DateTime.Today;

            var sprintData = Context.Sprints
                                    .Where(x => x.Pod == podName && x.StartDate <= today)
                                    .OrderByDescending(x => x.StartDate)
                                    .FirstOrDefault();

            return sprintData == null ? null : new Sprint(sprintData.Name, sprintData.Pod, sprintData.StartDate, sprintData.DurationDays, sprintData.PlannedEffort);
        }

        public Sprint GetSprint(string podName, string sprintName)
        {
            var sprintData = Context.Sprints
                                    .Where(x => x.Pod == podName && x.Name == sprintName)
                                    .FirstOrDefault();

            return sprintData == null ? null : new Sprint(sprintData.Name, sprintData.Pod, sprintData.StartDate, sprintData.DurationDays, sprintData.PlannedEffort);
        }
    }
}