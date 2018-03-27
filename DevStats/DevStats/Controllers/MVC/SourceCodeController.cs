using System;
using System.Web.Mvc;
using DevStats.Domain.SourceCode;

namespace DevStats.Controllers.MVC
{
    public class SourceCodeController : Controller
    {
        private const string DefaultRepository = "Cascade";
        private readonly ISourceCodeService service;

        public SourceCodeController(ISourceCodeService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.service = service;
        }

        [HttpGet]
        [Route("SourceCode/{repoName?}")]
        public ActionResult Branches(string repoName)
        {
            repoName = string.IsNullOrWhiteSpace(repoName) ? DefaultRepository : repoName;

            var model = service.Get(repoName);

            return View(model);
        }
    }
}