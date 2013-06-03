//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GXDLMSDirector/Development/Views/GXDLMSClockView.cs $
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
using Gurux.DLMS.Objects;
using Gurux.DLMS;

namespace GXDLMSDirector.Views
{
    [GXDLMSViewAttribute(typeof(GXDLMSClock))]
    class GXDLMSClockView : Form, IGXDLMSView
    {
        private GroupBox groupBox1;
        private GXValueField TimeTB;
        private Label TimeLbl;
        private GXValueField LogicalNameTB;
        private GXValueField StatusTB;
        private Label StatusLbl;
        private GXValueField TimeZoneTB;
        private Label TimeZoneLbl;
        private GXValueField ClockBaseTB;
        private Label ClockBaseLbl;
        private GroupBox groupBox2;
        private CheckBox EnabledCB;
        private GXValueField DeviationTB;
        private Label DeviationLbl;
        private GXValueField EndTB;
        private Label EndLbl;
        private GXValueField BeginTB;
        private Label BeginLbl;
        private Label EnabledLbl;
        private ErrorProvider errorProvider1;
        private System.ComponentModel.IContainer components;
        private Label LogicalNameLbl;
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSClockView()
        {
            InitializeComponent();
            this.EnabledCB.Checked = false;
        }
        
        #region IGXDLMSView Members

        public GXDLMSObject Target
        {
            get;
            set;
        }

        public void OnValueChanged(int attributeID, object value)
        {
            GXDLMSClock target = Target as GXDLMSClock;            
            if (attributeID == 5)
            {
                BeginTB.Value = value;
            }
            else if (attributeID == 6)
            {
                EndTB.Value = value;
            }
            else if (attributeID == 7)
            {
                DeviationTB.Value = value;
            }                
            else if (attributeID == 8)
            {                
                this.EnabledCB.Checked = target.Enabled;
                EnabledCB_CheckedChanged(this, null);                
            }
            else
            {
                throw new IndexOutOfRangeException("attributeID");
            }        
        }

