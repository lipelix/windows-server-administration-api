using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using WinRemoteAdministration.Filters;
using WinRemoteAdministration.Providers;

namespace WinRemoteAdministration.Controllers {
    /// <summary>
    /// Controller handle calls for executing PowerShell scripts to managing target station.
    /// </summary>
    //    [RequireHttps]
    [Authorize]
    public class PscriptsController : ApiController {

        /// <summary>
        /// Use <see cref="PscriptsProvider"/> to execute script. Handling GET requests.
        /// </summary>
        /// <param name="param">Name of script (defined in <see cref="pscripts.xml"/> configuration file).</param>
        /// <returns>Output of script or error in Json format.</returns>
        [HttpGet]
        public string RunScript(string param) {
            var ps = new PscriptsProvider();
            return ps.RunScript(param, this.Request.GetQueryNameValuePairs().GetEnumerator());
        }

        /// <summary>
        /// Use <see cref="PscriptsProvider"/> to execute script. Handling POST requests with parameters.
        /// </summary>
        /// <param name="param">Name of script (defined in <see cref="pscripts.xml"/> configuration file).</param>
        /// <param name="formData">Input parameters of script.</param>
        /// <returns>Output of script or error in Json format.</returns>
        [HttpPost]
        public string RunScriptPost(string param, FormDataCollection formData) {
            IEnumerator<KeyValuePair<string, string>> valueMap = WebAPIUtils.Convert(formData);
            var ps = new PscriptsProvider();
            return ps.RunScript(param, valueMap);
        }

    }
}
