using System;
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
                new SelectListItem { Value = "/api/jira/story/create/@@id@@", Text = "Story or Bug Created" },
                new SelectListItem { Value = "/api/jira/story/update/@@id@@", Text = "Story or Bug Updated" },
                new SelectListItem { Value = "/api/Jira/story/delete/@@id@@", Text = "Story or Bug Deleted" },
                new SelectListItem { Value = "/api/Jira/subtask/update/@@id@@", Text = "Subtask Updated" },
                new SelectListItem { Value = string.Format("/api/aha/GetDefectUpdates/{0:yyyy-MM-dd}", DateTime.Today.AddMonths(-2)), Text = "Get Defect Updates from Aha" }
            }; 
        }
    }
}