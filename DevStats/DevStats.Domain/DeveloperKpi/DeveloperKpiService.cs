using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DevStats.Domain.DeveloperKpi
{
    public class DeveloperKpiService : IDeveloperKpiService
    {
        private readonly IDeveloperKpiRepository repository;

        public DeveloperKpiService(IDeveloperKpiRepository repository)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));

            this.repository = repository;
        }

        public Dictionary<string, string> GetDevelopers()
        {
            return repository.GetDevelopers().ToDictionary(x => x, y => FormatName(y));
        }

        public DeveloperQualityKPI GetQualityKpi(string developer)
        {
            var stories = repository.GetQualityApi(developer);

            return new DeveloperQualityKPI(stories);
        }

        private string FormatName(string userName)
        {
            userName = userName.Replace('.', ' ');

            TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
            return textInfo.ToTitleCase(userName);
        }
    }
}
