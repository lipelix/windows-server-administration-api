using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using WinRemoteAdministration.Filters;
using WinRemoteAdministration.Models;
using WinRemoteAdministration.Services;

namespace WinRemoteAdministration.Controllers {

    [RequireLogin]
    public class LogController : Controller {
        // GET: Log
        public ActionResult Index() {
            return View();
        }

        public ActionResult getLog(string date) {
            var fileName = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "/Log/" + date + ".txt";
            try {
                string file = "[" + System.IO.File.ReadAllText(fileName) + "]";
                List<ApiLogEntry> log = JsonConvert.DeserializeObject<List<ApiLogEntry>>(file);
                var iterator = 1;
                log.ToList().ForEach(x => x.Id = iterator++);
                var response = new { data = log };

                return Content(new JavaScriptSerializer().Serialize(response), "application/json");
            }
            catch (FileNotFoundException e) {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
}