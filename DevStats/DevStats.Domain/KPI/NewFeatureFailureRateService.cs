using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DevStats.Domain.KPI
{
    public class NewFeatureFailureRateService : INewFeatureFailureRateService
    {
        private readonly INewFeatureFailureRateRepository repository;

        public NewFeatureFailureRateService(INewFeatureFailureRateRepository repository)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));

            this.repository = repository;
        }

        public Dictionary<string, string> GetDevelopers()
        {
            return repository.GetDevelopers().ToDictionary(x => x, y => FormatName(y));
        }

        public NewFeatureFailureRate GetQualityKpi(string developer)
        {
            var stories = repository.GetQualityApi(developer);

            return new NewFeatureFailureRate(stories);
        }

        public Dictionary<string, NewFeatureFailureRate> GetQualityKpi()
        {
            var results = new Dictionary<string, NewFeatureFailureRate>();
            var developers = GetDevelopers().Select(x => x.Key);

            foreach(var developer in developers)
            {
                results.Add(developer, GetQualityKpi(developer));
            }

            return results;
        }

        private string FormatName(string userName)
        {
            userName = userName.Replace('.', ' ');

            TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
            return textInfo.ToTitleCase(userName);
        }
    }
}
