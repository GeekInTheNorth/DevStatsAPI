using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DevStats.Attributes;
using DevStats.Domain.DefectAnalysis;
using DevStats.Models.DefectTracking;

namespace DevStats.Controllers.API
{
    [AllowCors("*", "GET,POST")]
    public class DefectTrackingController : ApiController
    {
        private readonly IDefectService service;

        public DefectTrackingController(IDefectService service)
        {
            this.service = service;
        }

        [HttpGet]
        public DefectSummaries Get()
        {
            return service.GetSummary();
        }

        [HttpPost]
        public bool Save([FromBody]List<DefectModel> defects)
        {
            if (defects == null || !defects.Any())
                return false;

            var defectsToSave = defects.Select(x => new Defect(x.DefectId, x.Module, x.Type, x.Created, x.Closed));

            try
            {
                service.Save(defectsToSave);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}