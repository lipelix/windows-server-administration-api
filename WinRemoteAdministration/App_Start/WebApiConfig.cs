﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WinRemoteAdministration {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

//            config.Routes.MapHttpRoute(
//                name: "DefaultApi",
//                routeTemplate: "api/{controller}/{id}",
//                defaults: new { id = RouteParameter.Optional }
//            );

            config.Routes.MapHttpRoute(
                name: "DefaultActionsApi",
                routeTemplate: "api/{controller}/{action}/{param}",
                defaults: new { param = RouteParameter.Optional }
            );

            // Enforce HTTPS
//            config.Filters.Add(new WinRemoteAdministration.Filters.RequireHttpsAttribute());
        }
    }
}
