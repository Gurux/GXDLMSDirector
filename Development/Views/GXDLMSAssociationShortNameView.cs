//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GXDLMSDirector/Development/Views/GXDLMSAssociationShortNameView.cs $
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
using Gurux.DLMS.Objects;
using GXDLMS.Common;
using Gurux.DLMS;

namespace GXDLMSDirector.Views
{
    [GXDLMSViewAttribute(typeof(GXDLMSAssociationShortName))]
    class GXDLMSAssociationShortNameView : Form, IGXDLMSView
    {
        private GroupBox groupBox1;
        private GXValueField LogicalNameTB;        
        private ListView CallingWindowLV;
        private ColumnHeader BaseNameHeader;
        private ColumnHeader ClassIdHeader;
        private ColumnHeader VersionHeader;
        private ColumnHeader LogicalNameHeader;
        private ColumnHeader AttributeAccesssHeader;
        private ColumnHeader MethodAccessHeader;
        private ErrorProvider errorProvider1;
        private System.ComponentModel.IContainer components;
        private Label LogicalNameLbl;
        Dictionary<int, ListViewItem> SNItems = new Dictionary<int, ListViewItem>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSAssociationShortNameView()
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
            GXDLMSAssociationShortName target = Target as GXDLMSAssociationShortName;
            if (attributeID == 2)
            {
                GXDLMSObjectCollection items = target.ObjectList;
                CallingWindowLV.Items.Clear();
                if (items != null)
                {
                    SNItems.Clear();
                    foreach (GXDLMSObject it in items)
                    {
                        int sn = it.ShortName;
                        /*
                        int tmp = Convert.ToInt32(it[0]);
                        if (((tmp >> 16) & 0xFFFF) == 0xFFFF)
                        {
                            tmp = tmp & 0xFFFF;
                        }
                         * */
                        ListViewItem li = CallingWindowLV.Items.Add(Convert.ToString(sn, 16));
                        /*
                        li.SubItems.Add(((ObjectType)Convert.ToInt32(it[1])).ToString());
                        li.SubItems.Add(it[2].ToString());
                        li.SubItems.Add(GXHelpers.ConvertFromDLMS(it[3],DataType.OctetString, DataType.OctetString, false).ToString());
                        li.SubItems.Add("");
                         * */
                        li.SubItems.AddRange(new string[] { it.ObjectType.ToString(), it.Version.ToString(), 
                                            it.LogicalName, "", ""});
                        SNItems.Add(sn, li);                        
                    }
                }
            }
            //Update Access rights.
            if (attributeID == 3)
            {
                //access_rights: access_right
                object[] access = (object[])target.AccessRightsList;
                if (access != null)
                {
                    foreach (object[] it in access)
                    {
                        int sn = (Convert.ToInt32(it[0]) & 0xFFFF);
                        if (SNItems.ContainsKey(sn))
                        {
                            ListViewItem li = SNItems[sn];
                            List<string> modes = new List<string>();                            
                            foreach (object[] attributeAccess in (object[])it[1])
                            {
                                uint id = Convert.ToUInt32(attributeAccess[0]);
                                AccessMode mode = (AccessMode)Convert.ToInt32(attributeAccess[1]);
                                modes.Add(id.ToString() + " = " + mode);
                            }
                            string str = null;
                            //Show attribute access.
                            foreach (string m in modes)
                            {
                                if (str != null)
                                {
                                    str += ", ";
                                }
                                str += m.ToString();
                            }
                            li.SubItems[4].Text = str;
                            foreach (object[] attributeAccess in (object[])it[2])
                            {
                                uint id = Convert.ToUInt32(attributeAccess[0]);
                                AccessMode mode = (AccessMode)Convert.ToInt32(attributeAccess[1]);
                                modes.Add(id.ToString() + " = " + mode);
                            }
                            //Show Method access.
                            str = null;
                            foreach (string m in modes)
                            {
                                if (str != null)
                                {
                                    str += ", ";
                                }
                                str += m.ToString();
                            }
                            li.SubItems[5].Text = str;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDLMSAssociationShortNameView));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CallingWindowLV = new System.Windows.Forms.ListView();
            this.BaseNameHeader = new System.Windows.Forms.ColumnHeader();
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
            this.groupBox1.Controls.Add(this.CallingWindowLV);
            this.groupBox1.Controls.Add(this.LogicalNameTB);
            this.groupBox1.Controls.Add(this.LogicalNameLbl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(546, 324);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Association Short Name Object";
            // 
            // CallingWindowLV
            // 
            this.CallingWindowLV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CallingWindowLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.BaseNameHeader,
            this.ClassIdHeader,
            this.VersionHeader,
            this.LogicalNameHeader,
            this.AttributeAccesssHeader,
            this.MethodAccessHeader});
            this.CallingWindowLV.FullRowSelect = true;
            this.CallingWindowLV.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.CallingWindowLV.HideSelection = false;
            this.CallingWindowLV.Location = new System.Drawing.Point(9, 47);
            this.CallingWindowLV.Name = "CallingWindowLV";
            this.CallingWindowLV.Size = new System.Drawing.Size(514, 271);
            this.CallingWindowLV.TabIndex = 9;
            this.CallingWindowLV.UseCompatibleStateImageBehavior = false;
            this.CallingWindowLV.View = System.Windows.Forms.View.Details;
            // 
            // BaseNameHeader
            // 
            this.BaseNameHeader.Text = "Base Name:";
            this.BaseNameHeader.Width = 77;
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
            this.LogicalNameHeader.Width = 84;
            // 
            // AttributeAccesssHeader
            // 
            this.AttributeAccesssHeader.Text = "Attribute Accesss";
            this.AttributeAccesssHeader.Width = 83;
            // 
            // MethodAccessHeader
            // 
            this.MethodAccessHeader.Text = "Method Access";
            // 
            // LogicalNameTB
            // 
            this.LogicalNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LogicalNameTB.AttributeID = 1;
            this.LogicalNameTB.Location = new System.Drawing.Point(102, 21);
            this.LogicalNameTB.Name = "LogicalNameTB";
            this.LogicalNameTB.Size = new System.Drawing.Size(421, 20);
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
            // GXDLMSAssociationShortNameView
            // 
            this.ClientSize = new System.Drawing.Size(570, 348);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GXDLMSAssociationShortNameView";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
