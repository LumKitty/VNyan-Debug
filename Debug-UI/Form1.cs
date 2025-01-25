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
                    //Console.Error.WriteLine("ADDTRG:" + textTriggerName.Text.ToLower());
                    textTriggerName.Text = "";
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
            if (listTriggers.CheckedItems.Contains(TriggerName))
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

        private void listTriggers_DoubleClick(object sender, EventArgs e)
        {
            listTriggers.Items.Remove(listTriggers.Items[listTriggers.SelectedIndex]);
        }

        void SaveTriggers(string FileName)
        {
            System.IO.StreamWriter TriggerFile = File.CreateText(FileName);
            foreach (var Item in listTriggers.Items)
            {
                if (listTriggers.CheckedItems.Contains(Item))
                {
                    TriggerFile.Write("+");
                }
                else
                {
                    TriggerFile.Write("-");
                }
                TriggerFile.WriteLine(Item);
            }
            TriggerFile.Close();
        }
        void LoadTriggers(string FileName)
        {
            Console.WriteLine("CLRTRG::");
            listTriggers.Items.Clear();
            foreach (string Item in File.ReadAllLines(FileName))
            {
                switch (Item.Substring(0, 1))
                {
                    case "+":
                        listTriggers.Items.Add(Item.Substring(1), true);
                        break;
                    case "-":
                        listTriggers.Items.Add(Item.Substring(1), false);
                        break;
                    default:
                        listTriggers.Items.Add(Item, false);
                        break;
                }
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
            DefaultTriggers = Debug_UI.ProfilePath + "\\DefaultTriggers.txt";
            dlgLoadTriggers.InitialDirectory = Debug_UI.ProfilePath;
            dlgSaveTriggers.InitialDirectory = Debug_UI.ProfilePath;
            //Console.WriteLine(Debug_UI.ProfilePath);
            if (File.Exists(DefaultTriggers))
            {
                LoadTriggers(DefaultTriggers);
            }
        }
    }
}
