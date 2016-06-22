using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WinRemoteAdministration.Models {
    /// <summary>
    /// Setting up database context configuration
    /// </summary>
    public class OwinAuthDbContext : IdentityDbContext {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwinAuthDbContext"/> class.
        /// </summary>
        public OwinAuthDbContext() : base("OwinAuthDbContext") {
            Database.SetInitializer<OwinAuthDbContext>(new DropCreateDatabaseIfModelChanges<OwinAuthDbContext>());
        }
    }
}