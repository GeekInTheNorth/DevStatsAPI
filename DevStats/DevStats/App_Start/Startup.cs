using DevStats.Data.Repositories;
using DevStats.Domain.Communications;
using DevStats.Domain.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace DevStats
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new ApplicationUserStore(new UserRepository()));
            app.CreatePerOwinContext<ApplicationUserManager>((opt, cont) => new ApplicationUserManager(cont.Get<ApplicationUserStore>(), new EmailService(new SystemPropertyRepository())));
            app.CreatePerOwinContext<SignInManager<ApplicationUser, int>>((opt, cont) => new SignInManager<ApplicationUser, int>(cont.Get<ApplicationUserManager>(), cont.Authentication));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }
    }
}