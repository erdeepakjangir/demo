namespace Survey.Web.Controllers
{
    using AutoMapper;
    using System.Collections.Generic;
    using System.Web;
    using System.Linq;
    using Core.Contracts;
    using System.Web.Mvc;
    using Castle.Windsor;
    using System;
    using Castle.Core.Logging;
    using System.Security.Claims;


    using Survey.Business.Entities.Enums;


    /// <summary>
    /// Base controller for all authorized controller
    /// </summary>
   [Authorize]
    public abstract class BaseController : Controller
    {
        protected string RootUrl => (Request.Url == null ? string.Empty : Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~/"));

        private readonly IWindsorContainer _container;
        private IExceptionReporter _exceptionReporter;

        private IWebUtility _webUtil;

        protected BaseController()
        {
            _container = WindsorActivator.Bootstrapper.Container;


        }

        #region Properties



        public string ControllerName
        {
            get { return this.RouteData.Values["controller"] != null ? this.RouteData.Values["controller"].ToString() : string.Empty; }
        }

        public string ActionName
        {
            get { return this.RouteData.Values["action"] != null ? this.RouteData.Values["action"].ToString() : string.Empty; }
        }


        protected IWebUtility WebUtil
        {
            get { return _webUtil ?? (_webUtil = DependencyResolver.Current.GetService<IWebUtility>()); }
        }

        protected IExceptionReporter ExceptionReporter
        {
            get { return _exceptionReporter ?? (_exceptionReporter = DependencyResolver.Current.GetService<IExceptionReporter>()); }

        }

        public IContextProperties ContextProperties
        {
            get; set;
        }

        private ILogger logger = NullLogger.Instance;

        public ILogger Logger
        {
            get { return logger; }
            set { logger = value; }
        }


        /// <summary>
        /// Get the user Name
        /// </summary>
        protected string UserIdentityName
        {
            get
            {
                if (HttpContext.User.Identity == null || (HttpContext.User.Identity != null && string.IsNullOrEmpty(HttpContext.User.Identity.Name)))
                {
                    return string.Empty;
                }
                return System.Web.HttpContext.Current.User.Identity.Name;
            }
        }

        protected string LoggedInUserName
        {
            get
            {
                if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
                {
                    return GetClaimValue(this.HttpContext, ClaimTypes.WindowsAccountName);
                }
                return string.Empty;
            }
        }




        protected long LoggedInUserId
        {
            get
            {
                if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated && HttpContext.User.IsInRole(UserRole.Student.ToString()))
                {
                    return Convert.ToInt64(GetClaimValue(this.HttpContext, ClaimConfiguration.UserId.ToString()));
                }
                return 0;
            }
            private set {; }
        }


        protected string LoggedInUserFacultyCode
        {
            get
            {
                return GetClaimValue(ClaimConfiguration.FacultyCode.ToString());
            }
        }


        private string GetClaimValue(string ClaimId)
        {

            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return GetClaimValue(this.HttpContext, ClaimId);
            }
            return null;

        }


        #endregion

        #region Protected Methods


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_webUtil != null)
                {
                    _container.Release(_webUtil);
                }
                if (_exceptionReporter != null)
                {
                    _container.Release(_exceptionReporter);
                }
            }
        }


        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (!Request.IsAjaxRequest() && User != null && User.Identity.IsAuthenticated && User.IsInRole(UserRole.Student.ToString()))
            {
                ViewBag.StudentStageId = GetClaimValue(ClaimConfiguration.StageId.ToString());
                ViewBag.StudentQualAim = GetClaimValue(ClaimConfiguration.QualAim.ToString());
                ViewBag.StudentMode = GetClaimValue(ClaimConfiguration.Mode.ToString());
                ViewBag.StudentCourseCode = GetClaimValue(ClaimConfiguration.CourseCode.ToString());
                ViewBag.StudentCourseTitle = GetClaimValue(ClaimConfiguration.CourseTitle.ToString());
            }
        }

        protected static string GetClaimValue(HttpContextBase httpContext, string type)
        {
            if (((ClaimsIdentity)httpContext.User.Identity).Claims.FirstOrDefault(claim => claim.Type.Equals(type)) != null)
                return ((ClaimsIdentity)httpContext.User.Identity).Claims.FirstOrDefault(claim => claim.Type.Equals(type)).Value;
            else
                return string.Empty;
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            // SetCultureInfo();
            SetUserContextInfo();
            return base.BeginExecuteCore(callback, state);
        }


        /// <summary>
        /// Set the context to be used by logger
        /// </summary>
        protected virtual void SetUserContextInfo()
        {
            ContextProperties["UserName"] = GetClaimValue(this.HttpContext, ClaimTypes.WindowsAccountName);
            ContextProperties["Role"] = GetClaimValue(this.HttpContext, ClaimTypes.Role);
        }


        #endregion
    }
}