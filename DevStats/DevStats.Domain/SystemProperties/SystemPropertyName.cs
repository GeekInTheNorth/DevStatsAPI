namespace DevStats.Domain.SystemProperties
{
    /// <summary>
    /// Stored as Ints in the DB
    /// </summary>
    public enum SystemPropertyName
    {
        AhaApiKey = 0,
        AhaApiRoot = 1,
        AllowedIpAddresses = 2,
        BitbucketPassword = 3,
        BitbucketUserName = 4,
        EmailHost = 5,
        EmailPassword = 6,
        EmailPort = 7,
        EmailUserName = 8,
        JiraApiRoot = 9,
        JiraPassword = 10,
        JiraProjects = 11,
        JiraServiceDeskGroup = 12,
        JiraUserName = 13,
        MvpDistributionEmail = 14
    }
}