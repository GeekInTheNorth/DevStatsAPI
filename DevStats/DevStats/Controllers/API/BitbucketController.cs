using System;
using System.Web.Http;
using System.Web.Http.Cors;
using DevStats.Domain.Bitbucket;

namespace DevStats.Controllers.API
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/bitbucket")]
    public class BitbucketController : ApiController
    {
        private readonly IBitbucketService service;

        public BitbucketController(IBitbucketService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.service = service;
        }

        [HttpPost]
        [Route("build/status")]
        public void BuildStatus([FromBody]BuildStatusModel model)
        {
            service.Update(model, Request.RequestUri.GetLeftPart(UriPartial.Authority));
        }
    }
}
