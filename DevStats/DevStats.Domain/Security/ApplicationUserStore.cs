using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace DevStats.Domain.Security
{
    public class ApplicationUserManager : UserManager<ApplicationUser, int>
    {
        public ApplicationUserManager(IUserPasswordStore<ApplicationUser, int> store) : base(store)
        {
        }
    }

    public class ApplicationUserStore : IUserPasswordStore<ApplicationUser, int>, IUserLockoutStore<ApplicationUser, int>, IUserTwoFactorStore<ApplicationUser, int>
    {
        private readonly IUserRepository repository;

        public ApplicationUserStore(IUserRepository repository)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));

            this.repository = repository;
        }

        public async Task CreateAsync(ApplicationUser user)
        {
            var userFound = await repository.FindByNameAsync(user.UserName);

            if (userFound != null)
                throw new Exception("User Exists");

            await repository.CreateAsync(user);

            return;
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            return repository.DeleteAsync(user);
        }

        public void Dispose()
        {
            repository.Dispose();
        }

        public Task<ApplicationUser> FindByIdAsync(int userId)
        {
            return repository.FindByIdAsync(userId);
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return repository.FindByNameAsync(userName);
        }

        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            return Task.FromResult(user.AccessAttempts);
        }

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            return Task.FromResult(false);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            return Task.FromResult(DateTimeOffset.Now);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return Task.FromResult(false);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            user.AccessAttempts++;

            return Task.FromResult(user.AccessAttempts);
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            user.AccessAttempts = 0;

            return Task.CompletedTask;
        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            return Task.CompletedTask;
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.CompletedTask;
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            return Task.CompletedTask;
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            return repository.UpdateAsync(user);
        }
    }
}