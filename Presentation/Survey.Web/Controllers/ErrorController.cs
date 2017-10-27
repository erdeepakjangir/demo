using Castle.Core.Logging;
using Survey.Core.Contracts;
using System;
using System.Configuration;

using System.Web.Mvc;

namespace Survey.Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error

        #region Private Members 
         
        private   IExceptionReporter ExceptionReporter
        {
            get
            {
                return DependencyResolver.Current.GetService<IExceptionReporter>();
            }
        }
        #endregion
        #region Public Methods

        ////// Will be called by authorize filter up on unauthorized access//////
        public ActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        /// <summary>
        /// Handle exception and present View
        /// </summary>
        /// <returns></returns>
        public ActionResult Handled()
        {
            HttpContext.Response.Clear();
            HttpContext.ClearError();

            try
            {
            //override value in production
            string environment = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Environment"]) ? string.Empty : ConfigurationManager.AppSettings["Environment"].ToString();
            if (environment.ToLower() == "production")
            {
                ViewData["ProductionException"] = Resources.ModelValidations.Exception_Production_Message;
                 ViewData["DevelopmentException"] = "";
            }
            else
            {
                    ViewData["ProductionException"] = "";
                    ViewData["DevelopmentException"] = ExceptionReporter.GenerateErrorText(Resources.ModelValidations.Exception_Development_Message).Replace("\r\n", "</br>");
                    //Resources.ModelValidations.Exception_Production_Message;
             }
            }
            catch (Exception )
            {

                
            }
            return View();
        }
        #endregion
    }
}