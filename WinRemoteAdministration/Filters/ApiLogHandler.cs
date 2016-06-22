using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using WinRemoteAdministration.Models;

namespace WinRemoteAdministration.Filters {

    /// <summary>
    /// Api log handler catches api calls and save them to logs.
    /// </summary>
    public class ApiLogHandler : DelegatingHandler {
        /// <summary>
        /// Process http request to save api log as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
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

        /// <summary>
        /// Saves api call to file at Log folder.
        /// </summary>
        /// <param name="entry"><see cref="ApiLogEntry"/> entry.</param>
        private void saveEntry(ApiLogEntry entry) {  
            var day = entry.RequestTimestamp.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            var path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "/Log/" + day + ".txt";

            if (File.Exists(path))
                File.AppendAllText(path, ",");

            File.AppendAllText(path, JsonConvert.SerializeObject(entry, Formatting.Indented));
        }

        /// <summary>
        /// Create log entry from request message.
        /// </summary>
        /// <param name="request"></param>
        /// <returns><see cref="ApiLogEntry"/> entry.</returns>
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