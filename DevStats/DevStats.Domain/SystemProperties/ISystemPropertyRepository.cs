using System.Collections.Generic;

namespace DevStats.Domain.SystemProperties
{
    public interface ISystemPropertyRepository
    {
        SystemProperty Get(SystemPropertyName propertyName);

        string GetNonNullValue(SystemPropertyName propertyName);

        IEnumerable<SystemProperty> Get(IEnumerable<SystemPropertyName> propertyNames);

        void Save(SystemProperty property);
    }
}
