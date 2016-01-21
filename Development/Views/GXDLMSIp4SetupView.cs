//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/Views/GXDLMSIp4SetupView.cs $
//
// Version:         $Revision: 8063 $,
//                  $Date: 2016-01-20 14:17:03 +0200 (ke, 20 tammi 2016) $
//                  $Author: kurumi $
//
// Copyright (c) Gurux Ltd
//
//---------------------------------------------------------------------------
//
//  DESCRIPTION
//
// This file is a part of Gurux Device Framework.
//
// Gurux Device Framework is Open Source software; you can redistribute it
// and/or modify it under the terms of the GNU General Public License 
// as published by the Free Software Foundation; version 2 of the License.
// Gurux Device Framework is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details.
//
// More information of Gurux DLMS/COSEM Director: http://www.gurux.org/GXDLMSDirector
//
// This code is licensed under the GNU General Public License v2. 
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Gurux.DLMS.Objects;
using Gurux.DLMS;
using Gurux.DLMS.Enums;

namespace GXDLMSDirector.Views
{
    [GXDLMSViewAttribute(typeof(GXDLMSIp4Setup))]
    class GXDLMSIp4SetupView : Form, IGXDLMSView
    {
        private GroupBox groupBox1;
        private Label DataLinkLayerLbl;
        private GXValueField LogicalNameTB;
        private GXValueField SubnetMaskTB;
        private Label SubnetMaskLbl;
        private GXValueField IPOptionsTB;
        private Label IPOptionsLbl;
        private GXValueField MulticastIPAddressTB;
        private Label MulticastIPAddressLbl;
        private GXValueField IPAddressTB;
        private Label IPAddressLbl;
        private GXValueField DataLinkLayerTB;
        private GXValueField SecondaryDNSAddressTB;
        private Label SecondaryDNSAddressLbl;
        private GXValueField PrimaryDNSAddressTB;
        private Label PrimaryDNSAddressLbl;
        private Label UseDHCPLbl;
        private GXValueField GatewayIPAddressTB;
        private Label GatewayIPAddressLbl;
        private CheckBox UseDHCCB;
        private ErrorProvider errorProvider1;
        private System.ComponentModel.IContainer components;
        private Label LogicalNameLbl;
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSIp4SetupView()
        {
            InitializeComponent();
        }
        #region IGXDLMSView Members

        public GXDLMSObject Target
        {
            get;
            set;
        }

        public void OnValueChanged(int attributeID, object value)
        {            
            if (attributeID == 8)
            {
                GXDLMSIp4Setup target = Target as GXDLMSIp4Setup;
                this.UseDHCCB.Checked = target.UseDHCP;
            }
            else if (attributeID != 0)
            {
                throw new IndexOutOfRangeException("attributeID");
            }
        }

        public void OnAccessRightsChange(int attributeID, AccessMode access)
        {
            if (attributeID == 8)
            {
                this.UseDHCCB.Enabled = (access & AccessMode.Write) != 0;
            }
            else
            {
                throw new IndexOutOfRangeException("attributeID");
            }
        }

        public System.Windows.Forms.ErrorProvider ErrorProvider
        {
            get
            {
                return errorProvider1;
            }
        }

        public string Description
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public void OnDirtyChange(int attributeID, bool Dirty)
        {

        }

