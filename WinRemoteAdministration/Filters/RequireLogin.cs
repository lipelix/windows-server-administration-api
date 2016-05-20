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

            if (token == null) {
                RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary();
                redirectTargetDictionary.Add("action", "Index");
                redirectTargetDictionary.Add("controller", "Login");

                filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
            }

            this.OnActionExecuting(filterContext);
        }
    }
}