namespace Survey.Web.WebUtilities
{
    using System;
    using System.Web.Mvc;
    public class WebViewPageBase<TModel> : WebViewPage<TModel> where TModel : class
	{
        protected string RootUrl => (Request.Url == null ? string.Empty : Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~/"));
        protected string UserIdentityName
        {
            get
            {
                if (ViewContext.HttpContext.User.Identity == null || (ViewContext.HttpContext.User.Identity != null && string.IsNullOrEmpty(ViewContext.HttpContext.User.Identity.Name)))
                {
                    return string.Empty;
                }
                return System.Web.HttpContext.Current.User.Identity.Name;
            }
        }

		public override void Execute()
		{
		}
	}
}