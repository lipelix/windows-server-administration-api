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
    public class AuthRepository : IDisposable {
        public OwinAuthDbContext ctx;

        private UserManager<IdentityUser> userManager;

        public AuthRepository() {
            ctx = new OwinAuthDbContext();
            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(ctx));
        }

        public IdentityResult RegisterUser(UserRegModel userModel) {
//            var roleStore = new RoleStore<IdentityRole>(ctx);
//            var roleManager = new RoleManager<IdentityRole>(roleStore);
//
//            roleManager.Create(new IdentityRole { Name = "admin" });
//
//            var userStore = new UserStore<IdentityUser>(ctx);
//            var userManager = new UserManager<IdentityUser>(userStore);
//
//            var user = new IdentityUser { UserName = "superadmin", PasswordHash = "123456" };
//            userManager.Create(user);
//            var result = userManager.AddToRole(user.Id, "admin");

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

        public async Task<IdentityUser> FindUser(string userName, string password) {
            if (userName.IsNullOrWhiteSpace() || password.IsNullOrWhiteSpace())
                return null;

            IdentityUser user = await userManager.FindAsync(userName, password);
            return user;
        }

        public IdentityUser FindUser(string id) {
            if (id.IsNullOrWhiteSpace())
                return null;
            IdentityUser user = userManager.FindById(id);
            return user;
        }

        public IdentityUser FindUserByName(string userName) {
            if (userName.IsNullOrWhiteSpace())
                return null;
            IdentityUser user = userManager.FindByName(userName);
            return user;
        }

        public IList<string> GetRoles(string UserName) {
            IList<string> roles = null;
            if (!string.IsNullOrWhiteSpace(UserName)) {
                var user = ctx.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (user != null)
                    roles = userManager.GetRoles(user.Id);
            }

            return roles;
        }

        public bool IsInRole(string userName, string roleName) {
            if (userName.IsNullOrWhiteSpace() || roleName.IsNullOrWhiteSpace())
                return false;

            var user = FindUserByName(userName);
            return userManager.IsInRole(user.Id, roleName);
        }

        public void RoleAddToUser(string userName, string roleName) {
            var user = ctx.Users.Where(u => u.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (user != null)
                userManager.AddToRole(user.Id, roleName);
        }

        public void RemoveFromRole(string UserName, String RoleName) {
            var user = ctx.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (user != null && userManager.IsInRole(user.Id, RoleName)) {
                userManager.RemoveFromRole(user.Id, RoleName);
            }
        }

        public IdentityResult ChangePassword(string userId, string newPassword) {
            IdentityUser user = FindUser(userId);

            if (user == null)
                return null;

            user.PasswordHash = userManager.PasswordHasher.HashPassword(newPassword);

            return userManager.Update(user);
        }

        public List<IdentityUser> GetAllUsers() {
            return ctx.Users.ToList();
        }

        public void Dispose() {
            ctx.Dispose();
            userManager.Dispose();
        }
    }
}