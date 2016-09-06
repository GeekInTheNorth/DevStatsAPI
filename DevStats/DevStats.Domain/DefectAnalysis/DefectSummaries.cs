using System;
using System.Collections.Generic;
using System.Linq;

namespace DevStats.Domain.DefectAnalysis
{
    public class DefectSummaries
    {
        private DefectType[] AllDefectTypes = new [] { DefectType.Internal, DefectType.External, DefectType.Rework };

        private DefectType[] InternalAndExternal = new [] { DefectType.Internal, DefectType.External };

        public List<DefectSummary> TotalOutstandingDefects { get; private set; }

        public List<DefectSummary> InternalOutstandingDefects { get; private set; }

        public List<DefectSummary> ExternalOutstandingDefects { get; private set; }

        public List<DefectSummary> TotalItemsLogged { get; private set; }

        public List<DefectSummary> InternalItemsLogged { get; private set; }

        public List<DefectSummary> ExternalItemsLogged { get; private set; }

        public List<DefectSummary> ReworkItemsLogged { get; private set; }

        public DefectSummaries(IEnumerable<Defect> defects, DateTime firstMonthStart, DateTime lastMonthEnd)
        {
            var today = DateTime.Today;
            var currentMonthEnd = firstMonthStart.AddMonths(1).AddDays(-1);
            var distinctModules = defects.Select(x => x.Module).Distinct();

            TotalOutstandingDefects = new List<DefectSummary>();
            InternalOutstandingDefects = new List<DefectSummary>();
            ExternalOutstandingDefects = new List<DefectSummary>();
            TotalItemsLogged = new List<DefectSummary>();
            InternalItemsLogged = new List<DefectSummary>();
            ExternalItemsLogged = new List<DefectSummary>();
            ReworkItemsLogged = new List<DefectSummary>();

            while (currentMonthEnd <= lastMonthEnd)
            {
                TotalOutstandingDefects.Add(GetOutstandingDefectsForPeriod(defects, distinctModules, currentMonthEnd, InternalAndExternal));
                InternalOutstandingDefects.Add(GetOutstandingDefectsForPeriod(defects, distinctModules, currentMonthEnd, DefectType.Internal));
                ExternalOutstandingDefects.Add(GetOutstandingDefectsForPeriod(defects, distinctModules, currentMonthEnd, DefectType.External));
                TotalItemsLogged.Add(GetLoggedDefectsForPeriod(defects, distinctModules, currentMonthEnd, AllDefectTypes));
                InternalItemsLogged.Add(GetLoggedDefectsForPeriod(defects, distinctModules, currentMonthEnd, DefectType.Internal));
                ExternalItemsLogged.Add(GetLoggedDefectsForPeriod(defects, distinctModules, currentMonthEnd, DefectType.External));
                ReworkItemsLogged.Add(GetLoggedDefectsForPeriod(defects, distinctModules, currentMonthEnd, DefectType.External));

                currentMonthEnd = new DateTime(currentMonthEnd.Year, currentMonthEnd.Month, 1).AddMonths(2).AddDays(-1);
            }
        }

        private DefectSummary GetOutstandingDefectsForPeriod(IEnumerable<Defect> defects, IEnumerable<string> distinctModules, DateTime currentMonthEnd, DefectType defectType)
        {
            var defectTypes = new DefectType[] { defectType };

            return GetOutstandingDefectsForPeriod(defects, distinctModules, currentMonthEnd, defectTypes);
        }

        private DefectSummary GetOutstandingDefectsForPeriod(IEnumerable<Defect> defects, IEnumerable<string> distinctModules, DateTime currentMonthEnd, DefectType[] defectTypes)
        {
            var moduleBreakdown = defects.Where(x => defectTypes.Contains(x.Type))
                                         .Where(x => x.Created <= currentMonthEnd && !x.WasClosedBy(currentMonthEnd))
                                         .GroupBy(x => x.Module)
                                         .Select(x => new { Module = x.Key, Items = x.Count() })
                                         .ToDictionary(x => x.Module, x => x.Items);

            var absentModules = distinctModules.Where(x => !moduleBreakdown.ContainsKey(x));

            foreach (var absentModule in absentModules)
                moduleBreakdown.Add(absentModule, 0);

            return new DefectSummary
            {
                MonthString = currentMonthEnd.ToString("MMM-yy"),
                ModuleBreakdown = moduleBreakdown.OrderBy(x => x.Key).ToDictionary(x => x.Key, x=> x.Value)
            };
        }

        private DefectSummary GetLoggedDefectsForPeriod(IEnumerable<Defect> defects, IEnumerable<string> distinctModules, DateTime currentMonthEnd, DefectType defectType)
        {
            var defectTypes = new DefectType[] { defectType };

            return GetLoggedDefectsForPeriod(defects, distinctModules, currentMonthEnd, defectTypes);
        }

        private DefectSummary GetLoggedDefectsForPeriod(IEnumerable<Defect> defects, IEnumerable<string> distinctModules, DateTime currentMonthEnd, DefectType[] defectTypes)
        {
            var currentMonthStart = new DateTime(currentMonthEnd.Year, currentMonthEnd.Month, 1);
            var moduleBreakdown = defects.Where(x => defectTypes.Contains(x.Type))
                                         .Where(x => x.Created >= currentMonthStart && x.Created <= currentMonthEnd)
                                         .GroupBy(x => x.Module)
                                         .Select(x => new { Module = x.Key, Items = x.Count() })
                                         .ToDictionary(x => x.Module, x => x.Items);

            var absentModules = distinctModules.Where(x => !moduleBreakdown.ContainsKey(x));

            foreach (var absentModule in absentModules)
                moduleBreakdown.Add(absentModule, 0);

            return new DefectSummary
            {
                MonthString = currentMonthEnd.ToString("MMM-yy"),
                ModuleBreakdown = moduleBreakdown.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value)
            };
        }
    }
}