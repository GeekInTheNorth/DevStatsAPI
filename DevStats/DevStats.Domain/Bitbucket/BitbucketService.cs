using System;
using System.Text.RegularExpressions;
using DevStats.Domain.Bitbucket.Models.Webhook;
using DevStats.Domain.Jira;
using DevStats.Domain.Jira.Logging;
using DevStats.Domain.SystemProperties;

namespace DevStats.Domain.Bitbucket
{
    public class BitbucketService : IBitbucketService
    {
        private const string CommitUrl = "https://api.bitbucket.org/2.0/repositories/{0}/{1}/commit/{2}";
        private const string BuildStatusUrl = "https://api.bitbucket.org/2.0/repositories/{0}/{1}/commit/{2}/statuses/build";
        private const string jiraBrowseUrl = "{0}/browse/{1}";
        private const string jiraCommentUrl = "{0}/rest/api/2/issue/{1}/comment";
        private readonly IBitbucketSender bitBucketsender;
        private readonly IJiraSender jiraSender;
        private readonly IJiraLogRepository loggingRepository;
        private readonly IBuildStatusRepository buildStatusRepository;
        private readonly ISystemPropertyRepository systemPropertyRepository;

        public BitbucketService(IBitbucketSender bitBucketsender, IJiraSender jiraSender, IJiraLogRepository loggingRepository, IBuildStatusRepository buildStatusRepository, ISystemPropertyRepository systemPropertyRepository)
        {
            if (bitBucketsender == null) throw new ArgumentNullException(nameof(bitBucketsender));
            if (jiraSender == null) throw new ArgumentNullException(nameof(jiraSender));
            if (loggingRepository == null) throw new ArgumentNullException(nameof(loggingRepository));
            if (buildStatusRepository == null) throw new ArgumentNullException(nameof(buildStatusRepository));
            if (systemPropertyRepository == null) throw new ArgumentNullException(nameof(systemPropertyRepository));

            this.bitBucketsender = bitBucketsender;
            this.jiraSender = jiraSender;
            this.loggingRepository = loggingRepository;
            this.buildStatusRepository = buildStatusRepository;
            this.systemPropertyRepository = systemPropertyRepository;
        }

        public void Update(BuildStatusModel buildStatus, string url)
        {
            if (buildStatus == null) throw new ArgumentNullException(nameof(buildStatus));
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException(nameof(url));

            var jiraId = GetJiraId(buildStatus);
            var statusUrl = string.IsNullOrWhiteSpace(jiraId) ? url : string.Format(jiraBrowseUrl, GetJiraApiRoot(), jiraId);

            var state = new BuildStatus
            {
                Key = "1",
                State = buildStatus.Status.ToUpper(),
                Url = statusUrl,
                Name = buildStatus.BuildNumber
            };

            var bitbucketurl = string.Format(BuildStatusUrl, buildStatus.BitbucketOrganisation, buildStatus.RepositoryName, buildStatus.CommitSha);

            var postResult = bitBucketsender.Post(bitbucketurl, state);
            loggingRepository.Log(buildStatus.BuildNumber, "Update Bitbucket build status", postResult.Response, postResult.WasSuccessful);
            buildStatusRepository.Log(jiraId, buildStatus.BuildNumber, buildStatus.CommitSha, buildStatus.Status.ToUpper(), buildStatus.RepositoryName, buildStatus.BitbucketOrganisation, postResult.WasSuccessful);
        }

        public void Approval(Payload payload, bool approving)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));

            try
            {
                var branchName = payload.PullRequest.Source.Branch.Name;
                var jiraId = GetJiraId(branchName);
                var approver = payload.Actor.DisplayUserName;
                var repositoryName = payload.Repository.Name;
                var action = approving ? "Pull Request: {0} approved {1}" : "Pull Request: {0} removed their approval of {1}";
                action = string.Format(action, approver, branchName);

                if (string.IsNullOrWhiteSpace(jiraId))
                {
                    loggingRepository.Log(jiraId, action, "Unable to identify a jira story to update.", false);
                    return;
                }

                var url = string.Format(jiraCommentUrl, GetJiraApiRoot(), jiraId);
                var comment = approving ? "{0} approved branch '{1}' against the '{2}' repository for this story" : "{0} removed their approval of branch '{1}' against the '{2}' repository for this story";
                comment = string.Format(comment, approver, branchName, repositoryName);

                var package = "{ \"body\": \"@@COMMENT@@\" }";
                package = package.Replace("@@COMMENT@@", comment);

                var postResult = jiraSender.Post(url, package);

                loggingRepository.Log(jiraId, action, postResult.Response, postResult.WasSuccessful);
            }
            catch(Exception ex)
            {
                var action = approving ? "Pull Request Approval" : "Pull Request Approval Removed";

                loggingRepository.Log("n/a", action, ex.Message, false);
            }
        }

        private string GetJiraId(string textToCheck)
        {
            var regEx = new Regex("([a-zA-Z]{3,7}[-][0-9]{1,7})");

            if (regEx.IsMatch(textToCheck))
                return regEx.Match(textToCheck).Value.ToUpper();

            return string.Empty;
        }

        private string GetJiraId(BuildStatusModel buildStatus)
        {
            var url = string.Format(CommitUrl, buildStatus.BitbucketOrganisation, buildStatus.RepositoryName, buildStatus.CommitSha);

            try
            {
                var commit = bitBucketsender.Get<Commit>(url);

                return GetJiraId(commit.Message);
            }
            catch (Exception ex)
            {
                loggingRepository.Log(buildStatus.BuildNumber, "Update Bitbucket build status: Get Commit Details", ex.Message, false);
            }

            return string.Empty;
        }

        private string GetJiraApiRoot()
        {
            return systemPropertyRepository.GetNonNullValue(SystemPropertyName.JiraApiRoot);
        }
    }
}
