using System.Collections.Generic;
using System.Configuration;

namespace DevStats.Domain.Jira
{
    public class ProjectsRepository: IProjectsRepository
    {
        public IEnumerable<string> Get()
        {
            var allowedProjects = ConfigurationManager.AppSettings.Get("JiraProjects") ?? string.Empty;

            if (string.IsNullOrWhiteSpace(allowedProjects)) return new List<string>();

            return allowedProjects.Split(',');
        }
    }
}