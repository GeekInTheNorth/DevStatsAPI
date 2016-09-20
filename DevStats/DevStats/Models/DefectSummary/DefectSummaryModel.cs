using System.Collections.Generic;

namespace DevStats.Models.DefectSummary
{
    public class DefectSummaryModel
    {
        public List<DefectSummaryModelItem> Summaries { get; set; }

        public DefectSummaryModel(Domain.DefectAnalysis.DefectSummaries summaries)
        {
            Summaries = new List<DefectSummaryModelItem>
            {
                new DefectSummaryModelItem("Total Outstanding Defects", summaries.TotalOutstandingDefects),
                new DefectSummaryModelItem("Internal Outstanding Defects", summaries.InternalOutstandingDefects),
                new DefectSummaryModelItem("External Outstanding Defects", summaries.ExternalOutstandingDefects),
                new DefectSummaryModelItem("Total Items Logged", summaries.TotalItemsLogged),
                new DefectSummaryModelItem("Internal Items Logged", summaries.InternalItemsLogged),
                new DefectSummaryModelItem("External Items Logged", summaries.ExternalItemsLogged),
                new DefectSummaryModelItem("Rework Items Logged", summaries.ReworkItemsLogged)
            };
        }
    }

    public class DefectSummaryModelItem
    {
        public string SummaryTitle { get; private set; }

        public List<Domain.DefectAnalysis.DefectSummary> Summaries { get; private set; }

        public DefectSummaryModelItem(string summaryTitle, List<Domain.DefectAnalysis.DefectSummary> summaries)
        {
            SummaryTitle = summaryTitle;
            Summaries = summaries;
        }
    }
}
