//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/Views/GXDLMSDemandRegisterView.cs $
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
using Gurux.DLMS;
using Gurux.DLMS.Objects;
using Gurux.DLMS.Enums;

namespace GXDLMSDirector.Views
{
    [GXDLMSViewAttribute(typeof(GXDLMSDemandRegister))]
    class GXDLMSDemandRegisterView : Form, IGXDLMSView
    {
        private GroupBox groupBox1;
        private GXValueField CAValueTB;
        private Label ValueLbl;
        private GXValueField LogicalNameTB;
        private GXValueField UnitTB;
        private Label UnitLbl;
        private GXValueField ScalerTB;
        private Label ScalerLbl;
        private GXValueField CurrentStartTimeTB;
        private Label CurrentStartTimeLbl;
        private GXValueField StatusTB;
        private Label StatusLbl;
        private GXValueField NOPeriodTB;
        private Label NoPeriodLbl;
        private GXValueField PeriodTB;
        private Label PeriodLbl;
        private GXValueField LAValueTB;
        private Label LAValueLbl;
        private GXValueField CaptureTimeTB;
        private Label CaptureTimeLbl;
        private ErrorProvider errorProvider1;
        private System.ComponentModel.IContainer components;
        private Label LogicalNameLbl;
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSDemandRegisterView()
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
            if (attributeID == 4)
            {
                this.ScalerTB.Value = ((GXDLMSDemandRegister)Target).Scaler.ToString();
                this.UnitTB.Value = ((GXDLMSDemandRegister)Target).Unit;
            }
            else
            {
                throw new IndexOutOfRangeException("attributeID");
            }        
        }

