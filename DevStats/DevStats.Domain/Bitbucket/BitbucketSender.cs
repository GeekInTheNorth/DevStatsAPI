using System;
using System.IO;
using System.Net;
using System.Text;
using DevStats.Domain.Messages;
using DevStats.Domain.SystemProperties;

namespace DevStats.Domain.Bitbucket
{
    public class BitbucketSender : IBitbucketSender
    {
        private readonly IJsonConvertor convertor;
        private readonly ISystemPropertyRepository systemPropertyRepository;

        public BitbucketSender(IJsonConvertor convertor, ISystemPropertyRepository systemPropertyRepository)
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

            return Send(url, objectJson, "POST");
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
            var user = systemPropertyRepository.GetNonNullValue(SystemPropertyName.BitbucketUserName);
            var pass = systemPropertyRepository.GetNonNullValue(SystemPropertyName.BitbucketPassword);
            var plainTextKey = string.Format("{0}:{1}", user, pass);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainTextKey);

            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
