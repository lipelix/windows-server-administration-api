using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRemoteAdministration.Models {

    interface IPscriptsServices {

        void LoadPScripts();
        IEnumerable<Pscript> GetAllScripts();
        Pscript GetScriptByName(string name);

    }
}
