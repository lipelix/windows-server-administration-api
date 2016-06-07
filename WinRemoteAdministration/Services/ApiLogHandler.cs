using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;
using System.Web.Routing;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using WinRemoteAdministration.Models;

namespace WinRemoteAdministration.Services {
    public class ApiLogHandler : DelegatingHandler {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            var apiLogEntry = CreateApiLogEntryWithRequestData(request);
            if (request.Content != null) {
                await request.Content.ReadAsStringAsync()
                    .ContinueWith(task => {
                        apiLogEntry.RequestContentBody = task.Result;
                    }, cancellationToken);
            }

            return await base.SendAsync(request, cancellationToken)
                .ContinueWith(task => {
                    var response = task.Result;

                // Update the API log entry with response info
                apiLogEntry.ResponseStatusCode = (int)response.StatusCode;
                    apiLogEntry.ResponseTimestamp = DateTime.Now;

                    if (response.Content != null) {
                        apiLogEntry.ResponseContentBody = response.Content.ReadAsStringAsync().Result;
                        apiLogEntry.ResponseContentType = response.Content.Headers.ContentType.MediaType;
                    }

                    saveEntry(apiLogEntry);

                    return response;
                }, cancellationToken);
        }

        private void saveEntry(ApiLogEntry entry) {  
            var day = entry.RequestTimestamp.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            var path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "/Log/" + day + ".txt";

            if (File.Exists(path))
                File.AppendAllText(path, ",");

            File.AppendAllText(path, JsonConvert.SerializeObject(entry, Formatting.Indented));
        }

        private ApiLogEntry CreateApiLogEntryWithRequestData(HttpRequestMessage request) {
            var context = ((HttpContextBase)request.Properties["MS_HttpContext"]);
            var routeData = request.GetRouteData();
            ClaimsIdentity identity = (ClaimsIdentity)request.GetRequestContext().Principal.Identity;
            var username = identity.Claims.First().Value;
            System.Diagnostics.Debug.WriteLine(username);

            return new ApiLogEntry {
                User = username,
                Machine = Environment.MachineName,
                RequestContentType = context.Request.ContentType,
                Controller = routeData.Values["controller"].ToString(),
                Action = routeData.Values["action"].ToString(),
                Param = routeData.Values["param"].ToString(),
                RequestIpAddress = context.Request.UserHostAddress,
                RequestMethod = request.Method.Method,
                RequestTimestamp = DateTime.Now,
                RequestUri = request.RequestUri.ToString()
            };
        }
    }
}