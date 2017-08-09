using System;
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
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.service = service;
        }

        [EnableCors("*", "*", "*")]
        [AcceptVerbs("GET", "POST")]
        public void CreateSubtasks([FromUri]string issueId, [FromUri]string displayIssueId)
        {
            service.CreateSubTasks(issueId, displayIssueId);
        }
    }
}