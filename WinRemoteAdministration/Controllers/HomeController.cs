using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinRemoteAdministration.Controllers {

    /// <summary>
    /// Controller processing Homepage of web interface.
    /// </summary>
    public class HomeController : Controller {

        /// <summary>
        /// Render View of Homepage.
        /// </summary>
        /// <returns>Html page with homepage.</returns>
        public ActionResult Index() {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
