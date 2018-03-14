namespace DevStats.Domain.Bitbucket
{
    public interface IBitbucketService
    {
        void Update(BuildStatusModel buildStatus, string url);
    }
}