        public void OnAccessRightsChange(int attributeID, AccessMode access)
        {
            if (attributeID == 4)
            {
                this.UnitTB.ReadOnly = this.ScalerTB.ReadOnly = true;
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
                    errorProvider1.SetError(CAValueTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);                        
                break;
                case 3:
                    errorProvider1.SetError(LAValueTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                break;
                case 5:
                    errorProvider1.SetError(StatusTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                break;
                case 6:
                    errorProvider1.SetError(CaptureTimeTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                break;
                case 7:
                    errorProvider1.SetError(CurrentStartTimeTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                break;
                case 8:
                    errorProvider1.SetError(PeriodTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                break;
                case 9:
                    errorProvider1.SetError(NOPeriodTB, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDLMSDemandRegisterView));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CaptureTimeTB = new GXDLMSDirector.Views.GXValueField();
            this.CaptureTimeLbl = new System.Windows.Forms.Label();
            this.NOPeriodTB = new GXDLMSDirector.Views.GXValueField();
            this.NoPeriodLbl = new System.Windows.Forms.Label();
            this.PeriodTB = new GXDLMSDirector.Views.GXValueField();
            this.PeriodLbl = new System.Windows.Forms.Label();
            this.LAValueTB = new GXDLMSDirector.Views.GXValueField();
            this.LAValueLbl = new System.Windows.Forms.Label();
            this.CurrentStartTimeTB = new GXDLMSDirector.Views.GXValueField();
            this.CurrentStartTimeLbl = new System.Windows.Forms.Label();
            this.StatusTB = new GXDLMSDirector.Views.GXValueField();
            this.StatusLbl = new System.Windows.Forms.Label();
            this.UnitTB = new GXDLMSDirector.Views.GXValueField();
            this.UnitLbl = new System.Windows.Forms.Label();
            this.ScalerTB = new GXDLMSDirector.Views.GXValueField();
            this.ScalerLbl = new System.Windows.Forms.Label();
            this.CAValueTB = new GXDLMSDirector.Views.GXValueField();
            this.ValueLbl = new System.Windows.Forms.Label();
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
            this.groupBox1.Controls.Add(this.CaptureTimeTB);
            this.groupBox1.Controls.Add(this.CaptureTimeLbl);
            this.groupBox1.Controls.Add(this.NOPeriodTB);
            this.groupBox1.Controls.Add(this.NoPeriodLbl);
            this.groupBox1.Controls.Add(this.PeriodTB);
            this.groupBox1.Controls.Add(this.PeriodLbl);
            this.groupBox1.Controls.Add(this.LAValueTB);
            this.groupBox1.Controls.Add(this.LAValueLbl);
            this.groupBox1.Controls.Add(this.CurrentStartTimeTB);
            this.groupBox1.Controls.Add(this.CurrentStartTimeLbl);
            this.groupBox1.Controls.Add(this.StatusTB);
            this.groupBox1.Controls.Add(this.StatusLbl);
            this.groupBox1.Controls.Add(this.UnitTB);
            this.groupBox1.Controls.Add(this.UnitLbl);
            this.groupBox1.Controls.Add(this.ScalerTB);
            this.groupBox1.Controls.Add(this.ScalerLbl);
            this.groupBox1.Controls.Add(this.CAValueTB);
            this.groupBox1.Controls.Add(this.ValueLbl);
            this.groupBox1.Controls.Add(this.LogicalNameTB);
            this.groupBox1.Controls.Add(this.LogicalNameLbl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(333, 286);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Demand Register Object";
            // 
            // CaptureTimeTB
            // 
            this.CaptureTimeTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CaptureTimeTB.AttributeID = 6;
            this.CaptureTimeTB.Location = new System.Drawing.Point(102, 178);
            this.CaptureTimeTB.Name = "CaptureTimeTB";
            this.CaptureTimeTB.Size = new System.Drawing.Size(208, 20);
            this.CaptureTimeTB.TabIndex = 5;
            this.CaptureTimeTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // CaptureTimeLbl
            // 
            this.CaptureTimeLbl.AutoSize = true;
            this.CaptureTimeLbl.Location = new System.Drawing.Point(6, 181);
            this.CaptureTimeLbl.Name = "CaptureTimeLbl";
            this.CaptureTimeLbl.Size = new System.Drawing.Size(73, 13);
            this.CaptureTimeLbl.TabIndex = 20;
            this.CaptureTimeLbl.Text = "Capture Time:";
            // 
            // NOPeriodTB
            // 
            this.NOPeriodTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.NOPeriodTB.AttributeID = 9;
            this.NOPeriodTB.Location = new System.Drawing.Point(102, 256);
            this.NOPeriodTB.Name = "NOPeriodTB";
            this.NOPeriodTB.Size = new System.Drawing.Size(208, 20);
            this.NOPeriodTB.TabIndex = 8;
            this.NOPeriodTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // NoPeriodLbl
            // 
            this.NoPeriodLbl.AutoSize = true;
            this.NoPeriodLbl.Location = new System.Drawing.Point(6, 259);
            this.NoPeriodLbl.Name = "NoPeriodLbl";
            this.NoPeriodLbl.Size = new System.Drawing.Size(97, 13);
            this.NoPeriodLbl.TabIndex = 18;
            this.NoPeriodLbl.Text = "Number of Periods:";
            // 
            // PeriodTB
            // 
            this.PeriodTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PeriodTB.AttributeID = 8;
            this.PeriodTB.Location = new System.Drawing.Point(102, 230);
            this.PeriodTB.Name = "PeriodTB";
            this.PeriodTB.Size = new System.Drawing.Size(208, 20);
            this.PeriodTB.TabIndex = 7;
            this.PeriodTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // PeriodLbl
            // 
            this.PeriodLbl.AutoSize = true;
            this.PeriodLbl.Location = new System.Drawing.Point(6, 233);
            this.PeriodLbl.Name = "PeriodLbl";
            this.PeriodLbl.Size = new System.Drawing.Size(40, 13);
            this.PeriodLbl.TabIndex = 16;
            this.PeriodLbl.Text = "Period:";
            // 
            // LAValueTB
            // 
            this.LAValueTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LAValueTB.AttributeID = 3;
            this.LAValueTB.Location = new System.Drawing.Point(102, 73);
            this.LAValueTB.Name = "LAValueTB";
            this.LAValueTB.Size = new System.Drawing.Size(208, 20);
            this.LAValueTB.TabIndex = 1;
            this.LAValueTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // LAValueLbl
            // 
            this.LAValueLbl.AutoSize = true;
            this.LAValueLbl.Location = new System.Drawing.Point(6, 76);
            this.LAValueLbl.Name = "LAValueLbl";
            this.LAValueLbl.Size = new System.Drawing.Size(73, 13);
            this.LAValueLbl.TabIndex = 12;
            this.LAValueLbl.Text = "Last Average:";
            // 
            // CurrentStartTimeTB
            // 
            this.CurrentStartTimeTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CurrentStartTimeTB.AttributeID = 7;
            this.CurrentStartTimeTB.Location = new System.Drawing.Point(102, 204);
            this.CurrentStartTimeTB.Name = "CurrentStartTimeTB";
            this.CurrentStartTimeTB.Size = new System.Drawing.Size(208, 20);
            this.CurrentStartTimeTB.TabIndex = 6;
            this.CurrentStartTimeTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // CurrentStartTimeLbl
            // 
            this.CurrentStartTimeLbl.AutoSize = true;
            this.CurrentStartTimeLbl.Location = new System.Drawing.Point(6, 207);
            this.CurrentStartTimeLbl.Name = "CurrentStartTimeLbl";
            this.CurrentStartTimeLbl.Size = new System.Drawing.Size(95, 13);
            this.CurrentStartTimeLbl.TabIndex = 10;
            this.CurrentStartTimeLbl.Text = "Current Start Time:";
            // 
            // StatusTB
            // 
            this.StatusTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusTB.AttributeID = 5;
            this.StatusTB.Location = new System.Drawing.Point(102, 152);
            this.StatusTB.Name = "StatusTB";
            this.StatusTB.Size = new System.Drawing.Size(208, 20);
            this.StatusTB.TabIndex = 4;
            this.StatusTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // StatusLbl
            // 
            this.StatusLbl.AutoSize = true;
            this.StatusLbl.Location = new System.Drawing.Point(6, 155);
            this.StatusLbl.Name = "StatusLbl";
            this.StatusLbl.Size = new System.Drawing.Size(40, 13);
            this.StatusLbl.TabIndex = 8;
            this.StatusLbl.Text = "Status:";
            // 
            // UnitTB
            // 
            this.UnitTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.UnitTB.AttributeID = 0;
            this.UnitTB.Location = new System.Drawing.Point(102, 126);
            this.UnitTB.Name = "UnitTB";
            this.UnitTB.Size = new System.Drawing.Size(208, 20);
            this.UnitTB.TabIndex = 3;
            this.UnitTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // UnitLbl
            // 
            this.UnitLbl.AutoSize = true;
            this.UnitLbl.Location = new System.Drawing.Point(6, 129);
            this.UnitLbl.Name = "UnitLbl";
            this.UnitLbl.Size = new System.Drawing.Size(29, 13);
            this.UnitLbl.TabIndex = 6;
            this.UnitLbl.Text = "Unit:";
            // 
            // ScalerTB
            // 
            this.ScalerTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ScalerTB.AttributeID = 0;
            this.ScalerTB.Location = new System.Drawing.Point(102, 100);
            this.ScalerTB.Name = "ScalerTB";
            this.ScalerTB.Size = new System.Drawing.Size(208, 20);
            this.ScalerTB.TabIndex = 2;
            this.ScalerTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // ScalerLbl
            // 
            this.ScalerLbl.AutoSize = true;
            this.ScalerLbl.Location = new System.Drawing.Point(6, 103);
            this.ScalerLbl.Name = "ScalerLbl";
            this.ScalerLbl.Size = new System.Drawing.Size(40, 13);
            this.ScalerLbl.TabIndex = 4;
            this.ScalerLbl.Text = "Scaler:";
            // 
            // CAValueTB
            // 
            this.CAValueTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CAValueTB.AttributeID = 2;
            this.CAValueTB.Location = new System.Drawing.Point(102, 47);
            this.CAValueTB.Name = "CAValueTB";
            this.CAValueTB.Size = new System.Drawing.Size(208, 20);
            this.CAValueTB.TabIndex = 0;
            this.CAValueTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // ValueLbl
            // 
            this.ValueLbl.AutoSize = true;
            this.ValueLbl.Location = new System.Drawing.Point(6, 50);
            this.ValueLbl.Name = "ValueLbl";
            this.ValueLbl.Size = new System.Drawing.Size(87, 13);
            this.ValueLbl.TabIndex = 2;
            this.ValueLbl.Text = "Current Average:";
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
            // GXDLMSDemandRegisterView
            // 
            this.ClientSize = new System.Drawing.Size(357, 337);
            this.Controls.Add(this.groupBox1);
            this.Name = "GXDLMSDemandRegisterView";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

    }
}
