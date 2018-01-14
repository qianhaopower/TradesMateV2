using DataService.AuthProviders;
using DataService.Providers;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Ninject;
using Owin;

namespace DataService
{
    public partial class Startup
    {
        private void ConfigureOAuth(IAppBuilder app, IKernel kernel)
        {
           
            app.UseOAuthAuthorizationServer(
                kernel.Get<MyOAuthAuthorizationServerOptions>().GetOptions());

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
   