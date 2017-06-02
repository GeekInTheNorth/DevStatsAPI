using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

namespace DevStats.Domain.Jira
{
    public class JiraSender : IJiraSender
    {
        private readonly IJiraConvertor convertor;
        private string jiraKey;

        public JiraSender(IJiraConvertor convertor)
        {
            if (convertor == null) throw new ArgumentNullException(nameof(convertor));

            this.convertor = convertor;
        }

        public T Get<T>(string url)
        {
            var webRequest = WebRequest.Create(url);
            webRequest.Headers.Add(string.Format("Authorization: Basic {0}", GetEncryptedCredentials()));
            webRequest.ContentType = "application/json; charset=utf-8";
            webRequest.Method = "GET";

            var httpResponse = (HttpWebResponse)webRequest.GetResponse();
            var response = string.Empty;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            return convertor.Deserialize<T>(response);
        }

        public PostResult Post<T>(string url, T objectToSend)
        {
            var objectJson = convertor.Serialize<T>(objectToSend);

            return Post(url, objectJson);
        }

        public PostResult Post(string url, string jsonPackage)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return new PostResult
                {
                    WasSuccessful = false,
                    Response = string.Format("Invalid Url: {0}", url)
                };
            }

            var postResult = new PostResult();

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers.Add("Authorization", "Basic " + GetEncryptedCredentials());

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(jsonPackage);
            }

            WebResponse response;
            try
            {
                response = request.GetResponse() as WebResponse;
                postResult.WasSuccessful = true;
            }
            catch (WebException ex)
            {
                response = ex.Response as WebResponse;
                postResult.WasSuccessful = false;
            }

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                if (!reader.EndOfStream)
                    postResult.Response = reader.ReadToEnd();
            }

            return postResult;
        }

        private string GetEncryptedCredentials()
        {
            if (string.IsNullOrWhiteSpace(jiraKey))
            {
                jiraKey = ConfigurationManager.AppSettings.Get("JiraEncryptedAuth") ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(jiraKey))
            {
                var user = ConfigurationManager.AppSettings.Get("JiraUserName") ?? string.Empty;
                var pass = ConfigurationManager.AppSettings.Get("JiraPassword") ?? string.Empty;
                var plainTextKey = string.Format("{0}:{1}", user, pass);
                var plainTextBytes = Encoding.UTF8.GetBytes(plainTextKey);

                jiraKey = Convert.ToBase64String(plainTextBytes);
            }

            return jiraKey;
        }
    }
}