namespace Survey.Core.Exceptions
{
    using System;
    using System.Collections;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    using Survey.Core.Contracts;
    using Survey.Core.Enums;

    using Castle.Core.Logging;
    using System.Web.Mvc;

    /// <summary>
    /// </summary>
    /// <summary>
    /// </summary>
    public class ExceptionReporter : IExceptionReporter
    {
        /// <summary>
        /// </summary>

        #region Public Members
        
        private static ILogger Logger
        {
            get
            {
                return DependencyResolver.Current.GetService<ILogger>();
            }
        }


        private static IContextProperties ContextProperties
        {
            get
            {
                return DependencyResolver.Current.GetService<IContextProperties>();
            }
        }

        #endregion

        private readonly string _ipAddress;

        /// <summary>
        /// </summary>
        private readonly IWebUtility _webUtil;

        public ExceptionReporter()
        {

        }
        /// <summary>
        /// </summary>
        /// <param name="ipAddress"></param>
        public ExceptionReporter(string ipAddress)
        {
            _ipAddress = ipAddress;
        }

        /// <summary>
        /// </summary>
        /// <param name="webUtil"></param>
        public ExceptionReporter(IWebUtility webUtil)
        {
            _webUtil = webUtil;
        }

        /// <summary>
        /// </summary>
        /// <param name="ex"></param>
        public void Report(Exception ex)
        {
            Report(ex, string.Empty, Logger);
        }

        /// <summary>
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="logger"></param>
        public void Report(Exception ex, ILogger logger)
        {
            Report(ex, string.Empty, logger);
        }

        /// <summary>
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="details"></param>
        public void Report(Exception ex, string details)
        {
            SetExceptionInLoggerContext(ex, details, _ipAddress ?? _webUtil.IpAddress);
            Task.Factory.StartNew(() => LogException(Logger, LoggerType.Error, details, ex));
        }

        /// <summary>
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="details"></param>
        /// <param name="logger"></param>
        public void Report(Exception ex, string details, ILogger logger)
        {
            SetExceptionInLoggerContext(ex, details, _ipAddress ?? _webUtil.IpAddress);
            Task.Factory.StartNew(() => LogException(logger, LoggerType.Error, details, ex));
        }

        /// <summary>
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="details"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public string GenerateErrorText(string details)
        {
            string specificDetails = GetSpecificDetails(details);

            return FormatExceptionMessage(specificDetails);
        }

