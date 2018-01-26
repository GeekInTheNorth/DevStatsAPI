using System;
using System.Web.Http;
using System.Web.Http.Cors;
using DevStats.Domain.Jira;

namespace DevStats.Controllers.API
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/jira")]
    public class JiraController : ApiController
    {
        private readonly IJiraService jiraService;

        public JiraController(IJiraService jiraService, IJiraIdValidator idValidator)
        {
            if (jiraService == null) throw new ArgumentNullException(nameof(jiraService));

            this.jiraService = jiraService;
        }

        [AcceptVerbs("POST")]
        [Route("story/create/{jiraId}")]
        public void StoryCreate(string jiraId)
        {
            jiraService.ProcessStoryCreate(jiraId);
        }

        [AcceptVerbs("POST")]
        [Route("story/update/{jiraId}")]
        public void StoryUpdate(string jiraId)
        {
            jiraService.ProcessStoryUpdate(jiraId);
        }

        [AcceptVerbs("DELETE", "POST")]
        [Route("story/Delete/{jiraId}")]
        public void Delete(string jiraId)
        {
            jiraService.Delete(jiraId);
        }

        [AcceptVerbs("POST")]
        [Route("subtask/update/{jiraId}")]
        public void SubTaskUpdate(string jiraId)
        {
            jiraService.ProcessSubtaskUpdate(jiraId);
        }

        [AcceptVerbs("POST")]
        [Route("iam/update/{jiraId}")]
        public void ImpactAnalysisModelUpdate(string jiraId)
        {
            jiraService.ProcessImpactAnalysisModelUpdate(jiraId);
        }
    }
}