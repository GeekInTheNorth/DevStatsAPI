namespace DevStats.Domain.Jira
{
    public interface IJiraIdValidator
    {
        bool Validate(string idToValidate);
    }
}