using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WinRemoteAdministration.Migrations {
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    /// <summary>
    /// Class Configuration set up database. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="System.Data.Entity.Migrations.DbMigrationsConfiguration{WinRemoteAdministration.Models.OwinAuthDbContext}" />
    internal sealed class Configuration : DbMigrationsConfiguration<WinRemoteAdministration.Models.OwinAuthDbContext> {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration() {
            AutomaticMigrationsEnabled = true;
        }

        /// <summary>
        /// Seeds the specified context with starting values.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void Seed(WinRemoteAdministration.Models.OwinAuthDbContext context) {
            const string UserName = "supervisor";
            const string RoleName = "supervisor";
            const string Password = "6o3hwr";

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
        }
    }
}
