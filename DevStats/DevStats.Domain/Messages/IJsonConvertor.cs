namespace DevStats.Domain.Messages
{
    public interface IJsonConvertor
    {
        T Deserialize<T>(byte[] jsonData);

        T Deserialize<T>(string jsonData);

        string Serialize<T>(T obj);
    }
}