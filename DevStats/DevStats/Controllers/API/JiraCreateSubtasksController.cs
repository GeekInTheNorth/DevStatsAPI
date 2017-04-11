using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using DevStats.Domain.Jira;

namespace DevStats.Controllers.API
{
    public class JiraCreateSubtasksController : ApiController
    {
        private readonly IJiraService service;

        public JiraCreateSubtasksController(IJiraService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [EnableCors("*", "*", "*")]
        [HttpPost]
        public void CreateSubtasks([FromUri]string issueId, [FromUri]string displayIssueId)
        {
            var jsonContent = Request.Content.ReadAsStringAsync().Result;
            var domain = string.Empty;

            if (Request.Headers.Contains("Origin"))
            {
                domain = Request.Headers.GetValues("Origin").FirstOrDefault();
            }

            service.CreateSubTasks(issueId, displayIssueId, domain, jsonContent);
        }
    }
}
