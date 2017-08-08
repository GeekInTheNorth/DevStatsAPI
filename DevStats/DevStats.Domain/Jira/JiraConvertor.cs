using System.Text;

namespace DevStats.Domain.Jira
{
    public class JiraConvertor : IJiraConvertor
    {
        public T Deserialize<T>(byte[] jsonData)
        {
            var str = Encoding.Default.GetString(jsonData);

            return Deserialize<T>(str);
        }

        public T Deserialize<T>(string jsonData)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonData);
        }

        public string Serialize<T>(T obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }
}