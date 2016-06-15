using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WinRemoteAdministration.Models;
using WinRemoteAdministration.Providers;

namespace WinRemoteAdministration.Controllers {

    /// <summary>
    /// Account controller process api calls related to managing user accounts. Also provides process calls to get server certificate and test server availability.
    /// </summary>
    [System.Web.Http.RoutePrefix("api/Account")]
    public class AccountController : ApiController {
        private AuthRepository repo = null;

        public AccountController() {
            repo = new AuthRepository();
        }

        /// <summary>
        /// Call for downloading server certificate from app_data folder.
        /// </summary>
        /// <returns>certificate file</returns>
        [System.Web.Http.HttpGet]        
        public HttpResponseMessage getCert() {
            var path = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/cert.pfx");
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            try {                
                var stream = new FileStream(path, FileMode.Open);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = Path.GetFileName(path);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentLength = stream.Length;
            }
            catch (Exception) {
                result.Content = new StringContent (WebAPIUtils.CreateSimpleErrorResponse("Server does not provide certificate."));
                return result;
            }

            return result;
        }

        /// <summary>
        /// Test server availability.
        /// </summary>
        /// <returns>"hello" string</returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Ping() {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "pong");
            response.Content = new StringContent("hello", Encoding.Unicode);
            return response;
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                repo.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
