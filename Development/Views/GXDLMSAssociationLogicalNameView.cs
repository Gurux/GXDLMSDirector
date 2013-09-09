//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GXDLMSDirector/Development/Views/GXDLMSAssociationLogicalNameView.cs $
//
// Version:         $Revision: 4781 $,
//                  $Date: 2012-03-19 10:23:38 +0200 (ma, 19 maalis 2012) $
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
using GXDLMS.Common;
using Gurux.DLMS.Objects;

namespace GXDLMSDirector.Views
{
    [GXDLMSViewAttribute(typeof(GXDLMSAssociationLogicalName))]
    class GXDLMSAssociationLogicalNameView : Form, IGXDLMSView
    {
        private GroupBox groupBox1;
        private GXValueField LogicalNameTB;
        private ListView CallingWindowLV;
        private ColumnHeader ClassIdHeader;
        private ColumnHeader VersionHeader;
        private ColumnHeader LogicalNameHeader;
        private ColumnHeader AttributeAccesssHeader;
        private ColumnHeader MethodAccessHeader;
        private GXValueField gxValueField1;
        private Label label1;
        private ErrorProvider errorProvider1;
        private System.ComponentModel.IContainer components;
        private Label LogicalNameLbl;
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSAssociationLogicalNameView()
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
                GXDLMSAssociationLogicalName target = Target as GXDLMSAssociationLogicalName;
                GXDLMSObjectCollection items = target.ObjectList;
                CallingWindowLV.Items.Clear();
                if (items != null)
                {
                    foreach (GXDLMSObject it in items)
                    {
                        ListViewItem li = CallingWindowLV.Items.Add(it.ObjectType.ToString());
                        li.SubItems.Add(it.Version.ToString()); //Version                                                
                        li.SubItems.Add(it.LogicalName);
                        //access_rights: access_right                        
                        if (it is IGXDLMSBase)
                        {
                            string str = null;
                            //Show attribute access.
                            int cnt = (it as IGXDLMSBase).GetAttributeCount();                            
                            for(int pos = 1; pos != cnt + 1; ++pos)
                            {
                                if (str != null)
                                {
                                    str += ", ";
                                }
                                AccessMode mode = it.GetAccess(pos);
                                str += pos.ToString() + " = " + mode;
                            }                            
                            li.SubItems.Add(str);
                            //Show method access.
                            str = null;
                            cnt = (it as IGXDLMSBase).GetMethodCount();
                            for (int pos = 1; pos != cnt + 1; ++pos)
                            {
                                if (str != null)
                                {
                                    str += ", ";
                                }
                                MethodAccessMode mode = it.GetMethodAccess(pos);
                                str += pos.ToString() + " = " + mode;
                            }
                            li.SubItems.Add(str);
                        }
                    }
                }                 
            }           
        }

        public void OnAccessRightsChange(int attributeID, AccessMode access)
        {            
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDLMSAssociationLogicalNameView));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gxValueField1 = new GXDLMSDirector.Views.GXValueField();
            this.label1 = new System.Windows.Forms.Label();
            this.CallingWindowLV = new System.Windows.Forms.ListView();
            this.ClassIdHeader = new System.Windows.Forms.ColumnHeader();
            this.VersionHeader = new System.Windows.Forms.ColumnHeader();
            this.LogicalNameHeader = new System.Windows.Forms.ColumnHeader();
            this.AttributeAccesssHeader = new System.Windows.Forms.ColumnHeader();
            this.MethodAccessHeader = new System.Windows.Forms.ColumnHeader();
            this.LogicalNameTB = new GXDLMSDirector.Views.GXValueField();
            this.LogicalNameLbl = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gxValueField1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.CallingWindowLV);
            this.groupBox1.Controls.Add(this.LogicalNameTB);
            this.groupBox1.Controls.Add(this.LogicalNameLbl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(515, 324);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Association Logical  Name Object";
            // 
            // gxValueField1
            // 
            this.gxValueField1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gxValueField1.AttributeID = 8;
            this.gxValueField1.Location = new System.Drawing.Point(102, 47);
            this.gxValueField1.Name = "gxValueField1";
            this.gxValueField1.Size = new System.Drawing.Size(390, 20);
            this.gxValueField1.TabIndex = 11;
            this.gxValueField1.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Association Status:";
            // 
            // CallingWindowLV
            // 
            this.CallingWindowLV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CallingWindowLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ClassIdHeader,
            this.VersionHeader,
            this.LogicalNameHeader,
            this.AttributeAccesssHeader,
            this.MethodAccessHeader});
            this.CallingWindowLV.FullRowSelect = true;
            this.CallingWindowLV.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.CallingWindowLV.HideSelection = false;
            this.CallingWindowLV.Location = new System.Drawing.Point(9, 103);
            this.CallingWindowLV.Name = "CallingWindowLV";
            this.CallingWindowLV.Size = new System.Drawing.Size(483, 215);
            this.CallingWindowLV.TabIndex = 9;
            this.CallingWindowLV.UseCompatibleStateImageBehavior = false;
            this.CallingWindowLV.View = System.Windows.Forms.View.Details;
            // 
            // ClassIdHeader
            // 
            this.ClassIdHeader.Text = "Class ID:";
            this.ClassIdHeader.Width = 105;
            // 
            // VersionHeader
            // 
            this.VersionHeader.Text = "Version:";
            // 
            // LogicalNameHeader
            // 
            this.LogicalNameHeader.Text = "Logical Name:";
            this.LogicalNameHeader.Width = 106;
            // 
            // AttributeAccesssHeader
            // 
            this.AttributeAccesssHeader.Text = "Acctibute Access:";
            this.AttributeAccesssHeader.Width = 106;
            // 
            // MethodAccessHeader
            // 
            this.MethodAccessHeader.Text = "Method Access:";
            this.MethodAccessHeader.Width = 97;
            // 
            // LogicalNameTB
            // 
            this.LogicalNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LogicalNameTB.AttributeID = 1;
            this.LogicalNameTB.Location = new System.Drawing.Point(102, 21);
            this.LogicalNameTB.Name = "LogicalNameTB";
            this.LogicalNameTB.Size = new System.Drawing.Size(390, 20);
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
            // GXDLMSAssociationLogicalNameView
            // 
            this.ClientSize = new System.Drawing.Size(539, 348);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GXDLMSAssociationLogicalNameView";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
