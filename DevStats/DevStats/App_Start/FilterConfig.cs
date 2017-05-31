using System.Web.Mvc;
using DevStats.Filters;

namespace DevStats
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new IPAccessAttribute());
        }
    }
}