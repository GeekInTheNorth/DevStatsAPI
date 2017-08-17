using Microsoft.Practices.Unity;
using System.Web.Http;
using DevStats.Data.Repositories;
using DevStats.Domain.Burndown;
using DevStats.Domain.DefectAnalysis;
using System.Web.Mvc;
using DevStats.Domain.Sprints;
using DevStats.Domain.Jira;
using DevStats.Domain.Jira.Logging;

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
            container.RegisterType<IProjectsRepository, ProjectsRepository>();
            container.RegisterType<IWorkLogRepository, WorkLogRepository>();

            // Utilities
            container.RegisterType<IJiraConvertor, JiraConvertor>();
            container.RegisterType<IJiraSender, JiraSender>();

            // Services
            container.RegisterType<IDefectService, DefectService>();
            container.RegisterType<ISprintService, SprintService>();
            container.RegisterType<IJiraService, JiraService>();
            container.RegisterType<ISprintPlannerService, SprintPlannerService>();

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}