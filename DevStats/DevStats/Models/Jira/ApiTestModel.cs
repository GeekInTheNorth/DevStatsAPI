using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DevStats.Domain.Messages;

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
                new SelectListItem { Value = "/api/jira/story/create/@@id@@", Text = "Story or Bug Created (Create Subtasks)" },
                new SelectListItem { Value = "/api/jira/story/update/@@id@@", Text = "Story or Bug Updated (Update Defect Stats, KPIs, Version and Team)" },
                new SelectListItem { Value = "/api/Jira/story/delete/@@id@@", Text = "Story or Bug Deleted (Update Defect Stats)" },
                new SelectListItem { Value = "/api/jira/iam/update/@@id@@", Text = "Update IAM Model" },
                new SelectListItem { Value = "/api/Jira/subtask/update/@@id@@", Text = "Subtask Updated (Update Parent Status)" },
                new SelectListItem { Value = string.Format("/api/aha/GetDefectUpdates/{0:yyyy-MM-dd}", DateTime.Today.AddMonths(-2)), Text = "Get Defect Updates from Aha" }
            }; 
        }
    }
}