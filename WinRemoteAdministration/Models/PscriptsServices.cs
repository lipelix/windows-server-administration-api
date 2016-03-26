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
                try {
                    var shell = PowerShell.Create();
                    shell.Commands.AddScript("& \"" + script.Path + "\" " + scriptParams + " | ConvertTo-Json");
                    Collection<PSObject> results = shell.Invoke();
                    System.Diagnostics.Debug.WriteLine("Output:");
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
                        return CreateSimpleErrorResponse(err.ToString());
                    }
                }
                catch (RuntimeException ex) {
                    System.Diagnostics.Debug.WriteLine("Terminating error:");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return CreateSimpleErrorResponse(ex.Message);
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

        public object getUserFilter(object resultObject) {
            dynamic resObj = resultObject;        
            var filteredObject = new {
                GivenName = resObj.GivenName,
                Surname = resObj.Surname,
                Enabled = resObj.Enabled,
                SamAccountName = resObj.SamAccountName,
                DistinguishedName = resObj.DistinguishedName,
                Name = resObj.Name,
                AccountExpirationDate = resObj.AccountExpirationDate.ToString("G"),
                EmailAddress = resObj.EmailAddress,
                LastLogonDate = resObj.LastLogonDate.ToString("G"),
                PasswordLastSet = resObj.PasswordLastSet.ToString("G"),
                Created = resObj.Created.ToString("G")
            };
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