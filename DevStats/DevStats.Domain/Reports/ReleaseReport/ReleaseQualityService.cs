using System;
using System.Collections.Generic;

namespace DevStats.Domain.Reports.ReleaseReport
{
    public class ReleaseQualityService : IReleaseQualityService
    {
        private readonly IReleaseQualityRepository repository;

        public ReleaseQualityService(IReleaseQualityRepository repository)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));

            this.repository = repository;
        }

        public ReleaseQuality Get(int id)
        {
            return repository.Get(id);
        }

        public ReleaseQuality Get(string product, string version)
        {
            return repository.Get(product, version);
        }

        public IEnumerable<ReleaseQuality> Get()
        {
            return repository.Get();
        }
    }
}