using System;
using System.Text.RegularExpressions;
using DevStats.Domain.DefectAnalysis;
using DevStats.Domain.Jira.JsonModels;

namespace DevStats.Domain.Jira
{
    public class JiraDefect
    {
        public string JiraId { get; set; }

        public string AhaId { get; set; }

        public string Product { get; set; }

        public string Module { get; set; }

        public DefectType Type { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Closed { get; set; }

        public JiraDefect(Issue issue)
        {
            JiraId = issue.Key;

            var ahaId = issue.Fields.AhaReference ?? string.Empty;
            var regEx = new Regex(@"([A-Z]{2,3}[\-]{1}[0-9]{1,5})");

            if (regEx.IsMatch(issue.Fields.AhaReference))
                ahaId = regEx.Match(ahaId).Value;

            if (issue.Key.StartsWith("CPR"))
            {
                Product = "Payroll";
                Module = issue.Fields.PayrollModule != null ? issue.Fields.PayrollModule.Value : "Unknown";
            }
            else if (issue.Key.StartsWith("CHR"))
            {
                Product = "Cascade";
                Module = issue.Fields.HRModule != null ? issue.Fields.HRModule.Value : "Unknown";
            }
            else if (issue.Key.StartsWith("OCT"))
            {
                Product = "Octopus";
                Module = issue.Fields.OctopusModule != null ? issue.Fields.OctopusModule.Value : "Unknown";
            }

            if (issue.Fields.Issuetype.Name.Equals("Bug", StringComparison.CurrentCultureIgnoreCase))
            {
                var category = issue.Fields.Category == null ? string.Empty : issue.Fields.Category.ToString();

                if (category.Contains("Retention") && category.Contains("External"))
                    Type = DefectType.External;
                if (category.Contains("Retention") && category.Contains("Internal"))
                    Type = DefectType.Internal;
            }
            else
                Type = DefectType.NonDefect;

            Created = issue.Fields.Created;
            Closed = issue.Fields.Resolutiondate;
        }
    }
}