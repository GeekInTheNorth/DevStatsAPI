using Microsoft.Practices.Unity;
using System.Web.Http;
using DevStats.Data.Repositories;
using DevStats.Domain.DefectAnalysis;
using System.Web.Mvc;
using DevStats.Domain.Sprints;
using DevStats.Domain.Jira;
using DevStats.Domain.Jira.Logging;
using DevStats.Domain.Security;
using DevStats.Domain.Aha;
using DevStats.Domain.Logging;
using DevStats.Domain.MVP;
using DevStats.Domain.Communications;
using DevStats.Domain.KPI;
using DevStats.Domain.Reports.ReleaseReport;

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
            container.RegisterType<INewFeatureFailureRateRepository, NewFeatureFailureRateRepository>();
            container.RegisterType<IApiLogRepository, ApiLogRepository>();
            container.RegisterType<IMvpRepository, MvpRepository>();
            container.RegisterType<IActualsVsEstimatesRepository, ActualsVsEstimatesRepository>();
            container.RegisterType<IReleaseQualityRepository, ReleaseQualityRepository>();

            // Utilities
            container.RegisterType<IJiraConvertor, JiraConvertor>();
            container.RegisterType<IJiraSender, JiraSender>();
            container.RegisterType<IAhaSender, AhaSender>();
            container.RegisterType<IJiraIdValidator, JiraIdValidator>();

            // Services
            container.RegisterType<IDefectService, DefectService>();
            container.RegisterType<IJiraService, JiraService>();
            container.RegisterType<ISprintPlannerService, SprintPlannerService>();
            container.RegisterType<INewFeatureFailureRateService, NewFeatureFailureRateService>();
            container.RegisterType<IAhaService, AhaService>();
            container.RegisterType<IMvpService, MvpService>();
            container.RegisterType<IEmailService, EmailService>();
            container.RegisterType<IActualsVsEstimatesService, ActualsVsEstimatesService>();
            container.RegisterType<IReleaseQualityService, ReleaseQualityService>();

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}