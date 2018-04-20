using System;

namespace DevStats.Domain.Logging
{
    public class ApiLog
    {
        public string Action { get; set; }

        public string ApiName { get; set; }

        public string ApiUrl { get; set; }

        public string Content { get; set; }

        public bool ContentIsJson
        {
            get { return Content != null && Content.StartsWith("{"); }
        }

        public string IssueKey { get; set; }

        public bool Success { get; set; }

        public string SuccessAsString
        {
            get { return Success ? "Yes" : "No"; }
        }

        public DateTime Triggered { get; set; }

        public ApiLog(string apiName, string apiUrl, string action, string content, string issueKey, bool success, DateTime triggered)
        {
            ApiName = apiName;
            ApiUrl = apiUrl;
            IssueKey = issueKey;
            Triggered = triggered;
            Success = success;
            Action = action;
            Content = content;
        }
    }
}