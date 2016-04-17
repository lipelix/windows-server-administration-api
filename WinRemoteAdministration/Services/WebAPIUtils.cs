using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;

namespace WinRemoteAdministration.Services {
    public static class WebAPIUtils {

        /// <summary>
        /// Copy the values contained in the given FormDataCollection into 
        /// a NameValueCollection instance.
        /// </summary>
        /// <param name="formDataCollection">The FormDataCollection instance. (required, but can be empty)</param>
        /// <returns>The NameValueCollection. Never returned null, but may be empty.</returns>
        public static IEnumerator<KeyValuePair<string, string>> Convert(FormDataCollection formDataCollection) {
            IEnumerator<KeyValuePair<string, string>> pairs = null;

            if (formDataCollection != null)
                pairs = formDataCollection.GetEnumerator();

            return pairs;
        }

        public static string CreateSimpleErrorResponse(string errorText) {
            var errorObject = new {
                error = errorText
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(errorObject);
        }

    }
}