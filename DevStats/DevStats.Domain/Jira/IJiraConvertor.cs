namespace DevStats.Domain.Jira
{
    public interface IJiraConvertor
    {
        T Deserialize<T>(byte[] jsonData);

        T Deserialize<T>(string jsonData);

        string Serialize<T>(T obj);
    }
}