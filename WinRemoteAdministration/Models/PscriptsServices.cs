using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Web.Helpers;

namespace WinRemoteAdministration.Models {
    public class PscriptsServices : IPscriptsServices {

        protected Pscripts Pscripts;

        public PscriptsServices() {
            LoadPScripts();
        }

        public void LoadPScripts() {
            var fileName = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "/pscripts.xml";

            using (TextReader reader = new StreamReader(fileName)) {
                XmlSerializer serializer = new XmlSerializer(typeof(Pscripts));
                Pscripts = (Pscripts) serializer.Deserialize(reader);
                Pscripts.PScriptsList.ForEach(item => item.Path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + item.Path);                          
            }           
        }

        public IEnumerable<Pscript> GetAllScripts() {
            return Pscripts.PScriptsList;
        }

        public Pscript GetScriptByName(string name) {
            return Pscripts.PScriptsList.Find(item => item.Name.Equals(name));
        }

        public string RunScript(string name, IEnumerable<KeyValuePair<string, string>> parameters) {
            var script = Pscripts.PScriptsList.Find(item => item.Name.Equals(name));

            string scriptParams = "";
            foreach (KeyValuePair<string, string> param in parameters) {
                scriptParams += "-" + param.Key + " " + param.Value;
            }

            if (script != null) {
                // Initialize PowerShell engine
                var shell = PowerShell.Create();

                // Add the script to the PowerShell object
                shell.Commands.AddScript("& \"" + script.Path + "\" "+ scriptParams + " | ConvertTo-Json -Compress");

                // Execute the script
                var output = shell.Invoke();

                if (output.Count > 0) {
                    var builder = new StringBuilder();

                    if (filterExist(script)) {
                        var outputObject = Newtonsoft.Json.JsonConvert.DeserializeObject(output[0].BaseObject.ToString());
                        var filteredObject = CallFilter(script, outputObject);

                        return Newtonsoft.Json.JsonConvert.SerializeObject(filteredObject);
                    }
                    else {
                        return output[0].BaseObject.ToString();
                    }
                }
            }
            else {
                return CreateSimpleErrorResponse("Script not found");
            }

            return "";
        }

        protected bool filterExist(Pscript script) {
            var methodName = script.Name + "Filter";
            MethodInfo method = typeof(PscriptsServices).GetMethod(methodName);

            if (method != null) {
                return true;
            }
            else {
                return false;
            }
        }

        protected object CallFilter(Pscript script, object json) {
            var methodName = script.Name + "Filter";
            MethodInfo method = typeof(PscriptsServices).GetMethod(methodName);
            return method.Invoke(this, new object[] {json});   
        }

        public object getCultureFilter(object resultObject) {
            dynamic resObj = resultObject;
            var filteredObject = new { name = resObj.Name, displayName = resObj.DisplayName, englishName = resObj.EnglishName };
            return filteredObject;
        }

        public string CreateSimpleErrorResponse(string errorText) {
            var errorObject = new {
                error = errorText
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(errorObject);
        }
    }
}