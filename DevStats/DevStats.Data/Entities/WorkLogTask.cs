using System;
using System.Collections.Generic;

namespace DevStats.Data.Entities
{
    public class WorkLogTask
    {
        public int ID { get; set; }

        public int WorkLogStoryID { get; set; }

        public string TaskKey { get; set; }

        public string Owner { get; set; }

        public string Description { get; set; }

        public string Activity { get; set; }

        public string Complexity { get; set; }

        public int EstimateInSeconds { get; set; }

        public int ActualTimeInSeconds { get; set; }

        public DateTime? LastWorkedOn { get; set; }

        public virtual WorkLogStory WorkLogStory { get; set; }

        public virtual ICollection<WorkLogEntry> WorkLogEntries { get; set; }
    }
}