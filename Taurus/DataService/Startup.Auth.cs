using DataService.AuthProviders;
using DataService.Providers;
using EF.Data;
using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using Owin;
using System;
using System.Configuration;

namespace DataService
{
    public partial class Startup
    {
        private void ConfigureAuth(IAppBuilder app)
        {
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);

            var options = new OAuthBearerAuthenticationOptions();
            OAuthBearerOptions = options;
            app.UseOAuthBearerAuthentication(options);
            //app.UseOAuthAuthorizationServer(kernel.Get<MyOAuthAuthorizationServerOptions>().GetOptions());
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true, //TODO: HTTPS
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider( new AuthRepository(new EFDbContext())),
            });

            //Configure Google External Login
            GoogleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = ConfigurationManager.AppSettings["GoogleClientId"],
                ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"],
                Provider = new GoogleAuthProvider()
            };
            app.UseGoogleAuthentication(GoogleAuthOptions);

            //Configure Facebook External Login
            FacebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = ConfigurationManager.AppSettings["FacebookAppId"],
                AppSecret = ConfigurationManager.AppSettings["FacebookAppSecret"],
                Provider = new FacebookAuthProvider()
            };
            app.UseFacebookAuthentication(FacebookAuthOptions);

        }

    }
}
   