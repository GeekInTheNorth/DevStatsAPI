using System;
using DevStats.Data.Entities;
using DevStats.Domain.Logging;

namespace DevStats.Data.Repositories
{
    public class ApiLogRepository : BaseRepository, IApiLogRepository
    {
        public void Log(string apiName, string apiUrl, string action, bool success, string content)
        {
            var newLog = new ApiLog
            {
                ApiName = apiName,
                ApUrl = apiUrl,
                Action = action,
                Triggered = DateTime.Now,
                Success = success,
                Content = content
            };

            Context.ApiLogs.Add(newLog);
            Context.SaveChanges();
        }

        public void Success(string apiName, string apiUrl, string action)
        {
            Log(apiName, apiUrl, action, true, null);
        }

        public void Failure(string apiName, string apiUrl, string action, Exception exception)
        {
            Log(apiName, apiUrl, action, false, exception.Message);
        }
    }
}