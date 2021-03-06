﻿using System.Collections.Generic;

namespace DevStats.Domain.Sprints
{
    public interface ISprintPlannerService
    {
        IEnumerable<SprintInformation> GetSprints();

        IEnumerable<SprintStory> GetSprintItems(int boardId, int sprintId);

        IEnumerable<SprintStory> GetRefinedItems(string owningTeam, int currentSprint);

        void SetSprintContents(int boardId, int sprintId, string teamName, IEnumerable<string> storyKeys);
    }
}