using System.Web.Mvc;
using DevStats.Domain.DefectAnalysis;
using DevStats.Domain.Jira;
using DevStats.Models.Jira;

namespace DevStats.Controllers.MVC
{
    [Authorize]
    public class DefectReportController : Controller
    {
        private readonly IDefectService defectService;
        private readonly IJiraService jiraService; 

        public DefectReportController(IDefectService defectService, IJiraService jiraService)
        {
            this.defectService = defectService;
            this.jiraService = jiraService;
        }

        [HttpGet]
        public ActionResult Analysis()
        {
            var model = defectService.GetSummary();

            return View(model);
        }

        [HttpGet]
        public ActionResult JiraSummary()
        {
            var defects = jiraService.GetDefects();
            var model = new DefectAnalysisModel(defects);

            return View(model);
        }
    }
}