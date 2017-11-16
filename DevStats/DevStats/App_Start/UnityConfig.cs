using Microsoft.Practices.Unity;
using System.Web.Http;
using DevStats.Data.Repositories;
using DevStats.Domain.DefectAnalysis;
using System.Web.Mvc;
using DevStats.Domain.Sprints;
using DevStats.Domain.Jira;
using DevStats.Domain.Jira.Logging;
using DevStats.Domain.Security;
using DevStats.Domain.DeveloperKpi;
using DevStats.Domain.Aha;
using DevStats.Domain.Logging;
using DevStats.Domain.MVP;
using DevStats.Domain.Communications;
using DevStats.Domain.KPI;

namespace DevStats
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // Repositories
            container.RegisterType<IDefectRepository, DefectRepository>();
            container.RegisterType<IJiraLogRepository, JiraLogRepository>();
            container.RegisterType<IProjectsRepository, ProjectsRepository>();
            container.RegisterType<IWorkLogRepository, WorkLogRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IDeveloperKpiRepository, DeveloperKpiRepository>();
            container.RegisterType<IApiLogRepository, ApiLogRepository>();
            container.RegisterType<IMvpRepository, MvpRepository>();
            container.RegisterType<IActualsVsEstimatesRepository, ActualsVsEstimatesRepository>();

            // Utilities
            container.RegisterType<IJiraConvertor, JiraConvertor>();
            container.RegisterType<IJiraSender, JiraSender>();
            container.RegisterType<IAhaSender, AhaSender>();
            container.RegisterType<IJiraIdValidator, JiraIdValidator>();

            // Services
            container.RegisterType<IDefectService, DefectService>();
            container.RegisterType<IJiraService, JiraService>();
            container.RegisterType<ISprintPlannerService, SprintPlannerService>();
            container.RegisterType<IDeveloperKpiService, DeveloperKpiService>();
            container.RegisterType<IAhaService, AhaService>();
            container.RegisterType<IMvpService, MvpService>();
            container.RegisterType<IEmailService, EmailService>();
            container.RegisterType<IActualsVsEstimatesService, ActualsVsEstimatesService>();

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}