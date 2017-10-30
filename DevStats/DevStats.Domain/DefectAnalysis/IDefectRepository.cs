using System;
using System.Collections.Generic;
using DevStats.Domain.Aha;
using DevStats.Domain.Jira;

namespace DevStats.Domain.DefectAnalysis
{
    public interface IDefectRepository
    {
        IEnumerable<Defect> Get(DateTime createdFrom, DateTime createdTo);

        void Save(IEnumerable<AhaDefect> defects);

        void Save(IEnumerable<JiraDefect> defects);

        void Delete(string jiraId);
    }
}