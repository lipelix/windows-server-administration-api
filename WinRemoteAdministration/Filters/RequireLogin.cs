using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;

namespace WinRemoteAdministration.Filters {
    public class RequireLogin : ActionFilterAttribute, IActionFilter {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext) {
            HttpCookie token = filterContext.HttpContext.Request.Cookies.Get("access_token");
            HttpCookie role = filterContext.HttpContext.Request.Cookies.Get("role");

            if (token == null || role == null || !role.Value.Equals("supervisor")) {
                RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                redirectTargetDictionary.Add("action", "Index");
                redirectTargetDictionary.Add("controller", "Login");

                filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
            }

            this.OnActionExecuting(filterContext);
        }
    }
}