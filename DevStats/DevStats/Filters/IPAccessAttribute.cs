using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DevStats.Filters
{
    public class IPAccessAttribute : ActionFilterAttribute
    {
        private string RedirectController;
        private string RedirectAction;
        private DateTime LastCheckedIPList;

        public IPAccessAttribute() : this("Home", "Index")
        {
        }

        public IPAccessAttribute(string redirectController, string redirectAction)
        {
            RedirectController = redirectController;
            RedirectAction = redirectAction;
            LastCheckedIPList = DateTime.MinValue;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!HttpContext.Current.Request.IsLocal)
                PerformIpCheck(filterContext);

            base.OnActionExecuting(filterContext);
        }

        private void PerformIpCheck(ActionExecutingContext filterContext)
        {
            string ipAddress = HttpContext.Current.Request.UserHostAddress;

            if (!IsAllowedIp(ipAddress.Trim()))
            {
                var routeValues = new RouteValueDictionary();
                routeValues.Add("Controller", RedirectController);
                routeValues.Add("Action", RedirectAction);

                filterContext.Result = new RedirectToRouteResult("Default", routeValues);
            }
        }

        private bool IsAllowedIp(string ipAddress)
        {
            var appSetting = ConfigurationManager.AppSettings["AllowedIPAddresses"];
            appSetting = string.IsNullOrWhiteSpace(appSetting) ? string.Empty : appSetting;

            if (appSetting == "N/A") return true;

            var allowedIpAddresses = appSetting.Split(',');

            foreach (var allowedIp in allowedIpAddresses)
            {
                if (allowedIp.Trim().Equals(ipAddress))
                    return true;
            }

            return false;
        }
    }
}