using System;
using System.Linq;
using DevStats.Domain.Aha.JsonModels;
using DevStats.Domain.DefectAnalysis;
using DevStats.Domain.Logging;
using DevStats.Domain.SystemProperties;

namespace DevStats.Domain.Aha
{
    public class AhaService : IAhaService
    {
        private readonly IAhaSender sender;
        private readonly IApiLogRepository apiLogRepository;
        private readonly IDefectRepository defectRepository;
        private readonly ISystemPropertyRepository systemPropertyRepository;
        private const string IssueSearchPath = @"{0}/api/v1/features?updated_since={1:yyyy-MM-dd}&fields=reference_num,name,created_at,updated_at,workflow_status,integration_fields,custom_fields,workflow_kind&page={2}";

        public AhaService(IAhaSender sender, IApiLogRepository apiLogRepository, IDefectRepository defectRepository, ISystemPropertyRepository systemPropertyRepository)
        {
            this.sender = sender;
            this.apiLogRepository = apiLogRepository;
            this.defectRepository = defectRepository;
            this.systemPropertyRepository = systemPropertyRepository;
        }

        public void UpdateDefectsFromAha(DateTime earliestModified)
        {
            var pageNumber = 1;
            var numberOfPages = 1;

            while (pageNumber <= numberOfPages)
            {
                var ahaData = GetDefectsFromAha(earliestModified, pageNumber);

                if (ahaData == null) break;

                numberOfPages = ahaData.Pagination.TotalPages;

                var defects = ahaData.Features
                                     .Select(x => CaseFeatureToDefect(x));

                defectRepository.Save(defects);

                pageNumber++;
            }
        }

        private FeatureCollection GetDefectsFromAha(DateTime earliestModified, int pageNumber)
        {
            var url = string.Format(IssueSearchPath, GetApiRoot(), earliestModified, pageNumber);

            try
            {
                var features = sender.Get<FeatureCollection>(url);

                apiLogRepository.Success("AHA", url, "Get defects from Aha");

                return features;
            }
            catch(Exception ex)
            {
                apiLogRepository.Failure("AHA", url, "Get defects from Aha", ex);

                return null;
            }
        }

        private AhaDefect CaseFeatureToDefect(Feature feature)
        {
            var completedStates = new string[] { "Shipped", "Ready to ship", "Will not Implement" };
            DateTime? endDate = null;
            string jiraId = null;
            string module = null;
            DefectType type = DefectType.NonDefect;

            if (completedStates.Contains(feature.Status.Name))
                endDate = feature.Updated;

            if (feature.IntegrationFields != null)
                jiraId = feature.IntegrationFields
                                .Where(x => x.Service.Equals("Jira", StringComparison.CurrentCultureIgnoreCase) && x.Name.Equals("Key", StringComparison.CurrentCultureIgnoreCase))
                                .Select(x => x.Value)
                                .FirstOrDefault();

            if (feature.CustomFields != null)
                module = feature.CustomFields.Where(x => x.Name == "Module").Select(x => x.Value).FirstOrDefault();

            if (feature.FeatureType != null && feature.FeatureType.Name == "Bug fix")
                type = DefectType.External;

            if (feature.FeatureType != null && feature.FeatureType.Name == "Defect")
                type = DefectType.Internal;

            return new AhaDefect(jiraId, feature.Id, module, type, feature.Created, endDate);
        }

        private string GetApiRoot()
        {
            return systemPropertyRepository.GetNonNullValue(SystemPropertyName.AhaApiRoot);
        }
    }
}