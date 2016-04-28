//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/Views/GXDLMSHdlcSetupView.cs $
//
// Version:         $Revision: 8429 $,
//                  $Date: 2016-04-28 13:44:52 +0300 (to, 28 huhti 2016) $
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
using Gurux.DLMS;
using Gurux.DLMS.Objects;
using Gurux.DLMS.Enums;

namespace GXDLMSDirector.Views
{
    [GXDLMSViewAttribute(typeof(GXDLMSHdlcSetup))]
    class GXDLMSHdlcSetupView : Form, IGXDLMSView
    {
        private GroupBox groupBox1;
        private GXValueField SpeedTB;
        private Label SpeedLbl;
        private GXValueField LogicalNameTB;
        private GXValueField DeviceAddressTB;
        private Label DeviceAddressLbl;
        private GXValueField InactivityTimeoutTB;
        private Label InactivityTimeoutLbl;
        private GXValueField InternalTimeOutLTB;
        private Label InternalTimeOutLbl;
        private GroupBox groupBox3;
        private GXValueField ReceiveMaxLengthTB;
        private Label ReceiveMaxLengthLbl;
        private GXValueField ReceiveWindowSizeTB;
        private Label ReceiveWindowSizeLbl;
        private GroupBox groupBox2;
        private GXValueField TransmitMaxLengthTB;
        private Label TransmitMaxLengthLbl;
        private GXValueField TransmitWindowSizeTB;
        private Label TransmitWindowSizeLbl;
        private ErrorProvider errorProvider1;
        private System.ComponentModel.IContainer components;
        private Label LogicalNameLbl;
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSHdlcSetupView()
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
            throw new IndexOutOfRangeException("attributeID");         
        }

