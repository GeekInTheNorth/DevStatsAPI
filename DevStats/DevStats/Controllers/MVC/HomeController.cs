using System;
using System.Linq;
using System.Web.Mvc;
using DevStats.Domain.SiteStats;
using DevStats.Models.Home;

namespace DevStats.Controllers.MVC
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ISiteStatisticService service;

        public HomeController(ISiteStatisticService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.service = service;
        }

        // GET: Home
        public ActionResult Index()
        {
            var model = new HomeModel
            {
                Statistics = service.GetUsageStatistics().ToList()
            };

            return View(model);
        }

        public ActionResult NoAccess()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}