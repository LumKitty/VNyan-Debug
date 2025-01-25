namespace Debug_UI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listTriggers = new CheckedListBox();
            textTriggerName = new TextBox();
            label1 = new Label();
            btnAdd = new Button();
            btnSave = new Button();
            btnLoad = new Button();
            dlgSaveTriggers = new SaveFileDialog();
            dlgLoadTriggers = new OpenFileDialog();
            SuspendLayout();
            // 
            // listTriggers
            // 
            listTriggers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listTriggers.CheckOnClick = true;
            listTriggers.FormattingEnabled = true;
            listTriggers.Location = new Point(12, 34);
            listTriggers.Name = "listTriggers";
            listTriggers.Size = new Size(256, 400);
            listTriggers.TabIndex = 0;
            listTriggers.ItemCheck += listTriggers_ItemCheck;
            listTriggers.DoubleClick += listTriggers_DoubleClick;
            // 
            // textTriggerName
            // 
            textTriggerName.Location = new Point(66, 12);
            textTriggerName.Name = "textTriggerName";
            textTriggerName.Size = new Size(188, 23);
            textTriggerName.TabIndex = 1;
            textTriggerName.KeyPress += textTriggerName_KeyPress;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(48, 15);
            label1.TabIndex = 2;
            label1.Text = "Triggers";
            // 
            // btnAdd
            // 
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Location = new Point(251, 12);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(17, 23);
            btnAdd.TabIndex = 3;
            btnAdd.Text = "+";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.Location = new Point(280, 11);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 4;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnLoad
            // 
            btnLoad.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLoad.Location = new Point(280, 40);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(75, 23);
            btnLoad.TabIndex = 5;
            btnLoad.Text = "Load";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += btnLoad_Click;
            // 
            // dlgSaveTriggers
            // 
            dlgSaveTriggers.FileName = "*.txt";
            dlgSaveTriggers.Filter = "Text files|*.txt|All files|*.*";
            dlgSaveTriggers.Title = "Save trigger list to...";
            // 
            // dlgLoadTriggers
            // 
            dlgLoadTriggers.Filter = "Text files|*.txt|All files|*.*";
            dlgLoadTriggers.Title = "Load trigger list from...";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(367, 450);
            Controls.Add(btnLoad);
            Controls.Add(btnSave);
            Controls.Add(btnAdd);
            Controls.Add(label1);
            Controls.Add(textTriggerName);
            Controls.Add(listTriggers);
            Name = "Form1";
            Text = "Debug Settings";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckedListBox listTriggers;
        private TextBox textTriggerName;
        private Label label1;
        private Button btnAdd;
        private Button btnSave;
        private Button btnLoad;
        private SaveFileDialog dlgSaveTriggers;
        private OpenFileDialog dlgLoadTriggers;
    }
}