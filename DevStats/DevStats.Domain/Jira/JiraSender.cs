using System;
using System.IO;
using System.Net;
using System.Text;
using DevStats.Domain.Messages;
using DevStats.Domain.SystemProperties;

namespace DevStats.Domain.Jira
{
    public class JiraSender : IJiraSender
    {
        private readonly ISystemPropertyRepository systemPropertyRepository;
        private readonly IJsonConvertor convertor;
        private string jiraKey;

        public JiraSender(IJsonConvertor convertor, ISystemPropertyRepository systemPropertyRepository)
        {
            if (convertor == null) throw new ArgumentNullException(nameof(convertor));
            if (systemPropertyRepository == null) throw new ArgumentNullException(nameof(systemPropertyRepository));

            this.convertor = convertor;
            this.systemPropertyRepository = systemPropertyRepository;
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
            return Send(url, jsonPackage, "POST");
        }

        public PostResult Put<T>(string url, T objectToSend)
        {
            var objectJson = convertor.Serialize<T>(objectToSend);

            return Put(url, objectJson);
        }

        public PostResult Put(string url, string jsonPackage)
        {
            return Send(url, jsonPackage, "PUT");
        }

        private PostResult Send(string url, string jsonPackage, string method)
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
            request.Method = method;
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
                var user = systemPropertyRepository.GetNonNullValue(SystemPropertyName.JiraUserName);
                var pass = systemPropertyRepository.GetNonNullValue(SystemPropertyName.JiraPassword);
                var plainTextKey = string.Format("{0}:{1}", user, pass);
                var plainTextBytes = Encoding.UTF8.GetBytes(plainTextKey);

                jiraKey = Convert.ToBase64String(plainTextBytes);
            }

            return jiraKey;
        }
    }
}