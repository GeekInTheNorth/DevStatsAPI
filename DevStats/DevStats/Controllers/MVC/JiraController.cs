using System;
using System.Linq;
using System.Web.Mvc;
using DevStats.Domain.Jira;
using DevStats.Filters;
using DevStats.Models.Jira;

namespace DevStats.Controllers.MVC
{
    [IPAccess]
    public class JiraController : Controller
    {
        private readonly IJiraService service;
        private readonly IJiraSender sender;

        public JiraController(IJiraService service, IJiraSender sender)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (sender == null) throw new ArgumentNullException(nameof(sender));

            this.service = service;
            this.sender = sender;
        }

        [HttpGet]
        public ActionResult Audit()
        {
            var dateFrom = DateTime.Today;
            var dateTo = DateTime.Today;

            var model = new AuditModel
            {
                FilterFrom = dateFrom,
                FilterTo = dateFrom,
                AuditItems = service.GetJiraAudit(dateFrom, dateTo.AddDays(1)).Select(x => new AuditModelItem(x)).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Audit(DateTime dateFrom, DateTime dateTo)
        {
            var model = new AuditModel
            {
                FilterFrom = dateFrom,
                FilterTo = dateTo,
                AuditItems = service.GetJiraAudit(dateFrom, dateTo.AddDays(1)).Select(x => new AuditModelItem(x)).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult ApiTest()
        {
            return View(new ApiTestModel());
        }

        [HttpPost]
        public ActionResult ApiTest(string apiUrl, string jiraId)
        {
            var url = Request.Url.AbsoluteUri.Replace(Request.Url.LocalPath, apiUrl).Replace("@@id@@", jiraId.ToUpper());
            var model = new ApiTestModel
            {
                PostResult = sender.Post(url, string.Empty)
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult StateChecker()
        {
            return View(new StateCheckerModel());
        }

        [HttpPost]
        public ActionResult StateChecker(string stateRequestData)
        {
            var model = new StateCheckerModel();

            try
            {
                model.Summaries = service.GetStateSummaries(stateRequestData.ToUpper()).ToList();
            }
            catch(Exception)
            {
                model.ErrorMessage = "An error has occured.";
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult DefectAnalysis()
        {
            var defects = service.GetDefects();
            var model = new DefectAnalysisModel(defects);

            return View(model);
        }

        [HttpGet]
        public ActionResult SprintPlanner()
        {
            return View();
        }
    }
}