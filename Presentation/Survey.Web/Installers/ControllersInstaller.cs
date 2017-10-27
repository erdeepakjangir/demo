using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Survey.Web.Installers
{
    using Core.Windsor;
    using Plumbing;

    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ISetLifestyle>().ImplementedBy<WebLifestyle>());
            container.Register(Classes.FromThisAssembly().BasedOn<IController>().If(c => c.Name.EndsWith("Controller")).LifestylePerWebRequest());
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));
        }
    }
}