        public void OnAccessRightsChange(int attributeID, AccessMode access)
        {
            throw new NotImplementedException();
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
            switch (attributeID)
            {
                case 2:
                    errorProvider1.SetError(SpeedTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 3:
                    errorProvider1.SetError(TransmitWindowSizeTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 4:
                    errorProvider1.SetError(TransmitMaxLengthTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 5:
                    errorProvider1.SetError(ReceiveWindowSizeTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 6:
                    errorProvider1.SetError(ReceiveMaxLengthTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 7:
                    errorProvider1.SetError(InternalTimeOutLTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 8:
                    errorProvider1.SetError(InactivityTimeoutTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 9:
                    errorProvider1.SetError(DeviceAddressTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                default:
                    errorProvider1.Clear();
                    break;
            }
        }

        #endregion


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDLMSHdlcSetupView));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ReceiveMaxLengthTB = new GXDLMSDirector.Views.GXValueField();
            this.ReceiveMaxLengthLbl = new System.Windows.Forms.Label();
            this.ReceiveWindowSizeTB = new GXDLMSDirector.Views.GXValueField();
            this.ReceiveWindowSizeLbl = new System.Windows.Forms.Label();
            this.DeviceAddressTB = new GXDLMSDirector.Views.GXValueField();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TransmitMaxLengthTB = new GXDLMSDirector.Views.GXValueField();
            this.TransmitMaxLengthLbl = new System.Windows.Forms.Label();
            this.TransmitWindowSizeTB = new GXDLMSDirector.Views.GXValueField();
            this.TransmitWindowSizeLbl = new System.Windows.Forms.Label();
            this.DeviceAddressLbl = new System.Windows.Forms.Label();
            this.InactivityTimeoutTB = new GXDLMSDirector.Views.GXValueField();
            this.InactivityTimeoutLbl = new System.Windows.Forms.Label();
            this.InternalTimeOutLTB = new GXDLMSDirector.Views.GXValueField();
            this.InternalTimeOutLbl = new System.Windows.Forms.Label();
            this.SpeedTB = new GXDLMSDirector.Views.GXValueField();
            this.SpeedLbl = new System.Windows.Forms.Label();
            this.LogicalNameTB = new GXDLMSDirector.Views.GXValueField();
            this.LogicalNameLbl = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.DeviceAddressTB);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.DeviceAddressLbl);
            this.groupBox1.Controls.Add(this.InactivityTimeoutTB);
            this.groupBox1.Controls.Add(this.InactivityTimeoutLbl);
            this.groupBox1.Controls.Add(this.InternalTimeOutLTB);
            this.groupBox1.Controls.Add(this.InternalTimeOutLbl);
            this.groupBox1.Controls.Add(this.SpeedTB);
            this.groupBox1.Controls.Add(this.SpeedLbl);
            this.groupBox1.Controls.Add(this.LogicalNameTB);
            this.groupBox1.Controls.Add(this.LogicalNameLbl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(333, 313);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "HDLC Setup Object";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.ReceiveMaxLengthTB);
            this.groupBox3.Controls.Add(this.ReceiveMaxLengthLbl);
            this.groupBox3.Controls.Add(this.ReceiveWindowSizeTB);
            this.groupBox3.Controls.Add(this.ReceiveWindowSizeLbl);
            this.groupBox3.Location = new System.Drawing.Point(0, 146);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(333, 74);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Receive:";
            // 
            // ReceiveMaxLengthTB
            // 
            this.ReceiveMaxLengthTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ReceiveMaxLengthTB.AttributeID = 6;
            this.ReceiveMaxLengthTB.Location = new System.Drawing.Point(102, 45);
            this.ReceiveMaxLengthTB.Name = "ReceiveMaxLengthTB";
            this.ReceiveMaxLengthTB.Size = new System.Drawing.Size(208, 20);
            this.ReceiveMaxLengthTB.TabIndex = 13;
            this.ReceiveMaxLengthTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // ReceiveMaxLengthLbl
            // 
            this.ReceiveMaxLengthLbl.AutoSize = true;
            this.ReceiveMaxLengthLbl.Location = new System.Drawing.Point(6, 48);
            this.ReceiveMaxLengthLbl.Name = "ReceiveMaxLengthLbl";
            this.ReceiveMaxLengthLbl.Size = new System.Drawing.Size(90, 13);
            this.ReceiveMaxLengthLbl.TabIndex = 14;
            this.ReceiveMaxLengthLbl.Text = "Maximum Length:";
            // 
            // ReceiveWindowSizeTB
            // 
            this.ReceiveWindowSizeTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ReceiveWindowSizeTB.AttributeID = 4;
            this.ReceiveWindowSizeTB.Location = new System.Drawing.Point(102, 19);
            this.ReceiveWindowSizeTB.Name = "ReceiveWindowSizeTB";
            this.ReceiveWindowSizeTB.Size = new System.Drawing.Size(208, 20);
            this.ReceiveWindowSizeTB.TabIndex = 11;
            this.ReceiveWindowSizeTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // ReceiveWindowSizeLbl
            // 
            this.ReceiveWindowSizeLbl.AutoSize = true;
            this.ReceiveWindowSizeLbl.Location = new System.Drawing.Point(6, 22);
            this.ReceiveWindowSizeLbl.Name = "ReceiveWindowSizeLbl";
            this.ReceiveWindowSizeLbl.Size = new System.Drawing.Size(72, 13);
            this.ReceiveWindowSizeLbl.TabIndex = 12;
            this.ReceiveWindowSizeLbl.Text = "Window Size:";
            // 
            // DeviceAddressTB
            // 
            this.DeviceAddressTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DeviceAddressTB.AttributeID = 9;
            this.DeviceAddressTB.Location = new System.Drawing.Point(102, 278);
            this.DeviceAddressTB.Name = "DeviceAddressTB";
            this.DeviceAddressTB.Size = new System.Drawing.Size(208, 20);
            this.DeviceAddressTB.TabIndex = 15;
            this.DeviceAddressTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.TransmitMaxLengthTB);
            this.groupBox2.Controls.Add(this.TransmitMaxLengthLbl);
            this.groupBox2.Controls.Add(this.TransmitWindowSizeTB);
            this.groupBox2.Controls.Add(this.TransmitWindowSizeLbl);
            this.groupBox2.Location = new System.Drawing.Point(0, 73);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(333, 80);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Transmit";
            // 
            // TransmitMaxLengthTB
            // 
            this.TransmitMaxLengthTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TransmitMaxLengthTB.AttributeID = 5;
            this.TransmitMaxLengthTB.Location = new System.Drawing.Point(102, 48);
            this.TransmitMaxLengthTB.Name = "TransmitMaxLengthTB";
            this.TransmitMaxLengthTB.Size = new System.Drawing.Size(208, 20);
            this.TransmitMaxLengthTB.TabIndex = 11;
            this.TransmitMaxLengthTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // TransmitMaxLengthLbl
            // 
            this.TransmitMaxLengthLbl.AutoSize = true;
            this.TransmitMaxLengthLbl.Location = new System.Drawing.Point(6, 51);
            this.TransmitMaxLengthLbl.Name = "TransmitMaxLengthLbl";
            this.TransmitMaxLengthLbl.Size = new System.Drawing.Size(90, 13);
            this.TransmitMaxLengthLbl.TabIndex = 12;
            this.TransmitMaxLengthLbl.Text = "Maximum Length:";
            // 
            // TransmitWindowSizeTB
            // 
            this.TransmitWindowSizeTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TransmitWindowSizeTB.AttributeID = 3;
            this.TransmitWindowSizeTB.Location = new System.Drawing.Point(102, 22);
            this.TransmitWindowSizeTB.Name = "TransmitWindowSizeTB";
            this.TransmitWindowSizeTB.Size = new System.Drawing.Size(208, 20);
            this.TransmitWindowSizeTB.TabIndex = 9;
            this.TransmitWindowSizeTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // TransmitWindowSizeLbl
            // 
            this.TransmitWindowSizeLbl.AutoSize = true;
            this.TransmitWindowSizeLbl.Location = new System.Drawing.Point(6, 25);
            this.TransmitWindowSizeLbl.Name = "TransmitWindowSizeLbl";
            this.TransmitWindowSizeLbl.Size = new System.Drawing.Size(72, 13);
            this.TransmitWindowSizeLbl.TabIndex = 10;
            this.TransmitWindowSizeLbl.Text = "Window Size:";
            // 
            // DeviceAddressLbl
            // 
            this.DeviceAddressLbl.AutoSize = true;
            this.DeviceAddressLbl.Location = new System.Drawing.Point(6, 281);
            this.DeviceAddressLbl.Name = "DeviceAddressLbl";
            this.DeviceAddressLbl.Size = new System.Drawing.Size(85, 13);
            this.DeviceAddressLbl.TabIndex = 16;
            this.DeviceAddressLbl.Text = "Device Address:";
            // 
            // InactivityTimeoutTB
            // 
            this.InactivityTimeoutTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.InactivityTimeoutTB.AttributeID = 8;
            this.InactivityTimeoutTB.Location = new System.Drawing.Point(102, 252);
            this.InactivityTimeoutTB.Name = "InactivityTimeoutTB";
            this.InactivityTimeoutTB.Size = new System.Drawing.Size(208, 20);
            this.InactivityTimeoutTB.TabIndex = 13;
            this.InactivityTimeoutTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // InactivityTimeoutLbl
            // 
            this.InactivityTimeoutLbl.AutoSize = true;
            this.InactivityTimeoutLbl.Location = new System.Drawing.Point(6, 255);
            this.InactivityTimeoutLbl.Name = "InactivityTimeoutLbl";
            this.InactivityTimeoutLbl.Size = new System.Drawing.Size(93, 13);
            this.InactivityTimeoutLbl.TabIndex = 14;
            this.InactivityTimeoutLbl.Text = "Inactivity Timeout:";
            // 
            // InternalTimeOutLTB
            // 
            this.InternalTimeOutLTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.InternalTimeOutLTB.AttributeID = 7;
            this.InternalTimeOutLTB.Location = new System.Drawing.Point(102, 226);
            this.InternalTimeOutLTB.Name = "InternalTimeOutLTB";
            this.InternalTimeOutLTB.Size = new System.Drawing.Size(208, 20);
            this.InternalTimeOutLTB.TabIndex = 11;
            this.InternalTimeOutLTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // InternalTimeOutLbl
            // 
            this.InternalTimeOutLbl.AutoSize = true;
            this.InternalTimeOutLbl.Location = new System.Drawing.Point(6, 229);
            this.InternalTimeOutLbl.Name = "InternalTimeOutLbl";
            this.InternalTimeOutLbl.Size = new System.Drawing.Size(83, 13);
            this.InternalTimeOutLbl.TabIndex = 12;
            this.InternalTimeOutLbl.Text = "Internal Timeout";
            // 
            // SpeedTB
            // 
            this.SpeedTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SpeedTB.AttributeID = 2;
            this.SpeedTB.Location = new System.Drawing.Point(102, 47);
            this.SpeedTB.Name = "SpeedTB";
            this.SpeedTB.Size = new System.Drawing.Size(208, 20);
            this.SpeedTB.TabIndex = 0;
            this.SpeedTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // SpeedLbl
            // 
            this.SpeedLbl.AutoSize = true;
            this.SpeedLbl.Location = new System.Drawing.Point(6, 50);
            this.SpeedLbl.Name = "SpeedLbl";
            this.SpeedLbl.Size = new System.Drawing.Size(41, 13);
            this.SpeedLbl.TabIndex = 2;
            this.SpeedLbl.Text = "Speed:";
            // 
            // LogicalNameTB
            // 
            this.LogicalNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LogicalNameTB.AttributeID = 1;
            this.LogicalNameTB.Location = new System.Drawing.Point(102, 21);
            this.LogicalNameTB.Name = "LogicalNameTB";
            this.LogicalNameTB.Size = new System.Drawing.Size(208, 20);
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
            // GXDLMSHdlcSetupView
            // 
            this.ClientSize = new System.Drawing.Size(357, 335);
            this.Controls.Add(this.groupBox1);
            this.Name = "GXDLMSHdlcSetupView";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

    }
}
