using DevStats.Domain.Messages;

namespace DevStats.Domain.Bitbucket
{
    public interface IBitbucketSender
    {
        PostResult Post<T>(string url, T objectToSend);
    }
}
