using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DevStats.Domain.KPI;
using DevStats.Domain.Security;
using DevStats.Models.KPI;
using Microsoft.AspNet.Identity.Owin;

namespace DevStats.Controllers.MVC
{
    [Authorize]
    public class KPIController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().Get<ApplicationUserManager>();

        private readonly IActualsVsEstimatesService actualsVsEstimatesService;

        public KPIController(IActualsVsEstimatesService actualsVsEstimatesService)
        {
            if (actualsVsEstimatesService == null) throw new ArgumentNullException(nameof(actualsVsEstimatesService));

            this.actualsVsEstimatesService = actualsVsEstimatesService;
        }

        [HttpGet]
        public async Task<ActionResult> ActualsVsEstimates()
        {
            var user = Request.GetOwinContext().Authentication.User;
            var isAdmin = await UserManager.IsInRoleAsync(user.Identity.Name, "Admin");

            var teamMembers = actualsVsEstimatesService.GetTeamMembers();
            var model = new ActualsVsEstimatesModel(teamMembers, user.Identity.Name, isAdmin);
            model.Summary = actualsVsEstimatesService.Get(model.SelectedTeamMember);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ActualsVsEstimates(string selectedTeamMember)
        {
            var user = Request.GetOwinContext().Authentication.User;
            var isAdmin = await UserManager.IsInRoleAsync(user.Identity.Name, "Admin");

            var teamMembers = actualsVsEstimatesService.GetTeamMembers();
            var model = new ActualsVsEstimatesModel(teamMembers, selectedTeamMember, user.Identity.Name, isAdmin);
            model.Summary = actualsVsEstimatesService.Get(model.SelectedTeamMember);

            return View(model);
        }
    }
}