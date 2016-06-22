using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WinRemoteAdministration.Models {

    /// <summary>
    /// Authentication Repository manipulate with user related data. 
    /// It uses database context and provides methods which are provide management functionality to other parts of application.
    /// </summary>
    public class AuthRepository : IDisposable {

        /// <summary>
        /// The database context
        /// </summary>
        public OwinAuthDbContext ctx;
        /// <summary>
        /// The user manager
        /// </summary>
        private UserManager<IdentityUser> userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthRepository"/> class.
        /// </summary>
        public AuthRepository() {
            ctx = new OwinAuthDbContext();
            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(ctx));
        }

        /// <summary>
        /// Register new user in database
        /// </summary>
        /// <param name="userModel"><see cref="UserRegModel"/> registration model.</param>
        /// <returns>Result object from UserManger.</returns>
        public IdentityResult RegisterUser(UserRegModel userModel) {        
            IdentityUser user = new IdentityUser {
                UserName = userModel.UserName
            };

            var result = userManager.Create(user, userModel.Password);

            if (result.Succeeded) {
                var currentUser = userManager.FindByName(user.UserName);
                userManager.AddToRole(currentUser.Id, "admin");
                user.Email = userModel.Email;
                userManager.Update(user);
            }

            return result;
        }

        /// <summary>
        /// Finds user according to provided parameters.
        /// </summary>
        /// <param name="userName">Name of user.</param>
        /// <param name="password">Password of user.</param>
        /// <returns>IdentityUser object.</returns>
        public async Task<IdentityUser> FindUser(string userName, string password) {
            if (userName.IsNullOrWhiteSpace() || password.IsNullOrWhiteSpace())
                return null;

            IdentityUser user = await userManager.FindAsync(userName, password);
            return user;
        }

        /// <summary>
        /// Finds user according to provided parameters.
        /// </summary>
        /// <param name="id">Id user.</param>
        /// <returns>IdentityUser object.</returns>
        public IdentityUser FindUser(string id) {
            if (id.IsNullOrWhiteSpace())
                return null;
            IdentityUser user = userManager.FindById(id);
            return user;
        }

        /// <summary>
        /// Finds user according to provided parameters.
        /// </summary>
        /// <param name="userName">Name of user.</param>
        /// <returns>IdentityUser object.</returns>
        public IdentityUser FindUserByName(string userName) {
            if (userName.IsNullOrWhiteSpace())
                return null;
            IdentityUser user = userManager.FindByName(userName);
            return user;
        }

        /// <summary>
        /// Get user roles membership.
        /// </summary>
        /// <param name="UserName">Name of user.</param>
        /// <returns>List of roles memebership.</returns>
        public IList<string> GetRoles(string UserName) {
            IList<string> roles = null;
            if (!string.IsNullOrWhiteSpace(UserName)) {
                var user = ctx.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (user != null)
                    roles = userManager.GetRoles(user.Id);
            }

            return roles;
        }

        /// <summary>
        /// Test if user is in specified role.
        /// </summary>
        /// <param name="userName">Name of user.</param>
        /// <param name="roleName">Name of role.</param>
        /// <returns>True or false</returns>
        public bool IsInRole(string userName, string roleName) {
            if (userName.IsNullOrWhiteSpace() || roleName.IsNullOrWhiteSpace())
                return false;

            var user = FindUserByName(userName);
            return userManager.IsInRole(user.Id, roleName);
        }

        /// <summary>
        /// Add user to role.
        /// </summary>
        /// <param name="userName">Name of user.</param>
        /// <param name="roleName">Name of role.</param>
        public void RoleAddToUser(string userName, string roleName) {
            var user = ctx.Users.Where(u => u.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (user != null)
                userManager.AddToRole(user.Id, roleName);
        }

        /// <summary>
        /// Remove user from role.
        /// </summary>
        /// <param name="userName">Name of user.</param>
        /// <param name="roleName">Name of role.</param>
        public void RemoveFromRole(string userName, string roleName) {
            var user = ctx.Users.Where(u => u.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (user != null && userManager.IsInRole(user.Id, roleName)) {
                userManager.RemoveFromRole(user.Id, roleName);
            }
        }

        /// <summary>
        /// Change password of specified account.
        /// </summary>
        /// <param name="userId">Id of user account.</param>
        /// <param name="newPassword">New password.</param>
        /// <returns>Identity Result object</returns>
        public IdentityResult ChangePassword(string userId, string newPassword) {
            IdentityUser user = FindUser(userId);

            if (user == null)
                return null;

            user.PasswordHash = userManager.PasswordHasher.HashPassword(newPassword);

            return userManager.Update(user);
        }

        /// <summary>
        /// Get all users accounts.
        /// </summary>
        /// <returns>List of IdentityUser objets</returns>
        public List<IdentityUser> GetAllUsers() {
            return ctx.Users.ToList();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            ctx.Dispose();
            userManager.Dispose();
        }
    }
}