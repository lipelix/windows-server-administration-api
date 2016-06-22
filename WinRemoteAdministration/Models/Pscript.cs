namespace WinRemoteAdministration.Models {

    /// <summary>
    /// Model class for PowerShell script info, loaded from pscripts.xml.
    /// </summary>
    public class Pscript {
        /// <summary>
        /// Gets or sets the name of script in API.
        /// </summary>
        /// <value>The name of script in API.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the path to script.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; } 
    }
}