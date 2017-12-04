using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DevStats.Domain.Reports.ReleaseReport;
using DevStats.Domain.Security;
using DevStats.Models.ReleaseQuality;
using Microsoft.AspNet.Identity.Owin;

namespace DevStats.Controllers.MVC
{
    [Authorize]
    public class ReleaseQualityController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().Get<ApplicationUserManager>();

        private readonly IReleaseQualityService service;

        public ReleaseQualityController(IReleaseQualityService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.service = service;
        }

        public async Task<ActionResult> Index()
        {
            var user = Request.GetOwinContext().Authentication.User;
            var isAdmin = await UserManager.IsInRoleAsync(user.Identity.Name, "Admin");

            var model = new ReleaseQualityModel
            {
                IsAdmin = isAdmin,
                Releases = service.Get().ToList()
            };

            return View(model);
        }
    }
}