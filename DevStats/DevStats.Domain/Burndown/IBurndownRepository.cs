namespace DevStats.Domain.Burndown
{
    public interface IBurndownRepository
    {
        BurndownSummary Get(string sprintName);

        void Save(BurndownDay burndownDay);
    }
}
