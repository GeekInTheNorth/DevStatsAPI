using System;
using System.Collections.Generic;

namespace DevStats.Domain.DefectAnalysis
{
    public interface IDefectRepository
    {
        IEnumerable<Defect> Get(DateTime createdFrom, DateTime createdTo);

        void Save(IEnumerable<Defect> defects);
    }
}