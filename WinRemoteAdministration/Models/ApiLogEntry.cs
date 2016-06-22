using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace WinRemoteAdministration.Models {

    /// <summary>
    /// Model class of Api log entry which is saved into log file.
    /// </summary>
    public class ApiLogEntry {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public string User { get; set; }
        /// <summary>
        /// Gets or sets the machine.
        /// </summary>
        /// <value>The machine that made the request.</value>
        public string Machine { get; set; }
        /// <summary>
        /// Gets or sets the request ip address.
        /// </summary>
        /// <value>The request ip address.</value>
        public string RequestIpAddress { get; set; }        
        /// <summary>
        /// Gets or sets the type of the request content.
        /// </summary>
        /// <value>The type of the request content.</value>
        public string RequestContentType { get; set; }
        /// <summary>
        /// Gets or sets the request content body.
        /// </summary>
        /// <value>The request content body.</value>
        public string RequestContentBody { get; set; }
        /// <summary>
        /// Gets or sets the request URI.
        /// </summary>
        /// <value>The request URI.</value>
        public string RequestUri { get; set; }
        /// <summary>
        /// Gets or sets the request method.
        /// </summary>
        /// <value>The request method (GET / POST).</value>
        public string RequestMethod { get; set; }
        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>The controller.</value>
        public string Controller { get; set; }
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public string Action { get; set; }
        /// <summary>
        /// Gets or sets the parameter.
        /// </summary>
        /// <value>The parameter.</value>
        public string Param { get; set; }
        /// <summary>
        /// Gets or sets the request timestamp.
        /// </summary>
        /// <value>The request timestamp.</value>
        public DateTime RequestTimestamp { get; set; }
        /// <summary>
        /// Gets or sets the type of the response content.
        /// </summary>
        /// <value>The type of the response content.</value>
        public string ResponseContentType { get; set; }
        /// <summary>
        /// Gets or sets the response content body.
        /// </summary>
        /// <value>The response content body.</value>
        public string ResponseContentBody { get; set; }
        /// <summary>
        /// Gets or sets the response status code.
        /// </summary>
        /// <value>The response status code.</value>
        public int? ResponseStatusCode { get; set; }
        /// <summary>
        /// Gets or sets the response timestamp.
        /// </summary>
        /// <value>The response timestamp.</value>
        public DateTime ResponseTimestamp { get; set; }   
    }
}