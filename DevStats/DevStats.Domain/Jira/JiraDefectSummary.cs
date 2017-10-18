using System;
using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.DefectAnalysis;
using DevStats.Domain.Jira.JsonModels;

namespace DevStats.Domain.Jira
{
    public class JiraDefectSummary
    {
        public string Id { get; set; }

        public DefectCategory Category { get; set; }

        public DefectType Type { get; set; }

        public JiraDefectSummary(Issue issue, IEnumerable<string> supportUsers)
        {
            var hrModuleName = issue.Fields.HRModule == null ? string.Empty : issue.Fields.HRModule.Value;
            var payrollModuleName = issue.Fields.PayrollModule == null ? string.Empty : issue.Fields.PayrollModule.Value;
            var projectName = issue.Fields.Project == null ? string.Empty : issue.Fields.Project.Name;

            Id = issue.Key;
            Type = DefectType.Internal;

            if (supportUsers.Contains(issue.Fields.Creator.Name, StringComparer.CurrentCultureIgnoreCase))
                Type = DefectType.External;

            if (hrModuleName.Equals("workflow", StringComparison.CurrentCultureIgnoreCase))
                Category = DefectCategory.Workflow;
            else if (hrModuleName.Equals("Recruitment", StringComparison.CurrentCultureIgnoreCase))
                Category = DefectCategory.InternalRecruitment;
            else if (hrModuleName.Equals("Recruitment+", StringComparison.CurrentCultureIgnoreCase))
                Category = DefectCategory.RecruitmentPlus;
            else if (hrModuleName.Equals("Online Recruitment", StringComparison.CurrentCultureIgnoreCase))
                Category = DefectCategory.OnlineRecruitment;
            else if (hrModuleName.Equals("Auto Enrolment", StringComparison.CurrentCultureIgnoreCase))
                Category = DefectCategory.AutoEnrolment;
            else if (hrModuleName.Equals("Reports & Analysis", StringComparison.CurrentCultureIgnoreCase))
                Category = DefectCategory.QueryBuilder;
            else if (hrModuleName.Equals("Query Builder", StringComparison.CurrentCultureIgnoreCase))
                Category = DefectCategory.QueryBuilder;
            else if (payrollModuleName.Equals("Legislation", StringComparison.CurrentCultureIgnoreCase))
                Category = DefectCategory.Legislation;
            else if (projectName.Contains("Octopus"))
                Category = DefectCategory.Octopus;
            else if (projectName.Contains("Payroll"))
                Category = DefectCategory.Payroll;
            else if (projectName.Contains("HR"))
                Category = DefectCategory.HR;
        }
    }

    public enum DefectCategory
    {
        Unknown,
        HR,
        Workflow,
        InternalRecruitment,
        RecruitmentPlus,
        OnlineRecruitment,
        AutoEnrolment,
        QueryBuilder,
        Payroll,
        Legislation,
        Octopus
    }
}