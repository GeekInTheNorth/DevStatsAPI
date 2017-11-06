using System;

namespace DevStats.Data.Entities
{
    public class MvpVote
    {
        public int ID { get; set; }

        public int VoteeId { get; set; }

        public int VoterId { get; set; }

        public DateTime VoteDate { get; set; }

        public string Description { get; set; }

        public bool Authorised { get; set; }
    }
}