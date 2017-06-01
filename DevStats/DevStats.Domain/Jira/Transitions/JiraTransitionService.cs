using System;
using System.Collections.Generic;
using System.Configuration;
using DevStats.Domain.Jira.Transitions.Models;

namespace DevStats.Domain.Jira.Transitions
{
    public class JiraTransitionService : IJiraTransitionService
    {
        private readonly IJiraTransitionRepository repository;
        private readonly IJiraSender jiraSender;
        private string apiRoot;
        private const string JiraTransitionApi = @"{0}/rest/api/2/project/{1}/statuses";

        public JiraTransitionService(IJiraTransitionRepository repository, IJiraSender jiraSender)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (jiraSender == null) throw new ArgumentNullException(nameof(jiraSender));

            this.repository = repository;
            this.jiraSender = jiraSender;
        }

        public void UpdateTransitions(string project, bool fullRefresh)
        {
            var url = string.Format(JiraTransitionApi, GetApiRoot(), project);
            var transitions = jiraSender.Get<List<Transition>>(url);

            if (fullRefresh) repository.RemoveAllTransitions(project);

            foreach(var transition in transitions)
            {
                foreach(var status in transition.Statuses)
                {
                    repository.SaveTransition(project, transition.IssueType, status.Name, status.TransitionId);
                }
            }
        }

        public void UpdateTransitions(bool fullRefresh)
        {
            var projects = new List<string> { "CPR", "CHR", "OCT" };

            foreach(var project in projects)
            {
                UpdateTransitions(project, fullRefresh);
            }
        }

        private string GetApiRoot()
        {
            if (string.IsNullOrWhiteSpace(apiRoot))
            {
                apiRoot = ConfigurationManager.AppSettings.Get("JiraApiRoot") ?? string.Empty;
            }

            return apiRoot;
        }
    }
}