using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DevStats.Domain.KPI;
using DevStats.Models.KPI;

namespace DevStats.Controllers.API
{
    public class KPIController : SecureBaseApiController
    {
        private readonly INewFeatureFailureRateService service;

        public KPIController(INewFeatureFailureRateService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.service = service;
        }

        [HttpGet]
        [Route("api/DeveloperKPI/Quality")]
        public async Task<HttpResponseMessage> Quality()
        {
            var canAccess = await CanAccess();

            if (!canAccess) return Request.CreateResponse(HttpStatusCode.Forbidden);

            var kpis = service.GetQualityKpi();
            var model = new NewFeatureFailureRateApiModel(kpis);

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        [HttpGet]
        [Route("api/DeveloperKPI/Quality/{developer}")]
        public async Task<HttpResponseMessage> Quality([FromUri]string developer)
        {
            var canAccess = await CanAccess();

            if (!canAccess) return Request.CreateResponse(HttpStatusCode.Forbidden);

            var kpi = service.GetQualityKpi(developer);
            var model = new NewFeatureFailureRateApiModel(developer, kpi);

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }
    }
}