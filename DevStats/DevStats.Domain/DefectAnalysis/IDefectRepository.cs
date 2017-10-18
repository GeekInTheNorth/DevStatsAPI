using System;
using System.Collections.Generic;
using DevStats.Domain.Aha;

namespace DevStats.Domain.DefectAnalysis
{
    public interface IDefectRepository
    {
        IEnumerable<Defect> Get(DateTime createdFrom, DateTime createdTo);

        void Save(IEnumerable<Defect> defects);

        void Save(IEnumerable<AhaDefect> defects);
    }
}