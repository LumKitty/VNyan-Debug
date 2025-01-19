using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.Networking.UnityWebRequest;

namespace Debug_Plugin
{
    public class Debug_Plugin : MonoBehaviour, VNyanInterface.IButtonClickedHandler, VNyanInterface.ITriggerHandler
    {
        private string ErrorFile = "";
        // string TriggersFile = VNyanInterface.VNyanInterface.VNyanSettings.getProfilePath() + "Lum-Debug-Triggers.txt";
        private string LogFile = "";
        private string Version = "0.2-alpha";
        private string ConsolePath = "";
        private StreamWriter DebugStreamWriter = null;
        private Process DebugProcess = null;
        private bool DebugProcessRunning = false;
        dynamic SettingsJSON;
        List<String> MonitorTriggers;
        private void Log(string message)
        {
            if (LogFile.ToString().Length > 0)
            {
                System.IO.File.AppendAllText(LogFile, message + "\r\n");
            }
            if (DebugProcessRunning)
            {
                DebugStreamWriter.WriteLine(message);
            }
        }

        private void LoadPluginSettings()
        {
            // Get settings in dictionary
            Dictionary<string, string> settings = VNyanInterface.VNyanInterface.VNyanSettings.loadSettings("Lum-Debug.cfg");
            if (settings != null)
            {
                string temp_ErrorFile;
                string temp_LogFile;
                settings.TryGetValue("ErrorFile", out temp_ErrorFile);
                settings.TryGetValue("LogFile", out temp_LogFile);
                if (temp_ErrorFile != null)
                {
                    ErrorFile = temp_ErrorFile;
                }
                else
                {
                    ErrorFile = System.IO.Path.GetTempPath() + "\\Lum_DBG_Error.txt";
                }
                if (temp_LogFile != null)
                {
                    LogFile = temp_LogFile;
                }
                else
                {
                    LogFile = System.IO.Path.GetTempPath() + "\\Lum_DBG_Log.txt";
                }
            }
            /*if (File.Exists(TriggersFile))
            {
                String[] Triggers = File.ReadLines(TriggersFile).ToString().Split('\n');
                foreach (string Trigger in Triggers) {
                    MonitorTriggers.Add(Trigger.ToString());
                }
            }*/

        }
        private void OnApplicationQuit()
        {
            // Save settings
            SavePluginSettings();
        }
        private void SavePluginSettings()
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            settings["ErrorFile"] = ErrorFile;
            settings["LogFile"] = LogFile;
            // settings["SomeValue2"] = someValue2.ToString(CultureInfo.InvariantCulture); // Make sure to use InvariantCulture to avoid decimal delimeter errors

            VNyanInterface.VNyanInterface.VNyanSettings.saveSettings("Lum-Debug.cfg", settings);
            //File.WriteAllText(JsonFile, SettingsJSON.ToString());
        }

