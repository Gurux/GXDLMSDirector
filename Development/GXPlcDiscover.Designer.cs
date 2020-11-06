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
    partial class GXPlcDiscover
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXPlcDiscover));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.DiscoverMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.RegisterMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.EditMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.InterfaceTypeMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.LlcMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.SFskMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.MediaMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.SerialMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.NetworkMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.MediaSettingsMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.SerialBtn = new System.Windows.Forms.ToolStripButton();
            this.NetworkBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.MediaSettingsBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.DiscoverBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.MetersView = new System.Windows.Forms.ListView();
            this.SystemTitleCh = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AlarmCodeCh = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MacAddressCh = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DiscoveryMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CreateDeviceMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.DiscoverMnu2 = new System.Windows.Forms.ToolStripMenuItem();
            this.RegisterMnu2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ClearMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.TraceView = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.DiscoveryMenu.SuspendLayout();
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
            this.menuStrip1.Size = new System.Drawing.Size(425, 24);
            this.menuStrip1.TabIndex = 66;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FileMnu
            // 
            this.FileMnu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DiscoverMnu,
            this.RegisterMnu,
            this.toolStripSeparator1,
            this.ExitMnu});
            this.FileMnu.Name = "FileMnu";
            this.FileMnu.Size = new System.Drawing.Size(37, 20);
            this.FileMnu.Text = "File";
            // 
            // DiscoverMnu
            // 
            this.DiscoverMnu.Name = "DiscoverMnu";
            this.DiscoverMnu.Size = new System.Drawing.Size(119, 22);
            this.DiscoverMnu.Text = "&Discover";
            this.DiscoverMnu.Click += new System.EventHandler(this.DiscoverMnu_Click);
            // 
            // RegisterMnu
            // 
            this.RegisterMnu.Name = "RegisterMnu";
            this.RegisterMnu.Size = new System.Drawing.Size(119, 22);
            this.RegisterMnu.Text = "&Register";
            this.RegisterMnu.Click += new System.EventHandler(this.RegisterMnu_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(116, 6);
            // 
            // ExitMnu
            // 
            this.ExitMnu.Name = "ExitMnu";
            this.ExitMnu.Size = new System.Drawing.Size(119, 22);
            this.ExitMnu.Text = "Exit";
            this.ExitMnu.Click += new System.EventHandler(this.ExitMnu_Click);
            // 
            // EditMnu
            // 
            this.EditMnu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.InterfaceTypeMnu,
            this.MediaMnu,
            this.MediaSettingsMnu});
            this.EditMnu.Name = "EditMnu";
            this.EditMnu.Size = new System.Drawing.Size(39, 20);
            this.EditMnu.Text = "Edit";
            // 
            // InterfaceTypeMnu
            // 
            this.InterfaceTypeMnu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LlcMnu,
            this.SFskMnu});
            this.InterfaceTypeMnu.Name = "InterfaceTypeMnu";
            this.InterfaceTypeMnu.Size = new System.Drawing.Size(180, 22);
            this.InterfaceTypeMnu.Text = "Interface Type";
            // 
            // LlcMnu
            // 
            this.LlcMnu.Name = "LlcMnu";
            this.LlcMnu.Size = new System.Drawing.Size(180, 22);
            this.LlcMnu.Text = "PLC LLC";
            this.LlcMnu.Click += new System.EventHandler(this.InterfaceTypeChanged);
            // 
            // SFskMnu
            // 
            this.SFskMnu.Name = "SFskMnu";
            this.SFskMnu.Size = new System.Drawing.Size(180, 22);
            this.SFskMnu.Text = "PLC HDLC";
            this.SFskMnu.Click += new System.EventHandler(this.InterfaceTypeChanged);
            // 
            // MediaMnu
            // 
            this.MediaMnu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SerialMnu,
            this.NetworkMnu});
            this.MediaMnu.Name = "MediaMnu";
            this.MediaMnu.Size = new System.Drawing.Size(180, 22);
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
            // MediaSettingsMnu
            // 
            this.MediaSettingsMnu.Name = "MediaSettingsMnu";
            this.MediaSettingsMnu.Size = new System.Drawing.Size(180, 22);
            this.MediaSettingsMnu.Text = "Media Settings...";
            this.MediaSettingsMnu.Click += new System.EventHandler(this.MediaSettingsMnu_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLbl,
            this.toolStripStatusLabel1,
            this.ProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 182);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(425, 22);
            this.statusStrip1.TabIndex = 67;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLbl
            // 
            this.StatusLbl.Name = "StatusLbl";
            this.StatusLbl.Size = new System.Drawing.Size(308, 17);
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
            this.MediaSettingsBtn,
            this.toolStripSeparator3,
            this.DiscoverBtn,
            this.toolStripSeparator5,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(425, 25);
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
            // MediaSettingsBtn
            // 
            this.MediaSettingsBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MediaSettingsBtn.Image = ((System.Drawing.Image)(resources.GetObject("MediaSettingsBtn.Image")));
            this.MediaSettingsBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MediaSettingsBtn.Name = "MediaSettingsBtn";
            this.MediaSettingsBtn.Size = new System.Drawing.Size(23, 22);
            this.MediaSettingsBtn.ToolTipText = "Media Settings";
            this.MediaSettingsBtn.Click += new System.EventHandler(this.MediaSettingsMnu_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // DiscoverBtn
            // 
            this.DiscoverBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DiscoverBtn.Image = ((System.Drawing.Image)(resources.GetObject("DiscoverBtn.Image")));
            this.DiscoverBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DiscoverBtn.Name = "DiscoverBtn";
            this.DiscoverBtn.Size = new System.Drawing.Size(23, 22);
            this.DiscoverBtn.ToolTipText = "Discover";
            this.DiscoverBtn.Click += new System.EventHandler(this.DiscoverMnu_Click);
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
            this.TabControl1.Size = new System.Drawing.Size(425, 133);
            this.TabControl1.TabIndex = 71;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.MetersView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(417, 107);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Available meters";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // MetersView
            // 
            this.MetersView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SystemTitleCh,
            this.AlarmCodeCh,
            this.MacAddressCh});
            this.MetersView.ContextMenuStrip = this.DiscoveryMenu;
            this.MetersView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MetersView.FullRowSelect = true;
            this.MetersView.HideSelection = false;
            this.MetersView.Location = new System.Drawing.Point(3, 3);
            this.MetersView.MultiSelect = false;
            this.MetersView.Name = "MetersView";
            this.MetersView.Size = new System.Drawing.Size(411, 101);
            this.MetersView.TabIndex = 25;
            this.MetersView.UseCompatibleStateImageBehavior = false;
            this.MetersView.View = System.Windows.Forms.View.Details;
            this.MetersView.SelectedIndexChanged += new System.EventHandler(this.MetersView_SelectedIndexChanged);
            // 
            // SystemTitleCh
            // 
            this.SystemTitleCh.Text = "System Title";
            this.SystemTitleCh.Width = 100;
            // 
            // AlarmCodeCh
            // 
            this.AlarmCodeCh.Text = "Alarm Code";
            this.AlarmCodeCh.Width = 88;
            // 
            // MacAddressCh
            // 
            this.MacAddressCh.Text = "MAC Address";
            this.MacAddressCh.Width = 94;
            // 
            // DiscoveryMenu
            // 
            this.DiscoveryMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateDeviceMnu,
            this.DiscoverMnu2,
            this.RegisterMnu2,
            this.toolStripSeparator2,
            this.ClearMnu});
            this.DiscoveryMenu.Name = "contextMenuStrip1";
            this.DiscoveryMenu.Size = new System.Drawing.Size(147, 98);
            this.DiscoveryMenu.Opening += new System.ComponentModel.CancelEventHandler(this.DiscoveryMenu_Opening);
            // 
            // CreateDeviceMnu
            // 
            this.CreateDeviceMnu.Name = "CreateDeviceMnu";
            this.CreateDeviceMnu.Size = new System.Drawing.Size(146, 22);
            this.CreateDeviceMnu.Text = "Create Device";
            this.CreateDeviceMnu.Click += new System.EventHandler(this.CreateDeviceMnu_Click);
            // 
            // DiscoverMnu2
            // 
            this.DiscoverMnu2.Name = "DiscoverMnu2";
            this.DiscoverMnu2.Size = new System.Drawing.Size(146, 22);
            this.DiscoverMnu2.Text = "Discover";
            this.DiscoverMnu2.Click += new System.EventHandler(this.DiscoverMnu_Click);
            // 
            // RegisterMnu2
            // 
            this.RegisterMnu2.Name = "RegisterMnu2";
            this.RegisterMnu2.Size = new System.Drawing.Size(146, 22);
            this.RegisterMnu2.Text = "Register";
            this.RegisterMnu2.Click += new System.EventHandler(this.RegisterMnu_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
            // 
            // ClearMnu
            // 
            this.ClearMnu.Name = "ClearMnu";
            this.ClearMnu.Size = new System.Drawing.Size(146, 22);
            this.ClearMnu.Text = "Clear";
            this.ClearMnu.Click += new System.EventHandler(this.ClearMnu_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.TraceView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(417, 107);
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
            this.TraceView.Size = new System.Drawing.Size(411, 101);
            this.TraceView.TabIndex = 72;
            // 
            // GXPlcDiscover
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 204);
            this.Controls.Add(this.TabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GXPlcDiscover";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PLC Discover";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GXPlcDiscovery_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.DiscoveryMenu.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileMnu;
        private System.Windows.Forms.ToolStripMenuItem EditMnu;
        private System.Windows.Forms.ToolStripMenuItem DiscoverMnu;
        private System.Windows.Forms.ToolStripMenuItem ExitMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem MediaMnu;
        private System.Windows.Forms.ToolStripMenuItem SerialMnu;
        private System.Windows.Forms.ToolStripMenuItem NetworkMnu;
        private System.Windows.Forms.ToolStripMenuItem MediaSettingsMnu;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLbl;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar ProgressBar;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton SerialBtn;
        private System.Windows.Forms.ToolStripButton NetworkBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton DiscoverBtn;
        private System.Windows.Forms.ToolStripButton MediaSettingsBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.TabControl TabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox TraceView;
        private System.Windows.Forms.ToolStripMenuItem RegisterMnu;
        private System.Windows.Forms.ToolStripMenuItem InterfaceTypeMnu;
        private System.Windows.Forms.ToolStripMenuItem LlcMnu;
        private System.Windows.Forms.ToolStripMenuItem SFskMnu;
        private System.Windows.Forms.ListView MetersView;
        private System.Windows.Forms.ColumnHeader SystemTitleCh;
        private System.Windows.Forms.ColumnHeader AlarmCodeCh;
        private System.Windows.Forms.ContextMenuStrip DiscoveryMenu;
        private System.Windows.Forms.ToolStripMenuItem DiscoverMnu2;
        private System.Windows.Forms.ToolStripMenuItem RegisterMnu2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem ClearMnu;
        private System.Windows.Forms.ToolStripMenuItem CreateDeviceMnu;
        private System.Windows.Forms.ColumnHeader MacAddressCh;
    }
}