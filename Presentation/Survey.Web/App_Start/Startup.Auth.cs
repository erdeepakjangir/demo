
namespace Survey.Web
{
    using System;
    using System.Web;
    using Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin;


    using Microsoft.Owin.Infrastructure;

    using Microsoft.AspNet.Identity;

    /// <summary>
    /// configure startup 
    /// </summary>
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);


            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });


        }


    }
}
