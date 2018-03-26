using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DevStats.Domain.MVP;
using DevStats.Domain.Security;
using DevStats.Models.MVP;
using Microsoft.AspNet.Identity.Owin;

namespace DevStats.Controllers.MVC
{
    [Authorize]
    public class MvpController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().Get<ApplicationUserManager>();

        private readonly IMvpService service;

        public MvpController(IMvpService service)
        {
            if (service == null) throw new System.ArgumentNullException(nameof(service));

            this.service = service;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var user = Request.GetOwinContext().Authentication.User;
            var isAdmin = await UserManager.IsInRoleAsync(user.Identity.Name, "Admin");

            if (!isAdmin)
                return RedirectToAction("Vote", "Mvp");

            var model = new MvpVoteListModel
            {
                Votes = service.Get().ToList()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Vote()
        {
            var user = Request.GetOwinContext().Authentication.User;
            var userDetails = await UserManager.FindByNameAsync(user.Identity.Name);

            var model = new MvpVoteModel
            {
                Users = service.GetVotableUsers(userDetails.Id),
                HasAdminAccess = userDetails.IsInRole("Admin")
            };

            return View(model);
        }
    }
}