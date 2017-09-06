using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DevStats.Domain.DeveloperKpi;
using DevStats.Domain.Security;
using DevStats.Models.DeveloperKPI;
using Microsoft.AspNet.Identity.Owin;

namespace DevStats.Controllers.MVC
{
    [Authorize]
    public class DeveloperKPIController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().Get<ApplicationUserManager>();

        private readonly IDeveloperKpiService service;

        public DeveloperKPIController(IDeveloperKpiService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.service = service;
        }

        public async Task<ActionResult> Quality()
        {
            var user = Request.GetOwinContext().Authentication.User;
            var isAdmin = await UserManager.IsInRoleAsync(user.Identity.Name, "Admin");
            var developers = service.GetDevelopers();

            var model = new QualityKpiModel(developers, user.Identity.Name, isAdmin);
            model.Quality = service.GetQualityKpi(model.SelectedDeveloper);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Quality(string selectedDeveloper)
        {
            var user = Request.GetOwinContext().Authentication.User;
            var isAdmin = await UserManager.IsInRoleAsync(user.Identity.Name, "Admin");
            var developers = service.GetDevelopers();

            var model = new QualityKpiModel(developers, selectedDeveloper, isAdmin, selectedDeveloper);
            model.Quality = service.GetQualityKpi(model.SelectedDeveloper);

            return View(model);
        }
    }
}