using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DevStats.Domain.Security;
using Microsoft.AspNet.Identity.Owin;

namespace DevStats.Controllers.API
{
    public class SecureBaseApiController : ApiController
    {
        private ApplicationUserManager UserManager => HttpContext.Current.Request.GetOwinContext().Get<ApplicationUserManager>();

        protected async Task<bool> CanAccess()
        {
            var isIpAllowed = IsIpAllowed();

            if (isIpAllowed) return true;

            return await IsAuthorised();
        }

        protected async Task<bool> IsAuthorised()
        {
            var hasAuthHeader = HttpContext.Current.Request.Headers.AllKeys.Contains("Authorization");

            if (!hasAuthHeader) return false;

            var authHeader = GetDecodedAuthorization(HttpContext.Current.Request.Headers["Authorization"]);

            if (!authHeader.Contains(":"))
                return false;

            var username = authHeader.Split(':')[0];
            var password = authHeader.Split(':')[1];

            var user = await UserManager.FindByNameAsync(username);
            var passwordCheck = await UserManager.CheckPasswordAsync(user, password);

            return passwordCheck;
        }

        protected bool IsIpAllowed()
        {
            var config = ConfigurationManager.AppSettings["AllowedIpAddresses"];

            if (config == "N/A" || Request.IsLocal())
                return true;

            var userIp = HttpContext.Current.Request.UserHostAddress;

            return config.Contains(userIp);
        }

        private string GetDecodedAuthorization(string authorizationHeader)
        {
            if (authorizationHeader.Contains(":")) return authorizationHeader;

            var authData = Convert.FromBase64String(authorizationHeader);

            return Encoding.UTF8.GetString(authData);
        }
    }
}