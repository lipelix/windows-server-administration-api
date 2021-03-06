﻿using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WinRemoteAdministration.Filters {

    /// <summary>
    /// Filter for enforcement of secured transfer.
    /// </summary>
    public class RequireHttpsAttribute : AuthorizationFilterAttribute {
        /// <summary>
        /// Called when [authorization] is proccesed.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnAuthorization(HttpActionContext actionContext) {
            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps) {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden) {
                    ReasonPhrase = "HTTPS Required"
                };
            }
            else {
                base.OnAuthorization(actionContext);
            }
        }
    }

}