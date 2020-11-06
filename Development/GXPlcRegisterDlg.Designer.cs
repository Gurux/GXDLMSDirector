namespace GXDLMSDirector
{
    partial class GXPlcRegisterDlg
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
            this.MACAddressTb = new System.Windows.Forms.TextBox();
            this.MACAddressLbl = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OkBtn = new System.Windows.Forms.Button();
            this.SystemTitleTb = new System.Windows.Forms.TextBox();
            this.SystemTitleLbl = new System.Windows.Forms.Label();
            this.ActiveInitiatorTb = new System.Windows.Forms.TextBox();
            this.ActiveInitiatorLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MACAddressTb
            // 
            this.MACAddressTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MACAddressTb.Location = new System.Drawing.Point(98, 64);
            this.MACAddressTb.Name = "MACAddressTb";
            this.MACAddressTb.Size = new System.Drawing.Size(191, 20);
            this.MACAddressTb.TabIndex = 2;
            // 
            // MACAddressLbl
            // 
            this.MACAddressLbl.AutoSize = true;
            this.MACAddressLbl.Location = new System.Drawing.Point(9, 67);
            this.MACAddressLbl.Name = "MACAddressLbl";
            this.MACAddressLbl.Size = new System.Drawing.Size(74, 13);
            this.MACAddressLbl.TabIndex = 19;
            this.MACAddressLbl.Text = "MAC Address:";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(214, 102);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 4;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // OkBtn
            // 
            this.OkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkBtn.Location = new System.Drawing.Point(133, 102);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 3;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // SystemTitleTb
            // 
            this.SystemTitleTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SystemTitleTb.Location = new System.Drawing.Point(98, 38);
            this.SystemTitleTb.Name = "SystemTitleTb";
            this.SystemTitleTb.Size = new System.Drawing.Size(191, 20);
            this.SystemTitleTb.TabIndex = 1;
            // 
            // SystemTitleLbl
            // 
            this.SystemTitleLbl.AutoSize = true;
            this.SystemTitleLbl.Location = new System.Drawing.Point(9, 41);
            this.SystemTitleLbl.Name = "SystemTitleLbl";
            this.SystemTitleLbl.Size = new System.Drawing.Size(67, 13);
            this.SystemTitleLbl.TabIndex = 21;
            this.SystemTitleLbl.Text = "System Title:";
            // 
            // ActiveInitiatorTb
            // 
            this.ActiveInitiatorTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ActiveInitiatorTb.Location = new System.Drawing.Point(98, 12);
            this.ActiveInitiatorTb.Name = "ActiveInitiatorTb";
            this.ActiveInitiatorTb.Size = new System.Drawing.Size(191, 20);
            this.ActiveInitiatorTb.TabIndex = 0;
            // 
            // ActiveInitiatorLbl
            // 
            this.ActiveInitiatorLbl.AutoSize = true;
            this.ActiveInitiatorLbl.Location = new System.Drawing.Point(9, 15);
            this.ActiveInitiatorLbl.Name = "ActiveInitiatorLbl";
            this.ActiveInitiatorLbl.Size = new System.Drawing.Size(77, 13);
            this.ActiveInitiatorLbl.TabIndex = 23;
            this.ActiveInitiatorLbl.Text = "Active Initiator:";
            // 
            // GXPlcRegisterDlg
            // 
            this.AcceptButton = this.OkBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(301, 137);
            this.ControlBox = false;
            this.Controls.Add(this.ActiveInitiatorTb);
            this.Controls.Add(this.ActiveInitiatorLbl);
            this.Controls.Add(this.SystemTitleTb);
            this.Controls.Add(this.SystemTitleLbl);
            this.Controls.Add(this.MACAddressTb);
            this.Controls.Add(this.MACAddressLbl);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OkBtn);
            this.Name = "GXPlcRegisterDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Registering PLC device";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox MACAddressTb;
        private System.Windows.Forms.Label MACAddressLbl;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.TextBox SystemTitleTb;
        private System.Windows.Forms.Label SystemTitleLbl;
        private System.Windows.Forms.TextBox ActiveInitiatorTb;
        private System.Windows.Forms.Label ActiveInitiatorLbl;
    }
}