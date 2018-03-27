using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using DevStats.Domain.Bitbucket;
using DevStats.Domain.Jira;
using DevStats.Domain.Jira.JsonModels;

namespace DevStats.Domain.SourceCode
{
    public class SourceCodeService : ISourceCodeService
    {
        private readonly IBitbucketSender bitbucketSender;
        private readonly IJiraSender jiraSender;
        private const string BranchApi = "https://api.bitbucket.org/1.0/repositories/{0}/{1}/branches";
        private const string JiraIssueSearchPath = @"{0}/rest/api/2/search?jql={1}";

        public SourceCodeService(IBitbucketSender bitbucketSender, IJiraSender jiraSender)
        {
            if (bitbucketSender == null) throw new ArgumentNullException(nameof(bitbucketSender));
            if (jiraSender == null) throw new ArgumentNullException(nameof(jiraSender));

            this.bitbucketSender = bitbucketSender;
            this.jiraSender = jiraSender;
        }

        public SourceCodeBranches Get(string repoName)
        {
            var url = string.Format(BranchApi, "ngiris", repoName);

            var branches =  bitbucketSender.Get<Dictionary<string, SourceCodeBranch>>(url)
                                         .Select(x => x.Value)
                                         .OrderBy(x => x.Name)
                                         .ToList();

            var jiraIds = branches.Where(x => x.IsJiraBranch).Select(x => x.JiraId);

            var storySearch = string.Format("issueKey in ({0})", string.Join(",", jiraIds));
            var storyUrl = string.Format(JiraIssueSearchPath, GetApiRoot(), HttpUtility.JavaScriptStringEncode(storySearch));
            var stories = jiraSender.Get<JiraIssues>(storyUrl).Issues ?? new Issue[] { };

            foreach(var story in stories)
            {
                var branch = branches.FirstOrDefault(x => x.JiraId == story.Key);

                if (branch != null)
                {
                    branch.Status = story.Fields.Status.Name;
                    branch.Title = story.Fields.Summary;
                }
            }

            return new SourceCodeBranches
            {
                RepositoryName = repoName,
                Branches = branches
            };
        }

        private string GetApiRoot()
        {
            return ConfigurationManager.AppSettings.Get("JiraApiRoot") ?? string.Empty;
        }
    }
}
