//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/DevicePropertiesForm.Designer.cs $
//
// Version:         $Revision: 8655 $,
//                  $Date: 2016-07-20 15:55:25 +0300 (ke, 20 heinä 2016) $
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


namespace GXDLMSDirector
{
    partial class DevicePropertiesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevicePropertiesForm));
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.InitialSettingsBtn = new System.Windows.Forms.Button();
            this.SupportedServicesTab = new System.Windows.Forms.TabPage();
            this.SupportedServicesGrid = new System.Windows.Forms.PropertyGrid();
            this.CipheringTab = new System.Windows.Forms.TabPage();
            this.SecurityLbl = new System.Windows.Forms.Label();
            this.SystemTitleTB = new System.Windows.Forms.TextBox();
            this.HexRB = new System.Windows.Forms.RadioButton();
            this.AsciiRB = new System.Windows.Forms.RadioButton();
            this.SystemtitleLbl = new System.Windows.Forms.Label();
            this.BlockCipherKeyLbl = new System.Windows.Forms.Label();
            this.BlockCipherKeyTB = new System.Windows.Forms.TextBox();
            this.AuthenticationKeyLbl = new System.Windows.Forms.Label();
            this.AuthenticationKeyTB = new System.Windows.Forms.TextBox();
            this.SecurityCB = new System.Windows.Forms.ComboBox();
            this.DeviceSettingsTab = new System.Windows.Forms.TabPage();
            this.TerminalSettingsGB = new System.Windows.Forms.GroupBox();
            this.TerminalPhoneNumberLbl = new System.Windows.Forms.Label();
            this.TerminalPhoneNumberTB = new System.Windows.Forms.TextBox();
            this.TerminalPortCB = new System.Windows.Forms.ComboBox();
            this.TerminalAdvancedBtn = new System.Windows.Forms.Button();
            this.TerminalPortLbl = new System.Windows.Forms.Label();
            this.NetworkSettingsGB = new System.Windows.Forms.GroupBox();
            this.HostLbl = new System.Windows.Forms.Label();
            this.HostNameTB = new System.Windows.Forms.TextBox();
            this.PortLbl = new System.Windows.Forms.Label();
            this.PortTB = new System.Windows.Forms.TextBox();
            this.UseRemoteSerialCB = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.NetProtocolCB = new System.Windows.Forms.ComboBox();
            this.SerialSettingsGB = new System.Windows.Forms.GroupBox();
            this.SerialPortCB = new System.Windows.Forms.ComboBox();
            this.SerialPortLbl = new System.Windows.Forms.Label();
            this.AdvancedBtn = new System.Windows.Forms.Button();
            this.MaximumBaudRateCB = new System.Windows.Forms.ComboBox();
            this.UseMaximumBaudRateCB = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.MediasCB = new System.Windows.Forms.ComboBox();
            this.NameLbl = new System.Windows.Forms.Label();
            this.NameTB = new System.Windows.Forms.TextBox();
            this.AuthenticationLbl = new System.Windows.Forms.Label();
            this.AuthenticationCB = new System.Windows.Forms.ComboBox();
            this.PasswordLbl = new System.Windows.Forms.Label();
            this.PasswordTB = new System.Windows.Forms.TextBox();
            this.ManufacturerLbl = new System.Windows.Forms.Label();
            this.ManufacturerCB = new System.Windows.Forms.ComboBox();
            this.WaitTimeLbl = new System.Windows.Forms.Label();
            this.WaitTimeTB = new System.Windows.Forms.NumericUpDown();
            this.PhysicalServerAddressLbl = new System.Windows.Forms.Label();
            this.PhysicalServerAddressTB = new System.Windows.Forms.NumericUpDown();
            this.LogicalServerAddressLbl = new System.Windows.Forms.Label();
            this.LogicalServerAddressTB = new System.Windows.Forms.NumericUpDown();
            this.StartProtocolLbl = new System.Windows.Forms.Label();
            this.StartProtocolCB = new System.Windows.Forms.ComboBox();
            this.ClientAddTB = new System.Windows.Forms.NumericUpDown();
            this.ClientAddLbl = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ServerAddressTypeCB = new System.Windows.Forms.ComboBox();
            this.UseLNCB = new System.Windows.Forms.CheckBox();
            this.VerboseModeCB = new System.Windows.Forms.CheckBox();
            this.DeviceTab = new System.Windows.Forms.TabControl();
            this.SupportedServicesTab.SuspendLayout();
            this.CipheringTab.SuspendLayout();
            this.DeviceSettingsTab.SuspendLayout();
            this.TerminalSettingsGB.SuspendLayout();
            this.NetworkSettingsGB.SuspendLayout();
            this.SerialSettingsGB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WaitTimeTB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhysicalServerAddressTB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogicalServerAddressTB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClientAddTB)).BeginInit();
            this.DeviceTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(323, 375);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 15;
            this.CancelBtn.Text = "&Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Location = new System.Drawing.Point(242, 375);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 14;
            this.OKBtn.Text = "&OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // InitialSettingsBtn
            // 
            this.InitialSettingsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.InitialSettingsBtn.Location = new System.Drawing.Point(10, 375);
            this.InitialSettingsBtn.Name = "InitialSettingsBtn";
            this.InitialSettingsBtn.Size = new System.Drawing.Size(117, 23);
            this.InitialSettingsBtn.TabIndex = 16;
            this.InitialSettingsBtn.Text = "Initial settings...";
            this.InitialSettingsBtn.UseVisualStyleBackColor = true;
            this.InitialSettingsBtn.Click += new System.EventHandler(this.InitialSettingsBtn_Click);
            // 
            // SupportedServicesTab
            // 
            this.SupportedServicesTab.Controls.Add(this.SupportedServicesGrid);
            this.SupportedServicesTab.Location = new System.Drawing.Point(4, 22);
            this.SupportedServicesTab.Name = "SupportedServicesTab";
            this.SupportedServicesTab.Padding = new System.Windows.Forms.Padding(3);
            this.SupportedServicesTab.Size = new System.Drawing.Size(403, 341);
            this.SupportedServicesTab.TabIndex = 1;
            this.SupportedServicesTab.Text = "Supported Services";
            this.SupportedServicesTab.UseVisualStyleBackColor = true;
            // 
            // SupportedServicesGrid
            // 
            this.SupportedServicesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SupportedServicesGrid.HelpVisible = false;
            this.SupportedServicesGrid.Location = new System.Drawing.Point(3, 3);
            this.SupportedServicesGrid.Name = "SupportedServicesGrid";
            this.SupportedServicesGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.SupportedServicesGrid.Size = new System.Drawing.Size(397, 335);
            this.SupportedServicesGrid.TabIndex = 0;
            this.SupportedServicesGrid.ToolbarVisible = false;
            // 
            // CipheringTab
            // 
            this.CipheringTab.Controls.Add(this.SecurityCB);
            this.CipheringTab.Controls.Add(this.AuthenticationKeyTB);
            this.CipheringTab.Controls.Add(this.BlockCipherKeyTB);
            this.CipheringTab.Controls.Add(this.SystemTitleTB);
            this.CipheringTab.Controls.Add(this.AuthenticationKeyLbl);
            this.CipheringTab.Controls.Add(this.BlockCipherKeyLbl);
            this.CipheringTab.Controls.Add(this.SystemtitleLbl);
            this.CipheringTab.Controls.Add(this.AsciiRB);
            this.CipheringTab.Controls.Add(this.HexRB);
            this.CipheringTab.Controls.Add(this.SecurityLbl);
            this.CipheringTab.Location = new System.Drawing.Point(4, 22);
            this.CipheringTab.Name = "CipheringTab";
            this.CipheringTab.Size = new System.Drawing.Size(403, 341);
            this.CipheringTab.TabIndex = 2;
            this.CipheringTab.Text = "Ciphering";
            this.CipheringTab.UseVisualStyleBackColor = true;
            // 
            // SecurityLbl
            // 
            this.SecurityLbl.AutoSize = true;
            this.SecurityLbl.Location = new System.Drawing.Point(8, 6);
            this.SecurityLbl.Name = "SecurityLbl";
            this.SecurityLbl.Size = new System.Drawing.Size(48, 13);
            this.SecurityLbl.TabIndex = 40;
            this.SecurityLbl.Text = "Security:";
            // 
            // SystemTitleTB
            // 
            this.SystemTitleTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SystemTitleTB.Location = new System.Drawing.Point(113, 28);
            this.SystemTitleTB.Name = "SystemTitleTB";
            this.SystemTitleTB.Size = new System.Drawing.Size(226, 20);
            this.SystemTitleTB.TabIndex = 37;
            this.SystemTitleTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ValidateHex);
            // 
            // HexRB
            // 
            this.HexRB.AutoSize = true;
            this.HexRB.Checked = true;
            this.HexRB.Location = new System.Drawing.Point(15, 110);
            this.HexRB.Name = "HexRB";
            this.HexRB.Size = new System.Drawing.Size(44, 17);
            this.HexRB.TabIndex = 41;
            this.HexRB.TabStop = true;
            this.HexRB.Text = "Hex";
            this.HexRB.UseVisualStyleBackColor = true;
            this.HexRB.CheckedChanged += new System.EventHandler(this.HexRB_CheckedChanged);
            // 
            // AsciiRB
            // 
            this.AsciiRB.AutoSize = true;
            this.AsciiRB.Location = new System.Drawing.Point(15, 133);
            this.AsciiRB.Name = "AsciiRB";
            this.AsciiRB.Size = new System.Drawing.Size(52, 17);
            this.AsciiRB.TabIndex = 42;
            this.AsciiRB.Text = "ASCII";
            this.AsciiRB.UseVisualStyleBackColor = true;
            // 
            // SystemtitleLbl
            // 
            this.SystemtitleLbl.AutoSize = true;
            this.SystemtitleLbl.Location = new System.Drawing.Point(8, 32);
            this.SystemtitleLbl.Name = "SystemtitleLbl";
            this.SystemtitleLbl.Size = new System.Drawing.Size(63, 13);
            this.SystemtitleLbl.TabIndex = 43;
            this.SystemtitleLbl.Text = "System title:";
            // 
            // BlockCipherKeyLbl
            // 
            this.BlockCipherKeyLbl.AutoSize = true;
            this.BlockCipherKeyLbl.Location = new System.Drawing.Point(8, 58);
            this.BlockCipherKeyLbl.Name = "BlockCipherKeyLbl";
            this.BlockCipherKeyLbl.Size = new System.Drawing.Size(91, 13);
            this.BlockCipherKeyLbl.TabIndex = 44;
            this.BlockCipherKeyLbl.Text = "Block Cipher Key:";
            // 
            // BlockCipherKeyTB
            // 
            this.BlockCipherKeyTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BlockCipherKeyTB.Location = new System.Drawing.Point(113, 55);
            this.BlockCipherKeyTB.Name = "BlockCipherKeyTB";
            this.BlockCipherKeyTB.Size = new System.Drawing.Size(226, 20);
            this.BlockCipherKeyTB.TabIndex = 38;
            this.BlockCipherKeyTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ValidateHex);
            // 
            // AuthenticationKeyLbl
            // 
            this.AuthenticationKeyLbl.AutoSize = true;
            this.AuthenticationKeyLbl.Location = new System.Drawing.Point(8, 84);
            this.AuthenticationKeyLbl.Name = "AuthenticationKeyLbl";
            this.AuthenticationKeyLbl.Size = new System.Drawing.Size(99, 13);
            this.AuthenticationKeyLbl.TabIndex = 45;
            this.AuthenticationKeyLbl.Text = "Authentication Key:";
            // 
            // AuthenticationKeyTB
            // 
            this.AuthenticationKeyTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AuthenticationKeyTB.Location = new System.Drawing.Point(113, 81);
            this.AuthenticationKeyTB.Name = "AuthenticationKeyTB";
            this.AuthenticationKeyTB.Size = new System.Drawing.Size(226, 20);
            this.AuthenticationKeyTB.TabIndex = 39;
            this.AuthenticationKeyTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ValidateHex);
            // 
            // SecurityCB
            // 
            this.SecurityCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SecurityCB.Location = new System.Drawing.Point(113, 3);
            this.SecurityCB.Name = "SecurityCB";
            this.SecurityCB.Size = new System.Drawing.Size(226, 21);
            this.SecurityCB.TabIndex = 36;
            this.SecurityCB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.SecurityCB_DrawItem);
            // 
            // DeviceSettingsTab
            // 
            this.DeviceSettingsTab.Controls.Add(this.VerboseModeCB);
            this.DeviceSettingsTab.Controls.Add(this.UseLNCB);
            this.DeviceSettingsTab.Controls.Add(this.ServerAddressTypeCB);
            this.DeviceSettingsTab.Controls.Add(this.label3);
            this.DeviceSettingsTab.Controls.Add(this.ClientAddLbl);
            this.DeviceSettingsTab.Controls.Add(this.ClientAddTB);
            this.DeviceSettingsTab.Controls.Add(this.StartProtocolCB);
            this.DeviceSettingsTab.Controls.Add(this.StartProtocolLbl);
            this.DeviceSettingsTab.Controls.Add(this.LogicalServerAddressTB);
            this.DeviceSettingsTab.Controls.Add(this.LogicalServerAddressLbl);
            this.DeviceSettingsTab.Controls.Add(this.PhysicalServerAddressTB);
            this.DeviceSettingsTab.Controls.Add(this.PhysicalServerAddressLbl);
            this.DeviceSettingsTab.Controls.Add(this.WaitTimeTB);
            this.DeviceSettingsTab.Controls.Add(this.WaitTimeLbl);
            this.DeviceSettingsTab.Controls.Add(this.ManufacturerCB);
            this.DeviceSettingsTab.Controls.Add(this.ManufacturerLbl);
            this.DeviceSettingsTab.Controls.Add(this.PasswordTB);
            this.DeviceSettingsTab.Controls.Add(this.NameTB);
            this.DeviceSettingsTab.Controls.Add(this.PasswordLbl);
            this.DeviceSettingsTab.Controls.Add(this.AuthenticationCB);
            this.DeviceSettingsTab.Controls.Add(this.AuthenticationLbl);
            this.DeviceSettingsTab.Controls.Add(this.NameLbl);
            this.DeviceSettingsTab.Controls.Add(this.MediasCB);
            this.DeviceSettingsTab.Controls.Add(this.label1);
            this.DeviceSettingsTab.Controls.Add(this.SerialSettingsGB);
            this.DeviceSettingsTab.Controls.Add(this.NetworkSettingsGB);
            this.DeviceSettingsTab.Controls.Add(this.TerminalSettingsGB);
            this.DeviceSettingsTab.Location = new System.Drawing.Point(4, 22);
            this.DeviceSettingsTab.Name = "DeviceSettingsTab";
            this.DeviceSettingsTab.Padding = new System.Windows.Forms.Padding(3);
            this.DeviceSettingsTab.Size = new System.Drawing.Size(403, 341);
            this.DeviceSettingsTab.TabIndex = 0;
            this.DeviceSettingsTab.Text = "Device Settings";
            this.DeviceSettingsTab.UseVisualStyleBackColor = true;
            // 
            // TerminalSettingsGB
            // 
            this.TerminalSettingsGB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TerminalSettingsGB.Controls.Add(this.TerminalPortLbl);
            this.TerminalSettingsGB.Controls.Add(this.TerminalAdvancedBtn);
            this.TerminalSettingsGB.Controls.Add(this.TerminalPortCB);
            this.TerminalSettingsGB.Controls.Add(this.TerminalPhoneNumberTB);
            this.TerminalSettingsGB.Controls.Add(this.TerminalPhoneNumberLbl);
            this.TerminalSettingsGB.Location = new System.Drawing.Point(165, 240);
            this.TerminalSettingsGB.Name = "TerminalSettingsGB";
            this.TerminalSettingsGB.Size = new System.Drawing.Size(362, 99);
            this.TerminalSettingsGB.TabIndex = 46;
            this.TerminalSettingsGB.TabStop = false;
            this.TerminalSettingsGB.Text = "Settings";
            // 
            // TerminalPhoneNumberLbl
            // 
            this.TerminalPhoneNumberLbl.AutoSize = true;
            this.TerminalPhoneNumberLbl.Location = new System.Drawing.Point(12, 22);
            this.TerminalPhoneNumberLbl.Name = "TerminalPhoneNumberLbl";
            this.TerminalPhoneNumberLbl.Size = new System.Drawing.Size(81, 13);
            this.TerminalPhoneNumberLbl.TabIndex = 9;
            this.TerminalPhoneNumberLbl.Text = "Phone Number:";
            // 
            // TerminalPhoneNumberTB
            // 
            this.TerminalPhoneNumberTB.Location = new System.Drawing.Point(101, 19);
            this.TerminalPhoneNumberTB.Name = "TerminalPhoneNumberTB";
            this.TerminalPhoneNumberTB.Size = new System.Drawing.Size(274, 20);
            this.TerminalPhoneNumberTB.TabIndex = 11;
            // 
            // TerminalPortCB
            // 
            this.TerminalPortCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TerminalPortCB.FormattingEnabled = true;
            this.TerminalPortCB.Location = new System.Drawing.Point(101, 45);
            this.TerminalPortCB.Name = "TerminalPortCB";
            this.TerminalPortCB.Size = new System.Drawing.Size(139, 21);
            this.TerminalPortCB.TabIndex = 12;
            // 
            // TerminalAdvancedBtn
            // 
            this.TerminalAdvancedBtn.Location = new System.Drawing.Point(261, 43);
            this.TerminalAdvancedBtn.Name = "TerminalAdvancedBtn";
            this.TerminalAdvancedBtn.Size = new System.Drawing.Size(75, 23);
            this.TerminalAdvancedBtn.TabIndex = 13;
            this.TerminalAdvancedBtn.Text = "Advanced...";
            this.TerminalAdvancedBtn.UseVisualStyleBackColor = true;
            this.TerminalAdvancedBtn.Click += new System.EventHandler(this.TerminalAdvancedBtn_Click);
            // 
            // TerminalPortLbl
            // 
            this.TerminalPortLbl.AutoSize = true;
            this.TerminalPortLbl.Location = new System.Drawing.Point(14, 48);
            this.TerminalPortLbl.Name = "TerminalPortLbl";
            this.TerminalPortLbl.Size = new System.Drawing.Size(58, 13);
            this.TerminalPortLbl.TabIndex = 14;
            this.TerminalPortLbl.Text = "Serial Port:";
            // 
            // NetworkSettingsGB
            // 
            this.NetworkSettingsGB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NetworkSettingsGB.Controls.Add(this.NetProtocolCB);
            this.NetworkSettingsGB.Controls.Add(this.label2);
            this.NetworkSettingsGB.Controls.Add(this.UseRemoteSerialCB);
            this.NetworkSettingsGB.Controls.Add(this.PortTB);
            this.NetworkSettingsGB.Controls.Add(this.PortLbl);
            this.NetworkSettingsGB.Controls.Add(this.HostNameTB);
            this.NetworkSettingsGB.Controls.Add(this.HostLbl);
            this.NetworkSettingsGB.Location = new System.Drawing.Point(6, 238);
            this.NetworkSettingsGB.Name = "NetworkSettingsGB";
            this.NetworkSettingsGB.Size = new System.Drawing.Size(388, 99);
            this.NetworkSettingsGB.TabIndex = 34;
            this.NetworkSettingsGB.TabStop = false;
            this.NetworkSettingsGB.Text = "Settings";
            // 
            // HostLbl
            // 
            this.HostLbl.AutoSize = true;
            this.HostLbl.Location = new System.Drawing.Point(12, 22);
            this.HostLbl.Name = "HostLbl";
            this.HostLbl.Size = new System.Drawing.Size(61, 13);
            this.HostLbl.TabIndex = 9;
            this.HostLbl.Text = "Host name:";
            // 
            // HostNameTB
            // 
            this.HostNameTB.Location = new System.Drawing.Point(101, 19);
            this.HostNameTB.Name = "HostNameTB";
            this.HostNameTB.Size = new System.Drawing.Size(274, 20);
            this.HostNameTB.TabIndex = 11;
            // 
            // PortLbl
            // 
            this.PortLbl.AutoSize = true;
            this.PortLbl.Location = new System.Drawing.Point(12, 48);
            this.PortLbl.Name = "PortLbl";
            this.PortLbl.Size = new System.Drawing.Size(29, 13);
            this.PortLbl.TabIndex = 11;
            this.PortLbl.Text = "Port:";
            // 
            // PortTB
            // 
            this.PortTB.Location = new System.Drawing.Point(101, 45);
            this.PortTB.Name = "PortTB";
            this.PortTB.Size = new System.Drawing.Size(274, 20);
            this.PortTB.TabIndex = 12;
            // 
            // UseRemoteSerialCB
            // 
            this.UseRemoteSerialCB.AutoSize = true;
            this.UseRemoteSerialCB.Location = new System.Drawing.Point(204, 71);
            this.UseRemoteSerialCB.Name = "UseRemoteSerialCB";
            this.UseRemoteSerialCB.Size = new System.Drawing.Size(179, 17);
            this.UseRemoteSerialCB.TabIndex = 14;
            this.UseRemoteSerialCB.Text = "Use Serial port through ethernet.";
            this.UseRemoteSerialCB.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Protocol:";
            // 
            // NetProtocolCB
            // 
            this.NetProtocolCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NetProtocolCB.FormattingEnabled = true;
            this.NetProtocolCB.Location = new System.Drawing.Point(101, 70);
            this.NetProtocolCB.Name = "NetProtocolCB";
            this.NetProtocolCB.Size = new System.Drawing.Size(85, 21);
            this.NetProtocolCB.TabIndex = 13;
            // 
            // SerialSettingsGB
            // 
            this.SerialSettingsGB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SerialSettingsGB.Controls.Add(this.UseMaximumBaudRateCB);
            this.SerialSettingsGB.Controls.Add(this.MaximumBaudRateCB);
            this.SerialSettingsGB.Controls.Add(this.AdvancedBtn);
            this.SerialSettingsGB.Controls.Add(this.SerialPortLbl);
            this.SerialSettingsGB.Controls.Add(this.SerialPortCB);
            this.SerialSettingsGB.Location = new System.Drawing.Point(82, 240);
            this.SerialSettingsGB.Name = "SerialSettingsGB";
            this.SerialSettingsGB.Size = new System.Drawing.Size(312, 89);
            this.SerialSettingsGB.TabIndex = 35;
            this.SerialSettingsGB.TabStop = false;
            this.SerialSettingsGB.Text = "Settings";
            // 
            // SerialPortCB
            // 
            this.SerialPortCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SerialPortCB.FormattingEnabled = true;
            this.SerialPortCB.Location = new System.Drawing.Point(103, 19);
            this.SerialPortCB.Name = "SerialPortCB";
            this.SerialPortCB.Size = new System.Drawing.Size(139, 21);
            this.SerialPortCB.TabIndex = 11;
            // 
            // SerialPortLbl
            // 
            this.SerialPortLbl.AutoSize = true;
            this.SerialPortLbl.Location = new System.Drawing.Point(7, 22);
            this.SerialPortLbl.Name = "SerialPortLbl";
            this.SerialPortLbl.Size = new System.Drawing.Size(58, 13);
            this.SerialPortLbl.TabIndex = 11;
            this.SerialPortLbl.Text = "Serial Port:";
            // 
            // AdvancedBtn
            // 
            this.AdvancedBtn.Location = new System.Drawing.Point(263, 17);
            this.AdvancedBtn.Name = "AdvancedBtn";
            this.AdvancedBtn.Size = new System.Drawing.Size(87, 23);
            this.AdvancedBtn.TabIndex = 12;
            this.AdvancedBtn.Text = "Advanced...";
            this.AdvancedBtn.UseVisualStyleBackColor = true;
            this.AdvancedBtn.Click += new System.EventHandler(this.AdvancedBtn_Click);
            // 
            // MaximumBaudRateCB
            // 
            this.MaximumBaudRateCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MaximumBaudRateCB.FormattingEnabled = true;
            this.MaximumBaudRateCB.Location = new System.Drawing.Point(263, 50);
            this.MaximumBaudRateCB.Name = "MaximumBaudRateCB";
            this.MaximumBaudRateCB.Size = new System.Drawing.Size(87, 21);
            this.MaximumBaudRateCB.TabIndex = 54;
            // 
            // UseMaximumBaudRateCB
            // 
            this.UseMaximumBaudRateCB.AutoSize = true;
            this.UseMaximumBaudRateCB.Location = new System.Drawing.Point(7, 50);
            this.UseMaximumBaudRateCB.Name = "UseMaximumBaudRateCB";
            this.UseMaximumBaudRateCB.Size = new System.Drawing.Size(146, 17);
            this.UseMaximumBaudRateCB.TabIndex = 55;
            this.UseMaximumBaudRateCB.Text = "Use Maximum Baud Rate";
            this.UseMaximumBaudRateCB.UseVisualStyleBackColor = true;
            this.UseMaximumBaudRateCB.CheckedChanged += new System.EventHandler(this.UseMaximumBaudRateCB_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(208, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Media:";
            // 
            // MediasCB
            // 
            this.MediasCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MediasCB.FormattingEnabled = true;
            this.MediasCB.Location = new System.Drawing.Point(298, 58);
            this.MediasCB.Name = "MediasCB";
            this.MediasCB.Size = new System.Drawing.Size(85, 21);
            this.MediasCB.TabIndex = 3;
            this.MediasCB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.MediasCB_DrawItem);
            this.MediasCB.SelectedIndexChanged += new System.EventHandler(this.MediasCB_SelectedIndexChanged);
            // 
            // NameLbl
            // 
            this.NameLbl.AutoSize = true;
            this.NameLbl.Location = new System.Drawing.Point(8, 8);
            this.NameLbl.Name = "NameLbl";
            this.NameLbl.Size = new System.Drawing.Size(38, 13);
            this.NameLbl.TabIndex = 32;
            this.NameLbl.Text = "Name:";
            // 
            // NameTB
            // 
            this.NameTB.Location = new System.Drawing.Point(109, 5);
            this.NameTB.Name = "NameTB";
            this.NameTB.Size = new System.Drawing.Size(274, 20);
            this.NameTB.TabIndex = 0;
            // 
            // AuthenticationLbl
            // 
            this.AuthenticationLbl.AutoSize = true;
            this.AuthenticationLbl.Location = new System.Drawing.Point(6, 91);
            this.AuthenticationLbl.Name = "AuthenticationLbl";
            this.AuthenticationLbl.Size = new System.Drawing.Size(78, 13);
            this.AuthenticationLbl.TabIndex = 33;
            this.AuthenticationLbl.Text = "Authentication:";
            // 
            // AuthenticationCB
            // 
            this.AuthenticationCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AuthenticationCB.FormattingEnabled = true;
            this.AuthenticationCB.Location = new System.Drawing.Point(109, 85);
            this.AuthenticationCB.Name = "AuthenticationCB";
            this.AuthenticationCB.Size = new System.Drawing.Size(87, 21);
            this.AuthenticationCB.TabIndex = 4;
            this.AuthenticationCB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.AuthenticationCB_DrawItem);
            this.AuthenticationCB.SelectedIndexChanged += new System.EventHandler(this.AuthenticationCB_SelectedIndexChanged);
            // 
            // PasswordLbl
            // 
            this.PasswordLbl.AutoSize = true;
            this.PasswordLbl.Location = new System.Drawing.Point(6, 119);
            this.PasswordLbl.Name = "PasswordLbl";
            this.PasswordLbl.Size = new System.Drawing.Size(56, 13);
            this.PasswordLbl.TabIndex = 36;
            this.PasswordLbl.Text = "Password:";
            // 
            // PasswordTB
            // 
            this.PasswordTB.Location = new System.Drawing.Point(109, 115);
            this.PasswordTB.Name = "PasswordTB";
            this.PasswordTB.PasswordChar = '*';
            this.PasswordTB.Size = new System.Drawing.Size(87, 20);
            this.PasswordTB.TabIndex = 6;
            // 
            // ManufacturerLbl
            // 
            this.ManufacturerLbl.AutoSize = true;
            this.ManufacturerLbl.Location = new System.Drawing.Point(8, 36);
            this.ManufacturerLbl.Name = "ManufacturerLbl";
            this.ManufacturerLbl.Size = new System.Drawing.Size(73, 13);
            this.ManufacturerLbl.TabIndex = 37;
            this.ManufacturerLbl.Text = "Manufacturer:";
            // 
            // ManufacturerCB
            // 
            this.ManufacturerCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ManufacturerCB.FormattingEnabled = true;
            this.ManufacturerCB.Location = new System.Drawing.Point(110, 31);
            this.ManufacturerCB.Name = "ManufacturerCB";
            this.ManufacturerCB.Size = new System.Drawing.Size(273, 21);
            this.ManufacturerCB.TabIndex = 1;
            this.ManufacturerCB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ManufacturerCB_DrawItem);
            this.ManufacturerCB.SelectedIndexChanged += new System.EventHandler(this.ManufacturerCB_SelectedIndexChanged);
            // 
            // WaitTimeLbl
            // 
            this.WaitTimeLbl.AutoSize = true;
            this.WaitTimeLbl.Location = new System.Drawing.Point(6, 148);
            this.WaitTimeLbl.Name = "WaitTimeLbl";
            this.WaitTimeLbl.Size = new System.Drawing.Size(58, 13);
            this.WaitTimeLbl.TabIndex = 38;
            this.WaitTimeLbl.Text = "Wait Time:";
            // 
            // WaitTimeTB
            // 
            this.WaitTimeTB.Location = new System.Drawing.Point(109, 143);
            this.WaitTimeTB.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.WaitTimeTB.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.WaitTimeTB.Name = "WaitTimeTB";
            this.WaitTimeTB.Size = new System.Drawing.Size(85, 20);
            this.WaitTimeTB.TabIndex = 7;
            this.WaitTimeTB.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // PhysicalServerAddressLbl
            // 
            this.PhysicalServerAddressLbl.AutoSize = true;
            this.PhysicalServerAddressLbl.Location = new System.Drawing.Point(6, 199);
            this.PhysicalServerAddressLbl.Name = "PhysicalServerAddressLbl";
            this.PhysicalServerAddressLbl.Size = new System.Drawing.Size(83, 13);
            this.PhysicalServerAddressLbl.TabIndex = 39;
            this.PhysicalServerAddressLbl.Text = "Physical Server:";
            // 
            // PhysicalServerAddressTB
            // 
            this.PhysicalServerAddressTB.Hexadecimal = true;
            this.PhysicalServerAddressTB.Location = new System.Drawing.Point(108, 194);
            this.PhysicalServerAddressTB.Maximum = new decimal(new int[] {
            0,
            1,
            0,
            0});
            this.PhysicalServerAddressTB.Name = "PhysicalServerAddressTB";
            this.PhysicalServerAddressTB.Size = new System.Drawing.Size(85, 20);
            this.PhysicalServerAddressTB.TabIndex = 10;
            this.PhysicalServerAddressTB.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // LogicalServerAddressLbl
            // 
            this.LogicalServerAddressLbl.AutoSize = true;
            this.LogicalServerAddressLbl.Location = new System.Drawing.Point(206, 199);
            this.LogicalServerAddressLbl.Name = "LogicalServerAddressLbl";
            this.LogicalServerAddressLbl.Size = new System.Drawing.Size(78, 13);
            this.LogicalServerAddressLbl.TabIndex = 41;
            this.LogicalServerAddressLbl.Text = "Logical Server:";
            // 
            // LogicalServerAddressTB
            // 
            this.LogicalServerAddressTB.Hexadecimal = true;
            this.LogicalServerAddressTB.Location = new System.Drawing.Point(298, 197);
            this.LogicalServerAddressTB.Maximum = new decimal(new int[] {
            0,
            1,
            0,
            0});
            this.LogicalServerAddressTB.Name = "LogicalServerAddressTB";
            this.LogicalServerAddressTB.Size = new System.Drawing.Size(85, 20);
            this.LogicalServerAddressTB.TabIndex = 11;
            this.LogicalServerAddressTB.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // StartProtocolLbl
            // 
            this.StartProtocolLbl.AutoSize = true;
            this.StartProtocolLbl.Location = new System.Drawing.Point(8, 61);
            this.StartProtocolLbl.Name = "StartProtocolLbl";
            this.StartProtocolLbl.Size = new System.Drawing.Size(74, 13);
            this.StartProtocolLbl.TabIndex = 43;
            this.StartProtocolLbl.Text = "Start Protocol:";
            // 
            // StartProtocolCB
            // 
            this.StartProtocolCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.StartProtocolCB.FormattingEnabled = true;
            this.StartProtocolCB.Location = new System.Drawing.Point(111, 58);
            this.StartProtocolCB.Name = "StartProtocolCB";
            this.StartProtocolCB.Size = new System.Drawing.Size(85, 21);
            this.StartProtocolCB.TabIndex = 2;
            this.StartProtocolCB.SelectedIndexChanged += new System.EventHandler(this.StartProtocolCB_SelectedIndexChanged);
            // 
            // ClientAddTB
            // 
            this.ClientAddTB.Hexadecimal = true;
            this.ClientAddTB.Location = new System.Drawing.Point(298, 144);
            this.ClientAddTB.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.ClientAddTB.Name = "ClientAddTB";
            this.ClientAddTB.Size = new System.Drawing.Size(85, 20);
            this.ClientAddTB.TabIndex = 8;
            this.ClientAddTB.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // ClientAddLbl
            // 
            this.ClientAddLbl.AutoSize = true;
            this.ClientAddLbl.Location = new System.Drawing.Point(205, 147);
            this.ClientAddLbl.Name = "ClientAddLbl";
            this.ClientAddLbl.Size = new System.Drawing.Size(77, 13);
            this.ClientAddLbl.TabIndex = 45;
            this.ClientAddLbl.Text = "Client Address:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 48;
            this.label3.Text = "Address Type:";
            // 
            // ServerAddressTypeCB
            // 
            this.ServerAddressTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ServerAddressTypeCB.FormattingEnabled = true;
            this.ServerAddressTypeCB.Location = new System.Drawing.Point(107, 169);
            this.ServerAddressTypeCB.Name = "ServerAddressTypeCB";
            this.ServerAddressTypeCB.Size = new System.Drawing.Size(141, 21);
            this.ServerAddressTypeCB.TabIndex = 9;
            this.ServerAddressTypeCB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ServerAddressTypeCB_DrawItem);
            // 
            // UseLNCB
            // 
            this.UseLNCB.AutoSize = true;
            this.UseLNCB.Location = new System.Drawing.Point(211, 87);
            this.UseLNCB.Name = "UseLNCB";
            this.UseLNCB.Size = new System.Drawing.Size(152, 17);
            this.UseLNCB.TabIndex = 5;
            this.UseLNCB.Text = "Logical Name Referencing";
            this.UseLNCB.UseVisualStyleBackColor = true;
            // 
            // VerboseModeCB
            // 
            this.VerboseModeCB.AutoSize = true;
            this.VerboseModeCB.Location = new System.Drawing.Point(8, 220);
            this.VerboseModeCB.Name = "VerboseModeCB";
            this.VerboseModeCB.Size = new System.Drawing.Size(95, 17);
            this.VerboseModeCB.TabIndex = 12;
            this.VerboseModeCB.Text = "Verbose Mode";
            this.VerboseModeCB.UseVisualStyleBackColor = true;
            // 
            // DeviceTab
            // 
            this.DeviceTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DeviceTab.Controls.Add(this.DeviceSettingsTab);
            this.DeviceTab.Controls.Add(this.CipheringTab);
            this.DeviceTab.Controls.Add(this.SupportedServicesTab);
            this.DeviceTab.Location = new System.Drawing.Point(0, 0);
            this.DeviceTab.Name = "DeviceTab";
            this.DeviceTab.SelectedIndex = 0;
            this.DeviceTab.Size = new System.Drawing.Size(411, 367);
            this.DeviceTab.TabIndex = 0;
            // 
            // DevicePropertiesForm
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(410, 409);
            this.Controls.Add(this.InitialSettingsBtn);
            this.Controls.Add(this.DeviceTab);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.CancelBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DevicePropertiesForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Device Properties";
            this.SupportedServicesTab.ResumeLayout(false);
            this.CipheringTab.ResumeLayout(false);
            this.CipheringTab.PerformLayout();
            this.DeviceSettingsTab.ResumeLayout(false);
            this.DeviceSettingsTab.PerformLayout();
            this.TerminalSettingsGB.ResumeLayout(false);
            this.TerminalSettingsGB.PerformLayout();
            this.NetworkSettingsGB.ResumeLayout(false);
            this.NetworkSettingsGB.PerformLayout();
            this.SerialSettingsGB.ResumeLayout(false);
            this.SerialSettingsGB.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WaitTimeTB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhysicalServerAddressTB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogicalServerAddressTB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClientAddTB)).EndInit();
            this.DeviceTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button InitialSettingsBtn;
        private System.Windows.Forms.TabPage SupportedServicesTab;
        private System.Windows.Forms.PropertyGrid SupportedServicesGrid;
        private System.Windows.Forms.TabPage CipheringTab;
        private System.Windows.Forms.ComboBox SecurityCB;
        private System.Windows.Forms.TextBox AuthenticationKeyTB;
        private System.Windows.Forms.TextBox BlockCipherKeyTB;
        private System.Windows.Forms.TextBox SystemTitleTB;
        private System.Windows.Forms.Label AuthenticationKeyLbl;
        private System.Windows.Forms.Label BlockCipherKeyLbl;
        private System.Windows.Forms.Label SystemtitleLbl;
        private System.Windows.Forms.RadioButton AsciiRB;
        private System.Windows.Forms.RadioButton HexRB;
        private System.Windows.Forms.Label SecurityLbl;
        private System.Windows.Forms.TabPage DeviceSettingsTab;
        private System.Windows.Forms.CheckBox VerboseModeCB;
        private System.Windows.Forms.CheckBox UseLNCB;
        private System.Windows.Forms.ComboBox ServerAddressTypeCB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label ClientAddLbl;
        private System.Windows.Forms.NumericUpDown ClientAddTB;
        private System.Windows.Forms.ComboBox StartProtocolCB;
        private System.Windows.Forms.Label StartProtocolLbl;
        private System.Windows.Forms.NumericUpDown LogicalServerAddressTB;
        private System.Windows.Forms.Label LogicalServerAddressLbl;
        private System.Windows.Forms.NumericUpDown PhysicalServerAddressTB;
        private System.Windows.Forms.Label PhysicalServerAddressLbl;
        private System.Windows.Forms.NumericUpDown WaitTimeTB;
        private System.Windows.Forms.Label WaitTimeLbl;
        private System.Windows.Forms.ComboBox ManufacturerCB;
        private System.Windows.Forms.Label ManufacturerLbl;
        private System.Windows.Forms.TextBox PasswordTB;
        private System.Windows.Forms.TextBox NameTB;
        private System.Windows.Forms.Label PasswordLbl;
        private System.Windows.Forms.ComboBox AuthenticationCB;
        private System.Windows.Forms.Label AuthenticationLbl;
        private System.Windows.Forms.Label NameLbl;
        private System.Windows.Forms.ComboBox MediasCB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox SerialSettingsGB;
        private System.Windows.Forms.CheckBox UseMaximumBaudRateCB;
        private System.Windows.Forms.ComboBox MaximumBaudRateCB;
        private System.Windows.Forms.Button AdvancedBtn;
        private System.Windows.Forms.Label SerialPortLbl;
        private System.Windows.Forms.ComboBox SerialPortCB;
        private System.Windows.Forms.GroupBox NetworkSettingsGB;
        private System.Windows.Forms.ComboBox NetProtocolCB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox UseRemoteSerialCB;
        private System.Windows.Forms.TextBox PortTB;
        private System.Windows.Forms.Label PortLbl;
        private System.Windows.Forms.TextBox HostNameTB;
        private System.Windows.Forms.Label HostLbl;
        private System.Windows.Forms.GroupBox TerminalSettingsGB;
        private System.Windows.Forms.Label TerminalPortLbl;
        private System.Windows.Forms.Button TerminalAdvancedBtn;
        private System.Windows.Forms.ComboBox TerminalPortCB;
        private System.Windows.Forms.TextBox TerminalPhoneNumberTB;
        private System.Windows.Forms.Label TerminalPhoneNumberLbl;
        private System.Windows.Forms.TabControl DeviceTab;
    }
}