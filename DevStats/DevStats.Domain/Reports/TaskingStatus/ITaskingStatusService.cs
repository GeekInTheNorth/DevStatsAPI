using System.Collections.Generic;

namespace DevStats.Domain.Reports.TaskingStatus
{
    public interface ITaskingStatusService
    {
        Dictionary<string, decimal> GetTaskingStatus(string team);
    }
}
