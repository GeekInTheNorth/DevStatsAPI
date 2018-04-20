using System;
using System.Collections.Generic;

namespace DevStats.Domain.Logging
{
    public interface IApiLogService
    {
        IEnumerable<ApiLog> Get(DateTime from, DateTime to);
    }
}