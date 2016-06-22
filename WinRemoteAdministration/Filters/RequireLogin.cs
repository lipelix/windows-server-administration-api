using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;

namespace WinRemoteAdministration.Filters {

    /// <summary>
    /// Filter for enforcement that user has to be logged in as supervisor, to use specific action.
    /// </summary>
    public class RequireLogin : ActionFilterAttribute, IActionFilter {
        /// <summary>
        /// Called when any Web Controller action is procceses. Check if user is logged and has permisions.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
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