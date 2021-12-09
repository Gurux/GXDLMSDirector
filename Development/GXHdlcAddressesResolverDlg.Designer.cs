//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Filename:        $HeadURL$
//
// Version:         $Revision$,
//                  $Date$
//                  $Author$
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
// More information of Gurux products: https://www.gurux.org
//
// This code is licensed under the GNU General Public License v2.
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------

namespace GXDLMSDirector
{
    partial class GXHdlcAddressesResolverDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXHdlcAddressesResolverDlg));
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.CustomSettings = new System.Windows.Forms.Panel();
            this.BaudRatesPanel = new System.Windows.Forms.Panel();
            this.ConnectionDelayLbl = new System.Windows.Forms.Label();
            this.ConnectionDelayTb = new System.Windows.Forms.TextBox();
            this.BaudRatesCl = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ClientAddressesTb = new System.Windows.Forms.TextBox();
            this.ClientAddressesLbl = new System.Windows.Forms.Label();
            this.ServerAddressesTb = new System.Windows.Forms.TextBox();
            this.ServerAddressesLbl = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TestFailedClientsLastCb = new System.Windows.Forms.CheckBox();
            this.TestFoundMetersFirstCb = new System.Windows.Forms.CheckBox();
            this.InitializeWaitTimeLbl = new System.Windows.Forms.Label();
            this.InitializeWaitTimeTb = new System.Windows.Forms.TextBox();
            this.SearchWaitTimeLbl = new System.Windows.Forms.Label();
            this.SearchWaitTimeTb = new System.Windows.Forms.TextBox();
            this.HexCb = new System.Windows.Forms.CheckBox();
            this.ServerGenerateAllBtn = new System.Windows.Forms.Button();
            this.ClientGenerateAllBtn = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.BaudRatesPanel.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Location = new System.Drawing.Point(407, 446);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 4;
            this.OKBtn.Text = "&OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(488, 446);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 5;
            this.CancelBtn.Text = "&Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(2, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(561, 437);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.CustomSettings);
            this.tabPage1.Controls.Add(this.BaudRatesPanel);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(553, 411);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Media settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // CustomSettings
            // 
            this.CustomSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CustomSettings.Location = new System.Drawing.Point(3, 3);
            this.CustomSettings.Name = "CustomSettings";
            this.CustomSettings.Size = new System.Drawing.Size(547, 194);
            this.CustomSettings.TabIndex = 2;
            // 
            // BaudRatesPanel
            // 
            this.BaudRatesPanel.Controls.Add(this.ConnectionDelayLbl);
            this.BaudRatesPanel.Controls.Add(this.ConnectionDelayTb);
            this.BaudRatesPanel.Controls.Add(this.BaudRatesCl);
            this.BaudRatesPanel.Controls.Add(this.label1);
            this.BaudRatesPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BaudRatesPanel.Location = new System.Drawing.Point(3, 197);
            this.BaudRatesPanel.Name = "BaudRatesPanel";
            this.BaudRatesPanel.Size = new System.Drawing.Size(547, 211);
            this.BaudRatesPanel.TabIndex = 1;
            // 
            // ConnectionDelayLbl
            // 
            this.ConnectionDelayLbl.AutoSize = true;
            this.ConnectionDelayLbl.Location = new System.Drawing.Point(132, 5);
            this.ConnectionDelayLbl.Name = "ConnectionDelayLbl";
            this.ConnectionDelayLbl.Size = new System.Drawing.Size(100, 13);
            this.ConnectionDelayLbl.TabIndex = 27;
            this.ConnectionDelayLbl.Text = "Connections delay :";
            // 
            // ConnectionDelayTb
            // 
            this.ConnectionDelayTb.Location = new System.Drawing.Point(251, 3);
            this.ConnectionDelayTb.Name = "ConnectionDelayTb";
            this.ConnectionDelayTb.Size = new System.Drawing.Size(66, 20);
            this.ConnectionDelayTb.TabIndex = 26;
            this.ConnectionDelayTb.Text = "5";
            // 
            // BaudRatesCl
            // 
            this.BaudRatesCl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BaudRatesCl.FormattingEnabled = true;
            this.BaudRatesCl.Location = new System.Drawing.Point(3, 25);
            this.BaudRatesCl.Name = "BaudRatesCl";
            this.BaudRatesCl.Size = new System.Drawing.Size(544, 184);
            this.BaudRatesCl.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Tested Baud rates";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.InitializeWaitTimeLbl);
            this.tabPage2.Controls.Add(this.InitializeWaitTimeTb);
            this.tabPage2.Controls.Add(this.SearchWaitTimeLbl);
            this.tabPage2.Controls.Add(this.SearchWaitTimeTb);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(553, 411);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Search settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ClientGenerateAllBtn);
            this.groupBox2.Controls.Add(this.ServerGenerateAllBtn);
            this.groupBox2.Controls.Add(this.ClientAddressesTb);
            this.groupBox2.Controls.Add(this.ClientAddressesLbl);
            this.groupBox2.Controls.Add(this.ServerAddressesTb);
            this.groupBox2.Controls.Add(this.ServerAddressesLbl);
            this.groupBox2.Location = new System.Drawing.Point(3, 89);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(535, 316);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Used addresses";
            // 
            // ClientAddressesTb
            // 
            this.ClientAddressesTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ClientAddressesTb.Location = new System.Drawing.Point(108, 201);
            this.ClientAddressesTb.Multiline = true;
            this.ClientAddressesTb.Name = "ClientAddressesTb";
            this.ClientAddressesTb.Size = new System.Drawing.Size(305, 109);
            this.ClientAddressesTb.TabIndex = 28;
            // 
            // ClientAddressesLbl
            // 
            this.ClientAddressesLbl.AutoSize = true;
            this.ClientAddressesLbl.Location = new System.Drawing.Point(8, 201);
            this.ClientAddressesLbl.Name = "ClientAddressesLbl";
            this.ClientAddressesLbl.Size = new System.Drawing.Size(87, 13);
            this.ClientAddressesLbl.TabIndex = 27;
            this.ClientAddressesLbl.Text = "Client addresses:";
            // 
            // ServerAddressesTb
            // 
            this.ServerAddressesTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ServerAddressesTb.Location = new System.Drawing.Point(108, 19);
            this.ServerAddressesTb.Multiline = true;
            this.ServerAddressesTb.Name = "ServerAddressesTb";
            this.ServerAddressesTb.Size = new System.Drawing.Size(305, 176);
            this.ServerAddressesTb.TabIndex = 26;
            // 
            // ServerAddressesLbl
            // 
            this.ServerAddressesLbl.AutoSize = true;
            this.ServerAddressesLbl.Location = new System.Drawing.Point(8, 19);
            this.ServerAddressesLbl.Name = "ServerAddressesLbl";
            this.ServerAddressesLbl.Size = new System.Drawing.Size(92, 13);
            this.ServerAddressesLbl.TabIndex = 25;
            this.ServerAddressesLbl.Text = "Server addresses:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TestFailedClientsLastCb);
            this.groupBox1.Controls.Add(this.TestFoundMetersFirstCb);
            this.groupBox1.Location = new System.Drawing.Point(3, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(535, 50);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Performance";
            // 
            // TestFailedClientsLastCb
            // 
            this.TestFailedClientsLastCb.AutoSize = true;
            this.TestFailedClientsLastCb.Location = new System.Drawing.Point(189, 19);
            this.TestFailedClientsLastCb.Name = "TestFailedClientsLastCb";
            this.TestFailedClientsLastCb.Size = new System.Drawing.Size(159, 17);
            this.TestFailedClientsLastCb.TabIndex = 28;
            this.TestFailedClientsLastCb.Text = "Failed clients are tested last.";
            this.TestFailedClientsLastCb.UseVisualStyleBackColor = true;
            // 
            // TestFoundMetersFirstCb
            // 
            this.TestFoundMetersFirstCb.AutoSize = true;
            this.TestFoundMetersFirstCb.Location = new System.Drawing.Point(6, 19);
            this.TestFoundMetersFirstCb.Name = "TestFoundMetersFirstCb";
            this.TestFoundMetersFirstCb.Size = new System.Drawing.Size(162, 17);
            this.TestFoundMetersFirstCb.TabIndex = 27;
            this.TestFoundMetersFirstCb.Text = "Found meters are tested first.";
            this.TestFoundMetersFirstCb.UseVisualStyleBackColor = true;
            // 
            // InitializeWaitTimeLbl
            // 
            this.InitializeWaitTimeLbl.AutoSize = true;
            this.InitializeWaitTimeLbl.Location = new System.Drawing.Point(214, 8);
            this.InitializeWaitTimeLbl.Name = "InitializeWaitTimeLbl";
            this.InitializeWaitTimeLbl.Size = new System.Drawing.Size(108, 13);
            this.InitializeWaitTimeLbl.TabIndex = 21;
            this.InitializeWaitTimeLbl.Text = "Initialize Wait time (s):";
            // 
            // InitializeWaitTimeTb
            // 
            this.InitializeWaitTimeTb.Location = new System.Drawing.Point(333, 7);
            this.InitializeWaitTimeTb.Name = "InitializeWaitTimeTb";
            this.InitializeWaitTimeTb.Size = new System.Drawing.Size(66, 20);
            this.InitializeWaitTimeTb.TabIndex = 20;
            this.InitializeWaitTimeTb.Text = "1";
            // 
            // SearchWaitTimeLbl
            // 
            this.SearchWaitTimeLbl.AutoSize = true;
            this.SearchWaitTimeLbl.Location = new System.Drawing.Point(8, 7);
            this.SearchWaitTimeLbl.Name = "SearchWaitTimeLbl";
            this.SearchWaitTimeLbl.Size = new System.Drawing.Size(113, 13);
            this.SearchWaitTimeLbl.TabIndex = 19;
            this.SearchWaitTimeLbl.Text = "Search Wait time (ms):";
            // 
            // SearchWaitTimeTb
            // 
            this.SearchWaitTimeTb.Location = new System.Drawing.Point(127, 6);
            this.SearchWaitTimeTb.Name = "SearchWaitTimeTb";
            this.SearchWaitTimeTb.Size = new System.Drawing.Size(66, 20);
            this.SearchWaitTimeTb.TabIndex = 18;
            this.SearchWaitTimeTb.Text = "100";
            // 
            // HexCb
            // 
            this.HexCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.HexCb.AutoSize = true;
            this.HexCb.Location = new System.Drawing.Point(12, 450);
            this.HexCb.Name = "HexCb";
            this.HexCb.Size = new System.Drawing.Size(45, 17);
            this.HexCb.TabIndex = 29;
            this.HexCb.Text = "Hex";
            this.HexCb.UseVisualStyleBackColor = true;
            this.HexCb.CheckedChanged += new System.EventHandler(this.HexCb_CheckedChanged);
            // 
            // ServerGenerateAllBtn
            // 
            this.ServerGenerateAllBtn.Location = new System.Drawing.Point(419, 19);
            this.ServerGenerateAllBtn.Name = "ServerGenerateAllBtn";
            this.ServerGenerateAllBtn.Size = new System.Drawing.Size(82, 23);
            this.ServerGenerateAllBtn.TabIndex = 29;
            this.ServerGenerateAllBtn.Text = "Generate All";
            this.ServerGenerateAllBtn.UseVisualStyleBackColor = true;
            this.ServerGenerateAllBtn.Click += new System.EventHandler(this.ServerGenerateAllBtn_Click);
            // 
            // ClientGenerateAllBtn
            // 
            this.ClientGenerateAllBtn.Location = new System.Drawing.Point(419, 201);
            this.ClientGenerateAllBtn.Name = "ClientGenerateAllBtn";
            this.ClientGenerateAllBtn.Size = new System.Drawing.Size(82, 23);
            this.ClientGenerateAllBtn.TabIndex = 30;
            this.ClientGenerateAllBtn.Text = "Generate All";
            this.ClientGenerateAllBtn.UseVisualStyleBackColor = true;
            this.ClientGenerateAllBtn.Click += new System.EventHandler(this.ClientGenerateAllBtn_Click);
            // 
            // GXHdlcAddressesResolverDlg
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(575, 481);
            this.Controls.Add(this.HexCb);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.CancelBtn);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GXHdlcAddressesResolverDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "HDLC Address resolver settings";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.BaudRatesPanel.ResumeLayout(false);
            this.BaudRatesPanel.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label InitializeWaitTimeLbl;
        private System.Windows.Forms.TextBox InitializeWaitTimeTb;
        private System.Windows.Forms.Label SearchWaitTimeLbl;
        private System.Windows.Forms.TextBox SearchWaitTimeTb;
        private System.Windows.Forms.Panel CustomSettings;
        private System.Windows.Forms.Panel BaudRatesPanel;
        private System.Windows.Forms.CheckedListBox BaudRatesCl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox TestFailedClientsLastCb;
        private System.Windows.Forms.CheckBox TestFoundMetersFirstCb;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox HexCb;
        private System.Windows.Forms.TextBox ClientAddressesTb;
        private System.Windows.Forms.Label ClientAddressesLbl;
        private System.Windows.Forms.TextBox ServerAddressesTb;
        private System.Windows.Forms.Label ServerAddressesLbl;
        private System.Windows.Forms.Label ConnectionDelayLbl;
        private System.Windows.Forms.TextBox ConnectionDelayTb;
        private System.Windows.Forms.Button ServerGenerateAllBtn;
        private System.Windows.Forms.Button ClientGenerateAllBtn;
    }
}