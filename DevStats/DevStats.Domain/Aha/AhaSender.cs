using System.Configuration;
using System.IO;
using System.Net;

namespace DevStats.Domain.Aha
{
    public class AhaSender : IAhaSender
    {
        private string ahaKey;

        public T Get<T>(string url)
        {
            var webRequest = WebRequest.Create(url);
            webRequest.Headers.Add(string.Format("Authorization: Bearer {0}", GetEncryptedCredentials()));
            webRequest.ContentType = "application/json; charset=utf-8";
            webRequest.Method = "GET";

            var httpResponse = (HttpWebResponse)webRequest.GetResponse();
            var response = string.Empty;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response);
        }

        private string GetEncryptedCredentials()
        {
            if (string.IsNullOrWhiteSpace(ahaKey))
            {
                ahaKey = ConfigurationManager.AppSettings.Get("AhaApiKey") ?? string.Empty;
            }

            return ahaKey;
        }
    }
}