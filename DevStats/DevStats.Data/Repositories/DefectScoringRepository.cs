using System.Collections.Generic;
using DevStats.Domain.Jira;

namespace DevStats.Data.Repositories
{
    public class DefectScoringRepository : IDefectScoringRepository
    {
        public Dictionary<string, decimal> GetUserImpactScores()
        {
            return new Dictionary<string, decimal>
            {
                { "All users of a product", 10 },
                { "All users attempting a specific common scenario", 5 },
                { "All users attempting a specific less common scenario", 2 },
                { "Specific customer due to data/environment", 1 }
            };
        }

        public Dictionary<string, decimal> GetFunctionalImpactScores()
        {
            return new Dictionary<string, decimal>
            {
                { "Operation of a key product workflow or time pressured action", 10 },
                { "Operation of a secondary product workflow", 5 },
                { "Product crashes", 2 },
                { "Presentation issue", 1 }
            };
        }

        public Dictionary<string, decimal> GetWorkAroundScores()
        {
            return new Dictionary<string, decimal>
            {
                { "No workaround", 10 },
                { "Out of product workaround", 5 },
                { "Complex or undesirable per operation workaround exists", 5 },
                { "Complex or undesirable one time only workaround exists", 2 },
                { "Simple per operation workaround exists", 2 },
                { "Simple one time only workaround exists", 1 }
            };
        }
    }
}