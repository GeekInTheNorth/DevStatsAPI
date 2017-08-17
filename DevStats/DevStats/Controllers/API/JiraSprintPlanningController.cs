using System;
using System.Collections.Generic;
using System.Web.Http;
using DevStats.Domain.Sprints;

namespace DevStats.Controllers.API
{
    public class JiraSprintPlanningController : ApiController
    {
        private readonly ISprintPlannerService service;

        public JiraSprintPlanningController(ISprintPlannerService service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));

            this.service = service;
        }

        [Route("api/sprintplanning/sprintlist")]
        public IEnumerable<SprintInformation> GetSprints()
        {
            return service.GetSprints();
        }

        [Route("api/sprintplanning/sprintstories/{boardId}/{sprintId}")]
        public IEnumerable<SprintStory> GetStoriesInSprint(int boardId, int sprintId)
        {
            return service.GetSprintItems(boardId, sprintId);
        }

        [Route("api/sprintplanning/refinedstories/{team}/{sprintBeingPlanned}")]
        public IEnumerable<SprintStory> GetRefinedStories(string team, int sprintBeingPlanned)
        {
            return service.GetRefinedItems(team, sprintBeingPlanned);
        }
    }
}