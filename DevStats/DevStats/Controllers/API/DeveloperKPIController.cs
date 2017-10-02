using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DevStats.Domain.DeveloperKpi;
using DevStats.Models.DeveloperKPI;

namespace DevStats.Controllers.API
{
    public class DeveloperKPIController : SecureBaseApiController
    {
        private readonly IDeveloperKpiService service;

        public DeveloperKPIController(IDeveloperKpiService service)
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
            var model = new QualityKpiApiModel(kpis);

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        [HttpGet]
        [Route("api/DeveloperKPI/Quality/{developer}")]
        public async Task<HttpResponseMessage> Quality([FromUri]string developer)
        {
            var canAccess = await CanAccess();

            if (!canAccess) return Request.CreateResponse(HttpStatusCode.Forbidden);

            var kpi = service.GetQualityKpi(developer);
            var model = new QualityKpiApiModel(developer, kpi);

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }
    }
}