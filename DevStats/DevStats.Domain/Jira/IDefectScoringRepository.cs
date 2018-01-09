using System.Collections.Generic;

namespace DevStats.Domain.Jira
{
    public interface IDefectScoringRepository
    {
        Dictionary<string, decimal> GetUserImpactScores();

        Dictionary<string, decimal> GetFunctionalImpactScores();

        Dictionary<string, decimal> GetWorkAroundScores();
    }
}