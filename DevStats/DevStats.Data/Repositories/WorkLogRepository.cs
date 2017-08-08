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

            var workLogStory = new WorkLogStory
            {
                ActualTimeInSeconds = storyEffort.ActualTime,
                Complexity = storyEffort.Complexity,
                EstimateInSeconds = storyEffort.Estimate,
                LooseEstimateInHours = storyEffort.LooseEstimate,
                StoryKey = storyEffort.Key,
                TShirtSize = storyEffort.TShirtSize,
                WorkLogTasks = storyEffort.Tasks.Select(x => new WorkLogTask
                {
                    Activity = x.Activity,
                    ActualTimeInSeconds = x.ActualTime,
                    Complexity = x.Complexity,
                    EstimateInSeconds = x.Estimate,
                    Owner = x.Owner,
                    TaskKey = x.Key,
                    WorkLogEntries = x.Logs.Select(y => new WorkLogEntry
                    {
                        EffortInSeconds = y.Duration,
                        Worker = y.Worker
                    }).ToList()
                }).ToList()
            };

            Context.WorkLogStories.Add(workLogStory);
            Context.SaveChanges();
        }
    }
}