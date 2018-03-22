using System.Collections.Generic;
using DevStats.Domain.Sprints;

namespace DevStats.Models.Jira
{
    public class SprintStoriesModel
    {
        public int BoardId { get; set; }

        public int SprintId { get; set; }

        public List<SprintStory> Stories { get; set; }
    }
}