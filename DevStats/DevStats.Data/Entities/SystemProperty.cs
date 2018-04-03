using DevStats.Domain.SystemProperties;

namespace DevStats.Data.Entities
{
    public class SystemProperty
    {
        public int Id { get; set; }

        public SystemPropertyName Name { get; set; }

        public SystemPropertyType DataType { get; set; }

        public string Value { get; set; }
    }
}
