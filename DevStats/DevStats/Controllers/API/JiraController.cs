using System;
using System.Net;
using System.Net.Http;
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
        public HttpResponseMessage StoryCreate(string jiraId)
        {
            try
            {
                jiraService.ProcessStoryCreate(jiraId);

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

        [AcceptVerbs("DELETE", "POST")]
        [Route("story/Delete/{jiraId}")]
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

        [AcceptVerbs("POST")]
        [Route("subtask/update/{jiraId}")]
        public HttpResponseMessage SubTaskUpdate(string jiraId)
        {
            try
            {
                jiraService.ProcessSubtaskUpdate(jiraId);

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

        [AcceptVerbs("POST")]
        [Route("bug/update/{jiraId}")]
        public HttpResponseMessage BugUpdate(string jiraId)
        {
            try
            {
                jiraService.ProcessBugUpdate(jiraId);

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