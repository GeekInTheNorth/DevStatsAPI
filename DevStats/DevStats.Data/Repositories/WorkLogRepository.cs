using System;
using System.Linq;
using DevStats.Data.Entities;
using DevStats.Domain.Jira;

namespace DevStats.Data.Repositories
{
    public class WorkLogRepository : BaseRepository, IWorkLogRepository
    {
        public void Save(StoryEffort storyEffort)
        {
            if (storyEffort == null) throw new ArgumentNullException(nameof(storyEffort));

            var existingStory = Context.WorkLogStories.FirstOrDefault(x => x.StoryKey == storyEffort.Key);
            if (existingStory != null)
            {
                Context.WorkLogStories.Remove(existingStory);
            }

            var workLogStory = new WorkLogStory
            {
                ActualTimeInSeconds = storyEffort.ActualTime,
                Complexity = storyEffort.Complexity,
                Description = storyEffort.Description,
                EstimateInSeconds = storyEffort.Estimate,
                LastWorkedOn = storyEffort.LastWorkedOn,
                LooseEstimateInHours = storyEffort.LooseEstimate,
                StoryKey = storyEffort.Key,
                TShirtSize = storyEffort.TShirtSize,
                WorkLogTasks = storyEffort.Tasks.Select(x => new WorkLogTask
                {
                    Activity = storyEffort.Key == x.Key ? "Dev" : x.Activity,
                    ActualTimeInSeconds = x.ActualTime,
                    Complexity = x.Complexity,
                    Description = x.Description,
                    EstimateInSeconds = x.Estimate,
                    LastWorkedOn = x.LastWorkedOn,
                    Owner = x.Owner,
                    TaskKey = x.Key
                }).ToList()
            };

            Context.WorkLogStories.Add(workLogStory);
            Context.SaveChanges();
        }
    }
}