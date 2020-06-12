namespace GXDLMSDirector
{
    partial class GXSerialMonitor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXSerialMonitor));
            this.label1 = new System.Windows.Forms.Label();
            this.Trace = new System.Windows.Forms.TextBox();
            this.ClearBtb = new System.Windows.Forms.Button();
            this.SettingsBtn = new System.Windows.Forms.Button();
            this.OpenBtn = new System.Windows.Forms.Button();
            this.PortCb = new System.Windows.Forms.ComboBox();
            this.HexCb = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Serial port:";
            // 
            // Trace
            // 
            this.Trace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trace.Location = new System.Drawing.Point(15, 37);
            this.Trace.Multiline = true;
            this.Trace.Name = "Trace";
            this.Trace.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Trace.Size = new System.Drawing.Size(451, 191);
            this.Trace.TabIndex = 2;
            // 
            // ClearBtb
            // 
            this.ClearBtb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearBtb.Location = new System.Drawing.Point(391, 230);
            this.ClearBtb.Name = "ClearBtb";
            this.ClearBtb.Size = new System.Drawing.Size(75, 23);
            this.ClearBtb.TabIndex = 3;
            this.ClearBtb.Text = "Clear output";
            this.ClearBtb.UseVisualStyleBackColor = true;
            this.ClearBtb.Click += new System.EventHandler(this.ClearBtb_Click);
            // 
            // SettingsBtn
            // 
            this.SettingsBtn.Location = new System.Drawing.Point(185, 9);
            this.SettingsBtn.Name = "SettingsBtn";
            this.SettingsBtn.Size = new System.Drawing.Size(75, 23);
            this.SettingsBtn.TabIndex = 4;
            this.SettingsBtn.Text = "Settings...";
            this.SettingsBtn.UseVisualStyleBackColor = true;
            this.SettingsBtn.Click += new System.EventHandler(this.SettingsBtn_Click);
            // 
            // OpenBtn
            // 
            this.OpenBtn.Location = new System.Drawing.Point(266, 9);
            this.OpenBtn.Name = "OpenBtn";
            this.OpenBtn.Size = new System.Drawing.Size(75, 23);
            this.OpenBtn.TabIndex = 5;
            this.OpenBtn.Text = "Open";
            this.OpenBtn.UseVisualStyleBackColor = true;
            this.OpenBtn.Click += new System.EventHandler(this.OpenBtn_Click);
            // 
            // PortCb
            // 
            this.PortCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PortCb.FormattingEnabled = true;
            this.PortCb.Location = new System.Drawing.Point(75, 10);
            this.PortCb.Name = "PortCb";
            this.PortCb.Size = new System.Drawing.Size(104, 21);
            this.PortCb.TabIndex = 6;
            this.PortCb.SelectedIndexChanged += new System.EventHandler(this.PortCb_SelectedIndexChanged);
            // 
            // HexCb
            // 
            this.HexCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.HexCb.AutoSize = true;
            this.HexCb.Location = new System.Drawing.Point(15, 234);
            this.HexCb.Name = "HexCb";
            this.HexCb.Size = new System.Drawing.Size(45, 17);
            this.HexCb.TabIndex = 7;
            this.HexCb.Text = "Hex";
            this.HexCb.UseVisualStyleBackColor = true;
            this.HexCb.CheckedChanged += new System.EventHandler(this.HexCb_CheckedChanged);
            // 
            // GXSerialMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 255);
            this.Controls.Add(this.HexCb);
            this.Controls.Add(this.PortCb);
            this.Controls.Add(this.OpenBtn);
            this.Controls.Add(this.SettingsBtn);
            this.Controls.Add(this.ClearBtb);
            this.Controls.Add(this.Trace);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GXSerialMonitor";
            this.ShowInTaskbar = false;
            this.Text = "Serial Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GXSerialMonitor_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Trace;
        private System.Windows.Forms.Button ClearBtb;
        private System.Windows.Forms.Button SettingsBtn;
        private System.Windows.Forms.Button OpenBtn;
        private System.Windows.Forms.ComboBox PortCb;
        private System.Windows.Forms.CheckBox HexCb;
    }
}