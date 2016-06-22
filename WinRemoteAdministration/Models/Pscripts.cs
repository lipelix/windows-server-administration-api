using System.Collections.Generic;
using System.Xml.Serialization;

namespace WinRemoteAdministration.Models {
    /// <summary>
    /// Model class for loading list of PowerShell scripts informations from pscripts.xml.
    /// </summary>
    public class Pscripts {

        /// <summary>
        /// The PowerShell scripts list
        /// </summary>
        [XmlElement("Pscript")]
        public List<Pscript> PScriptsList = new List<Pscript>(); 

    }
}