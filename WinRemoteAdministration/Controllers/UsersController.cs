using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WinRemoteAdministration.Models;

namespace WinRemoteAdministration.Controllers {
    public class UsersController : Controller {
        private AuthRepository repo = null;

        public UsersController() {
            repo = new AuthRepository();
        }

        public ActionResult Index() {
            return View();
        }

        public ActionResult Roles() {
            // prepopulat roles for the view dropdown
            var list = repo.ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr =>new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            return View();
        }

        public ActionResult RoleAddToUser(string UserName, string RoleName) {
            repo.RoleAddToUser(UserName, RoleName);
            ViewBag.ResultMessage = "Role created successfully !";

            var list = repo.ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("Roles");
        }

        public ActionResult DeleteRoleForUser(string UserName, string RoleName) {            
            repo.RemoveFromRole(UserName, RoleName);

            var list = repo.ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("Roles");
        }


        // POST api/Account/Register
        [Filters.RequireHttps]
        public ActionResult Register(UserRegModel userModel) {
            if (!ModelState.IsValid) {
                Response.StatusCode = 400;
            }

            IdentityResult result = repo.RegisterUser(userModel);

            if (result == null) {
                Response.StatusCode = 401;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUsers() {
            var allusers = repo.GetAllUsers();
            var users = from user in allusers
                        select new {
                            DT_RowId = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            Roles = repo.GetRoles(user.UserName)
                        };

            return Json(new { data = users }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUser(String id) {            
            var user = repo.FindUser(id);
//            var users = from user in allusers
//                        select new {
//                            UserName = user.UserName,
//                            Email = user.Email
//                        };

            return Json(new { data = user }, JsonRequestBehavior.AllowGet);
        }
    }
}