using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using DevStats.Domain.Messages;

namespace DevStats.Domain.Bitbucket
{
    public class BitbucketSender : IBitbucketSender
    {
        private readonly IJsonConvertor convertor;

        public BitbucketSender(IJsonConvertor convertor)
        {
            if (convertor == null) throw new ArgumentNullException(nameof(convertor));

            this.convertor = convertor;
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
            var user = ConfigurationManager.AppSettings.Get("BitbucketUserName") ?? string.Empty;
            var pass = ConfigurationManager.AppSettings.Get("BitbucketPassword") ?? string.Empty;
            var plainTextKey = string.Format("{0}:{1}", user, pass);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainTextKey);

            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
