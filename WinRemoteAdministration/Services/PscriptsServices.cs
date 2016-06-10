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

namespace WinRemoteAdministration.Services {
    public class PscriptsServices {

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

        public string RunScript(string name, IEnumerator<KeyValuePair<string, string>> parameters) {
            var script = Pscripts.PScriptsList.Find(item => item.Name.Equals(name));

            string scriptParams = "";
            while (parameters.MoveNext()) {
                scriptParams += "-" + parameters.Current.Key + " \"" + parameters.Current.Value + "\" ";
            }

            if (script != null) {
                try {
                    var shell = PowerShell.Create();
                    shell.Commands.AddScript("& \"" + script.Path + "\" " + scriptParams + " | ConvertTo-Json");
                    Collection<PSObject> results = shell.Invoke();

                    foreach (var psObject in results) {
                        System.Diagnostics.Debug.WriteLine(psObject);
                        var outputObject = Newtonsoft.Json.JsonConvert.DeserializeObject(psObject.ToString());
                        if (filterExist(script)) {
                            var filteredObject = CallFilter(script, outputObject);
                            return Newtonsoft.Json.JsonConvert.SerializeObject(filteredObject);
                        }
                        else {
                            return Newtonsoft.Json.JsonConvert.SerializeObject(outputObject);
                        }
                    }
                    System.Diagnostics.Debug.WriteLine("Non-terminating errors:");
                    foreach (ErrorRecord err in shell.Streams.Error) {
                        System.Diagnostics.Debug.WriteLine(err.ToString());
                        return WebAPIUtils.CreateSimpleErrorResponse(err.ToString());
                    }
                }
                catch (RuntimeException ex) {
                    System.Diagnostics.Debug.WriteLine("Terminating error:");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return WebAPIUtils.CreateSimpleErrorResponse(ex.Message);
                }

            }
            else {
                return WebAPIUtils.CreateSimpleErrorResponse("Script not found");
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

    }
}