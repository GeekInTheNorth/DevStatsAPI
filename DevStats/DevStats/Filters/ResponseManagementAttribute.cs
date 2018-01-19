using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using DevStats.Data.Repositories;
using DevStats.Domain.Logging;

namespace DevStats.Filters
{
    public class ResponseManagementAttribute : ActionFilterAttribute
    {
        private readonly IApiLogRepository logRepository;

        public ResponseManagementAttribute()
        {
            logRepository = new ApiLogRepository();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError);

            if (actionExecutedContext.Response != null && actionExecutedContext.Response.StatusCode == HttpStatusCode.NoContent)
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.OK);

            LogApiCall(actionExecutedContext);
            base.OnActionExecuted(actionExecutedContext);
        }

        private void LogApiCall(HttpActionExecutedContext actionExecutedContext)
        {
            var isSuccess = actionExecutedContext.Response != null && actionExecutedContext.Response.StatusCode == HttpStatusCode.OK;
            var statusCode = actionExecutedContext.Response != null ? actionExecutedContext.Response.StatusCode.ToString() : "Unknown";

            logRepository.Log(
                actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                actionExecutedContext.ActionContext.Request.RequestUri.ToString(),
                actionExecutedContext.ActionContext.ActionDescriptor.ActionName,
                isSuccess,
                statusCode);
        }
    }
}