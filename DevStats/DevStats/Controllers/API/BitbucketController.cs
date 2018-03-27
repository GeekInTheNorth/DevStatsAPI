using System;
using System.Web.Http;
using System.Web.Http.Cors;
using DevStats.Domain.Bitbucket;
using DevStats.Domain.Messages;

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

        [HttpGet]
        [Route("test/{account}/{repo}")]
        public void Test(string account, string repo)
        {
            var url = string.Format("https://api.bitbucket.org/1.0/repositories/{0}/{1}/branches", account, repo);
            var sender = new BitbucketSender(new JsonConvertor());

            var result = sender.Get<string>(url);
        }
    }
}
