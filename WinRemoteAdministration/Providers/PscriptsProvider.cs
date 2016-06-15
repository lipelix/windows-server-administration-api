using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Reflection;
using System.Xml.Serialization;
using WinRemoteAdministration.Models;

namespace WinRemoteAdministration.Providers {

    /// <summary>
    /// Provides methods to load and run PowerShell scripts.
    /// </summary>
    public class PscriptsProvider {

        protected Pscripts Pscripts;

        public PscriptsProvider() {
            LoadPScripts();
        }

        /// <summary>
        /// Load informations from PowerShell scripts configuration file.
        /// </summary>
        public void LoadPScripts() {
            var fileName = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "/pscripts.xml";

            using (TextReader reader = new StreamReader(fileName)) {
                XmlSerializer serializer = new XmlSerializer(typeof(Pscripts));
                Pscripts = (Pscripts) serializer.Deserialize(reader);
                Pscripts.PScriptsList.ForEach(item => item.Path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + item.Path);                          
            }           
        }

        /// <summary>
        /// Get list of scripts.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Pscript> GetAllScripts() {
            return Pscripts.PScriptsList;
        }

        /// <summary>
        /// Get specified script info.
        /// </summary>
        /// <param name="name">Name of script.</param>
        /// <returns>Script information model <see cref="Pscript"/></returns>
        public Pscript GetScriptByName(string name) {
            return Pscripts.PScriptsList.Find(item => item.Name.Equals(name));
        }

        /// <summary>
        /// Execute provided PowerShell script.
        /// </summary>
        /// <param name="name">Name of script to run.</param>
        /// <param name="parameters">Script input parameters.</param>
        /// <returns>Result of script execution in serialized JSON.</returns>
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

        /// <summary>
        /// Check if exist filter method.
        /// </summary>
        /// <param name="script">Name of method.</param>
        /// <returns>True or false.</returns>
        protected bool filterExist(Pscript script) {
            var methodName = script.Name + "Filter";
            MethodInfo method = typeof(PscriptsProvider).GetMethod(methodName);

            if (method != null) {
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Call filter method if exists.
        /// </summary>
        /// <param name="script">Filter method name.</param>
        /// <param name="json">Json result object which will be filtered.</param>
        /// <returns>Filtered anonymous object.</returns>
        protected object CallFilter(Pscript script, object json) {
            var methodName = script.Name + "Filter";
            MethodInfo method = typeof(PscriptsProvider).GetMethod(methodName);
            return method.Invoke(this, new object[] {json});   
        }

    }
}