        #endregion


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDLMSIp4SetupView));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.UseDHCCB = new System.Windows.Forms.CheckBox();
            this.SecondaryDNSAddressTB = new GXDLMSDirector.Views.GXValueField();
            this.SecondaryDNSAddressLbl = new System.Windows.Forms.Label();
            this.PrimaryDNSAddressTB = new GXDLMSDirector.Views.GXValueField();
            this.PrimaryDNSAddressLbl = new System.Windows.Forms.Label();
            this.UseDHCPLbl = new System.Windows.Forms.Label();
            this.GatewayIPAddressTB = new GXDLMSDirector.Views.GXValueField();
            this.GatewayIPAddressLbl = new System.Windows.Forms.Label();
            this.SubnetMaskTB = new GXDLMSDirector.Views.GXValueField();
            this.SubnetMaskLbl = new System.Windows.Forms.Label();
            this.IPOptionsTB = new GXDLMSDirector.Views.GXValueField();
            this.IPOptionsLbl = new System.Windows.Forms.Label();
            this.MulticastIPAddressTB = new GXDLMSDirector.Views.GXValueField();
            this.MulticastIPAddressLbl = new System.Windows.Forms.Label();
            this.IPAddressTB = new GXDLMSDirector.Views.GXValueField();
            this.IPAddressLbl = new System.Windows.Forms.Label();
            this.DataLinkLayerTB = new GXDLMSDirector.Views.GXValueField();
            this.DataLinkLayerLbl = new System.Windows.Forms.Label();
            this.LogicalNameTB = new GXDLMSDirector.Views.GXValueField();
            this.LogicalNameLbl = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.UseDHCCB);
            this.groupBox1.Controls.Add(this.SecondaryDNSAddressTB);
            this.groupBox1.Controls.Add(this.SecondaryDNSAddressLbl);
            this.groupBox1.Controls.Add(this.PrimaryDNSAddressTB);
            this.groupBox1.Controls.Add(this.PrimaryDNSAddressLbl);
            this.groupBox1.Controls.Add(this.UseDHCPLbl);
            this.groupBox1.Controls.Add(this.GatewayIPAddressTB);
            this.groupBox1.Controls.Add(this.GatewayIPAddressLbl);
            this.groupBox1.Controls.Add(this.SubnetMaskTB);
            this.groupBox1.Controls.Add(this.SubnetMaskLbl);
            this.groupBox1.Controls.Add(this.IPOptionsTB);
            this.groupBox1.Controls.Add(this.IPOptionsLbl);
            this.groupBox1.Controls.Add(this.MulticastIPAddressTB);
            this.groupBox1.Controls.Add(this.MulticastIPAddressLbl);
            this.groupBox1.Controls.Add(this.IPAddressTB);
            this.groupBox1.Controls.Add(this.IPAddressLbl);
            this.groupBox1.Controls.Add(this.DataLinkLayerTB);
            this.groupBox1.Controls.Add(this.DataLinkLayerLbl);
            this.groupBox1.Controls.Add(this.LogicalNameTB);
            this.groupBox1.Controls.Add(this.LogicalNameLbl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(348, 292);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TCP/UDP Setup Object";
            this.groupBox1.UseCompatibleTextRendering = true;
            // 
            // UseDHCCB
            // 
            this.UseDHCCB.Checked = true;
            this.UseDHCCB.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.UseDHCCB.Location = new System.Drawing.Point(129, 204);
            this.UseDHCCB.Name = "UseDHCCB";
            this.UseDHCCB.Size = new System.Drawing.Size(27, 17);
            this.UseDHCCB.TabIndex = 7;
            this.UseDHCCB.UseVisualStyleBackColor = false;
            // 
            // SecondaryDNSAddressTB
            // 
            this.SecondaryDNSAddressTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SecondaryDNSAddressTB.AttributeID = 10;
            this.SecondaryDNSAddressTB.Location = new System.Drawing.Point(136, 255);
            this.SecondaryDNSAddressTB.Name = "SecondaryDNSAddressTB";
            this.SecondaryDNSAddressTB.Size = new System.Drawing.Size(206, 20);
            this.SecondaryDNSAddressTB.TabIndex = 9;
            this.SecondaryDNSAddressTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // SecondaryDNSAddressLbl
            // 
            this.SecondaryDNSAddressLbl.AutoSize = true;
            this.SecondaryDNSAddressLbl.Location = new System.Drawing.Point(6, 258);
            this.SecondaryDNSAddressLbl.Name = "SecondaryDNSAddressLbl";
            this.SecondaryDNSAddressLbl.Size = new System.Drawing.Size(128, 13);
            this.SecondaryDNSAddressLbl.TabIndex = 18;
            this.SecondaryDNSAddressLbl.Text = "Secondary DNS Address:";
            // 
            // PrimaryDNSAddressTB
            // 
            this.PrimaryDNSAddressTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PrimaryDNSAddressTB.AttributeID = 9;
            this.PrimaryDNSAddressTB.Location = new System.Drawing.Point(136, 229);
            this.PrimaryDNSAddressTB.Name = "PrimaryDNSAddressTB";
            this.PrimaryDNSAddressTB.Size = new System.Drawing.Size(206, 20);
            this.PrimaryDNSAddressTB.TabIndex = 8;
            this.PrimaryDNSAddressTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // PrimaryDNSAddressLbl
            // 
            this.PrimaryDNSAddressLbl.AutoSize = true;
            this.PrimaryDNSAddressLbl.Location = new System.Drawing.Point(6, 232);
            this.PrimaryDNSAddressLbl.Name = "PrimaryDNSAddressLbl";
            this.PrimaryDNSAddressLbl.Size = new System.Drawing.Size(111, 13);
            this.PrimaryDNSAddressLbl.TabIndex = 16;
            this.PrimaryDNSAddressLbl.Text = "Primary DNS Address:";
            // 
            // UseDHCPLbl
            // 
            this.UseDHCPLbl.AutoSize = true;
            this.UseDHCPLbl.Location = new System.Drawing.Point(6, 206);
            this.UseDHCPLbl.Name = "UseDHCPLbl";
            this.UseDHCPLbl.Size = new System.Drawing.Size(62, 13);
            this.UseDHCPLbl.TabIndex = 14;
            this.UseDHCPLbl.Text = "Use DHCP:";
            // 
            // GatewayIPAddressTB
            // 
            this.GatewayIPAddressTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GatewayIPAddressTB.AttributeID = 7;
            this.GatewayIPAddressTB.Location = new System.Drawing.Point(136, 177);
            this.GatewayIPAddressTB.Name = "GatewayIPAddressTB";
            this.GatewayIPAddressTB.Size = new System.Drawing.Size(206, 20);
            this.GatewayIPAddressTB.TabIndex = 6;
            this.GatewayIPAddressTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // GatewayIPAddressLbl
            // 
            this.GatewayIPAddressLbl.AutoSize = true;
            this.GatewayIPAddressLbl.Location = new System.Drawing.Point(6, 180);
            this.GatewayIPAddressLbl.Name = "GatewayIPAddressLbl";
            this.GatewayIPAddressLbl.Size = new System.Drawing.Size(106, 13);
            this.GatewayIPAddressLbl.TabIndex = 12;
            this.GatewayIPAddressLbl.Text = "Gateway IP Address:";
            // 
            // SubnetMaskTB
            // 
            this.SubnetMaskTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SubnetMaskTB.AttributeID = 6;
            this.SubnetMaskTB.Location = new System.Drawing.Point(136, 151);
            this.SubnetMaskTB.Name = "SubnetMaskTB";
            this.SubnetMaskTB.Size = new System.Drawing.Size(206, 20);
            this.SubnetMaskTB.TabIndex = 5;
            this.SubnetMaskTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // SubnetMaskLbl
            // 
            this.SubnetMaskLbl.AutoSize = true;
            this.SubnetMaskLbl.Location = new System.Drawing.Point(6, 154);
            this.SubnetMaskLbl.Name = "SubnetMaskLbl";
            this.SubnetMaskLbl.Size = new System.Drawing.Size(73, 13);
            this.SubnetMaskLbl.TabIndex = 10;
            this.SubnetMaskLbl.Text = "Subnet Mask:";
            // 
            // IPOptionsTB
            // 
            this.IPOptionsTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.IPOptionsTB.AttributeID = 5;
            this.IPOptionsTB.Location = new System.Drawing.Point(136, 125);
            this.IPOptionsTB.Name = "IPOptionsTB";
            this.IPOptionsTB.Size = new System.Drawing.Size(206, 20);
            this.IPOptionsTB.TabIndex = 4;
            this.IPOptionsTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // IPOptionsLbl
            // 
            this.IPOptionsLbl.AutoSize = true;
            this.IPOptionsLbl.Location = new System.Drawing.Point(6, 128);
            this.IPOptionsLbl.Name = "IPOptionsLbl";
            this.IPOptionsLbl.Size = new System.Drawing.Size(59, 13);
            this.IPOptionsLbl.TabIndex = 8;
            this.IPOptionsLbl.Text = "IP Options:";
            // 
            // MulticastIPAddressTB
            // 
            this.MulticastIPAddressTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MulticastIPAddressTB.AttributeID = 4;
            this.MulticastIPAddressTB.Location = new System.Drawing.Point(136, 99);
            this.MulticastIPAddressTB.Name = "MulticastIPAddressTB";
            this.MulticastIPAddressTB.Size = new System.Drawing.Size(206, 20);
            this.MulticastIPAddressTB.TabIndex = 3;
            this.MulticastIPAddressTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // MulticastIPAddressLbl
            // 
            this.MulticastIPAddressLbl.AutoSize = true;
            this.MulticastIPAddressLbl.Location = new System.Drawing.Point(6, 102);
            this.MulticastIPAddressLbl.Name = "MulticastIPAddressLbl";
            this.MulticastIPAddressLbl.Size = new System.Drawing.Size(106, 13);
            this.MulticastIPAddressLbl.TabIndex = 6;
            this.MulticastIPAddressLbl.Text = "Multicast IP Address:";
            // 
            // IPAddressTB
            // 
            this.IPAddressTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.IPAddressTB.AttributeID = 3;
            this.IPAddressTB.Location = new System.Drawing.Point(136, 73);
            this.IPAddressTB.Name = "IPAddressTB";
            this.IPAddressTB.Size = new System.Drawing.Size(206, 20);
            this.IPAddressTB.TabIndex = 2;
            this.IPAddressTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // IPAddressLbl
            // 
            this.IPAddressLbl.AutoSize = true;
            this.IPAddressLbl.Location = new System.Drawing.Point(6, 76);
            this.IPAddressLbl.Name = "IPAddressLbl";
            this.IPAddressLbl.Size = new System.Drawing.Size(61, 13);
            this.IPAddressLbl.TabIndex = 4;
            this.IPAddressLbl.Text = "IP Address:";
            // 
            // DataLinkLayerTB
            // 
            this.DataLinkLayerTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DataLinkLayerTB.AttributeID = 2;
            this.DataLinkLayerTB.Location = new System.Drawing.Point(136, 47);
            this.DataLinkLayerTB.Name = "DataLinkLayerTB";
            this.DataLinkLayerTB.Size = new System.Drawing.Size(206, 20);
            this.DataLinkLayerTB.TabIndex = 1;
            this.DataLinkLayerTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // DataLinkLayerLbl
            // 
            this.DataLinkLayerLbl.AutoSize = true;
            this.DataLinkLayerLbl.Location = new System.Drawing.Point(6, 50);
            this.DataLinkLayerLbl.Name = "DataLinkLayerLbl";
            this.DataLinkLayerLbl.Size = new System.Drawing.Size(77, 13);
            this.DataLinkLayerLbl.TabIndex = 2;
            this.DataLinkLayerLbl.Text = "Data link layer:";
            // 
            // LogicalNameTB
            // 
            this.LogicalNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LogicalNameTB.AttributeID = 1;
            this.LogicalNameTB.Location = new System.Drawing.Point(136, 21);
            this.LogicalNameTB.Name = "LogicalNameTB";
            this.LogicalNameTB.Size = new System.Drawing.Size(206, 20);
            this.LogicalNameTB.TabIndex = 0;
            this.LogicalNameTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // LogicalNameLbl
            // 
            this.LogicalNameLbl.AutoSize = true;
            this.LogicalNameLbl.Location = new System.Drawing.Point(6, 24);
            this.LogicalNameLbl.Name = "LogicalNameLbl";
            this.LogicalNameLbl.Size = new System.Drawing.Size(75, 13);
            this.LogicalNameLbl.TabIndex = 0;
            this.LogicalNameLbl.Text = "Logical Name:";
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.Icon = ((System.Drawing.Icon)(resources.GetObject("errorProvider1.Icon")));
            // 
            // GXDLMSIp4SetupView
            // 
            this.ClientSize = new System.Drawing.Size(372, 335);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GXDLMSIp4SetupView";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

    }
}
