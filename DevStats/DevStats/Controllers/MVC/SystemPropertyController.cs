using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DevStats.Domain.Security;
using DevStats.Domain.SystemProperties;
using DevStats.Models.SystemProperties;
using Microsoft.AspNet.Identity.Owin;

namespace DevStats.Controllers.MVC
{
    public class SystemPropertyController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().Get<ApplicationUserManager>();

        private readonly ISystemPropertyService service;

        public SystemPropertyController(ISystemPropertyService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.service = service;
        }

        public async Task<ActionResult> Index()
        {
            var user = Request.GetOwinContext().Authentication.User;
            var isAdmin = await UserManager.IsInRoleAsync(user.Identity.Name, "Admin");

            if (!isAdmin)
                return RedirectToAction("NoAccess", "Home");

            var model = new SystemPropertiesModel
            {
                Properties = service.Get().ToList()
            };

            return View(model);
        }
    }
}