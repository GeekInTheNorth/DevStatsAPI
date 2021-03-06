using System.Web.Http;
using System.Web.Mvc;
using DevStats.Data.Repositories;
using DevStats.Domain.Aha;
using DevStats.Domain.Communications;
using DevStats.Domain.DefectAnalysis;
using DevStats.Domain.Jira;
using DevStats.Domain.Jira.Logging;
using DevStats.Domain.Security;
using DevStats.Domain.KPI;
using DevStats.Domain.Logging;
using DevStats.Domain.MVP;
using DevStats.Domain.Reports.TaskingStatus;
using DevStats.Domain.Sprints;
using Microsoft.Practices.Unity;
using DevStats.Domain.Bitbucket;
using DevStats.Domain.Messages;
using DevStats.Domain.SiteStats;
using DevStats.Domain.SourceCode;
using DevStats.Domain.SystemProperties;

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
            container.RegisterType<IWorkLogRepository, WorkLogRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<INewFeaturePassRateRepository, NewFeaturePassRateRepository>();
            container.RegisterType<IApiLogRepository, ApiLogRepository>();
            container.RegisterType<IMvpRepository, MvpRepository>();
            container.RegisterType<IActualsVsEstimatesRepository, ActualsVsEstimatesRepository>();
            container.RegisterType<IDefectScoringRepository, DefectScoringRepository>();
            container.RegisterType<IEstimationRepository, EstimationRepository>();
            container.RegisterType<IBuildStatusRepository, BuildStatusRepository>();
            container.RegisterType<ISiteStatisticRepository, SiteStatisticRepository>();
            container.RegisterType<ISystemPropertyRepository, SystemPropertyRepository>();

            // Utilities
            container.RegisterType<IJsonConvertor, JsonConvertor>();
            container.RegisterType<IJiraSender, JiraSender>();
            container.RegisterType<IAhaSender, AhaSender>();
            container.RegisterType<IJiraIdValidator, JiraIdValidator>();
            container.RegisterType<IBitbucketSender, BitbucketSender>();

            // Services
            container.RegisterType<IDefectService, DefectService>();
            container.RegisterType<IJiraService, JiraService>();
            container.RegisterType<ISprintPlannerService, SprintPlannerService>();
            container.RegisterType<INewFeaturePassRateService, NewFeaturePassRateService>();
            container.RegisterType<IAhaService, AhaService>();
            container.RegisterType<IMvpService, MvpService>();
            container.RegisterType<IEmailService, EmailService>();
            container.RegisterType<IActualsVsEstimatesService, ActualsVsEstimatesService>();
            container.RegisterType<ITaskingStatusService, TaskingStatusService>();
            container.RegisterType<IBitbucketService, BitbucketService>();
            container.RegisterType<ISiteStatisticService, SiteStatisticService>();
            container.RegisterType<ISourceCodeService, SourceCodeService>();
            container.RegisterType<ISystemPropertyService, SystemPropertyService>();
            container.RegisterType<IApiLogService, ApiLogService>();

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}