        public void OnAccessRightsChange(int attributeID, AccessMode access)
        {
            if (attributeID == 5)
            {
                BeginTB.Enabled = access > AccessMode.Read;
            }
            else if (attributeID == 6)
            {
                EndTB.Enabled = access > AccessMode.Read;
            }
            else if (attributeID == 7)
            {
                DeviationTB.Enabled = access > AccessMode.Read;
            }
            else if (attributeID == 8)
            {
                EnabledCB.Enabled = access > AccessMode.Read;
            }
            else
            {
                throw new NotImplementedException();
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
            switch (attributeID)
            {
                case 2:
                    errorProvider1.SetError(TimeTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 3:
                    errorProvider1.SetError(TimeZoneTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 4:
                    errorProvider1.SetError(StatusTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 5:
                    errorProvider1.SetError(BeginTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 6:
                    errorProvider1.SetError(EndTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 7:
                    errorProvider1.SetError(DeviationTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 8:
                    errorProvider1.SetError(EnabledCB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                    break;
                case 9:
                    errorProvider1.SetError(ClockBaseTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDLMSClockView));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ClockBaseTB = new GXDLMSDirector.Views.GXValueField();
            this.ClockBaseLbl = new System.Windows.Forms.Label();
            this.StatusTB = new GXDLMSDirector.Views.GXValueField();
            this.StatusLbl = new System.Windows.Forms.Label();
            this.TimeZoneTB = new GXDLMSDirector.Views.GXValueField();
            this.TimeZoneLbl = new System.Windows.Forms.Label();
            this.TimeTB = new GXDLMSDirector.Views.GXValueField();
            this.TimeLbl = new System.Windows.Forms.Label();
            this.LogicalNameTB = new GXDLMSDirector.Views.GXValueField();
            this.LogicalNameLbl = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.EndTB = new GXDLMSDirector.Views.GXValueField();
            this.EnabledLbl = new System.Windows.Forms.Label();
            this.EnabledCB = new System.Windows.Forms.CheckBox();
            this.DeviationTB = new GXDLMSDirector.Views.GXValueField();
            this.DeviationLbl = new System.Windows.Forms.Label();
            this.EndLbl = new System.Windows.Forms.Label();
            this.BeginTB = new GXDLMSDirector.Views.GXValueField();
            this.BeginLbl = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ClockBaseTB);
            this.groupBox1.Controls.Add(this.ClockBaseLbl);
            this.groupBox1.Controls.Add(this.StatusTB);
            this.groupBox1.Controls.Add(this.StatusLbl);
            this.groupBox1.Controls.Add(this.TimeZoneTB);
            this.groupBox1.Controls.Add(this.TimeZoneLbl);
            this.groupBox1.Controls.Add(this.TimeTB);
            this.groupBox1.Controls.Add(this.TimeLbl);
            this.groupBox1.Controls.Add(this.LogicalNameTB);
            this.groupBox1.Controls.Add(this.LogicalNameLbl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(333, 291);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Clock Object";
            // 
            // ClockBaseTB
            // 
            this.ClockBaseTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ClockBaseTB.AttributeID = 9;
            this.ClockBaseTB.Location = new System.Drawing.Point(102, 257);
            this.ClockBaseTB.Name = "ClockBaseTB";
            this.ClockBaseTB.Size = new System.Drawing.Size(208, 20);
            this.ClockBaseTB.TabIndex = 7;
            this.ClockBaseTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            this.ClockBaseTB.TextChanged += new System.EventHandler(this.ClockBaseTB_TextChanged);
            // 
            // ClockBaseLbl
            // 
            this.ClockBaseLbl.AutoSize = true;
            this.ClockBaseLbl.Location = new System.Drawing.Point(6, 260);
            this.ClockBaseLbl.Name = "ClockBaseLbl";
            this.ClockBaseLbl.Size = new System.Drawing.Size(64, 13);
            this.ClockBaseLbl.TabIndex = 22;
            this.ClockBaseLbl.Text = "Clock Base:";
            this.ClockBaseLbl.Click += new System.EventHandler(this.ClockBaseLbl_Click);
            // 
            // StatusTB
            // 
            this.StatusTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusTB.AttributeID = 4;
            this.StatusTB.Location = new System.Drawing.Point(102, 99);
            this.StatusTB.Name = "StatusTB";
            this.StatusTB.ReadOnly = true;
            this.StatusTB.Size = new System.Drawing.Size(208, 20);
            this.StatusTB.TabIndex = 2;
            this.StatusTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // StatusLbl
            // 
            this.StatusLbl.AutoSize = true;
            this.StatusLbl.Location = new System.Drawing.Point(6, 102);
            this.StatusLbl.Name = "StatusLbl";
            this.StatusLbl.Size = new System.Drawing.Size(40, 13);
            this.StatusLbl.TabIndex = 6;
            this.StatusLbl.Text = "Status:";
            // 
            // TimeZoneTB
            // 
            this.TimeZoneTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TimeZoneTB.AttributeID = 3;
            this.TimeZoneTB.Location = new System.Drawing.Point(102, 73);
            this.TimeZoneTB.Name = "TimeZoneTB";
            this.TimeZoneTB.Size = new System.Drawing.Size(208, 20);
            this.TimeZoneTB.TabIndex = 1;
            this.TimeZoneTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // TimeZoneLbl
            // 
            this.TimeZoneLbl.AutoSize = true;
            this.TimeZoneLbl.Location = new System.Drawing.Point(6, 76);
            this.TimeZoneLbl.Name = "TimeZoneLbl";
            this.TimeZoneLbl.Size = new System.Drawing.Size(61, 13);
            this.TimeZoneLbl.TabIndex = 4;
            this.TimeZoneLbl.Text = "Time Zone:";
            // 
            // TimeTB
            // 
            this.TimeTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TimeTB.AttributeID = 2;
            this.TimeTB.Location = new System.Drawing.Point(102, 47);
            this.TimeTB.Name = "TimeTB";
            this.TimeTB.Size = new System.Drawing.Size(208, 20);
            this.TimeTB.TabIndex = 0;
            this.TimeTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // TimeLbl
            // 
            this.TimeLbl.AutoSize = true;
            this.TimeLbl.Location = new System.Drawing.Point(6, 50);
            this.TimeLbl.Name = "TimeLbl";
            this.TimeLbl.Size = new System.Drawing.Size(33, 13);
            this.TimeLbl.TabIndex = 2;
            this.TimeLbl.Text = "Time:";
            // 
            // LogicalNameTB
            // 
            this.LogicalNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LogicalNameTB.AttributeID = 1;
            this.LogicalNameTB.Location = new System.Drawing.Point(102, 21);
            this.LogicalNameTB.Name = "LogicalNameTB";
            this.LogicalNameTB.ReadOnly = true;
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
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.EndTB);
            this.groupBox2.Controls.Add(this.EnabledLbl);
            this.groupBox2.Controls.Add(this.EnabledCB);
            this.groupBox2.Controls.Add(this.DeviationTB);
            this.groupBox2.Controls.Add(this.DeviationLbl);
            this.groupBox2.Controls.Add(this.EndLbl);
            this.groupBox2.Controls.Add(this.BeginTB);
            this.groupBox2.Controls.Add(this.BeginLbl);
            this.groupBox2.Location = new System.Drawing.Point(12, 137);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(333, 126);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Daylight Savings";
            // 
            // EndTB
            // 
            this.EndTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.EndTB.AttributeID = 0;
            this.EndTB.Location = new System.Drawing.Point(102, 68);
            this.EndTB.Name = "EndTB";
            this.EndTB.Size = new System.Drawing.Size(208, 20);
            this.EndTB.TabIndex = 5;
            this.EndTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // EnabledLbl
            // 
            this.EnabledLbl.AutoSize = true;
            this.EnabledLbl.Location = new System.Drawing.Point(6, 21);
            this.EnabledLbl.Name = "EnabledLbl";
            this.EnabledLbl.Size = new System.Drawing.Size(49, 13);
            this.EnabledLbl.TabIndex = 22;
            this.EnabledLbl.Text = "Enabled:";
            // 
            // EnabledCB
            // 
            this.EnabledCB.Location = new System.Drawing.Point(102, 19);
            this.EnabledCB.Name = "EnabledCB";
            this.EnabledCB.Size = new System.Drawing.Size(37, 17);
            this.EnabledCB.TabIndex = 3;
            this.EnabledCB.UseVisualStyleBackColor = false;
            this.EnabledCB.CheckedChanged += new System.EventHandler(this.EnabledCB_CheckedChanged);
            // 
            // DeviationTB
            // 
            this.DeviationTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DeviationTB.AttributeID = 0;
            this.DeviationTB.Location = new System.Drawing.Point(102, 94);
            this.DeviationTB.Name = "DeviationTB";
            this.DeviationTB.Size = new System.Drawing.Size(208, 20);
            this.DeviationTB.TabIndex = 6;
            this.DeviationTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // DeviationLbl
            // 
            this.DeviationLbl.AutoSize = true;
            this.DeviationLbl.Location = new System.Drawing.Point(6, 97);
            this.DeviationLbl.Name = "DeviationLbl";
            this.DeviationLbl.Size = new System.Drawing.Size(55, 13);
            this.DeviationLbl.TabIndex = 20;
            this.DeviationLbl.Text = "Deviation:";
            // 
            // EndLbl
            // 
            this.EndLbl.AutoSize = true;
            this.EndLbl.Location = new System.Drawing.Point(6, 71);
            this.EndLbl.Name = "EndLbl";
            this.EndLbl.Size = new System.Drawing.Size(29, 13);
            this.EndLbl.TabIndex = 18;
            this.EndLbl.Text = "End:";
            // 
            // BeginTB
            // 
            this.BeginTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.BeginTB.AttributeID = 0;
            this.BeginTB.Location = new System.Drawing.Point(102, 42);
            this.BeginTB.Name = "BeginTB";
            this.BeginTB.Size = new System.Drawing.Size(208, 20);
            this.BeginTB.TabIndex = 4;
            this.BeginTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // BeginLbl
            // 
            this.BeginLbl.AutoSize = true;
            this.BeginLbl.Location = new System.Drawing.Point(6, 45);
            this.BeginLbl.Name = "BeginLbl";
            this.BeginLbl.Size = new System.Drawing.Size(37, 13);
            this.BeginLbl.TabIndex = 16;
            this.BeginLbl.Text = "Begin:";
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.Icon = ((System.Drawing.Icon)(resources.GetObject("errorProvider1.Icon")));
            // 
            // GXDLMSClockView
            // 
            this.ClientSize = new System.Drawing.Size(357, 320);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "GXDLMSClockView";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        private void ClockBaseLbl_Click(object sender, EventArgs e)
        {

        }

        private void ClockBaseTB_TextChanged(object sender, EventArgs e)
        {

        }

        private void EnabledCB_CheckedChanged(object sender, EventArgs e)
        {
            BeginTB.ReadOnly = EndTB.ReadOnly = DeviationTB.ReadOnly = !EnabledCB.Checked;
        }

    }
}