        /// <summary>
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        private static string GetSpecificDetails(string details)
        {
            var specificDetails = new StringBuilder();

            try
            {
                if (!string.IsNullOrEmpty(details))
                {
                    specificDetails.Append("Details : ");
                    specificDetails.AppendLine(details);
                }

                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && HttpContext.Current.User.Identity.IsAuthenticated)
                    {

                        specificDetails.Append("UserName: ");
                        specificDetails.AppendLine(Convert.ToString(ContextProperties["UserName"]));

                        specificDetails.Append("Role: ");
                        specificDetails.AppendLine(Convert.ToString(ContextProperties["Role"]));

                        

                    }
                }
            }
            catch (Exception errorReporterEx)
            {
                Logger.Error("GetSpecificDetails threw an error!", errorReporterEx);
            }
            return specificDetails.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="specificDetails"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        private static string FormatExceptionMessage(string specificDetails)
        {
            var errorText = new StringBuilder();
            errorText.Append("ERROR MESSAGE: ");
            errorText.AppendLine(Convert.ToString(ContextProperties["exceptionMessage"]));

            errorText.AppendLine(string.Empty);
            errorText.Append("ERROR AT: ");
            errorText.AppendLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
            errorText.AppendLine(string.Empty);
            if (!string.IsNullOrEmpty(specificDetails))
            {
                errorText.AppendLine("SPECIFIC DETAILS: ");
                errorText.AppendLine(specificDetails);
                errorText.AppendLine(string.Empty);
            }

            errorText.AppendLine("GENERAL DETAILS: ");
            if (HttpContext.Current != null)
            {
                errorText.Append("Page Name: ");
                errorText.AppendLine(Convert.ToString(ContextProperties["pageName"]));

                errorText.AppendLine(Convert.ToString(ContextProperties["urlReferrer"]));


                errorText.Append("Browser Details: ");

                errorText.AppendLine(Convert.ToString(ContextProperties["browserDetails"]));

                errorText.Append("IP Address: ");
                errorText.AppendLine(Convert.ToString(ContextProperties["ipAddress"]));

            }
            errorText.Append("Machine Name: ");
            errorText.AppendLine(Convert.ToString(ContextProperties["machineName"]));

            errorText.AppendLine("EXCEPTION DETAILS: ");

            errorText.Append("Reporting Method: ");
            errorText.AppendLine(Convert.ToString(ContextProperties["reportingMethod"]));
            errorText.AppendLine(string.Empty);
            errorText.Append("Exception Method: ");
            errorText.AppendLine(Convert.ToString(ContextProperties["exceptionMethod"]));


            errorText.Append("Error Source: ");
            errorText.AppendLine(Convert.ToString(ContextProperties["exceptionSource"]));

            errorText.Append("Error Type: ");
            errorText.AppendLine(Convert.ToString(ContextProperties["exceptionType"]));

            errorText.Append("Exception Text: ");
            errorText.AppendLine(Convert.ToString(ContextProperties["exceptionText"]));

            errorText.Append("Exception Text Detail: ");
            errorText.AppendLine(Convert.ToString(ContextProperties["exceptionTextDetail"]));

            return errorText.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="details"></param>
        /// <param name="ipAddress"></param>
        private static void SetExceptionInLoggerContext(Exception ex, string details, string ipAddress)
        {
            try
            {
                //initialize context

                ContextProperties["urlReferrer"] = string.Empty;
                ContextProperties["browserDetails"] = string.Empty;
                ContextProperties["ipAddress"] = string.Empty;
                ContextProperties["pageName"] = string.Empty;
                ContextProperties["exceptionMethod"] = string.Empty;
                ContextProperties["exceptionMessage"] = string.Empty;
                ContextProperties["exceptionSource"] = string.Empty;
                ContextProperties["exceptionType"] = string.Empty;
                ContextProperties["exceptionText"] = string.Empty;
                ContextProperties["exceptionTextDetail"] = string.Empty;
                //

                ContextProperties["machineName"] = Environment.MachineName;
                ContextProperties["reportingMethod"] = GetReportingMethod();

                if (HttpContext.Current != null)
                {
                    //userId & marketId are set at start
                    //if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && HttpContext.Current.User.Identity.IsAuthenticated)
                    //{
                    //    ContextProperties["userId"] = GetUserId();
                    //    ContextProperties["marketId"] = GetMarketID();
                    //}

                    HttpRequest request = HttpContext.Current.Request;

                    ContextProperties["pageName"] = request.RawUrl;


                    if (request.UrlReferrer != null)
                    {
                        ContextProperties["urlReferrer"] = request.UrlReferrer.ToString();
                    }

                    string browserDetails = request.ServerVariables["HTTP_USER_AGENT"];

                    ContextProperties["browserDetails"] = browserDetails;


                    ContextProperties["ipAddress"] = ipAddress;
                }



                StringBuilder contextMessage = new StringBuilder();


                if (ex != null)
                {
                    MethodBase method = ex.TargetSite;
                    if (method != null && method.DeclaringType != null)
                    {
                        ContextProperties["exceptionMethod"] = string.Format("{0}.{1}", method.DeclaringType.FullName, method.Name);
                    }

                    ContextProperties["exceptionMessage"] = ex.Message;
                    ContextProperties["exceptionText"] = details;
                    ContextProperties["exceptionSource"] = ex.Source;
                    ContextProperties["exceptionType"] = ex.GetType().ToString();

                    contextMessage.AppendLine(ex.ToString());

                    if (ex.Data.Count > 0)
                    {
                        contextMessage.AppendLine("Stack Trace :");

                        foreach (DictionaryEntry de in ex.Data)
                        {
                            contextMessage.AppendFormat("Key: {0} Value: {1}", de.Key, de.Value);
                            contextMessage.AppendLine();
                        }
                    }
                    contextMessage.Append("Error Base Exception: ");
                    contextMessage.AppendLine(ex.GetBaseException().ToString());
                    Exception currentExp = ex.InnerException;

                    while (currentExp != null && currentExp.InnerException != null)
                    {
                        contextMessage.Append("Inner Exception: ");
                        contextMessage.AppendLine(currentExp.InnerException.ToString());
                        currentExp = currentExp.InnerException;
                    }
                }

                var efValidationError = ex as DbEntityValidationException;
                if (efValidationError != null)
                {
                    foreach (DbEntityValidationResult dbEntityValidationResult in efValidationError.EntityValidationErrors)
                    {
                        contextMessage.Append("EF Entity: ");
                        contextMessage.AppendLine(dbEntityValidationResult.GetType().Name);

                        foreach (DbValidationError dbValidationError in dbEntityValidationResult.ValidationErrors)
                        {
                            contextMessage.Append("Property: ");
                            contextMessage.Append(dbValidationError.PropertyName);
                            contextMessage.Append(" - Error: ");
                            contextMessage.AppendLine(dbValidationError.ErrorMessage);
                        }
                    }
                }

                ContextProperties["exceptionTextDetail"] = contextMessage.ToString();


            }
            catch (Exception exception)
            {
                Logger.Error("Exception occurred while setting context", exception);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="loggerType"></param>
        /// <param name="errorText"></param>
        private static void LogException(ILogger logger, LoggerType loggerType, string errorText, Exception ex)
        {
            try
            {
                switch (loggerType)
                {
                    case LoggerType.Debug:
                        logger.Debug(errorText);
                        break;
                    case LoggerType.Info:
                        logger.Info(errorText);
                        break;
                    case LoggerType.Warn:
                        logger.Warn(errorText);
                        break;
                    case LoggerType.Error:
                        logger.Error(errorText, ex);
                        break;
                    case LoggerType.Fatal:
                        logger.Fatal(errorText, ex);
                        break;
                    default:
                        logger.Error(errorText, ex);
                        break;
                }
            }
            catch (Exception)
            {
                //	Intentionally logging is skipped
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private static string GetReportingMethod()
        {
            try
            {
                MethodBase method;
                int stackDepth = 1;

                // Loop until we find a stack level that's outside of ErrorDAL and ErrorBAL (or throw an error)
                do
                {
                    // 0 == this private method; 1 == the method on this class which called it
                    // So we'll effectively start looking at stackDepth = 2
                    stackDepth++;
                    method = new StackFrame(stackDepth).GetMethod();
                } while (method.DeclaringType != null && (method.DeclaringType.FullName.Contains("ErrorDAL") || method.DeclaringType.FullName.Contains("ErrorBAL") || method.DeclaringType.FullName.Contains("ErrorBLL")));
                if (method.DeclaringType == null)
                {
                    return "";
                }
                return string.Format("{0}.{1}", method.DeclaringType.FullName, method.Name);
            }
            catch
            {
                return "UNKNOWN";
            }
        }

    }
}