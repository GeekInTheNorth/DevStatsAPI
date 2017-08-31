using System.Web.Mvc;

namespace DevStats.Controllers.MVC
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}