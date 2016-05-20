using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WinRemoteAdministration.Filters;
using WinRemoteAdministration.Models;

namespace WinRemoteAdministration.Controllers {

    [RequireLogin]
    public class UsersController : Controller {
        private AuthRepository repo = null;

        public UsersController() {
            repo = new AuthRepository();
        }

        public ActionResult Index() {
            return View();
        }


        public ActionResult Roles(string UserName) {
            // prepopulat roles for the view dropdown
            BindUserAndRoles(UserName);

            return View();
        }

        private void BindUserAndRoles(string UserName) {
            var list = repo.ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            var user = repo.FindUserByName(UserName);
            ViewBag.User = user;
            var userRoles = repo.GetRoles(UserName);
            ViewBag.UserName = UserName;

            if (user != null && userRoles.Any()) {
                ViewBag.UserRoles = userRoles.Aggregate((x, y) => x + ", " + y);
            }
            else if (user == null) {
                ViewBag.ResultType = "danger";
                ViewBag.ResultMessage = "User " + UserName + " doesn´t exist";
            }
        }

        public ActionResult RoleAddToUser(string UserName, string RoleName) {
            if (!ValidateInputs(UserName, RoleName)) {
                BindUserAndRoles(UserName);
                return View("Roles");
            }

            if (repo.IsInRole(UserName, RoleName)) {
                ViewBag.ResultType = "danger";
                ViewBag.ResultMessage = "User " + UserName + " is already in role " + RoleName;
            }
            else {
                repo.RoleAddToUser(UserName, RoleName);
                ViewBag.ResultType = "success";
                ViewBag.ResultMessage = "User " + UserName + " has been added to role " + RoleName;
            }
//
//            ViewBag.User = repo.FindUserByName(UserName);
//            var list = repo.ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
//            ViewBag.Roles = list;

            BindUserAndRoles(UserName);

            return View("Roles");
        }

        private bool ValidateInputs(string UserName, string RoleName) {
            if (UserName.IsNullOrWhiteSpace() || RoleName.IsNullOrWhiteSpace()) {
                ViewBag.ResultType = "warning";
                ViewBag.ResultMessage = "Please fill inputs properly";
                return false;
            }

            return true;
        }

        public ActionResult DeleteRoleForUser(string UserName, string RoleName) {
            if (!ValidateInputs(UserName, RoleName)) {
                BindUserAndRoles(UserName);
                return View("Roles");
            }

            if (!repo.IsInRole(UserName, RoleName)) {
                ViewBag.ResultType = "danger";
                ViewBag.ResultMessage = "User " + UserName + " is not in role " + RoleName;
            }
            else {
                repo.RemoveFromRole(UserName, RoleName);
                ViewBag.ResultType = "success";
                ViewBag.ResultMessage = "User " + UserName + " has been removed from " + RoleName;
            }
//
//            ViewBag.User = repo.FindUserByName(UserName);
//            var list = repo.ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
//            ViewBag.Roles = list;

            BindUserAndRoles(UserName);

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
            UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);

            var users = from user in allusers
                        select new {
                            DT_RowId = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            Roles = repo.GetRoles(user.UserName),
                            EditRolesHref = u.Action("Roles", "Users", new {
                                UserName = user.UserName
                            })
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