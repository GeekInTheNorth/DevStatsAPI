using System.Collections.Generic;

namespace DevStats.Domain.Reports.TaskingStatus
{
    public interface IEstimationRepository
    {
        Dictionary<string, decimal> GetTShirtAverages();
    }
}