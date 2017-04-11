using DevStats.Domain.Jira.JsonModels;
using DevStats.Domain.Jira.JsonModels.Create;

namespace DevStats.Domain.Jira
{
    public interface IJiraConvertor
    {
        JiraIssues Convert(byte[] jsonData);

        JiraIssues Convert(string jsonData);

        string Convert(Subtask subtask);
    }
}