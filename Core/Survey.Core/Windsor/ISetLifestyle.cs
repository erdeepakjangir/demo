namespace Survey.Core.Windsor
{
    using Castle.MicroKernel.Registration;

    public interface ISetLifestyle
	{
		BasedOnDescriptor SetLifestyle(BasedOnDescriptor descriptor);
		ComponentRegistration<T> SetLifestyle<T>(ComponentRegistration<T> descriptor) where T : class;
	}
}