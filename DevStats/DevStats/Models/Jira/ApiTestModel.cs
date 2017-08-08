using System.Collections.Generic;
using System.Web.Mvc;
using DevStats.Domain.Jira;

namespace DevStats.Models.Jira
{
    public class ApiTestModel
    {
        public List<SelectListItem> ApiRoots { get; set; }

        public PostResult PostResult { get; set; }

        public ApiTestModel()
        {
            ApiRoots = new List<SelectListItem>
            {
                new SelectListItem { Value = "/api/JiraCreateSubtasks?issueId=@@id@@&displayIssueId=@@id@@", Text = "Story Created (Create Subtasks)" },
                new SelectListItem { Value = "/api/JiraStoryUpdated?issueId=@@id@@&displayIssueId=@@id@@", Text = "Story Updated" },
                new SelectListItem { Value = "/api/JiraStoryCompleted?issueId=@@id@@&displayIssueId=@@id@@", Text = "Story Completed" },
                new SelectListItem { Value = "/api/JiraSubtaskUpdated?issueId=@@id@@&displayIssueId=@@id@@", Text = "Subtask updated" }
            }; 
        }
    }
}