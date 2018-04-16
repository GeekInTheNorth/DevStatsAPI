using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using DevStats.Domain.Bitbucket;
using DevStats.Domain.Jira.Logging;

namespace DevStats.Controllers.API
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/bitbucket")]
    public class BitbucketController : ApiController
    {
        private readonly IBitbucketService service;
        private readonly IJiraLogRepository logRepository;

        public BitbucketController(IBitbucketService service, IJiraLogRepository logRepository)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (logRepository == null) throw new ArgumentNullException(nameof(logRepository));

            this.service = service;
            this.logRepository = logRepository;
        }

        [HttpPost]
        [Route("build/status")]
        public void BuildStatus([FromBody]BuildStatusModel model)
        {
            service.Update(model, Request.RequestUri.GetLeftPart(UriPartial.Authority));
        }

        [HttpPost]
        [Route("pullrequest/approve")]
        public void Approve([FromBody]Domain.Bitbucket.Models.Webhook.Payload payload)
        {
            service.Approval(payload, true);
        }

        [HttpPost]
        [Route("pullrequest/disapprove")]
        public void Disapprove([FromBody]Domain.Bitbucket.Models.Webhook.Payload payload)
        {
            service.Approval(payload, false);
        }

        [HttpPost]
        [Route("pullrequest/test")]
        public async Task TestAndLog()
        {
            var content = await Request.Content.ReadAsStringAsync();

            logRepository.Log("n/a", "Recieved Test Message", content, true);
        }
    }
}
