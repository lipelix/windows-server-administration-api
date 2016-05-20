using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace WinRemoteAdministration.Controllers {

    public class LoginController : Controller {
        // GET: Login
        public ActionResult Index() {
            return View();
        }

        public ActionResult Logout() {
            Response.Cookies["access_token"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["logged_user"].Expires = DateTime.Now.AddDays(-1);

            return RedirectToAction("Index", "Home");
        }
    }
}