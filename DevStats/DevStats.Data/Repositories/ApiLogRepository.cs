using System;
using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.Logging;

namespace DevStats.Data.Repositories
{
    public class ApiLogRepository : IApiLogRepository
    {
        public void Log(string apiName, string apiUrl, string action, bool success, string content)
        {
            var newLog = new Entities.ApiLog
            {
                ApiName = apiName,
                ApiUrl = apiUrl,
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

        public IEnumerable<ApiLog> Get(DateTime from, DateTime to)
        {
            using (var context = new DevStatContext())
            {
                return context.ApiLogs
                              .Where(x => x.Triggered >= from && x.Triggered <= to)
                              .AsEnumerable()
                              .Select(x => new ApiLog(x.ApiName, x.ApiUrl, x.Action, x.Content, x.IssueKey, x.Success, x.Triggered))
                              .ToList();
            }
        }
    }
}