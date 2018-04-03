using System.Collections.Generic;

namespace DevStats.Domain.SystemProperties
{
    public interface ISystemPropertyService
    {
        SystemProperty Get(SystemPropertyName propertyName);

        IEnumerable<SystemProperty> Get();

        IEnumerable<SystemProperty> Get(IEnumerable<SystemPropertyName> propertyNames);

        void Save(SystemPropertyName propertyName, string propertyValue);
    }
}
