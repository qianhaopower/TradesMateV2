using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataService.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;

namespace DataService.AuthProviders
{
    public class MyOAuthAuthorizationServerOptions : IOAuthAuthorizationServerOptions
    {
        private IOAuthAuthorizationServerProvider _provider;
        private IAuthenticationTokenProvider _tokenProvider;

        public MyOAuthAuthorizationServerOptions(IAuthenticationTokenProvider tProvider,
            IOAuthAuthorizationServerProvider provider)
        {
            _provider = provider;
            _tokenProvider = tProvider;
        }
        public OAuthAuthorizationServerOptions GetOptions()
        {
            return new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true, //TODO: HTTPS
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = _provider,
                RefreshTokenProvider = _tokenProvider
            };
        }
    }
}