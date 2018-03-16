using System;

namespace DevStats.Data.Entities
{
    public class BuildStatusLog
    {
        public int Id { get; set; }

        public string JiraId { get; set; }

        public string BuildNumber { get; set; }

        public string CommitSha { get; set; }

        public string Status { get; set; }

        public string Repository { get; set; }

        public string Organisation { get; set; }

        public DateTime StatusDate { get; set; }

        public bool BitbucketUpdated { get; set; }
    }
}
