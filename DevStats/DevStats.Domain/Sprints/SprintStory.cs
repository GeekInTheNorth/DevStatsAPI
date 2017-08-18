using System;
using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.Jira.JsonModels;

namespace DevStats.Domain.Sprints
{
    public class SprintStory
    {
        public string Key { get; private set; }

        public string Url { get; private set; }

        public string Description { get; private set; }

        public string Type { get; private set; }

        public string Refinement { get; private set; }

        public string State { get; private set; }

        public int DevelopmentRemainingInSeconds { get; private set; }

        public decimal DevelopmentRemainingInHours
        {
            get { return SecondsToHours(DevelopmentRemainingInSeconds); }
        }

        public int TestingRemainingInSeconds { get; private set; }

        public decimal TestingRemainingInHours
        {
            get { return SecondsToHours(TestingRemainingInSeconds); }
        }

        public SprintStory(Issue story, IEnumerable<Issue> tasks, string urlRoot)
        {
            var tasksForStory = tasks.Where(x => x.Fields.Parent != null && x.Fields.Parent.Key == story.Key)
                                     .Where(x => x.Fields.TimeTracking != null);

            var testingTaskTypes = new List<string> { "Test", "Automation" };


            Key = story.Key;
            Url = string.Format("{0}/browse/{1}", urlRoot, story.Key);
            Description = story.Fields.Summary;
            Type = story.Fields.Issuetype.Name;
            State = story.Fields.Status.Name;
            Refinement = story.Fields.Refinement.Value;
            DevelopmentRemainingInSeconds = tasksForStory.Where(x => !testingTaskTypes.Contains(x.Fields.TaskType.Value))
                                                         .Sum(x => x.Fields.TimeTracking.TimeRemainingInSeconds);
            TestingRemainingInSeconds = tasksForStory.Where(x => testingTaskTypes.Contains(x.Fields.TaskType.Value))
                                                     .Sum(x => x.Fields.TimeTracking.TimeRemainingInSeconds);
        }

        private decimal SecondsToHours(decimal seconds)
        {
            var hours = seconds / (60.0M * 60.0M);
            return Math.Round(hours, 2);
        }
    }
}