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

namespace DataService
{
    public partial class Startup
    {
        private void ConfigureAuth(IAppBuilder app)
        {
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);


            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
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
            googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "154249831186-0ieu1cvimgqnbbu7h7ul2d5cmc0ldun9.apps.googleusercontent.com",
                ClientSecret = "Qd9Yh5ch2gdARXkw5JPMXm02",
                Provider = new GoogleAuthProvider()
            };
            app.UseGoogleAuthentication(googleAuthOptions);

            //Configure Facebook External Login
            facebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = "202526469786564",
                AppSecret = "bd798ef4d55ee25365b9a28e23ac9f00",
                Provider = new FacebookAuthProvider()
            };
            app.UseFacebookAuthentication(facebookAuthOptions);

        }

    }
}
   