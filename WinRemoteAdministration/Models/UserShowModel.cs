using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WinRemoteAdministration.Models {

    /// <summary>
    /// Model of user for showing informations.
    /// </summary>
    public class UserShowModel {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
    }
}