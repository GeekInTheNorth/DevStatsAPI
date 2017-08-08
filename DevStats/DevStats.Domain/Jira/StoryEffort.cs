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

        public int LooseEstimate { get; private set; }

        public int Estimate { get; private set; }

        public int ActualTime { get; private set; }

        public List<TaskEffort> Tasks { get; private set; }

        public StoryEffort(Issue story, IEnumerable<Issue> tasks, IEnumerable<WorkLog> logs)
        {
            Key = story.Key;
            Description = story.Fields.Summary ?? string.Empty;
            TShirtSize = GetValue(story.Fields.TShirtSize);
            Complexity = GetValue(story.Fields.Complexity);
            LooseEstimate = story.Fields.StoryPoints.HasValue ? (int)Math.Truncate(story.Fields.StoryPoints.Value) : 0;

            Tasks = tasks.Select(x => new TaskEffort(x, logs)).ToList();

            Estimate = Tasks.Sum(x => x.Estimate);
            ActualTime = Tasks.Sum(x => x.ActualTime);
        }

        private string GetValue(ComplexField field)
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

        public List<TaskEffortLog> Logs { get; private set; }

        public TaskEffort(Issue task, IEnumerable<WorkLog> logs)
        {
            Key = task.Key;
            Description = task.Fields.Summary ?? string.Empty;
            Complexity = GetValue(task.Fields.Complexity);
            Estimate = task.Fields.Timeoriginalestimate ?? 0;
            Owner = task.Fields.Assignee == null ? string.Empty : task.Fields.Assignee.Name;
            Activity = GetValue(task.Fields.TaskType);

            Logs = logs.Where(x => x.Issue.Key == task.Key).Select(x => new TaskEffortLog(x)).ToList();
            ActualTime = Logs.Sum(x => x.Duration);
        }

        private string GetValue(ComplexField field)
        {
            if (field == null)
                return string.Empty;

            return field.Value ?? string.Empty;
        }
    }

    public class TaskEffortLog
    {
        public string Worker { get; set; }

        public int Duration { get; set; }

        public TaskEffortLog(WorkLog log)
        {
            Worker = log.Worker;
            Duration = log.TimeSpentSeconds;
        }
    }
}