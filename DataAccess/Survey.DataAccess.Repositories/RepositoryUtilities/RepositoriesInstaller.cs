namespace Survey.Data.DataAccess.Repositories.RepositoryUtilities
{
    using System.Data.Entity;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Survey.Core.Windsor;
 
    using Survey.Data.DataAccess.Entities;

    public class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var lifeStyleSetter = container.Resolve<ISetLifestyle>();

            container.Register(
                lifeStyleSetter.SetLifestyle(
                    Component.For<DbContext, SurveyToolDBContext>().ImplementedBy<SurveyToolDBContext>()));

            container.Register(
                lifeStyleSetter.SetLifestyle(
                    Classes.FromThisAssembly()
                        .Where(type => type.Name.EndsWith("Repository") || type.Name.EndsWith("UnitOfWork"))
                        .WithServiceDefaultInterfaces()));
        }
    }
}