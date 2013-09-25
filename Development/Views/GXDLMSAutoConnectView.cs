//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GuruxClub/GXDLMSDirector/Development/Views/GXDLMSAutoConnectView.cs $
//
// Version:         $Revision: 6333 $,
//                  $Date: 2013-05-17 12:15:22 +0300 (pe, 17 touko 2013) $
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
    [GXDLMSViewAttribute(typeof(GXDLMSAutoConnect))]
    class GXDLMSAutoConnectView : Form, IGXDLMSView
    {
        private GroupBox groupBox1;
        private GXValueField ModeTB;
        private Label CommunicationSpeedLbl;
        private GXValueField LogicalNameTB;
        private GXValueField RepetitionDelayTB;
        private Label RepetitionDelayLbl;
        private GXValueField RepetitionsTB;
        private Label RepetitionsLbl;
        private Label CallingWindowLbl;
        private ListView CallingWindowLV;
        private ColumnHeader StartTimeHeader;
        private ColumnHeader EndTimeHeader;
        private Label DestinationLbl;
        private ErrorProvider errorProvider1;
        private System.ComponentModel.IContainer components;
        private TextBox DestinationTB;
        private Label LogicalNameLbl;
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSAutoConnectView()
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
            GXDLMSAutoConnect target = Target as GXDLMSAutoConnect;
            if (attributeID == 5)
            {                
                CallingWindowLV.Items.Clear();
                foreach (var it in target.CallingWindow)
                {
                    ListViewItem li = CallingWindowLV.Items.Add(it.Key.ToString());
                    li.SubItems.Add(it.Value.ToString());
                }
            }
            else if (attributeID == 6)
            {
                DestinationTB.Text = "";
                if (target.Destinations != null)
                {
                    foreach (string it in target.Destinations)
                    {
                        DestinationTB.Text = it + Environment.NewLine;
                    }
                }
            }
            else
            {
                throw new IndexOutOfRangeException("attributeID");
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDLMSAutoConnectView));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DestinationLbl = new System.Windows.Forms.Label();
            this.CallingWindowLV = new System.Windows.Forms.ListView();
            this.StartTimeHeader = new System.Windows.Forms.ColumnHeader();
            this.EndTimeHeader = new System.Windows.Forms.ColumnHeader();
            this.CallingWindowLbl = new System.Windows.Forms.Label();
            this.RepetitionDelayTB = new GXDLMSDirector.Views.GXValueField();
            this.RepetitionDelayLbl = new System.Windows.Forms.Label();
            this.RepetitionsTB = new GXDLMSDirector.Views.GXValueField();
            this.RepetitionsLbl = new System.Windows.Forms.Label();
            this.ModeTB = new GXDLMSDirector.Views.GXValueField();
            this.CommunicationSpeedLbl = new System.Windows.Forms.Label();
            this.LogicalNameTB = new GXDLMSDirector.Views.GXValueField();
            this.LogicalNameLbl = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.DestinationTB = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.DestinationTB);
            this.groupBox1.Controls.Add(this.DestinationLbl);
            this.groupBox1.Controls.Add(this.CallingWindowLV);
            this.groupBox1.Controls.Add(this.CallingWindowLbl);
            this.groupBox1.Controls.Add(this.RepetitionDelayTB);
            this.groupBox1.Controls.Add(this.RepetitionDelayLbl);
            this.groupBox1.Controls.Add(this.RepetitionsTB);
            this.groupBox1.Controls.Add(this.RepetitionsLbl);
            this.groupBox1.Controls.Add(this.ModeTB);
            this.groupBox1.Controls.Add(this.CommunicationSpeedLbl);
            this.groupBox1.Controls.Add(this.LogicalNameTB);
            this.groupBox1.Controls.Add(this.LogicalNameLbl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 316);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Auto Connect Object";
            this.groupBox1.UseCompatibleTextRendering = true;
            // 
            // DestinationLbl
            // 
            this.DestinationLbl.AutoSize = true;
            this.DestinationLbl.Location = new System.Drawing.Point(6, 223);
            this.DestinationLbl.Name = "DestinationLbl";
            this.DestinationLbl.Size = new System.Drawing.Size(68, 13);
            this.DestinationLbl.TabIndex = 10;
            this.DestinationLbl.Text = "Destinations:";
            // 
            // CallingWindowLV
            // 
            this.CallingWindowLV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CallingWindowLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.StartTimeHeader,
            this.EndTimeHeader});
            this.CallingWindowLV.FullRowSelect = true;
            this.CallingWindowLV.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.CallingWindowLV.HideSelection = false;
            this.CallingWindowLV.Location = new System.Drawing.Point(102, 125);
            this.CallingWindowLV.Name = "CallingWindowLV";
            this.CallingWindowLV.Size = new System.Drawing.Size(188, 92);
            this.CallingWindowLV.TabIndex = 8;
            this.CallingWindowLV.UseCompatibleStateImageBehavior = false;
            this.CallingWindowLV.View = System.Windows.Forms.View.Details;
            // 
            // StartTimeHeader
            // 
            this.StartTimeHeader.Text = "Start Time:";
            this.StartTimeHeader.Width = 77;
            // 
            // EndTimeHeader
            // 
            this.EndTimeHeader.Text = "End Time:";
            this.EndTimeHeader.Width = 105;
            // 
            // CallingWindowLbl
            // 
            this.CallingWindowLbl.AutoSize = true;
            this.CallingWindowLbl.Location = new System.Drawing.Point(6, 138);
            this.CallingWindowLbl.Name = "CallingWindowLbl";
            this.CallingWindowLbl.Size = new System.Drawing.Size(83, 13);
            this.CallingWindowLbl.TabIndex = 7;
            this.CallingWindowLbl.Text = "Calling Window:";
            // 
            // RepetitionDelayTB
            // 
            this.RepetitionDelayTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RepetitionDelayTB.AttributeID = 4;
            this.RepetitionDelayTB.Location = new System.Drawing.Point(102, 99);
            this.RepetitionDelayTB.Name = "RepetitionDelayTB";
            this.RepetitionDelayTB.ReadOnly = true;
            this.RepetitionDelayTB.Size = new System.Drawing.Size(188, 20);
            this.RepetitionDelayTB.TabIndex = 5;
            this.RepetitionDelayTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // RepetitionDelayLbl
            // 
            this.RepetitionDelayLbl.AutoSize = true;
            this.RepetitionDelayLbl.Location = new System.Drawing.Point(6, 102);
            this.RepetitionDelayLbl.Name = "RepetitionDelayLbl";
            this.RepetitionDelayLbl.Size = new System.Drawing.Size(88, 13);
            this.RepetitionDelayLbl.TabIndex = 6;
            this.RepetitionDelayLbl.Text = "Repetition Delay:";
            // 
            // RepetitionsTB
            // 
            this.RepetitionsTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RepetitionsTB.AttributeID = 3;
            this.RepetitionsTB.Location = new System.Drawing.Point(102, 73);
            this.RepetitionsTB.Name = "RepetitionsTB";
            this.RepetitionsTB.ReadOnly = true;
            this.RepetitionsTB.Size = new System.Drawing.Size(188, 20);
            this.RepetitionsTB.TabIndex = 3;
            this.RepetitionsTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // RepetitionsLbl
            // 
            this.RepetitionsLbl.AutoSize = true;
            this.RepetitionsLbl.Location = new System.Drawing.Point(6, 76);
            this.RepetitionsLbl.Name = "RepetitionsLbl";
            this.RepetitionsLbl.Size = new System.Drawing.Size(63, 13);
            this.RepetitionsLbl.TabIndex = 4;
            this.RepetitionsLbl.Text = "Repetitions:";
            // 
            // ModeTB
            // 
            this.ModeTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ModeTB.AttributeID = 2;
            this.ModeTB.Location = new System.Drawing.Point(102, 47);
            this.ModeTB.Name = "ModeTB";
            this.ModeTB.ReadOnly = true;
            this.ModeTB.Size = new System.Drawing.Size(188, 20);
            this.ModeTB.TabIndex = 0;
            this.ModeTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // CommunicationSpeedLbl
            // 
            this.CommunicationSpeedLbl.AutoSize = true;
            this.CommunicationSpeedLbl.Location = new System.Drawing.Point(6, 50);
            this.CommunicationSpeedLbl.Name = "CommunicationSpeedLbl";
            this.CommunicationSpeedLbl.Size = new System.Drawing.Size(37, 13);
            this.CommunicationSpeedLbl.TabIndex = 2;
            this.CommunicationSpeedLbl.Text = "Mode:";
            // 
            // LogicalNameTB
            // 
            this.LogicalNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LogicalNameTB.AttributeID = 1;
            this.LogicalNameTB.Location = new System.Drawing.Point(102, 21);
            this.LogicalNameTB.Name = "LogicalNameTB";
            this.LogicalNameTB.ReadOnly = true;
            this.LogicalNameTB.Size = new System.Drawing.Size(188, 20);
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
            // DestinationTB
            // 
            this.DestinationTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DestinationTB.Location = new System.Drawing.Point(102, 223);
            this.DestinationTB.Multiline = true;
            this.DestinationTB.Name = "DestinationTB";
            this.DestinationTB.Size = new System.Drawing.Size(188, 87);
            this.DestinationTB.TabIndex = 11;
            // 
            // GXDLMSAutoConnectView
            // 
            this.ClientSize = new System.Drawing.Size(320, 335);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GXDLMSAutoConnectView";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

    }
}
