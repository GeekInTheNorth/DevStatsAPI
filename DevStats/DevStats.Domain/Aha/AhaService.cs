using System;
using System.Configuration;
using DevStats.Domain.Aha.JsonModels;

namespace DevStats.Domain.Aha
{
    public class AhaService : IAhaService
    {
        private readonly IAhaSender sender;
        private readonly string apiRoot;
        private const string IssueSearchPath = @"{0}/api/v1/features?updated_since={1:yyyy-MM-dd}&fields=reference_num,name,created_at,updated_at,workflow_status,integration_fields&page={2}";

        public AhaService(IAhaSender sender)
        {
            this.sender = sender;
            this.apiRoot = ConfigurationManager.AppSettings.Get("AhaApiRoot") ?? string.Empty;
        }

        public void UpdateDefectsFromAha(DateTime earliestModified)
        {
            var pageNumber = 1;
            var numberOfPages = 1;

            try
            {
                while (pageNumber <= numberOfPages)
                {
                    var defects = GetDefectsFromAha(earliestModified, pageNumber);

                    numberOfPages = defects.Pagination.TotalPages;

                    pageNumber++;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private FeatureCollection GetDefectsFromAha(DateTime earliestModified, int pageNumber)
        {
            var url = string.Format(IssueSearchPath, apiRoot, earliestModified, pageNumber);

            return sender.Get<FeatureCollection>(url);
        }
    }
}