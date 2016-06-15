using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WinRemoteAdministration {
    /// <summary>
    /// Cofiguration of WebApi part of application
    /// </summary>
    public static class WebApiConfig {

        /// <summary>
        /// Register Http configuration such as routing for api calls
        /// </summary>
        /// <param name="config">Http configuration settings</param>
        public static void Register(HttpConfiguration config) {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultActionsApi",
                routeTemplate: "api/{controller}/{action}/{param}",
                defaults: new { param = RouteParameter.Optional }
            );
        }
    }
}
