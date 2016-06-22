using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using WinRemoteAdministration.Models;

namespace WinRemoteAdministration.Providers {

    /// <summary>
    /// Authorization server provider check request on authorization token and provides valid token or error message. 
    /// </summary>
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider {
        /// <summary>
        /// Validates the client authentication.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Task.</returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context) {        
            context.Validated();
        }
        /// <summary>
        /// Grants the resource owner credentials.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Task.</returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context) {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (AuthRepository repo = new AuthRepository()) {
                IdentityUser user = await repo.FindUser(context.UserName, context.Password);
                bool isAndroid = context.Request.Headers["User-Agent"].ToString().Contains("Android");

                if (user == null) {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                } else if (!repo.IsInRole(user.UserName, "admin") && isAndroid) {
                    context.SetError("invalid_grant", "The user is not admin.");
                    return;
                } else if (!repo.IsInRole(user.UserName, "supervisor") && !isAndroid) {
                    context.SetError("invalid_grant", "The user is not supervisor.");
                    return;
                }
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            context.Validated(identity);
        }
    }
}