using System;
using DevStats.Domain.Jira;

namespace DevStats.Data.Repositories
{
    public class WorkLogRepository : BaseRepository, IWorkLogRepository
    {
        public void Save(StoryEffort storyEffort)
        {
            throw new NotImplementedException();
        }
    }
}