using System.Collections.Generic;

namespace DevStats.Domain.Sprints
{
    public interface ISprintService
    {
        IEnumerable<Sprint> Get();

        Sprint GetSprint(string podName);

        Sprint GetSprint(string podName, string sprintName);
    }
}