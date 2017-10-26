using System;
using System.Collections.Generic;
using System.Linq;

namespace DevStats.Domain.DefectAnalysis
{
    public class ProductDefectSummaries
    {
        public Dictionary<string, DefectSummaries> ProductSummaries { get; set; }

        public ProductDefectSummaries(IEnumerable<Defect> defects, DateTime firstMonthStart, DateTime lastMonthEnd)
        {
            ProductSummaries = new Dictionary<string, DefectSummaries>();

            var distinctProducts = defects.Select(x => x.Product).Distinct();

            foreach(var distinctProduct in distinctProducts)
            {
                var productDefects = defects.Where(x => x.Product == distinctProduct);
                var summary = new DefectSummaries(productDefects, firstMonthStart, lastMonthEnd);

                ProductSummaries.Add(distinctProduct, summary);
            }
        }
    }
}