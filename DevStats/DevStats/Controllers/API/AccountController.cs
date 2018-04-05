using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DevStats.Domain.Security;
using Microsoft.AspNet.Identity.Owin;

namespace DevStats.Controllers.API
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private ApplicationUserManager UserManager => HttpContext.Current.Request.GetOwinContext().Get<ApplicationUserManager>();

        [AcceptVerbs("POST", "PUT")]
        [Route("promote/{id}")]
        public async Task<HttpResponseMessage> Promote([FromUri]int id)
        {
            var user = HttpContext.Current.Request.GetOwinContext().Authentication.User;
            var isAdmin = await UserManager.IsInRoleAsync(user.Identity.Name, "Admin");

            if (!isAdmin) return Request.CreateResponse(HttpStatusCode.Forbidden);

            try
            {
                var result = await UserManager.AddToRoleAsync(id, "Admin");

                if (!result.Succeeded)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [AcceptVerbs("POST", "PUT")]
        [Route("demote/{id}")]
        public async Task<HttpResponseMessage> Demote([FromUri]int id)
        {
            var user = HttpContext.Current.Request.GetOwinContext().Authentication.User;
            var isAdmin = await UserManager.IsInRoleAsync(user.Identity.Name, "Admin");

            if (!isAdmin) return Request.CreateResponse(HttpStatusCode.Forbidden);

            try
            {
                var result = await UserManager.AddToRoleAsync(id, "User");

                if (!result.Succeeded)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [AcceptVerbs("DELETE")]
        [Route("delete/{id}")]
        public async Task<HttpResponseMessage> Delete([FromUri]int id)
        {
            var user = HttpContext.Current.Request.GetOwinContext().Authentication.User;
            var isAdmin = await UserManager.IsInRoleAsync(user.Identity.Name, "Admin");

            if (!isAdmin) return Request.CreateResponse(HttpStatusCode.Forbidden);

            try
            {
                var userToDelete = await UserManager.FindByIdAsync(id);

                var result = await UserManager.DeleteAsync(userToDelete);

                if (!result.Succeeded)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
