using Microsoft.Practices.Unity;
using System.Web.Http;
using DevStats.Data.Repositories;
using DevStats.Domain.Burndown;
using DevStats.Domain.DefectAnalysis;
using System.Web.Mvc;

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

            // Services
            container.RegisterType<IDefectService, DefectService>();

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}