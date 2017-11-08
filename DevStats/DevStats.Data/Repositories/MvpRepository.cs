using System;
using System.Collections.Generic;
using System.Linq;
using DevStats.Data.Entities;
using DevStats.Domain.MVP;

namespace DevStats.Data.Repositories
{
    public class MvpRepository : BaseRepository, IMvpRepository
    {
        public IEnumerable<Vote> Get()
        {
            var data = (from vote in Context.MvpVotes
                        join votee in Context.Users on vote.VoteeId equals votee.ID
                        join voter in Context.Users on vote.VoterId equals voter.ID
                        select new
                        {
                            Id = vote.ID,
                            Votee = votee.DisplayName ?? votee.UserName,
                            Voter = voter.DisplayName ?? voter.UserName,
                            Reason = vote.Description,
                            Voted = vote.VoteDate,
                            Authorised = vote.Authorised
                        }).ToList();

            return data.Select(x => new Vote(x.Id, x.Votee, x.Voter, x.Reason, x.Voted, x.Authorised)).ToList();
        }

        public DateTime? GetLastVotedFor(int voteeId)
        {
            return Context.MvpVotes.Where(x => x.VoteeId == voteeId).OrderByDescending(x => x.VoteDate).Select(x => x.VoteDate).FirstOrDefault();
        }

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

        public void AuthorizeVote(int voteId, bool isAuthorised)
        {
            var dataItem = Context.MvpVotes.FirstOrDefault(x => x.ID == voteId);

            if (dataItem == null) return;

            dataItem.Authorised = isAuthorised;
            Context.SaveChanges();
        }
    }
}