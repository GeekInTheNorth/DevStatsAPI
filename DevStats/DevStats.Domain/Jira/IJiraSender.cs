namespace DevStats.Domain.Jira
{
    public interface IJiraSender
    {
        T Get<T>(string url);

        PostResult Post(string url, string package);

        PostResult Put(string url, string package);
    }
}