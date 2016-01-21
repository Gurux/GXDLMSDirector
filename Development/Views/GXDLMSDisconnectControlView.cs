//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/Views/GXDLMSDisconnectControlView.cs $
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
using Gurux.DLMS.Enums;

namespace GXDLMSDirector.Views
{
    [GXDLMSViewAttribute(typeof(GXDLMSDisconnectControl))]
    class GXDLMSDisconnectControlView : Form, IGXDLMSView
    {
        private GroupBox groupBox1;
        private GXValueField ControlStateCB;
        private Label ControlStateLbl;
        private GXValueField LogicalNameTB;
        private ErrorProvider errorProvider1;
        private System.ComponentModel.IContainer components;
        private Label DescriptionLbl;
        private TextBox DescriptionTB;
        private CheckBox OutputStateCB;
        private Label OutputStateLbl;
        private GXValueField ControlModeCB;
        private Label ControlModeLbl;
        private Label LogicalNameLbl;
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSDisconnectControlView()
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
            if (attributeID == 2)
            {
                GXDLMSDisconnectControl target = Target as GXDLMSDisconnectControl;
                this.OutputStateCB.Checked = target.OutputState;
            }
            else if (attributeID != 0)
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
                return DescriptionTB.Text;
            }
            set
            {
                DescriptionTB.Text = value;
            }
        }

        public void OnDirtyChange(int attributeID, bool Dirty)
        {
            if (Dirty && attributeID == 2)
            {
                errorProvider1.SetError(ControlStateCB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        public void OnAccessRightsChange(int attributeID, AccessMode access)
        {
        }

        #endregion


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDLMSDisconnectControlView));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.OutputStateCB = new System.Windows.Forms.CheckBox();
            this.OutputStateLbl = new System.Windows.Forms.Label();
            this.DescriptionLbl = new System.Windows.Forms.Label();
            this.DescriptionTB = new System.Windows.Forms.TextBox();
            this.ControlStateCB = new GXDLMSDirector.Views.GXValueField();
            this.ControlStateLbl = new System.Windows.Forms.Label();
            this.LogicalNameTB = new GXDLMSDirector.Views.GXValueField();
            this.LogicalNameLbl = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.ControlModeCB = new GXDLMSDirector.Views.GXValueField();
            this.ControlModeLbl = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ControlModeCB);
            this.groupBox1.Controls.Add(this.ControlModeLbl);
            this.groupBox1.Controls.Add(this.OutputStateCB);
            this.groupBox1.Controls.Add(this.OutputStateLbl);
            this.groupBox1.Controls.Add(this.DescriptionLbl);
            this.groupBox1.Controls.Add(this.DescriptionTB);
            this.groupBox1.Controls.Add(this.ControlStateCB);
            this.groupBox1.Controls.Add(this.ControlStateLbl);
            this.groupBox1.Controls.Add(this.LogicalNameTB);
            this.groupBox1.Controls.Add(this.LogicalNameLbl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 172);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Disconnect Control Object";
            // 
            // OutputStateCB
            // 
            this.OutputStateCB.Checked = true;
            this.OutputStateCB.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.OutputStateCB.Location = new System.Drawing.Point(103, 71);
            this.OutputStateCB.Name = "OutputStateCB";
            this.OutputStateCB.Size = new System.Drawing.Size(27, 17);
            this.OutputStateCB.TabIndex = 15;
            this.OutputStateCB.UseVisualStyleBackColor = false;
            // 
            // OutputStateLbl
            // 
            this.OutputStateLbl.AutoSize = true;
            this.OutputStateLbl.Location = new System.Drawing.Point(8, 72);
            this.OutputStateLbl.Name = "OutputStateLbl";
            this.OutputStateLbl.Size = new System.Drawing.Size(70, 13);
            this.OutputStateLbl.TabIndex = 16;
            this.OutputStateLbl.Text = "Output State:";
            // 
            // DescriptionLbl
            // 
            this.DescriptionLbl.AutoSize = true;
            this.DescriptionLbl.Location = new System.Drawing.Point(7, 22);
            this.DescriptionLbl.Name = "DescriptionLbl";
            this.DescriptionLbl.Size = new System.Drawing.Size(63, 13);
            this.DescriptionLbl.TabIndex = 5;
            this.DescriptionLbl.Text = "Description:";
            // 
            // DescriptionTB
            // 
            this.DescriptionTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DescriptionTB.Location = new System.Drawing.Point(103, 19);
            this.DescriptionTB.Name = "DescriptionTB";
            this.DescriptionTB.ReadOnly = true;
            this.DescriptionTB.Size = new System.Drawing.Size(172, 20);
            this.DescriptionTB.TabIndex = 4;
            // 
            // ControlStateCB
            // 
            this.ControlStateCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ControlStateCB.AttributeID = 3;
            this.ControlStateCB.Location = new System.Drawing.Point(103, 92);
            this.ControlStateCB.Name = "ControlStateCB";
            this.ControlStateCB.ReadOnly = true;
            this.ControlStateCB.Size = new System.Drawing.Size(171, 74);
            this.ControlStateCB.TabIndex = 0;
            this.ControlStateCB.Type = GXDLMSDirector.Views.GXValueFieldType.CompoBox;
            this.ControlStateCB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ValueTB_KeyPress);
            this.ControlStateCB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ValueTB_KeyUp);
            // 
            // ControlStateLbl
            // 
            this.ControlStateLbl.AutoSize = true;
            this.ControlStateLbl.Location = new System.Drawing.Point(6, 95);
            this.ControlStateLbl.Name = "ControlStateLbl";
            this.ControlStateLbl.Size = new System.Drawing.Size(71, 13);
            this.ControlStateLbl.TabIndex = 2;
            this.ControlStateLbl.Text = "Control State:";
            // 
            // LogicalNameTB
            // 
            this.LogicalNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogicalNameTB.AttributeID = 1;
            this.LogicalNameTB.Location = new System.Drawing.Point(103, 45);
            this.LogicalNameTB.Name = "LogicalNameTB";
            this.LogicalNameTB.Size = new System.Drawing.Size(171, 20);
            this.LogicalNameTB.TabIndex = 1;
            this.LogicalNameTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // LogicalNameLbl
            // 
            this.LogicalNameLbl.AutoSize = true;
            this.LogicalNameLbl.Location = new System.Drawing.Point(7, 48);
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
            // ControlModeCB
            // 
            this.ControlModeCB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ControlModeCB.AttributeID = 4;
            this.ControlModeCB.Location = new System.Drawing.Point(104, 118);
            this.ControlModeCB.Name = "ControlModeCB";
            this.ControlModeCB.ReadOnly = true;
            this.ControlModeCB.Size = new System.Drawing.Size(171, 74);
            this.ControlModeCB.TabIndex = 17;
            this.ControlModeCB.Type = GXDLMSDirector.Views.GXValueFieldType.CompoBox;
            // 
            // ControlModeLbl
            // 
            this.ControlModeLbl.AutoSize = true;
            this.ControlModeLbl.Location = new System.Drawing.Point(7, 121);
            this.ControlModeLbl.Name = "ControlModeLbl";
            this.ControlModeLbl.Size = new System.Drawing.Size(73, 13);
            this.ControlModeLbl.TabIndex = 18;
            this.ControlModeLbl.Text = "Control Mode:";
            // 
            // GXDLMSDisconnectControlView
            // 
            this.ClientSize = new System.Drawing.Size(320, 196);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GXDLMSDisconnectControlView";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        private void ValueTB_KeyUp(object sender, KeyEventArgs e)
        {
            errorProvider1.SetError((Control) sender, "Value changed.");
        }

        private void ValueTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            errorProvider1.SetError((Control)sender, "Value changed.");
        }
    }
}
