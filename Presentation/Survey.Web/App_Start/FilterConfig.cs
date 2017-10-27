using Survey.Core.Mvc.Filters.Exceptions;
using System.Web.Mvc;

namespace Survey.Web
{
    internal static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           
           //add error handle throughout the application
            filters.Add(new CustomHandleErrorAttribute());
            
        }
    }
}
