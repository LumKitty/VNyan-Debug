using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Debug_UI
{
    public partial class Form1 : Form
    {
        string DefaultTriggers;
        public Form1()
        {
            InitializeComponent();
        }


        void AddTrigger()
        {
            if (textTriggerName.Text.Length > 0)
            {
                if (!listTriggers.Items.Contains(textTriggerName.Text))
                {
                    int n = listTriggers.Items.Add(textTriggerName.Text, true);
                    Console.Error.WriteLine("ADDTRG:" + textTriggerName.Text.ToLower());
                    textTriggerName.Text = "";
                }
            }
        }

        void AddDecimal()
        {
            if (textTriggerName.Text.Length > 0)
            {
                if (!listDecimals.Items.Contains(textDecimalName.Text))
                {
                    int n = listDecimals.Items.Add(textDecimalName.Text, true);
                    Console.Error.WriteLine("ADDDEC:" + textDecimalName.Text.ToLower());
                    textDecimalName.Text = "";
                }
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddTrigger();
        }

        private void listTriggers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string TriggerName = listTriggers.Items[e.Index].ToString().ToLower();
            //Console.WriteLine(TriggerName);
            if (e.NewValue == CheckState.Unchecked)
            {
                Console.WriteLine("DELTRG:" + TriggerName);
            }
            else
            {
                Console.WriteLine("ADDTRG:" + TriggerName);
            }
        }

        private void textTriggerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                AddTrigger();
            }
        }
        private void textDecimalName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                AddDecimal();
            }
        }


        private void listTriggers_DoubleClick(object sender, EventArgs e)
        {
            listTriggers.Items.Remove(listTriggers.Items[listTriggers.SelectedIndex]);
        }

        void SaveTriggers(string FileName)
        {
            JArray Triggers = new JArray();
            JArray Decimals = new JArray();
            foreach (var Item in listTriggers.Items)
            {
                Triggers.Add(
                    new JObject(
                        new JProperty("Name", Item),
                        new JProperty("Monitor", listTriggers.CheckedItems.Contains(Item))
                    )
                );
            }
            foreach (var Item in listDecimals.Items)
            {
                Decimals.Add(
                    new JObject(
                        new JProperty("Name", Item),
                        new JProperty("Monitor", listDecimals.CheckedItems.Contains(Item))
                    )
                );
            }

            JObject Settings = new JObject(
                new JProperty("Triggers", Triggers),
                new JProperty("Decimals", Decimals),
                new JProperty("Text", new JArray()),
                new JProperty("Dictionaries", new JArray())
            );
            File.WriteAllText(FileName, Settings.ToString());
        }
        void LoadTriggers(string FileName)
        {
            Console.WriteLine("CLRTRG::");
            listTriggers.Items.Clear();
            dynamic Settings = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(FileName));
            foreach (var Trigger in Settings.Triggers)
            {
                listTriggers.Items.Add(Trigger.Name.ToString(), (Trigger.Monitor.ToString().ToLower() == "true"));
            }
            foreach (var Decimal in Settings.Decimals)
            {
                listDecimals.Items.Add(Decimal.Name.ToString(), (Decimal.Monitor.ToString().ToLower() == "true"));
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dlgSaveTriggers.ShowDialog() == DialogResult.OK)
            {
                SaveTriggers(dlgSaveTriggers.FileName);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (dlgLoadTriggers.ShowDialog() == DialogResult.OK)
            {
                LoadTriggers(dlgLoadTriggers.FileName);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DefaultTriggers = Debug_UI.ProfilePath + Debug_UI.DefaultSettingsFile;
            dlgLoadTriggers.InitialDirectory = Debug_UI.ProfilePath;
            dlgSaveTriggers.InitialDirectory = Debug_UI.ProfilePath;
            //Console.WriteLine(Debug_UI.ProfilePath);
            if (File.Exists(DefaultTriggers))
            {
                LoadTriggers(DefaultTriggers);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveTriggers(Debug_UI.ProfilePath + Debug_UI.DefaultSettingsFile);
        }

        private void btnAddDecimal_Click(object sender, EventArgs e)
        {
            AddDecimal();
        }

        private void listDecimals_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string DecimalName = listDecimals.Items[e.Index].ToString().ToLower();
            //Console.WriteLine(TriggerName);
            if (e.NewValue == CheckState.Unchecked)
            {
                Console.WriteLine("DELDEC:" + DecimalName);
            }
            else
            {
                Console.WriteLine("ADDDEC:" + DecimalName);
            }
        }
    }
}
