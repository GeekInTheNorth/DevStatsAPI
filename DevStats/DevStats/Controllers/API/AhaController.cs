using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DevStats.Domain.Aha;

namespace DevStats.Controllers.API
{
    public class AhaController : ApiController
    {
        private readonly IAhaService service;

        public AhaController(IAhaService service)
        {
            this.service = service;
        }

        [HttpGet]
        public HttpResponseMessage UpdateFromAha([FromUri]DateTime? earliestDate = null)
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