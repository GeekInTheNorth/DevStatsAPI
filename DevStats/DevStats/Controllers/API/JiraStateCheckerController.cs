using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DevStats.Domain.Jira;
using DevStats.Filters;

namespace DevStats.Controllers.API
{
    [IPAccess]
    public class JiraStateCheckerController : ApiController
    {
        private readonly IJiraService service;

        public JiraStateCheckerController(IJiraService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.service = service;
        }

        [HttpGet]
        [ResponseType(typeof(JiraStateSummary))]
        public HttpResponseMessage Get([FromUri]string id)
        {
            try
            {
                var jiraItem = service.GetStateSummaries(id.ToUpper()).FirstOrDefault();

                return Request.CreateResponse(HttpStatusCode.OK, jiraItem);
            }
            catch(Exception)
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
        }
    }
}