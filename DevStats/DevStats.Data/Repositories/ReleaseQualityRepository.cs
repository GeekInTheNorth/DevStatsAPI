using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.Reports.ReleaseReport;

namespace DevStats.Data.Repositories
{
    public class ReleaseQualityRepository : BaseRepository, IReleaseQualityRepository
    {
        public ReleaseQuality Get(int id)
        {
            var dataItem = Context.ReleaseQualities.Where(x => x.ID == id).FirstOrDefault();

            if (dataItem == null) return null;

            return ConvertToDomainObject(dataItem);
        }

        public ReleaseQuality Get(string product, string version)
        {
            var dataItem = Context.ReleaseQualities.Where(x => x.Product == product && x.Version == version).FirstOrDefault();

            if (dataItem == null) return null;

            return ConvertToDomainObject(dataItem);
        }

        public IEnumerable<ReleaseQuality> Get()
        {
            return Context.ReleaseQualities.OrderBy(x => x.Product).ThenBy(x => x.Version).ToList().Select(x => ConvertToDomainObject(x));
        }

        private ReleaseQuality ConvertToDomainObject(Entities.ReleaseQuality dataItem)
        {
            return new ReleaseQuality(
                dataItem.ID, 
                dataItem.Product, 
                dataItem.Version, 
                dataItem.Reworks, 
                dataItem.CodeReviewFailures, 
                dataItem.TestPlanReviewFailures, 
                dataItem.BacklogBugsResolved, 
                dataItem.RegressionBugs, 
                dataItem.DeploymentIssues, 
                dataItem.Hotfixes, 
                dataItem.DaysDelayed);
        }
    }
}