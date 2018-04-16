using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DevStats.Domain.Bitbucket.Models.Webhook
{
    public class PullRequest
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("approval")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("state")]
        public PullRequestState State { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("source")]
        public PullRequestTarget Source { get; set; }

        [JsonProperty("destination")]
        public PullRequestTarget Destination { get; set; }

        [JsonProperty("merge_commit")]
        public Commit MergeCommit { get; set; }

        [JsonProperty("participants")]
        public List<Participant> Participants { get; set; }

        [JsonProperty("reviewers")]
        public List<User> Reviewers { get; set; }

        [JsonProperty("close_source_branch")]
        public bool CloseSourceBranch { get; set; }

        [JsonProperty("closed_by")]
        public User ClosedBy { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("created_on")]
        public DateTime Created { get; set; }

        [JsonProperty("updated_on")]
        public DateTime Updated { get; set; }

        [JsonProperty("task_count")]
        public int TaskCount { get; set; }
    }
}
