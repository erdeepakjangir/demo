


using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Services.Logging.Log4netIntegration;
using Castle.Windsor;
using log4net.Config;

[assembly: XmlConfigurator(Watch = true)]
namespace Survey.Web.Installers
{
    public class LoggerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            log4net.Config.XmlConfigurator.Configure();
            //Add Facility 
            container.AddFacility<LoggingFacility>(f => f.UseLog4Net());
            //Context for logging MDC properties
            container.Register(Component.For<Castle.Core.Logging.IContextProperties>().ImplementedBy<GlobalContextProperties>());
        }
    }
}