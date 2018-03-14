using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
            if (argument != null && argument is IValidatableObject)
            {
                if (!actionContext.ModelState.IsValid)
                {
                    var errors = actionContext.ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage));
                    var errorMessage = string.Join("; ", errors);
                    LogValidationError(actionContext, argumentName, errorMessage);
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Format("Invalid model: {0}", errorMessage));
                }
            }
            else if (argumentName.Equals("jiraId", StringComparison.CurrentCultureIgnoreCase))
            {
                if (!jiraIdValidator.Validate(argument.ToString()))
                {
                    LogValidationError(actionContext, argumentName, argument);
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Format("Invalid Jira Id: {0}", argument.ToString()));
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