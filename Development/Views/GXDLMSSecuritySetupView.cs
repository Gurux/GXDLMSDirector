//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/Views/GXDLMSSecuritySetupView.cs $
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
    [GXDLMSViewAttribute(typeof(GXDLMSSecuritySetup))]
    class GXDLMSSecuritySetupView : Form, IGXDLMSView
    {
        private GroupBox groupBox1;
        private GXValueField SecurityPolicyTB;
        private Label SecurityPolicyLbl;
        private GXValueField LogicalNameTB;
        private ErrorProvider errorProvider1;
        private System.ComponentModel.IContainer components;
        private Label DescriptionLbl;
        private TextBox DescriptionTB;
        private GXValueField SuiteTB;
        private Label SuiteLbl;
        private GXValueField ServerSystemTitleTB;
        private Label ServerSystemTitleLbl;
        private GXValueField ClientSystemTitleTB;
        private Label ClientSystemTitleLbl;
        private Label LogicalNameLbl;
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSSecuritySetupView()
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
                errorProvider1.SetError(SecurityPolicyTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        public void OnAccessRightsChange(int attributeID, AccessMode access)
        {
            throw new IndexOutOfRangeException("attributeID");
        }

        #endregion


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDLMSSecuritySetupView));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ServerSystemTitleLbl = new System.Windows.Forms.Label();
            this.ClientSystemTitleLbl = new System.Windows.Forms.Label();
            this.SuiteLbl = new System.Windows.Forms.Label();
            this.DescriptionLbl = new System.Windows.Forms.Label();
            this.DescriptionTB = new System.Windows.Forms.TextBox();
            this.SecurityPolicyLbl = new System.Windows.Forms.Label();
            this.LogicalNameLbl = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.ServerSystemTitleTB = new GXDLMSDirector.Views.GXValueField();
            this.ClientSystemTitleTB = new GXDLMSDirector.Views.GXValueField();
            this.SuiteTB = new GXDLMSDirector.Views.GXValueField();
            this.SecurityPolicyTB = new GXDLMSDirector.Views.GXValueField();
            this.LogicalNameTB = new GXDLMSDirector.Views.GXValueField();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ServerSystemTitleTB);
            this.groupBox1.Controls.Add(this.ServerSystemTitleLbl);
            this.groupBox1.Controls.Add(this.ClientSystemTitleTB);
            this.groupBox1.Controls.Add(this.ClientSystemTitleLbl);
            this.groupBox1.Controls.Add(this.SuiteTB);
            this.groupBox1.Controls.Add(this.SuiteLbl);
            this.groupBox1.Controls.Add(this.DescriptionLbl);
            this.groupBox1.Controls.Add(this.DescriptionTB);
            this.groupBox1.Controls.Add(this.SecurityPolicyTB);
            this.groupBox1.Controls.Add(this.SecurityPolicyLbl);
            this.groupBox1.Controls.Add(this.LogicalNameTB);
            this.groupBox1.Controls.Add(this.LogicalNameLbl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 181);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Security Setup Object";
            // 
            // ServerSystemTitleLbl
            // 
            this.ServerSystemTitleLbl.AutoSize = true;
            this.ServerSystemTitleLbl.Location = new System.Drawing.Point(7, 152);
            this.ServerSystemTitleLbl.Name = "ServerSystemTitleLbl";
            this.ServerSystemTitleLbl.Size = new System.Drawing.Size(101, 13);
            this.ServerSystemTitleLbl.TabIndex = 11;
            this.ServerSystemTitleLbl.Text = "Server System Title:";
            // 
            // ClientSystemTitleLbl
            // 
            this.ClientSystemTitleLbl.AutoSize = true;
            this.ClientSystemTitleLbl.Location = new System.Drawing.Point(7, 126);
            this.ClientSystemTitleLbl.Name = "ClientSystemTitleLbl";
            this.ClientSystemTitleLbl.Size = new System.Drawing.Size(96, 13);
            this.ClientSystemTitleLbl.TabIndex = 9;
            this.ClientSystemTitleLbl.Text = "Client System Title:";
            // 
            // SuiteLbl
            // 
            this.SuiteLbl.AutoSize = true;
            this.SuiteLbl.Location = new System.Drawing.Point(7, 100);
            this.SuiteLbl.Name = "SuiteLbl";
            this.SuiteLbl.Size = new System.Drawing.Size(34, 13);
            this.SuiteLbl.TabIndex = 7;
            this.SuiteLbl.Text = "Suite:";
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
            // SecurityPolicyLbl
            // 
            this.SecurityPolicyLbl.AutoSize = true;
            this.SecurityPolicyLbl.Location = new System.Drawing.Point(7, 74);
            this.SecurityPolicyLbl.Name = "SecurityPolicyLbl";
            this.SecurityPolicyLbl.Size = new System.Drawing.Size(38, 13);
            this.SecurityPolicyLbl.TabIndex = 2;
            this.SecurityPolicyLbl.Text = "Policy:";
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
            // ServerSystemTitleTB
            // 
            this.ServerSystemTitleTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ServerSystemTitleTB.AttributeID = 5;
            this.ServerSystemTitleTB.Location = new System.Drawing.Point(104, 149);
            this.ServerSystemTitleTB.Name = "ServerSystemTitleTB";
            this.ServerSystemTitleTB.Size = new System.Drawing.Size(171, 20);
            this.ServerSystemTitleTB.TabIndex = 3;
            this.ServerSystemTitleTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // ClientSystemTitleTB
            // 
            this.ClientSystemTitleTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ClientSystemTitleTB.AttributeID = 4;
            this.ClientSystemTitleTB.Location = new System.Drawing.Point(104, 123);
            this.ClientSystemTitleTB.Name = "ClientSystemTitleTB";
            this.ClientSystemTitleTB.Size = new System.Drawing.Size(171, 20);
            this.ClientSystemTitleTB.TabIndex = 2;
            this.ClientSystemTitleTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // SuiteTB
            // 
            this.SuiteTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SuiteTB.AttributeID = 3;
            this.SuiteTB.Location = new System.Drawing.Point(104, 97);
            this.SuiteTB.Name = "SuiteTB";
            this.SuiteTB.Size = new System.Drawing.Size(171, 20);
            this.SuiteTB.TabIndex = 1;
            this.SuiteTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // SecurityPolicyTB
            // 
            this.SecurityPolicyTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SecurityPolicyTB.AttributeID = 2;
            this.SecurityPolicyTB.Location = new System.Drawing.Point(104, 71);
            this.SecurityPolicyTB.Name = "SecurityPolicyTB";
            this.SecurityPolicyTB.Size = new System.Drawing.Size(171, 20);
            this.SecurityPolicyTB.TabIndex = 0;
            this.SecurityPolicyTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            this.SecurityPolicyTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ValueTB_KeyPress);
            this.SecurityPolicyTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ValueTB_KeyUp);
            // 
            // LogicalNameTB
            // 
            this.LogicalNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LogicalNameTB.AttributeID = 1;
            this.LogicalNameTB.Location = new System.Drawing.Point(103, 45);
            this.LogicalNameTB.Name = "LogicalNameTB";
            this.LogicalNameTB.Size = new System.Drawing.Size(171, 20);
            this.LogicalNameTB.TabIndex = 5;
            this.LogicalNameTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // GXDLMSSecuritySetupView
            // 
            this.ClientSize = new System.Drawing.Size(320, 204);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GXDLMSSecuritySetupView";
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
