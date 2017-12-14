using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DevStats.Domain.MVP;
using DevStats.Domain.Security;
using DevStats.Models.MVP;
using Microsoft.AspNet.Identity.Owin;

namespace DevStats.Controllers.API
{
    [RoutePrefix("api/mvp")]
    public class MvpController : ApiController
    {
        private ApplicationUserManager UserManager => HttpContext.Current.Request.GetOwinContext().Get<ApplicationUserManager>();

        private readonly IMvpService service;

        public MvpController(IMvpService service)
        {
            if (service == null) throw new System.ArgumentNullException(nameof(service));

            this.service = service;
        }

        [HttpPost]
        [Route("Vote")]
        public async Task<HttpResponseMessage> Vote([FromBody]VoteModel vote)
        {
            try
            {
                var user = HttpContext.Current.Request.GetOwinContext().Authentication.User;
                var userDetails = await UserManager.FindByNameAsync(user.Identity.Name);

                if (userDetails == null) return Request.CreateResponse(HttpStatusCode.Forbidden);
                if (vote == null) return Request.CreateResponse(HttpStatusCode.BadRequest);

                var result = service.Vote(vote.VoteeId, userDetails.Id, vote.Reason);

                return Request.CreateResponse(HttpStatusCode.Created, result);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("AuthorizeVote/{id}")]
        public async Task<HttpResponseMessage> AuthorizeVote(int id)
        {
            var user = HttpContext.Current.Request.GetOwinContext().Authentication.User;
            var isAdmin = await UserManager.IsInRoleAsync(user.Identity.Name, "Admin");

            if (!isAdmin) return Request.CreateResponse(HttpStatusCode.Forbidden);

            try
            {
                service.AuthorizeVote(id, true);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("DeauthorizeVote/{id}")]
        public async Task<HttpResponseMessage> DeauthorizeVote(int id)
        {
            var user = HttpContext.Current.Request.GetOwinContext().Authentication.User;
            var isAdmin = await UserManager.IsInRoleAsync(user.Identity.Name, "Admin");

            if (!isAdmin) return Request.CreateResponse(HttpStatusCode.Forbidden);

            try
            {
                service.AuthorizeVote(id, false);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}