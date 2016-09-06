using System.Collections.Generic;
using System.Web.Http;
using DevStats.Attributes;
using DevStats.Data.Repositories;
using DevStats.Domain.DefectAnalysis;

namespace DevStats.Controllers
{
    [AllowCors("*", "GET,POST")]
    public class DefectTrackingController : ApiController
    {
        private readonly IDefectService service;

        public DefectTrackingController()
        {
            service = new DefectService(new DefectRepository());
        }

        [HttpGet]
        public DefectSummaries Get()
        {
            return service.GetSummary();
        }

        [HttpPost]
        public bool Save([FromBody]IEnumerable<Defect> defects)
        {
            try
            {
                service.Save(defects);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}