using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WinRemoteAdministration.Models {
    /// <summary>
    /// Setting up owin database context configuration
    /// </summary>
    public class OwinAuthDbContext : IdentityDbContext {
        public OwinAuthDbContext() : base("OwinAuthDbContext") {
            Database.SetInitializer<OwinAuthDbContext>(new DropCreateDatabaseIfModelChanges<OwinAuthDbContext>());
        }
    }
}