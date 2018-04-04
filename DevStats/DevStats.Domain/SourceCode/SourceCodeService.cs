using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevStats.Domain.Bitbucket;
using DevStats.Domain.Jira;
using DevStats.Domain.Jira.JsonModels;
using DevStats.Domain.SystemProperties;

namespace DevStats.Domain.SourceCode
{
    public class SourceCodeService : ISourceCodeService
    {
        private readonly IBitbucketSender bitbucketSender;
        private readonly IJiraSender jiraSender;
        private readonly ISystemPropertyRepository systemPropertyRepository;
        private const string BranchApi = "https://api.bitbucket.org/1.0/repositories/{0}/{1}/branches";
        private const string JiraIssueSearchPath = @"{0}/rest/api/2/search?jql={1}&validateQuery=Warn";

        public SourceCodeService(IBitbucketSender bitbucketSender, IJiraSender jiraSender, ISystemPropertyRepository systemPropertyRepository)
        {
            if (bitbucketSender == null) throw new ArgumentNullException(nameof(bitbucketSender));
            if (jiraSender == null) throw new ArgumentNullException(nameof(jiraSender));
            if (systemPropertyRepository == null) throw new ArgumentNullException(nameof(systemPropertyRepository));

            this.bitbucketSender = bitbucketSender;
            this.jiraSender = jiraSender;
            this.systemPropertyRepository = systemPropertyRepository;
        }

        public SourceCodeBranches Get(string repoName)
        {
            var url = string.Format(BranchApi, "ngiris", repoName);

            var branches = bitbucketSender.Get<Dictionary<string, SourceCodeBranch>>(url);

            if (branches == null)
                return new SourceCodeBranches { RepositoryName = repoName };

            var filteredBranches = branches.Where(x => !x.Key.Equals("master", StringComparison.OrdinalIgnoreCase))
                                           .Where(x => !x.Key.StartsWith("release/", StringComparison.OrdinalIgnoreCase))
                                           .OrderBy(x => x.Key)
                                           .Select(x => new SourceCodeBranch
                                           {
                                               Author = x.Value.Author,
                                               Name = x.Value.Name ?? x.Key
                                           })
                                           .ToList();

            var jiraIds = filteredBranches.Where(x => x.IsJiraBranch).Select(x => x.JiraId);

            var storySearch = string.Format("issueKey in ({0})", string.Join(",", jiraIds));
            var storyUrl = string.Format(JiraIssueSearchPath, GetApiRoot(), HttpUtility.JavaScriptStringEncode(storySearch));
            var stories = jiraSender.Get<JiraIssues>(storyUrl).Issues ?? new Issue[] { };

            foreach(var story in stories)
            {
                var branch = filteredBranches.FirstOrDefault(x => x.JiraId == story.Key);

                if (branch != null)
                {
                    branch.Status = story.Fields.Status.Name;
                    branch.Title = story.Fields.Summary;
                }
            }

            return new SourceCodeBranches
            {
                RepositoryName = repoName,
                Branches = filteredBranches
            };
        }

        private string GetApiRoot()
        {
            return systemPropertyRepository.GetNonNullValue(SystemPropertyName.JiraApiRoot);
        }
    }
}
