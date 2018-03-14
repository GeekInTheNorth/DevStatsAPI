﻿using System;

namespace DevStats.Data.Entities
{
    public class ApiLog
    {
        public int ID { get; set; }

        public string ApiName { get; set; }

        public string ApiUrl { get; set; }

        public string IssueKey { get; set; }

        public DateTime Triggered { get; set; }

        public bool Success { get; set; }

        public string Action { get; set; }

        public string Content { get; set; }
    }
}