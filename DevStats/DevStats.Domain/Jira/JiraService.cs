using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using DevStats.Domain.Jira.JsonModels;
using DevStats.Domain.Jira.JsonModels.Create;
using DevStats.Domain.Jira.Logging;

namespace DevStats.Domain.Jira
{
    public class JiraService: IJiraService
    {
        private readonly IJiraConvertor convertor;
        private readonly IJiraLogRepository loggingRepository;
        private const string JiraSearchPath = @"{0}/rest/api/2/search?jql={1}";
        private const string JiraCreateTaskPath = @"{0}/rest/api/2/issue/";
        private string jiraKey;
        private string apiRoot;

        public JiraService(IJiraConvertor convertor, IJiraLogRepository loggingRepository)
        {
            if (convertor == null) throw new ArgumentNullException(nameof(convertor));
            if (loggingRepository == null) throw new ArgumentNullException(nameof(loggingRepository));

            this.convertor = convertor;
            this.loggingRepository = loggingRepository;
        }

        public void ProcessUpdatedItems()
        {
            var jiraIssues = GetJiraIssues();
            var defects = jiraIssues.Select(x => x.ToDefect());
        }

        public void CreateSubTasks(string issueId, string displayIssueId, string sourceDomain, string content)
        {
            loggingRepository.LogIncomingHook(issueId, displayIssueId, content);

            CreateAndPost(issueId, displayIssueId, SubtaskType.Merge);
            CreateAndPost(issueId, displayIssueId, SubtaskType.POReview);
        }

        public IEnumerable<JiraAudit> GetJiraAudit(DateTime from, DateTime to)
        {
            return loggingRepository.Get(from, to);
        }

        private void CreateAndPost(string issueId, string displayIssueId, SubtaskType taskType)
        {
            var task = convertor.Convert(new Subtask(displayIssueId, taskType));
            Post(issueId, displayIssueId, taskType, task);
        }

        private IEnumerable<Issue> GetJiraIssues()
        {
            var filter = FilterBuilder.Create()
                                      .WithAProject(JiraProject.CascadePayroll)
                                      .WithIssueTypesOf(JiraIssueType.Subtasks)
                                      .WithIssueStatesOf(JiraState.Done)
                                      .Build();
            filter = WebUtility.UrlEncode(filter);

            var url = string.Format(JiraSearchPath, GetApiRoot(), filter);

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

            var jiraIssueCollection = convertor.Convert(response);

            if (jiraIssueCollection == null || jiraIssueCollection.Issues == null || !jiraIssueCollection.Issues.Any())
                return new List<Issue>();

            return jiraIssueCollection.Issues.AsEnumerable();
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

        private string GetApiRoot()
        {
            if (string.IsNullOrWhiteSpace(apiRoot))
            {
                apiRoot = ConfigurationManager.AppSettings.Get("JiraApiRoot") ?? string.Empty;
            }

            return apiRoot;
        }

        private void Post(string issueId, string displayIssueId, SubtaskType subTaskType, string jsonObject)
        {
            var url = string.Format(JiraCreateTaskPath, GetApiRoot());

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers.Add("Authorization", "Basic " + GetEncryptedCredentials());

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(jsonObject);
            }

            WebResponse response;
            bool wasSuccessful;
            try
            {
                response = request.GetResponse() as WebResponse;
                wasSuccessful = true;
            }
            catch (WebException ex)
            {
                response = ex.Response as WebResponse;
                wasSuccessful = false;
            }

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                var result = reader.ReadToEnd();
                loggingRepository.LogTaskCreateEvent(issueId, displayIssueId, subTaskType, wasSuccessful, result);
            }
        }
    }
}