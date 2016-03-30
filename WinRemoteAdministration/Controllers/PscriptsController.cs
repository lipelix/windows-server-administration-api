using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WinRemoteAdministration.Filters;
using WinRemoteAdministration.Services;

namespace WinRemoteAdministration.Controllers {

    //    [RequireHttps]
    [Authorize]
    public class PscriptsController : ApiController {

        [HttpGet]
        public IEnumerable<Pscript> GetAll() {
            var ps = new PscriptsServices();
            return ps.GetAllScripts();
        }

        [HttpGet]
        public Pscript GetScript(string param) {
            var ps = new PscriptsServices();
            return ps.GetScriptByName(param);
        }

        [HttpGet]
        public string RunScript(string param) {
            var ps = new PscriptsServices();
            return ps.RunScript(param, this.Request.GetQueryNameValuePairs());
        }

    }
}
