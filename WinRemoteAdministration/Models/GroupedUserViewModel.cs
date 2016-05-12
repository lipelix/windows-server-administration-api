using System.Collections.Generic;

namespace WinRemoteAdministration.Models {
    public class GroupedUserViewModel {
        public List<UserViewModel> Users { get; set; }
        public List<UserViewModel> Admins { get; set; }
    }
}