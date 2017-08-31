using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DevStats.Domain.Security;
using DevStats.Models.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace DevStats.Controllers.MVC
{
    public class AccountController : Controller
    {
        private ApplicationUserStore UserStore => HttpContext.GetOwinContext().Get<ApplicationUserStore>();

        private SignInManager<ApplicationUser, int> SignInManager => HttpContext.GetOwinContext().Get<SignInManager<ApplicationUser, int>>();

        public ActionResult Login([System.Web.Http.FromUri]string returnUrl)
        {
            var model = new LoginModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (true == false)
                {
                    var hasher = new PasswordHasher();

                    var newUser = new ApplicationUser
                    {
                        UserName = model.UserName,
                        EmailAddress = "bob@bob.com",
                        Role = "Admin",
                        PasswordHash = hasher.HashPassword(model.Password)
                    };
                    await UserStore.CreateAsync(newUser);
                }

                Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                var signInStatus = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                switch (signInStatus)
                {
                    case SignInStatus.Success:
                        if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                            return Redirect(model.ReturnUrl);
                        else
                            return RedirectToAction("Index", "Home");
                    default:
                        ModelState.AddModelError("", "Invalid Credentials");
                        return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff()
        {
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Login", "Account");
        }
    }
}