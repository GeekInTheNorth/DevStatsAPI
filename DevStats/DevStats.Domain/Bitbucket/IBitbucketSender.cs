using DevStats.Domain.Messages;

namespace DevStats.Domain.Bitbucket
{
    public interface IBitbucketSender
    {
        T Get<T>(string url);

        PostResult Post<T>(string url, T objectToSend);
    }
}
