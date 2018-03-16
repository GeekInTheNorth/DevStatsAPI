namespace DevStats.Domain.Bitbucket
{
    public interface IBuildStatusRepository
    {
        void Log(string jiraId, string buildNumber, string commitSha, string status, string repository, string organisation);
    }
}
