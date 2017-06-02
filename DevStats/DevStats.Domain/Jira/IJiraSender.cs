namespace DevStats.Domain.Jira
{
    public interface IJiraSender
    {
        T Get<T>(string url);

        PostResult Post<T>(string url, T objectToSend);

        PostResult Post(string url, string package);
    }
}