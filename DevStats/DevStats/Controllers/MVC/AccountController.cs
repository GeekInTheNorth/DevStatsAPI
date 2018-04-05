using System;
using System.Linq;
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

        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().Get<ApplicationUserManager>();

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
                        ModelState.AddModelError(string.Empty, "Invalid Credentials");
                        return View(model);
                }
            }

            // Model is not valid, show login prompt again
            return View(model);
        }

        public ActionResult LogOff()
        {
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public async Task<ActionResult> Register()
        {
            var user = Request.GetOwinContext().Authentication.User;
            var isAdmin = await UserManager.IsInRoleAsync(user.Identity.Name, "Admin");

            if (!isAdmin) return RedirectToAction("NoAccess", "Home");

            return View(new RegisterModel());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            var user = Request.GetOwinContext().Authentication.User;
            var isAdmin = await UserManager.IsInRoleAsync(user.Identity.Name, "Admin");

            if (!isAdmin) return RedirectToAction("NoAccess", "Home");

            if (ModelState.IsValid)
            {
                var newUser = new ApplicationUser
                {
                    UserName = model.Email.Split('@')[0],
                    DisplayName = model.DisplayName,
                    EmailAddress = model.Email,
                    Role = model.Role,
                    PasswordHash = UserManager.PasswordHasher.HashPassword(new Guid().ToString().Replace("'", string.Empty))
                };

                try
                {
                    await UserManager.CreateAsync(newUser);

                    return RedirectToAction("Index", "Account");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("UserName", ex.Message);
                    return View(model);
                }
            }

            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var user = Request.GetOwinContext().Authentication.User;
            var isAdmin = await UserManager.IsInRoleAsync(user.Identity.Name, "Admin");

            if (!isAdmin) return RedirectToAction("NoAccess", "Home");

            var model = new IndexModel(UserStore.GetUsers());

            return View(model);
        }

        public ActionResult ResetPasswordRequest()
        {
            return View(new ResetPasswordRequestModel());
        }

        [HttpPost]
        public async Task<ActionResult> ResetPasswordRequest(ResetPasswordRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("Email", "Unrecognised email address");
                    return View(model);
                }
                else
                {
                    var root = Request.Url.GetLeftPart(UriPartial.Authority);
                    await UserManager.GeneratePasswordResetEmail(user, root);
                    
                    return RedirectToAction("ResetPasswordRequestSent", "Account");
                }
            }

            return View(model);
        }

        public ActionResult ResetPasswordRequestSent()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ResetPassword([System.Web.Http.FromUri]string emailAddress, [System.Web.Http.FromUri]string token)
        {
            var model = new ResetPasswordModel
            {
                EmailAddress = emailAddress,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.ProcessPasswordReset(model.EmailAddress, model.Token, model.NewPassword);
                if (result.Any())
                {
                    foreach (var error in result)
                        ModelState.AddModelError(string.Empty, error);
                    return View(model);
                }
                else
                    return RedirectToAction("Login", "Account");
            }

            return View(model);
        }
    }
}