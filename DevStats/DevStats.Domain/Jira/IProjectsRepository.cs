using System.Collections.Generic;

namespace DevStats.Domain.Jira
{
    public interface IProjectsRepository
    {
        IEnumerable<string> Get();
    }
}