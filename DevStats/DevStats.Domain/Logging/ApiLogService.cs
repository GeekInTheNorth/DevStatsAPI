using System;
using System.Collections.Generic;

namespace DevStats.Domain.Logging
{
    public class ApiLogService : IApiLogService
    {
        private readonly IApiLogRepository repository;

        public ApiLogService(IApiLogRepository repository)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));

            this.repository = repository;
        }

        public IEnumerable<ApiLog> Get(DateTime from, DateTime to)
        {
            return repository.Get(from, to);
        }
    }
}