//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/Views/GXDLMSDataView.cs $
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
    [GXDLMSViewAttribute(typeof(GXDLMSData))]
    class GXDLMSDataView : Form, IGXDLMSView
    {
        private GroupBox groupBox1;
        private GXValueField ValueTB;
        private Label ValueLbl;
        private GXValueField LogicalNameTB;
        private ErrorProvider errorProvider1;
        private System.ComponentModel.IContainer components;
        private Label DescriptionLbl;
        private TextBox DescriptionTB;
        private Label LogicalNameLbl;
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSDataView()
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
                errorProvider1.SetError(ValueTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDLMSDataView));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ValueTB = new GXDLMSDirector.Views.GXValueField();
            this.ValueLbl = new System.Windows.Forms.Label();
            this.LogicalNameTB = new GXDLMSDirector.Views.GXValueField();
            this.LogicalNameLbl = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.DescriptionLbl = new System.Windows.Forms.Label();
            this.DescriptionTB = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.DescriptionLbl);
            this.groupBox1.Controls.Add(this.DescriptionTB);
            this.groupBox1.Controls.Add(this.ValueTB);
            this.groupBox1.Controls.Add(this.ValueLbl);
            this.groupBox1.Controls.Add(this.LogicalNameTB);
            this.groupBox1.Controls.Add(this.LogicalNameLbl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 113);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Object";
            // 
            // ValueTB
            // 
            this.ValueTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ValueTB.AttributeID = 2;
            this.ValueTB.Location = new System.Drawing.Point(104, 71);
            this.ValueTB.Name = "ValueTB";
            this.ValueTB.Size = new System.Drawing.Size(171, 20);
            this.ValueTB.TabIndex = 0;
            this.ValueTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            this.ValueTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ValueTB_KeyUp);
            this.ValueTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ValueTB_KeyPress);
            // 
            // ValueLbl
            // 
            this.ValueLbl.AutoSize = true;
            this.ValueLbl.Location = new System.Drawing.Point(7, 74);
            this.ValueLbl.Name = "ValueLbl";
            this.ValueLbl.Size = new System.Drawing.Size(37, 13);
            this.ValueLbl.TabIndex = 2;
            this.ValueLbl.Text = "Value:";
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
            // GXDLMSDataView
            // 
            this.ClientSize = new System.Drawing.Size(320, 141);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GXDLMSDataView";
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
