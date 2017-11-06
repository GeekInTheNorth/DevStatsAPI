using System;
using DevStats.Data.Entities;
using DevStats.Domain.MVP;

namespace DevStats.Data.Repositories
{
    public class MvpRepository : BaseRepository, IMvpRepository
    {
        public void Vote(int voteeId, int voterId, string description)
        {
            var dataItem = new MvpVote
            {
                VoteeId = voteeId,
                VoterId = voterId,
                Description = description,
                VoteDate = DateTime.Now,
                Authorised = true
            };

            Context.MvpVotes.Add(dataItem);
            Context.SaveChanges();
        }
    }
}