namespace DevStats.Domain.KPI
{
    public interface IActualsVsEstimatesRepository
    {
        ActualsVsEstimateSummary Get(string userName);
    }
}