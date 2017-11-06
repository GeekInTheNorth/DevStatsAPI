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
    [Route("MVP")]
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

        [HttpPost]
        public async Task<ActionResult> Vote(int voteeId, string description)
        {
            var user = Request.GetOwinContext().Authentication.User;
            var userDetails = await UserManager.FindByNameAsync(user.Identity.Name);

            var model = new MvpVoteModel
            {
                VoteeId = voteeId,
                Users = service.GetVotableUsers(userDetails.Id),
                HasAdminAccess = userDetails.IsInRole("Admin"),
                Result = service.Vote(voteeId, userDetails.Id, description)
            };

            return View(model);
        }
    }
}