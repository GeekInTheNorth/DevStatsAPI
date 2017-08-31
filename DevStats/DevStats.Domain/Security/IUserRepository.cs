using System.Threading.Tasks;

namespace DevStats.Domain.Security
{
    public interface IUserRepository
    {
        Task CreateAsync(ApplicationUser user);

        Task DeleteAsync(ApplicationUser user);

        Task<ApplicationUser> FindByIdAsync(int userId);

        Task<ApplicationUser> FindByNameAsync(string userName);

        Task UpdateAsync(ApplicationUser user);

        void Dispose();
    }
}
