namespace Survey.Web
{
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Routing;
	using Business.Services.ServiceUtilities;
	using WebUtilities;
    using System.Web.Optimization;

    public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new RazorViewEngine());
			RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            AutoMapper.Mapper.Initialize(config =>
			{
				config.AddProfile<AutoMapperViewModelProfile>();
				config.AddProfile<AutoMapperServiceProfile>();
			});

            System.Web.Helpers.AntiForgeryConfig.UniqueClaimTypeIdentifier =
            System.Security.Claims.ClaimTypes.NameIdentifier;
        }
	}
}