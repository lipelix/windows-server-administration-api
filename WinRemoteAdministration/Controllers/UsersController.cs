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

    /// <summary>
    /// Handling operations to manipulate and manage users and their roles.
    /// </summary>
    [RequireLogin]
    public class UsersController : Controller {
        private AuthRepository repo = null;

        public UsersController() {
            repo = new AuthRepository();
        }

        /// <summary>
        /// Render View of users administration.
        /// </summary>
        /// <returns>Html page with users administration.</returns>
        public ActionResult Index() {
            return View();
        }

        /// <summary>
        /// Render View with user role management.
        /// </summary>
        /// <param name="UserName">Name of user to manage.</param>
        /// <returns>Html page with user role administration.</returns>
        public ActionResult Roles(string UserName) {
            // prepopulat roles for the view dropdown
            BindUserAndRoles(UserName);

            return View();
        }

        /// <summary>
        /// Bind information about user role membership to view.
        /// </summary>
        /// <param name="UserName">User name whose info will be bind.</param>
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

        /// <summary>
        /// Add user to specified role.
        /// </summary>
        /// <param name="UserName">Name of user.</param>
        /// <param name="RoleName">Name of role to which user will be added.</param>
        /// <returns>Html page with user role administration.</returns>
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

            BindUserAndRoles(UserName);

            return View("Roles");
        }

        /// <summary>
        /// Validate inputs of user name and role.
        /// </summary>
        /// <param name="UserName">Name of user.</param>
        /// <param name="RoleName">Name of role.</param>
        /// <returns>True of false according to validation result.</returns>
        private bool ValidateInputs(string UserName, string RoleName) {
            if (UserName.IsNullOrWhiteSpace() || RoleName.IsNullOrWhiteSpace()) {
                ViewBag.ResultType = "warning";
                ViewBag.ResultMessage = "Please fill inputs properly";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Removed user from specified role.
        /// </summary>
        /// <param name="UserName">Name of user.</param>
        /// <param name="RoleName">Name of role.</param>
        /// <returns>Html page with user role administration.</returns>
        public ActionResult DeleteRoleForUser(string UserName, string RoleName) {
            if (!ValidateInputs(UserName, RoleName)) {
                BindUserAndRoles(UserName);
                return View("Roles");
            }

            if (!repo.IsInRole(UserName, RoleName)) {
                ViewBag.ResultType = "danger";
                ViewBag.ResultMessage = "User " + UserName + " is not in role " + RoleName;
            } else if (Request.Cookies["logged_user"].Value.Equals(UserName.ToLower()) && RoleName.Equals("supervisor")) {
                ViewBag.ResultType = "danger";
                ViewBag.ResultMessage = "You can´t remove yourself from supervisor role";
            }
            else {
                repo.RemoveFromRole(UserName, RoleName);
                ViewBag.ResultType = "success";
                ViewBag.ResultMessage = "User " + UserName + " has been removed from " + RoleName;
            }

            BindUserAndRoles(UserName);

            return View("Roles");
        }

        /// <summary>
        /// Create new user account.
        /// </summary>
        /// <param name="user">User registration model <see cref="UserRegModel"/></param>
        /// <returns>Html page with create user form.</returns>
        public ActionResult Create(UserRegModel user) {
            try {
                if (ModelState.IsValid) {
                    IdentityResult result = repo.RegisterUser(user);
                    if (result.Succeeded) {
                        ViewBag.ResultType = "success";
                        ViewBag.ResultMessage = "User " + user.UserName + " has been created";
                    }
                    else {
                        ViewBag.ResultType = "danger";
                        ViewBag.ResultMessage = result.Errors.Aggregate((x, y) => x + "; " + y);
                    }
                }            
            }
            catch {
                return View();
            }

            return View();
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>All users with informations in Json array.</returns>
        public ActionResult GetUsers() {
            var allusers = repo.GetAllUsers();
            UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);

            var users = from user in allusers
                        select new {
                            DT_RowId = user.Id,
                            UserInfoHref = u.Action("User", "Users", new {
                                name = user.UserName
                            }),
                            UserName = user.UserName,
                            Email = user.Email,
                            Roles = repo.GetRoles(user.UserName),
                            EditRolesHref = u.Action("Roles", "Users", new {
                                UserName = user.UserName
                            })
                        };

            return Json(new { data = users }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Render View with information about user.
        /// </summary>
        /// <param name="name">Name of user.</param>
        /// <returns>Html page with user info.</returns>
        public ActionResult User(String name) {            
            var user = repo.FindUserByName(name);

            if (user != null) {
                ViewBag.Id = user.Id;
                ViewBag.UserName = user.UserName.ToLower();
                ViewBag.Email = user.Email;
                ViewBag.IsSupervisor = repo.IsInRole(user.UserName, "supervisor");
                var roles = repo.GetRoles(user.UserName);
                if (roles.Any())
                    ViewBag.Roles = repo.GetRoles(user.UserName).Aggregate((x, y) => x + ", " + y);             
            }

            return View("User");
        }

        /// <summary>
        /// Change user password.
        /// </summary>
        /// <param name="model">User password reset model <see cref="PasswordResetModel"/></param>
        /// <returns>Call <see cref="User"/> to return user info page.</returns>
        public ActionResult ResetPassword(PasswordResetModel model) {
            try {
                if (ModelState.IsValid) {
                    IdentityResult result = repo.ChangePassword(model.Id, model.Password);
                    if (result.Succeeded) {
                        ViewBag.ResultType = "success";
                        ViewBag.ResultMessage = "User password has been changed";
                    }
                    else {
                        ViewBag.ResultType = "danger";
                        ViewBag.ResultMessage = result.Errors.Aggregate((x, y) => x + "; " + y);
                    }
                }
            }
            catch {
                return User(model.UserName);
            }

            return User(model.UserName);
        }
    }
}