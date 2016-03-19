using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WinRemoteAdministration.Models {

    public class Pscripts {

        [XmlElement("Pscript")]
        public List<Pscript> PScriptsList = new List<Pscript>(); 

    }
}