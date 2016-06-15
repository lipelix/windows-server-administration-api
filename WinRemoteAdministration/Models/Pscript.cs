namespace WinRemoteAdministration.Models {

    /// <summary>
    /// Model class for PowerShell script info, loaded from pscripts.xml.
    /// </summary>
    public class Pscript {
        public string Name { get; set; }
        public string Path { get; set; } 
    }
}