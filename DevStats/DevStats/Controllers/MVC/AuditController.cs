using System;
using System.Linq;
using System.Web.Mvc;
using DevStats.Domain.Logging;
using DevStats.Models.Audit;

namespace DevStats.Controllers.MVC
{
    [Authorize]
    public class ApiAuditController : Controller
    {
        private readonly IApiLogService service;

        public ApiAuditController(IApiLogService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.service = service;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var dateFrom = DateTime.Today;
            var dateTo = DateTime.Today;

            var model = new ApiAuditModel
            {
                FilterFrom = dateFrom,
                FilterTo = dateFrom,
                AuditItems = service.Get(dateFrom, dateTo.AddDays(1)).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(DateTime dateFrom, DateTime dateTo)
        {
            var model = new ApiAuditModel
            {
                FilterFrom = dateFrom,
                FilterTo = dateTo,
                AuditItems = service.Get(dateFrom, dateTo.AddDays(1)).ToList()
            };

            return View(model);
        }
    }
}