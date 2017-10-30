﻿using System.Collections.Generic;
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
                new SelectListItem { Value = "/api/JiraCreateSubtasks?issueId=@@id@@&displayIssueId=@@id@@", Text = "Story or Bug Created (Create Subtasks)" },
                new SelectListItem { Value = "/api/JiraStoryUpdated?issueId=@@id@@&displayIssueId=@@id@@", Text = "Story or Bug Updated" },
                new SelectListItem { Value = "/api/Jira/Delete/@@id@@", Text = "Story or Bug Deleted" },
                new SelectListItem { Value = "/api/JiraSubtaskUpdated?issueId=@@id@@&displayIssueId=@@id@@", Text = "Subtask Updated" }
            }; 
        }
    }
}