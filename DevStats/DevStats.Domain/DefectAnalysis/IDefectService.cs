using System.Collections.Generic;

namespace DevStats.Domain.DefectAnalysis
{
    public interface IDefectService
    {
        ProductDefectSummaries GetSummary();

        void Save(IEnumerable<Defect> defects);
    }
}