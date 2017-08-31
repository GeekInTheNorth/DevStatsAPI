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

        public Task<ApplicationUser> FindByIdAsync(int userId)
        {
            var user = Context.Users.FirstOrDefault(x => x.ID == userId);
            var domainUser = new ApplicationUser
            {
                Id = user.ID,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                EmailAddress = user.EmailAddress,
                Role = user.Role
            };

            return Task.FromResult(domainUser);
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var user = Context.Users.FirstOrDefault(x => x.UserName == userName);

            if (user == null) return Task.FromResult<ApplicationUser>(null);

            var domainUser = new ApplicationUser
            {
                Id = user.ID,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                EmailAddress = user.EmailAddress,
                Role = user.Role
            };

            return Task.FromResult(domainUser);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.CompletedTask;
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            return Task.CompletedTask;
        }
    }
}