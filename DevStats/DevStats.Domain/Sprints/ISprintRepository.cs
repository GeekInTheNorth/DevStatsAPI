namespace DevStats.Domain.Sprints
{
    public interface ISprintRepository
    {
        Sprint GetSprint(string podName);

        Sprint GetSprint(string podName, string sprintName);
    }
}