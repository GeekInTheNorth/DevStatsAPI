using Microsoft.Practices.Unity;
using System.Web.Http;
using DevStats.Data.Repositories;
using DevStats.Domain.Burndown;
using DevStats.Domain.DefectAnalysis;
using System.Web.Mvc;
using DevStats.Domain.Sprints;
using DevStats.Domain.Jira;
using DevStats.Domain.Jira.Logging;
using DevStats.Domain.Jira.Transitions;

namespace DevStats
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // Repositories
            container.RegisterType<IBurndownRepository, BurndownRepository>();
            container.RegisterType<IDefectRepository, DefectRepository>();
            container.RegisterType<ISprintRepository, SprintRepository>();
            container.RegisterType<IJiraLogRepository, JiraLogRepository>();
            container.RegisterType<IJiraTransitionRepository, JiraTransitionRepository>();

            // Utilities
            container.RegisterType<IJiraConvertor, JiraConvertor>();
            container.RegisterType<IJiraSender, JiraSender>();

            // Services
            container.RegisterType<IDefectService, DefectService>();
            container.RegisterType<ISprintService, SprintService>();
            container.RegisterType<IJiraService, JiraService>();
            container.RegisterType<IJiraTransitionService, JiraTransitionService>();

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}