using System.Text.RegularExpressions;

namespace DevStats.Domain.Jira
{
    public class JiraIdValidator : IJiraIdValidator
    {
        public bool Validate(string idToValidate)
        {
            if (string.IsNullOrWhiteSpace(idToValidate))
                return false;

            var regEx = new Regex(@"^([A-Z0-9]{2,9}[\-]{1}[0-9]{1,9})$");

            return regEx.IsMatch(idToValidate);
        }
    }
}