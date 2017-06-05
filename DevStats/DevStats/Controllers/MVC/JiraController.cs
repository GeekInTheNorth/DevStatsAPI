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

        public JiraController(IJiraService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.service = service;
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
                AuditItems = service.GetJiraAudit(dateFrom, dateTo.AddDays(1)).ToList()
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
                AuditItems = service.GetJiraAudit(dateFrom, dateTo.AddDays(1)).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult ApiTest()
        {
            return View(new ApiTestModel());
        }

        [HttpPost]
        public ActionResult ApiTest(string apiUrl, string jiraId, string package)
        {
            var url = Request.Url.AbsoluteUri.Replace(Request.Url.LocalPath, apiUrl).Replace("@@id@@", jiraId.ToUpper());

            var jiraSender = new JiraSender(new JiraConvertor());
            var model = new ApiTestModel();
            model.PostResult = jiraSender.Post(url, package);

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
            catch(Exception ex)
            {
                model.ErrorMessage = "An error has occured.";
            }

            return View(model);
        }
    }
}