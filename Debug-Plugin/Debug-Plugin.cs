using System;
using System.Collections;
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
using System.Runtime.InteropServices.ComTypes;
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
        private const string VarMonLoop = "_lum_dbg_varmon_loop";
        private string ErrorFile = "";
        // string TriggersFile = VNyanInterface.VNyanInterface.VNyanSettings.getProfilePath() + "Lum-Debug-Triggers.txt";
        private string LogFile = "";
        private string Version = "0.4-alpha";
        private string ConsolePath = "";
        private StreamWriter DebugStreamWriter = null;
        private StreamReader UIStreamReader = null;
        private Process DebugProcess = null;
        private Process UIProcess = null;
        private bool DebugProcessRunning = false;
        private bool UIProcessRunning = false;
        private int DebugPID;
        private int UIPID;
        dynamic SettingsJSON;
        List<string> MonitorTriggers = new List<string> { };
        Dictionary<string,float> MonitorFloats = new Dictionary<string, float> {};
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
            Log("DBG:" + e.ToString());
        }
        void CallVNyan(string TriggerName, int int1, int int2, int int3, string Text1, string Text2, string Text3)
        {
            if (TriggerName.Length > 0)
            {
                if (TriggerName != VarMonLoop)
                {
                    Log("Calling " + TriggerName + " with " + int1.ToString() + ", " + int2.ToString() + ", " + int3.ToString() + ", " + Text1 + ", " + Text2 + ", " + Text3);
                }
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
                string ItemName;
                if (CommandLine[0] == '"')
                {
                    int n = CommandLine.IndexOf('"', 1);
                    CommandLine = CommandLine.Substring(1, n - 1);
                } else
                {
                    CommandLine = CommandLine.Substring(0,CommandLine.IndexOf(" "));
                }
                Log("VNyan path: " + CommandLine);
                ConsolePath = Path.GetDirectoryName(CommandLine) + "\\Items\\Assemblies\\Debug-UI.exe";
                if (File.Exists(ConsolePath))
                {
                    Log("Debug Console found at: " + ConsolePath);
                } else
                {
                    Log("Debug Console not found should be at: " + ConsolePath);
                }
                string DefaultTriggers = VNyanInterface.VNyanInterface.VNyanSettings.getProfilePath() + "\\DefaultTriggers.txt";
                if (File.Exists (DefaultTriggers))
                {
                    foreach (string Item in File.ReadAllLines(DefaultTriggers))
                    {
                        if (Item.Substring(0, 1) == "+")
                        {
                            MonitorTriggers.Add(Item.Substring(1).ToLower());
                        }
                    }
                }
                string DefaultFloats = VNyanInterface.VNyanInterface.VNyanSettings.getProfilePath() + "\\DefaultDecimals.txt";
                if (File.Exists(DefaultFloats))
                {
                    foreach (string Item in File.ReadAllLines(DefaultFloats))
                    {
                        if (Item.Substring(0, 1) == "+")
                        {
                            ItemName = Item.Substring(1).ToLower();
                            MonitorFloats.Add(ItemName, VNyanInterface.VNyanInterface.VNyanParameter.getVNyanParameterFloat(ItemName));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorHandler(e);
            }
        }
        private void DoVariableMonitor()
        {
            float TempValue;
            List<string> Keys = new List<string>(MonitorFloats.Keys);
            foreach (string FloatName in Keys)
            {
                TempValue = VNyanInterface.VNyanInterface.VNyanParameter.getVNyanParameterFloat(FloatName);
                if (MonitorFloats[FloatName] != TempValue)
                {
                    Log("DEC: " + FloatName + " = " + TempValue.ToString());
                    MonitorFloats[FloatName] = TempValue;
                }
            }
            
        }
        
        public void triggerCalled(string name, int int1, int int2, int int3, string text1, string text2, string text3)
        {
            try
            {
                if (name.Length > 10)
                {
                    name = name.ToLower();
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
                            case "_getcam":
                                var camera = Camera.main;
                                float pX = camera.transform.position.x;
                                float pY = camera.transform.position.y;
                                float pZ = camera.transform.position.z;
                                float rX = camera.transform.rotation.eulerAngles.x;
                                float rY = camera.transform.rotation.eulerAngles.y;
                                float rZ = camera.transform.rotation.eulerAngles.z;
                                float FOV = camera.fieldOfView;
                                Log("Copy this text and import into LIV:\n" +
                                    "fov=" + FOV.ToString("0.0000000000000") + "\n" +
                                    "x=" + pX.ToString("0.0000000000000") + "\n" +
                                    "y=" + pY.ToString("0.0000000000000") + "\n" +
                                    "z=" + pZ.ToString("0.0000000000000") + "\n" +
                                    "rx=" + rX.ToString("0.0000000000000") + "\n" +
                                    "ry=" + rY.ToString("0.0000000000000") + "\n" +
                                    "rz=" + rZ.ToString("0.0000000000000")
                                );
                                break;
                            case "_varmon":
                                DoVariableMonitor();
                                if (DebugProcessRunning)
                                {
                                    CallVNyan(VarMonLoop, 0, 0, 0, "", "", "");
                                }
                                break;
                        }
                    }
                }
                if (MonitorTriggers.Contains(name)) {
                    Log("TRG: " +name+"|"+ int1.ToString() + ", " + int2.ToString() + ", " + int3.ToString() + "|" + text1 + "|" + text2 + "|" + text3);
                }
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

            DebugProcess = new Process();
            DebugProcess.StartInfo.FileName = ConsolePath;
            DebugProcess.StartInfo.UseShellExecute = false;
            DebugProcess.StartInfo.RedirectStandardInput = true;
            DebugProcess.StartInfo.RedirectStandardOutput = false;
            DebugProcess.StartInfo.RedirectStandardError = true;
            DebugProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            DebugProcess.StartInfo.CreateNoWindow = false;
            DebugProcess.EnableRaisingEvents = true;
            DebugProcess.Start();
            DebugProcessRunning = true;
            DebugPID = DebugProcess.Id;
            DebugStreamWriter = DebugProcess.StandardInput;
            // DebugStreamWriter.WriteLine("Connected to plugin v"+Version);
            // DebugStreamWriter.WriteLine("Checking for triggers: " + MonitorTriggers.ToString());
            CallVNyan(VarMonLoop, 0, 0, 0, "", "", "");
            do
            {
                Thread.Sleep(100);
            } while (ProcessExists(DebugPID));
            DebugProcessRunning = false;
            Log("Debug console process terminated");
        }

        async Task RunDebugUI()
        {
            try {
                string ConsoleInput;
                string Message;
                string Trigger;
                UIProcess = new Process();
                UIProcess.StartInfo.FileName = ConsolePath;
                UIProcess.StartInfo.UseShellExecute = false;
                UIProcess.StartInfo.RedirectStandardInput = false;
                UIProcess.StartInfo.RedirectStandardOutput = true;
                UIProcess.StartInfo.RedirectStandardError = false;
                UIProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                UIProcess.StartInfo.CreateNoWindow = true;
                UIProcess.StartInfo.Arguments = VNyanInterface.VNyanInterface.VNyanSettings.getProfilePath();
                UIProcess.EnableRaisingEvents = true;
                UIProcess.Start();
                UIProcessRunning = true;
                UIPID = UIProcess.Id;
                UIStreamReader = UIProcess.StandardOutput;
                do
                {
                    Thread.Sleep(100);
                    ConsoleInput = UIStreamReader.ReadLine();
                    if (ConsoleInput.Length > 0)
                    {
                        //Log(ConsoleInput);
                        if (ConsoleInput.Length >= 8)
                        {
                            Message = ConsoleInput.Substring(7);
                            switch (ConsoleInput.Substring(0, 6))
                            {
                                case "ADDTRG":
                                    Trigger = Message.ToLower();
                                    if (!MonitorTriggers.Contains(Message))
                                    {
                                        MonitorTriggers.Add(Message);
                                        Log("SYS:Added trigger " + Message);
                                    }
                                    else
                                    {
                                        Log("SYS:Trigger already being monitored: " + Message);
                                    }
                                    break;
                                case "DELTRG":
                                    Trigger = Message.ToLower();
                                    if (MonitorTriggers.Contains(Trigger))
                                    {
                                        MonitorTriggers.Remove(Trigger);
                                        Log("SYS:Deleted trigger " + Message);
                                    }
                                    else
                                    {
                                        Log("SYS:Trigger not being monitored: " + Message);
                                    }
                                    break;
                                case "CLRTRG":
                                    MonitorTriggers.Clear();
                                    Log("SYS:Trigger list cleared");
                                    break;
                                case "UIEXIT":
                                    UIProcessRunning = false;
                                    break;
                            }
                        }
                    }
                } while (UIProcessRunning);
                //UIProcess.WaitForExit();
                UIProcessRunning = false;
                Log("Debug UI process terminated");
            }
            catch (Exception e)
            {
                ErrorHandler(e);
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
                Log("Plugin button pressed - Debug console is running - Launching UI");
                if (!UIProcessRunning)
                {
                    Task.Run(() => RunDebugUI());
                }
            }
        }
    }
}