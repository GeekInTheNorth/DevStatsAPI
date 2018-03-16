using System;
using DevStats.Data.Entities;
using DevStats.Domain.Bitbucket;

namespace DevStats.Data.Repositories
{
    public class BuildStatusRepository : IBuildStatusRepository
    {
        public void Log(string jiraId, string buildNumber, string commitSha, string status, string repository, string organisation)
        {
            var newObject = new BuildStatusLog
            {
                BuildNumber = buildNumber,
                CommitSha = commitSha,
                JiraId = jiraId,
                Organisation = organisation,
                Repository = repository,
                Status = status,
                StatusDate = DateTime.Now
            };

            using (var context = new DevStatContext())
            {
                context.BuildStatusLogs.Add(newObject);
                context.SaveChanges();
            }
        }
    }
}
