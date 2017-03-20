using System;
using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.Sprints;

namespace DevStats.Data.Repositories
{
    public class SprintRepository : BaseRepository, ISprintRepository
    {
        public IEnumerable<Sprint> Get()
        {
            return Context.Sprints
                          .OrderByDescending(x => x.StartDate).ThenBy(x => x.Pod)
                          .Select(x => new Sprint
                          {
                              Name = x.Name,
                              Pod = x.Pod,
                              StartDate = x.StartDate,
                              DurationDays = x.DurationDays,
                              PlannedEffort = x.PlannedEffort
                          });
        }

        public Sprint GetSprint(string podName)
        {
            var today = DateTime.Today;

            return Context.Sprints
                          .Where(x => x.Pod == podName && x.StartDate <= today)
                          .OrderByDescending(x => x.StartDate)
                          .Select(x => new Sprint
                          {
                              Name = x.Name,
                              Pod = x.Pod,
                              StartDate = x.StartDate,
                              DurationDays = x.DurationDays,
                              PlannedEffort = x.PlannedEffort
                          })
                          .FirstOrDefault();
        }

        public Sprint GetSprint(string podName, string sprintName)
        {
            return Context.Sprints
                          .Where(x => x.Pod == podName && x.Name == sprintName)
                          .Select(x => new Sprint
                          {
                              Name = x.Name,
                              Pod = x.Pod,
                              StartDate = x.StartDate,
                              DurationDays = x.DurationDays,
                              PlannedEffort = x.PlannedEffort
                          })
                          .FirstOrDefault();
        }

        public void Save(Sprint sprint)
        {
            var data = Context.Sprints.FirstOrDefault(x => x.Pod == sprint.Pod && x.Name == sprint.Name);

            if (data == null)
            {
                data = new Entities.Sprint();
                data.Pod = sprint.Pod;
                data.Name = sprint.Name;
                Context.Sprints.Add(data);
            }

            data.StartDate = sprint.StartDate;
            data.PlannedEffort = sprint.PlannedEffort;

            Context.SaveChanges();
        }
    }
}