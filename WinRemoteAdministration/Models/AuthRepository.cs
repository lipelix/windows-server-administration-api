using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
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
                var roleresult = userManager.AddToRole(currentUser.Id, "admin");
            }

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password) {
            IdentityUser user = await userManager.FindAsync(userName, password);
            return user;
        }

        public IdentityUser FindUser(string id) {
            IdentityUser user = userManager.FindById(id);
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

        public void RoleAddToUser(string UserName, string RoleName) {
            var user = ctx.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (user != null)
                userManager.AddToRole(user.Id, RoleName);
        }

        public void RemoveFromRole(string UserName, String RoleName) {
            var user = ctx.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (user != null && userManager.IsInRole(user.Id, RoleName)) {
                userManager.RemoveFromRole(user.Id, RoleName);
            }
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