namespace Survey.Business.Services.ServiceUtilities
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor; 
    using Core.Windsor;
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var lifeStyleSetter = container.Resolve<ISetLifestyle>();

            container.Register(
                 lifeStyleSetter.SetLifestyle(
                     Classes.FromThisAssembly()
                         .Where(type => type.Name.EndsWith("Service"))
                         .WithServiceDefaultInterfaces()));

        }
    }
}