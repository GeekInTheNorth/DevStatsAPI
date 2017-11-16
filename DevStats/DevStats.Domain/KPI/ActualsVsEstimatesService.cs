using System;
using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.Security;

namespace DevStats.Domain.KPI
{
    public class ActualsVsEstimatesService : IActualsVsEstimatesService
    {
        private readonly IActualsVsEstimatesRepository actualsVsEstimatesRepository;
        private readonly IUserRepository userRepository;

        public ActualsVsEstimatesService(IActualsVsEstimatesRepository actualsVsEstimatesRepository, IUserRepository userRepository)
        {
            if (actualsVsEstimatesRepository == null) throw new ArgumentNullException(nameof(actualsVsEstimatesRepository));
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));

            this.actualsVsEstimatesRepository = actualsVsEstimatesRepository;
            this.userRepository = userRepository;
        }

        public Dictionary<string, string> GetTeamMembers()
        {
            var validUserRoles = new string[] { "User", "Admin" };

            return userRepository.Get()
                                 .Where(x => validUserRoles.Contains(x.Role))
                                 .OrderBy(x => x.DisplayName)
                                 .ToDictionary(x => x.UserName, y => y.DisplayName);
        }

        public ActualsVsEstimateSummary Get(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return null;

            return actualsVsEstimatesRepository.Get(userName);
        }
    }
}