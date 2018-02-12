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
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Location = new System.Drawing.Point(161, 163);
            this.OKBtn.Name = "OKBtn";
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
            this.CancelBtn.Location = new System.Drawing.Point(242, 163);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 5;
            this.CancelBtn.Text = "&Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
            this.logoPictureBox.Location = new System.Drawing.Point(9, 9);
            this.logoPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(104, 180);
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
            this.ConcurrentReadingCb.Location = new System.Drawing.Point(119, 93);
            this.ConcurrentReadingCb.Name = "ConcurrentReadingCb";
            this.ConcurrentReadingCb.Size = new System.Drawing.Size(112, 17);
            this.ConcurrentReadingCb.TabIndex = 16;
            this.ConcurrentReadingCb.Text = "Concurrent testing";
            this.ConcurrentReadingCb.UseVisualStyleBackColor = true;
            // 
            // ReReadCb
            // 
            this.ReReadCb.AutoSize = true;
            this.ReReadCb.Location = new System.Drawing.Point(119, 140);
            this.ReReadCb.Name = "ReReadCb";
            this.ReReadCb.Size = new System.Drawing.Size(147, 17);
            this.ReReadCb.TabIndex = 17;
            this.ReReadCb.Text = "Re-read Association View";
            this.ReReadCb.UseVisualStyleBackColor = true;
            // 
            // ShowValuesCb
            // 
            this.ShowValuesCb.AutoSize = true;
            this.ShowValuesCb.Checked = true;
            this.ShowValuesCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowValuesCb.Location = new System.Drawing.Point(119, 116);
            this.ShowValuesCb.Name = "ShowValuesCb";
            this.ShowValuesCb.Size = new System.Drawing.Size(87, 17);
            this.ShowValuesCb.TabIndex = 18;
            this.ShowValuesCb.Text = "Show values";
            this.ShowValuesCb.UseVisualStyleBackColor = true;
            // 
            // GXConformanceDlg
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(329, 198);
            this.Controls.Add(this.ShowValuesCb);
            this.Controls.Add(this.ReReadCb);
            this.Controls.Add(this.ConcurrentReadingCb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.logoPictureBox);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.CancelBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GXConformanceDlg";
            this.Padding = new System.Windows.Forms.Padding(9);
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
    }
}