        void ErrorHandler(Exception e)
        {
            System.IO.File.WriteAllText(ErrorFile, e.ToString());
            CallVNyan("_lum_miu_error", 0, 0, 0, e.ToString(), "", "");
        }
        void CallVNyan(string TriggerName, int int1, int int2, int int3, string Text1, string Text2, string Text3)
        {
            if (TriggerName.Length > 0)
            {
                Log("Calling " + TriggerName + " with " + int1.ToString() + ", " + int2.ToString() + ", " + int3.ToString() + ", " + Text1 + ", " + Text2 + ", " + Text3);
                VNyanInterface.VNyanInterface.VNyanTrigger.callTrigger(TriggerName, int1, int2, int3, Text1, Text2, Text3);
            }
            else
            {
                Log("Invalid trigger name");
            }
        }
        public void Awake()
        {
            try
            {
                VNyanInterface.VNyanInterface.VNyanTrigger.registerTriggerListener(this);
                VNyanInterface.VNyanInterface.VNyanUI.registerPluginButton("LumKitty's Debug Tool", this);
                LoadPluginSettings();
                System.IO.File.WriteAllText(LogFile, "Started v"+Version+"\r\n");
                string CommandLine = Environment.CommandLine;
                if (CommandLine[0] == '"')
                {
                    int n = CommandLine.IndexOf('"', 1);
                    CommandLine = CommandLine.Substring(1, n - 1);
                } else
                {
                    CommandLine = CommandLine.Substring(0,CommandLine.IndexOf(" "));
                }
                Log("VNyan path: " + CommandLine);
                ConsolePath = Path.GetDirectoryName(CommandLine) + "\\Items\\Assemblies\\Debug-Console.exe";
                if (File.Exists(ConsolePath))
                {
                    Log("Debug Console found at: " + ConsolePath);
                } else
                {
                    Log("Debug Console not found should be at: " + ConsolePath);
                }
            }
            catch (Exception e)
            {
                ErrorHandler(e);
            }
        }
        public void triggerCalled(string name, int int1, int int2, int int3, string text1, string text2, string text3)
        {
            try
            {
                if (name.Length > 10)
                {
                    if (name.Substring(0, 9) == "_lum_dbg_")
                    {
                        //Log("Detected trigger: " + name + " with " + int1.ToString() + ", " + SessionID.ToString() + ", " + PlatformID.ToString() + ", " + text1 + ", " + text2 + ", " + Callback);
                        switch (name.Substring(8))
                        {
                            case "_log":
                                Log("LOG: " + int1.ToString() + ", " + int2.ToString() + ", " + int3.ToString() + "|" + text1 + "|" + text2 + "|" + text3);
                                break;
                            case "_err":
                                Log("ERR: " + int1.ToString() + ", " + int2.ToString() + ", " + int3.ToString() + "|" + text1 + "|" + text2 + "|" + text3);
                                break;
                                // Log("Detected: " + name + " with " + int1.ToString() + ", " + int2.ToString() + ", " + int3.ToString() + ", " + text1 + ", " + text2 + ", " + text3);
                        }
                    }
                }
                /*if (MonitorTriggers.Contains(name)) {
                    Log("TRG: " +name+"|"+ int1.ToString() + ", " + int2.ToString() + ", " + int3.ToString() + "|" + text1 + "|" + text2 + "|" + text3);
                }*/
            }
            catch (Exception e)
            {
                ErrorHandler(e);
            }
        }
        private bool ProcessExists(int id)
        {
            return Process.GetProcesses().Any(x => x.Id == id);
        }
        async Task RunDebugProcess()
        {
            using (DebugProcess = new Process())
            {
                DebugProcess.StartInfo.FileName = ConsolePath;
                DebugProcess.StartInfo.UseShellExecute = false;
                DebugProcess.StartInfo.RedirectStandardInput = true;
                DebugProcess.StartInfo.RedirectStandardOutput = false;
                DebugProcess.StartInfo.RedirectStandardError = false;
                DebugProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                DebugProcess.StartInfo.CreateNoWindow = false;
                DebugProcess.EnableRaisingEvents = true;
                DebugProcess.Start();
                DebugProcessRunning = true;
                // int PID = DebugProcess.Id;
                DebugStreamWriter = DebugProcess.StandardInput;
                // DebugStreamWriter.WriteLine("Connected to plugin v"+Version);
                // DebugStreamWriter.WriteLine("Checking for triggers: " + MonitorTriggers.ToString());

                /* do {
                    Thread.Sleep(1000);
                } while (ProcessExists(PID)); */
                DebugProcess.WaitForExit();
                DebugProcessRunning = false;
                Log("Monitoring process terminated");
                Log("Cleanup complete");
            }
        }

        public void pluginButtonClicked()
        {
            if (!DebugProcessRunning)
            {
                Log("Plugin button pressed (Debug console not running)");
                Task.Run(() => RunDebugProcess());

            } else
            {
                Log("Plugin button pressed (Debug console is running)");
            }
        }
    }
}