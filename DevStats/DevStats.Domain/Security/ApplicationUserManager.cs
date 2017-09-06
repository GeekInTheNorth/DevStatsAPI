using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace DevStats.Domain.Security
{
    public class ApplicationUserManager : UserManager<ApplicationUser, int>
    {
        private readonly IUserPasswordStore<ApplicationUser, int> store;

        public ApplicationUserManager(IUserPasswordStore<ApplicationUser, int> store) : base(store)
        {
            this.store = store;
        }

        public async Task<bool> IsInRoleAsync(string userName, string role)
        {
            var user = await store.FindByNameAsync(userName);

            if (user == null) return false;

            return user.Role.Equals(role, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}