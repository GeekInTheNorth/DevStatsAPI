using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevStats.Domain.Security
{
    public class ApplicationUserStore : IApplicationUserStore<ApplicationUser, int>
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposeAll)
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

            return repository.UpdateAsync(user);
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            return Task.CompletedTask;
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            return repository.UpdateAsync(user);
        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return repository.Get();
        }

        public Task SetEmailAsync(ApplicationUser user, string email)
        {
            user.EmailAddress = email;

            return repository.UpdateAsync(user);
        }

        public Task<string> GetEmailAsync(ApplicationUser user)
        {
            return Task.FromResult(user.EmailAddress);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return repository.FindByEmailAsync(email);
        }

        public Task SetSecurityStampAsync(ApplicationUser user, string stamp)
        {
            user.PasswordResetToken = stamp;
            user.PasswordResetTokenExpiry = DateTime.Now.AddDays(1);

            return repository.UpdateAsync(user);
        }

        public Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            var expiry = user.PasswordResetTokenExpiry ?? DateTime.MinValue;
            var token = user.PasswordResetTokenExpiry < DateTime.Now ? "Error" : user.PasswordResetToken;
            token = token ?? string.Empty;

            return Task.FromResult(token);
        }
    }
}