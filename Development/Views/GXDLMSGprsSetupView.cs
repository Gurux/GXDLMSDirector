//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GuruxClub/GXDLMSDirector/Development/Views/GXDLMSGprsSetupView.cs $
//
// Version:         $Revision: 6959 $,
//                  $Date: 2014-02-03 09:52:28 +0200 (ma, 03 helmi 2014) $
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
    [GXDLMSViewAttribute(typeof(GXDLMSGprsSetup))]
    class GXDLMSGprsSetupView : Form, IGXDLMSView
    {
        private GroupBox groupBox1;
        private Label APNLbl;
        private GXValueField LogicalNameTB;
        private GXValueField PINCodeTB;
        private Label PINCodeLbl;
        private GXValueField APNTB;
        private GroupBox groupBox2;
        private TextBox CPeakThroughputTB;
        private Label CPeakThroughputLbl;
        private TextBox CReliabilityTB;
        private Label CReliabilityLbl;
        private TextBox CDelayTB;
        private Label CDelayLbl;
        private TextBox CPrecedenceTB;
        private Label CPrecedenceLbl;
        private TextBox CMeanThroughputTB;
        private Label CMeanThroughputLbl;
        private GroupBox groupBox3;
        private TextBox MMeanThroughputTB;
        private Label MMeanThroughputLbl;
        private TextBox MPeakThroughputTB;
        private Label MPeakThroughputLbl;
        private TextBox MReliabilityTB;
        private Label MReliabilityLbl;
        private TextBox MDelayTB;
        private Label MDelayLbl;
        private TextBox MPrecedenceTB;
        private Label MPrecedenceLbl;
        private ErrorProvider errorProvider1;
        private System.ComponentModel.IContainer components;
        private Label LogicalNameLbl;
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSGprsSetupView()
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
            GXDLMSGprsSetup target = Target as GXDLMSGprsSetup;
            if (attributeID == 4)
            {
                CPrecedenceTB.Text = target.DefaultQualityOfService.Precedence.ToString();
                CDelayTB.Text = target.DefaultQualityOfService.Delay.ToString();
                CReliabilityTB.Text = target.DefaultQualityOfService.Reliability.ToString();
                CPeakThroughputTB.Text = target.DefaultQualityOfService.PeakThroughput.ToString();
                CMeanThroughputTB.Text = target.DefaultQualityOfService.MeanThroughput.ToString();
                MPrecedenceTB.Text = target.RequestedQualityOfService.Precedence.ToString();
                MDelayTB.Text = target.RequestedQualityOfService.Delay.ToString();
                MReliabilityTB.Text = target.RequestedQualityOfService.Reliability.ToString();
                MPeakThroughputTB.Text = target.RequestedQualityOfService.PeakThroughput.ToString();
                MMeanThroughputTB.Text = target.RequestedQualityOfService.MeanThroughput.ToString();
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
                CPrecedenceTB.ReadOnly = !(access == AccessMode.Write || access == AccessMode.ReadWrite);
                CReliabilityTB.ReadOnly = CDelayTB.ReadOnly = CPrecedenceTB.ReadOnly;                
                CPeakThroughputTB.ReadOnly = CMeanThroughputTB.ReadOnly = CPrecedenceTB.ReadOnly;
                MPrecedenceTB.ReadOnly = MDelayTB.ReadOnly = MReliabilityTB.ReadOnly = CPrecedenceTB.ReadOnly;
                MPeakThroughputTB.ReadOnly = MMeanThroughputTB.ReadOnly = CPrecedenceTB.ReadOnly;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDLMSGprsSetupView));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.MMeanThroughputTB = new System.Windows.Forms.TextBox();
            this.MMeanThroughputLbl = new System.Windows.Forms.Label();
            this.MPeakThroughputTB = new System.Windows.Forms.TextBox();
            this.MPeakThroughputLbl = new System.Windows.Forms.Label();
            this.MReliabilityTB = new System.Windows.Forms.TextBox();
            this.MReliabilityLbl = new System.Windows.Forms.Label();
            this.MDelayTB = new System.Windows.Forms.TextBox();
            this.MDelayLbl = new System.Windows.Forms.Label();
            this.MPrecedenceTB = new System.Windows.Forms.TextBox();
            this.MPrecedenceLbl = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.CMeanThroughputTB = new System.Windows.Forms.TextBox();
            this.CMeanThroughputLbl = new System.Windows.Forms.Label();
            this.CPeakThroughputTB = new System.Windows.Forms.TextBox();
            this.CPeakThroughputLbl = new System.Windows.Forms.Label();
            this.CReliabilityTB = new System.Windows.Forms.TextBox();
            this.CReliabilityLbl = new System.Windows.Forms.Label();
            this.CDelayTB = new System.Windows.Forms.TextBox();
            this.CDelayLbl = new System.Windows.Forms.Label();
            this.CPrecedenceTB = new System.Windows.Forms.TextBox();
            this.CPrecedenceLbl = new System.Windows.Forms.Label();
            this.PINCodeTB = new GXDLMSDirector.Views.GXValueField();
            this.PINCodeLbl = new System.Windows.Forms.Label();
            this.APNTB = new GXDLMSDirector.Views.GXValueField();
            this.APNLbl = new System.Windows.Forms.Label();
            this.LogicalNameTB = new GXDLMSDirector.Views.GXValueField();
            this.LogicalNameLbl = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.PINCodeTB);
            this.groupBox1.Controls.Add(this.PINCodeLbl);
            this.groupBox1.Controls.Add(this.APNTB);
            this.groupBox1.Controls.Add(this.APNLbl);
            this.groupBox1.Controls.Add(this.LogicalNameTB);
            this.groupBox1.Controls.Add(this.LogicalNameLbl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 368);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GPRS Modem Setup Object";
            this.groupBox1.UseCompatibleTextRendering = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.MMeanThroughputTB);
            this.groupBox3.Controls.Add(this.MMeanThroughputLbl);
            this.groupBox3.Controls.Add(this.MPeakThroughputTB);
            this.groupBox3.Controls.Add(this.MPeakThroughputLbl);
            this.groupBox3.Controls.Add(this.MReliabilityTB);
            this.groupBox3.Controls.Add(this.MReliabilityLbl);
            this.groupBox3.Controls.Add(this.MDelayTB);
            this.groupBox3.Controls.Add(this.MDelayLbl);
            this.groupBox3.Controls.Add(this.MPrecedenceTB);
            this.groupBox3.Controls.Add(this.MPrecedenceLbl);
            this.groupBox3.Location = new System.Drawing.Point(0, 227);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(296, 142);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Quality of Service";
            // 
            // MMeanThroughputTB
            // 
            this.MMeanThroughputTB.Location = new System.Drawing.Point(105, 117);
            this.MMeanThroughputTB.Name = "MMeanThroughputTB";
            this.MMeanThroughputTB.Size = new System.Drawing.Size(168, 20);
            this.MMeanThroughputTB.TabIndex = 16;
            // 
            // MMeanThroughputLbl
            // 
            this.MMeanThroughputLbl.AutoSize = true;
            this.MMeanThroughputLbl.Location = new System.Drawing.Point(6, 120);
            this.MMeanThroughputLbl.Name = "MMeanThroughputLbl";
            this.MMeanThroughputLbl.Size = new System.Drawing.Size(95, 13);
            this.MMeanThroughputLbl.TabIndex = 15;
            this.MMeanThroughputLbl.Text = "Mean Throughput:";
            // 
            // MPeakThroughputTB
            // 
            this.MPeakThroughputTB.Location = new System.Drawing.Point(105, 91);
            this.MPeakThroughputTB.Name = "MPeakThroughputTB";
            this.MPeakThroughputTB.Size = new System.Drawing.Size(168, 20);
            this.MPeakThroughputTB.TabIndex = 14;
            // 
            // MPeakThroughputLbl
            // 
            this.MPeakThroughputLbl.AutoSize = true;
            this.MPeakThroughputLbl.Location = new System.Drawing.Point(6, 94);
            this.MPeakThroughputLbl.Name = "MPeakThroughputLbl";
            this.MPeakThroughputLbl.Size = new System.Drawing.Size(93, 13);
            this.MPeakThroughputLbl.TabIndex = 13;
            this.MPeakThroughputLbl.Text = "Peak Throughput:";
            // 
            // MReliabilityTB
            // 
            this.MReliabilityTB.Location = new System.Drawing.Point(105, 65);
            this.MReliabilityTB.Name = "MReliabilityTB";
            this.MReliabilityTB.Size = new System.Drawing.Size(168, 20);
            this.MReliabilityTB.TabIndex = 12;
            // 
            // MReliabilityLbl
            // 
            this.MReliabilityLbl.AutoSize = true;
            this.MReliabilityLbl.Location = new System.Drawing.Point(6, 68);
            this.MReliabilityLbl.Name = "MReliabilityLbl";
            this.MReliabilityLbl.Size = new System.Drawing.Size(54, 13);
            this.MReliabilityLbl.TabIndex = 11;
            this.MReliabilityLbl.Text = "Reliability:";
            // 
            // MDelayTB
            // 
            this.MDelayTB.Location = new System.Drawing.Point(105, 39);
            this.MDelayTB.Name = "MDelayTB";
            this.MDelayTB.Size = new System.Drawing.Size(168, 20);
            this.MDelayTB.TabIndex = 10;
            // 
            // MDelayLbl
            // 
            this.MDelayLbl.AutoSize = true;
            this.MDelayLbl.Location = new System.Drawing.Point(6, 42);
            this.MDelayLbl.Name = "MDelayLbl";
            this.MDelayLbl.Size = new System.Drawing.Size(37, 13);
            this.MDelayLbl.TabIndex = 9;
            this.MDelayLbl.Text = "Delay:";
            // 
            // MPrecedenceTB
            // 
            this.MPrecedenceTB.Location = new System.Drawing.Point(105, 13);
            this.MPrecedenceTB.Name = "MPrecedenceTB";
            this.MPrecedenceTB.Size = new System.Drawing.Size(168, 20);
            this.MPrecedenceTB.TabIndex = 8;
            // 
            // MPrecedenceLbl
            // 
            this.MPrecedenceLbl.AutoSize = true;
            this.MPrecedenceLbl.Location = new System.Drawing.Point(6, 16);
            this.MPrecedenceLbl.Name = "MPrecedenceLbl";
            this.MPrecedenceLbl.Size = new System.Drawing.Size(68, 13);
            this.MPrecedenceLbl.TabIndex = 7;
            this.MPrecedenceLbl.Text = "Precedence:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CMeanThroughputTB);
            this.groupBox2.Controls.Add(this.CMeanThroughputLbl);
            this.groupBox2.Controls.Add(this.CPeakThroughputTB);
            this.groupBox2.Controls.Add(this.CPeakThroughputLbl);
            this.groupBox2.Controls.Add(this.CReliabilityTB);
            this.groupBox2.Controls.Add(this.CReliabilityLbl);
            this.groupBox2.Controls.Add(this.CDelayTB);
            this.groupBox2.Controls.Add(this.CDelayLbl);
            this.groupBox2.Controls.Add(this.CPrecedenceTB);
            this.groupBox2.Controls.Add(this.CPrecedenceLbl);
            this.groupBox2.Location = new System.Drawing.Point(0, 93);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(296, 142);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Quality of Service";
            // 
            // CMeanThroughputTB
            // 
            this.CMeanThroughputTB.Location = new System.Drawing.Point(105, 117);
            this.CMeanThroughputTB.Name = "CMeanThroughputTB";
            this.CMeanThroughputTB.Size = new System.Drawing.Size(168, 20);
            this.CMeanThroughputTB.TabIndex = 16;
            // 
            // CMeanThroughputLbl
            // 
            this.CMeanThroughputLbl.AutoSize = true;
            this.CMeanThroughputLbl.Location = new System.Drawing.Point(6, 120);
            this.CMeanThroughputLbl.Name = "CMeanThroughputLbl";
            this.CMeanThroughputLbl.Size = new System.Drawing.Size(95, 13);
            this.CMeanThroughputLbl.TabIndex = 15;
            this.CMeanThroughputLbl.Text = "Mean Throughput:";
            // 
            // CPeakThroughputTB
            // 
            this.CPeakThroughputTB.Location = new System.Drawing.Point(105, 91);
            this.CPeakThroughputTB.Name = "CPeakThroughputTB";
            this.CPeakThroughputTB.Size = new System.Drawing.Size(168, 20);
            this.CPeakThroughputTB.TabIndex = 14;
            // 
            // CPeakThroughputLbl
            // 
            this.CPeakThroughputLbl.AutoSize = true;
            this.CPeakThroughputLbl.Location = new System.Drawing.Point(6, 94);
            this.CPeakThroughputLbl.Name = "CPeakThroughputLbl";
            this.CPeakThroughputLbl.Size = new System.Drawing.Size(93, 13);
            this.CPeakThroughputLbl.TabIndex = 13;
            this.CPeakThroughputLbl.Text = "Peak Throughput:";
            // 
            // CReliabilityTB
            // 
            this.CReliabilityTB.Location = new System.Drawing.Point(105, 65);
            this.CReliabilityTB.Name = "CReliabilityTB";
            this.CReliabilityTB.Size = new System.Drawing.Size(168, 20);
            this.CReliabilityTB.TabIndex = 12;
            // 
            // CReliabilityLbl
            // 
            this.CReliabilityLbl.AutoSize = true;
            this.CReliabilityLbl.Location = new System.Drawing.Point(6, 68);
            this.CReliabilityLbl.Name = "CReliabilityLbl";
            this.CReliabilityLbl.Size = new System.Drawing.Size(54, 13);
            this.CReliabilityLbl.TabIndex = 11;
            this.CReliabilityLbl.Text = "Reliability:";
            // 
            // CDelayTB
            // 
            this.CDelayTB.Location = new System.Drawing.Point(105, 39);
            this.CDelayTB.Name = "CDelayTB";
            this.CDelayTB.Size = new System.Drawing.Size(168, 20);
            this.CDelayTB.TabIndex = 10;
            // 
            // CDelayLbl
            // 
            this.CDelayLbl.AutoSize = true;
            this.CDelayLbl.Location = new System.Drawing.Point(6, 42);
            this.CDelayLbl.Name = "CDelayLbl";
            this.CDelayLbl.Size = new System.Drawing.Size(37, 13);
            this.CDelayLbl.TabIndex = 9;
            this.CDelayLbl.Text = "Delay:";
            // 
            // CPrecedenceTB
            // 
            this.CPrecedenceTB.Location = new System.Drawing.Point(105, 13);
            this.CPrecedenceTB.Name = "CPrecedenceTB";
            this.CPrecedenceTB.Size = new System.Drawing.Size(168, 20);
            this.CPrecedenceTB.TabIndex = 8;
            // 
            // CPrecedenceLbl
            // 
            this.CPrecedenceLbl.AutoSize = true;
            this.CPrecedenceLbl.Location = new System.Drawing.Point(6, 16);
            this.CPrecedenceLbl.Name = "CPrecedenceLbl";
            this.CPrecedenceLbl.Size = new System.Drawing.Size(68, 13);
            this.CPrecedenceLbl.TabIndex = 7;
            this.CPrecedenceLbl.Text = "Precedence:";
            // 
            // PINCodeTB
            // 
            this.PINCodeTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PINCodeTB.AttributeID = 3;
            this.PINCodeTB.Location = new System.Drawing.Point(102, 73);
            this.PINCodeTB.Name = "PINCodeTB";
            this.PINCodeTB.Size = new System.Drawing.Size(171, 20);
            this.PINCodeTB.TabIndex = 3;
            this.PINCodeTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // PINCodeLbl
            // 
            this.PINCodeLbl.AutoSize = true;
            this.PINCodeLbl.Location = new System.Drawing.Point(6, 76);
            this.PINCodeLbl.Name = "PINCodeLbl";
            this.PINCodeLbl.Size = new System.Drawing.Size(56, 13);
            this.PINCodeLbl.TabIndex = 4;
            this.PINCodeLbl.Text = "PIN Code:";
            // 
            // APNTB
            // 
            this.APNTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.APNTB.AttributeID = 2;
            this.APNTB.Location = new System.Drawing.Point(102, 47);
            this.APNTB.Name = "APNTB";
            this.APNTB.Size = new System.Drawing.Size(171, 20);
            this.APNTB.TabIndex = 2;
            this.APNTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // APNLbl
            // 
            this.APNLbl.AutoSize = true;
            this.APNLbl.Location = new System.Drawing.Point(6, 50);
            this.APNLbl.Name = "APNLbl";
            this.APNLbl.Size = new System.Drawing.Size(32, 13);
            this.APNLbl.TabIndex = 2;
            this.APNLbl.Text = "APN:";
            // 
            // LogicalNameTB
            // 
            this.LogicalNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LogicalNameTB.AttributeID = 1;
            this.LogicalNameTB.Location = new System.Drawing.Point(102, 21);
            this.LogicalNameTB.Name = "LogicalNameTB";
            this.LogicalNameTB.Size = new System.Drawing.Size(171, 20);
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
            // GXDLMSGprsSetupView
            // 
            this.ClientSize = new System.Drawing.Size(320, 387);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GXDLMSGprsSetupView";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
