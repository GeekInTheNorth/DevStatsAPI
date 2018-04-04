using System.IO;
using System.Net;
using DevStats.Domain.SystemProperties;

namespace DevStats.Domain.Aha
{
    public class AhaSender : IAhaSender
    {
        private readonly ISystemPropertyRepository systemPropertyRepository;
        private string ahaKey;

        public AhaSender(ISystemPropertyRepository systemPropertyRepository)
        {
            if (systemPropertyRepository == null) throw new System.ArgumentNullException(nameof(systemPropertyRepository));

            this.systemPropertyRepository = systemPropertyRepository;
        }

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
                ahaKey = systemPropertyRepository.GetNonNullValue(SystemPropertyName.AhaApiKey);
            }

            return ahaKey;
        }
    }
}