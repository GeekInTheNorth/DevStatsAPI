using System;
using System.Collections.Generic;
using System.Web.Http;
using DevStats.Domain.Reports.TaskingStatus;

namespace DevStats.Controllers.API
{
    [RoutePrefix("api/taskingstatus")]
    public class TaskingStatusController : ApiController
    {
        private readonly ITaskingStatusService service;

        public TaskingStatusController(ITaskingStatusService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.service = service;
        }

        [HttpGet]
        [Route("{team}")]
        public Dictionary<string, decimal> GetTaskingStatus([FromUri]string team)
        {
            return service.GetTaskingStatus(team);
        }
    }
}
