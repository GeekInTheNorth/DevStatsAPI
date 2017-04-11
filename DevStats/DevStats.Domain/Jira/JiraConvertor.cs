using System.Text;
using DevStats.Domain.Jira.JsonModels;
using DevStats.Domain.Jira.JsonModels.Create;

namespace DevStats.Domain.Jira
{
    public class JiraConvertor : IJiraConvertor
    {
        public JiraIssues Convert(byte[] jsonData)
        {
            var str = Encoding.Default.GetString(jsonData);

            return Convert(str);
        }

        public JiraIssues Convert(string jsonData)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<JiraIssues>(jsonData);
        }

        public string Convert(Subtask subtask)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(subtask);
        }
    }
}