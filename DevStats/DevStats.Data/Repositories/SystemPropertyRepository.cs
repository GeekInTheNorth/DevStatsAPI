using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.SystemProperties;

namespace DevStats.Data.Repositories
{
    public class SystemPropertyRepository : ISystemPropertyRepository
    {
        private readonly IDictionary<SystemPropertyName, SystemProperty> properties;

        public SystemPropertyRepository()
        {
            properties = new Dictionary<SystemPropertyName, SystemProperty>();
        }

        public SystemProperty Get(SystemPropertyName propertyName)
        {
            if (properties.ContainsKey(propertyName))
                return properties[propertyName];

            using (var context = new DevStatContext())
            {
                var dataObject = context.SystemProperties.Where(x => x.Name == propertyName).FirstOrDefault();

                return ToDomainObject(propertyName, dataObject);
            }
        }

        public IEnumerable<SystemProperty> Get(IEnumerable<SystemPropertyName> propertyNames)
        {
            using (var context = new DevStatContext())
            {
                var items = context.SystemProperties.Where(x => propertyNames.Contains(x.Name)).ToList().Select(x => ToDomainObject(x.Name, x)).ToList();
                var dataNames = items.Select(x => x.Name);
                var absentNames = propertyNames.Where(x => !dataNames.Contains(x));

                items.AddRange(absentNames.Select(x => new SystemProperty(x)));

                return items.OrderBy(x => x.Name);
            }
        }

        public void Save(SystemProperty property)
        {
            var propertyValue = property.Value ?? string.Empty;

            using (var context = new DevStatContext())
            {
                var dataObject = context.SystemProperties.Where(x => x.Name == property.Name).FirstOrDefault();

                if (dataObject == null)
                {
                    dataObject = new Entities.SystemProperty
                    {
                        Name = property.Name,
                        DataType = property.DataType,
                    };

                    context.SystemProperties.Add(dataObject);
                }

                dataObject.Value = propertyValue.Trim();
                context.SaveChanges();
            }
        }

        private SystemProperty ToDomainObject(SystemPropertyName propertyName, Entities.SystemProperty dataObject)
        {
            if (dataObject == null)
                return new SystemProperty(propertyName);

            return new SystemProperty(dataObject.Name, dataObject.DataType, dataObject.Value);
        }
    }
}
