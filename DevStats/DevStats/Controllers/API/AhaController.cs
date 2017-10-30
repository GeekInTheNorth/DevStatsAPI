using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DevStats.Domain.Aha;

namespace DevStats.Controllers.API
{
    [RoutePrefix("api/aha")]
    public class AhaController : ApiController
    {
        private readonly IAhaService service;

        public AhaController(IAhaService service)
        {
            this.service = service;
        }

        [AcceptVerbs("GET", "POST")]
        [Route("GetDefectUpdates")]
        [Route("GetDefectUpdates/{earliestDate}")]
        public HttpResponseMessage GetDefectUpdates([FromUri]DateTime? earliestDate = null)
        {
            var earliestModifiedDate = earliestDate ?? DateTime.Today.AddMonths(-2);

            try
            {
                service.UpdateDefectsFromAha(earliestModifiedDate);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}