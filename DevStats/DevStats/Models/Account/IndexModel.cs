using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DevStats.Domain.Security;

namespace DevStats.Models.Account
{
    public class IndexModel
    {
        public IEnumerable<IndexModelUser> Users { get; set; }

        public IndexModel(IEnumerable<ApplicationUser> users)
        {
            Users = users.Select(x => new IndexModelUser(x));
        }
    }

    public class IndexModelUser
    {
        public string User
        {
            get { return FormatName(UserName); }
        }

        public string UserName { get; private set; }

        public string EmailAddress { get; private set; }

        public string Role { get; private set; }

        public IndexModelUser(ApplicationUser user)
        {
            UserName = user.UserName;
            EmailAddress = user.EmailAddress;
            Role = user.Role;
        }

        private string FormatName(string userName)
        {
            userName = userName.Replace('.', ' ');

            TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
            return textInfo.ToTitleCase(userName);
        }
    }
}