//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
//
// Version:         $Revision: 14858 $,
//                  $Date: 2024-09-17 16:11:12 +0300 (Tue, 17 Sep 2024) $
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevicePropertiesForm));
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.InitialSettingsBtn = new System.Windows.Forms.Button();
            this.SupportedServicesTab = new System.Windows.Forms.TabPage();
            this.SNSettings = new System.Windows.Forms.GroupBox();
            this.SNDeltaValueEncodingCb = new System.Windows.Forms.CheckBox();
            this.SNGeneralProtectionCB = new System.Windows.Forms.CheckBox();
            this.SNGeneralBlockTransferCB = new System.Windows.Forms.CheckBox();
            this.ReadCB = new System.Windows.Forms.CheckBox();
            this.WriteCB = new System.Windows.Forms.CheckBox();
            this.UnconfirmedWriteCB = new System.Windows.Forms.CheckBox();
            this.ReadBlockTransferCB = new System.Windows.Forms.CheckBox();
            this.WriteBlockTransferCB = new System.Windows.Forms.CheckBox();
            this.SNMultipleReferencesCB = new System.Windows.Forms.CheckBox();
            this.InformationReportCB = new System.Windows.Forms.CheckBox();
            this.SNDataNotificationCB = new System.Windows.Forms.CheckBox();
            this.ParameterizedAccessCB = new System.Windows.Forms.CheckBox();
            this.LNSettings = new System.Windows.Forms.GroupBox();
            this.DeltaValueEncodingCb = new System.Windows.Forms.CheckBox();
            this.GeneralProtectionCB = new System.Windows.Forms.CheckBox();
            this.GeneralBlockTransferCB = new System.Windows.Forms.CheckBox();
            this.Attribute0SetReferencingCB = new System.Windows.Forms.CheckBox();
            this.PriorityManagementCB = new System.Windows.Forms.CheckBox();
            this.Attribute0GetReferencingCB = new System.Windows.Forms.CheckBox();
            this.GetBlockTransferCB = new System.Windows.Forms.CheckBox();
            this.SetBlockTransferCB = new System.Windows.Forms.CheckBox();
            this.ActionBlockTransferCB = new System.Windows.Forms.CheckBox();
            this.MultipleReferencesCB = new System.Windows.Forms.CheckBox();
            this.DataNotificationCB = new System.Windows.Forms.CheckBox();
            this.AccessCB = new System.Windows.Forms.CheckBox();
            this.GetCB = new System.Windows.Forms.CheckBox();
            this.SetCB = new System.Windows.Forms.CheckBox();
            this.SelectiveAccessCB = new System.Windows.Forms.CheckBox();
            this.EventNotificationCB = new System.Windows.Forms.CheckBox();
            this.ActionCB = new System.Windows.Forms.CheckBox();
            this.CipheringTab = new System.Windows.Forms.TabPage();
            this.SettingsPanel = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.InvocationCounterCb = new System.Windows.Forms.CheckBox();
            this.FrameCounterTb = new System.Windows.Forms.TextBox();
            this.FrameCounterLbl = new System.Windows.Forms.Label();
            this.InvocationCounterLbl = new System.Windows.Forms.Label();
            this.InvocationCounterTB = new System.Windows.Forms.TextBox();
            this.DeviceSettingsTab = new System.Windows.Forms.TabPage();
            this.BroadcastCb = new System.Windows.Forms.CheckBox();
            this.InterfaceCb = new System.Windows.Forms.ComboBox();
            this.InterfaceLbl = new System.Windows.Forms.Label();
            this.WaitTimeTB = new System.Windows.Forms.DateTimePicker();
            this.ResendTb = new System.Windows.Forms.NumericUpDown();
            this.ResendLbl = new System.Windows.Forms.Label();
            this.CustomSettings = new System.Windows.Forms.GroupBox();
            this.PasswordAsciiCb = new System.Windows.Forms.CheckBox();
            this.VerboseModeCB = new System.Windows.Forms.CheckBox();
            this.UseLNCB = new System.Windows.Forms.CheckBox();
            this.ServerAddressTypeCB = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ClientAddLbl = new System.Windows.Forms.Label();
            this.ClientAddTB = new System.Windows.Forms.NumericUpDown();
            this.LogicalServerAddressTB = new System.Windows.Forms.NumericUpDown();
            this.LogicalServerAddressLbl = new System.Windows.Forms.Label();
            this.PhysicalServerAddressTB = new System.Windows.Forms.NumericUpDown();
            this.PhysicalServerAddressLbl = new System.Windows.Forms.Label();
            this.WaitTimeLbl = new System.Windows.Forms.Label();
            this.ManufacturerCB = new System.Windows.Forms.ComboBox();
            this.ManufacturerLbl = new System.Windows.Forms.Label();
            this.PasswordTB = new System.Windows.Forms.TextBox();
            this.NameTB = new System.Windows.Forms.TextBox();
            this.PasswordLbl = new System.Windows.Forms.Label();
            this.AuthenticationCB = new System.Windows.Forms.ComboBox();
            this.AuthenticationLbl = new System.Windows.Forms.Label();
            this.NameLbl = new System.Windows.Forms.Label();
            this.MediasCB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NetworkSettingsGB = new System.Windows.Forms.GroupBox();
            this.NetProtocolCB = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.UseRemoteSerialCB = new System.Windows.Forms.CheckBox();
            this.PortTB = new System.Windows.Forms.TextBox();
            this.PortLbl = new System.Windows.Forms.Label();
            this.HostNameTB = new System.Windows.Forms.TextBox();
            this.HostLbl = new System.Windows.Forms.Label();
            this.TerminalSettingsGB = new System.Windows.Forms.GroupBox();
            this.TerminalPortLbl = new System.Windows.Forms.Label();
            this.TerminalAdvancedBtn = new System.Windows.Forms.Button();
            this.TerminalPortCB = new System.Windows.Forms.ComboBox();
            this.TerminalPhoneNumberTB = new System.Windows.Forms.TextBox();
            this.TerminalPhoneNumberLbl = new System.Windows.Forms.Label();
            this.SerialSettingsGB = new System.Windows.Forms.GroupBox();
            this.UseMaximumBaudRateCB = new System.Windows.Forms.CheckBox();
            this.MaximumBaudRateCB = new System.Windows.Forms.ComboBox();
            this.AdvancedBtn = new System.Windows.Forms.Button();
            this.SerialPortLbl = new System.Windows.Forms.Label();
            this.SerialPortCB = new System.Windows.Forms.ComboBox();
            this.SigningKeyCb = new System.Windows.Forms.ComboBox();
            this.DeviceTab = new System.Windows.Forms.TabControl();
            this.AdvancedTab = new System.Windows.Forms.TabPage();
            this.IncreaseInvocationCounterForGMacAuthenticationCB = new System.Windows.Forms.CheckBox();
            this.OverwriteAttributeAccessRightsCb = new System.Windows.Forms.CheckBox();
            this.GBTWindowSizeTb = new System.Windows.Forms.TextBox();
            this.GBTWindowSizeLbl = new System.Windows.Forms.Label();
            this.SignInitiateRequestResponseCb = new System.Windows.Forms.CheckBox();
            this.IncludePublicKeyCb = new System.Windows.Forms.CheckBox();
            this.ChallengeSizeCb = new System.Windows.Forms.ComboBox();
            this.ChallengeSizeLbl = new System.Windows.Forms.Label();
            this.SecurityChangeCheckCb = new System.Windows.Forms.CheckBox();
            this.IgnoreTimeStatusCb = new System.Windows.Forms.CheckBox();
            this.IgnoreTimeZoneCb = new System.Windows.Forms.CheckBox();
            this.UseProtectedReleaseCb = new System.Windows.Forms.CheckBox();
            this.InactivityTimeoutTb = new System.Windows.Forms.TextBox();
            this.InactivityTimeoutLbl = new System.Windows.Forms.Label();
            this.ServiceClassCb = new System.Windows.Forms.ComboBox();
            this.StandardCb = new System.Windows.Forms.ComboBox();
            this.PriorityCb = new System.Windows.Forms.ComboBox();
            this.ServiceClassLbl = new System.Windows.Forms.Label();
            this.StandardLbl = new System.Windows.Forms.Label();
            this.PriorityLbl = new System.Windows.Forms.Label();
            this.UserIdTb = new System.Windows.Forms.TextBox();
            this.UserIDLbl = new System.Windows.Forms.Label();
            this.MaxPduTb = new System.Windows.Forms.TextBox();
            this.MaxPduLbl = new System.Windows.Forms.Label();
            this.UseUtcTimeZone = new System.Windows.Forms.CheckBox();
            this.HdlcFrameTab = new System.Windows.Forms.TabPage();
            this.HdlcGroup = new System.Windows.Forms.GroupBox();
            this.FrameSizeCb = new System.Windows.Forms.CheckBox();
            this.WindowSizeRXTb = new System.Windows.Forms.TextBox();
            this.WindowSizeRXLbl = new System.Windows.Forms.Label();
            this.WindowSizeTXTb = new System.Windows.Forms.TextBox();
            this.WindowSizeTXLbl = new System.Windows.Forms.Label();
            this.MaxInfoRXTb = new System.Windows.Forms.TextBox();
            this.MaxInfoRXLbl = new System.Windows.Forms.Label();
            this.MaxInfoTXTb = new System.Windows.Forms.TextBox();
            this.MaxInfoTXLbl = new System.Windows.Forms.Label();
            this.ServerAddressSizeCb = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.PlcFrame = new System.Windows.Forms.TabPage();
            this.PlcGroup = new System.Windows.Forms.GroupBox();
            this.MACTargetAddressTb = new System.Windows.Forms.NumericUpDown();
            this.MACSourceAddressTb = new System.Windows.Forms.NumericUpDown();
            this.MACTargetAddressLbl = new System.Windows.Forms.Label();
            this.MACSourceAddressLbl = new System.Windows.Forms.Label();
            this.PduFrame = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PduWaitTimeTb = new System.Windows.Forms.TextBox();
            this.PduWaitTimelbl = new System.Windows.Forms.Label();
            this.DelaysTab = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ObjectDelayLbl = new System.Windows.Forms.Label();
            this.FrameDelayLbl = new System.Windows.Forms.Label();
            this.GatewayTab = new System.Windows.Forms.TabPage();
            this.UseGatewayCb = new System.Windows.Forms.CheckBox();
            this.PhysicalDeviceAddressAsciiCb = new System.Windows.Forms.CheckBox();
            this.PhysicalDeviceAddressTb = new System.Windows.Forms.TextBox();
            this.PhysicalDeviceAddressLbl = new System.Windows.Forms.Label();
            this.NetworkIDTb = new System.Windows.Forms.TextBox();
            this.NetworkIDLbl = new System.Windows.Forms.Label();
            this.XmlTab = new System.Windows.Forms.TabPage();
            this.PasteFromClipboardBtn = new System.Windows.Forms.Button();
            this.CopyBtn = new System.Windows.Forms.Button();
            this.ApplyBtn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.conformanceHelpProvider = new System.Windows.Forms.HelpProvider();
            this.ShowAsHex = new System.Windows.Forms.CheckBox();
            this.FrameDelayTb = new System.Windows.Forms.TextBox();
            this.ObjectDelayTb = new System.Windows.Forms.TextBox();
            this.SupportedServicesTab.SuspendLayout();
            this.SNSettings.SuspendLayout();
            this.LNSettings.SuspendLayout();
            this.CipheringTab.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.DeviceSettingsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResendTb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClientAddTB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogicalServerAddressTB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhysicalServerAddressTB)).BeginInit();
            this.NetworkSettingsGB.SuspendLayout();
            this.TerminalSettingsGB.SuspendLayout();
            this.SerialSettingsGB.SuspendLayout();
            this.DeviceTab.SuspendLayout();
            this.AdvancedTab.SuspendLayout();
            this.HdlcFrameTab.SuspendLayout();
            this.HdlcGroup.SuspendLayout();
            this.PlcFrame.SuspendLayout();
            this.PlcGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MACTargetAddressTb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MACSourceAddressTb)).BeginInit();
            this.PduFrame.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.DelaysTab.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.GatewayTab.SuspendLayout();
            this.XmlTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(431, 422);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 4;
            this.CancelBtn.Text = "&Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Location = new System.Drawing.Point(350, 422);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 3;
            this.OKBtn.Text = "&OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // InitialSettingsBtn
            // 
            this.InitialSettingsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.InitialSettingsBtn.Location = new System.Drawing.Point(8, 422);
            this.InitialSettingsBtn.Name = "InitialSettingsBtn";
            this.InitialSettingsBtn.Size = new System.Drawing.Size(117, 23);
            this.InitialSettingsBtn.TabIndex = 1;
            this.InitialSettingsBtn.Text = "Initial settings...";
            this.InitialSettingsBtn.UseVisualStyleBackColor = true;
            this.InitialSettingsBtn.Click += new System.EventHandler(this.InitialSettingsBtn_Click);
            // 
            // SupportedServicesTab
            // 
            this.SupportedServicesTab.Controls.Add(this.SNSettings);
            this.SupportedServicesTab.Controls.Add(this.LNSettings);
            this.conformanceHelpProvider.SetHelpNavigator(this.SupportedServicesTab, System.Windows.Forms.HelpNavigator.Topic);
            this.SupportedServicesTab.Location = new System.Drawing.Point(4, 22);
            this.SupportedServicesTab.Name = "SupportedServicesTab";
            this.SupportedServicesTab.Padding = new System.Windows.Forms.Padding(3);
            this.conformanceHelpProvider.SetShowHelp(this.SupportedServicesTab, true);
            this.SupportedServicesTab.Size = new System.Drawing.Size(511, 388);
            this.SupportedServicesTab.TabIndex = 1;
            this.SupportedServicesTab.Text = "Supported Services";
            this.SupportedServicesTab.UseVisualStyleBackColor = true;
            // 
            // SNSettings
            // 
            this.SNSettings.Controls.Add(this.SNDeltaValueEncodingCb);
            this.SNSettings.Controls.Add(this.SNGeneralProtectionCB);
            this.SNSettings.Controls.Add(this.SNGeneralBlockTransferCB);
            this.SNSettings.Controls.Add(this.ReadCB);
            this.SNSettings.Controls.Add(this.WriteCB);
            this.SNSettings.Controls.Add(this.UnconfirmedWriteCB);
            this.SNSettings.Controls.Add(this.ReadBlockTransferCB);
            this.SNSettings.Controls.Add(this.WriteBlockTransferCB);
            this.SNSettings.Controls.Add(this.SNMultipleReferencesCB);
            this.SNSettings.Controls.Add(this.InformationReportCB);
            this.SNSettings.Controls.Add(this.SNDataNotificationCB);
            this.SNSettings.Controls.Add(this.ParameterizedAccessCB);
            this.conformanceHelpProvider.SetHelpKeyword(this.SNSettings, "Action");
            this.conformanceHelpProvider.SetHelpNavigator(this.SNSettings, System.Windows.Forms.HelpNavigator.Topic);
            this.SNSettings.Location = new System.Drawing.Point(188, 6);
            this.SNSettings.Name = "SNSettings";
            this.conformanceHelpProvider.SetShowHelp(this.SNSettings, true);
            this.SNSettings.Size = new System.Drawing.Size(206, 382);
            this.SNSettings.TabIndex = 1;
            this.SNSettings.TabStop = false;
            this.SNSettings.Text = "SN settings";
            // 
            // SNDeltaValueEncodingCb
            // 
            this.SNDeltaValueEncodingCb.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.SNDeltaValueEncodingCb, "DeltaValueEncoding ");
            this.conformanceHelpProvider.SetHelpNavigator(this.SNDeltaValueEncodingCb, System.Windows.Forms.HelpNavigator.Topic);
            this.SNDeltaValueEncodingCb.Location = new System.Drawing.Point(6, 240);
            this.SNDeltaValueEncodingCb.Name = "SNDeltaValueEncodingCb";
            this.conformanceHelpProvider.SetShowHelp(this.SNDeltaValueEncodingCb, true);
            this.SNDeltaValueEncodingCb.Size = new System.Drawing.Size(127, 17);
            this.SNDeltaValueEncodingCb.TabIndex = 18;
            this.SNDeltaValueEncodingCb.Text = "Delta value encoding";
            this.SNDeltaValueEncodingCb.UseVisualStyleBackColor = true;
            // 
            // SNGeneralProtectionCB
            // 
            this.SNGeneralProtectionCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.SNGeneralProtectionCB, "GeneralProtection");
            this.conformanceHelpProvider.SetHelpNavigator(this.SNGeneralProtectionCB, System.Windows.Forms.HelpNavigator.Topic);
            this.SNGeneralProtectionCB.Location = new System.Drawing.Point(6, 220);
            this.SNGeneralProtectionCB.Name = "SNGeneralProtectionCB";
            this.conformanceHelpProvider.SetShowHelp(this.SNGeneralProtectionCB, true);
            this.SNGeneralProtectionCB.Size = new System.Drawing.Size(113, 17);
            this.SNGeneralProtectionCB.TabIndex = 11;
            this.SNGeneralProtectionCB.Text = "General protection";
            this.SNGeneralProtectionCB.UseVisualStyleBackColor = true;
            // 
            // SNGeneralBlockTransferCB
            // 
            this.SNGeneralBlockTransferCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.SNGeneralBlockTransferCB, "GeneralBlockTransfer");
            this.conformanceHelpProvider.SetHelpNavigator(this.SNGeneralBlockTransferCB, System.Windows.Forms.HelpNavigator.Topic);
            this.SNGeneralBlockTransferCB.Location = new System.Drawing.Point(6, 200);
            this.SNGeneralBlockTransferCB.Name = "SNGeneralBlockTransferCB";
            this.conformanceHelpProvider.SetShowHelp(this.SNGeneralBlockTransferCB, true);
            this.SNGeneralBlockTransferCB.Size = new System.Drawing.Size(130, 17);
            this.SNGeneralBlockTransferCB.TabIndex = 10;
            this.SNGeneralBlockTransferCB.Text = "General block transfer";
            this.SNGeneralBlockTransferCB.UseVisualStyleBackColor = true;
            // 
            // ReadCB
            // 
            this.ReadCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.ReadCB, "Read");
            this.conformanceHelpProvider.SetHelpNavigator(this.ReadCB, System.Windows.Forms.HelpNavigator.Topic);
            this.ReadCB.Location = new System.Drawing.Point(6, 180);
            this.ReadCB.Name = "ReadCB";
            this.conformanceHelpProvider.SetShowHelp(this.ReadCB, true);
            this.ReadCB.Size = new System.Drawing.Size(52, 17);
            this.ReadCB.TabIndex = 9;
            this.ReadCB.Text = "Read";
            this.ReadCB.UseVisualStyleBackColor = true;
            // 
            // WriteCB
            // 
            this.WriteCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.WriteCB, "Write");
            this.conformanceHelpProvider.SetHelpNavigator(this.WriteCB, System.Windows.Forms.HelpNavigator.Topic);
            this.WriteCB.Location = new System.Drawing.Point(6, 160);
            this.WriteCB.Name = "WriteCB";
            this.conformanceHelpProvider.SetShowHelp(this.WriteCB, true);
            this.WriteCB.Size = new System.Drawing.Size(51, 17);
            this.WriteCB.TabIndex = 8;
            this.WriteCB.Text = "Write";
            this.WriteCB.UseVisualStyleBackColor = true;
            // 
            // UnconfirmedWriteCB
            // 
            this.UnconfirmedWriteCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.UnconfirmedWriteCB, "UnconfirmedWrite");
            this.conformanceHelpProvider.SetHelpNavigator(this.UnconfirmedWriteCB, System.Windows.Forms.HelpNavigator.Topic);
            this.UnconfirmedWriteCB.Location = new System.Drawing.Point(6, 140);
            this.UnconfirmedWriteCB.Name = "UnconfirmedWriteCB";
            this.conformanceHelpProvider.SetShowHelp(this.UnconfirmedWriteCB, true);
            this.UnconfirmedWriteCB.Size = new System.Drawing.Size(111, 17);
            this.UnconfirmedWriteCB.TabIndex = 7;
            this.UnconfirmedWriteCB.Text = "Unconfirmed write";
            this.UnconfirmedWriteCB.UseVisualStyleBackColor = true;
            // 
            // ReadBlockTransferCB
            // 
            this.ReadBlockTransferCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.ReadBlockTransferCB, "BlockTransferWithGetOrRead");
            this.conformanceHelpProvider.SetHelpNavigator(this.ReadBlockTransferCB, System.Windows.Forms.HelpNavigator.Topic);
            this.ReadBlockTransferCB.Location = new System.Drawing.Point(6, 120);
            this.ReadBlockTransferCB.Name = "ReadBlockTransferCB";
            this.conformanceHelpProvider.SetShowHelp(this.ReadBlockTransferCB, true);
            this.ReadBlockTransferCB.Size = new System.Drawing.Size(119, 17);
            this.ReadBlockTransferCB.TabIndex = 6;
            this.ReadBlockTransferCB.Text = "Read block transfer";
            this.ReadBlockTransferCB.UseVisualStyleBackColor = true;
            // 
            // WriteBlockTransferCB
            // 
            this.WriteBlockTransferCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.WriteBlockTransferCB, "BlockTransferWithSetOrWrite");
            this.conformanceHelpProvider.SetHelpNavigator(this.WriteBlockTransferCB, System.Windows.Forms.HelpNavigator.Topic);
            this.WriteBlockTransferCB.Location = new System.Drawing.Point(6, 100);
            this.WriteBlockTransferCB.Name = "WriteBlockTransferCB";
            this.conformanceHelpProvider.SetShowHelp(this.WriteBlockTransferCB, true);
            this.WriteBlockTransferCB.Size = new System.Drawing.Size(118, 17);
            this.WriteBlockTransferCB.TabIndex = 5;
            this.WriteBlockTransferCB.Text = "Write block transfer";
            this.WriteBlockTransferCB.UseVisualStyleBackColor = true;
            // 
            // SNMultipleReferencesCB
            // 
            this.SNMultipleReferencesCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.SNMultipleReferencesCB, "MultipleReferences");
            this.conformanceHelpProvider.SetHelpNavigator(this.SNMultipleReferencesCB, System.Windows.Forms.HelpNavigator.Topic);
            this.SNMultipleReferencesCB.Location = new System.Drawing.Point(6, 80);
            this.SNMultipleReferencesCB.Name = "SNMultipleReferencesCB";
            this.conformanceHelpProvider.SetShowHelp(this.SNMultipleReferencesCB, true);
            this.SNMultipleReferencesCB.Size = new System.Drawing.Size(115, 17);
            this.SNMultipleReferencesCB.TabIndex = 4;
            this.SNMultipleReferencesCB.Text = "Multiple references";
            this.SNMultipleReferencesCB.UseVisualStyleBackColor = true;
            // 
            // InformationReportCB
            // 
            this.InformationReportCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.InformationReportCB, "InformationReport");
            this.conformanceHelpProvider.SetHelpNavigator(this.InformationReportCB, System.Windows.Forms.HelpNavigator.Topic);
            this.InformationReportCB.Location = new System.Drawing.Point(6, 60);
            this.InformationReportCB.Name = "InformationReportCB";
            this.conformanceHelpProvider.SetShowHelp(this.InformationReportCB, true);
            this.InformationReportCB.Size = new System.Drawing.Size(108, 17);
            this.InformationReportCB.TabIndex = 3;
            this.InformationReportCB.Text = "Information report";
            this.InformationReportCB.UseVisualStyleBackColor = true;
            // 
            // SNDataNotificationCB
            // 
            this.SNDataNotificationCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.SNDataNotificationCB, "DataNotification");
            this.conformanceHelpProvider.SetHelpNavigator(this.SNDataNotificationCB, System.Windows.Forms.HelpNavigator.Topic);
            this.SNDataNotificationCB.Location = new System.Drawing.Point(6, 40);
            this.SNDataNotificationCB.Name = "SNDataNotificationCB";
            this.conformanceHelpProvider.SetShowHelp(this.SNDataNotificationCB, true);
            this.SNDataNotificationCB.Size = new System.Drawing.Size(103, 17);
            this.SNDataNotificationCB.TabIndex = 2;
            this.SNDataNotificationCB.Text = "Data notification";
            this.SNDataNotificationCB.UseVisualStyleBackColor = true;
            // 
            // ParameterizedAccessCB
            // 
            this.ParameterizedAccessCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.ParameterizedAccessCB, "ParameterizedAccess");
            this.conformanceHelpProvider.SetHelpNavigator(this.ParameterizedAccessCB, System.Windows.Forms.HelpNavigator.Topic);
            this.ParameterizedAccessCB.Location = new System.Drawing.Point(6, 20);
            this.ParameterizedAccessCB.Name = "ParameterizedAccessCB";
            this.conformanceHelpProvider.SetShowHelp(this.ParameterizedAccessCB, true);
            this.ParameterizedAccessCB.Size = new System.Drawing.Size(130, 17);
            this.ParameterizedAccessCB.TabIndex = 1;
            this.ParameterizedAccessCB.Text = "Parameterized access";
            this.ParameterizedAccessCB.UseVisualStyleBackColor = true;
            // 
            // LNSettings
            // 
            this.LNSettings.Controls.Add(this.DeltaValueEncodingCb);
            this.LNSettings.Controls.Add(this.GeneralProtectionCB);
            this.LNSettings.Controls.Add(this.GeneralBlockTransferCB);
            this.LNSettings.Controls.Add(this.Attribute0SetReferencingCB);
            this.LNSettings.Controls.Add(this.PriorityManagementCB);
            this.LNSettings.Controls.Add(this.Attribute0GetReferencingCB);
            this.LNSettings.Controls.Add(this.GetBlockTransferCB);
            this.LNSettings.Controls.Add(this.SetBlockTransferCB);
            this.LNSettings.Controls.Add(this.ActionBlockTransferCB);
            this.LNSettings.Controls.Add(this.MultipleReferencesCB);
            this.LNSettings.Controls.Add(this.DataNotificationCB);
            this.LNSettings.Controls.Add(this.AccessCB);
            this.LNSettings.Controls.Add(this.GetCB);
            this.LNSettings.Controls.Add(this.SetCB);
            this.LNSettings.Controls.Add(this.SelectiveAccessCB);
            this.LNSettings.Controls.Add(this.EventNotificationCB);
            this.LNSettings.Controls.Add(this.ActionCB);
            this.conformanceHelpProvider.SetHelpKeyword(this.LNSettings, "Action");
            this.conformanceHelpProvider.SetHelpNavigator(this.LNSettings, System.Windows.Forms.HelpNavigator.Topic);
            this.LNSettings.Location = new System.Drawing.Point(8, 0);
            this.LNSettings.Name = "LNSettings";
            this.conformanceHelpProvider.SetShowHelp(this.LNSettings, true);
            this.LNSettings.Size = new System.Drawing.Size(172, 382);
            this.LNSettings.TabIndex = 0;
            this.LNSettings.TabStop = false;
            this.LNSettings.Text = "LN settings";
            // 
            // DeltaValueEncodingCb
            // 
            this.DeltaValueEncodingCb.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.DeltaValueEncodingCb, "DeltaValueEncoding ");
            this.conformanceHelpProvider.SetHelpNavigator(this.DeltaValueEncodingCb, System.Windows.Forms.HelpNavigator.Topic);
            this.DeltaValueEncodingCb.Location = new System.Drawing.Point(6, 340);
            this.DeltaValueEncodingCb.Name = "DeltaValueEncodingCb";
            this.conformanceHelpProvider.SetShowHelp(this.DeltaValueEncodingCb, true);
            this.DeltaValueEncodingCb.Size = new System.Drawing.Size(127, 17);
            this.DeltaValueEncodingCb.TabIndex = 17;
            this.DeltaValueEncodingCb.Text = "Delta value encoding";
            this.DeltaValueEncodingCb.UseVisualStyleBackColor = true;
            // 
            // GeneralProtectionCB
            // 
            this.GeneralProtectionCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.GeneralProtectionCB, "GeneralProtection");
            this.conformanceHelpProvider.SetHelpNavigator(this.GeneralProtectionCB, System.Windows.Forms.HelpNavigator.Topic);
            this.GeneralProtectionCB.Location = new System.Drawing.Point(6, 320);
            this.GeneralProtectionCB.Name = "GeneralProtectionCB";
            this.conformanceHelpProvider.SetShowHelp(this.GeneralProtectionCB, true);
            this.GeneralProtectionCB.Size = new System.Drawing.Size(113, 17);
            this.GeneralProtectionCB.TabIndex = 16;
            this.GeneralProtectionCB.Text = "General protection";
            this.GeneralProtectionCB.UseVisualStyleBackColor = true;
            // 
            // GeneralBlockTransferCB
            // 
            this.GeneralBlockTransferCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.GeneralBlockTransferCB, "GeneralBlockTransfer");
            this.conformanceHelpProvider.SetHelpNavigator(this.GeneralBlockTransferCB, System.Windows.Forms.HelpNavigator.Topic);
            this.GeneralBlockTransferCB.Location = new System.Drawing.Point(6, 300);
            this.GeneralBlockTransferCB.Name = "GeneralBlockTransferCB";
            this.conformanceHelpProvider.SetShowHelp(this.GeneralBlockTransferCB, true);
            this.GeneralBlockTransferCB.Size = new System.Drawing.Size(130, 17);
            this.GeneralBlockTransferCB.TabIndex = 15;
            this.GeneralBlockTransferCB.Text = "General block transfer";
            this.GeneralBlockTransferCB.UseVisualStyleBackColor = true;
            // 
            // Attribute0SetReferencingCB
            // 
            this.Attribute0SetReferencingCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.Attribute0SetReferencingCB, "Attribute0SetReferencing");
            this.conformanceHelpProvider.SetHelpNavigator(this.Attribute0SetReferencingCB, System.Windows.Forms.HelpNavigator.Topic);
            this.Attribute0SetReferencingCB.Location = new System.Drawing.Point(6, 280);
            this.Attribute0SetReferencingCB.Name = "Attribute0SetReferencingCB";
            this.conformanceHelpProvider.SetShowHelp(this.Attribute0SetReferencingCB, true);
            this.Attribute0SetReferencingCB.Size = new System.Drawing.Size(147, 17);
            this.Attribute0SetReferencingCB.TabIndex = 14;
            this.Attribute0SetReferencingCB.Text = "Attribute 0 set referencing";
            this.Attribute0SetReferencingCB.UseVisualStyleBackColor = true;
            // 
            // PriorityManagementCB
            // 
            this.PriorityManagementCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.PriorityManagementCB, "PriorityManagement");
            this.conformanceHelpProvider.SetHelpNavigator(this.PriorityManagementCB, System.Windows.Forms.HelpNavigator.Topic);
            this.PriorityManagementCB.Location = new System.Drawing.Point(6, 260);
            this.PriorityManagementCB.Name = "PriorityManagementCB";
            this.conformanceHelpProvider.SetShowHelp(this.PriorityManagementCB, true);
            this.PriorityManagementCB.Size = new System.Drawing.Size(121, 17);
            this.PriorityManagementCB.TabIndex = 13;
            this.PriorityManagementCB.Text = "Priority management";
            this.PriorityManagementCB.UseVisualStyleBackColor = true;
            // 
            // Attribute0GetReferencingCB
            // 
            this.Attribute0GetReferencingCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.Attribute0GetReferencingCB, "Attribute0GetReferencing");
            this.conformanceHelpProvider.SetHelpNavigator(this.Attribute0GetReferencingCB, System.Windows.Forms.HelpNavigator.Topic);
            this.Attribute0GetReferencingCB.Location = new System.Drawing.Point(5, 240);
            this.Attribute0GetReferencingCB.Name = "Attribute0GetReferencingCB";
            this.conformanceHelpProvider.SetShowHelp(this.Attribute0GetReferencingCB, true);
            this.Attribute0GetReferencingCB.Size = new System.Drawing.Size(148, 17);
            this.Attribute0GetReferencingCB.TabIndex = 12;
            this.Attribute0GetReferencingCB.Text = "Attribute 0 get referencing";
            this.Attribute0GetReferencingCB.UseVisualStyleBackColor = true;
            // 
            // GetBlockTransferCB
            // 
            this.GetBlockTransferCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.GetBlockTransferCB, "BlockTransferWithGetOrRead");
            this.conformanceHelpProvider.SetHelpNavigator(this.GetBlockTransferCB, System.Windows.Forms.HelpNavigator.Topic);
            this.GetBlockTransferCB.Location = new System.Drawing.Point(6, 220);
            this.GetBlockTransferCB.Name = "GetBlockTransferCB";
            this.conformanceHelpProvider.SetShowHelp(this.GetBlockTransferCB, true);
            this.GetBlockTransferCB.Size = new System.Drawing.Size(110, 17);
            this.GetBlockTransferCB.TabIndex = 11;
            this.GetBlockTransferCB.Text = "Get block transfer";
            this.GetBlockTransferCB.UseVisualStyleBackColor = true;
            // 
            // SetBlockTransferCB
            // 
            this.SetBlockTransferCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.SetBlockTransferCB, "BlockTransferWithSetOrWrite ");
            this.conformanceHelpProvider.SetHelpNavigator(this.SetBlockTransferCB, System.Windows.Forms.HelpNavigator.Topic);
            this.SetBlockTransferCB.Location = new System.Drawing.Point(6, 200);
            this.SetBlockTransferCB.Name = "SetBlockTransferCB";
            this.conformanceHelpProvider.SetShowHelp(this.SetBlockTransferCB, true);
            this.SetBlockTransferCB.Size = new System.Drawing.Size(109, 17);
            this.SetBlockTransferCB.TabIndex = 10;
            this.SetBlockTransferCB.Text = "Set block transfer";
            this.SetBlockTransferCB.UseVisualStyleBackColor = true;
            // 
            // ActionBlockTransferCB
            // 
            this.ActionBlockTransferCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.ActionBlockTransferCB, "BlockTransferWithAction");
            this.conformanceHelpProvider.SetHelpNavigator(this.ActionBlockTransferCB, System.Windows.Forms.HelpNavigator.Topic);
            this.ActionBlockTransferCB.Location = new System.Drawing.Point(6, 180);
            this.ActionBlockTransferCB.Name = "ActionBlockTransferCB";
            this.conformanceHelpProvider.SetShowHelp(this.ActionBlockTransferCB, true);
            this.ActionBlockTransferCB.Size = new System.Drawing.Size(123, 17);
            this.ActionBlockTransferCB.TabIndex = 9;
            this.ActionBlockTransferCB.Text = "Action block transfer";
            this.ActionBlockTransferCB.UseVisualStyleBackColor = true;
            // 
            // MultipleReferencesCB
            // 
            this.MultipleReferencesCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.MultipleReferencesCB, "MultipleReferences");
            this.conformanceHelpProvider.SetHelpNavigator(this.MultipleReferencesCB, System.Windows.Forms.HelpNavigator.Topic);
            this.MultipleReferencesCB.Location = new System.Drawing.Point(6, 160);
            this.MultipleReferencesCB.Name = "MultipleReferencesCB";
            this.conformanceHelpProvider.SetShowHelp(this.MultipleReferencesCB, true);
            this.MultipleReferencesCB.Size = new System.Drawing.Size(120, 17);
            this.MultipleReferencesCB.TabIndex = 8;
            this.MultipleReferencesCB.Text = "Multiple References";
            this.MultipleReferencesCB.UseVisualStyleBackColor = true;
            // 
            // DataNotificationCB
            // 
            this.DataNotificationCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.DataNotificationCB, "DataNotification");
            this.conformanceHelpProvider.SetHelpNavigator(this.DataNotificationCB, System.Windows.Forms.HelpNavigator.Topic);
            this.DataNotificationCB.Location = new System.Drawing.Point(6, 140);
            this.DataNotificationCB.Name = "DataNotificationCB";
            this.conformanceHelpProvider.SetShowHelp(this.DataNotificationCB, true);
            this.DataNotificationCB.Size = new System.Drawing.Size(105, 17);
            this.DataNotificationCB.TabIndex = 7;
            this.DataNotificationCB.Text = "Data Notification";
            this.DataNotificationCB.UseVisualStyleBackColor = true;
            // 
            // AccessCB
            // 
            this.AccessCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.AccessCB, "Access");
            this.conformanceHelpProvider.SetHelpNavigator(this.AccessCB, System.Windows.Forms.HelpNavigator.Topic);
            this.AccessCB.Location = new System.Drawing.Point(6, 120);
            this.AccessCB.Name = "AccessCB";
            this.conformanceHelpProvider.SetShowHelp(this.AccessCB, true);
            this.AccessCB.Size = new System.Drawing.Size(61, 17);
            this.AccessCB.TabIndex = 6;
            this.AccessCB.Text = "Access";
            this.AccessCB.UseVisualStyleBackColor = true;
            // 
            // GetCB
            // 
            this.GetCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.GetCB, "Get");
            this.conformanceHelpProvider.SetHelpNavigator(this.GetCB, System.Windows.Forms.HelpNavigator.Topic);
            this.GetCB.Location = new System.Drawing.Point(6, 100);
            this.GetCB.Name = "GetCB";
            this.conformanceHelpProvider.SetShowHelp(this.GetCB, true);
            this.GetCB.Size = new System.Drawing.Size(43, 17);
            this.GetCB.TabIndex = 5;
            this.GetCB.Text = "Get";
            this.GetCB.UseVisualStyleBackColor = true;
            // 
            // SetCB
            // 
            this.SetCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.SetCB, "Set");
            this.conformanceHelpProvider.SetHelpNavigator(this.SetCB, System.Windows.Forms.HelpNavigator.Topic);
            this.SetCB.Location = new System.Drawing.Point(6, 80);
            this.SetCB.Name = "SetCB";
            this.conformanceHelpProvider.SetShowHelp(this.SetCB, true);
            this.SetCB.Size = new System.Drawing.Size(42, 17);
            this.SetCB.TabIndex = 4;
            this.SetCB.Text = "Set";
            this.SetCB.UseVisualStyleBackColor = true;
            // 
            // SelectiveAccessCB
            // 
            this.SelectiveAccessCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.SelectiveAccessCB, "SelectiveAccess");
            this.conformanceHelpProvider.SetHelpNavigator(this.SelectiveAccessCB, System.Windows.Forms.HelpNavigator.Topic);
            this.SelectiveAccessCB.Location = new System.Drawing.Point(6, 60);
            this.SelectiveAccessCB.Name = "SelectiveAccessCB";
            this.conformanceHelpProvider.SetShowHelp(this.SelectiveAccessCB, true);
            this.SelectiveAccessCB.Size = new System.Drawing.Size(108, 17);
            this.SelectiveAccessCB.TabIndex = 3;
            this.SelectiveAccessCB.Text = "Selective Access";
            this.SelectiveAccessCB.UseVisualStyleBackColor = true;
            // 
            // EventNotificationCB
            // 
            this.EventNotificationCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.EventNotificationCB, "EventNotification");
            this.conformanceHelpProvider.SetHelpNavigator(this.EventNotificationCB, System.Windows.Forms.HelpNavigator.Topic);
            this.EventNotificationCB.Location = new System.Drawing.Point(6, 40);
            this.EventNotificationCB.Name = "EventNotificationCB";
            this.conformanceHelpProvider.SetShowHelp(this.EventNotificationCB, true);
            this.EventNotificationCB.Size = new System.Drawing.Size(110, 17);
            this.EventNotificationCB.TabIndex = 2;
            this.EventNotificationCB.Text = "Event Notification";
            this.EventNotificationCB.UseVisualStyleBackColor = true;
            // 
            // ActionCB
            // 
            this.ActionCB.AutoSize = true;
            this.conformanceHelpProvider.SetHelpKeyword(this.ActionCB, "Action");
            this.conformanceHelpProvider.SetHelpNavigator(this.ActionCB, System.Windows.Forms.HelpNavigator.Topic);
            this.ActionCB.Location = new System.Drawing.Point(6, 20);
            this.ActionCB.Name = "ActionCB";
            this.conformanceHelpProvider.SetShowHelp(this.ActionCB, true);
            this.ActionCB.Size = new System.Drawing.Size(56, 17);
            this.ActionCB.TabIndex = 1;
            this.ActionCB.Text = "Action";
            this.ActionCB.UseVisualStyleBackColor = true;
            // 
            // CipheringTab
            // 
            this.CipheringTab.Controls.Add(this.SettingsPanel);
            this.CipheringTab.Controls.Add(this.groupBox2);
            this.CipheringTab.Location = new System.Drawing.Point(4, 22);
            this.CipheringTab.Name = "CipheringTab";
            this.CipheringTab.Size = new System.Drawing.Size(511, 388);
            this.CipheringTab.TabIndex = 2;
            this.CipheringTab.Text = "Secured Connections";
            this.CipheringTab.UseVisualStyleBackColor = true;
            // 
            // SettingsPanel
            // 
            this.SettingsPanel.Location = new System.Drawing.Point(2, 3);
            this.SettingsPanel.Name = "SettingsPanel";
            this.SettingsPanel.Size = new System.Drawing.Size(504, 280);
            this.SettingsPanel.TabIndex = 80;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.InvocationCounterCb);
            this.groupBox2.Controls.Add(this.FrameCounterTb);
            this.groupBox2.Controls.Add(this.FrameCounterLbl);
            this.groupBox2.Controls.Add(this.InvocationCounterLbl);
            this.groupBox2.Controls.Add(this.InvocationCounterTB);
            this.groupBox2.Location = new System.Drawing.Point(1, 288);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(339, 97);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Invocation Counter:";
            // 
            // InvocationCounterCb
            // 
            this.InvocationCounterCb.AutoSize = true;
            this.InvocationCounterCb.Location = new System.Drawing.Point(6, 19);
            this.InvocationCounterCb.Name = "InvocationCounterCb";
            this.InvocationCounterCb.Size = new System.Drawing.Size(117, 17);
            this.InvocationCounterCb.TabIndex = 0;
            this.InvocationCounterCb.Text = "Read Automatically";
            this.InvocationCounterCb.UseVisualStyleBackColor = true;
            this.InvocationCounterCb.CheckedChanged += new System.EventHandler(this.InvocationCounterCb_CheckedChanged);
            // 
            // FrameCounterTb
            // 
            this.FrameCounterTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FrameCounterTb.Location = new System.Drawing.Point(112, 68);
            this.FrameCounterTb.Name = "FrameCounterTb";
            this.FrameCounterTb.Size = new System.Drawing.Size(221, 20);
            this.FrameCounterTb.TabIndex = 10;
            // 
            // FrameCounterLbl
            // 
            this.FrameCounterLbl.AutoSize = true;
            this.FrameCounterLbl.Location = new System.Drawing.Point(7, 71);
            this.FrameCounterLbl.Name = "FrameCounterLbl";
            this.FrameCounterLbl.Size = new System.Drawing.Size(96, 13);
            this.FrameCounterLbl.TabIndex = 71;
            this.FrameCounterLbl.Text = "Frame Counter LN:";
            // 
            // InvocationCounterLbl
            // 
            this.InvocationCounterLbl.AutoSize = true;
            this.InvocationCounterLbl.Location = new System.Drawing.Point(7, 45);
            this.InvocationCounterLbl.Name = "InvocationCounterLbl";
            this.InvocationCounterLbl.Size = new System.Drawing.Size(100, 13);
            this.InvocationCounterLbl.TabIndex = 63;
            this.InvocationCounterLbl.Text = "Invocation Counter:";
            // 
            // InvocationCounterTB
            // 
            this.InvocationCounterTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InvocationCounterTB.Location = new System.Drawing.Point(112, 42);
            this.InvocationCounterTB.Name = "InvocationCounterTB";
            this.InvocationCounterTB.Size = new System.Drawing.Size(221, 20);
            this.InvocationCounterTB.TabIndex = 9;
            // 
            // DeviceSettingsTab
            // 
            this.DeviceSettingsTab.Controls.Add(this.BroadcastCb);
            this.DeviceSettingsTab.Controls.Add(this.InterfaceCb);
            this.DeviceSettingsTab.Controls.Add(this.InterfaceLbl);
            this.DeviceSettingsTab.Controls.Add(this.WaitTimeTB);
            this.DeviceSettingsTab.Controls.Add(this.ResendTb);
            this.DeviceSettingsTab.Controls.Add(this.ResendLbl);
            this.DeviceSettingsTab.Controls.Add(this.CustomSettings);
            this.DeviceSettingsTab.Controls.Add(this.PasswordAsciiCb);
            this.DeviceSettingsTab.Controls.Add(this.VerboseModeCB);
            this.DeviceSettingsTab.Controls.Add(this.UseLNCB);
            this.DeviceSettingsTab.Controls.Add(this.ServerAddressTypeCB);
            this.DeviceSettingsTab.Controls.Add(this.label3);
            this.DeviceSettingsTab.Controls.Add(this.ClientAddLbl);
            this.DeviceSettingsTab.Controls.Add(this.ClientAddTB);
            this.DeviceSettingsTab.Controls.Add(this.LogicalServerAddressTB);
            this.DeviceSettingsTab.Controls.Add(this.LogicalServerAddressLbl);
            this.DeviceSettingsTab.Controls.Add(this.PhysicalServerAddressTB);
            this.DeviceSettingsTab.Controls.Add(this.PhysicalServerAddressLbl);
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
            this.DeviceSettingsTab.Controls.Add(this.NetworkSettingsGB);
            this.DeviceSettingsTab.Controls.Add(this.TerminalSettingsGB);
            this.DeviceSettingsTab.Controls.Add(this.SerialSettingsGB);
            this.DeviceSettingsTab.Controls.Add(this.SigningKeyCb);
            this.helpProvider1.SetHelpKeyword(this.DeviceSettingsTab, "Advanced");
            this.helpProvider1.SetHelpNavigator(this.DeviceSettingsTab, System.Windows.Forms.HelpNavigator.Topic);
            this.DeviceSettingsTab.Location = new System.Drawing.Point(4, 22);
            this.DeviceSettingsTab.Name = "DeviceSettingsTab";
            this.DeviceSettingsTab.Padding = new System.Windows.Forms.Padding(3);
            this.helpProvider1.SetShowHelp(this.DeviceSettingsTab, true);
            this.DeviceSettingsTab.Size = new System.Drawing.Size(511, 388);
            this.DeviceSettingsTab.TabIndex = 0;
            this.DeviceSettingsTab.Text = "Device Settings";
            this.DeviceSettingsTab.UseVisualStyleBackColor = true;
            // 
            // BroadcastCb
            // 
            this.BroadcastCb.AutoSize = true;
            this.helpProvider1.SetHelpKeyword(this.BroadcastCb, "VerboseMode");
            this.helpProvider1.SetHelpNavigator(this.BroadcastCb, System.Windows.Forms.HelpNavigator.Topic);
            this.BroadcastCb.Location = new System.Drawing.Point(349, 172);
            this.BroadcastCb.Name = "BroadcastCb";
            this.helpProvider1.SetShowHelp(this.BroadcastCb, true);
            this.BroadcastCb.Size = new System.Drawing.Size(74, 17);
            this.BroadcastCb.TabIndex = 65;
            this.BroadcastCb.Text = "Broadcast";
            this.BroadcastCb.UseVisualStyleBackColor = true;
            // 
            // InterfaceCb
            // 
            this.InterfaceCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InterfaceCb.FormattingEnabled = true;
            this.helpProvider1.SetHelpKeyword(this.InterfaceCb, "Manufacturer");
            this.helpProvider1.SetHelpNavigator(this.InterfaceCb, System.Windows.Forms.HelpNavigator.Topic);
            this.InterfaceCb.Location = new System.Drawing.Point(109, 58);
            this.InterfaceCb.Name = "InterfaceCb";
            this.helpProvider1.SetShowHelp(this.InterfaceCb, true);
            this.InterfaceCb.Size = new System.Drawing.Size(139, 21);
            this.InterfaceCb.Sorted = true;
            this.InterfaceCb.TabIndex = 2;
            this.InterfaceCb.SelectedIndexChanged += new System.EventHandler(this.InterfaceCb_SelectedIndexChanged);
            // 
            // InterfaceLbl
            // 
            this.InterfaceLbl.AutoSize = true;
            this.InterfaceLbl.Location = new System.Drawing.Point(8, 63);
            this.InterfaceLbl.Name = "InterfaceLbl";
            this.InterfaceLbl.Size = new System.Drawing.Size(52, 13);
            this.InterfaceLbl.TabIndex = 63;
            this.InterfaceLbl.Text = "Interface:";
            // 
            // WaitTimeTB
            // 
            this.WaitTimeTB.CustomFormat = "HH:mm:ss";
            this.WaitTimeTB.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.helpProvider1.SetHelpKeyword(this.WaitTimeTB, "WaitTime");
            this.helpProvider1.SetHelpNavigator(this.WaitTimeTB, System.Windows.Forms.HelpNavigator.Topic);
            this.WaitTimeTB.Location = new System.Drawing.Point(109, 142);
            this.WaitTimeTB.Name = "WaitTimeTB";
            this.helpProvider1.SetShowHelp(this.WaitTimeTB, true);
            this.WaitTimeTB.ShowUpDown = true;
            this.WaitTimeTB.Size = new System.Drawing.Size(139, 20);
            this.WaitTimeTB.TabIndex = 8;
            this.WaitTimeTB.Value = new System.DateTime(2019, 4, 1, 14, 52, 0, 0);
            // 
            // ResendTb
            // 
            this.helpProvider1.SetHelpKeyword(this.ResendTb, "ResendCount");
            this.helpProvider1.SetHelpNavigator(this.ResendTb, System.Windows.Forms.HelpNavigator.Topic);
            this.ResendTb.Location = new System.Drawing.Point(351, 143);
            this.ResendTb.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ResendTb.Name = "ResendTb";
            this.helpProvider1.SetShowHelp(this.ResendTb, true);
            this.ResendTb.Size = new System.Drawing.Size(105, 20);
            this.ResendTb.TabIndex = 9;
            this.ResendTb.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // ResendLbl
            // 
            this.ResendLbl.AutoSize = true;
            this.ResendLbl.Location = new System.Drawing.Point(266, 148);
            this.ResendLbl.Name = "ResendLbl";
            this.ResendLbl.Size = new System.Drawing.Size(77, 13);
            this.ResendLbl.TabIndex = 52;
            this.ResendLbl.Text = "Resend count:";
            // 
            // CustomSettings
            // 
            this.CustomSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CustomSettings.Location = new System.Drawing.Point(8, 275);
            this.CustomSettings.Name = "CustomSettings";
            this.CustomSettings.Size = new System.Drawing.Size(457, 99);
            this.CustomSettings.TabIndex = 50;
            this.CustomSettings.TabStop = false;
            this.CustomSettings.Text = "Settings";
            // 
            // PasswordAsciiCb
            // 
            this.PasswordAsciiCb.AutoSize = true;
            this.PasswordAsciiCb.Checked = true;
            this.PasswordAsciiCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.helpProvider1.SetHelpString(this.PasswordAsciiCb, "Is password given in ASCII or Hex.");
            this.PasswordAsciiCb.Location = new System.Drawing.Point(369, 115);
            this.PasswordAsciiCb.Name = "PasswordAsciiCb";
            this.helpProvider1.SetShowHelp(this.PasswordAsciiCb, true);
            this.PasswordAsciiCb.Size = new System.Drawing.Size(53, 17);
            this.PasswordAsciiCb.TabIndex = 7;
            this.PasswordAsciiCb.Text = "ASCII";
            this.PasswordAsciiCb.UseVisualStyleBackColor = true;
            this.PasswordAsciiCb.CheckedChanged += new System.EventHandler(this.PasswordAsciiCb_CheckedChanged);
            // 
            // VerboseModeCB
            // 
            this.VerboseModeCB.AutoSize = true;
            this.helpProvider1.SetHelpKeyword(this.VerboseModeCB, "VerboseMode");
            this.helpProvider1.SetHelpNavigator(this.VerboseModeCB, System.Windows.Forms.HelpNavigator.Topic);
            this.VerboseModeCB.Location = new System.Drawing.Point(350, 223);
            this.VerboseModeCB.Name = "VerboseModeCB";
            this.helpProvider1.SetShowHelp(this.VerboseModeCB, true);
            this.VerboseModeCB.Size = new System.Drawing.Size(95, 17);
            this.VerboseModeCB.TabIndex = 14;
            this.VerboseModeCB.Text = "Verbose Mode";
            this.VerboseModeCB.UseVisualStyleBackColor = true;
            // 
            // UseLNCB
            // 
            this.UseLNCB.AutoSize = true;
            this.UseLNCB.Location = new System.Drawing.Point(270, 61);
            this.UseLNCB.Name = "UseLNCB";
            this.UseLNCB.Size = new System.Drawing.Size(152, 17);
            this.UseLNCB.TabIndex = 3;
            this.UseLNCB.Text = "Logical Name Referencing";
            this.UseLNCB.UseVisualStyleBackColor = true;
            this.UseLNCB.CheckedChanged += new System.EventHandler(this.UseLNCB_CheckedChanged);
            // 
            // ServerAddressTypeCB
            // 
            this.ServerAddressTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ServerAddressTypeCB.FormattingEnabled = true;
            this.helpProvider1.SetHelpKeyword(this.ServerAddressTypeCB, "AddressType");
            this.helpProvider1.SetHelpNavigator(this.ServerAddressTypeCB, System.Windows.Forms.HelpNavigator.Topic);
            this.ServerAddressTypeCB.Location = new System.Drawing.Point(109, 169);
            this.ServerAddressTypeCB.Name = "ServerAddressTypeCB";
            this.helpProvider1.SetShowHelp(this.ServerAddressTypeCB, true);
            this.ServerAddressTypeCB.Size = new System.Drawing.Size(141, 21);
            this.ServerAddressTypeCB.TabIndex = 10;
            this.ServerAddressTypeCB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ServerAddressTypeCB_DrawItem);
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
            // ClientAddLbl
            // 
            this.ClientAddLbl.AutoSize = true;
            this.ClientAddLbl.Location = new System.Drawing.Point(266, 89);
            this.ClientAddLbl.Name = "ClientAddLbl";
            this.ClientAddLbl.Size = new System.Drawing.Size(77, 13);
            this.ClientAddLbl.TabIndex = 45;
            this.ClientAddLbl.Text = "Client Address:";
            // 
            // ClientAddTB
            // 
            this.helpProvider1.SetHelpKeyword(this.ClientAddTB, "ClientAddress");
            this.helpProvider1.SetHelpNavigator(this.ClientAddTB, System.Windows.Forms.HelpNavigator.Topic);
            this.ClientAddTB.Hexadecimal = true;
            this.ClientAddTB.Location = new System.Drawing.Point(349, 86);
            this.ClientAddTB.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.ClientAddTB.Name = "ClientAddTB";
            this.helpProvider1.SetShowHelp(this.ClientAddTB, true);
            this.ClientAddTB.Size = new System.Drawing.Size(105, 20);
            this.ClientAddTB.TabIndex = 5;
            this.ClientAddTB.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // LogicalServerAddressTB
            // 
            this.helpProvider1.SetHelpKeyword(this.LogicalServerAddressTB, "LogicalServer");
            this.helpProvider1.SetHelpNavigator(this.LogicalServerAddressTB, System.Windows.Forms.HelpNavigator.Topic);
            this.LogicalServerAddressTB.Hexadecimal = true;
            this.LogicalServerAddressTB.Location = new System.Drawing.Point(109, 194);
            this.LogicalServerAddressTB.Maximum = new decimal(new int[] {
            0,
            1,
            0,
            0});
            this.LogicalServerAddressTB.Name = "LogicalServerAddressTB";
            this.helpProvider1.SetShowHelp(this.LogicalServerAddressTB, true);
            this.LogicalServerAddressTB.Size = new System.Drawing.Size(140, 20);
            this.LogicalServerAddressTB.TabIndex = 11;
            this.LogicalServerAddressTB.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // LogicalServerAddressLbl
            // 
            this.LogicalServerAddressLbl.AutoSize = true;
            this.LogicalServerAddressLbl.Location = new System.Drawing.Point(6, 196);
            this.LogicalServerAddressLbl.Name = "LogicalServerAddressLbl";
            this.LogicalServerAddressLbl.Size = new System.Drawing.Size(78, 13);
            this.LogicalServerAddressLbl.TabIndex = 41;
            this.LogicalServerAddressLbl.Text = "Logical Server:";
            // 
            // PhysicalServerAddressTB
            // 
            this.helpProvider1.SetHelpKeyword(this.PhysicalServerAddressTB, "PhysicalServer");
            this.helpProvider1.SetHelpNavigator(this.PhysicalServerAddressTB, System.Windows.Forms.HelpNavigator.Topic);
            this.PhysicalServerAddressTB.Hexadecimal = true;
            this.PhysicalServerAddressTB.Location = new System.Drawing.Point(350, 194);
            this.PhysicalServerAddressTB.Maximum = new decimal(new int[] {
            0,
            1,
            0,
            0});
            this.PhysicalServerAddressTB.Name = "PhysicalServerAddressTB";
            this.helpProvider1.SetShowHelp(this.PhysicalServerAddressTB, true);
            this.PhysicalServerAddressTB.Size = new System.Drawing.Size(104, 20);
            this.PhysicalServerAddressTB.TabIndex = 12;
            this.PhysicalServerAddressTB.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // PhysicalServerAddressLbl
            // 
            this.PhysicalServerAddressLbl.AutoSize = true;
            this.PhysicalServerAddressLbl.Location = new System.Drawing.Point(266, 199);
            this.PhysicalServerAddressLbl.Name = "PhysicalServerAddressLbl";
            this.PhysicalServerAddressLbl.Size = new System.Drawing.Size(83, 13);
            this.PhysicalServerAddressLbl.TabIndex = 39;
            this.PhysicalServerAddressLbl.Text = "Physical Server:";
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
            // ManufacturerCB
            // 
            this.ManufacturerCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ManufacturerCB.FormattingEnabled = true;
            this.helpProvider1.SetHelpKeyword(this.ManufacturerCB, "Manufacturer");
            this.helpProvider1.SetHelpNavigator(this.ManufacturerCB, System.Windows.Forms.HelpNavigator.Topic);
            this.ManufacturerCB.Location = new System.Drawing.Point(109, 31);
            this.ManufacturerCB.Name = "ManufacturerCB";
            this.helpProvider1.SetShowHelp(this.ManufacturerCB, true);
            this.ManufacturerCB.Size = new System.Drawing.Size(344, 21);
            this.ManufacturerCB.TabIndex = 1;
            this.ManufacturerCB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ManufacturerCB_DrawItem);
            this.ManufacturerCB.SelectedIndexChanged += new System.EventHandler(this.ManufacturerCB_SelectedIndexChanged);
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
            // PasswordTB
            // 
            this.helpProvider1.SetHelpKeyword(this.PasswordTB, "Password");
            this.helpProvider1.SetHelpNavigator(this.PasswordTB, System.Windows.Forms.HelpNavigator.Topic);
            this.helpProvider1.SetHelpString(this.PasswordTB, "Authentication is not used.");
            this.PasswordTB.Location = new System.Drawing.Point(109, 115);
            this.PasswordTB.Name = "PasswordTB";
            this.helpProvider1.SetShowHelp(this.PasswordTB, true);
            this.PasswordTB.Size = new System.Drawing.Size(139, 20);
            this.PasswordTB.TabIndex = 6;
            // 
            // NameTB
            // 
            this.helpProvider1.SetHelpKeyword(this.NameTB, "DeviceName");
            this.helpProvider1.SetHelpNavigator(this.NameTB, System.Windows.Forms.HelpNavigator.Topic);
            this.NameTB.Location = new System.Drawing.Point(109, 5);
            this.NameTB.Name = "NameTB";
            this.helpProvider1.SetShowHelp(this.NameTB, true);
            this.NameTB.Size = new System.Drawing.Size(345, 20);
            this.NameTB.TabIndex = 0;
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
            // AuthenticationCB
            // 
            this.AuthenticationCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AuthenticationCB.FormattingEnabled = true;
            this.helpProvider1.SetHelpKeyword(this.AuthenticationCB, "Authentication");
            this.helpProvider1.SetHelpNavigator(this.AuthenticationCB, System.Windows.Forms.HelpNavigator.Topic);
            this.AuthenticationCB.Location = new System.Drawing.Point(109, 85);
            this.AuthenticationCB.Name = "AuthenticationCB";
            this.helpProvider1.SetShowHelp(this.AuthenticationCB, true);
            this.AuthenticationCB.Size = new System.Drawing.Size(139, 21);
            this.AuthenticationCB.TabIndex = 4;
            this.AuthenticationCB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.AuthenticationCB_DrawItem);
            this.AuthenticationCB.SelectedIndexChanged += new System.EventHandler(this.AuthenticationCB_SelectedIndexChanged);
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
            // NameLbl
            // 
            this.NameLbl.AutoSize = true;
            this.NameLbl.Location = new System.Drawing.Point(8, 8);
            this.NameLbl.Name = "NameLbl";
            this.NameLbl.Size = new System.Drawing.Size(38, 13);
            this.NameLbl.TabIndex = 32;
            this.NameLbl.Text = "Name:";
            // 
            // MediasCB
            // 
            this.MediasCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MediasCB.FormattingEnabled = true;
            this.helpProvider1.SetHelpKeyword(this.MediasCB, "Media");
            this.helpProvider1.SetHelpNavigator(this.MediasCB, System.Windows.Forms.HelpNavigator.Topic);
            this.MediasCB.Location = new System.Drawing.Point(109, 219);
            this.MediasCB.Name = "MediasCB";
            this.helpProvider1.SetShowHelp(this.MediasCB, true);
            this.MediasCB.Size = new System.Drawing.Size(141, 21);
            this.MediasCB.TabIndex = 13;
            this.MediasCB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.MediasCB_DrawItem);
            this.MediasCB.SelectedIndexChanged += new System.EventHandler(this.MediasCB_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 222);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Media:";
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
            this.NetworkSettingsGB.Location = new System.Drawing.Point(6, 243);
            this.NetworkSettingsGB.Name = "NetworkSettingsGB";
            this.NetworkSettingsGB.Size = new System.Drawing.Size(457, 99);
            this.NetworkSettingsGB.TabIndex = 34;
            this.NetworkSettingsGB.TabStop = false;
            this.NetworkSettingsGB.Text = "Settings";
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Protocol:";
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
            // PortTB
            // 
            this.PortTB.Location = new System.Drawing.Point(101, 45);
            this.PortTB.Name = "PortTB";
            this.PortTB.Size = new System.Drawing.Size(274, 20);
            this.PortTB.TabIndex = 12;
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
            // HostNameTB
            // 
            this.HostNameTB.Location = new System.Drawing.Point(101, 19);
            this.HostNameTB.Name = "HostNameTB";
            this.HostNameTB.Size = new System.Drawing.Size(274, 20);
            this.HostNameTB.TabIndex = 11;
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
            this.TerminalSettingsGB.Size = new System.Drawing.Size(431, 99);
            this.TerminalSettingsGB.TabIndex = 46;
            this.TerminalSettingsGB.TabStop = false;
            this.TerminalSettingsGB.Text = "Settings";
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
            // TerminalPortCB
            // 
            this.TerminalPortCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TerminalPortCB.FormattingEnabled = true;
            this.TerminalPortCB.Location = new System.Drawing.Point(101, 45);
            this.TerminalPortCB.Name = "TerminalPortCB";
            this.TerminalPortCB.Size = new System.Drawing.Size(139, 21);
            this.TerminalPortCB.TabIndex = 12;
            // 
            // TerminalPhoneNumberTB
            // 
            this.TerminalPhoneNumberTB.Location = new System.Drawing.Point(101, 19);
            this.TerminalPhoneNumberTB.Name = "TerminalPhoneNumberTB";
            this.TerminalPhoneNumberTB.Size = new System.Drawing.Size(274, 20);
            this.TerminalPhoneNumberTB.TabIndex = 11;
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
            this.SerialSettingsGB.Size = new System.Drawing.Size(381, 89);
            this.SerialSettingsGB.TabIndex = 35;
            this.SerialSettingsGB.TabStop = false;
            this.SerialSettingsGB.Text = "Settings";
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
            // MaximumBaudRateCB
            // 
            this.MaximumBaudRateCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MaximumBaudRateCB.FormattingEnabled = true;
            this.MaximumBaudRateCB.Location = new System.Drawing.Point(263, 50);
            this.MaximumBaudRateCB.Name = "MaximumBaudRateCB";
            this.MaximumBaudRateCB.Size = new System.Drawing.Size(87, 21);
            this.MaximumBaudRateCB.TabIndex = 54;
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
            // SerialPortLbl
            // 
            this.SerialPortLbl.AutoSize = true;
            this.SerialPortLbl.Location = new System.Drawing.Point(7, 22);
            this.SerialPortLbl.Name = "SerialPortLbl";
            this.SerialPortLbl.Size = new System.Drawing.Size(58, 13);
            this.SerialPortLbl.TabIndex = 11;
            this.SerialPortLbl.Text = "Serial Port:";
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
            // SigningKeyCb
            // 
            this.SigningKeyCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SigningKeyCb.FormattingEnabled = true;
            this.helpProvider1.SetHelpKeyword(this.SigningKeyCb, "Authentication");
            this.helpProvider1.SetHelpNavigator(this.SigningKeyCb, System.Windows.Forms.HelpNavigator.Topic);
            this.SigningKeyCb.Location = new System.Drawing.Point(109, 116);
            this.SigningKeyCb.Name = "SigningKeyCb";
            this.helpProvider1.SetShowHelp(this.SigningKeyCb, true);
            this.SigningKeyCb.Size = new System.Drawing.Size(345, 21);
            this.SigningKeyCb.TabIndex = 64;
            this.SigningKeyCb.SelectedIndexChanged += new System.EventHandler(this.SigningKeyCb_SelectedIndexChanged);
            this.SigningKeyCb.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.SigningKeyCb_Format);
            // 
            // DeviceTab
            // 
            this.DeviceTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DeviceTab.Controls.Add(this.DeviceSettingsTab);
            this.DeviceTab.Controls.Add(this.AdvancedTab);
            this.DeviceTab.Controls.Add(this.HdlcFrameTab);
            this.DeviceTab.Controls.Add(this.PlcFrame);
            this.DeviceTab.Controls.Add(this.PduFrame);
            this.DeviceTab.Controls.Add(this.SupportedServicesTab);
            this.DeviceTab.Controls.Add(this.CipheringTab);
            this.DeviceTab.Controls.Add(this.DelaysTab);
            this.DeviceTab.Controls.Add(this.GatewayTab);
            this.DeviceTab.Controls.Add(this.XmlTab);
            this.helpProvider1.SetHelpKeyword(this.DeviceTab, "DeviceProperties");
            this.helpProvider1.SetHelpNavigator(this.DeviceTab, System.Windows.Forms.HelpNavigator.Topic);
            this.DeviceTab.Location = new System.Drawing.Point(0, 0);
            this.DeviceTab.Name = "DeviceTab";
            this.DeviceTab.SelectedIndex = 0;
            this.helpProvider1.SetShowHelp(this.DeviceTab, true);
            this.DeviceTab.Size = new System.Drawing.Size(519, 414);
            this.DeviceTab.TabIndex = 0;
            this.DeviceTab.SelectedIndexChanged += new System.EventHandler(this.DeviceTab_SelectedIndexChanged);
            // 
            // AdvancedTab
            // 
            this.AdvancedTab.Controls.Add(this.IncreaseInvocationCounterForGMacAuthenticationCB);
            this.AdvancedTab.Controls.Add(this.OverwriteAttributeAccessRightsCb);
            this.AdvancedTab.Controls.Add(this.GBTWindowSizeTb);
            this.AdvancedTab.Controls.Add(this.GBTWindowSizeLbl);
            this.AdvancedTab.Controls.Add(this.SignInitiateRequestResponseCb);
            this.AdvancedTab.Controls.Add(this.IncludePublicKeyCb);
            this.AdvancedTab.Controls.Add(this.ChallengeSizeCb);
            this.AdvancedTab.Controls.Add(this.ChallengeSizeLbl);
            this.AdvancedTab.Controls.Add(this.SecurityChangeCheckCb);
            this.AdvancedTab.Controls.Add(this.IgnoreTimeStatusCb);
            this.AdvancedTab.Controls.Add(this.IgnoreTimeZoneCb);
            this.AdvancedTab.Controls.Add(this.UseProtectedReleaseCb);
            this.AdvancedTab.Controls.Add(this.InactivityTimeoutTb);
            this.AdvancedTab.Controls.Add(this.InactivityTimeoutLbl);
            this.AdvancedTab.Controls.Add(this.ServiceClassCb);
            this.AdvancedTab.Controls.Add(this.StandardCb);
            this.AdvancedTab.Controls.Add(this.PriorityCb);
            this.AdvancedTab.Controls.Add(this.ServiceClassLbl);
            this.AdvancedTab.Controls.Add(this.StandardLbl);
            this.AdvancedTab.Controls.Add(this.PriorityLbl);
            this.AdvancedTab.Controls.Add(this.UserIdTb);
            this.AdvancedTab.Controls.Add(this.UserIDLbl);
            this.AdvancedTab.Controls.Add(this.MaxPduTb);
            this.AdvancedTab.Controls.Add(this.MaxPduLbl);
            this.AdvancedTab.Controls.Add(this.UseUtcTimeZone);
            this.helpProvider1.SetHelpKeyword(this.AdvancedTab, "advanced");
            this.helpProvider1.SetHelpNavigator(this.AdvancedTab, System.Windows.Forms.HelpNavigator.Topic);
            this.AdvancedTab.Location = new System.Drawing.Point(4, 22);
            this.AdvancedTab.Name = "AdvancedTab";
            this.AdvancedTab.Padding = new System.Windows.Forms.Padding(3);
            this.helpProvider1.SetShowHelp(this.AdvancedTab, true);
            this.AdvancedTab.Size = new System.Drawing.Size(511, 388);
            this.AdvancedTab.TabIndex = 3;
            this.AdvancedTab.Text = "Advanced";
            this.AdvancedTab.UseVisualStyleBackColor = true;
            // 
            // IncreaseInvocationCounterForGMacAuthenticationCB
            // 
            this.IncreaseInvocationCounterForGMacAuthenticationCB.AutoSize = true;
            this.helpProvider1.SetHelpString(this.IncreaseInvocationCounterForGMacAuthenticationCB, "Some meters expect that Invocation Counter is increased for GMAC Authentication w" +
        "hen connection is established.");
            this.IncreaseInvocationCounterForGMacAuthenticationCB.Location = new System.Drawing.Point(14, 365);
            this.IncreaseInvocationCounterForGMacAuthenticationCB.Name = "IncreaseInvocationCounterForGMacAuthenticationCB";
            this.helpProvider1.SetShowHelp(this.IncreaseInvocationCounterForGMacAuthenticationCB, true);
            this.IncreaseInvocationCounterForGMacAuthenticationCB.Size = new System.Drawing.Size(281, 17);
            this.IncreaseInvocationCounterForGMacAuthenticationCB.TabIndex = 69;
            this.IncreaseInvocationCounterForGMacAuthenticationCB.Text = "Increase invocation counter for GMAC Authentication ";
            this.IncreaseInvocationCounterForGMacAuthenticationCB.UseVisualStyleBackColor = true;
            // 
            // OverwriteAttributeAccessRightsCb
            // 
            this.OverwriteAttributeAccessRightsCb.AutoSize = true;
            this.helpProvider1.SetHelpString(this.OverwriteAttributeAccessRightsCb, "Is protected release used.");
            this.OverwriteAttributeAccessRightsCb.Location = new System.Drawing.Point(14, 347);
            this.OverwriteAttributeAccessRightsCb.Name = "OverwriteAttributeAccessRightsCb";
            this.helpProvider1.SetShowHelp(this.OverwriteAttributeAccessRightsCb, true);
            this.OverwriteAttributeAccessRightsCb.Size = new System.Drawing.Size(177, 17);
            this.OverwriteAttributeAccessRightsCb.TabIndex = 68;
            this.OverwriteAttributeAccessRightsCb.Text = "Overwrite attribute access rights";
            this.OverwriteAttributeAccessRightsCb.UseVisualStyleBackColor = true;
            // 
            // GBTWindowSizeTb
            // 
            this.helpProvider1.SetHelpKeyword(this.GBTWindowSizeTb, "MaxPdu");
            this.helpProvider1.SetHelpNavigator(this.GBTWindowSizeTb, System.Windows.Forms.HelpNavigator.Topic);
            this.GBTWindowSizeTb.Location = new System.Drawing.Point(154, 153);
            this.GBTWindowSizeTb.Name = "GBTWindowSizeTb";
            this.helpProvider1.SetShowHelp(this.GBTWindowSizeTb, true);
            this.GBTWindowSizeTb.Size = new System.Drawing.Size(93, 20);
            this.GBTWindowSizeTb.TabIndex = 66;
            // 
            // GBTWindowSizeLbl
            // 
            this.GBTWindowSizeLbl.AutoSize = true;
            this.GBTWindowSizeLbl.Location = new System.Drawing.Point(11, 157);
            this.GBTWindowSizeLbl.Name = "GBTWindowSizeLbl";
            this.GBTWindowSizeLbl.Size = new System.Drawing.Size(95, 13);
            this.GBTWindowSizeLbl.TabIndex = 67;
            this.GBTWindowSizeLbl.Text = "GBT Window size:";
            // 
            // SignInitiateRequestResponseCb
            // 
            this.SignInitiateRequestResponseCb.AutoSize = true;
            this.helpProvider1.SetHelpString(this.SignInitiateRequestResponseCb, "Is protected release used.");
            this.SignInitiateRequestResponseCb.Location = new System.Drawing.Point(14, 324);
            this.SignInitiateRequestResponseCb.Name = "SignInitiateRequestResponseCb";
            this.helpProvider1.SetShowHelp(this.SignInitiateRequestResponseCb, true);
            this.SignInitiateRequestResponseCb.Size = new System.Drawing.Size(186, 17);
            this.SignInitiateRequestResponseCb.TabIndex = 65;
            this.SignInitiateRequestResponseCb.Text = "Sign Initiate request and response";
            this.SignInitiateRequestResponseCb.UseVisualStyleBackColor = true;
            // 
            // IncludePublicKeyCb
            // 
            this.IncludePublicKeyCb.AutoSize = true;
            this.helpProvider1.SetHelpNavigator(this.IncludePublicKeyCb, System.Windows.Forms.HelpNavigator.Topic);
            this.helpProvider1.SetHelpString(this.IncludePublicKeyCb, "SendPublicKeyCertificateInInitialize");
            this.IncludePublicKeyCb.Location = new System.Drawing.Point(14, 301);
            this.IncludePublicKeyCb.Name = "IncludePublicKeyCb";
            this.helpProvider1.SetShowHelp(this.IncludePublicKeyCb, true);
            this.IncludePublicKeyCb.Size = new System.Drawing.Size(203, 17);
            this.IncludePublicKeyCb.TabIndex = 64;
            this.IncludePublicKeyCb.Text = "Send Public Key certificate in initialize";
            this.IncludePublicKeyCb.UseVisualStyleBackColor = true;
            // 
            // ChallengeSizeCb
            // 
            this.ChallengeSizeCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.helpProvider1.SetHelpKeyword(this.ChallengeSizeCb, "ChallengeSize");
            this.helpProvider1.SetHelpNavigator(this.ChallengeSizeCb, System.Windows.Forms.HelpNavigator.Topic);
            this.ChallengeSizeCb.Location = new System.Drawing.Point(154, 272);
            this.ChallengeSizeCb.Name = "ChallengeSizeCb";
            this.helpProvider1.SetShowHelp(this.ChallengeSizeCb, true);
            this.ChallengeSizeCb.Size = new System.Drawing.Size(93, 21);
            this.ChallengeSizeCb.TabIndex = 63;
            // 
            // ChallengeSizeLbl
            // 
            this.ChallengeSizeLbl.AutoSize = true;
            this.ChallengeSizeLbl.Location = new System.Drawing.Point(11, 280);
            this.ChallengeSizeLbl.Name = "ChallengeSizeLbl";
            this.ChallengeSizeLbl.Size = new System.Drawing.Size(80, 13);
            this.ChallengeSizeLbl.TabIndex = 62;
            this.ChallengeSizeLbl.Text = "Challenge Size:";
            // 
            // SecurityChangeCheckCb
            // 
            this.SecurityChangeCheckCb.AutoSize = true;
            this.helpProvider1.SetHelpNavigator(this.SecurityChangeCheckCb, System.Windows.Forms.HelpNavigator.Topic);
            this.helpProvider1.SetHelpString(this.SecurityChangeCheckCb, "SecurityChangeCheck");
            this.SecurityChangeCheckCb.Location = new System.Drawing.Point(11, 257);
            this.SecurityChangeCheckCb.Name = "SecurityChangeCheckCb";
            this.helpProvider1.SetShowHelp(this.SecurityChangeCheckCb, true);
            this.SecurityChangeCheckCb.Size = new System.Drawing.Size(136, 17);
            this.SecurityChangeCheckCb.TabIndex = 60;
            this.SecurityChangeCheckCb.Text = "Security change check";
            this.SecurityChangeCheckCb.UseVisualStyleBackColor = true;
            // 
            // IgnoreTimeStatusCb
            // 
            this.IgnoreTimeStatusCb.AutoSize = true;
            this.helpProvider1.SetHelpKeyword(this.IgnoreTimeStatusCb, "IgnoreTimeStatus");
            this.helpProvider1.SetHelpNavigator(this.IgnoreTimeStatusCb, System.Windows.Forms.HelpNavigator.Topic);
            this.helpProvider1.SetHelpString(this.IgnoreTimeStatusCb, "");
            this.IgnoreTimeStatusCb.Location = new System.Drawing.Point(130, 108);
            this.IgnoreTimeStatusCb.Name = "IgnoreTimeStatusCb";
            this.helpProvider1.SetShowHelp(this.IgnoreTimeStatusCb, true);
            this.IgnoreTimeStatusCb.Size = new System.Drawing.Size(115, 17);
            this.IgnoreTimeStatusCb.TabIndex = 5;
            this.IgnoreTimeStatusCb.Text = "Ignore Time Status";
            this.IgnoreTimeStatusCb.UseVisualStyleBackColor = true;
            // 
            // IgnoreTimeZoneCb
            // 
            this.IgnoreTimeZoneCb.AutoSize = true;
            this.helpProvider1.SetHelpKeyword(this.IgnoreTimeZoneCb, "IgnoreTimeZone");
            this.helpProvider1.SetHelpNavigator(this.IgnoreTimeZoneCb, System.Windows.Forms.HelpNavigator.Topic);
            this.helpProvider1.SetHelpString(this.IgnoreTimeZoneCb, "");
            this.IgnoreTimeZoneCb.Location = new System.Drawing.Point(14, 108);
            this.IgnoreTimeZoneCb.Name = "IgnoreTimeZoneCb";
            this.helpProvider1.SetShowHelp(this.IgnoreTimeZoneCb, true);
            this.IgnoreTimeZoneCb.Size = new System.Drawing.Size(110, 17);
            this.IgnoreTimeZoneCb.TabIndex = 4;
            this.IgnoreTimeZoneCb.Text = "Ignore Time Zone";
            this.IgnoreTimeZoneCb.UseVisualStyleBackColor = true;
            // 
            // UseProtectedReleaseCb
            // 
            this.UseProtectedReleaseCb.AutoSize = true;
            this.helpProvider1.SetHelpNavigator(this.UseProtectedReleaseCb, System.Windows.Forms.HelpNavigator.Topic);
            this.helpProvider1.SetHelpString(this.UseProtectedReleaseCb, "UseProtectedRelease");
            this.UseProtectedReleaseCb.Location = new System.Drawing.Point(11, 234);
            this.UseProtectedReleaseCb.Name = "UseProtectedReleaseCb";
            this.helpProvider1.SetShowHelp(this.UseProtectedReleaseCb, true);
            this.UseProtectedReleaseCb.Size = new System.Drawing.Size(130, 17);
            this.UseProtectedReleaseCb.TabIndex = 59;
            this.UseProtectedReleaseCb.Text = "Use protected release";
            this.UseProtectedReleaseCb.UseVisualStyleBackColor = true;
            // 
            // InactivityTimeoutTb
            // 
            this.helpProvider1.SetHelpKeyword(this.InactivityTimeoutTb, "InactivityTimeout");
            this.helpProvider1.SetHelpNavigator(this.InactivityTimeoutTb, System.Windows.Forms.HelpNavigator.Topic);
            this.InactivityTimeoutTb.Location = new System.Drawing.Point(154, 6);
            this.InactivityTimeoutTb.Name = "InactivityTimeoutTb";
            this.helpProvider1.SetShowHelp(this.InactivityTimeoutTb, true);
            this.InactivityTimeoutTb.Size = new System.Drawing.Size(93, 20);
            this.InactivityTimeoutTb.TabIndex = 0;
            this.InactivityTimeoutTb.TextChanged += new System.EventHandler(this.InactivityTimeoutTb_TextChanged);
            // 
            // InactivityTimeoutLbl
            // 
            this.InactivityTimeoutLbl.AutoSize = true;
            this.InactivityTimeoutLbl.Location = new System.Drawing.Point(11, 10);
            this.InactivityTimeoutLbl.Name = "InactivityTimeoutLbl";
            this.InactivityTimeoutLbl.Size = new System.Drawing.Size(86, 13);
            this.InactivityTimeoutLbl.TabIndex = 46;
            this.InactivityTimeoutLbl.Text = "Inactivity timeout";
            // 
            // ServiceClassCb
            // 
            this.ServiceClassCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.helpProvider1.SetHelpKeyword(this.ServiceClassCb, "ServiceClass");
            this.helpProvider1.SetHelpNavigator(this.ServiceClassCb, System.Windows.Forms.HelpNavigator.Topic);
            this.ServiceClassCb.Location = new System.Drawing.Point(154, 62);
            this.ServiceClassCb.Name = "ServiceClassCb";
            this.helpProvider1.SetShowHelp(this.ServiceClassCb, true);
            this.ServiceClassCb.Size = new System.Drawing.Size(93, 21);
            this.ServiceClassCb.TabIndex = 2;
            // 
            // StandardCb
            // 
            this.StandardCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.helpProvider1.SetHelpKeyword(this.StandardCb, "Standard");
            this.helpProvider1.SetHelpNavigator(this.StandardCb, System.Windows.Forms.HelpNavigator.Topic);
            this.StandardCb.Location = new System.Drawing.Point(154, 204);
            this.StandardCb.Name = "StandardCb";
            this.helpProvider1.SetShowHelp(this.StandardCb, true);
            this.StandardCb.Size = new System.Drawing.Size(93, 21);
            this.StandardCb.TabIndex = 8;
            this.StandardCb.SelectedIndexChanged += new System.EventHandler(this.StandardCb_SelectedIndexChanged);
            // 
            // PriorityCb
            // 
            this.PriorityCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.helpProvider1.SetHelpKeyword(this.PriorityCb, "Priority");
            this.helpProvider1.SetHelpNavigator(this.PriorityCb, System.Windows.Forms.HelpNavigator.Topic);
            this.PriorityCb.Location = new System.Drawing.Point(154, 32);
            this.PriorityCb.Name = "PriorityCb";
            this.helpProvider1.SetShowHelp(this.PriorityCb, true);
            this.PriorityCb.Size = new System.Drawing.Size(93, 21);
            this.PriorityCb.TabIndex = 1;
            // 
            // ServiceClassLbl
            // 
            this.ServiceClassLbl.AutoSize = true;
            this.ServiceClassLbl.Location = new System.Drawing.Point(11, 62);
            this.ServiceClassLbl.Name = "ServiceClassLbl";
            this.ServiceClassLbl.Size = new System.Drawing.Size(74, 13);
            this.ServiceClassLbl.TabIndex = 50;
            this.ServiceClassLbl.Text = "Service Class:";
            // 
            // StandardLbl
            // 
            this.StandardLbl.AutoSize = true;
            this.StandardLbl.Location = new System.Drawing.Point(11, 208);
            this.StandardLbl.Name = "StandardLbl";
            this.StandardLbl.Size = new System.Drawing.Size(53, 13);
            this.StandardLbl.TabIndex = 58;
            this.StandardLbl.Text = "Standard:";
            // 
            // PriorityLbl
            // 
            this.PriorityLbl.AutoSize = true;
            this.PriorityLbl.Location = new System.Drawing.Point(11, 36);
            this.PriorityLbl.Name = "PriorityLbl";
            this.PriorityLbl.Size = new System.Drawing.Size(41, 13);
            this.PriorityLbl.TabIndex = 48;
            this.PriorityLbl.Text = "Priority:";
            // 
            // UserIdTb
            // 
            this.helpProvider1.SetHelpKeyword(this.UserIdTb, "UserId");
            this.helpProvider1.SetHelpNavigator(this.UserIdTb, System.Windows.Forms.HelpNavigator.Topic);
            this.UserIdTb.Location = new System.Drawing.Point(154, 178);
            this.UserIdTb.Name = "UserIdTb";
            this.helpProvider1.SetShowHelp(this.UserIdTb, true);
            this.UserIdTb.Size = new System.Drawing.Size(93, 20);
            this.UserIdTb.TabIndex = 7;
            // 
            // UserIDLbl
            // 
            this.UserIDLbl.AutoSize = true;
            this.UserIDLbl.Location = new System.Drawing.Point(11, 182);
            this.UserIDLbl.Name = "UserIDLbl";
            this.UserIDLbl.Size = new System.Drawing.Size(46, 13);
            this.UserIDLbl.TabIndex = 54;
            this.UserIDLbl.Text = "User ID:";
            // 
            // MaxPduTb
            // 
            this.helpProvider1.SetHelpKeyword(this.MaxPduTb, "MaxPdu");
            this.helpProvider1.SetHelpNavigator(this.MaxPduTb, System.Windows.Forms.HelpNavigator.Topic);
            this.MaxPduTb.Location = new System.Drawing.Point(154, 127);
            this.MaxPduTb.Name = "MaxPduTb";
            this.helpProvider1.SetShowHelp(this.MaxPduTb, true);
            this.MaxPduTb.Size = new System.Drawing.Size(93, 20);
            this.MaxPduTb.TabIndex = 6;
            // 
            // MaxPduLbl
            // 
            this.MaxPduLbl.AutoSize = true;
            this.MaxPduLbl.Location = new System.Drawing.Point(11, 131);
            this.MaxPduLbl.Name = "MaxPduLbl";
            this.MaxPduLbl.Size = new System.Drawing.Size(77, 13);
            this.MaxPduLbl.TabIndex = 48;
            this.MaxPduLbl.Text = "Max PDU size:";
            // 
            // UseUtcTimeZone
            // 
            this.UseUtcTimeZone.AutoSize = true;
            this.helpProvider1.SetHelpKeyword(this.UseUtcTimeZone, "UseUTC");
            this.helpProvider1.SetHelpNavigator(this.UseUtcTimeZone, System.Windows.Forms.HelpNavigator.Topic);
            this.UseUtcTimeZone.Location = new System.Drawing.Point(14, 89);
            this.UseUtcTimeZone.Name = "UseUtcTimeZone";
            this.helpProvider1.SetShowHelp(this.UseUtcTimeZone, true);
            this.UseUtcTimeZone.Size = new System.Drawing.Size(216, 17);
            this.UseUtcTimeZone.TabIndex = 3;
            this.UseUtcTimeZone.Text = "Use UTC time zone, not DLMS standard";
            this.UseUtcTimeZone.UseVisualStyleBackColor = true;
            // 
            // HdlcFrameTab
            // 
            this.HdlcFrameTab.Controls.Add(this.HdlcGroup);
            this.HdlcFrameTab.Location = new System.Drawing.Point(4, 22);
            this.HdlcFrameTab.Name = "HdlcFrameTab";
            this.HdlcFrameTab.Padding = new System.Windows.Forms.Padding(3);
            this.HdlcFrameTab.Size = new System.Drawing.Size(511, 388);
            this.HdlcFrameTab.TabIndex = 7;
            this.HdlcFrameTab.Text = "Frame";
            this.HdlcFrameTab.UseVisualStyleBackColor = true;
            // 
            // HdlcGroup
            // 
            this.HdlcGroup.Controls.Add(this.FrameSizeCb);
            this.HdlcGroup.Controls.Add(this.WindowSizeRXTb);
            this.HdlcGroup.Controls.Add(this.WindowSizeRXLbl);
            this.HdlcGroup.Controls.Add(this.WindowSizeTXTb);
            this.HdlcGroup.Controls.Add(this.WindowSizeTXLbl);
            this.HdlcGroup.Controls.Add(this.MaxInfoRXTb);
            this.HdlcGroup.Controls.Add(this.MaxInfoRXLbl);
            this.HdlcGroup.Controls.Add(this.MaxInfoTXTb);
            this.HdlcGroup.Controls.Add(this.MaxInfoTXLbl);
            this.HdlcGroup.Controls.Add(this.ServerAddressSizeCb);
            this.HdlcGroup.Controls.Add(this.label5);
            this.helpProvider1.SetHelpKeyword(this.HdlcGroup, "advanced");
            this.helpProvider1.SetHelpNavigator(this.HdlcGroup, System.Windows.Forms.HelpNavigator.Topic);
            this.HdlcGroup.Location = new System.Drawing.Point(6, 6);
            this.HdlcGroup.Name = "HdlcGroup";
            this.helpProvider1.SetShowHelp(this.HdlcGroup, true);
            this.HdlcGroup.Size = new System.Drawing.Size(375, 149);
            this.HdlcGroup.TabIndex = 1;
            this.HdlcGroup.TabStop = false;
            this.HdlcGroup.Text = "HDLC settings";
            // 
            // FrameSizeCb
            // 
            this.FrameSizeCb.AutoSize = true;
            this.helpProvider1.SetHelpString(this.FrameSizeCb, "Is password given in ASCII or Hex.");
            this.FrameSizeCb.Location = new System.Drawing.Point(267, 23);
            this.FrameSizeCb.Name = "FrameSizeCb";
            this.helpProvider1.SetShowHelp(this.FrameSizeCb, true);
            this.FrameSizeCb.Size = new System.Drawing.Size(95, 17);
            this.FrameSizeCb.TabIndex = 58;
            this.FrameSizeCb.Text = "Use frame size";
            this.FrameSizeCb.UseVisualStyleBackColor = true;
            // 
            // WindowSizeRXTb
            // 
            this.helpProvider1.SetHelpKeyword(this.WindowSizeRXTb, "WindowSizeRX");
            this.helpProvider1.SetHelpNavigator(this.WindowSizeRXTb, System.Windows.Forms.HelpNavigator.Topic);
            this.WindowSizeRXTb.Location = new System.Drawing.Point(153, 97);
            this.WindowSizeRXTb.Name = "WindowSizeRXTb";
            this.helpProvider1.SetShowHelp(this.WindowSizeRXTb, true);
            this.WindowSizeRXTb.Size = new System.Drawing.Size(93, 20);
            this.WindowSizeRXTb.TabIndex = 3;
            // 
            // WindowSizeRXLbl
            // 
            this.WindowSizeRXLbl.AutoSize = true;
            this.WindowSizeRXLbl.Location = new System.Drawing.Point(10, 101);
            this.WindowSizeRXLbl.Name = "WindowSizeRXLbl";
            this.WindowSizeRXLbl.Size = new System.Drawing.Size(116, 13);
            this.WindowSizeRXLbl.TabIndex = 44;
            this.WindowSizeRXLbl.Text = "Window size in receive";
            // 
            // WindowSizeTXTb
            // 
            this.helpProvider1.SetHelpKeyword(this.WindowSizeTXTb, "WindowSizeTX");
            this.helpProvider1.SetHelpNavigator(this.WindowSizeTXTb, System.Windows.Forms.HelpNavigator.Topic);
            this.WindowSizeTXTb.Location = new System.Drawing.Point(153, 71);
            this.WindowSizeTXTb.Name = "WindowSizeTXTb";
            this.helpProvider1.SetShowHelp(this.WindowSizeTXTb, true);
            this.WindowSizeTXTb.Size = new System.Drawing.Size(93, 20);
            this.WindowSizeTXTb.TabIndex = 2;
            // 
            // WindowSizeTXLbl
            // 
            this.WindowSizeTXLbl.AutoSize = true;
            this.WindowSizeTXLbl.Location = new System.Drawing.Point(10, 75);
            this.WindowSizeTXLbl.Name = "WindowSizeTXLbl";
            this.WindowSizeTXLbl.Size = new System.Drawing.Size(117, 13);
            this.WindowSizeTXLbl.TabIndex = 42;
            this.WindowSizeTXLbl.Text = "Window size in transmit";
            // 
            // MaxInfoRXTb
            // 
            this.helpProvider1.SetHelpKeyword(this.MaxInfoRXTb, "MaxInfoRX");
            this.helpProvider1.SetHelpNavigator(this.MaxInfoRXTb, System.Windows.Forms.HelpNavigator.Topic);
            this.MaxInfoRXTb.Location = new System.Drawing.Point(153, 45);
            this.MaxInfoRXTb.Name = "MaxInfoRXTb";
            this.helpProvider1.SetShowHelp(this.MaxInfoRXTb, true);
            this.MaxInfoRXTb.Size = new System.Drawing.Size(93, 20);
            this.MaxInfoRXTb.TabIndex = 1;
            // 
            // MaxInfoRXLbl
            // 
            this.MaxInfoRXLbl.AutoSize = true;
            this.MaxInfoRXLbl.Location = new System.Drawing.Point(10, 48);
            this.MaxInfoRXLbl.Name = "MaxInfoRXLbl";
            this.MaxInfoRXLbl.Size = new System.Drawing.Size(137, 13);
            this.MaxInfoRXLbl.TabIndex = 40;
            this.MaxInfoRXLbl.Text = "Max payload size in receive";
            // 
            // MaxInfoTXTb
            // 
            this.helpProvider1.SetHelpKeyword(this.MaxInfoTXTb, "MaxInfoTX");
            this.helpProvider1.SetHelpNavigator(this.MaxInfoTXTb, System.Windows.Forms.HelpNavigator.Topic);
            this.MaxInfoTXTb.Location = new System.Drawing.Point(153, 19);
            this.MaxInfoTXTb.Name = "MaxInfoTXTb";
            this.helpProvider1.SetShowHelp(this.MaxInfoTXTb, true);
            this.MaxInfoTXTb.Size = new System.Drawing.Size(93, 20);
            this.MaxInfoTXTb.TabIndex = 0;
            // 
            // MaxInfoTXLbl
            // 
            this.MaxInfoTXLbl.AutoSize = true;
            this.MaxInfoTXLbl.Location = new System.Drawing.Point(10, 23);
            this.MaxInfoTXLbl.Name = "MaxInfoTXLbl";
            this.MaxInfoTXLbl.Size = new System.Drawing.Size(138, 13);
            this.MaxInfoTXLbl.TabIndex = 38;
            this.MaxInfoTXLbl.Text = "Max payload size in transmit";
            // 
            // ServerAddressSizeCb
            // 
            this.ServerAddressSizeCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.helpProvider1.SetHelpKeyword(this.ServerAddressSizeCb, "AddressSize");
            this.helpProvider1.SetHelpNavigator(this.ServerAddressSizeCb, System.Windows.Forms.HelpNavigator.Topic);
            this.ServerAddressSizeCb.Location = new System.Drawing.Point(153, 122);
            this.ServerAddressSizeCb.Name = "ServerAddressSizeCb";
            this.helpProvider1.SetShowHelp(this.ServerAddressSizeCb, true);
            this.ServerAddressSizeCb.Size = new System.Drawing.Size(93, 21);
            this.ServerAddressSizeCb.TabIndex = 57;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 56;
            this.label5.Text = "Server address size:";
            // 
            // PlcFrame
            // 
            this.PlcFrame.Controls.Add(this.PlcGroup);
            this.PlcFrame.Location = new System.Drawing.Point(4, 22);
            this.PlcFrame.Name = "PlcFrame";
            this.PlcFrame.Size = new System.Drawing.Size(511, 388);
            this.PlcFrame.TabIndex = 8;
            this.PlcFrame.Text = "Frame";
            this.PlcFrame.UseVisualStyleBackColor = true;
            // 
            // PlcGroup
            // 
            this.PlcGroup.Controls.Add(this.MACTargetAddressTb);
            this.PlcGroup.Controls.Add(this.MACSourceAddressTb);
            this.PlcGroup.Controls.Add(this.MACTargetAddressLbl);
            this.PlcGroup.Controls.Add(this.MACSourceAddressLbl);
            this.helpProvider1.SetHelpKeyword(this.PlcGroup, "advanced");
            this.helpProvider1.SetHelpNavigator(this.PlcGroup, System.Windows.Forms.HelpNavigator.Topic);
            this.PlcGroup.Location = new System.Drawing.Point(8, 3);
            this.PlcGroup.Name = "PlcGroup";
            this.helpProvider1.SetShowHelp(this.PlcGroup, true);
            this.PlcGroup.Size = new System.Drawing.Size(375, 149);
            this.PlcGroup.TabIndex = 60;
            this.PlcGroup.TabStop = false;
            this.PlcGroup.Text = "PLC settings";
            // 
            // MACTargetAddressTb
            // 
            this.helpProvider1.SetHelpKeyword(this.MACTargetAddressTb, "ClientAddress");
            this.helpProvider1.SetHelpNavigator(this.MACTargetAddressTb, System.Windows.Forms.HelpNavigator.Topic);
            this.MACTargetAddressTb.Hexadecimal = true;
            this.MACTargetAddressTb.Location = new System.Drawing.Point(153, 45);
            this.MACTargetAddressTb.Maximum = new decimal(new int[] {
            4095,
            0,
            0,
            0});
            this.MACTargetAddressTb.Name = "MACTargetAddressTb";
            this.helpProvider1.SetShowHelp(this.MACTargetAddressTb, true);
            this.MACTargetAddressTb.Size = new System.Drawing.Size(105, 20);
            this.MACTargetAddressTb.TabIndex = 42;
            this.MACTargetAddressTb.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // MACSourceAddressTb
            // 
            this.helpProvider1.SetHelpKeyword(this.MACSourceAddressTb, "ClientAddress");
            this.helpProvider1.SetHelpNavigator(this.MACSourceAddressTb, System.Windows.Forms.HelpNavigator.Topic);
            this.MACSourceAddressTb.Hexadecimal = true;
            this.MACSourceAddressTb.Location = new System.Drawing.Point(153, 19);
            this.MACSourceAddressTb.Maximum = new decimal(new int[] {
            4095,
            0,
            0,
            0});
            this.MACSourceAddressTb.Name = "MACSourceAddressTb";
            this.helpProvider1.SetShowHelp(this.MACSourceAddressTb, true);
            this.MACSourceAddressTb.Size = new System.Drawing.Size(105, 20);
            this.MACSourceAddressTb.TabIndex = 41;
            this.MACSourceAddressTb.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // MACTargetAddressLbl
            // 
            this.MACTargetAddressLbl.AutoSize = true;
            this.MACTargetAddressLbl.Location = new System.Drawing.Point(10, 48);
            this.MACTargetAddressLbl.Name = "MACTargetAddressLbl";
            this.MACTargetAddressLbl.Size = new System.Drawing.Size(108, 13);
            this.MACTargetAddressLbl.TabIndex = 40;
            this.MACTargetAddressLbl.Text = "MAC Target Address:";
            // 
            // MACSourceAddressLbl
            // 
            this.MACSourceAddressLbl.AutoSize = true;
            this.MACSourceAddressLbl.Location = new System.Drawing.Point(10, 23);
            this.MACSourceAddressLbl.Name = "MACSourceAddressLbl";
            this.MACSourceAddressLbl.Size = new System.Drawing.Size(111, 13);
            this.MACSourceAddressLbl.TabIndex = 38;
            this.MACSourceAddressLbl.Text = "MAC Source Address:";
            // 
            // PduFrame
            // 
            this.PduFrame.Controls.Add(this.groupBox1);
            this.PduFrame.Location = new System.Drawing.Point(4, 22);
            this.PduFrame.Name = "PduFrame";
            this.PduFrame.Padding = new System.Windows.Forms.Padding(3);
            this.PduFrame.Size = new System.Drawing.Size(511, 388);
            this.PduFrame.TabIndex = 9;
            this.PduFrame.Text = "Framing";
            this.PduFrame.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PduWaitTimeTb);
            this.groupBox1.Controls.Add(this.PduWaitTimelbl);
            this.helpProvider1.SetHelpKeyword(this.groupBox1, "advanced");
            this.helpProvider1.SetHelpNavigator(this.groupBox1, System.Windows.Forms.HelpNavigator.Topic);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.helpProvider1.SetShowHelp(this.groupBox1, true);
            this.groupBox1.Size = new System.Drawing.Size(375, 149);
            this.groupBox1.TabIndex = 61;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PDU settings";
            // 
            // PduWaitTimeTb
            // 
            this.helpProvider1.SetHelpKeyword(this.PduWaitTimeTb, "Password");
            this.helpProvider1.SetHelpNavigator(this.PduWaitTimeTb, System.Windows.Forms.HelpNavigator.Topic);
            this.helpProvider1.SetHelpString(this.PduWaitTimeTb, "Authentication is not used.");
            this.PduWaitTimeTb.Location = new System.Drawing.Point(121, 23);
            this.PduWaitTimeTb.Name = "PduWaitTimeTb";
            this.helpProvider1.SetShowHelp(this.PduWaitTimeTb, true);
            this.PduWaitTimeTb.Size = new System.Drawing.Size(139, 20);
            this.PduWaitTimeTb.TabIndex = 42;
            // 
            // PduWaitTimelbl
            // 
            this.PduWaitTimelbl.AutoSize = true;
            this.PduWaitTimelbl.Location = new System.Drawing.Point(10, 23);
            this.PduWaitTimelbl.Name = "PduWaitTimelbl";
            this.PduWaitTimelbl.Size = new System.Drawing.Size(51, 13);
            this.PduWaitTimelbl.TabIndex = 38;
            this.PduWaitTimelbl.Text = "Wait time";
            // 
            // DelaysTab
            // 
            this.DelaysTab.Controls.Add(this.groupBox3);
            this.DelaysTab.Location = new System.Drawing.Point(4, 22);
            this.DelaysTab.Name = "DelaysTab";
            this.DelaysTab.Size = new System.Drawing.Size(511, 388);
            this.DelaysTab.TabIndex = 11;
            this.DelaysTab.Text = "Delays";
            this.DelaysTab.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ObjectDelayTb);
            this.groupBox3.Controls.Add(this.FrameDelayTb);
            this.groupBox3.Controls.Add(this.ObjectDelayLbl);
            this.groupBox3.Controls.Add(this.FrameDelayLbl);
            this.helpProvider1.SetHelpKeyword(this.groupBox3, "advanced");
            this.helpProvider1.SetHelpNavigator(this.groupBox3, System.Windows.Forms.HelpNavigator.Topic);
            this.groupBox3.Location = new System.Drawing.Point(8, 3);
            this.groupBox3.Name = "groupBox3";
            this.helpProvider1.SetShowHelp(this.groupBox3, true);
            this.groupBox3.Size = new System.Drawing.Size(375, 149);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Delays";
            // 
            // ObjectDelayLbl
            // 
            this.ObjectDelayLbl.AutoSize = true;
            this.ObjectDelayLbl.Location = new System.Drawing.Point(10, 49);
            this.ObjectDelayLbl.Name = "ObjectDelayLbl";
            this.ObjectDelayLbl.Size = new System.Drawing.Size(91, 13);
            this.ObjectDelayLbl.TabIndex = 40;
            this.ObjectDelayLbl.Text = "Object delay (ms):";
            // 
            // FrameDelayLbl
            // 
            this.FrameDelayLbl.AutoSize = true;
            this.FrameDelayLbl.Location = new System.Drawing.Point(10, 23);
            this.FrameDelayLbl.Name = "FrameDelayLbl";
            this.FrameDelayLbl.Size = new System.Drawing.Size(89, 13);
            this.FrameDelayLbl.TabIndex = 38;
            this.FrameDelayLbl.Text = "Frame delay (ms):";
            // 
            // GatewayTab
            // 
            this.GatewayTab.Controls.Add(this.UseGatewayCb);
            this.GatewayTab.Controls.Add(this.PhysicalDeviceAddressAsciiCb);
            this.GatewayTab.Controls.Add(this.PhysicalDeviceAddressTb);
            this.GatewayTab.Controls.Add(this.PhysicalDeviceAddressLbl);
            this.GatewayTab.Controls.Add(this.NetworkIDTb);
            this.GatewayTab.Controls.Add(this.NetworkIDLbl);
            this.GatewayTab.Location = new System.Drawing.Point(4, 22);
            this.GatewayTab.Name = "GatewayTab";
            this.GatewayTab.Size = new System.Drawing.Size(511, 388);
            this.GatewayTab.TabIndex = 5;
            this.GatewayTab.Text = "Gateway";
            this.GatewayTab.UseVisualStyleBackColor = true;
            // 
            // UseGatewayCb
            // 
            this.UseGatewayCb.AutoSize = true;
            this.UseGatewayCb.Checked = true;
            this.UseGatewayCb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UseGatewayCb.Location = new System.Drawing.Point(14, 12);
            this.UseGatewayCb.Name = "UseGatewayCb";
            this.UseGatewayCb.Size = new System.Drawing.Size(90, 17);
            this.UseGatewayCb.TabIndex = 47;
            this.UseGatewayCb.Text = "Use Gateway";
            this.UseGatewayCb.UseVisualStyleBackColor = true;
            this.UseGatewayCb.CheckedChanged += new System.EventHandler(this.UseGatewayCb_CheckedChanged);
            // 
            // PhysicalDeviceAddressAsciiCb
            // 
            this.PhysicalDeviceAddressAsciiCb.AutoSize = true;
            this.PhysicalDeviceAddressAsciiCb.Location = new System.Drawing.Point(349, 63);
            this.PhysicalDeviceAddressAsciiCb.Name = "PhysicalDeviceAddressAsciiCb";
            this.PhysicalDeviceAddressAsciiCb.Size = new System.Drawing.Size(53, 17);
            this.PhysicalDeviceAddressAsciiCb.TabIndex = 45;
            this.PhysicalDeviceAddressAsciiCb.Text = "ASCII";
            this.PhysicalDeviceAddressAsciiCb.UseVisualStyleBackColor = true;
            this.PhysicalDeviceAddressAsciiCb.CheckedChanged += new System.EventHandler(this.PhysicalDeviceAddressAsciiCb_CheckedChanged);
            // 
            // PhysicalDeviceAddressTb
            // 
            this.PhysicalDeviceAddressTb.Location = new System.Drawing.Point(141, 61);
            this.PhysicalDeviceAddressTb.Name = "PhysicalDeviceAddressTb";
            this.PhysicalDeviceAddressTb.Size = new System.Drawing.Size(200, 20);
            this.PhysicalDeviceAddressTb.TabIndex = 44;
            // 
            // PhysicalDeviceAddressLbl
            // 
            this.PhysicalDeviceAddressLbl.AutoSize = true;
            this.PhysicalDeviceAddressLbl.Location = new System.Drawing.Point(10, 65);
            this.PhysicalDeviceAddressLbl.Name = "PhysicalDeviceAddressLbl";
            this.PhysicalDeviceAddressLbl.Size = new System.Drawing.Size(124, 13);
            this.PhysicalDeviceAddressLbl.TabIndex = 46;
            this.PhysicalDeviceAddressLbl.Text = "Physical device address:";
            // 
            // NetworkIDTb
            // 
            this.helpProvider1.SetHelpKeyword(this.NetworkIDTb, "MaxInfoTX");
            this.helpProvider1.SetHelpNavigator(this.NetworkIDTb, System.Windows.Forms.HelpNavigator.Topic);
            this.NetworkIDTb.Location = new System.Drawing.Point(141, 35);
            this.NetworkIDTb.Name = "NetworkIDTb";
            this.helpProvider1.SetShowHelp(this.NetworkIDTb, true);
            this.NetworkIDTb.Size = new System.Drawing.Size(93, 20);
            this.NetworkIDTb.TabIndex = 39;
            // 
            // NetworkIDLbl
            // 
            this.NetworkIDLbl.AutoSize = true;
            this.NetworkIDLbl.Location = new System.Drawing.Point(11, 39);
            this.NetworkIDLbl.Name = "NetworkIDLbl";
            this.NetworkIDLbl.Size = new System.Drawing.Size(64, 13);
            this.NetworkIDLbl.TabIndex = 40;
            this.NetworkIDLbl.Text = "Network ID:";
            // 
            // XmlTab
            // 
            this.XmlTab.Controls.Add(this.PasteFromClipboardBtn);
            this.XmlTab.Controls.Add(this.CopyBtn);
            this.XmlTab.Controls.Add(this.ApplyBtn);
            this.XmlTab.Controls.Add(this.textBox1);
            this.XmlTab.Location = new System.Drawing.Point(4, 22);
            this.XmlTab.Name = "XmlTab";
            this.XmlTab.Size = new System.Drawing.Size(511, 388);
            this.XmlTab.TabIndex = 4;
            this.XmlTab.Text = "XML";
            this.XmlTab.UseVisualStyleBackColor = true;
            // 
            // PasteFromClipboardBtn
            // 
            this.PasteFromClipboardBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PasteFromClipboardBtn.Location = new System.Drawing.Point(137, 361);
            this.PasteFromClipboardBtn.Name = "PasteFromClipboardBtn";
            this.PasteFromClipboardBtn.Size = new System.Drawing.Size(117, 23);
            this.PasteFromClipboardBtn.TabIndex = 10;
            this.PasteFromClipboardBtn.Text = "Paste from Clipboard";
            this.PasteFromClipboardBtn.UseVisualStyleBackColor = true;
            this.PasteFromClipboardBtn.Click += new System.EventHandler(this.PasteFromClipboardBtn_Click);
            // 
            // CopyBtn
            // 
            this.CopyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CopyBtn.Location = new System.Drawing.Point(14, 361);
            this.CopyBtn.Name = "CopyBtn";
            this.CopyBtn.Size = new System.Drawing.Size(117, 23);
            this.CopyBtn.TabIndex = 9;
            this.CopyBtn.Text = "Copy to Clipboard";
            this.CopyBtn.UseVisualStyleBackColor = true;
            this.CopyBtn.Click += new System.EventHandler(this.CopyBtn_Click);
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyBtn.Location = new System.Drawing.Point(388, 361);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(75, 23);
            this.ApplyBtn.TabIndex = 8;
            this.ApplyBtn.Text = "Apply";
            this.ApplyBtn.UseVisualStyleBackColor = true;
            this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(8, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(455, 352);
            this.textBox1.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "publicKey.png");
            this.imageList1.Images.SetKeyName(1, "privateKey.png");
            // 
            // helpProvider1
            // 
            this.helpProvider1.HelpNamespace = "https://www.gurux.fi/GXDLMSDirector.DeviceProperties";
            // 
            // conformanceHelpProvider
            // 
            this.conformanceHelpProvider.HelpNamespace = "https://www.gurux.fi/Gurux.DLMS.Conformance";
            // 
            // ShowAsHex
            // 
            this.ShowAsHex.AutoSize = true;
            this.ShowAsHex.Checked = true;
            this.ShowAsHex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowAsHex.Location = new System.Drawing.Point(133, 426);
            this.ShowAsHex.Name = "ShowAsHex";
            this.ShowAsHex.Size = new System.Drawing.Size(45, 17);
            this.ShowAsHex.TabIndex = 2;
            this.ShowAsHex.Text = "Hex";
            this.ShowAsHex.UseVisualStyleBackColor = true;
            this.ShowAsHex.CheckedChanged += new System.EventHandler(this.ShowAsHex_CheckedChanged);
            // 
            // FrameDelayTb
            // 
            this.helpProvider1.SetHelpKeyword(this.FrameDelayTb, "MaxInfoTX");
            this.helpProvider1.SetHelpNavigator(this.FrameDelayTb, System.Windows.Forms.HelpNavigator.Topic);
            this.FrameDelayTb.Location = new System.Drawing.Point(108, 21);
            this.FrameDelayTb.Name = "FrameDelayTb";
            this.helpProvider1.SetShowHelp(this.FrameDelayTb, true);
            this.FrameDelayTb.Size = new System.Drawing.Size(93, 20);
            this.FrameDelayTb.TabIndex = 43;
            // 
            // ObjectDelayTb
            // 
            this.helpProvider1.SetHelpKeyword(this.ObjectDelayTb, "MaxInfoTX");
            this.helpProvider1.SetHelpNavigator(this.ObjectDelayTb, System.Windows.Forms.HelpNavigator.Topic);
            this.ObjectDelayTb.Location = new System.Drawing.Point(108, 48);
            this.ObjectDelayTb.Name = "ObjectDelayTb";
            this.helpProvider1.SetShowHelp(this.ObjectDelayTb, true);
            this.ObjectDelayTb.Size = new System.Drawing.Size(93, 20);
            this.ObjectDelayTb.TabIndex = 44;
            // 
            // DevicePropertiesForm
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(518, 456);
            this.Controls.Add(this.ShowAsHex);
            this.Controls.Add(this.InitialSettingsBtn);
            this.Controls.Add(this.DeviceTab);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.CancelBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DevicePropertiesForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Device Properties";
            this.SupportedServicesTab.ResumeLayout(false);
            this.SNSettings.ResumeLayout(false);
            this.SNSettings.PerformLayout();
            this.LNSettings.ResumeLayout(false);
            this.LNSettings.PerformLayout();
            this.CipheringTab.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.DeviceSettingsTab.ResumeLayout(false);
            this.DeviceSettingsTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResendTb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ClientAddTB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogicalServerAddressTB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhysicalServerAddressTB)).EndInit();
            this.NetworkSettingsGB.ResumeLayout(false);
            this.NetworkSettingsGB.PerformLayout();
            this.TerminalSettingsGB.ResumeLayout(false);
            this.TerminalSettingsGB.PerformLayout();
            this.SerialSettingsGB.ResumeLayout(false);
            this.SerialSettingsGB.PerformLayout();
            this.DeviceTab.ResumeLayout(false);
            this.AdvancedTab.ResumeLayout(false);
            this.AdvancedTab.PerformLayout();
            this.HdlcFrameTab.ResumeLayout(false);
            this.HdlcGroup.ResumeLayout(false);
            this.HdlcGroup.PerformLayout();
            this.PlcFrame.ResumeLayout(false);
            this.PlcGroup.ResumeLayout(false);
            this.PlcGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MACTargetAddressTb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MACSourceAddressTb)).EndInit();
            this.PduFrame.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.DelaysTab.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.GatewayTab.ResumeLayout(false);
            this.GatewayTab.PerformLayout();
            this.XmlTab.ResumeLayout(false);
            this.XmlTab.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button InitialSettingsBtn;
        private System.Windows.Forms.TabPage SupportedServicesTab;
        private System.Windows.Forms.TabPage CipheringTab;
        private System.Windows.Forms.TabPage DeviceSettingsTab;
        private System.Windows.Forms.CheckBox VerboseModeCB;
        private System.Windows.Forms.CheckBox UseLNCB;
        private System.Windows.Forms.ComboBox ServerAddressTypeCB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label ClientAddLbl;
        private System.Windows.Forms.NumericUpDown ClientAddTB;
        private System.Windows.Forms.NumericUpDown LogicalServerAddressTB;
        private System.Windows.Forms.Label LogicalServerAddressLbl;
        private System.Windows.Forms.NumericUpDown PhysicalServerAddressTB;
        private System.Windows.Forms.Label PhysicalServerAddressLbl;
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
        private System.Windows.Forms.GroupBox SNSettings;
        private System.Windows.Forms.GroupBox LNSettings;
        private System.Windows.Forms.CheckBox ActionCB;
        private System.Windows.Forms.CheckBox GeneralProtectionCB;
        private System.Windows.Forms.CheckBox GeneralBlockTransferCB;
        private System.Windows.Forms.CheckBox Attribute0SetReferencingCB;
        private System.Windows.Forms.CheckBox PriorityManagementCB;
        private System.Windows.Forms.CheckBox Attribute0GetReferencingCB;
        private System.Windows.Forms.CheckBox GetBlockTransferCB;
        private System.Windows.Forms.CheckBox SetBlockTransferCB;
        private System.Windows.Forms.CheckBox ActionBlockTransferCB;
        private System.Windows.Forms.CheckBox MultipleReferencesCB;
        private System.Windows.Forms.CheckBox DataNotificationCB;
        private System.Windows.Forms.CheckBox AccessCB;
        private System.Windows.Forms.CheckBox GetCB;
        private System.Windows.Forms.CheckBox SetCB;
        private System.Windows.Forms.CheckBox SelectiveAccessCB;
        private System.Windows.Forms.CheckBox EventNotificationCB;
        private System.Windows.Forms.CheckBox SNGeneralProtectionCB;
        private System.Windows.Forms.CheckBox SNGeneralBlockTransferCB;
        private System.Windows.Forms.CheckBox ReadCB;
        private System.Windows.Forms.CheckBox WriteCB;
        private System.Windows.Forms.CheckBox UnconfirmedWriteCB;
        private System.Windows.Forms.CheckBox ReadBlockTransferCB;
        private System.Windows.Forms.CheckBox WriteBlockTransferCB;
        private System.Windows.Forms.CheckBox SNMultipleReferencesCB;
        private System.Windows.Forms.CheckBox InformationReportCB;
        private System.Windows.Forms.CheckBox SNDataNotificationCB;
        private System.Windows.Forms.CheckBox ParameterizedAccessCB;
        private System.Windows.Forms.TextBox InvocationCounterTB;
        private System.Windows.Forms.Label InvocationCounterLbl;
        private System.Windows.Forms.CheckBox PasswordAsciiCb;
        private System.Windows.Forms.GroupBox CustomSettings;
        private System.Windows.Forms.TabPage AdvancedTab;
        private System.Windows.Forms.CheckBox UseUtcTimeZone;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.HelpProvider conformanceHelpProvider;
        private System.Windows.Forms.TextBox InactivityTimeoutTb;
        private System.Windows.Forms.Label InactivityTimeoutLbl;
        private System.Windows.Forms.TextBox MaxPduTb;
        private System.Windows.Forms.Label MaxPduLbl;
        private System.Windows.Forms.Label ServiceClassLbl;
        private System.Windows.Forms.Label PriorityLbl;
        private System.Windows.Forms.ComboBox ServiceClassCb;
        private System.Windows.Forms.ComboBox PriorityCb;
        private System.Windows.Forms.TextBox UserIdTb;
        private System.Windows.Forms.Label UserIDLbl;
        private System.Windows.Forms.ComboBox StandardCb;
        private System.Windows.Forms.Label StandardLbl;
        private System.Windows.Forms.TextBox FrameCounterTb;
        private System.Windows.Forms.Label FrameCounterLbl;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox InvocationCounterCb;
        private System.Windows.Forms.TabPage XmlTab;
        private System.Windows.Forms.Button ApplyBtn;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button CopyBtn;
        private System.Windows.Forms.Button PasteFromClipboardBtn;
        private System.Windows.Forms.TabPage GatewayTab;
        private System.Windows.Forms.CheckBox PhysicalDeviceAddressAsciiCb;
        private System.Windows.Forms.TextBox PhysicalDeviceAddressTb;
        private System.Windows.Forms.TextBox NetworkIDTb;
        private System.Windows.Forms.Label NetworkIDLbl;
        private System.Windows.Forms.Label PhysicalDeviceAddressLbl;
        private System.Windows.Forms.CheckBox UseGatewayCb;
        private System.Windows.Forms.NumericUpDown ResendTb;
        private System.Windows.Forms.Label ResendLbl;
        private System.Windows.Forms.DateTimePicker WaitTimeTB;
        private System.Windows.Forms.ComboBox InterfaceCb;
        private System.Windows.Forms.Label InterfaceLbl;
        private System.Windows.Forms.CheckBox UseProtectedReleaseCb;
        private System.Windows.Forms.CheckBox IgnoreTimeStatusCb;
        private System.Windows.Forms.CheckBox IgnoreTimeZoneCb;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabPage HdlcFrameTab;
        private System.Windows.Forms.GroupBox HdlcGroup;
        private System.Windows.Forms.CheckBox FrameSizeCb;
        private System.Windows.Forms.TextBox WindowSizeRXTb;
        private System.Windows.Forms.Label WindowSizeRXLbl;
        private System.Windows.Forms.TextBox WindowSizeTXTb;
        private System.Windows.Forms.Label WindowSizeTXLbl;
        private System.Windows.Forms.TextBox MaxInfoRXTb;
        private System.Windows.Forms.Label MaxInfoRXLbl;
        private System.Windows.Forms.TextBox MaxInfoTXTb;
        private System.Windows.Forms.Label MaxInfoTXLbl;
        private System.Windows.Forms.ComboBox ServerAddressSizeCb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage PlcFrame;
        private System.Windows.Forms.GroupBox PlcGroup;
        private System.Windows.Forms.Label MACTargetAddressLbl;
        private System.Windows.Forms.Label MACSourceAddressLbl;
        private System.Windows.Forms.CheckBox ShowAsHex;
        private System.Windows.Forms.NumericUpDown MACTargetAddressTb;
        private System.Windows.Forms.NumericUpDown MACSourceAddressTb;
        private System.Windows.Forms.Panel SettingsPanel;
        private System.Windows.Forms.CheckBox SNDeltaValueEncodingCb;
        private System.Windows.Forms.CheckBox DeltaValueEncodingCb;
        private System.Windows.Forms.TabPage PduFrame;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label PduWaitTimelbl;
        private System.Windows.Forms.TextBox PduWaitTimeTb;
        private System.Windows.Forms.CheckBox SecurityChangeCheckCb;
        private System.Windows.Forms.ComboBox SigningKeyCb;
        private System.Windows.Forms.ComboBox ChallengeSizeCb;
        private System.Windows.Forms.Label ChallengeSizeLbl;
        private System.Windows.Forms.CheckBox SignInitiateRequestResponseCb;
        private System.Windows.Forms.CheckBox IncludePublicKeyCb;
        private System.Windows.Forms.TextBox GBTWindowSizeTb;
        private System.Windows.Forms.Label GBTWindowSizeLbl;
        private System.Windows.Forms.CheckBox OverwriteAttributeAccessRightsCb;
        private System.Windows.Forms.CheckBox IncreaseInvocationCounterForGMacAuthenticationCB;
        private System.Windows.Forms.CheckBox BroadcastCb;
        private System.Windows.Forms.TabPage DelaysTab;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label FrameDelayLbl;
        private System.Windows.Forms.Label ObjectDelayLbl;
        private System.Windows.Forms.TextBox ObjectDelayTb;
        private System.Windows.Forms.TextBox FrameDelayTb;
    }
}