//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GuruxClub/GXDLMSDirector/Development/Views/GXDLMSTcpUdpSetupView.cs $
//
// Version:         $Revision: 5795 $,
//                  $Date: 2012-10-02 13:22:54 +0300 (ti, 02 loka 2012) $
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

namespace GXDLMSDirector.Views
{
    [GXDLMSViewAttribute(typeof(Gurux.DLMS.Objects.GXDLMSTcpUdpSetup))]
    class GXDLMSTcpUdpSetupView : Form, IGXDLMSView
    {
        private GroupBox groupBox1;
        private Label PortLbl;
        private GXValueField LogicalNameTB;
        private GXValueField InactivityTimeoutTB;
        private Label InactivityTimeoutLbl;
        private GXValueField MaxConnectionsTB;
        private Label MaxConnectionsLbl;
        private GXValueField MaximumSegmentSizeTB;
        private Label MaximumSegmentSizeLbl;
        private GXValueField IPReferenceTB;
        private Label IPReferenceLbl;
        private GXValueField PortTB;
        private ErrorProvider errorProvider1;
        private System.ComponentModel.IContainer components;
        private Label LogicalNameLbl;
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSTcpUdpSetupView()
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
        }

        public void OnAccessRightsChange(int attributeID, AccessMode access)
        {
            if (attributeID == 1)
            {
                this.LogicalNameTB.ReadOnly = (access & Gurux.DLMS.AccessMode.Write) == 0;
            }
            else if (attributeID == 2)
            {
                this.PortTB.ReadOnly = (access & Gurux.DLMS.AccessMode.Write) == 0;
            }
            else if (attributeID == 3)
            {
                this.IPReferenceTB.ReadOnly = (access & Gurux.DLMS.AccessMode.Write) == 0;
            }
            else if (attributeID == 4)
            {
                this.MaximumSegmentSizeTB.ReadOnly = (access & Gurux.DLMS.AccessMode.Write) == 0;
            }
            else if (attributeID == 5)
            {
                this.MaxConnectionsTB.ReadOnly = (access & Gurux.DLMS.AccessMode.Write) == 0;
            }
            else if (attributeID == 6)
            {
                this.InactivityTimeoutTB.ReadOnly = (access & Gurux.DLMS.AccessMode.Write) == 0;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDLMSTcpUdpSetupView));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.InactivityTimeoutTB = new GXDLMSDirector.Views.GXValueField();
            this.InactivityTimeoutLbl = new System.Windows.Forms.Label();
            this.MaxConnectionsTB = new GXDLMSDirector.Views.GXValueField();
            this.MaxConnectionsLbl = new System.Windows.Forms.Label();
            this.MaximumSegmentSizeTB = new GXDLMSDirector.Views.GXValueField();
            this.MaximumSegmentSizeLbl = new System.Windows.Forms.Label();
            this.IPReferenceTB = new GXDLMSDirector.Views.GXValueField();
            this.IPReferenceLbl = new System.Windows.Forms.Label();
            this.PortTB = new GXDLMSDirector.Views.GXValueField();
            this.PortLbl = new System.Windows.Forms.Label();
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
            this.groupBox1.Controls.Add(this.InactivityTimeoutTB);
            this.groupBox1.Controls.Add(this.InactivityTimeoutLbl);
            this.groupBox1.Controls.Add(this.MaxConnectionsTB);
            this.groupBox1.Controls.Add(this.MaxConnectionsLbl);
            this.groupBox1.Controls.Add(this.MaximumSegmentSizeTB);
            this.groupBox1.Controls.Add(this.MaximumSegmentSizeLbl);
            this.groupBox1.Controls.Add(this.IPReferenceTB);
            this.groupBox1.Controls.Add(this.IPReferenceLbl);
            this.groupBox1.Controls.Add(this.PortTB);
            this.groupBox1.Controls.Add(this.PortLbl);
            this.groupBox1.Controls.Add(this.LogicalNameTB);
            this.groupBox1.Controls.Add(this.LogicalNameLbl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 184);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TCP/UDP Setup Object";
            this.groupBox1.UseCompatibleTextRendering = true;
            // 
            // InactivityTimeoutTB
            // 
            this.InactivityTimeoutTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.InactivityTimeoutTB.AttributeID = 6;
            this.InactivityTimeoutTB.Location = new System.Drawing.Point(102, 151);
            this.InactivityTimeoutTB.Name = "InactivityTimeoutTB";
            this.InactivityTimeoutTB.Size = new System.Drawing.Size(171, 20);
            this.InactivityTimeoutTB.TabIndex = 11;
            this.InactivityTimeoutTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // InactivityTimeoutLbl
            // 
            this.InactivityTimeoutLbl.AutoSize = true;
            this.InactivityTimeoutLbl.Location = new System.Drawing.Point(6, 154);
            this.InactivityTimeoutLbl.Name = "InactivityTimeoutLbl";
            this.InactivityTimeoutLbl.Size = new System.Drawing.Size(93, 13);
            this.InactivityTimeoutLbl.TabIndex = 10;
            this.InactivityTimeoutLbl.Text = "Inactivity Timeout:";
            // 
            // MaxConnectionsTB
            // 
            this.MaxConnectionsTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxConnectionsTB.AttributeID = 5;
            this.MaxConnectionsTB.Location = new System.Drawing.Point(102, 125);
            this.MaxConnectionsTB.Name = "MaxConnectionsTB";
            this.MaxConnectionsTB.Size = new System.Drawing.Size(171, 20);
            this.MaxConnectionsTB.TabIndex = 9;
            this.MaxConnectionsTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // MaxConnectionsLbl
            // 
            this.MaxConnectionsLbl.AutoSize = true;
            this.MaxConnectionsLbl.Location = new System.Drawing.Point(6, 128);
            this.MaxConnectionsLbl.Name = "MaxConnectionsLbl";
            this.MaxConnectionsLbl.Size = new System.Drawing.Size(92, 13);
            this.MaxConnectionsLbl.TabIndex = 8;
            this.MaxConnectionsLbl.Text = "Max Connections:";
            // 
            // MaximumSegmentSizeTB
            // 
            this.MaximumSegmentSizeTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MaximumSegmentSizeTB.AttributeID = 4;
            this.MaximumSegmentSizeTB.Location = new System.Drawing.Point(102, 99);
            this.MaximumSegmentSizeTB.Name = "MaximumSegmentSizeTB";
            this.MaximumSegmentSizeTB.Size = new System.Drawing.Size(171, 20);
            this.MaximumSegmentSizeTB.TabIndex = 7;
            this.MaximumSegmentSizeTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // MaximumSegmentSizeLbl
            // 
            this.MaximumSegmentSizeLbl.AutoSize = true;
            this.MaximumSegmentSizeLbl.Location = new System.Drawing.Point(6, 102);
            this.MaximumSegmentSizeLbl.Name = "MaximumSegmentSizeLbl";
            this.MaximumSegmentSizeLbl.Size = new System.Drawing.Size(98, 13);
            this.MaximumSegmentSizeLbl.TabIndex = 6;
            this.MaximumSegmentSizeLbl.Text = "Max Segment Size:";
            // 
            // IPReferenceTB
            // 
            this.IPReferenceTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.IPReferenceTB.AttributeID = 3;
            this.IPReferenceTB.Location = new System.Drawing.Point(102, 73);
            this.IPReferenceTB.Name = "IPReferenceTB";
            this.IPReferenceTB.Size = new System.Drawing.Size(171, 20);
            this.IPReferenceTB.TabIndex = 5;
            this.IPReferenceTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // IPReferenceLbl
            // 
            this.IPReferenceLbl.AutoSize = true;
            this.IPReferenceLbl.Location = new System.Drawing.Point(6, 76);
            this.IPReferenceLbl.Name = "IPReferenceLbl";
            this.IPReferenceLbl.Size = new System.Drawing.Size(73, 13);
            this.IPReferenceLbl.TabIndex = 4;
            this.IPReferenceLbl.Text = "IP Reference:";
            // 
            // PortTB
            // 
            this.PortTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PortTB.AttributeID = 2;
            this.PortTB.Location = new System.Drawing.Point(102, 47);
            this.PortTB.Name = "PortTB";
            this.PortTB.Size = new System.Drawing.Size(171, 20);
            this.PortTB.TabIndex = 3;
            this.PortTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // PortLbl
            // 
            this.PortLbl.AutoSize = true;
            this.PortLbl.Location = new System.Drawing.Point(6, 50);
            this.PortLbl.Name = "PortLbl";
            this.PortLbl.Size = new System.Drawing.Size(29, 13);
            this.PortLbl.TabIndex = 2;
            this.PortLbl.Text = "Port:";
            // 
            // LogicalNameTB
            // 
            this.LogicalNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LogicalNameTB.AttributeID = 1;
            this.LogicalNameTB.Location = new System.Drawing.Point(102, 21);
            this.LogicalNameTB.Name = "LogicalNameTB";
            this.LogicalNameTB.Size = new System.Drawing.Size(171, 20);
            this.LogicalNameTB.TabIndex = 1;
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
            // GXDLMSTcpUdpSetupView
            // 
            this.ClientSize = new System.Drawing.Size(320, 199);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GXDLMSTcpUdpSetupView";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

    }
}
