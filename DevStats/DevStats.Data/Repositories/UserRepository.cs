using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevStats.Domain.Security;

namespace DevStats.Data.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public Task CreateAsync(ApplicationUser user)
        {
            Context.Users.Add(new Entities.User
            {
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                Role = user.Role ?? "User",
                EmailAddress = user.EmailAddress
            });

            return Context.SaveChangesAsync();
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            var userToDelete = Context.Users.FirstOrDefault(x => x.ID == user.Id);

            if (userToDelete != null)
            {
                Context.Users.Remove(userToDelete);
            }

            return Context.SaveChangesAsync();
        }

        public Task<ApplicationUser> FindByEmailAsync(string emailAddress)
        {
            var user = Context.Users.FirstOrDefault(x => x.EmailAddress == emailAddress);

            if (user == null) return Task.FromResult<ApplicationUser>(null);

            var domainUser = ConvertToDomain(user);

            return Task.FromResult(domainUser);
        }

        public Task<ApplicationUser> FindByIdAsync(int userId)
        {
            var user = Context.Users.FirstOrDefault(x => x.ID == userId);

            if (user == null) return Task.FromResult<ApplicationUser>(null);

            var domainUser = ConvertToDomain(user);

            return Task.FromResult(domainUser);
        }

        public Task<ApplicationUser> FindByNameOrEmail(string searchText)
        {
            var user = Context.Users.FirstOrDefault(x => x.UserName == searchText || x.EmailAddress == searchText);

            if (user == null) return Task.FromResult<ApplicationUser>(null);

            var domainUser = ConvertToDomain(user);

            return Task.FromResult(domainUser);
        }

        public IEnumerable<ApplicationUser> Get()
        {
            var data = Context.Users.ToList();

            return data.Select(x => ConvertToDomain(x));
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.CompletedTask;
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            var data = Context.Users.FirstOrDefault(x => x.ID == user.Id);

            if (data != null)
            {
                data.EmailAddress = user.EmailAddress;
                data.LoginErrors = user.AccessAttempts;
                data.PasswordHash = user.PasswordHash;
                data.PasswordResetToken = user.PasswordResetToken;
                data.PasswordResetTokenExpiry = user.PasswordResetTokenExpiry;
                data.Role = user.Role;
            }

            return Context.SaveChangesAsync();
        }

        private ApplicationUser ConvertToDomain(Entities.User data)
        {
            return new ApplicationUser
            {
                Id = data.ID,
                EmailAddress = data.EmailAddress,
                UserName = data.UserName,
                PasswordHash = data.PasswordHash,
                Role = data.Role,
                AccessAttempts = data.LoginErrors,
                PasswordResetToken = data.PasswordResetToken,
                PasswordResetTokenExpiry = data.PasswordResetTokenExpiry,
                DisplayName = data.DisplayName
            };
        }
    }
}