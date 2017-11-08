using System;
using System.Collections.Generic;

namespace DevStats.Domain.MVP
{
    public interface IMvpRepository
    {
        IEnumerable<Vote> Get();

        DateTime? GetLastVotedFor(int voteeId);

        void Vote(int voteeId, int voterId, string description);

        void AuthorizeVote(int voteId, bool isAuthorised);
    }
}