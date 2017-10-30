using System.Text.RegularExpressions;

namespace DevStats.Domain.Jira
{
    public class JiraIdValidator : IJiraIdValidator
    {
        public bool Validate(string idToValidate)
        {
            if (string.IsNullOrWhiteSpace(idToValidate))
                return false;

            var regEx = new Regex(@"([A-Z]{2,3}[\-]{1}[0-9]{1,5})");

            return regEx.IsMatch(idToValidate);
        }
    }
}