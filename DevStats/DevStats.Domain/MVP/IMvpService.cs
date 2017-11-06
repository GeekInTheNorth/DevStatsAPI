using System.Collections.Generic;

namespace DevStats.Domain.MVP
{
    public interface IMvpService
    {
        Dictionary<int, string> GetVotableUsers(int currentUserId);

        VoteResult Vote(int voteeId, int voterId, string description);
    }
}