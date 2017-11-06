using System.Collections.Generic;
using DevStats.Domain.MVP;

namespace DevStats.Models.MVP
{
    public class MvpVoteModel
    {
        public int VoteeId { get; set; }

        public Dictionary<int, string> Users { get; set; }

        public bool HasAdminAccess { get; set; }

        public VoteResult Result { get; set; } 
    }
}