using DevStats.Domain.Messages;

namespace DevStats.Domain.Jira
{
    public interface IJiraSender
    {
        T Get<T>(string url);

        PostResult Post<T>(string url, T objectToSend);

        PostResult Post(string url, string package);

        PostResult Put<T>(string url, T objectToSend);

        PostResult Put(string url, string package);
    }
}