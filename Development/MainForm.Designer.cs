//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/MainForm.Designer.cs $
//
// Version:         $Revision: 9204 $,
//                  $Date: 2017-02-06 12:36:45 +0200 (ma, 06 helmi 2017) $
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
// More information of Gurux DLMS/COSEM Director: http://www.gurux.org/GXDLMSDirector
//
// This code is licensed under the GNU General Public License v2.
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------


namespace GXDLMSDirector
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.AddDeviceMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.RefreshMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.RecentFilesMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewToolbarMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewStatusbarMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.ObjectTreeMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.ObjectListMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.GroupsMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.TraceMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.PropertiesMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConnectMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.DisconnectMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.ReadMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.WriteMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.CancelBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            this.LogMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.ClearLogMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.ManufacturersMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.OBISCodesMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.updateManufactureSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ContentsMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.LibraryVersionsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.AboutMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ConnectCMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.DisconnectCMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.ReadCMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.AddDeviceCMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.Line1CMnu = new System.Windows.Forms.ToolStripSeparator();
            this.DeleteCMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.PropertiesCMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.NewBtn = new System.Windows.Forms.ToolStripButton();
            this.OpenBtn = new System.Windows.Forms.ToolStripButton();
            this.SaveBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ConnectBtn = new System.Windows.Forms.ToolStripButton();
            this.ReadBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.WriteBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.OptionsBtn = new System.Windows.Forms.ToolStripButton();
            this.DeleteBtn = new System.Windows.Forms.ToolStripButton();
            this.NewListTBtn = new System.Windows.Forms.ToolBarButton();
            this.OpenTBtn = new System.Windows.Forms.ToolBarButton();
            this.SaveTBtn = new System.Windows.Forms.ToolBarButton();
            this.NewDeviceTBtn = new System.Windows.Forms.ToolBarButton();
            this.Separator1TBtn = new System.Windows.Forms.ToolBarButton();
            this.ConnectTBtn = new System.Windows.Forms.ToolBarButton();
            this.MonitorTBtn = new System.Windows.Forms.ToolBarButton();
            this.ReadAllTBtn = new System.Windows.Forms.ToolBarButton();
            this.WriteAllTBtn = new System.Windows.Forms.ToolBarButton();
            this.Separator2TBtn = new System.Windows.Forms.ToolBarButton();
            this.PropertiesTBtn = new System.Windows.Forms.ToolBarButton();
            this.RemoveTBtn = new System.Windows.Forms.ToolBarButton();
            this.ListView = new System.Windows.Forms.TabPage();
            this.ObjectList = new System.Windows.Forms.ListView();
            this.DescriptionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TreeView = new System.Windows.Forms.TabPage();
            this.ObjectTree = new System.Windows.Forms.TreeView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.TraceView = new System.Windows.Forms.TextBox();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.ObjectValueView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DeviceInfoView = new System.Windows.Forms.Panel();
            this.DeviceGb = new System.Windows.Forms.GroupBox();
            this.ManufacturerLbl = new System.Windows.Forms.Label();
            this.StatusValueLbl = new System.Windows.Forms.Label();
            this.ManufacturerValueLbl = new System.Windows.Forms.Label();
            this.DeviceStateLbl = new System.Windows.Forms.Label();
            this.PhysicalAddressLbl = new System.Windows.Forms.Label();
            this.ClientAddressValueLbl = new System.Windows.Forms.Label();
            this.PhysicalAddressValueLbl = new System.Windows.Forms.Label();
            this.ClientAddressLbl = new System.Windows.Forms.Label();
            this.LogicalAddressLbl = new System.Windows.Forms.Label();
            this.LogicalAddressValueLbl = new System.Windows.Forms.Label();
            this.DeviceList = new System.Windows.Forms.ListView();
            this.DeviceNameCH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ObjectPanelFrame = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.ListView.SuspendLayout();
            this.TreeView.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.DeviceInfoView.SuspendLayout();
            this.DeviceGb.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.updateManufactureSettingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(725, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewMnu,
            this.AddDeviceMnu,
            this.OpenMnu,
            this.toolStripMenuItem3,
            this.SaveMnu,
            this.SaveAsMnu,
            this.RefreshMnu,
            this.toolStripMenuItem2,
            this.RecentFilesMnu,
            this.toolStripMenuItem11,
            this.ExitMnu});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // NewMnu
            // 
            this.NewMnu.Name = "NewMnu";
            this.NewMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.NewMnu.Size = new System.Drawing.Size(176, 22);
            this.NewMnu.Text = "&New";
            this.NewMnu.Click += new System.EventHandler(this.NewMnu_Click);
            // 
            // AddDeviceMnu
            // 
            this.AddDeviceMnu.Name = "AddDeviceMnu";
            this.AddDeviceMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.AddDeviceMnu.Size = new System.Drawing.Size(176, 22);
            this.AddDeviceMnu.Text = "Add Device";
            this.AddDeviceMnu.Click += new System.EventHandler(this.AddDeviceMnu_Click);
            // 
            // OpenMnu
            // 
            this.OpenMnu.Name = "OpenMnu";
            this.OpenMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.OpenMnu.Size = new System.Drawing.Size(176, 22);
            this.OpenMnu.Text = "&Open";
            this.OpenMnu.Click += new System.EventHandler(this.OpenMnu_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(173, 6);
            // 
            // SaveMnu
            // 
            this.SaveMnu.Name = "SaveMnu";
            this.SaveMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveMnu.Size = new System.Drawing.Size(176, 22);
            this.SaveMnu.Text = "&Save";
            this.SaveMnu.Click += new System.EventHandler(this.SaveMnu_Click);
            // 
            // SaveAsMnu
            // 
            this.SaveAsMnu.Name = "SaveAsMnu";
            this.SaveAsMnu.Size = new System.Drawing.Size(176, 22);
            this.SaveAsMnu.Text = "Save As...";
            this.SaveAsMnu.Click += new System.EventHandler(this.SaveAsMnu_Click);
            // 
            // RefreshMnu
            // 
            this.RefreshMnu.Name = "RefreshMnu";
            this.RefreshMnu.Size = new System.Drawing.Size(176, 22);
            this.RefreshMnu.Text = "&Refresh";
            this.RefreshMnu.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(173, 6);
            // 
            // RecentFilesMnu
            // 
            this.RecentFilesMnu.Name = "RecentFilesMnu";
            this.RecentFilesMnu.Size = new System.Drawing.Size(176, 22);
            this.RecentFilesMnu.Text = "Recent files";
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(173, 6);
            // 
            // ExitMnu
            // 
            this.ExitMnu.Name = "ExitMnu";
            this.ExitMnu.Size = new System.Drawing.Size(176, 22);
            this.ExitMnu.Text = "&Exit";
            this.ExitMnu.Click += new System.EventHandler(this.ExitMnu_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewToolbarMnu,
            this.ViewStatusbarMnu,
            this.toolStripMenuItem12,
            this.ObjectTreeMnu,
            this.ObjectListMnu,
            this.GroupsMnu,
            this.TraceMnu,
            this.toolStripMenuItem7,
            this.PropertiesMnu});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // ViewToolbarMnu
            // 
            this.ViewToolbarMnu.Name = "ViewToolbarMnu";
            this.ViewToolbarMnu.Size = new System.Drawing.Size(136, 22);
            this.ViewToolbarMnu.Text = "ToolBar";
            this.ViewToolbarMnu.Click += new System.EventHandler(this.ViewToolbarMnu_Click);
            // 
            // ViewStatusbarMnu
            // 
            this.ViewStatusbarMnu.Name = "ViewStatusbarMnu";
            this.ViewStatusbarMnu.Size = new System.Drawing.Size(136, 22);
            this.ViewStatusbarMnu.Text = "&Statusbar";
            this.ViewStatusbarMnu.Click += new System.EventHandler(this.ViewStatusbarMnu_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(133, 6);
            // 
            // ObjectTreeMnu
            // 
            this.ObjectTreeMnu.Name = "ObjectTreeMnu";
            this.ObjectTreeMnu.Size = new System.Drawing.Size(136, 22);
            this.ObjectTreeMnu.Text = "Object Tree";
            this.ObjectTreeMnu.Click += new System.EventHandler(this.ObjectTreeMnu_Click);
            // 
            // ObjectListMnu
            // 
            this.ObjectListMnu.Name = "ObjectListMnu";
            this.ObjectListMnu.Size = new System.Drawing.Size(136, 22);
            this.ObjectListMnu.Text = "Object &List";
            this.ObjectListMnu.Click += new System.EventHandler(this.ObjectListMnu_Click);
            // 
            // GroupsMnu
            // 
            this.GroupsMnu.Name = "GroupsMnu";
            this.GroupsMnu.Size = new System.Drawing.Size(136, 22);
            this.GroupsMnu.Text = "Groups";
            this.GroupsMnu.Click += new System.EventHandler(this.GroupsMnu_Click);
            // 
            // TraceMnu
            // 
            this.TraceMnu.Name = "TraceMnu";
            this.TraceMnu.Size = new System.Drawing.Size(136, 22);
            this.TraceMnu.Text = "&Trace";
            this.TraceMnu.Click += new System.EventHandler(this.TraceMenu_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(133, 6);
            // 
            // PropertiesMnu
            // 
            this.PropertiesMnu.Name = "PropertiesMnu";
            this.PropertiesMnu.Size = new System.Drawing.Size(136, 22);
            this.PropertiesMnu.Text = "&Properties...";
            this.PropertiesMnu.Click += new System.EventHandler(this.PropertiesMnu_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConnectMnu,
            this.DisconnectMnu,
            this.toolStripMenuItem4,
            this.ReadMnu,
            this.WriteMnu,
            this.DeleteMnu,
            this.toolStripMenuItem6,
            this.CancelBtn,
            this.toolStripMenuItem10,
            this.LogMnu,
            this.ClearLogMnu,
            this.toolStripMenuItem8,
            this.ManufacturersMnu,
            this.OBISCodesMnu});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // ConnectMnu
            // 
            this.ConnectMnu.Name = "ConnectMnu";
            this.ConnectMnu.Size = new System.Drawing.Size(164, 22);
            this.ConnectMnu.Text = "&Connect";
            this.ConnectMnu.Click += new System.EventHandler(this.ConnectMnu_Click);
            // 
            // DisconnectMnu
            // 
            this.DisconnectMnu.Name = "DisconnectMnu";
            this.DisconnectMnu.Size = new System.Drawing.Size(164, 22);
            this.DisconnectMnu.Text = "&Disconnect";
            this.DisconnectMnu.Click += new System.EventHandler(this.DisconnectMnu_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(161, 6);
            // 
            // ReadMnu
            // 
            this.ReadMnu.Name = "ReadMnu";
            this.ReadMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.ReadMnu.Size = new System.Drawing.Size(164, 22);
            this.ReadMnu.Text = "&Read";
            this.ReadMnu.Click += new System.EventHandler(this.ReadMnu_Click);
            // 
            // WriteMnu
            // 
            this.WriteMnu.Name = "WriteMnu";
            this.WriteMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.WriteMnu.Size = new System.Drawing.Size(164, 22);
            this.WriteMnu.Text = "&Write";
            this.WriteMnu.Click += new System.EventHandler(this.WriteMnu_Click);
            // 
            // DeleteMnu
            // 
            this.DeleteMnu.Name = "DeleteMnu";
            this.DeleteMnu.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.DeleteMnu.Size = new System.Drawing.Size(164, 22);
            this.DeleteMnu.Text = "Delete";
            this.DeleteMnu.Click += new System.EventHandler(this.DeleteMnu_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(161, 6);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.End)));
            this.CancelBtn.Size = new System.Drawing.Size(164, 22);
            this.CancelBtn.Text = "&Cancel";
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(161, 6);
            // 
            // LogMnu
            // 
            this.LogMnu.Name = "LogMnu";
            this.LogMnu.Size = new System.Drawing.Size(164, 22);
            this.LogMnu.Text = "View Log...";
            this.LogMnu.Click += new System.EventHandler(this.LogMnu_Click);
            // 
            // ClearLogMnu
            // 
            this.ClearLogMnu.Name = "ClearLogMnu";
            this.ClearLogMnu.Size = new System.Drawing.Size(164, 22);
            this.ClearLogMnu.Text = "Clear Log";
            this.ClearLogMnu.Click += new System.EventHandler(this.ClearLogMnu_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(161, 6);
            // 
            // ManufacturersMnu
            // 
            this.ManufacturersMnu.Name = "ManufacturersMnu";
            this.ManufacturersMnu.Size = new System.Drawing.Size(164, 22);
            this.ManufacturersMnu.Text = "Manufacturers...";
            this.ManufacturersMnu.Click += new System.EventHandler(this.ManufacturersMnu_Click);
            // 
            // OBISCodesMnu
            // 
            this.OBISCodesMnu.Name = "OBISCodesMnu";
            this.OBISCodesMnu.Size = new System.Drawing.Size(164, 22);
            this.OBISCodesMnu.Text = "OBIS Codes...";
            this.OBISCodesMnu.Click += new System.EventHandler(this.OBISCodesMnu_Click);
            // 
            // updateManufactureSettingsToolStripMenuItem
            // 
            this.updateManufactureSettingsToolStripMenuItem.Name = "updateManufactureSettingsToolStripMenuItem";
            this.updateManufactureSettingsToolStripMenuItem.Size = new System.Drawing.Size(219, 20);
            this.updateManufactureSettingsToolStripMenuItem.Text = "New Manufacture Settings Available...";
            this.updateManufactureSettingsToolStripMenuItem.Visible = false;
            this.updateManufactureSettingsToolStripMenuItem.Click += new System.EventHandler(this.updateManufactureSettingsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContentsMnu,
            this.toolStripMenuItem1,
            this.LibraryVersionsMenu,
            this.toolStripMenuItem9,
            this.AboutMnu});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // ContentsMnu
            // 
            this.ContentsMnu.Name = "ContentsMnu";
            this.ContentsMnu.Size = new System.Drawing.Size(165, 22);
            this.ContentsMnu.Text = "Contents...";
            this.ContentsMnu.Click += new System.EventHandler(this.ContentsMnu_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(162, 6);
            // 
            // LibraryVersionsMenu
            // 
            this.LibraryVersionsMenu.Name = "LibraryVersionsMenu";
            this.LibraryVersionsMenu.Size = new System.Drawing.Size(165, 22);
            this.LibraryVersionsMenu.Text = "Library Versions...";
            this.LibraryVersionsMenu.Click += new System.EventHandler(this.LibraryVersionsMenu_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(162, 6);
            // 
            // AboutMnu
            // 
            this.AboutMnu.Name = "AboutMnu";
            this.AboutMnu.Size = new System.Drawing.Size(165, 22);
            this.AboutMnu.Text = "&About...";
            this.AboutMnu.Click += new System.EventHandler(this.AboutMnu_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLbl,
            this.toolStripStatusLabel1,
            this.ProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 514);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(725, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLbl
            // 
            this.StatusLbl.Name = "StatusLbl";
            this.StatusLbl.Size = new System.Drawing.Size(608, 17);
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
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            this.imageList1.Images.SetKeyName(7, "DeviceProperty.bmp");
            this.imageList1.Images.SetKeyName(8, "DeviceTable.bmp");
            this.imageList1.Images.SetKeyName(9, "DeviceDirty.bmp");
            this.imageList1.Images.SetKeyName(10, "PropertyDirty.bmp");
            this.imageList1.Images.SetKeyName(11, "DeviceCategory.bmp");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConnectCMnu,
            this.DisconnectCMnu,
            this.ReadCMnu,
            this.AddDeviceCMnu,
            this.Line1CMnu,
            this.DeleteCMnu,
            this.toolStripMenuItem5,
            this.PropertiesCMnu});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(135, 148);
            // 
            // ConnectCMnu
            // 
            this.ConnectCMnu.Name = "ConnectCMnu";
            this.ConnectCMnu.Size = new System.Drawing.Size(134, 22);
            this.ConnectCMnu.Text = "&Connect";
            this.ConnectCMnu.Click += new System.EventHandler(this.ConnectMnu_Click);
            // 
            // DisconnectCMnu
            // 
            this.DisconnectCMnu.Name = "DisconnectCMnu";
            this.DisconnectCMnu.Size = new System.Drawing.Size(134, 22);
            this.DisconnectCMnu.Text = "&Disconnect";
            this.DisconnectCMnu.Click += new System.EventHandler(this.DisconnectMnu_Click);
            // 
            // ReadCMnu
            // 
            this.ReadCMnu.Name = "ReadCMnu";
            this.ReadCMnu.Size = new System.Drawing.Size(134, 22);
            this.ReadCMnu.Text = "Read";
            this.ReadCMnu.Click += new System.EventHandler(this.ReadMnu_Click);
            // 
            // AddDeviceCMnu
            // 
            this.AddDeviceCMnu.Name = "AddDeviceCMnu";
            this.AddDeviceCMnu.Size = new System.Drawing.Size(134, 22);
            this.AddDeviceCMnu.Text = "&Add Device";
            this.AddDeviceCMnu.Click += new System.EventHandler(this.AddDeviceMnu_Click);
            // 
            // Line1CMnu
            // 
            this.Line1CMnu.Name = "Line1CMnu";
            this.Line1CMnu.Size = new System.Drawing.Size(131, 6);
            // 
            // DeleteCMnu
            // 
            this.DeleteCMnu.Name = "DeleteCMnu";
            this.DeleteCMnu.Size = new System.Drawing.Size(134, 22);
            this.DeleteCMnu.Text = "Delete";
            this.DeleteCMnu.Click += new System.EventHandler(this.DeleteMnu_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(131, 6);
            // 
            // PropertiesCMnu
            // 
            this.PropertiesCMnu.Name = "PropertiesCMnu";
            this.PropertiesCMnu.Size = new System.Drawing.Size(134, 22);
            this.PropertiesCMnu.Text = "&Properties";
            this.PropertiesCMnu.Click += new System.EventHandler(this.PropertiesMnu_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewBtn,
            this.OpenBtn,
            this.SaveBtn,
            this.toolStripSeparator1,
            this.ConnectBtn,
            this.ReadBtn,
            this.toolStripSeparator2,
            this.WriteBtn,
            this.toolStripSeparator3,
            this.OptionsBtn,
            this.DeleteBtn});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(725, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // NewBtn
            // 
            this.NewBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NewBtn.Image = global::GXDLMSDirector.Properties.Resources.NewMnu;
            this.NewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NewBtn.Name = "NewBtn";
            this.NewBtn.Size = new System.Drawing.Size(23, 22);
            this.NewBtn.ToolTipText = "New";
            this.NewBtn.Click += new System.EventHandler(this.NewMnu_Click);
            // 
            // OpenBtn
            // 
            this.OpenBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OpenBtn.Image = global::GXDLMSDirector.Properties.Resources.OpenMnu;
            this.OpenBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenBtn.Name = "OpenBtn";
            this.OpenBtn.Size = new System.Drawing.Size(23, 22);
            this.OpenBtn.Text = "toolStripButton4";
            this.OpenBtn.ToolTipText = "Open";
            this.OpenBtn.Click += new System.EventHandler(this.OpenMnu_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveBtn.Image = global::GXDLMSDirector.Properties.Resources.SaveMnu;
            this.SaveBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(23, 22);
            this.SaveBtn.Text = "toolStripButton1";
            this.SaveBtn.ToolTipText = "Save";
            this.SaveBtn.Click += new System.EventHandler(this.SaveMnu_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ConnectBtn
            // 
            this.ConnectBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ConnectBtn.Image = global::GXDLMSDirector.Properties.Resources.ConnectMnu;
            this.ConnectBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ConnectBtn.Name = "ConnectBtn";
            this.ConnectBtn.Size = new System.Drawing.Size(23, 22);
            this.ConnectBtn.Text = "toolStripButton1";
            this.ConnectBtn.ToolTipText = "Connect";
            this.ConnectBtn.Click += new System.EventHandler(this.ConnectBtn_Click);
            // 
            // ReadBtn
            // 
            this.ReadBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ReadBtn.Image = global::GXDLMSDirector.Properties.Resources.ReadMnu;
            this.ReadBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ReadBtn.Name = "ReadBtn";
            this.ReadBtn.Size = new System.Drawing.Size(23, 22);
            this.ReadBtn.Text = "toolStripButton5";
            this.ReadBtn.ToolTipText = "Read";
            this.ReadBtn.Click += new System.EventHandler(this.ReadMnu_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // WriteBtn
            // 
            this.WriteBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.WriteBtn.Image = global::GXDLMSDirector.Properties.Resources.WriteMnu;
            this.WriteBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.WriteBtn.Name = "WriteBtn";
            this.WriteBtn.Size = new System.Drawing.Size(23, 22);
            this.WriteBtn.Text = "Write";
            this.WriteBtn.Click += new System.EventHandler(this.WriteMnu_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // OptionsBtn
            // 
            this.OptionsBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OptionsBtn.Image = global::GXDLMSDirector.Properties.Resources.OptionsMnu;
            this.OptionsBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OptionsBtn.Name = "OptionsBtn";
            this.OptionsBtn.Size = new System.Drawing.Size(23, 22);
            this.OptionsBtn.Text = "toolStripButton2";
            this.OptionsBtn.ToolTipText = "Properties";
            this.OptionsBtn.Click += new System.EventHandler(this.PropertiesMnu_Click);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeleteBtn.Image = global::GXDLMSDirector.Properties.Resources.DeleteMnu;
            this.DeleteBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(23, 22);
            this.DeleteBtn.Text = "toolStripButton6";
            this.DeleteBtn.ToolTipText = "Delete";
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteMnu_Click);
            // 
            // NewListTBtn
            // 
            this.NewListTBtn.ImageIndex = 0;
            this.NewListTBtn.Name = "NewListTBtn";
            // 
            // OpenTBtn
            // 
            this.OpenTBtn.ImageIndex = 1;
            this.OpenTBtn.Name = "OpenTBtn";
            // 
            // SaveTBtn
            // 
            this.SaveTBtn.ImageIndex = 2;
            this.SaveTBtn.Name = "SaveTBtn";
            // 
            // NewDeviceTBtn
            // 
            this.NewDeviceTBtn.ImageIndex = 3;
            this.NewDeviceTBtn.Name = "NewDeviceTBtn";
            // 
            // Separator1TBtn
            // 
            this.Separator1TBtn.Name = "Separator1TBtn";
            this.Separator1TBtn.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // ConnectTBtn
            // 
            this.ConnectTBtn.ImageIndex = 4;
            this.ConnectTBtn.Name = "ConnectTBtn";
            // 
            // MonitorTBtn
            // 
            this.MonitorTBtn.ImageIndex = 5;
            this.MonitorTBtn.Name = "MonitorTBtn";
            // 
            // ReadAllTBtn
            // 
            this.ReadAllTBtn.ImageIndex = 6;
            this.ReadAllTBtn.Name = "ReadAllTBtn";
            // 
            // WriteAllTBtn
            // 
            this.WriteAllTBtn.ImageIndex = 7;
            this.WriteAllTBtn.Name = "WriteAllTBtn";
            // 
            // Separator2TBtn
            // 
            this.Separator2TBtn.Name = "Separator2TBtn";
            this.Separator2TBtn.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // PropertiesTBtn
            // 
            this.PropertiesTBtn.ImageIndex = 8;
            this.PropertiesTBtn.Name = "PropertiesTBtn";
            // 
            // RemoveTBtn
            // 
            this.RemoveTBtn.ImageIndex = 9;
            this.RemoveTBtn.Name = "RemoveTBtn";
            // 
            // ListView
            // 
            this.ListView.Controls.Add(this.ObjectList);
            this.ListView.Location = new System.Drawing.Point(4, 4);
            this.ListView.Name = "ListView";
            this.ListView.Padding = new System.Windows.Forms.Padding(3);
            this.ListView.Size = new System.Drawing.Size(192, 439);
            this.ListView.TabIndex = 1;
            this.ListView.Text = "List";
            this.ListView.UseVisualStyleBackColor = true;
            // 
            // ObjectList
            // 
            this.ObjectList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ObjectList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.DescriptionColumnHeader});
            this.ObjectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ObjectList.HideSelection = false;
            this.ObjectList.Location = new System.Drawing.Point(3, 3);
            this.ObjectList.Name = "ObjectList";
            this.ObjectList.Size = new System.Drawing.Size(186, 433);
            this.ObjectList.TabIndex = 0;
            this.ObjectList.UseCompatibleStateImageBehavior = false;
            this.ObjectList.View = System.Windows.Forms.View.Details;
            this.ObjectList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ObjectList_ItemSelectionChanged);
            this.ObjectList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ObjectList_KeyUp);
            this.ObjectList.Resize += new System.EventHandler(this.ObjectList_Resize);
            // 
            // DescriptionColumnHeader
            // 
            this.DescriptionColumnHeader.Text = "Items:";
            this.DescriptionColumnHeader.Width = 182;
            // 
            // TreeView
            // 
            this.TreeView.Controls.Add(this.ObjectTree);
            this.TreeView.Location = new System.Drawing.Point(4, 4);
            this.TreeView.Name = "TreeView";
            this.TreeView.Padding = new System.Windows.Forms.Padding(3);
            this.TreeView.Size = new System.Drawing.Size(192, 439);
            this.TreeView.TabIndex = 0;
            this.TreeView.Text = "Tree";
            this.TreeView.UseVisualStyleBackColor = true;
            // 
            // ObjectTree
            // 
            this.ObjectTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ObjectTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ObjectTree.HideSelection = false;
            this.ObjectTree.ImageIndex = 0;
            this.ObjectTree.ImageList = this.imageList1;
            this.ObjectTree.Location = new System.Drawing.Point(3, 3);
            this.ObjectTree.Name = "ObjectTree";
            this.ObjectTree.SelectedImageIndex = 0;
            this.ObjectTree.Size = new System.Drawing.Size(186, 433);
            this.ObjectTree.TabIndex = 3;
            this.ObjectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ObjectTree_AfterSelect);
            this.ObjectTree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ObjectTree_MouseUp);
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.TreeView);
            this.tabControl1.Controls.Add(this.ListView);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabControl1.Location = new System.Drawing.Point(0, 49);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(200, 465);
            this.tabControl1.TabIndex = 10;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(200, 49);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1, 465);
            this.splitter1.TabIndex = 11;
            this.splitter1.TabStop = false;
            // 
            // TraceView
            // 
            this.TraceView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TraceView.Location = new System.Drawing.Point(201, 465);
            this.TraceView.Multiline = true;
            this.TraceView.Name = "TraceView";
            this.TraceView.ReadOnly = true;
            this.TraceView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TraceView.Size = new System.Drawing.Size(524, 49);
            this.TraceView.TabIndex = 16;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(201, 462);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(524, 3);
            this.splitter2.TabIndex = 17;
            this.splitter2.TabStop = false;
            // 
            // ObjectValueView
            // 
            this.ObjectValueView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.ObjectValueView.FullRowSelect = true;
            this.ObjectValueView.HideSelection = false;
            this.ObjectValueView.Location = new System.Drawing.Point(564, 317);
            this.ObjectValueView.MultiSelect = false;
            this.ObjectValueView.Name = "ObjectValueView";
            this.ObjectValueView.Size = new System.Drawing.Size(134, 87);
            this.ObjectValueView.TabIndex = 21;
            this.ObjectValueView.UseCompatibleStateImageBehavior = false;
            this.ObjectValueView.View = System.Windows.Forms.View.Details;
            this.ObjectValueView.DoubleClick += new System.EventHandler(this.ObjectValueView_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 117;
            // 
            // DeviceInfoView
            // 
            this.DeviceInfoView.Controls.Add(this.DeviceGb);
            this.DeviceInfoView.Location = new System.Drawing.Point(269, 45);
            this.DeviceInfoView.Name = "DeviceInfoView";
            this.DeviceInfoView.Size = new System.Drawing.Size(422, 266);
            this.DeviceInfoView.TabIndex = 20;
            // 
            // DeviceGb
            // 
            this.DeviceGb.Controls.Add(this.ManufacturerLbl);
            this.DeviceGb.Controls.Add(this.StatusValueLbl);
            this.DeviceGb.Controls.Add(this.ManufacturerValueLbl);
            this.DeviceGb.Controls.Add(this.DeviceStateLbl);
            this.DeviceGb.Controls.Add(this.PhysicalAddressLbl);
            this.DeviceGb.Controls.Add(this.ClientAddressValueLbl);
            this.DeviceGb.Controls.Add(this.PhysicalAddressValueLbl);
            this.DeviceGb.Controls.Add(this.ClientAddressLbl);
            this.DeviceGb.Controls.Add(this.LogicalAddressLbl);
            this.DeviceGb.Controls.Add(this.LogicalAddressValueLbl);
            this.DeviceGb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeviceGb.Location = new System.Drawing.Point(0, 0);
            this.DeviceGb.Name = "DeviceGb";
            this.DeviceGb.Size = new System.Drawing.Size(422, 266);
            this.DeviceGb.TabIndex = 12;
            this.DeviceGb.TabStop = false;
            // 
            // ManufacturerLbl
            // 
            this.ManufacturerLbl.AutoSize = true;
            this.ManufacturerLbl.Location = new System.Drawing.Point(18, 28);
            this.ManufacturerLbl.Name = "ManufacturerLbl";
            this.ManufacturerLbl.Size = new System.Drawing.Size(73, 13);
            this.ManufacturerLbl.TabIndex = 2;
            this.ManufacturerLbl.Text = "Manufacturer:";
            // 
            // StatusValueLbl
            // 
            this.StatusValueLbl.AutoSize = true;
            this.StatusValueLbl.Location = new System.Drawing.Point(114, 120);
            this.StatusValueLbl.Name = "StatusValueLbl";
            this.StatusValueLbl.Size = new System.Drawing.Size(78, 13);
            this.StatusValueLbl.TabIndex = 11;
            this.StatusValueLbl.Text = "StatusValueLbl";
            // 
            // ManufacturerValueLbl
            // 
            this.ManufacturerValueLbl.AutoSize = true;
            this.ManufacturerValueLbl.Location = new System.Drawing.Point(114, 28);
            this.ManufacturerValueLbl.Name = "ManufacturerValueLbl";
            this.ManufacturerValueLbl.Size = new System.Drawing.Size(111, 13);
            this.ManufacturerValueLbl.TabIndex = 3;
            this.ManufacturerValueLbl.Text = "ManufacturerValueLbl";
            // 
            // DeviceStateLbl
            // 
            this.DeviceStateLbl.AutoSize = true;
            this.DeviceStateLbl.Location = new System.Drawing.Point(18, 120);
            this.DeviceStateLbl.Name = "DeviceStateLbl";
            this.DeviceStateLbl.Size = new System.Drawing.Size(40, 13);
            this.DeviceStateLbl.TabIndex = 10;
            this.DeviceStateLbl.Text = "Status:";
            // 
            // PhysicalAddressLbl
            // 
            this.PhysicalAddressLbl.AutoSize = true;
            this.PhysicalAddressLbl.Location = new System.Drawing.Point(18, 51);
            this.PhysicalAddressLbl.Name = "PhysicalAddressLbl";
            this.PhysicalAddressLbl.Size = new System.Drawing.Size(90, 13);
            this.PhysicalAddressLbl.TabIndex = 4;
            this.PhysicalAddressLbl.Text = "Physical Address:";
            // 
            // ClientAddressValueLbl
            // 
            this.ClientAddressValueLbl.AutoSize = true;
            this.ClientAddressValueLbl.Location = new System.Drawing.Point(114, 97);
            this.ClientAddressValueLbl.Name = "ClientAddressValueLbl";
            this.ClientAddressValueLbl.Size = new System.Drawing.Size(112, 13);
            this.ClientAddressValueLbl.TabIndex = 9;
            this.ClientAddressValueLbl.Text = "ClientAddressValueLbl";
            // 
            // PhysicalAddressValueLbl
            // 
            this.PhysicalAddressValueLbl.AutoSize = true;
            this.PhysicalAddressValueLbl.Location = new System.Drawing.Point(114, 51);
            this.PhysicalAddressValueLbl.Name = "PhysicalAddressValueLbl";
            this.PhysicalAddressValueLbl.Size = new System.Drawing.Size(125, 13);
            this.PhysicalAddressValueLbl.TabIndex = 5;
            this.PhysicalAddressValueLbl.Text = "PhysicalAddressValueLbl";
            // 
            // ClientAddressLbl
            // 
            this.ClientAddressLbl.AutoSize = true;
            this.ClientAddressLbl.Location = new System.Drawing.Point(18, 97);
            this.ClientAddressLbl.Name = "ClientAddressLbl";
            this.ClientAddressLbl.Size = new System.Drawing.Size(77, 13);
            this.ClientAddressLbl.TabIndex = 8;
            this.ClientAddressLbl.Text = "Client Address:";
            // 
            // LogicalAddressLbl
            // 
            this.LogicalAddressLbl.AutoSize = true;
            this.LogicalAddressLbl.Location = new System.Drawing.Point(18, 74);
            this.LogicalAddressLbl.Name = "LogicalAddressLbl";
            this.LogicalAddressLbl.Size = new System.Drawing.Size(85, 13);
            this.LogicalAddressLbl.TabIndex = 6;
            this.LogicalAddressLbl.Text = "Logical Address:";
            // 
            // LogicalAddressValueLbl
            // 
            this.LogicalAddressValueLbl.AutoSize = true;
            this.LogicalAddressValueLbl.Location = new System.Drawing.Point(114, 74);
            this.LogicalAddressValueLbl.Name = "LogicalAddressValueLbl";
            this.LogicalAddressValueLbl.Size = new System.Drawing.Size(120, 13);
            this.LogicalAddressValueLbl.TabIndex = 7;
            this.LogicalAddressValueLbl.Text = "LogicalAddressValueLbl";
            // 
            // DeviceList
            // 
            this.DeviceList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.DeviceNameCH});
            this.DeviceList.Location = new System.Drawing.Point(424, 306);
            this.DeviceList.MultiSelect = false;
            this.DeviceList.Name = "DeviceList";
            this.DeviceList.Size = new System.Drawing.Size(134, 87);
            this.DeviceList.TabIndex = 19;
            this.DeviceList.UseCompatibleStateImageBehavior = false;
            this.DeviceList.View = System.Windows.Forms.View.Details;
            // 
            // DeviceNameCH
            // 
            this.DeviceNameCH.Text = "Name";
            this.DeviceNameCH.Width = 117;
            // 
            // ObjectPanelFrame
            // 
            this.ObjectPanelFrame.Location = new System.Drawing.Point(218, 306);
            this.ObjectPanelFrame.Name = "ObjectPanelFrame";
            this.ObjectPanelFrame.Size = new System.Drawing.Size(187, 142);
            this.ObjectPanelFrame.TabIndex = 18;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 536);
            this.Controls.Add(this.ObjectValueView);
            this.Controls.Add(this.DeviceInfoView);
            this.Controls.Add(this.DeviceList);
            this.Controls.Add(this.ObjectPanelFrame);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.TraceView);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Gurux COSEM Director Community Edition";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ListView.ResumeLayout(false);
            this.TreeView.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.DeviceInfoView.ResumeLayout(false);
            this.DeviceGb.ResumeLayout(false);
            this.DeviceGb.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewMnu;
        private System.Windows.Forms.ToolStripMenuItem OpenMnu;
        private System.Windows.Forms.ToolStripMenuItem SaveMnu;
        private System.Windows.Forms.ToolStripMenuItem ExitMnu;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutMnu;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem RefreshMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PropertiesMnu;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ConnectMnu;
        private System.Windows.Forms.ToolStripMenuItem DisconnectMnu;
        private System.Windows.Forms.ToolStripMenuItem ReadMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem ViewToolbarMnu;
        private System.Windows.Forms.ToolStripMenuItem ViewStatusbarMnu;
        private System.Windows.Forms.ToolStripStatusLabel StatusLbl;
        private System.Windows.Forms.ToolStripProgressBar ProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripMenuItem DeleteMnu;
        private System.Windows.Forms.ToolBarButton NewListTBtn;
        private System.Windows.Forms.ToolBarButton OpenTBtn;
        private System.Windows.Forms.ToolBarButton SaveTBtn;
        private System.Windows.Forms.ToolBarButton NewDeviceTBtn;
        private System.Windows.Forms.ToolBarButton Separator1TBtn;
        private System.Windows.Forms.ToolBarButton ConnectTBtn;
        private System.Windows.Forms.ToolBarButton MonitorTBtn;
        private System.Windows.Forms.ToolBarButton ReadAllTBtn;
        private System.Windows.Forms.ToolBarButton WriteAllTBtn;
        private System.Windows.Forms.ToolBarButton Separator2TBtn;
        private System.Windows.Forms.ToolBarButton PropertiesTBtn;
        private System.Windows.Forms.ToolBarButton RemoveTBtn;
        private System.Windows.Forms.ToolStripButton NewBtn;
        private System.Windows.Forms.ToolStripButton OpenBtn;
        private System.Windows.Forms.ToolStripButton SaveBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton OptionsBtn;
        private System.Windows.Forms.ToolStripButton ReadBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton DeleteBtn;
        private System.Windows.Forms.ToolStripButton ConnectBtn;
        private System.Windows.Forms.ToolStripMenuItem AddDeviceMnu;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem SaveAsMnu;
        private System.Windows.Forms.ToolStripMenuItem ContentsMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem ManufacturersMnu;
        private System.Windows.Forms.ToolStripMenuItem OBISCodesMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem LogMnu;
        private System.Windows.Forms.ToolStripMenuItem ClearLogMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ConnectCMnu;
        private System.Windows.Forms.ToolStripMenuItem AddDeviceCMnu;
        private System.Windows.Forms.ToolStripMenuItem DisconnectCMnu;
        private System.Windows.Forms.ToolStripMenuItem ReadCMnu;
        private System.Windows.Forms.ToolStripSeparator Line1CMnu;
        private System.Windows.Forms.ToolStripMenuItem DeleteCMnu;
        private System.Windows.Forms.ToolStripMenuItem WriteMnu;
        private System.Windows.Forms.ToolStripButton WriteBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;

        private System.Windows.Forms.ToolStripMenuItem updateManufactureSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem PropertiesCMnu;

        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem CancelBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem RecentFilesMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem GroupsMnu;
        private System.Windows.Forms.TabPage ListView;
        private System.Windows.Forms.ListView ObjectList;
        private System.Windows.Forms.ColumnHeader DescriptionColumnHeader;
        private System.Windows.Forms.TabPage TreeView;
        private System.Windows.Forms.TreeView ObjectTree;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem12;
        private System.Windows.Forms.ToolStripMenuItem ObjectTreeMnu;
        private System.Windows.Forms.ToolStripMenuItem ObjectListMnu;
        private System.Windows.Forms.ToolStripMenuItem LibraryVersionsMenu;
        private System.Windows.Forms.ToolStripMenuItem TraceMnu;
        private System.Windows.Forms.TextBox TraceView;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.ListView ObjectValueView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Panel DeviceInfoView;
        private System.Windows.Forms.GroupBox DeviceGb;
        private System.Windows.Forms.Label ManufacturerLbl;
        private System.Windows.Forms.Label StatusValueLbl;
        private System.Windows.Forms.Label ManufacturerValueLbl;
        private System.Windows.Forms.Label DeviceStateLbl;
        private System.Windows.Forms.Label PhysicalAddressLbl;
        private System.Windows.Forms.Label ClientAddressValueLbl;
        private System.Windows.Forms.Label PhysicalAddressValueLbl;
        private System.Windows.Forms.Label ClientAddressLbl;
        private System.Windows.Forms.Label LogicalAddressLbl;
        private System.Windows.Forms.Label LogicalAddressValueLbl;
        private System.Windows.Forms.ListView DeviceList;
        private System.Windows.Forms.ColumnHeader DeviceNameCH;
        private System.Windows.Forms.Panel ObjectPanelFrame;
    }
}

