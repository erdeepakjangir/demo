using System;
using System.Web;

using Castle.Windsor;
using Castle.Windsor.Installer;
using System.Web.Mvc;
using Survey.Web.Plumbing;

namespace Survey.Web
{
    public class ContainerBootstrapper : IContainerAccessor, IDisposable
    {
        readonly IWindsorContainer container;

        ContainerBootstrapper(IWindsorContainer container)
        {
            this.container = container;
        }

        public IWindsorContainer Container
        {
            get { return container; }
        }

        public static ContainerBootstrapper Bootstrap()
        {
            var container = new WindsorContainer().Install(FromAssembly.InThisApplication());

            DependencyResolver.SetResolver(new WindsorDependencyResolver(container));

            return new ContainerBootstrapper(container);
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}