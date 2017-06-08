using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.DefectAnalysis;
using DevStats.Domain.Jira;

namespace DevStats.Models.Jira
{
    public class DefectAnalysisModel
    {
        public Dictionary<string, IEnumerable<DefectAnalysisModelItem>> Reports { get; set; }

        public IEnumerable<DefectAnalysisModelInlineItem> InlineReport { get; set; }

        public DefectAnalysisModel(IEnumerable<JiraDefect> defects)
        {
            CreateReports(defects);
            CreateInlineReport(defects);
        }

        private void CreateInlineReport(IEnumerable<JiraDefect> defects)
        {
            InlineReport = (from defect in defects
                            group defect by defect.Category into defectGrp
                            orderby defectGrp.Key
                            select new DefectAnalysisModelInlineItem
                            {
                                Category = defectGrp.Key,
                                AllReported = defectGrp.Count(),
                                InternallyReported = defectGrp.Count(x => x.Type == DefectType.Internal),
                                ExternallyReported = defectGrp.Count(x => x.Type == DefectType.External)
                            });
        }

        private void CreateReports(IEnumerable<JiraDefect> defects)
        {
            var allItems = (from defect in defects
                            group defect by defect.Category into defectGrp
                            orderby defectGrp.Key
                            select new DefectAnalysisModelItem
                            {
                                Category = defectGrp.Key,
                                Items = defectGrp.Count()
                            });

            var internalItems = (from defect in defects
                                 where defect.Type == DefectType.Internal
                                 group defect by defect.Category into defectGrp
                                 orderby defectGrp.Key
                                 select new DefectAnalysisModelItem
                                 {
                                     Category = defectGrp.Key,
                                     Items = defectGrp.Count()
                                 });

            var externalItems = (from defect in defects
                                 where defect.Type == DefectType.External
                                 group defect by defect.Category into defectGrp
                                 orderby defectGrp.Key
                                 select new DefectAnalysisModelItem
                                 {
                                     Category = defectGrp.Key,
                                     Items = defectGrp.Count()
                                 });

            Reports = new Dictionary<string, IEnumerable<DefectAnalysisModelItem>>();
            Reports.Add("All Defects", allItems);
            Reports.Add("Internally Reported", internalItems);
            Reports.Add("Externally Reported", externalItems);
        }
    }

    public class DefectAnalysisModelItem
    {
        public DefectCategory Category { get; set; }

        public string DisplayCategory
        {
            get
            {
                switch (Category)
                {
                    case DefectCategory.InternalRecruitment:
                        return "Recruitment";
                    case DefectCategory.RecruitmentPlus:
                        return "Recruitment+";
                    case DefectCategory.OnlineRecruitment:
                        return "Online Recruitment";
                    case DefectCategory.AutoEnrolment:
                        return "Auto Enrolment";
                    case DefectCategory.QueryBuilder:
                        return "Query Builder";
                    default:
                        return Category.ToString();
                }
            }
        }

        public int Items { get; set; }
    }

    public class DefectAnalysisModelInlineItem
    {
        public DefectCategory Category { get; set; }

        public string DisplayCategory
        {
            get
            {
                switch (Category)
                {
                    case DefectCategory.InternalRecruitment:
                        return "Recruitment";
                    case DefectCategory.RecruitmentPlus:
                        return "Recruitment+";
                    case DefectCategory.OnlineRecruitment:
                        return "Online Recruitment";
                    case DefectCategory.AutoEnrolment:
                        return "Auto Enrolment";
                    case DefectCategory.QueryBuilder:
                        return "Query Builder";
                    default:
                        return Category.ToString();
                }
            }
        }

        public int InternallyReported { get; set; }

        public int ExternallyReported { get; set; }

        public int AllReported { get; set; }
    }
}