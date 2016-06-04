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

    public class LoginController : Controller {
        // GET: Login
        public ActionResult Index() {
            return View();
        }

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

        public ActionResult Logout() {
            Response.Cookies["access_token"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["logged_user"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["role"].Expires = DateTime.Now.AddDays(-1);

            return RedirectToAction("Index", "Home");
        }
    }
}