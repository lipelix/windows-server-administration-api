using System.Web;
using System.Web.Mvc;
using WinRemoteAdministration.Filters;
using RequireHttpsAttribute = System.Web.Mvc.RequireHttpsAttribute;

namespace WinRemoteAdministration {

    /// <summary>
    /// Register filters of http request
    /// </summary>
    public class FilterConfig {

        /// <summary>
        /// Add filters to handle error attribute and require secured connection
        /// </summary>
        /// <param name="filters">Collection of global filters</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
