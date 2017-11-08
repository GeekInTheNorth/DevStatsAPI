using System;

namespace DevStats.Domain.MVP
{
    public class Vote
    {
        public int VoteId { get; private set; }

        public string Votee { get; private set; }

        public string Voter { get; private set; }

        public string Reason { get; private set; }

        public DateTime Voted { get; private set; }

        public bool IsAuthorised { get; private set; }

        public Vote(int voteId, string votee, string voter, string reason, DateTime voted, bool isAuthorised)
        {
            VoteId = voteId;
            Votee = votee;
            Voter = voter;
            Reason = reason;
            Voted = voted;
            IsAuthorised = isAuthorised;
        }
    }
}