namespace Survey.Web.Installers
{
    using System.Web;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Core.Caching;
    using Core.Contracts;
    using Core.Exceptions;
    using Core.Utilities;

    public class UtilityInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<HttpContextBase>().LifeStyle.PerWebRequest.UsingFactoryMethod(() => new HttpContextWrapper(HttpContext.Current)));
            container.Register(Component.For<IHttpContextBaseWrapper>().ImplementedBy<HttpContextBaseWrapper>().LifestylePerWebRequest());
            container.Register(Component.For<IWebUtility>().ImplementedBy<WebUtility>().LifestylePerWebRequest());
            container.Register(Component.For<IExceptionReporter>().ImplementedBy<ExceptionReporter>().LifestylePerWebRequest());
            container.Register(Component.For<ICacheProvider>().ImplementedBy<ServiceCacheProvider>().LifestylePerWebRequest());
            //container.Register(Component.For<IUserStore<ApplicationUser>>().ImplementedBy<ApplicationUserStore<ApplicationUser>>().LifestylePerWebRequest());
           // container.Register(Component.For<ApplicationUserManager>().ImplementedBy<ApplicationUserManager>().LifestylePerWebRequest());
        }
    }
}