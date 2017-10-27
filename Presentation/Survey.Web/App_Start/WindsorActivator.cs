using Survey.Web;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(WindsorActivator), "PreStart")]
[assembly: ApplicationShutdownMethod(typeof(WindsorActivator), "Shutdown")]

namespace Survey.Web
{
    public static class WindsorActivator
    {
        public static ContainerBootstrapper Bootstrapper;

        public static void PreStart()
        {
            Bootstrapper = ContainerBootstrapper.Bootstrap();
        }
        
        public static void Shutdown()
        {
            if (Bootstrapper != null)
                Bootstrapper.Dispose();
        }
    }
}