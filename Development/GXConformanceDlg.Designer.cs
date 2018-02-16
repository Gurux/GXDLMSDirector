namespace GXDLMSDirector
{
    partial class GXConformanceDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXConformanceDlg));
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ConcurrentReadingCb = new System.Windows.Forms.CheckBox();
            this.ReReadCb = new System.Windows.Forms.CheckBox();
            this.ShowValuesCb = new System.Windows.Forms.CheckBox();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.WriteTestingCb = new System.Windows.Forms.CheckBox();
            this.ExcludeBasicTestsCb = new System.Windows.Forms.CheckBox();
            this.TestsTb = new System.Windows.Forms.TextBox();
            this.DelayTb = new System.Windows.Forms.TextBox();
            this.BrowseBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.DelayLbl = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.helpProvider1.SetHelpNavigator(this.OKBtn, System.Windows.Forms.HelpNavigator.Topic);
            this.OKBtn.Location = new System.Drawing.Point(179, 304);
            this.OKBtn.Name = "OKBtn";
            this.helpProvider1.SetShowHelp(this.OKBtn, true);
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 8;
            this.OKBtn.Text = "&OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.helpProvider1.SetHelpNavigator(this.CancelBtn, System.Windows.Forms.HelpNavigator.Topic);
            this.CancelBtn.Location = new System.Drawing.Point(260, 304);
            this.CancelBtn.Name = "CancelBtn";
            this.helpProvider1.SetShowHelp(this.CancelBtn, true);
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 9;
            this.CancelBtn.Text = "&Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.helpProvider1.SetHelpNavigator(this.logoPictureBox, System.Windows.Forms.HelpNavigator.Topic);
            this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
            this.logoPictureBox.Location = new System.Drawing.Point(9, 9);
            this.logoPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.logoPictureBox.Name = "logoPictureBox";
            this.helpProvider1.SetShowHelp(this.logoPictureBox, true);
            this.logoPictureBox.Size = new System.Drawing.Size(104, 321);
            this.logoPictureBox.TabIndex = 13;
            this.logoPictureBox.TabStop = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(116, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(219, 52);
            this.label2.TabIndex = 14;
            this.label2.Text = "Using Gurux Conformance Tool you can check that your meter is supporting DLMS pro" +
    "tocol.";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(116, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 38);
            this.label1.TabIndex = 15;
            this.label1.Text = "Tests are executed for all the meters on the device list.";
            // 
            // ConcurrentReadingCb
            // 
            this.ConcurrentReadingCb.AutoSize = true;
            this.ConcurrentReadingCb.Checked = true;
            this.ConcurrentReadingCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.helpProvider1.SetHelpKeyword(this.ConcurrentReadingCb, "Concurrent");
            this.helpProvider1.SetHelpNavigator(this.ConcurrentReadingCb, System.Windows.Forms.HelpNavigator.Topic);
            this.helpProvider1.SetHelpString(this.ConcurrentReadingCb, "");
            this.ConcurrentReadingCb.Location = new System.Drawing.Point(119, 93);
            this.ConcurrentReadingCb.Name = "ConcurrentReadingCb";
            this.helpProvider1.SetShowHelp(this.ConcurrentReadingCb, true);
            this.ConcurrentReadingCb.Size = new System.Drawing.Size(112, 17);
            this.ConcurrentReadingCb.TabIndex = 0;
            this.ConcurrentReadingCb.Text = "Concurrent testing";
            this.ConcurrentReadingCb.UseVisualStyleBackColor = true;
            // 
            // ReReadCb
            // 
            this.ReReadCb.AutoSize = true;
            this.helpProvider1.SetHelpKeyword(this.ReReadCb, "UpdateAssociationView");
            this.helpProvider1.SetHelpNavigator(this.ReReadCb, System.Windows.Forms.HelpNavigator.Topic);
            this.ReReadCb.Location = new System.Drawing.Point(119, 133);
            this.ReReadCb.Name = "ReReadCb";
            this.helpProvider1.SetShowHelp(this.ReReadCb, true);
            this.ReReadCb.Size = new System.Drawing.Size(147, 17);
            this.ReReadCb.TabIndex = 2;
            this.ReReadCb.Text = "Re-read Association View";
            this.ReReadCb.UseVisualStyleBackColor = true;
            // 
            // ShowValuesCb
            // 
            this.ShowValuesCb.AutoSize = true;
            this.ShowValuesCb.Checked = true;
            this.ShowValuesCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.helpProvider1.SetHelpKeyword(this.ShowValuesCb, "ShowValues");
            this.helpProvider1.SetHelpNavigator(this.ShowValuesCb, System.Windows.Forms.HelpNavigator.Topic);
            this.ShowValuesCb.Location = new System.Drawing.Point(119, 114);
            this.ShowValuesCb.Name = "ShowValuesCb";
            this.helpProvider1.SetShowHelp(this.ShowValuesCb, true);
            this.ShowValuesCb.Size = new System.Drawing.Size(87, 17);
            this.ShowValuesCb.TabIndex = 1;
            this.ShowValuesCb.Text = "Show values";
            this.ShowValuesCb.UseVisualStyleBackColor = true;
            // 
            // helpProvider1
            // 
            this.helpProvider1.HelpNamespace = "https://www.gurux.fi/GXDLMSDirector.ConformanceTest";
            // 
            // WriteTestingCb
            // 
            this.WriteTestingCb.AutoSize = true;
            this.helpProvider1.SetHelpKeyword(this.WriteTestingCb, "Write");
            this.helpProvider1.SetHelpNavigator(this.WriteTestingCb, System.Windows.Forms.HelpNavigator.Topic);
            this.WriteTestingCb.Location = new System.Drawing.Point(119, 152);
            this.WriteTestingCb.Name = "WriteTestingCb";
            this.helpProvider1.SetShowHelp(this.WriteTestingCb, true);
            this.WriteTestingCb.Size = new System.Drawing.Size(85, 17);
            this.WriteTestingCb.TabIndex = 3;
            this.WriteTestingCb.Text = "Write testing";
            this.WriteTestingCb.UseVisualStyleBackColor = true;
            // 
            // ExcludeBasicTestsCb
            // 
            this.ExcludeBasicTestsCb.AutoSize = true;
            this.helpProvider1.SetHelpKeyword(this.ExcludeBasicTestsCb, "ExcludeBasic");
            this.helpProvider1.SetHelpNavigator(this.ExcludeBasicTestsCb, System.Windows.Forms.HelpNavigator.Topic);
            this.ExcludeBasicTestsCb.Location = new System.Drawing.Point(119, 169);
            this.ExcludeBasicTestsCb.Name = "ExcludeBasicTestsCb";
            this.helpProvider1.SetShowHelp(this.ExcludeBasicTestsCb, true);
            this.ExcludeBasicTestsCb.Size = new System.Drawing.Size(122, 17);
            this.ExcludeBasicTestsCb.TabIndex = 4;
            this.ExcludeBasicTestsCb.Text = "Exclude Basic Tests";
            this.ExcludeBasicTestsCb.UseVisualStyleBackColor = true;
            // 
            // TestsTb
            // 
            this.TestsTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.helpProvider1.SetHelpKeyword(this.TestsTb, "External");
            this.helpProvider1.SetHelpNavigator(this.TestsTb, System.Windows.Forms.HelpNavigator.Topic);
            this.TestsTb.Location = new System.Drawing.Point(119, 239);
            this.TestsTb.Name = "TestsTb";
            this.helpProvider1.SetShowHelp(this.TestsTb, true);
            this.TestsTb.Size = new System.Drawing.Size(216, 20);
            this.TestsTb.TabIndex = 6;
            // 
            // DelayTb
            // 
            this.DelayTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.helpProvider1.SetHelpKeyword(this.DelayTb, "Delay");
            this.helpProvider1.SetHelpNavigator(this.DelayTb, System.Windows.Forms.HelpNavigator.Topic);
            this.DelayTb.Location = new System.Drawing.Point(167, 190);
            this.DelayTb.Name = "DelayTb";
            this.helpProvider1.SetShowHelp(this.DelayTb, true);
            this.DelayTb.Size = new System.Drawing.Size(87, 20);
            this.DelayTb.TabIndex = 5;
            this.DelayTb.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // BrowseBtn
            // 
            this.BrowseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseBtn.Location = new System.Drawing.Point(260, 265);
            this.BrowseBtn.Name = "BrowseBtn";
            this.BrowseBtn.Size = new System.Drawing.Size(75, 23);
            this.BrowseBtn.TabIndex = 7;
            this.BrowseBtn.Text = "Browse...";
            this.BrowseBtn.UseVisualStyleBackColor = true;
            this.BrowseBtn.Click += new System.EventHandler(this.BrowseBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(121, 221);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "External tests:";
            // 
            // DelayLbl
            // 
            this.DelayLbl.AutoSize = true;
            this.DelayLbl.Location = new System.Drawing.Point(117, 193);
            this.DelayLbl.Name = "DelayLbl";
            this.DelayLbl.Size = new System.Drawing.Size(37, 13);
            this.DelayLbl.TabIndex = 20;
            this.DelayLbl.Text = "Delay:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(260, 193);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Seconds.";
            // 
            // GXConformanceDlg
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(347, 339);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DelayLbl);
            this.Controls.Add(this.DelayTb);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BrowseBtn);
            this.Controls.Add(this.TestsTb);
            this.Controls.Add(this.ExcludeBasicTestsCb);
            this.Controls.Add(this.WriteTestingCb);
            this.Controls.Add(this.ShowValuesCb);
            this.Controls.Add(this.ReReadCb);
            this.Controls.Add(this.ConcurrentReadingCb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.logoPictureBox);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.CancelBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.helpProvider1.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GXConformanceDlg";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.helpProvider1.SetShowHelp(this, true);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gurux Conformance Tests";
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ConcurrentReadingCb;
        private System.Windows.Forms.CheckBox ReReadCb;
        private System.Windows.Forms.CheckBox ShowValuesCb;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.CheckBox WriteTestingCb;
        private System.Windows.Forms.CheckBox ExcludeBasicTestsCb;
        private System.Windows.Forms.Button BrowseBtn;
        private System.Windows.Forms.TextBox TestsTb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox DelayTb;
        private System.Windows.Forms.Label DelayLbl;
        private System.Windows.Forms.Label label4;
    }
}
