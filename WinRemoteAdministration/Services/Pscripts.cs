﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WinRemoteAdministration.Services {

    public class Pscripts {

        [XmlElement("Pscript")]
        public List<Pscript> PScriptsList = new List<Pscript>(); 

    }
}