using System.Web;
using System.Web.Mvc;

namespace DevStats
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            if (!HttpContext.Current.IsDebuggingEnabled)
                GlobalFilters.Filters.Add(new RequireHttpsAttribute());
        }
    }
}