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
using WinRemoteAdministration.Services;

namespace WinRemoteAdministration.Controllers {

    //    [RequireHttps]
    [Authorize]
    public class PscriptsController : ApiController {

        [HttpGet]
        public string RunScript(string param) {
            var ps = new PscriptsServices();
            return ps.RunScript(param, this.Request.GetQueryNameValuePairs().GetEnumerator());
        }

        /// <summary>
        /// Execute script on server.
        /// </summary>
        /// <param name="param">Script name.</param>
        [HttpPost]
        public string RunScriptPost(string param, FormDataCollection formData) {
            IEnumerator<KeyValuePair<string, string>> valueMap = WebAPIUtils.Convert(formData);
            var ps = new PscriptsServices();
            return ps.RunScript(param, valueMap);
        }

    }
}
