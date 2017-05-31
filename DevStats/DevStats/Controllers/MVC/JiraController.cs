using System;
using System.Linq;
using System.Web.Mvc;
using DevStats.Domain.Jira;
using DevStats.Filters;
using DevStats.Models.Jira;

namespace DevStats.Controllers.MVC
{
    public class JiraController : Controller
    {
        private readonly IJiraService service;

        public JiraController(IJiraService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.service = service;
        }

        [HttpGet]
        [IPAccess]
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
        [IPAccess]
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
    }
}