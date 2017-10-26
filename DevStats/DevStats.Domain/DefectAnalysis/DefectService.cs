using System;
using System.Collections.Generic;

namespace DevStats.Domain.DefectAnalysis
{
    public class DefectService : IDefectService
    {
        private readonly IDefectRepository repository;

        public DefectService(IDefectRepository repository)
        {
            if (repository == null) throw new ArgumentNullException("repository");

            this.repository = repository;
        }

        public ProductDefectSummaries GetSummary()
        {
            var today = DateTime.Today;
            var lastMonthEnd = new DateTime(today.Year, today.Month, 1).AddMonths(1).AddDays(-1);
            var firstMonthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-12);
            var items = repository.Get(firstMonthStart, lastMonthEnd);

            return new ProductDefectSummaries(items, firstMonthStart, lastMonthEnd);
        }

        public void Save(IEnumerable<Defect> defects)
        {
            throw new NotImplementedException();
        }
    }
}