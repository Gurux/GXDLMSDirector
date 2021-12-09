//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
//
// Version:         $Revision: 9442 $,
//                  $Date: 2017-05-23 15:21:03 +0300 (ti, 23 touko 2017) $
//                  $Author: gurux01 $
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
// More information of Gurux DLMS/COSEM Director: https://www.gurux.org/GXDLMSDirector
//
// This code is licensed under the GNU General Public License v2.
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------

namespace GXDLMSDirector
{
    partial class GXHdlcAddressResolver
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXHdlcAddressResolver));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.ScanMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.EditMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.MediaMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.SerialMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.NetworkMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.UseOpticalProbeMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.ScanBaudRatesMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.SerialBtn = new System.Windows.Forms.ToolStripButton();
            this.NetworkBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.OpticalProbeBtn = new System.Windows.Forms.ToolStripButton();
            this.ScanBaudRatesBtn = new System.Windows.Forms.ToolStripButton();
            this.MediaSettingsBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ScanBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ProgressTb = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.TraceView = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMnu,
            this.EditMnu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(407, 24);
            this.menuStrip1.TabIndex = 66;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FileMnu
            // 
            this.FileMnu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ScanMnu,
            this.toolStripSeparator1,
            this.ExitMnu});
            this.FileMnu.Name = "FileMnu";
            this.FileMnu.Size = new System.Drawing.Size(37, 20);
            this.FileMnu.Text = "File";
            // 
            // ScanMnu
            // 
            this.ScanMnu.Name = "ScanMnu";
            this.ScanMnu.Size = new System.Drawing.Size(99, 22);
            this.ScanMnu.Text = "&Scan";
            this.ScanMnu.Click += new System.EventHandler(this.ScanMnu_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(96, 6);
            // 
            // ExitMnu
            // 
            this.ExitMnu.Name = "ExitMnu";
            this.ExitMnu.Size = new System.Drawing.Size(99, 22);
            this.ExitMnu.Text = "Exit";
            this.ExitMnu.Click += new System.EventHandler(this.ExitMnu_Click);
            // 
            // EditMnu
            // 
            this.EditMnu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MediaMnu,
            this.settingsToolStripMenuItem,
            this.toolStripSeparator2,
            this.UseOpticalProbeMnu,
            this.ScanBaudRatesMnu});
            this.EditMnu.Name = "EditMnu";
            this.EditMnu.Size = new System.Drawing.Size(39, 20);
            this.EditMnu.Text = "Edit";
            // 
            // MediaMnu
            // 
            this.MediaMnu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SerialMnu,
            this.NetworkMnu});
            this.MediaMnu.Name = "MediaMnu";
            this.MediaMnu.Size = new System.Drawing.Size(168, 22);
            this.MediaMnu.Text = "Media";
            // 
            // SerialMnu
            // 
            this.SerialMnu.Name = "SerialMnu";
            this.SerialMnu.Size = new System.Drawing.Size(119, 22);
            this.SerialMnu.Text = "Serial";
            this.SerialMnu.Click += new System.EventHandler(this.OnMediaTypeChanged);
            // 
            // NetworkMnu
            // 
            this.NetworkMnu.Name = "NetworkMnu";
            this.NetworkMnu.Size = new System.Drawing.Size(119, 22);
            this.NetworkMnu.Text = "Network";
            this.NetworkMnu.Click += new System.EventHandler(this.OnMediaTypeChanged);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.settingsToolStripMenuItem.Text = "Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.Settings_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(165, 6);
            // 
            // UseOpticalProbeMnu
            // 
            this.UseOpticalProbeMnu.Name = "UseOpticalProbeMnu";
            this.UseOpticalProbeMnu.Size = new System.Drawing.Size(168, 22);
            this.UseOpticalProbeMnu.Text = "Use Optical Probe";
            this.UseOpticalProbeMnu.Click += new System.EventHandler(this.UseOpticalProbeMnu_Click);
            // 
            // ScanBaudRatesMnu
            // 
            this.ScanBaudRatesMnu.Name = "ScanBaudRatesMnu";
            this.ScanBaudRatesMnu.Size = new System.Drawing.Size(168, 22);
            this.ScanBaudRatesMnu.Text = "Scan Baud Rates";
            this.ScanBaudRatesMnu.Click += new System.EventHandler(this.ScanBaudRateMnu_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLbl,
            this.toolStripStatusLabel1,
            this.ProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 171);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(407, 22);
            this.statusStrip1.TabIndex = 67;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLbl
            // 
            this.StatusLbl.Name = "StatusLbl";
            this.StatusLbl.Size = new System.Drawing.Size(290, 17);
            this.StatusLbl.Spring = true;
            this.StatusLbl.Text = "Ready";
            this.StatusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ProgressBar.AutoSize = false;
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.RightToLeftLayout = true;
            this.ProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SerialBtn,
            this.NetworkBtn,
            this.toolStripSeparator4,
            this.OpticalProbeBtn,
            this.ScanBaudRatesBtn,
            this.MediaSettingsBtn,
            this.toolStripSeparator3,
            this.ScanBtn,
            this.toolStripSeparator5,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(407, 25);
            this.toolStrip1.TabIndex = 69;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // SerialBtn
            // 
            this.SerialBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SerialBtn.Image = ((System.Drawing.Image)(resources.GetObject("SerialBtn.Image")));
            this.SerialBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SerialBtn.Name = "SerialBtn";
            this.SerialBtn.Size = new System.Drawing.Size(23, 22);
            this.SerialBtn.ToolTipText = "Serial port";
            this.SerialBtn.Click += new System.EventHandler(this.OnMediaTypeChanged);
            // 
            // NetworkBtn
            // 
            this.NetworkBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NetworkBtn.Image = ((System.Drawing.Image)(resources.GetObject("NetworkBtn.Image")));
            this.NetworkBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NetworkBtn.Name = "NetworkBtn";
            this.NetworkBtn.Size = new System.Drawing.Size(23, 22);
            this.NetworkBtn.Text = "toolStripButton5";
            this.NetworkBtn.ToolTipText = "Network";
            this.NetworkBtn.Click += new System.EventHandler(this.OnMediaTypeChanged);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // OpticalProbeBtn
            // 
            this.OpticalProbeBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OpticalProbeBtn.Image = ((System.Drawing.Image)(resources.GetObject("OpticalProbeBtn.Image")));
            this.OpticalProbeBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpticalProbeBtn.Name = "OpticalProbeBtn";
            this.OpticalProbeBtn.Size = new System.Drawing.Size(23, 22);
            this.OpticalProbeBtn.ToolTipText = "Optical Probe";
            this.OpticalProbeBtn.Click += new System.EventHandler(this.UseOpticalProbeMnu_Click);
            // 
            // ScanBaudRatesBtn
            // 
            this.ScanBaudRatesBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ScanBaudRatesBtn.Image = ((System.Drawing.Image)(resources.GetObject("ScanBaudRatesBtn.Image")));
            this.ScanBaudRatesBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ScanBaudRatesBtn.Name = "ScanBaudRatesBtn";
            this.ScanBaudRatesBtn.Size = new System.Drawing.Size(23, 22);
            this.ScanBaudRatesBtn.ToolTipText = "Scan Baud Rates";
            this.ScanBaudRatesBtn.Click += new System.EventHandler(this.ScanBaudRateMnu_Click);
            // 
            // MediaSettingsBtn
            // 
            this.MediaSettingsBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MediaSettingsBtn.Image = ((System.Drawing.Image)(resources.GetObject("MediaSettingsBtn.Image")));
            this.MediaSettingsBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MediaSettingsBtn.Name = "MediaSettingsBtn";
            this.MediaSettingsBtn.Size = new System.Drawing.Size(23, 22);
            this.MediaSettingsBtn.Click += new System.EventHandler(this.Settings_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // ScanBtn
            // 
            this.ScanBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ScanBtn.Image = ((System.Drawing.Image)(resources.GetObject("ScanBtn.Image")));
            this.ScanBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ScanBtn.Name = "ScanBtn";
            this.ScanBtn.Size = new System.Drawing.Size(23, 22);
            this.ScanBtn.Click += new System.EventHandler(this.ScanMnu_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Technical support...";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.tabPage1);
            this.TabControl1.Controls.Add(this.tabPage2);
            this.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl1.Location = new System.Drawing.Point(0, 49);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(407, 122);
            this.TabControl1.TabIndex = 71;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ProgressTb);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(399, 96);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Progress";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ProgressTb
            // 
            this.ProgressTb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProgressTb.Location = new System.Drawing.Point(3, 3);
            this.ProgressTb.Multiline = true;
            this.ProgressTb.Name = "ProgressTb";
            this.ProgressTb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ProgressTb.Size = new System.Drawing.Size(393, 90);
            this.ProgressTb.TabIndex = 71;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.TraceView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(399, 96);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Trace";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // TraceView
            // 
            this.TraceView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TraceView.Location = new System.Drawing.Point(3, 3);
            this.TraceView.Multiline = true;
            this.TraceView.Name = "TraceView";
            this.TraceView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TraceView.Size = new System.Drawing.Size(393, 90);
            this.TraceView.TabIndex = 72;
            // 
            // GXHdlcAddressResolver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 193);
            this.Controls.Add(this.TabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GXHdlcAddressResolver";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "HDLC Address resolver";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GXHdlcAddressResolver_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileMnu;
        private System.Windows.Forms.ToolStripMenuItem EditMnu;
        private System.Windows.Forms.ToolStripMenuItem ScanMnu;
        private System.Windows.Forms.ToolStripMenuItem ExitMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem UseOpticalProbeMnu;
        private System.Windows.Forms.ToolStripMenuItem MediaMnu;
        private System.Windows.Forms.ToolStripMenuItem SerialMnu;
        private System.Windows.Forms.ToolStripMenuItem NetworkMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLbl;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar ProgressBar;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ScanBaudRatesBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton SerialBtn;
        private System.Windows.Forms.ToolStripButton NetworkBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton OpticalProbeBtn;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton ScanBtn;
        private System.Windows.Forms.ToolStripButton MediaSettingsBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.TabControl TabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox ProgressTb;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox TraceView;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ScanBaudRatesMnu;
    }
}