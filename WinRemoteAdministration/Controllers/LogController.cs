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

        public ActionResult Stats() {
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

        public ActionResult getLogs(string[] dates) {
            List<ApiLogEntry> allLog = new List<ApiLogEntry>();

            foreach (var date in dates) {
                var fileName = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "/Log/" + date + ".txt";
                try {
                    string file = "[" + System.IO.File.ReadAllText(fileName) + "]";
                    List<ApiLogEntry> log = JsonConvert.DeserializeObject<List<ApiLogEntry>>(file);
                    var iterator = 1;
                    log.ToList().ForEach(x => x.Id = iterator++);
                    allLog.AddRange(log);
                }
                catch (FileNotFoundException e) {}
            }

            var paramsGrouped = allLog.GroupBy(n => n.Param).
                                 Select(group =>
                                     new {
                                         Param = group.Key,
                                         Count = group.Count()
                                     });

            var statusesGrouped = allLog.GroupBy(n => n.ResponseStatusCode).
                                 Select(group =>
                                     new {
                                         Status = group.Key,
                                         Count = group.Count()
                                     });

            var controllersGrouped = allLog.GroupBy(n => n.Controller).
                     Select(group =>
                         new {
                             Controller = group.Key,
                             Count = group.Count()
                         });

            var actionsGrouped = allLog.GroupBy(n => n.Action).
                     Select(group =>
                         new {
                             Action = group.Key,
                             Count = group.Count()
                         });

            var usersGrouped = allLog.GroupBy(n => n.User).
                     Select(group =>
                         new {
                             User = group.Key,
                             Count = group.Count()
                         });

            var response = new {
                Params = paramsGrouped,
                Statuses = statusesGrouped,
                Actions = actionsGrouped,
                Controllers = controllersGrouped,
                Users = usersGrouped,
            };

            return Content(new JavaScriptSerializer().Serialize(response), "application/json");
        }
    }
}