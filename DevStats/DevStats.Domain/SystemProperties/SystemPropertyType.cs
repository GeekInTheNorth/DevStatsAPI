namespace DevStats.Domain.SystemProperties
{
    /// <summary>
    /// Stored as Ints in the DB
    /// </summary>
    public enum SystemPropertyType
    {
        Text = 0,
        Password = 1,
        Integer = 2,
        EmailAddress = 3
    }
}