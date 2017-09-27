using System;
using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.Jira.JsonModels;

namespace DevStats.Domain.Jira
{
    public class StoryEffort
    {
        public string Key { get; private set; }

        public string Description { get; private set; }

        public string TShirtSize { get; private set; }

        public string Complexity { get; private set; }

        public string Release { get; private set; }

        public int LooseEstimate { get; private set; }

        public int Estimate { get; private set; }

        public int ActualTime { get; private set; }

        public List<TaskEffort> Tasks { get; private set; }

        public DateTime? LastWorkedOn { get; private set; }

        public StoryEffort(Issue story, IEnumerable<Issue> tasks)
        {
            Key = story.Key;
            Description = story.Fields.Summary ?? string.Empty;
            TShirtSize = GetValue(story.Fields.TShirtSize);
            Complexity = GetValue(story.Fields.Complexity);
            LooseEstimate = story.Fields.StoryPoints.HasValue ? (int)Math.Truncate(story.Fields.StoryPoints.Value) : 0;

            Tasks = tasks.Select(x => new TaskEffort(x)).ToList();

            if (story.Fields.FixVersions != null && story.Fields.FixVersions.Any())
            {
                Release = story.Fields.FixVersions.OrderBy(x => x.ReleaseDate ?? DateTime.MinValue).Last().Name;
            }

            if (Tasks.Any())
            {
                Estimate = Tasks.Sum(x => x.Estimate);
                ActualTime = Tasks.Sum(x => x.ActualTime);
            }

            if (Tasks.Any(x => x.LastWorkedOn.HasValue))
            {
                LastWorkedOn = Tasks.Where(x => x.LastWorkedOn.HasValue).Max(x => x.LastWorkedOn);
            }
        }

        private string GetValue(ValueField field)
        {
            if (field == null)
                return string.Empty;

            return field.Value ?? string.Empty;
        }
    }

    public class TaskEffort
    {
        public string Key { get; private set; }

        public string Description { get; private set; }

        public string Owner { get; private set; }

        public string Activity { get; set; }

        public string Complexity { get; private set; }

        public int Estimate { get; private set; }

        public int ActualTime { get; private set; }

        public DateTime? LastWorkedOn { get; private set; }

        public TaskEffort(Issue task)
        {
            Key = task.Key;
            Description = task.Fields.Summary ?? string.Empty;
            Complexity = GetValue(task.Fields.Complexity);
            Estimate = task.Fields.Timeoriginalestimate ?? 0;
            Owner = task.Fields.Assignee == null ? string.Empty : task.Fields.Assignee.Name;
            Activity = GetValue(task.Fields.TaskType);
            LastWorkedOn = task.Fields.Resolutiondate;

            if (task.Fields.TimeTracking != null)
            {
                Estimate = task.Fields.TimeTracking.EstimateInSeconds;
                ActualTime = task.Fields.TimeTracking.TimeSpentInSeconds;
            }
        }

        private string GetValue(ValueField field)
        {
            if (field == null)
                return string.Empty;

            return field.Value ?? string.Empty;
        }
    }
}