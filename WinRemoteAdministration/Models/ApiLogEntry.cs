using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace WinRemoteAdministration.Models {

    public class ApiLogEntry {
        public string User { get; set; }                    // The user that made the request.
        public string Machine { get; set; }                 // The machine that made the request.
        public string RequestIpAddress { get; set; }        // The IP address that made the request.
        public string RequestContentType { get; set; }      // The request content type.
        public string RequestContentBody { get; set; }      // The request content body.
        public string RequestUri { get; set; }              // The request URI.
        public string RequestMethod { get; set; }           // The request method (GET, POST, etc).
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Param { get; set; }
        public DateTime RequestTimestamp { get; set; }     // The request timestamp.
        public string ResponseContentType { get; set; }     // The response content type.
        public string ResponseContentBody { get; set; }     // The response content body.
        public int? ResponseStatusCode { get; set; }        // The response status code.
        public DateTime ResponseTimestamp { get; set; }    // The response timestamp.
    }
}