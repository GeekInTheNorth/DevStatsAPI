using System.Collections.Generic;

namespace DevStats.Domain.DefectAnalysis
{
    public interface IDefectService
    {
        DefectSummaries GetSummary();

        void Save(IEnumerable<Defect> defects);
    }
}