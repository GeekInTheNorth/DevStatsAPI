using System;
using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.KPI;

namespace DevStats.Data.Repositories
{
    public class NewFeaturePassRateRepository : BaseRepository, INewFeaturePassRateRepository
    {
        public IEnumerable<string> GetDevelopers()
        {
            return Context.WorkLogTasks
                          .Where(x => x.Activity == "Dev")
                          .Select(x => x.Owner)
                          .Distinct()
                          .ToList()
                          .Where(x => !string.IsNullOrWhiteSpace(x));
        }

        public IEnumerable<NewFeaturePassRateStory> GetQualityApi(string developer)
        {
            var contributedStories = (from task in Context.WorkLogTasks
                                      where task.Owner == developer
                                      select task.WorkLogStoryID).Distinct();

            var yearAgo = DateTime.Now.AddYears(-1);

            var stories = (from story in Context.WorkLogStories
                           join tasks in Context.WorkLogTasks on story.ID equals tasks.WorkLogStoryID into taskGrp
                           where contributedStories.Contains(story.ID)
                           && story.LastWorkedOn.HasValue
                           && story.LastWorkedOn.Value >= yearAgo
                           select new
                           {
                               Story = story,
                               Tasks = taskGrp.DefaultIfEmpty()
                           }).ToList();

            return stories.Select(x => new NewFeaturePassRateStory(x.Story.StoryKey, x.Story.Description, x.Story.DeliveredInRelease, x.Story.LastWorkedOn, x.Tasks.Select(y => new NewFeaturePassRateTask(y.Owner, y.Activity, y.ActualTimeInSeconds)), developer));
        }
    }
}