using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using DevStats.Domain.Security;
using DevStats.Domain.SystemProperties;
using DevStats.Models.SystemProperties;
using Microsoft.AspNet.Identity.Owin;

namespace DevStats.Controllers.API
{
    [RoutePrefix("api/systemproperty")]
    public class SystemPropertyController : ApiController
    {
        private ApplicationUserManager UserManager => HttpContext.Current.Request.GetOwinContext().Get<ApplicationUserManager>();

        private readonly ISystemPropertyService service;

        public SystemPropertyController(ISystemPropertyService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.service = service;
        }

        [AcceptVerbs("POST","PUT")]
        [Route("save")]
        public async Task<HttpResponseMessage> Save([FromBody]SystemPropertyModel model)
        {
            var user = HttpContext.Current.Request.GetOwinContext().Authentication.User;
            var isAdmin = await UserManager.IsInRoleAsync(user.Identity.Name, "Admin");
            
            if (!isAdmin) return Request.CreateResponse(HttpStatusCode.Forbidden);

            try
            {
                service.Save(model.GetSystemPropertyName(), model.Value.Trim());
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
