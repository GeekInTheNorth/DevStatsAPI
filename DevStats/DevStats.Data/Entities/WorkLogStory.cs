using System;
using System.Collections.Generic;

namespace DevStats.Data.Entities
{
    public class WorkLogStory
    {
        public int ID { get; set; }

        public string StoryKey { get; set; }

        public string Description { get; set; }

        public string TShirtSize { get; set; }

        public string Complexity { get; set; }

        public string DeliveredInRelease { get; set; }

        public int LooseEstimateInHours { get; set; }

        public int EstimateInSeconds { get; set; }

        public int ActualTimeInSeconds { get; set; }

        public DateTime? LastWorkedOn { get; set; }

        public virtual ICollection<WorkLogTask> WorkLogTasks { get; set; }
    }
}