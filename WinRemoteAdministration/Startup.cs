using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using WinRemoteAdministration.Models;
using WinRemoteAdministration.Providers;

[assembly: OwinStartup(typeof(WinRemoteAdministration.Startup))]

namespace WinRemoteAdministration {

    public class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureOAuth(app);
        }

        public void ConfigureOAuth(IAppBuilder app) {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions() {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new AuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}
