﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WinRemoteAdministration.Models {

    /// <summary>
    /// Model for showing information about user roles.
    /// </summary>
    public class UserViewModel {
        public string Username { get; set; }
        public string Roles { get; set; }
    }
}