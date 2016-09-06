using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;
using DevStats.Data.Repositories;

namespace DevStats.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AllowCorsAttribute : Attribute, ICorsPolicyProvider
    {
        private CorsPolicy corsPolicy;

        public AllowCorsAttribute() : this("*", "*")
        {
        }

        public AllowCorsAttribute(string headers, string methods)
        {
            BuildPolicy(headers, methods);
        }

        private void BuildPolicy(string allowedHeaders, string allowedMethods)
        {
            corsPolicy = new CorsPolicy();
            corsPolicy.AllowAnyOrigin = false;

            var repository = new OriginsRepository();
            var allowedOrigins = repository.Get();

            if (allowedOrigins.Any())
            {
                foreach (var allowedOrigin in allowedOrigins)
                {
                    corsPolicy.Origins.Add(allowedOrigin);
                }
            }
            else
                corsPolicy.AllowAnyOrigin = true;

            if (allowedHeaders == "*")
                corsPolicy.AllowAnyHeader = true;
            else
            {
                var headerList = allowedHeaders.Split(',');

                foreach (var header in headerList)
                    corsPolicy.Headers.Add(header);
            }

            if (allowedMethods == "*")
                corsPolicy.AllowAnyMethod = true;
            else
            {
                var methodList = allowedMethods.Split(',');

                foreach (var method in methodList)
                    corsPolicy.Methods.Add(method);
            }
        }

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(corsPolicy);
        }
    }
}