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
using WinRemoteAdministration.Services;

namespace WinRemoteAdministration.Controllers {

    [System.Web.Http.RoutePrefix("api/Account")]
    public class ApiAccountController : ApiController {
        private AuthRepository repo = null;

        public ApiAccountController() {
            repo = new AuthRepository();
        }

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

        [System.Web.Http.HttpGet]
        public HttpResponseMessage Ping() {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "pong");
            response.Content = new StringContent("hello", Encoding.Unicode);
            return response;
        }

        // POST api/account/isValid
        [Filters.RequireHttps]
        [System.Web.Http.Authorize]
        public async Task<IHttpActionResult> IsValid(UserModel userModel) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            IdentityUser result = await repo.FindUser(userModel.UserName, userModel.Password);
        
            if (result == null) {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], this.Request);
            }

            return Ok();
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result) {
            if (result == null) {
                return InternalServerError();
            }

            if (!result.Succeeded) {
                if (result.Errors != null) {
                    foreach (string error in result.Errors) {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid) {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
