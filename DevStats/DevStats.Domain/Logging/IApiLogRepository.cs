using System;
using System.Collections.Generic;

namespace DevStats.Domain.Logging
{
    public interface IApiLogRepository
    {
        void Log(string apiName, string apiUrl, string action, bool success, string content);

        void Success(string apiName, string apiUrl, string action);

        void Failure(string apiName, string apiUrl, string action, Exception exception);

        IEnumerable<ApiLog> Get(DateTime from, DateTime to);
    }
}