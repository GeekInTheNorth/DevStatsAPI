using System;

namespace DevStats.Domain.Burndown
{
    public class BurndownDay
    {
        public string Sprint { get; set; }

        public DateTime Date { get; set; }

        public int WorkRemaining { get; set; }
    }
}
