namespace DevStats.Domain.MVP
{
    public interface IMvpRepository
    {
        void Vote(int voteeId, int voterId, string description);
    }
}