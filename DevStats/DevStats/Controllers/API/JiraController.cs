using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using DevStats.Domain.Jira;

namespace DevStats.Controllers.API
{
    [RoutePrefix("api/jira")]
    public class JiraController : ApiController
    {
        private readonly IJiraService jiraService;

        public JiraController(IJiraService jiraService, IJiraIdValidator idValidator)
        {
            if (jiraService == null) throw new ArgumentNullException(nameof(jiraService));

            this.jiraService = jiraService;
        }

        [EnableCors("*", "*", "*")]
        [AcceptVerbs("POST")]
        [Route("story/update/{jiraId}")]
        public HttpResponseMessage StoryUpdate(string jiraId)
        {
            try
            {
                jiraService.ProcessStoryUpdate(jiraId);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ArgumentException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [EnableCors("*", "*", "*")]
        [AcceptVerbs("DELETE", "POST")]
        [Route("Delete/{jiraId}")]
        public HttpResponseMessage Delete(string jiraId)
        {
            try
            {
                jiraService.Delete(jiraId);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ArgumentException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}