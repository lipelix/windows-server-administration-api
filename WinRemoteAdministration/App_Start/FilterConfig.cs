using System.Web;
using System.Web.Mvc;
using WinRemoteAdministration.Filters;
using RequireHttpsAttribute = System.Web.Mvc.RequireHttpsAttribute;

namespace WinRemoteAdministration {
    public class FilterConfig {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
