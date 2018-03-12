using System;
using DevStats.Data.Entities;
using DevStats.Domain.Logging;

namespace DevStats.Data.Repositories
{
    public class ApiLogRepository : IApiLogRepository
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

            using (var context = new DevStatContext())
            {
                context.ApiLogs.Add(newLog);
                context.SaveChanges();
            }
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