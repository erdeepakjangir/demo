namespace Survey.Web.Installers
{
    using Castle.MicroKernel.Registration;
    using Survey.Core.Windsor;

    public class WebLifestyle : ISetLifestyle
	{
		public BasedOnDescriptor SetLifestyle(BasedOnDescriptor descriptor)
		{
			return descriptor.LifestylePerWebRequest();
		}

		public ComponentRegistration<T> SetLifestyle<T>(ComponentRegistration<T> descriptor) where T : class
		{
			return descriptor.LifestylePerWebRequest();
		}
	}
}