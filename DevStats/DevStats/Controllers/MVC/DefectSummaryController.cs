using System.Web.Http;
using System.Web.Mvc;
using DevStats.Domain.DefectAnalysis;
using DevStats.Filters;
using DevStats.Models.DefectSummary;

namespace DevStats.Controllers.MVC
{
    public class DefectSummaryController : Controller
    {
        private readonly IDefectService service;

        public DefectSummaryController(IDefectService service)
        {
            this.service = service;
        }

        [IPAccess]
        public ActionResult Index([FromUri]bool showTotals = false)
        {
            var model = new DefectSummaryModel(service.GetSummary(), showTotals);

            return View(model);
        }
    }
}