namespace DevStats.Domain.Sprints
{
    public interface ISprintService
    {
        Sprint GetSprint(string podName);

        Sprint GetSprint(string podName, string sprintName);
    }
}