//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GuruxClub/GXDLMSDirector/Development/Views/GXDLMSIECOpticalPortSetupView.cs $
//
// Version:         $Revision: 6489 $,
//                  $Date: 2013-06-27 15:56:54 +0300 (to, 27 kesä 2013) $
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

namespace GXDLMSDirector.Views
{
    [GXDLMSViewAttribute(typeof(GXDLMSIECOpticalPortSetup))]
    class GXDLMSIECOpticalPortSetupView : Form, IGXDLMSView
    {
        private GroupBox groupBox1;
        private GXValueField DefaultModeTB;
        private Label DefaultModeLbl;
        private GXValueField LogicalNameTB;
        private GXValueField Password5TB;
        private Label Password5Lbl;
        private GXValueField Password2TB;
        private Label Password2Lbl;
        private GXValueField Password1TB;
        private Label Password1Lbl;
        private GXValueField DeviceAddressTB;
        private Label DeviceAddressLbl;
        private GXValueField ResponseTimeTB;
        private Label ResponseTimeLbl;
        private GXValueField MaximumBaudrateTB;
        private Label MaximumBaudrateLbl;
        private GXValueField DefaultBaudrateTB;
        private Label DefaultBaudrateLbl;
        private ErrorProvider errorProvider1;
        private System.ComponentModel.IContainer components;
        private Label LogicalNameLbl;
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSIECOpticalPortSetupView()
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
                    errorProvider1.SetError(DefaultModeTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 3:
                    errorProvider1.SetError(DefaultBaudrateTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 4:
                    errorProvider1.SetError(MaximumBaudrateTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 5:
                    errorProvider1.SetError(ResponseTimeTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 6:
                    errorProvider1.SetError(DeviceAddressTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 7:
                    errorProvider1.SetError(Password1TB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 8:
                    errorProvider1.SetError(Password2TB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 9:
                    errorProvider1.SetError(Password5TB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDLMSIECOpticalPortSetupView));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Password5TB = new GXDLMSDirector.Views.GXValueField();
            this.Password5Lbl = new System.Windows.Forms.Label();
            this.Password2TB = new GXDLMSDirector.Views.GXValueField();
            this.Password2Lbl = new System.Windows.Forms.Label();
            this.Password1TB = new GXDLMSDirector.Views.GXValueField();
            this.Password1Lbl = new System.Windows.Forms.Label();
            this.DeviceAddressTB = new GXDLMSDirector.Views.GXValueField();
            this.DeviceAddressLbl = new System.Windows.Forms.Label();
            this.ResponseTimeTB = new GXDLMSDirector.Views.GXValueField();
            this.ResponseTimeLbl = new System.Windows.Forms.Label();
            this.MaximumBaudrateTB = new GXDLMSDirector.Views.GXValueField();
            this.MaximumBaudrateLbl = new System.Windows.Forms.Label();
            this.DefaultBaudrateTB = new GXDLMSDirector.Views.GXValueField();
            this.DefaultBaudrateLbl = new System.Windows.Forms.Label();
            this.DefaultModeTB = new GXDLMSDirector.Views.GXValueField();
            this.DefaultModeLbl = new System.Windows.Forms.Label();
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
            this.groupBox1.Controls.Add(this.Password5TB);
            this.groupBox1.Controls.Add(this.Password5Lbl);
            this.groupBox1.Controls.Add(this.Password2TB);
            this.groupBox1.Controls.Add(this.Password2Lbl);
            this.groupBox1.Controls.Add(this.Password1TB);
            this.groupBox1.Controls.Add(this.Password1Lbl);
            this.groupBox1.Controls.Add(this.DeviceAddressTB);
            this.groupBox1.Controls.Add(this.DeviceAddressLbl);
            this.groupBox1.Controls.Add(this.ResponseTimeTB);
            this.groupBox1.Controls.Add(this.ResponseTimeLbl);
            this.groupBox1.Controls.Add(this.MaximumBaudrateTB);
            this.groupBox1.Controls.Add(this.MaximumBaudrateLbl);
            this.groupBox1.Controls.Add(this.DefaultBaudrateTB);
            this.groupBox1.Controls.Add(this.DefaultBaudrateLbl);
            this.groupBox1.Controls.Add(this.DefaultModeTB);
            this.groupBox1.Controls.Add(this.DefaultModeLbl);
            this.groupBox1.Controls.Add(this.LogicalNameTB);
            this.groupBox1.Controls.Add(this.LogicalNameLbl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(333, 266);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "IEC Optical Port Setup";
            // 
            // Password5TB
            // 
            this.Password5TB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Password5TB.AttributeID = 9;
            this.Password5TB.Location = new System.Drawing.Point(102, 229);
            this.Password5TB.Name = "Password5TB";
            this.Password5TB.Size = new System.Drawing.Size(208, 20);
            this.Password5TB.TabIndex = 8;
            this.Password5TB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // Password5Lbl
            // 
            this.Password5Lbl.AutoSize = true;
            this.Password5Lbl.Location = new System.Drawing.Point(6, 232);
            this.Password5Lbl.Name = "Password5Lbl";
            this.Password5Lbl.Size = new System.Drawing.Size(65, 13);
            this.Password5Lbl.TabIndex = 16;
            this.Password5Lbl.Text = "Password 5:";
            // 
            // Password2TB
            // 
            this.Password2TB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Password2TB.AttributeID = 8;
            this.Password2TB.Location = new System.Drawing.Point(102, 203);
            this.Password2TB.Name = "Password2TB";
            this.Password2TB.Size = new System.Drawing.Size(208, 20);
            this.Password2TB.TabIndex = 7;
            this.Password2TB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // Password2Lbl
            // 
            this.Password2Lbl.AutoSize = true;
            this.Password2Lbl.Location = new System.Drawing.Point(6, 206);
            this.Password2Lbl.Name = "Password2Lbl";
            this.Password2Lbl.Size = new System.Drawing.Size(65, 13);
            this.Password2Lbl.TabIndex = 14;
            this.Password2Lbl.Text = "Password 2:";
            // 
            // Password1TB
            // 
            this.Password1TB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Password1TB.AttributeID = 7;
            this.Password1TB.Location = new System.Drawing.Point(102, 177);
            this.Password1TB.Name = "Password1TB";
            this.Password1TB.Size = new System.Drawing.Size(208, 20);
            this.Password1TB.TabIndex = 6;
            this.Password1TB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // Password1Lbl
            // 
            this.Password1Lbl.AutoSize = true;
            this.Password1Lbl.Location = new System.Drawing.Point(6, 180);
            this.Password1Lbl.Name = "Password1Lbl";
            this.Password1Lbl.Size = new System.Drawing.Size(65, 13);
            this.Password1Lbl.TabIndex = 12;
            this.Password1Lbl.Text = "Password 1:";
            // 
            // DeviceAddressTB
            // 
            this.DeviceAddressTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DeviceAddressTB.AttributeID = 6;
            this.DeviceAddressTB.Location = new System.Drawing.Point(102, 151);
            this.DeviceAddressTB.Name = "DeviceAddressTB";
            this.DeviceAddressTB.Size = new System.Drawing.Size(208, 20);
            this.DeviceAddressTB.TabIndex = 5;
            this.DeviceAddressTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // DeviceAddressLbl
            // 
            this.DeviceAddressLbl.AutoSize = true;
            this.DeviceAddressLbl.Location = new System.Drawing.Point(6, 154);
            this.DeviceAddressLbl.Name = "DeviceAddressLbl";
            this.DeviceAddressLbl.Size = new System.Drawing.Size(85, 13);
            this.DeviceAddressLbl.TabIndex = 10;
            this.DeviceAddressLbl.Text = "Device Address:";
            // 
            // ResponseTimeTB
            // 
            this.ResponseTimeTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ResponseTimeTB.AttributeID = 5;
            this.ResponseTimeTB.Location = new System.Drawing.Point(102, 125);
            this.ResponseTimeTB.Name = "ResponseTimeTB";
            this.ResponseTimeTB.ReadOnly = true;
            this.ResponseTimeTB.Size = new System.Drawing.Size(208, 20);
            this.ResponseTimeTB.TabIndex = 4;
            this.ResponseTimeTB.Type = GXDLMSDirector.Views.GXValueFieldType.CompoBox;
            // 
            // ResponseTimeLbl
            // 
            this.ResponseTimeLbl.AutoSize = true;
            this.ResponseTimeLbl.Location = new System.Drawing.Point(6, 128);
            this.ResponseTimeLbl.Name = "ResponseTimeLbl";
            this.ResponseTimeLbl.Size = new System.Drawing.Size(84, 13);
            this.ResponseTimeLbl.TabIndex = 8;
            this.ResponseTimeLbl.Text = "Response Time:";
            // 
            // MaximumBaudrateTB
            // 
            this.MaximumBaudrateTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MaximumBaudrateTB.AttributeID = 4;
            this.MaximumBaudrateTB.Location = new System.Drawing.Point(102, 99);
            this.MaximumBaudrateTB.Name = "MaximumBaudrateTB";
            this.MaximumBaudrateTB.ReadOnly = true;
            this.MaximumBaudrateTB.Size = new System.Drawing.Size(208, 20);
            this.MaximumBaudrateTB.TabIndex = 3;
            this.MaximumBaudrateTB.Type = GXDLMSDirector.Views.GXValueFieldType.CompoBox;
            // 
            // MaximumBaudrateLbl
            // 
            this.MaximumBaudrateLbl.AutoSize = true;
            this.MaximumBaudrateLbl.Location = new System.Drawing.Point(6, 102);
            this.MaximumBaudrateLbl.Name = "MaximumBaudrateLbl";
            this.MaximumBaudrateLbl.Size = new System.Drawing.Size(100, 13);
            this.MaximumBaudrateLbl.TabIndex = 6;
            this.MaximumBaudrateLbl.Text = "Maximum Baudrate:";
            // 
            // DefaultBaudrateTB
            // 
            this.DefaultBaudrateTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DefaultBaudrateTB.AttributeID = 3;
            this.DefaultBaudrateTB.Location = new System.Drawing.Point(102, 73);
            this.DefaultBaudrateTB.Name = "DefaultBaudrateTB";
            this.DefaultBaudrateTB.ReadOnly = true;
            this.DefaultBaudrateTB.Size = new System.Drawing.Size(208, 20);
            this.DefaultBaudrateTB.TabIndex = 2;
            this.DefaultBaudrateTB.Type = GXDLMSDirector.Views.GXValueFieldType.CompoBox;
            // 
            // DefaultBaudrateLbl
            // 
            this.DefaultBaudrateLbl.AutoSize = true;
            this.DefaultBaudrateLbl.Location = new System.Drawing.Point(6, 76);
            this.DefaultBaudrateLbl.Name = "DefaultBaudrateLbl";
            this.DefaultBaudrateLbl.Size = new System.Drawing.Size(90, 13);
            this.DefaultBaudrateLbl.TabIndex = 4;
            this.DefaultBaudrateLbl.Text = "Default Baudrate:";
            // 
            // DefaultModeTB
            // 
            this.DefaultModeTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DefaultModeTB.AttributeID = 2;
            this.DefaultModeTB.Location = new System.Drawing.Point(102, 47);
            this.DefaultModeTB.Name = "DefaultModeTB";
            this.DefaultModeTB.Size = new System.Drawing.Size(208, 20);
            this.DefaultModeTB.TabIndex = 1;
            this.DefaultModeTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // DefaultModeLbl
            // 
            this.DefaultModeLbl.AutoSize = true;
            this.DefaultModeLbl.Location = new System.Drawing.Point(6, 50);
            this.DefaultModeLbl.Name = "DefaultModeLbl";
            this.DefaultModeLbl.Size = new System.Drawing.Size(74, 13);
            this.DefaultModeLbl.TabIndex = 2;
            this.DefaultModeLbl.Text = "Default Mode:";
            // 
            // LogicalNameTB
            // 
            this.LogicalNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LogicalNameTB.AttributeID = 1;
            this.LogicalNameTB.Location = new System.Drawing.Point(102, 21);
            this.LogicalNameTB.Name = "LogicalNameTB";
            this.LogicalNameTB.Size = new System.Drawing.Size(208, 20);
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
            // GXDLMSIECOpticalPortSetupView
            // 
            this.ClientSize = new System.Drawing.Size(357, 290);
            this.Controls.Add(this.groupBox1);
            this.Name = "GXDLMSIECOpticalPortSetupView";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
