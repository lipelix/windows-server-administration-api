using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WinRemoteAdministration {
    /// <summary>
    /// This is route config which defines path to actual controllers and actions according to url format
    /// </summary>
    public class RouteConfig {

        /// <summary>
        /// Register routes configuration to routes collection
        /// </summary>
        /// <param name="routes">Collection of defined routes</param>
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
