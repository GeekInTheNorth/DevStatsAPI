using System;

namespace DevStats.Data
{
    public class BurndownDay
    {
        public int ID { get; set; }

        public string Sprint { get; set; }

        public DateTime Date { get; set; }

        public int WorkRemaining { get; set; }
    }
}