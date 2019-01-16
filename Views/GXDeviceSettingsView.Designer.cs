namespace GXDLMSDirector.Views
{
    partial class GXDeviceSettingsView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DeviceInfoView = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PhysicalDeviceAddressLbl = new System.Windows.Forms.Label();
            this.PhysicalDeviceAddressTb = new System.Windows.Forms.TextBox();
            this.NetworkIDTb = new System.Windows.Forms.TextBox();
            this.NetworkIDLbl = new System.Windows.Forms.Label();
            this.DeviceGb = new System.Windows.Forms.GroupBox();
            this.Negotiated = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ProposedConformanceTB = new System.Windows.Forms.TextBox();
            this.AuthenticationTb = new System.Windows.Forms.Label();
            this.AuthenticationLbl = new System.Windows.Forms.Label();
            this.Ciphering = new System.Windows.Forms.GroupBox();
            this.DedicatedKeyTb = new System.Windows.Forms.TextBox();
            this.DedicatedKeyLbl = new System.Windows.Forms.Label();
            this.AuthenticationKeyTb = new System.Windows.Forms.TextBox();
            this.BlockCipherKeyTb = new System.Windows.Forms.TextBox();
            this.ServerSystemTitleLbl = new System.Windows.Forms.Label();
            this.ServerSystemTitleTb = new System.Windows.Forms.TextBox();
            this.ClientSystemTitleTb = new System.Windows.Forms.TextBox();
            this.ClientSystemTitleLbl = new System.Windows.Forms.Label();
            this.SecurityTb = new System.Windows.Forms.Label();
            this.AuthenticationKeyLbl = new System.Windows.Forms.Label();
            this.BlockCipherKeyLbl = new System.Windows.Forms.Label();
            this.SecurityLbl = new System.Windows.Forms.Label();
            this.NegotiatedConformanceTB = new System.Windows.Forms.TextBox();
            this.ConformanceLbl = new System.Windows.Forms.Label();
            this.ManufacturerLbl = new System.Windows.Forms.Label();
            this.ManufacturerValueLbl = new System.Windows.Forms.Label();
            this.PhysicalAddressLbl = new System.Windows.Forms.Label();
            this.ClientAddressValueLbl = new System.Windows.Forms.Label();
            this.PhysicalAddressValueLbl = new System.Windows.Forms.Label();
            this.ClientAddressLbl = new System.Windows.Forms.Label();
            this.LogicalAddressLbl = new System.Windows.Forms.Label();
            this.LogicalAddressValueLbl = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.ErrorsView = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DeviceInfoView.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.DeviceGb.SuspendLayout();
            this.Ciphering.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // DeviceInfoView
            // 
            this.DeviceInfoView.Controls.Add(this.tabPage4);
            this.DeviceInfoView.Controls.Add(this.tabPage5);
            this.DeviceInfoView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeviceInfoView.Location = new System.Drawing.Point(0, 0);
            this.DeviceInfoView.Name = "DeviceInfoView";
            this.DeviceInfoView.SelectedIndex = 0;
            this.DeviceInfoView.Size = new System.Drawing.Size(521, 294);
            this.DeviceInfoView.TabIndex = 38;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.groupBox1);
            this.tabPage4.Controls.Add(this.DeviceGb);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(513, 268);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "General";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.PhysicalDeviceAddressLbl);
            this.groupBox1.Controls.Add(this.PhysicalDeviceAddressTb);
            this.groupBox1.Controls.Add(this.NetworkIDTb);
            this.groupBox1.Controls.Add(this.NetworkIDLbl);
            this.groupBox1.Location = new System.Drawing.Point(2, 327);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(510, 7);
            this.groupBox1.TabIndex = 71;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Gateway";
            // 
            // PhysicalDeviceAddressLbl
            // 
            this.PhysicalDeviceAddressLbl.AutoSize = true;
            this.PhysicalDeviceAddressLbl.Location = new System.Drawing.Point(173, 21);
            this.PhysicalDeviceAddressLbl.Name = "PhysicalDeviceAddressLbl";
            this.PhysicalDeviceAddressLbl.Size = new System.Drawing.Size(85, 13);
            this.PhysicalDeviceAddressLbl.TabIndex = 71;
            this.PhysicalDeviceAddressLbl.Text = "Device Address:";
            // 
            // PhysicalDeviceAddressTb
            // 
            this.PhysicalDeviceAddressTb.Location = new System.Drawing.Point(277, 18);
            this.PhysicalDeviceAddressTb.Name = "PhysicalDeviceAddressTb";
            this.PhysicalDeviceAddressTb.ReadOnly = true;
            this.PhysicalDeviceAddressTb.Size = new System.Drawing.Size(127, 20);
            this.PhysicalDeviceAddressTb.TabIndex = 70;
            // 
            // NetworkIDTb
            // 
            this.NetworkIDTb.Location = new System.Drawing.Point(115, 19);
            this.NetworkIDTb.Name = "NetworkIDTb";
            this.NetworkIDTb.ReadOnly = true;
            this.NetworkIDTb.Size = new System.Drawing.Size(46, 20);
            this.NetworkIDTb.TabIndex = 69;
            // 
            // NetworkIDLbl
            // 
            this.NetworkIDLbl.AutoSize = true;
            this.NetworkIDLbl.Location = new System.Drawing.Point(11, 21);
            this.NetworkIDLbl.Name = "NetworkIDLbl";
            this.NetworkIDLbl.Size = new System.Drawing.Size(64, 13);
            this.NetworkIDLbl.TabIndex = 68;
            this.NetworkIDLbl.Text = "Network ID:";
            // 
            // DeviceGb
            // 
            this.DeviceGb.Controls.Add(this.Negotiated);
            this.DeviceGb.Controls.Add(this.label1);
            this.DeviceGb.Controls.Add(this.ProposedConformanceTB);
            this.DeviceGb.Controls.Add(this.AuthenticationTb);
            this.DeviceGb.Controls.Add(this.AuthenticationLbl);
            this.DeviceGb.Controls.Add(this.Ciphering);
            this.DeviceGb.Controls.Add(this.NegotiatedConformanceTB);
            this.DeviceGb.Controls.Add(this.ConformanceLbl);
            this.DeviceGb.Controls.Add(this.ManufacturerLbl);
            this.DeviceGb.Controls.Add(this.ManufacturerValueLbl);
            this.DeviceGb.Controls.Add(this.PhysicalAddressLbl);
            this.DeviceGb.Controls.Add(this.ClientAddressValueLbl);
            this.DeviceGb.Controls.Add(this.PhysicalAddressValueLbl);
            this.DeviceGb.Controls.Add(this.ClientAddressLbl);
            this.DeviceGb.Controls.Add(this.LogicalAddressLbl);
            this.DeviceGb.Controls.Add(this.LogicalAddressValueLbl);
            this.DeviceGb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeviceGb.Location = new System.Drawing.Point(3, 3);
            this.DeviceGb.Name = "DeviceGb";
            this.DeviceGb.Size = new System.Drawing.Size(507, 262);
            this.DeviceGb.TabIndex = 13;
            this.DeviceGb.TabStop = false;
            // 
            // Negotiated
            // 
            this.Negotiated.AutoSize = true;
            this.Negotiated.Location = new System.Drawing.Point(341, 127);
            this.Negotiated.Name = "Negotiated";
            this.Negotiated.Size = new System.Drawing.Size(59, 13);
            this.Negotiated.TabIndex = 73;
            this.Negotiated.Text = "Negotiated";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(111, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 72;
            this.label1.Text = "Proposed:";
            // 
            // ProposedConformanceTB
            // 
            this.ProposedConformanceTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ProposedConformanceTB.Location = new System.Drawing.Point(111, 143);
            this.ProposedConformanceTB.Multiline = true;
            this.ProposedConformanceTB.Name = "ProposedConformanceTB";
            this.ProposedConformanceTB.ReadOnly = true;
            this.ProposedConformanceTB.Size = new System.Drawing.Size(203, 36);
            this.ProposedConformanceTB.TabIndex = 71;
            // 
            // AuthenticationTb
            // 
            this.AuthenticationTb.AutoSize = true;
            this.AuthenticationTb.Location = new System.Drawing.Point(111, 108);
            this.AuthenticationTb.Name = "AuthenticationTb";
            this.AuthenticationTb.Size = new System.Drawing.Size(35, 13);
            this.AuthenticationTb.TabIndex = 70;
            this.AuthenticationTb.Text = "label1";
            // 
            // AuthenticationLbl
            // 
            this.AuthenticationLbl.AutoSize = true;
            this.AuthenticationLbl.Location = new System.Drawing.Point(6, 108);
            this.AuthenticationLbl.Name = "AuthenticationLbl";
            this.AuthenticationLbl.Size = new System.Drawing.Size(78, 13);
            this.AuthenticationLbl.TabIndex = 69;
            this.AuthenticationLbl.Text = "Authentication:";
            // 
            // Ciphering
            // 
            this.Ciphering.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Ciphering.Controls.Add(this.DedicatedKeyTb);
            this.Ciphering.Controls.Add(this.DedicatedKeyLbl);
            this.Ciphering.Controls.Add(this.AuthenticationKeyTb);
            this.Ciphering.Controls.Add(this.BlockCipherKeyTb);
            this.Ciphering.Controls.Add(this.ServerSystemTitleLbl);
            this.Ciphering.Controls.Add(this.ServerSystemTitleTb);
            this.Ciphering.Controls.Add(this.ClientSystemTitleTb);
            this.Ciphering.Controls.Add(this.ClientSystemTitleLbl);
            this.Ciphering.Controls.Add(this.SecurityTb);
            this.Ciphering.Controls.Add(this.AuthenticationKeyLbl);
            this.Ciphering.Controls.Add(this.BlockCipherKeyLbl);
            this.Ciphering.Controls.Add(this.SecurityLbl);
            this.Ciphering.Location = new System.Drawing.Point(0, 188);
            this.Ciphering.Name = "Ciphering";
            this.Ciphering.Size = new System.Drawing.Size(510, 7);
            this.Ciphering.TabIndex = 68;
            this.Ciphering.TabStop = false;
            this.Ciphering.Text = "Ciphering";
            // 
            // DedicatedKeyTb
            // 
            this.DedicatedKeyTb.Location = new System.Drawing.Point(114, 98);
            this.DedicatedKeyTb.Name = "DedicatedKeyTb";
            this.DedicatedKeyTb.ReadOnly = true;
            this.DedicatedKeyTb.Size = new System.Drawing.Size(164, 20);
            this.DedicatedKeyTb.TabIndex = 75;
            // 
            // DedicatedKeyLbl
            // 
            this.DedicatedKeyLbl.AutoSize = true;
            this.DedicatedKeyLbl.Location = new System.Drawing.Point(10, 99);
            this.DedicatedKeyLbl.Name = "DedicatedKeyLbl";
            this.DedicatedKeyLbl.Size = new System.Drawing.Size(80, 13);
            this.DedicatedKeyLbl.TabIndex = 74;
            this.DedicatedKeyLbl.Text = "Dedicated Key:";
            // 
            // AuthenticationKeyTb
            // 
            this.AuthenticationKeyTb.Location = new System.Drawing.Point(392, 72);
            this.AuthenticationKeyTb.Name = "AuthenticationKeyTb";
            this.AuthenticationKeyTb.ReadOnly = true;
            this.AuthenticationKeyTb.Size = new System.Drawing.Size(164, 20);
            this.AuthenticationKeyTb.TabIndex = 73;
            // 
            // BlockCipherKeyTb
            // 
            this.BlockCipherKeyTb.Location = new System.Drawing.Point(114, 72);
            this.BlockCipherKeyTb.Name = "BlockCipherKeyTb";
            this.BlockCipherKeyTb.ReadOnly = true;
            this.BlockCipherKeyTb.Size = new System.Drawing.Size(164, 20);
            this.BlockCipherKeyTb.TabIndex = 72;
            // 
            // ServerSystemTitleLbl
            // 
            this.ServerSystemTitleLbl.AutoSize = true;
            this.ServerSystemTitleLbl.Location = new System.Drawing.Point(288, 48);
            this.ServerSystemTitleLbl.Name = "ServerSystemTitleLbl";
            this.ServerSystemTitleLbl.Size = new System.Drawing.Size(101, 13);
            this.ServerSystemTitleLbl.TabIndex = 71;
            this.ServerSystemTitleLbl.Text = "Server System Title:";
            // 
            // ServerSystemTitleTb
            // 
            this.ServerSystemTitleTb.Location = new System.Drawing.Point(392, 45);
            this.ServerSystemTitleTb.Name = "ServerSystemTitleTb";
            this.ServerSystemTitleTb.ReadOnly = true;
            this.ServerSystemTitleTb.Size = new System.Drawing.Size(164, 20);
            this.ServerSystemTitleTb.TabIndex = 70;
            // 
            // ClientSystemTitleTb
            // 
            this.ClientSystemTitleTb.Location = new System.Drawing.Point(114, 43);
            this.ClientSystemTitleTb.Name = "ClientSystemTitleTb";
            this.ClientSystemTitleTb.ReadOnly = true;
            this.ClientSystemTitleTb.Size = new System.Drawing.Size(164, 20);
            this.ClientSystemTitleTb.TabIndex = 69;
            // 
            // ClientSystemTitleLbl
            // 
            this.ClientSystemTitleLbl.AutoSize = true;
            this.ClientSystemTitleLbl.Location = new System.Drawing.Point(10, 45);
            this.ClientSystemTitleLbl.Name = "ClientSystemTitleLbl";
            this.ClientSystemTitleLbl.Size = new System.Drawing.Size(96, 13);
            this.ClientSystemTitleLbl.TabIndex = 68;
            this.ClientSystemTitleLbl.Text = "Client System Title:";
            // 
            // SecurityTb
            // 
            this.SecurityTb.AutoSize = true;
            this.SecurityTb.Location = new System.Drawing.Point(115, 22);
            this.SecurityTb.Name = "SecurityTb";
            this.SecurityTb.Size = new System.Drawing.Size(45, 13);
            this.SecurityTb.TabIndex = 56;
            this.SecurityTb.Text = "Security";
            // 
            // AuthenticationKeyLbl
            // 
            this.AuthenticationKeyLbl.AutoSize = true;
            this.AuthenticationKeyLbl.Location = new System.Drawing.Point(288, 73);
            this.AuthenticationKeyLbl.Name = "AuthenticationKeyLbl";
            this.AuthenticationKeyLbl.Size = new System.Drawing.Size(99, 13);
            this.AuthenticationKeyLbl.TabIndex = 48;
            this.AuthenticationKeyLbl.Text = "Authentication Key:";
            // 
            // BlockCipherKeyLbl
            // 
            this.BlockCipherKeyLbl.AutoSize = true;
            this.BlockCipherKeyLbl.Location = new System.Drawing.Point(10, 72);
            this.BlockCipherKeyLbl.Name = "BlockCipherKeyLbl";
            this.BlockCipherKeyLbl.Size = new System.Drawing.Size(91, 13);
            this.BlockCipherKeyLbl.TabIndex = 47;
            this.BlockCipherKeyLbl.Text = "Block Cipher Key:";
            // 
            // SecurityLbl
            // 
            this.SecurityLbl.AutoSize = true;
            this.SecurityLbl.Location = new System.Drawing.Point(10, 22);
            this.SecurityLbl.Name = "SecurityLbl";
            this.SecurityLbl.Size = new System.Drawing.Size(48, 13);
            this.SecurityLbl.TabIndex = 46;
            this.SecurityLbl.Text = "Security:";
            // 
            // NegotiatedConformanceTB
            // 
            this.NegotiatedConformanceTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.NegotiatedConformanceTB.Location = new System.Drawing.Point(344, 143);
            this.NegotiatedConformanceTB.Multiline = true;
            this.NegotiatedConformanceTB.Name = "NegotiatedConformanceTB";
            this.NegotiatedConformanceTB.ReadOnly = true;
            this.NegotiatedConformanceTB.Size = new System.Drawing.Size(212, 36);
            this.NegotiatedConformanceTB.TabIndex = 63;
            // 
            // ConformanceLbl
            // 
            this.ConformanceLbl.AutoSize = true;
            this.ConformanceLbl.Location = new System.Drawing.Point(6, 143);
            this.ConformanceLbl.Name = "ConformanceLbl";
            this.ConformanceLbl.Size = new System.Drawing.Size(73, 13);
            this.ConformanceLbl.TabIndex = 62;
            this.ConformanceLbl.Text = "Conformance:";
            // 
            // ManufacturerLbl
            // 
            this.ManufacturerLbl.AutoSize = true;
            this.ManufacturerLbl.Location = new System.Drawing.Point(6, 16);
            this.ManufacturerLbl.Name = "ManufacturerLbl";
            this.ManufacturerLbl.Size = new System.Drawing.Size(73, 13);
            this.ManufacturerLbl.TabIndex = 54;
            this.ManufacturerLbl.Text = "Manufacturer:";
            // 
            // ManufacturerValueLbl
            // 
            this.ManufacturerValueLbl.AutoSize = true;
            this.ManufacturerValueLbl.Location = new System.Drawing.Point(111, 16);
            this.ManufacturerValueLbl.Name = "ManufacturerValueLbl";
            this.ManufacturerValueLbl.Size = new System.Drawing.Size(111, 13);
            this.ManufacturerValueLbl.TabIndex = 55;
            this.ManufacturerValueLbl.Text = "ManufacturerValueLbl";
            // 
            // PhysicalAddressLbl
            // 
            this.PhysicalAddressLbl.AutoSize = true;
            this.PhysicalAddressLbl.Location = new System.Drawing.Point(6, 39);
            this.PhysicalAddressLbl.Name = "PhysicalAddressLbl";
            this.PhysicalAddressLbl.Size = new System.Drawing.Size(90, 13);
            this.PhysicalAddressLbl.TabIndex = 56;
            this.PhysicalAddressLbl.Text = "Physical Address:";
            // 
            // ClientAddressValueLbl
            // 
            this.ClientAddressValueLbl.AutoSize = true;
            this.ClientAddressValueLbl.Location = new System.Drawing.Point(111, 85);
            this.ClientAddressValueLbl.Name = "ClientAddressValueLbl";
            this.ClientAddressValueLbl.Size = new System.Drawing.Size(112, 13);
            this.ClientAddressValueLbl.TabIndex = 61;
            this.ClientAddressValueLbl.Text = "ClientAddressValueLbl";
            // 
            // PhysicalAddressValueLbl
            // 
            this.PhysicalAddressValueLbl.AutoSize = true;
            this.PhysicalAddressValueLbl.Location = new System.Drawing.Point(111, 39);
            this.PhysicalAddressValueLbl.Name = "PhysicalAddressValueLbl";
            this.PhysicalAddressValueLbl.Size = new System.Drawing.Size(125, 13);
            this.PhysicalAddressValueLbl.TabIndex = 57;
            this.PhysicalAddressValueLbl.Text = "PhysicalAddressValueLbl";
            // 
            // ClientAddressLbl
            // 
            this.ClientAddressLbl.AutoSize = true;
            this.ClientAddressLbl.Location = new System.Drawing.Point(6, 85);
            this.ClientAddressLbl.Name = "ClientAddressLbl";
            this.ClientAddressLbl.Size = new System.Drawing.Size(77, 13);
            this.ClientAddressLbl.TabIndex = 60;
            this.ClientAddressLbl.Text = "Client Address:";
            // 
            // LogicalAddressLbl
            // 
            this.LogicalAddressLbl.AutoSize = true;
            this.LogicalAddressLbl.Location = new System.Drawing.Point(6, 62);
            this.LogicalAddressLbl.Name = "LogicalAddressLbl";
            this.LogicalAddressLbl.Size = new System.Drawing.Size(85, 13);
            this.LogicalAddressLbl.TabIndex = 58;
            this.LogicalAddressLbl.Text = "Logical Address:";
            // 
            // LogicalAddressValueLbl
            // 
            this.LogicalAddressValueLbl.AutoSize = true;
            this.LogicalAddressValueLbl.Location = new System.Drawing.Point(111, 62);
            this.LogicalAddressValueLbl.Name = "LogicalAddressValueLbl";
            this.LogicalAddressValueLbl.Size = new System.Drawing.Size(120, 13);
            this.LogicalAddressValueLbl.TabIndex = 59;
            this.LogicalAddressValueLbl.Text = "LogicalAddressValueLbl";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.ErrorsView);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(510, 261);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Last errors";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // ErrorsView
            // 
            this.ErrorsView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.ErrorsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorsView.Location = new System.Drawing.Point(3, 3);
            this.ErrorsView.MultiSelect = false;
            this.ErrorsView.Name = "ErrorsView";
            this.ErrorsView.Size = new System.Drawing.Size(504, 255);
            this.ErrorsView.TabIndex = 35;
            this.ErrorsView.UseCompatibleStateImageBehavior = false;
            this.ErrorsView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 117;
            // 
            // GXDeviceSettingsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DeviceInfoView);
            this.Name = "GXDeviceSettingsView";
            this.Size = new System.Drawing.Size(521, 294);
            this.DeviceInfoView.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.DeviceGb.ResumeLayout(false);
            this.DeviceGb.PerformLayout();
            this.Ciphering.ResumeLayout(false);
            this.Ciphering.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl DeviceInfoView;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label PhysicalDeviceAddressLbl;
        private System.Windows.Forms.TextBox PhysicalDeviceAddressTb;
        private System.Windows.Forms.TextBox NetworkIDTb;
        private System.Windows.Forms.Label NetworkIDLbl;
        private System.Windows.Forms.GroupBox DeviceGb;
        private System.Windows.Forms.Label Negotiated;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ProposedConformanceTB;
        private System.Windows.Forms.Label AuthenticationTb;
        private System.Windows.Forms.Label AuthenticationLbl;
        private System.Windows.Forms.GroupBox Ciphering;
        private System.Windows.Forms.TextBox DedicatedKeyTb;
        private System.Windows.Forms.Label DedicatedKeyLbl;
        private System.Windows.Forms.TextBox AuthenticationKeyTb;
        private System.Windows.Forms.TextBox BlockCipherKeyTb;
        private System.Windows.Forms.Label ServerSystemTitleLbl;
        private System.Windows.Forms.TextBox ServerSystemTitleTb;
        private System.Windows.Forms.TextBox ClientSystemTitleTb;
        private System.Windows.Forms.Label ClientSystemTitleLbl;
        private System.Windows.Forms.Label SecurityTb;
        private System.Windows.Forms.Label AuthenticationKeyLbl;
        private System.Windows.Forms.Label BlockCipherKeyLbl;
        private System.Windows.Forms.Label SecurityLbl;
        private System.Windows.Forms.TextBox NegotiatedConformanceTB;
        private System.Windows.Forms.Label ConformanceLbl;
        private System.Windows.Forms.Label ManufacturerLbl;
        private System.Windows.Forms.Label ManufacturerValueLbl;
        private System.Windows.Forms.Label PhysicalAddressLbl;
        private System.Windows.Forms.Label ClientAddressValueLbl;
        private System.Windows.Forms.Label PhysicalAddressValueLbl;
        private System.Windows.Forms.Label ClientAddressLbl;
        private System.Windows.Forms.Label LogicalAddressLbl;
        private System.Windows.Forms.Label LogicalAddressValueLbl;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.ListView ErrorsView;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}
