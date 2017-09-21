using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DevStats.Domain.DeveloperKpi;
using DevStats.Models.DeveloperKPI;

namespace DevStats.Controllers.API
{
    public class DeveloperKPIController : ApiController
    {
        private readonly IDeveloperKpiService service;

        public DeveloperKPIController(IDeveloperKpiService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.service = service;
        }

        [HttpGet]
        [Route("api/DeveloperKPI/Quality")]
        public HttpResponseMessage Quality()
        {
            if (!CanAccessApi())
                return Request.CreateResponse(HttpStatusCode.Forbidden);

            var kpis = service.GetQualityKpi();
            var model = new QualityKpiApiModel(kpis);

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        [HttpGet]
        [Route("api/DeveloperKPI/Quality/{developer}")]
        public HttpResponseMessage Quality([FromUri]string developer)
        {
            if (!CanAccessApi())
                return Request.CreateResponse(HttpStatusCode.Forbidden);

            var kpi = service.GetQualityKpi(developer);
            var model = new QualityKpiApiModel(developer, kpi);

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        [HttpGet]
        [Route("api/DeveloperKPI/IpCheck")]
        public HttpResponseMessage IpCheck()
        {
            var userIp = HttpContext.Current.Request.UserHostAddress;

            return Request.CreateResponse(HttpStatusCode.OK, userIp);
        }

        private bool CanAccessApi()
        {
            var config = ConfigurationManager.AppSettings["EmailHost"];

            if (config == "N/A" || Request.IsLocal())
                return true;

            var userIp = HttpContext.Current.Request.UserHostAddress;

            return config.Contains(userIp);
        }
    }
}