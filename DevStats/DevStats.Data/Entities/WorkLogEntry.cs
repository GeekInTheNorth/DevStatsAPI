using System;

namespace DevStats.Data.Entities
{
    public class WorkLogEntry
    {
        public int ID { get; set; }

        public int WorkLogTaskID { get; set; }

        public string Worker { get; set; }

        public int EffortInSeconds { get; set; }

        public string Description { get; set; }

        public DateTime Logged { get; set; }

        public virtual WorkLogTask WorkLogTask { get; set; }
    }
}