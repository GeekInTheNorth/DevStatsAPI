using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DevStats.Domain.SystemProperties;

namespace DevStats.Models.SystemProperties
{
    public class SystemPropertyModel : IValidatableObject
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            SystemPropertyName propertyName;
            if (!Enum.TryParse(Name, true, out propertyName))
                results.Add(new ValidationResult("Invalid Name"));

            return results;
        }

        public SystemPropertyName GetSystemPropertyName()
        {
            SystemPropertyName propertyName;
            if (!Enum.TryParse(Name, true, out propertyName))
                throw new Exception("Invalid Name");

            return propertyName;
        }
    }
}