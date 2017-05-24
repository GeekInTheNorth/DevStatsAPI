using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using DevStats.Domain.Jira.WebHookLog;

namespace DevStats.Controllers.API
{
    public class JiraCreateSubtasksController : ApiController
    {
        private readonly IJiraLogRepository hookLogRepository;

        public JiraCreateSubtasksController(IJiraLogRepository hookLogRepository)
        {
            this.hookLogRepository = hookLogRepository;
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

            hookLogRepository.Save(issueId, displayIssueId, domain, jsonContent);
        }
    }
}
