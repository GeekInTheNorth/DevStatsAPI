using System;
using System.Collections.Generic;
using System.Linq;

namespace DevStats.Domain.SystemProperties
{
    public class SystemPropertyService : ISystemPropertyService
    {
        private readonly ISystemPropertyRepository repository;

        public SystemPropertyService(ISystemPropertyRepository repository)
        {
            this.repository = repository;
        }

        public SystemProperty Get(SystemPropertyName propertyName)
        {
            return repository.Get(propertyName);
        }

        public IEnumerable<SystemProperty> Get()
        {
            var allProperties = Enum.GetValues(typeof(SystemPropertyName)).Cast<SystemPropertyName>().ToList();

            return repository.Get(allProperties);
        }

        public IEnumerable<SystemProperty> Get(IEnumerable<SystemPropertyName> propertyNames)
        {
            return repository.Get(propertyNames);
        }

        public void Save(SystemPropertyName propertyName, string propertyValue)
        {
            var property = new SystemProperty(propertyName, propertyValue);

            repository.Save(property);
        }
    }
}
