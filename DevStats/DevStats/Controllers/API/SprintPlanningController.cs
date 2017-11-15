using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DevStats.Domain.Sprints;
using DevStats.Models.Jira;

namespace DevStats.Controllers.API
{
    public class SprintPlanningController : ApiController
    {
        private readonly ISprintPlannerService service;

        public SprintPlanningController(ISprintPlannerService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.service = service;
        }

        [HttpGet]
        [Route("api/sprintplanning/sprintlist")]
        public IEnumerable<SprintInformation> GetSprints()
        {
            return service.GetSprints();
        }

        [HttpGet]
        [Route("api/sprintplanning/sprintstories/{boardId}/{sprintId}")]
        public IEnumerable<SprintStory> GetStoriesInSprint(int boardId, int sprintId)
        {
            return service.GetSprintItems(boardId, sprintId);
        }

        [HttpGet]
        [Route("api/sprintplanning/refinedstories/{team}/{sprintBeingPlanned}")]
        public IEnumerable<SprintStory> GetRefinedStories(string team, int sprintBeingPlanned)
        {
            return service.GetRefinedItems(team, sprintBeingPlanned);
        }

        [HttpPost]
        [Route("api/sprintplanning/sprintstories/{boardId}/{sprintId}")]
        public HttpResponseMessage SetStoriesInSprint([FromUri]int boardId, [FromUri]int sprintId, [FromBody]CommitSprintModel sprintModel)
        {
            if (sprintModel == null || !sprintModel.IsValid())
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            try
            {
                service.SetSprintContents(boardId, sprintId, sprintModel.Keys);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}