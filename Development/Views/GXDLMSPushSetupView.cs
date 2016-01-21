//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/Views/GXDLMSPushSetupView.cs $
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
    [GXDLMSViewAttribute(typeof(GXDLMSPushSetup))]
    class GXDLMSPushSetupView : Form, IGXDLMSView
    {
        private GroupBox groupBox1;
        private Label ValueLbl;
        private GXValueField LogicalNameTB;
        private ErrorProvider errorProvider1;
        private System.ComponentModel.IContainer components;
        private Label DescriptionLbl;
        private TextBox DescriptionTB;
        private ListView ObjectsLV;
        private ColumnHeader TypeHeader;
        private ColumnHeader LogicalNameHeader;
        private ColumnHeader AttributeIndexHeader;
        private ColumnHeader DataIndexHeader;
        private ListView CommunicationWindowLV;
        private Label CommunicationWindowLbl;
        private ColumnHeader StartHeader;
        private GroupBox GXSendDestinationAndMethodGB;
        private Label MessageLbl;
        private TextBox MessageTB;
        private Label DestinationLbl;
        private TextBox DestinationTB;
        private Label ServiceLbl;
        private TextBox ServiceTB;
        private ColumnHeader EndHeader;
        private GXValueField RepetitionDelayTB;
        private Label LblRepetitionDelay;
        private GXValueField NumberOfRetriesTB;
        private Label NumberOfRetriesLbl;
        private GXValueField RandomisationStartIntervalTB;
        private Label RandomisationStartIntervalLbl;
        private Label LogicalNameLbl;
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSPushSetupView()
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
                ObjectsLV.Items.Clear();
                foreach (GXDLMSPushObject it in ((GXDLMSPushSetup)Target).PushObjectList)
                {
                    ListViewItem li = new ListViewItem(it.Type.ToString());
                    li.SubItems.Add(it.LogicalName);
                    li.SubItems.Add(it.AttributeIndex.ToString());
                    li.SubItems.Add(it.DataIndex.ToString());
                    ObjectsLV.Items.Add(li);
                }
            }
            else if (attributeID == 3)
            {
                ServiceTB.Text = ((GXDLMSPushSetup)Target).SendDestinationAndMethod.Service.ToString();
                DestinationTB.Text = ((GXDLMSPushSetup)Target).SendDestinationAndMethod.Destination;
                MessageTB.Text = ((GXDLMSPushSetup)Target).SendDestinationAndMethod.Message.ToString();
            }
            else if (attributeID == 4)
            {
                CommunicationWindowLV.Items.Clear();
                foreach (KeyValuePair<GXDateTime, GXDateTime> it in ((GXDLMSPushSetup)Target).CommunicationWindow)
                {
                    ListViewItem li = new ListViewItem(it.Key.ToString());
                    li.SubItems.Add(it.Value.ToString());
                    CommunicationWindowLV.Items.Add(li);
                }
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
                return DescriptionTB.Text;
            }
            set
            {
                DescriptionTB.Text = value;
            }
        }

        public void OnDirtyChange(int attributeID, bool Dirty)
        {
            errorProvider1.Clear();
        }

        public void OnAccessRightsChange(int attributeID, AccessMode access)
        {
            if (attributeID == 2)
            {
                ObjectsLV.Enabled = access != AccessMode.NoAccess;
            }
            else if (attributeID == 3)
            {
                ServiceTB.ReadOnly = DestinationTB.ReadOnly = MessageTB.ReadOnly = access == AccessMode.NoAccess;
            }
            else if (attributeID == 4)
            {
                CommunicationWindowLV.Enabled = access != AccessMode.NoAccess;
            }
            else
            {
                throw new IndexOutOfRangeException("attributeID");
            }
        }

        #endregion


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDLMSPushSetupView));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RepetitionDelayTB = new GXDLMSDirector.Views.GXValueField();
            this.LblRepetitionDelay = new System.Windows.Forms.Label();
            this.NumberOfRetriesTB = new GXDLMSDirector.Views.GXValueField();
            this.NumberOfRetriesLbl = new System.Windows.Forms.Label();
            this.RandomisationStartIntervalTB = new GXDLMSDirector.Views.GXValueField();
            this.RandomisationStartIntervalLbl = new System.Windows.Forms.Label();
            this.GXSendDestinationAndMethodGB = new System.Windows.Forms.GroupBox();
            this.MessageLbl = new System.Windows.Forms.Label();
            this.MessageTB = new System.Windows.Forms.TextBox();
            this.DestinationLbl = new System.Windows.Forms.Label();
            this.DestinationTB = new System.Windows.Forms.TextBox();
            this.ServiceLbl = new System.Windows.Forms.Label();
            this.ServiceTB = new System.Windows.Forms.TextBox();
            this.CommunicationWindowLV = new System.Windows.Forms.ListView();
            this.StartHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EndHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CommunicationWindowLbl = new System.Windows.Forms.Label();
            this.ObjectsLV = new System.Windows.Forms.ListView();
            this.TypeHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LogicalNameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AttributeIndexHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DataIndexHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DescriptionLbl = new System.Windows.Forms.Label();
            this.DescriptionTB = new System.Windows.Forms.TextBox();
            this.ValueLbl = new System.Windows.Forms.Label();
            this.LogicalNameTB = new GXDLMSDirector.Views.GXValueField();
            this.LogicalNameLbl = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.GXSendDestinationAndMethodGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.RepetitionDelayTB);
            this.groupBox1.Controls.Add(this.LblRepetitionDelay);
            this.groupBox1.Controls.Add(this.NumberOfRetriesTB);
            this.groupBox1.Controls.Add(this.NumberOfRetriesLbl);
            this.groupBox1.Controls.Add(this.RandomisationStartIntervalTB);
            this.groupBox1.Controls.Add(this.RandomisationStartIntervalLbl);
            this.groupBox1.Controls.Add(this.GXSendDestinationAndMethodGB);
            this.groupBox1.Controls.Add(this.CommunicationWindowLV);
            this.groupBox1.Controls.Add(this.CommunicationWindowLbl);
            this.groupBox1.Controls.Add(this.ObjectsLV);
            this.groupBox1.Controls.Add(this.DescriptionLbl);
            this.groupBox1.Controls.Add(this.DescriptionTB);
            this.groupBox1.Controls.Add(this.ValueLbl);
            this.groupBox1.Controls.Add(this.LogicalNameTB);
            this.groupBox1.Controls.Add(this.LogicalNameLbl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(421, 427);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Object";
            // 
            // RepetitionDelayTB
            // 
            this.RepetitionDelayTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RepetitionDelayTB.AttributeID = 7;
            this.RepetitionDelayTB.Location = new System.Drawing.Point(103, 401);
            this.RepetitionDelayTB.Name = "RepetitionDelayTB";
            this.RepetitionDelayTB.Size = new System.Drawing.Size(296, 20);
            this.RepetitionDelayTB.TabIndex = 22;
            this.RepetitionDelayTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // LblRepetitionDelay
            // 
            this.LblRepetitionDelay.AutoSize = true;
            this.LblRepetitionDelay.Location = new System.Drawing.Point(7, 404);
            this.LblRepetitionDelay.Name = "LblRepetitionDelay";
            this.LblRepetitionDelay.Size = new System.Drawing.Size(86, 13);
            this.LblRepetitionDelay.TabIndex = 21;
            this.LblRepetitionDelay.Text = "Repetition delay:";
            // 
            // NumberOfRetriesTB
            // 
            this.NumberOfRetriesTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.NumberOfRetriesTB.AttributeID = 6;
            this.NumberOfRetriesTB.Location = new System.Drawing.Point(103, 376);
            this.NumberOfRetriesTB.Name = "NumberOfRetriesTB";
            this.NumberOfRetriesTB.Size = new System.Drawing.Size(296, 20);
            this.NumberOfRetriesTB.TabIndex = 20;
            this.NumberOfRetriesTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // NumberOfRetriesLbl
            // 
            this.NumberOfRetriesLbl.AutoSize = true;
            this.NumberOfRetriesLbl.Location = new System.Drawing.Point(7, 379);
            this.NumberOfRetriesLbl.Name = "NumberOfRetriesLbl";
            this.NumberOfRetriesLbl.Size = new System.Drawing.Size(90, 13);
            this.NumberOfRetriesLbl.TabIndex = 19;
            this.NumberOfRetriesLbl.Text = "Number of retries:";
            // 
            // RandomisationStartIntervalTB
            // 
            this.RandomisationStartIntervalTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RandomisationStartIntervalTB.AttributeID = 5;
            this.RandomisationStartIntervalTB.Location = new System.Drawing.Point(103, 350);
            this.RandomisationStartIntervalTB.Name = "RandomisationStartIntervalTB";
            this.RandomisationStartIntervalTB.Size = new System.Drawing.Size(296, 20);
            this.RandomisationStartIntervalTB.TabIndex = 18;
            this.RandomisationStartIntervalTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // RandomisationStartIntervalLbl
            // 
            this.RandomisationStartIntervalLbl.AutoSize = true;
            this.RandomisationStartIntervalLbl.Location = new System.Drawing.Point(7, 353);
            this.RandomisationStartIntervalLbl.Name = "RandomisationStartIntervalLbl";
            this.RandomisationStartIntervalLbl.Size = new System.Drawing.Size(80, 13);
            this.RandomisationStartIntervalLbl.TabIndex = 17;
            this.RandomisationStartIntervalLbl.Text = "Randomisation:";
            // 
            // GXSendDestinationAndMethodGB
            // 
            this.GXSendDestinationAndMethodGB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GXSendDestinationAndMethodGB.Controls.Add(this.MessageLbl);
            this.GXSendDestinationAndMethodGB.Controls.Add(this.MessageTB);
            this.GXSendDestinationAndMethodGB.Controls.Add(this.DestinationLbl);
            this.GXSendDestinationAndMethodGB.Controls.Add(this.DestinationTB);
            this.GXSendDestinationAndMethodGB.Controls.Add(this.ServiceLbl);
            this.GXSendDestinationAndMethodGB.Controls.Add(this.ServiceTB);
            this.GXSendDestinationAndMethodGB.Location = new System.Drawing.Point(0, 155);
            this.GXSendDestinationAndMethodGB.Name = "GXSendDestinationAndMethodGB";
            this.GXSendDestinationAndMethodGB.Size = new System.Drawing.Size(421, 108);
            this.GXSendDestinationAndMethodGB.TabIndex = 16;
            this.GXSendDestinationAndMethodGB.TabStop = false;
            this.GXSendDestinationAndMethodGB.Text = "Send destination and method";
            // 
            // MessageLbl
            // 
            this.MessageLbl.AutoSize = true;
            this.MessageLbl.Location = new System.Drawing.Point(7, 74);
            this.MessageLbl.Name = "MessageLbl";
            this.MessageLbl.Size = new System.Drawing.Size(53, 13);
            this.MessageLbl.TabIndex = 11;
            this.MessageLbl.Text = "Message:";
            // 
            // MessageTB
            // 
            this.MessageTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MessageTB.Location = new System.Drawing.Point(103, 71);
            this.MessageTB.Name = "MessageTB";
            this.MessageTB.Size = new System.Drawing.Size(297, 20);
            this.MessageTB.TabIndex = 10;
            // 
            // DestinationLbl
            // 
            this.DestinationLbl.AutoSize = true;
            this.DestinationLbl.Location = new System.Drawing.Point(7, 48);
            this.DestinationLbl.Name = "DestinationLbl";
            this.DestinationLbl.Size = new System.Drawing.Size(63, 13);
            this.DestinationLbl.TabIndex = 9;
            this.DestinationLbl.Text = "Destination:";
            // 
            // DestinationTB
            // 
            this.DestinationTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DestinationTB.Location = new System.Drawing.Point(103, 45);
            this.DestinationTB.Name = "DestinationTB";
            this.DestinationTB.Size = new System.Drawing.Size(297, 20);
            this.DestinationTB.TabIndex = 8;
            // 
            // ServiceLbl
            // 
            this.ServiceLbl.AutoSize = true;
            this.ServiceLbl.Location = new System.Drawing.Point(7, 22);
            this.ServiceLbl.Name = "ServiceLbl";
            this.ServiceLbl.Size = new System.Drawing.Size(46, 13);
            this.ServiceLbl.TabIndex = 7;
            this.ServiceLbl.Text = "Service:";
            // 
            // ServiceTB
            // 
            this.ServiceTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ServiceTB.Location = new System.Drawing.Point(103, 19);
            this.ServiceTB.Name = "ServiceTB";
            this.ServiceTB.Size = new System.Drawing.Size(297, 20);
            this.ServiceTB.TabIndex = 6;
            // 
            // CommunicationWindowLV
            // 
            this.CommunicationWindowLV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CommunicationWindowLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.StartHeader,
            this.EndHeader});
            this.CommunicationWindowLV.FullRowSelect = true;
            this.CommunicationWindowLV.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.CommunicationWindowLV.HideSelection = false;
            this.CommunicationWindowLV.Location = new System.Drawing.Point(102, 268);
            this.CommunicationWindowLV.Name = "CommunicationWindowLV";
            this.CommunicationWindowLV.Size = new System.Drawing.Size(296, 75);
            this.CommunicationWindowLV.TabIndex = 15;
            this.CommunicationWindowLV.UseCompatibleStateImageBehavior = false;
            this.CommunicationWindowLV.View = System.Windows.Forms.View.Details;
            // 
            // StartHeader
            // 
            this.StartHeader.Text = "Start";
            this.StartHeader.Width = 128;
            // 
            // EndHeader
            // 
            this.EndHeader.Text = "End";
            this.EndHeader.Width = 157;
            // 
            // CommunicationWindowLbl
            // 
            this.CommunicationWindowLbl.Location = new System.Drawing.Point(6, 268);
            this.CommunicationWindowLbl.Name = "CommunicationWindowLbl";
            this.CommunicationWindowLbl.Size = new System.Drawing.Size(91, 61);
            this.CommunicationWindowLbl.TabIndex = 14;
            this.CommunicationWindowLbl.Text = "Communication Window:";
            // 
            // ObjectsLV
            // 
            this.ObjectsLV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ObjectsLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TypeHeader,
            this.LogicalNameHeader,
            this.AttributeIndexHeader,
            this.DataIndexHeader});
            this.ObjectsLV.FullRowSelect = true;
            this.ObjectsLV.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ObjectsLV.HideSelection = false;
            this.ObjectsLV.Location = new System.Drawing.Point(103, 74);
            this.ObjectsLV.Name = "ObjectsLV";
            this.ObjectsLV.Size = new System.Drawing.Size(296, 75);
            this.ObjectsLV.TabIndex = 13;
            this.ObjectsLV.UseCompatibleStateImageBehavior = false;
            this.ObjectsLV.View = System.Windows.Forms.View.Details;
            // 
            // TypeHeader
            // 
            this.TypeHeader.Text = "Type";
            this.TypeHeader.Width = 48;
            // 
            // LogicalNameHeader
            // 
            this.LogicalNameHeader.Text = "LogicalName";
            this.LogicalNameHeader.Width = 76;
            // 
            // AttributeIndexHeader
            // 
            this.AttributeIndexHeader.Text = "Attribute Index";
            this.AttributeIndexHeader.Width = 85;
            // 
            // DataIndexHeader
            // 
            this.DataIndexHeader.Text = "Data Index";
            this.DataIndexHeader.Width = 74;
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
            this.DescriptionTB.Size = new System.Drawing.Size(297, 20);
            this.DescriptionTB.TabIndex = 4;
            // 
            // ValueLbl
            // 
            this.ValueLbl.AutoSize = true;
            this.ValueLbl.Location = new System.Drawing.Point(7, 74);
            this.ValueLbl.Name = "ValueLbl";
            this.ValueLbl.Size = new System.Drawing.Size(46, 13);
            this.ValueLbl.TabIndex = 2;
            this.ValueLbl.Text = "Objects:";
            // 
            // LogicalNameTB
            // 
            this.LogicalNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LogicalNameTB.AttributeID = 1;
            this.LogicalNameTB.Location = new System.Drawing.Point(103, 45);
            this.LogicalNameTB.Name = "LogicalNameTB";
            this.LogicalNameTB.Size = new System.Drawing.Size(296, 20);
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
            // GXDLMSPushSetupView
            // 
            this.ClientSize = new System.Drawing.Size(445, 451);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GXDLMSPushSetupView";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.GXSendDestinationAndMethodGB.ResumeLayout(false);
            this.GXSendDestinationAndMethodGB.PerformLayout();
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
