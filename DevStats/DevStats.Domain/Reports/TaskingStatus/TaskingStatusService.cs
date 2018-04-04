using System;
using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.Jira;
using DevStats.Domain.Jira.JsonModels;
using DevStats.Domain.SystemProperties;

namespace DevStats.Domain.Reports.TaskingStatus
{
    public class TaskingStatusService : ITaskingStatusService
    {
        private readonly IJiraSender jiraSender;
        private readonly IEstimationRepository estimationRepository;
        private readonly ISystemPropertyRepository systemPropertyRepository;

        private const string IssueSearchPath = @"{0}/rest/api/2/search?jql={1}&fields=summary,issuetype,status,subtasks,customfield_13709,customfield_13704,customfield_13701,timeoriginalestimate,customfield_10004";
        private const string storyjql = "\"Cascade Team\" = {0} AND type in (Bug, Story, Task) AND Refinement in (Ready, \"QA to Task Out\", \"Dev to Task Out\") AND status = \"To Do\"";
        private const string taskjql = "parent in ({0})";

        public TaskingStatusService(IJiraSender jiraSender, IEstimationRepository estimationRepository, ISystemPropertyRepository systemPropertyRepository)
        {
            if (jiraSender == null) throw new ArgumentNullException(nameof(jiraSender));
            if (estimationRepository == null) throw new ArgumentNullException(nameof(estimationRepository));
            if (systemPropertyRepository == null) throw new ArgumentNullException(nameof(systemPropertyRepository));

            this.jiraSender = jiraSender;
            this.estimationRepository = estimationRepository;
            this.systemPropertyRepository = systemPropertyRepository;
        }

        private string GetApiRoot()
        {
            return systemPropertyRepository.GetNonNullValue(SystemPropertyName.JiraApiRoot);
        }

        private class BandEqualityComparer : IEqualityComparer<KeyValuePair<string, decimal>>
        {
            public bool Equals(KeyValuePair<string, decimal> b1, KeyValuePair<string, decimal> b2)
            {
                if (b1.Key == b2.Key)
                    return true;
                else
                    return false;
            }

            public int GetHashCode(KeyValuePair<string, decimal> obj)
            {
                return obj.GetHashCode();
            }
        }

        public Dictionary<string, decimal> GetTaskingStatus(string team)
        {
            var apiRoot = GetApiRoot();
            var genJql = string.Format(storyjql, team);
            var url = string.Format(IssueSearchPath, apiRoot, genJql);

            var issues = jiraSender.Get<JiraIssues>(url).Issues.ToList();

            var bandEstimates = new Dictionary<string, decimal> { { "(Select)", 0 },{ "Very Small", 6 }, { "Small", 12 }, { "Medium", 30 }, { "Large", 60 }, { "Very Large", 120 } };

            var bandActuals = estimationRepository.GetTShirtAverages();

            var bandSummaries = bandActuals.Union(bandEstimates, new BandEqualityComparer()).ToDictionary(x => x.Key, x => x.Value);

            var devTTOCount = issues.Where(x => x.Fields.Refinement.Value == "Dev to Task Out").Sum(x => bandSummaries[x.Fields.TShirtSize.Value]);

            var qaTTOCount = issues.Where(x => x.Fields.Refinement.Value == "QA to Task Out").Sum(x => bandSummaries[x.Fields.TShirtSize.Value]);
            
            var ready = issues.Where(x => x.Fields.Refinement.Value == "Ready");
            decimal devReadyHours = 0m;
            decimal qaReadyHours = 0m;
            if (ready.Any())
            { 
                genJql = string.Format(taskjql, string.Join(",", ready.Select(x => x.Key).ToArray()));
                url = string.Format(IssueSearchPath, apiRoot, genJql);

                var readysubtasks = jiraSender.Get<JiraIssues>(url).Issues.ToList();

                var devreadysubtasks = readysubtasks.Where(z => z.Fields.TaskType.Value == "Dev");
                var qareadysubtasks = readysubtasks.Where(z => z.Fields.TaskType.Value == "Test");

                devReadyHours = (devreadysubtasks.Sum(y => y.Fields.Timeoriginalestimate) ?? 0) / 3600;
                qaReadyHours = (qareadysubtasks.Sum(y => y.Fields.Timeoriginalestimate) ?? 0) / 3600;
            }

            return new Dictionary<string, decimal>
            {
                { "DevTTO", Math.Round(devTTOCount, 2) },
                { "QATTO", Math.Round(qaTTOCount, 2) },
                { "DevReady", Math.Round(devReadyHours, 2) },
                { "QAReady", Math.Round(qaReadyHours, 2) }
            };
        }
    }
}