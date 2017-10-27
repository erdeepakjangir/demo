namespace Survey.Core.Mvc.Filters.Exceptions
{
    using System.Net;
    using System.Web.Mvc;
    using Contracts;
    using Castle.Core.Logging;
    using System.Web.Routing;

    /// <summary>
    /// </summary>
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    { 

        private  ILogger Logger 
        {
            get
            {
                return DependencyResolver.Current.GetService<ILogger>();
            }
   
        }

        /// <summary>
        /// </summary>
        private   IExceptionReporter ExceptionReporter
        {
            get
            {
                return DependencyResolver.Current.GetService<IExceptionReporter>();
            }
        }



        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var result = new ContentResult
                {
                    Content = filterContext.HttpContext.Request.IsLocal ? string.Format("An error has occurred while processing an ajax request: \n\n{0}", filterContext.Exception) : "An error has occurred while processing your request. We apologize for the inconvenience."
                };
                filterContext.Result = result;
                filterContext.ExceptionHandled = true;
            }
            //Log Error           
            ExceptionReporter.Report(filterContext.Exception, filterContext.Exception.Message, Logger);
            filterContext.ExceptionHandled = true;

            var routeValue = new RouteValueDictionary();
            routeValue.Add("controller", "Error");
            routeValue.Add("action", "Handled");
            #region Calculate Action Controller Error
            RouteValueDictionary lRoutes = new RouteValueDictionary(new
            {
                action = "Handled",
                controller = "Error",
                area = string.Empty
            });

            #endregion

            filterContext.Result = new RedirectToRouteResult(lRoutes);


        }
    }
}