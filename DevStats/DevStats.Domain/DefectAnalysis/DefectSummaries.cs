using System;
using System.Collections.Generic;
using System.Linq;

namespace DevStats.Domain.DefectAnalysis
{
    public class DefectSummaries
    {
        private DefectType[] AllDefectTypes = new [] { DefectType.Internal, DefectType.External, DefectType.Rework };

        private DefectType[] InternalAndExternal = new [] { DefectType.Internal, DefectType.External };

        public Dictionary<string, List<DefectSummary>> Summaries
        {
            get
            {
                return new Dictionary<string, List<DefectSummary>>
                {
                    { "Total Outstanding Defects", TotalOutstandingDefects },
                    { "Internal Outstanding Defects", InternalOutstandingDefects },
                    { "External Outstanding Defects", ExternalOutstandingDefects },
                    { "Total Items Logged", TotalItemsLogged },
                    { "Internal Items Logged", InternalItemsLogged },
                    { "External Items Logged", ExternalItemsLogged }
                };
            }
        }

        public List<DefectSummary> TotalOutstandingDefects { get; private set; }

        public List<DefectSummary> InternalOutstandingDefects { get; private set; }

        public List<DefectSummary> ExternalOutstandingDefects { get; private set; }

        public List<DefectSummary> TotalItemsLogged { get; private set; }

        public List<DefectSummary> InternalItemsLogged { get; private set; }

        public List<DefectSummary> ExternalItemsLogged { get; private set; }

        public List<string> Modules { get; private set; }

        public DefectSummaries(IEnumerable<Defect> defects, DateTime firstMonthStart, DateTime lastMonthEnd)
        {
            Modules = defects.Select(x => x.Module).Distinct().OrderBy(x => x).ToList();
            TotalOutstandingDefects = new List<DefectSummary>();
            InternalOutstandingDefects = new List<DefectSummary>();
            ExternalOutstandingDefects = new List<DefectSummary>();
            TotalItemsLogged = new List<DefectSummary>();
            InternalItemsLogged = new List<DefectSummary>();
            ExternalItemsLogged = new List<DefectSummary>();

            foreach(var module in Modules)
            {
                var today = DateTime.Today;
                var currentMonthEnd = firstMonthStart.AddMonths(1).AddDays(-1);
                var totalOutstandingDefects = new DefectSummary(module);
                var internalOutstandingDefects = new DefectSummary(module);
                var externalOutstandingDefects = new DefectSummary(module);
                var totalItemsLogged = new DefectSummary(module);
                var internalItemsLogged = new DefectSummary(module);
                var externalItemsLogged = new DefectSummary(module);

                while (currentMonthEnd <= lastMonthEnd)
                {
                    var monthString = currentMonthEnd.ToString("MMM yy");

                    totalOutstandingDefects.MonthlyBreakdown.Add(monthString, GetOutstandingDefectsForPeriod(defects, module, currentMonthEnd, InternalAndExternal));
                    internalOutstandingDefects.MonthlyBreakdown.Add(monthString, GetOutstandingDefectsForPeriod(defects, module, currentMonthEnd, DefectType.Internal));
                    externalOutstandingDefects.MonthlyBreakdown.Add(monthString, GetOutstandingDefectsForPeriod(defects, module, currentMonthEnd, DefectType.External));
                    totalItemsLogged.MonthlyBreakdown.Add(monthString, GetLoggedDefectsForPeriod(defects, module, currentMonthEnd, AllDefectTypes));
                    internalItemsLogged.MonthlyBreakdown.Add(monthString, GetLoggedDefectsForPeriod(defects, module, currentMonthEnd, DefectType.Internal));
                    externalItemsLogged.MonthlyBreakdown.Add(monthString, GetLoggedDefectsForPeriod(defects, module, currentMonthEnd, DefectType.External));

                    currentMonthEnd = new DateTime(currentMonthEnd.Year, currentMonthEnd.Month, 1).AddMonths(2).AddDays(-1);
                }

                TotalOutstandingDefects.Add(totalOutstandingDefects);
                InternalOutstandingDefects.Add(internalOutstandingDefects);
                ExternalOutstandingDefects.Add(externalOutstandingDefects);
                TotalItemsLogged.Add(totalItemsLogged);
                InternalItemsLogged.Add(internalItemsLogged);
                ExternalItemsLogged.Add(externalItemsLogged);
            }
        }

        private int GetOutstandingDefectsForPeriod(IEnumerable<Defect> defects, string module, DateTime currentMonthEnd, DefectType defectType)
        {
            return defects.Where(x => x.Type == defectType && x.Module == module)
                          .Where(x => x.Created <= currentMonthEnd && !x.WasClosedBy(currentMonthEnd))
                          .Count();
        }

        private int GetOutstandingDefectsForPeriod(IEnumerable<Defect> defects, string module, DateTime currentMonthEnd, DefectType[] defectTypes)
        {
            return defects.Where(x => defectTypes.Contains(x.Type) && x.Module == module)
                          .Where(x => x.Created <= currentMonthEnd && !x.WasClosedBy(currentMonthEnd))
                          .Count();
        }

        private int GetLoggedDefectsForPeriod(IEnumerable<Defect> defects, string module, DateTime currentMonthEnd, DefectType defectType)
        {
            var currentMonthStart = new DateTime(currentMonthEnd.Year, currentMonthEnd.Month, 1);

            return defects.Where(x => x.Type == defectType && x.Module == module)
                          .Where(x => x.Created >= currentMonthStart && x.Created <= currentMonthEnd)
                          .Count();
        }

        private int GetLoggedDefectsForPeriod(IEnumerable<Defect> defects, string module, DateTime currentMonthEnd, DefectType[] defectTypes)
        {
            var currentMonthStart = new DateTime(currentMonthEnd.Year, currentMonthEnd.Month, 1);

            return defects.Where(x => defectTypes.Contains(x.Type) && x.Module == module)
                          .Where(x => x.Created >= currentMonthStart && x.Created <= currentMonthEnd)
                          .Count();
        }
    }
}