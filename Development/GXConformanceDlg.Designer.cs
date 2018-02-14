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
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.helpProvider1.SetHelpNavigator(this.OKBtn, System.Windows.Forms.HelpNavigator.Topic);
            this.OKBtn.Location = new System.Drawing.Point(161, 193);
            this.OKBtn.Name = "OKBtn";
            this.helpProvider1.SetShowHelp(this.OKBtn, true);
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 4;
            this.OKBtn.Text = "&OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.helpProvider1.SetHelpNavigator(this.CancelBtn, System.Windows.Forms.HelpNavigator.Topic);
            this.CancelBtn.Location = new System.Drawing.Point(242, 193);
            this.CancelBtn.Name = "CancelBtn";
            this.helpProvider1.SetShowHelp(this.CancelBtn, true);
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 5;
            this.CancelBtn.Text = "&Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
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
            this.logoPictureBox.Size = new System.Drawing.Size(104, 210);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoPictureBox.TabIndex = 13;
            this.logoPictureBox.TabStop = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(116, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(191, 52);
            this.label2.TabIndex = 14;
            this.label2.Text = "Using Gurux Conformance Tool you can check that your meter is supporting DLMS pro" +
    "tocol.";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(116, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 38);
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
            this.ReReadCb.Location = new System.Drawing.Point(119, 140);
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
            this.ShowValuesCb.Location = new System.Drawing.Point(119, 116);
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
            this.helpProvider1.SetHelpKeyword(this.WriteTestingCb, "UpdateAssociationView");
            this.helpProvider1.SetHelpNavigator(this.WriteTestingCb, System.Windows.Forms.HelpNavigator.Topic);
            this.WriteTestingCb.Location = new System.Drawing.Point(119, 163);
            this.WriteTestingCb.Name = "WriteTestingCb";
            this.helpProvider1.SetShowHelp(this.WriteTestingCb, true);
            this.WriteTestingCb.Size = new System.Drawing.Size(85, 17);
            this.WriteTestingCb.TabIndex = 3;
            this.WriteTestingCb.Text = "Write testing";
            this.WriteTestingCb.UseVisualStyleBackColor = true;
            // 
            // GXConformanceDlg
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(329, 228);
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
    }
}
