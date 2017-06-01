using System;
using System.Web.Http;
using DevStats.Domain.Jira.Transitions;
using DevStats.Filters;

namespace DevStats.Controllers.API
{
    public class JiraTransitionController : ApiController
    {
        private readonly IJiraTransitionService service;

        public JiraTransitionController(IJiraTransitionService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.service = service;
        }

        [HttpGet]
        [IPAccess]
        public string ForceUpdateOfTransitions([FromUri]string project = null, [FromUri]bool refreshAll = false)
        {
            if (string.IsNullOrWhiteSpace(project))
                service.UpdateTransitions(refreshAll);
            else
                service.UpdateTransitions(project, refreshAll);

            return "Finished";
        }
    }
}