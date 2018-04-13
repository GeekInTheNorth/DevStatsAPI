using System;
using System.Web.Http;
using System.Web.Http.Cors;
using DevStats.Domain.Bitbucket;
using DevStats.Domain.Jira.Logging;
using DevStats.Domain.Messages;

namespace DevStats.Controllers.API
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/bitbucket")]
    public class BitbucketController : ApiController
    {
        private readonly IBitbucketService service;
        private readonly IJiraLogRepository logRepository;
        private readonly IJsonConvertor jsonConvertor;

        public BitbucketController(IBitbucketService service, IJiraLogRepository logRepository, IJsonConvertor jsonConvertor)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (logRepository == null) throw new ArgumentNullException(nameof(logRepository));
            if (jsonConvertor == null) throw new ArgumentNullException(nameof(jsonConvertor));

            this.service = service;
            this.logRepository = logRepository;
            this.jsonConvertor = jsonConvertor;
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
            var content = jsonConvertor.Serialize(payload);

            logRepository.Log("n/a", "Approve Pull Request", content, true);
        }

        [HttpPost]
        [Route("pullrequest/disapprove")]
        public void Disapprove([FromBody]Domain.Bitbucket.Models.Webhook.Payload payload)
        {
            var content = jsonConvertor.Serialize(payload);

            logRepository.Log("n/a", "Disapprove Pull Request", content, true);
        }

        [HttpPost]
        [Route("pullrequest/test")]
        public void testapproval()
        {
            var content = Request.Content.ToString();

            logRepository.Log("n/a", "Disapprove Pull Request", content, true);
        }
    }
}
