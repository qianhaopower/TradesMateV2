using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Facebook;
using DataService.Infrastructure;
using EF.Data;

[assembly: OwinStartup("DataService",typeof(DataService.Startup))]
namespace DataService
{
    public partial class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions GoogleAuthOptions { get; private set; }
        public static FacebookAuthenticationOptions FacebookAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(EFDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            // app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            ConfigureAuth(app);
            ConfigureNinject(app);//note both ninject and web api are configured here. Challange results will not work if this is before configure Oauth.
            AutoMapperConfig.RegisterMappings();
        }
    }
}


