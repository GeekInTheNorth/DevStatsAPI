namespace DevStats.Domain.Jira
{
    public interface IWorkLogRepository
    {
        void Save(StoryEffort storyEffort);
    }
}