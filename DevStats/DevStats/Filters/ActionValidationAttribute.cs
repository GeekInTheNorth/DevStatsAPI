using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using DevStats.Data.Repositories;
using DevStats.Domain.Jira;
using DevStats.Domain.Logging;

namespace DevStats.Filters
{
    public class ActionValidationAttribute : ActionFilterAttribute
    {
        private readonly IJiraIdValidator jiraIdValidator;
        private readonly IApiLogRepository logRepository;

        public ActionValidationAttribute()
        {
            jiraIdValidator = new JiraIdValidator();
            logRepository = new ApiLogRepository();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            ValidateArguments(actionContext);
            base.OnActionExecuting(actionContext);
        }

        private void ValidateArguments(HttpActionContext actionContext)
        {
            if (actionContext.ActionArguments == null)
                return;

            foreach (var argument in actionContext.ActionArguments)
                ValidateArgument(actionContext, argument.Key, argument.Value);
        }

        private void ValidateArgument(HttpActionContext actionContext, string argumentName, object argument)
        {
            if (argumentName.Equals("jiraId", StringComparison.CurrentCultureIgnoreCase))
            {
                if (!jiraIdValidator.Validate(argument.ToString()))
                {
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Format("Invalid Jira Id: {0}", argument.ToString()));
                    LogValidationError(actionContext, argumentName, argument);
                }
            }
        }

        private void LogValidationError(HttpActionContext actionContext, string argumentName, object argument)
        {
            logRepository.Log(
                actionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                actionContext.Request.RequestUri.ToString(),
                actionContext.ActionDescriptor.ActionName,
                false,
                string.Format("{0}: Invalid {1}: {2}", HttpStatusCode.BadRequest, argumentName, argument));
        }
    }
}