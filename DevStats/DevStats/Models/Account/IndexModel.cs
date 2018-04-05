using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DevStats.Domain.Security;

namespace DevStats.Models.Account
{
    public class IndexModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }

        public IndexModel(IEnumerable<ApplicationUser> users)
        {
            Users = users.OrderBy(x => x.DisplayName);
        }
    }
}