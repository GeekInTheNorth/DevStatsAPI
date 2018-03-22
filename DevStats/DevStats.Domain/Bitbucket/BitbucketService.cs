using System;
using System.Configuration;
using System.Text.RegularExpressions;
using DevStats.Domain.Jira.Logging;

namespace DevStats.Domain.Bitbucket
{
    public class BitbucketService : IBitbucketService
    {
        private const string CommitUrl = "https://api.bitbucket.org/2.0/repositories/{0}/{1}/commit/{2}";
        private const string BuildStatusUrl = "https://api.bitbucket.org/2.0/repositories/{0}/{1}/commit/{2}/statuses/build";
        private const string jiraUrl = "{0}/browse/{1}";
        private readonly IBitbucketSender sender;
        private readonly IJiraLogRepository loggingRepository;
        private readonly IBuildStatusRepository buildStatusRepository;

        public BitbucketService(IBitbucketSender sender, IJiraLogRepository loggingRepository, IBuildStatusRepository buildStatusRepository)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            if (loggingRepository == null) throw new ArgumentNullException(nameof(loggingRepository));
            if (buildStatusRepository == null) throw new ArgumentNullException(nameof(buildStatusRepository));

            this.sender = sender;
            this.loggingRepository = loggingRepository;
            this.buildStatusRepository = buildStatusRepository;
        }

        public void Update(BuildStatusModel buildStatus, string url)
        {
            if (buildStatus == null) throw new ArgumentNullException(nameof(buildStatus));
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException(nameof(url));

            var jiraId = GetJiraIdFromCommit(buildStatus);
            var statusUrl = string.IsNullOrWhiteSpace(jiraId) ? url : string.Format(jiraUrl, GetApiRoot(), jiraId);

            var state = new BuildStatus
            {
                Key = "1",
                State = buildStatus.Status.ToUpper(),
                Url = statusUrl,
                Name = buildStatus.BuildNumber
            };

            var bitbucketurl = string.Format(BuildStatusUrl, buildStatus.BitbucketOrganisation, buildStatus.RepositoryName, buildStatus.CommitSha);

            var postResult = sender.Post(bitbucketurl, state);
            loggingRepository.Log(buildStatus.BuildNumber, "Update Bitbucket build status", postResult.Response, postResult.WasSuccessful);
            buildStatusRepository.Log(jiraId, buildStatus.BuildNumber, buildStatus.CommitSha, buildStatus.Status.ToUpper(), buildStatus.RepositoryName, buildStatus.BitbucketOrganisation, postResult.WasSuccessful);
        }

        private string GetJiraIdFromCommit(BuildStatusModel buildStatus)
        {
            var url = string.Format(CommitUrl, buildStatus.BitbucketOrganisation, buildStatus.RepositoryName, buildStatus.CommitSha);

            try
            {
                var commit = sender.Get<Commit>(url);
                var regEx = new Regex("([a-zA-Z]{3,7}[-][0-9]{1,7})");

                if (regEx.IsMatch(commit.Message))
                    return regEx.Match(commit.Message).Value;
            }
            catch (Exception ex)
            {
                loggingRepository.Log(buildStatus.BuildNumber, "Update Bitbucket build status: Get Commit Details", ex.Message, false);
            }

            return string.Empty;
        }

        private string GetApiRoot()
        {
            return ConfigurationManager.AppSettings.Get("JiraApiRoot") ?? string.Empty;
        }
    }
}
