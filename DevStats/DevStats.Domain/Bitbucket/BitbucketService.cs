using System;
using DevStats.Domain.Jira.Logging;

namespace DevStats.Domain.Bitbucket
{
    public class BitbucketService : IBitbucketService
    {
        private const string BuildUrl = "https://api.bitbucket.org/2.0/repositories/{0}/{1}/commit/{2}/statuses/build";
        private readonly IBitbucketSender sender;
        private readonly IJiraLogRepository loggingRepository;

        public BitbucketService(IBitbucketSender sender, IJiraLogRepository loggingRepository)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            if (loggingRepository == null) throw new ArgumentNullException(nameof(loggingRepository));

            this.sender = sender;
            this.loggingRepository = loggingRepository;
        }

        public void Update(BuildStatusModel buildStatus, string url)
        {
            if (buildStatus == null) throw new ArgumentNullException(nameof(buildStatus));
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException(nameof(url));

            var state = new BuildStatus
            {
                Key = "1",
                State = buildStatus.Status.ToUpper(),
                Url = url,
                Name = buildStatus.BuildNumber
            };

            var bitbucketurl = string.Format(BuildUrl, buildStatus.BitbucketOrganisation, buildStatus.RepositoryName, buildStatus.CommitSha);

            var postResult = sender.Post(bitbucketurl, state);
            loggingRepository.Log(buildStatus.BuildNumber, "Update Bitbucket build status", postResult.Response, postResult.WasSuccessful);
        }
    }
}
