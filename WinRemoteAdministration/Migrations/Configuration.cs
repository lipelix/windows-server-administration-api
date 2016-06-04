using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WinRemoteAdministration.Migrations {
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WinRemoteAdministration.Models.OwinAuthDbContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WinRemoteAdministration.Models.OwinAuthDbContext context) {
//                        var roleStore = new RoleStore<IdentityRole>(context);
//                        var roleManager = new RoleManager<IdentityRole>(roleStore);
//            
//                        roleManager.Create(new IdentityRole { Name = "admin" });
//                        roleManager.Create(new IdentityRole { Name = "supervisor" });
//            
//                        var userStore = new UserStore<IdentityUser>(context);
//                        var userManager = new UserManager<IdentityUser>(userStore);            
//            
//                        var user = new IdentityUser { UserName = "supervisor", PasswordHash = "123456"};     
//                        userManager.AddToRole(user.Id, "supervisor");


            const string UserName = "supervisor";
            const string RoleName = "supervisor";
            const string Password = "123456";

            var userRole = new IdentityRole { Name = RoleName, Id = Guid.NewGuid().ToString() };
            context.Roles.Add(userRole);

            var hasher = new PasswordHasher();

            var user = new IdentityUser {
                UserName = UserName,
                PasswordHash = hasher.HashPassword(Password),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            user.Roles.Add(new IdentityUserRole { RoleId = userRole.Id, UserId = user.Id });

            context.Users.Add(user);

            base.Seed(context);


            //            IdentityUser superAdmin = new IdentityUser();
            //            superAdmin.UserName = "superadmin";
            //            superAdmin.PasswordHash = "123456";
            //
            //            context.Users.Add(superAdmin);

            //              This method will be called after migrating to the latest version.
            //
            //              You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //              to avoid creating duplicate seed data. E.g.
            //            
            //                context.People.AddOrUpdate(
            //                  p => p.FullName,
            //                  new Person { FullName = "Andrew Peters" },
            //                  new Person { FullName = "Brice Lambson" },
            //                  new Person { FullName = "Rowan Miller" }
            //                );



        }
    }
}
