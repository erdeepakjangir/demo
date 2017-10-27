using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Survey.Web.Startup))]

namespace Survey.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
