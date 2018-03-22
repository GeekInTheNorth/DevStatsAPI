using System.Collections.Generic;
using DevStats.Domain.Sprints;

namespace DevStats.Models.Jira
{
    public class RefinedStoriesModel
    {
        public string Team { get; set; }

        public int SprintBeingPlanned { get; set; }

        public List<SprintStory> Stories { get; set; }
    }
}