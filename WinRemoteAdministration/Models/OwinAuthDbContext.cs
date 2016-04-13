using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WinRemoteAdministration.Models {
    public class OwinAuthDbContext : IdentityDbContext {
        public OwinAuthDbContext() : base("OwinAuthDbContext") {
            Database.SetInitializer<OwinAuthDbContext>(new DropCreateDatabaseIfModelChanges<OwinAuthDbContext>());
        }
    }
}