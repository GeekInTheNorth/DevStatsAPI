using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevStats.Domain.Security
{
    public interface IUserRepository
    {
        Task CreateAsync(ApplicationUser user);

        Task DeleteAsync(ApplicationUser user);

        Task<ApplicationUser> FindByIdAsync(int userId);

        Task<ApplicationUser> FindByNameOrEmail(string searchText);

        Task<ApplicationUser> FindByEmailAsync(string emailAddress);

        Task UpdateAsync(ApplicationUser user);

        IEnumerable<ApplicationUser> Get();

        void Dispose();
    }
}
