using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WinRemoteAdministration.Migrations {
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WinRemoteAdministration.Models.OwinAuthDbContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WinRemoteAdministration.Models.OwinAuthDbContext context) {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            roleManager.Create(new IdentityRole { Name = "admin" });
            roleManager.Create(new IdentityRole { Name = "supervisor" });

            var userStore = new UserStore<IdentityUser>(context);
            var userManager = new UserManager<IdentityUser>(userStore);

            var user = new IdentityUser { UserName = "supervisor", PasswordHash = "123456"};
            userManager.Create(user);
            userManager.AddToRole(user.Id, "supervisor");

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
