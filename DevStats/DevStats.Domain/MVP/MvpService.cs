using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using DevStats.Domain.Communications;
using DevStats.Domain.Security;
using DevStats.Domain.SystemProperties;

namespace DevStats.Domain.MVP
{
    public class MvpService : IMvpService
    {
        private readonly IUserRepository userRepository;
        private readonly IMvpRepository mvpRepository;
        private readonly IEmailService emailService;
        private readonly ISystemPropertyRepository systemPropertyRepository;

        public MvpService(IMvpRepository mvpRepository, IUserRepository userRepository, IEmailService emailService, ISystemPropertyRepository systemPropertyRepository)
        {
            if (mvpRepository == null) throw new ArgumentNullException(nameof(mvpRepository));
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));
            if (emailService == null) throw new ArgumentNullException(nameof(emailService));
            if (systemPropertyRepository == null) throw new ArgumentNullException(nameof(systemPropertyRepository));

            this.mvpRepository = mvpRepository;
            this.userRepository = userRepository;
            this.emailService = emailService;
            this.systemPropertyRepository = systemPropertyRepository;
        }

        public Dictionary<int, string> GetVotableUsers(int currentUserId)
        {
            var validUserRoles = new string[] { "User", "Admin" };

            return userRepository.Get()
                                 .Where(x => validUserRoles.Contains(x.Role) && x.Id != currentUserId)
                                 .OrderBy(x => x.DisplayName)
                                 .ToDictionary(x => x.Id, y => y.DisplayName);
        }

        public VoteResult Vote(int voteeId, int voterId, string description)
        {
            if (voteeId == voterId) return new VoteResult(false, "You cannot vote for yourself.");
            if (string.IsNullOrWhiteSpace(description)) return new VoteResult(false, "You cannot vote without giving a reason.");

            try
            {
                var users = userRepository.Get();

                if (!users.Any(x => x.Id == voteeId)) return new VoteResult(false, "Unrecognised Votee.");
                if (!users.Any(x => x.Id == voterId)) return new VoteResult(false, "Unrecognised Voter.");

                var lastVotedFor = mvpRepository.GetLastVotedFor(voteeId);

                if (lastVotedFor != null && lastVotedFor >= DateTime.Now.AddMinutes(-1))
                    return new VoteResult(false, "A vote has already been received in the last minute for this team member.");

                mvpRepository.Vote(voteeId, voterId, description);
                SendVoteEmail(users, voteeId, voterId, description);

                return new VoteResult(true, "Thank you for voting.");
            }
            catch (Exception)
            {
                return new VoteResult(false, "Unknown Error.");
            }
        }

        public IEnumerable<Vote> Get()
        {
            return mvpRepository.Get();
        }

        public void AuthorizeVote(int voteId, bool isAuthorised)
        {
            mvpRepository.AuthorizeVote(voteId, isAuthorised);
        }

        private void SendVoteEmail(IEnumerable<ApplicationUser> users, int voteeId, int voterId, string description)
        {
            try
            {
                var votee = users.FirstOrDefault(x => x.Id == voteeId);
                var voter = users.FirstOrDefault(x => x.Id == voterId);

                var targetEmail = systemPropertyRepository.GetNonNullValue(SystemPropertyName.MvpDistributionEmail);
                var subject = string.Format("MVP: Vote placed for {0}", votee.DisplayName);
                var bodyText = "<div style=\"margin: 10px; font-family: tahoma; font-size: 1.2em;\"><h1>MVP Award</h1><p>A vote has been placed for {0}.</p><p>For: {1}</p></div>";

                bodyText = string.Format(bodyText, HttpUtility.HtmlEncode(votee.DisplayName), HttpUtility.HtmlEncode(description));

                if (!string.IsNullOrWhiteSpace(targetEmail))
                    emailService.SendEmail(targetEmail, subject, bodyText);
            }
            catch (Exception)
            {
                // TODO: Log
            }
        }
    }
}