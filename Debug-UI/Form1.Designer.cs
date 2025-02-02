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
            listDecimals = new CheckedListBox();
            btnAddDecimal = new Button();
            label2 = new Label();
            textDecimalName = new TextBox();
            SuspendLayout();
            // 
            // listTriggers
            // 
            listTriggers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listTriggers.CheckOnClick = true;
            listTriggers.FormattingEnabled = true;
            listTriggers.Location = new Point(12, 34);
            listTriggers.Name = "listTriggers";
            listTriggers.Size = new Size(256, 436);
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
            btnSave.Location = new Point(700, 11);
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
            btnLoad.Location = new Point(700, 40);
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
            dlgSaveTriggers.Filter = "JSON files|*.json|All files|*.*";
            dlgSaveTriggers.Title = "Save trigger list to...";
            // 
            // dlgLoadTriggers
            // 
            dlgLoadTriggers.Filter = "JSON files|*.json|All files|*.*";
            dlgLoadTriggers.Title = "Load trigger list from...";
            // 
            // listDecimals
            // 
            listDecimals.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listDecimals.CheckOnClick = true;
            listDecimals.FormattingEnabled = true;
            listDecimals.Location = new Point(274, 34);
            listDecimals.Name = "listDecimals";
            listDecimals.Size = new Size(256, 436);
            listDecimals.TabIndex = 6;
            listDecimals.ItemCheck += listDecimals_ItemCheck;
            // 
            // btnAddDecimal
            // 
            btnAddDecimal.FlatStyle = FlatStyle.Flat;
            btnAddDecimal.Location = new Point(513, 12);
            btnAddDecimal.Name = "btnAddDecimal";
            btnAddDecimal.Size = new Size(17, 23);
            btnAddDecimal.TabIndex = 9;
            btnAddDecimal.Text = "+";
            btnAddDecimal.UseVisualStyleBackColor = true;
            btnAddDecimal.Click += btnAddDecimal_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(274, 15);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 8;
            label2.Text = "Decimals";
            // 
            // textDecimalName
            // 
            textDecimalName.Location = new Point(328, 12);
            textDecimalName.Name = "textDecimalName";
            textDecimalName.Size = new Size(188, 23);
            textDecimalName.TabIndex = 7;
            textDecimalName.KeyPress += textDecimalName_KeyPress;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(787, 485);
            Controls.Add(btnAddDecimal);
            Controls.Add(label2);
            Controls.Add(textDecimalName);
            Controls.Add(listDecimals);
            Controls.Add(btnLoad);
            Controls.Add(btnSave);
            Controls.Add(btnAdd);
            Controls.Add(label1);
            Controls.Add(textTriggerName);
            Controls.Add(listTriggers);
            Name = "Form1";
            Text = "Debug Settings";
            FormClosing += Form1_FormClosing;
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
        private CheckedListBox listDecimals;
        private Button btnAddDecimal;
        private Label label2;
        private TextBox textDecimalName;
    }
}