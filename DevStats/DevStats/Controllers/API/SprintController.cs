using System.Web.Http;
using DevStats.Domain.Sprints;

namespace DevStats.Controllers.API
{
    public class SprintController : ApiController
    {
        private readonly ISprintService service;

        public SprintController(ISprintService service)
        {
            this.service = service;
        }

        [HttpGet]
        public Sprint Get([FromUri]string pod, [FromUri]string sprint)
        {
            if (string.IsNullOrWhiteSpace(pod) || string.IsNullOrWhiteSpace(sprint)) return null;

            return service.GetSprint(pod, sprint);
        }

        [HttpGet]
        public Sprint Get([FromUri]string pod)
        {
            if (string.IsNullOrWhiteSpace(pod)) return null;

            return service.GetSprint(pod);
        }
    }
}