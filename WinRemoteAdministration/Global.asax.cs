﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WinRemoteAdministration.Filters;

namespace WinRemoteAdministration {

    /// <summary>
    /// Global application configuration.
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication {

        /// <summary>
        /// Register configuration when application starts.
        /// </summary>
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configuration.MessageHandlers.Add(new ApiLogHandler());
        }
    }
}
