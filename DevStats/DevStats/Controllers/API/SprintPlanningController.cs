using System;
using System.Collections.Generic;
using System.Linq;
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
        public SprintStoriesModel GetStoriesInSprint(int boardId, int sprintId)
        {
            return new SprintStoriesModel
            {
                BoardId = boardId,
                SprintId = sprintId,
                Stories = service.GetSprintItems(boardId, sprintId).ToList()
            };
        }

        [HttpGet]
        [Route("api/sprintplanning/refinedstories/{team}/{sprintBeingPlanned}")]
        public RefinedStoriesModel GetRefinedStories(string team, int sprintBeingPlanned)
        {
            return new RefinedStoriesModel
            {
                SprintBeingPlanned = sprintBeingPlanned,
                Team = team,
                Stories = service.GetRefinedItems(team, sprintBeingPlanned).ToList()
            };
        }

        [HttpPost]
        [Route("api/sprintplanning/sprintstories/{boardId}/{sprintId}")]
        public HttpResponseMessage SetStoriesInSprint([FromUri]int boardId, [FromUri]int sprintId, [FromBody]CommitSprintModel sprintModel)
        {
            try
            {
                service.SetSprintContents(boardId, sprintId, sprintModel.TeamName, sprintModel.Keys);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}