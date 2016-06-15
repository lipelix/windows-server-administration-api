using System.Collections.Generic;

namespace WinRemoteAdministration.Models {

    /// <summary>
    /// Model class for agregation of users and admins lists
    /// </summary>
    public class GroupedUserViewModel {
        public List<UserViewModel> Users { get; set; }
        public List<UserViewModel> Admins { get; set; }
    }
}