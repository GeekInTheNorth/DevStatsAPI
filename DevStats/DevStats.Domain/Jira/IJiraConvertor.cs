namespace DevStats.Domain.Jira
{
    public interface IJiraConvertor
    {
        T Deserialize<T>(byte[] jsonData);

        T Deserialize<T>(string jsonData);
    }
}