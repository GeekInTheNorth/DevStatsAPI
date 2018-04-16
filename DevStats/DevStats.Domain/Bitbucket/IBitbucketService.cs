using DevStats.Domain.Bitbucket.Models.Webhook;

namespace DevStats.Domain.Bitbucket
{
    public interface IBitbucketService
    {
        void Update(BuildStatusModel buildStatus, string url);

        void Approval(Payload payload, bool approving);
    }
}
