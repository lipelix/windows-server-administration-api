using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity.Owin;
using WinRemoteAdministration.Models;

namespace WinRemoteAdministration.Controllers {

    /// <summary>
    /// Processing data related to autorization of web interface users.
    /// </summary>
    public class LoginController : Controller {
        // GET: Login
        /// <summary>
        /// Render View of Login.
        /// </summary>
        /// <returns>Html page with Login form.</returns>
        public ActionResult Index() {
            return View();
        }

        /// <summary>
        /// Check if user is in supervisor role.
        /// </summary>
        /// <param name="username">Name of user.</param>
        /// <returns>Json object with true or false parameter.</returns>
        public ActionResult isSupervisor(string username) {
            AuthRepository repo = new AuthRepository();
            var Roles = repo.GetRoles(username);

            if (Roles == null) {
                return Json(new {isSupervisor = false , error = "User " + username + " is not in supervisor role."}, JsonRequestBehavior.AllowGet);
            }
            else if (Roles.Contains("supervisor")) {
                return Json(new {isSupervisor = true}, JsonRequestBehavior.AllowGet);
            }

            return Json(new { isSupervisor = false, error = "User " + username + " is not in supervisor role." }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Perform instant logout of user.
        /// </summary>
        /// <returns>Redirect user to Homepage.</returns>
        public ActionResult Logout() {
            Response.Cookies["access_token"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["logged_user"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["role"].Expires = DateTime.Now.AddDays(-1);

            return RedirectToAction("Index", "Home");
        }
    }
}