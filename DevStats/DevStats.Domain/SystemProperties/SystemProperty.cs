namespace DevStats.Domain.SystemProperties
{
    public class SystemProperty
    {
        public SystemPropertyName Name { get; set; }

        public SystemPropertyType DataType { get; set; }

        public string Value { get; set; }

        public SystemProperty(SystemPropertyName name)
        {
            Name = name;
            Value = Value ?? string.Empty;

            switch (Name)
            {
                case SystemPropertyName.AhaApiKey:
                case SystemPropertyName.BitbucketPassword:
                case SystemPropertyName.EmailPassword:
                case SystemPropertyName.JiraPassword:
                    DataType = SystemPropertyType.Password;
                    break;
                case SystemPropertyName.EmailPort:
                    DataType = SystemPropertyType.Integer;
                    break;
                case SystemPropertyName.MvpDistributionEmail:
                    DataType = SystemPropertyType.EmailAddress;
                    break;
                default:
                    DataType = SystemPropertyType.Text;
                    break;
            }
        }

        public SystemProperty(SystemPropertyName name, string value) : this(name)
        {
            Value = value;
        }

        public SystemProperty(SystemPropertyName name, SystemPropertyType dataType, string value)
        {
            Name = name;
            DataType = dataType;
            Value = value;
        }
    }
}
