namespace DevStats.Domain.Aha
{
    public interface IAhaSender
    {
        T Get<T>(string url);
    }
}