using System.Collections.Generic;

namespace WinRemoteAdministration.Models {

    /// <summary>
    /// Model class for agregation of users and admins lists
    /// </summary>
    public class GroupedUserViewModel {
        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>The users.</value>
        public List<UserViewModel> Users { get; set; }
        /// <summary>
        /// Gets or sets the admins.
        /// </summary>
        /// <value>The admins.</value>
        public List<UserViewModel> Admins { get; set; }
    }
}