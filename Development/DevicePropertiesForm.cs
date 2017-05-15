//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Filename:        $HeadURL: https://146.185.146.169/Projects/GuruxClub/GXDLMSDirector/Development/DevicePropertiesForm.cs $
//
// Version:         $Revision: 9397 $,
//                  $Date: 2017-05-15 10:43:42 +0300 (ma, 15 touko 2017) $
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


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Gurux.Serial;
using Gurux.DLMS;
using GXDLMS.ManufacturerSettings;
using Gurux.Net;
using System.Reflection;
using System.IO.Ports;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.Terminal;
using Gurux.DLMS.Enums;
using Gurux.Common;

namespace GXDLMSDirector
{
    partial class DevicePropertiesForm : Form
    {
        GXManufacturerCollection Manufacturers;
        Gurux.Common.IGXMedia SelectedMedia = null;
        public GXDLMSDevice Device = null;
        public DevicePropertiesForm(GXManufacturerCollection manufacturers, GXDLMSDevice dev2)
        {
            try
            {
                InitializeComponent();
                LNSettings.Dock = SNSettings.Dock = DockStyle.Fill;
                SecurityCB.Items.AddRange(new object[] { Security.None, Security.Authentication,
                                      Security.Encryption, Security.AuthenticationEncryption
                                                   });
                NetProtocolCB.Items.AddRange(new object[] { NetworkType.Tcp, NetworkType.Udp });
                this.ServerAddressTypeCB.SelectedIndexChanged += new System.EventHandler(this.ServerAddressTypeCB_SelectedIndexChanged);
                NetworkSettingsGB.Width = this.Width - NetworkSettingsGB.Left;
                SerialSettingsGB.Bounds = TerminalSettingsGB.Bounds = NetworkSettingsGB.Bounds;
                ServerAddressTypeCB.DrawMode = AuthenticationCB.DrawMode = DrawMode.OwnerDrawFixed;
                Manufacturers = manufacturers;
                //OK button is not enabled if there are no manufacturers.
                if (Manufacturers.Count == 0)
                {
                    OKBtn.Enabled = false;
                }
                Device = dev2;
                StartProtocolCB.Items.Add(StartProtocolType.IEC);
                StartProtocolCB.Items.Add(StartProtocolType.DLMS);
                int pos = 0;
                foreach (GXManufacturer it in Manufacturers)
                {
                    int index = this.ManufacturerCB.Items.Add(it);
                    if (it.Name == GXDLMSDirector.Properties.Settings.Default.SelectedManufacturer)
                    {
                        pos = index;
                    }
                }
                if (Device == null)
                {
                    Device = new GXDLMSDevice(null);
                    //Select first manufacturer.
                    if (Manufacturers.Count != 0)
                    {
                        ManufacturerCB.SelectedIndex = pos;
                    }
                    Device.Comm.client.ProposedConformance = GXDLMSClient.GetInitialConformance(UseLNCB.Checked);
                }
                else
                {
                    foreach (GXManufacturer it in this.ManufacturerCB.Items)
                    {
                        if (string.Compare(it.Identification, Device.Manufacturer, true) == 0)
                        {
                            this.ManufacturerCB.SelectedItem = it;
                            break;
                        }
                    }
                    if (IsAscii(GXCommon.HexToBytes(Device.SystemTitle)))
                    {
                        SystemTitleAsciiCb.CheckedChanged -= SystemTitleAsciiCb_CheckedChanged;
                        SystemTitleAsciiCb.Checked = true;
                        SystemTitleAsciiCb.CheckedChanged += SystemTitleAsciiCb_CheckedChanged;
                        SystemTitleTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(Device.SystemTitle));
                    }
                    else
                    {
                        SystemTitleAsciiCb.CheckedChanged -= SystemTitleAsciiCb_CheckedChanged;
                        SystemTitleAsciiCb.Checked = false;
                        SystemTitleAsciiCb.CheckedChanged += SystemTitleAsciiCb_CheckedChanged;
                        SystemTitleTB.Text = Device.SystemTitle;
                    }
                    if (IsAscii(GXCommon.HexToBytes(Device.BlockCipherKey)))
                    {
                        BlockCipherKeyAsciiCb.CheckedChanged -= BlockCipherKeyAsciiCb_CheckedChanged;
                        BlockCipherKeyAsciiCb.Checked = true;
                        BlockCipherKeyAsciiCb.CheckedChanged += BlockCipherKeyAsciiCb_CheckedChanged;
                        BlockCipherKeyTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(Device.BlockCipherKey));
                    }
                    else
                    {
                        BlockCipherKeyAsciiCb.CheckedChanged -= BlockCipherKeyAsciiCb_CheckedChanged;
                        BlockCipherKeyAsciiCb.Checked = false;
                        BlockCipherKeyAsciiCb.CheckedChanged += BlockCipherKeyAsciiCb_CheckedChanged;
                        BlockCipherKeyTB.Text = Device.BlockCipherKey;
                    }
                    if (IsAscii(GXCommon.HexToBytes(Device.AuthenticationKey)))
                    {
                        AuthenticationKeyAsciiCb.CheckedChanged -= AuthenticationKeyAsciiCb_CheckedChanged;
                        AuthenticationKeyAsciiCb.Checked = true;
                        AuthenticationKeyAsciiCb.CheckedChanged += AuthenticationKeyAsciiCb_CheckedChanged;
                        AuthenticationKeyTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(Device.AuthenticationKey));
                    }
                    else
                    {
                        AuthenticationKeyAsciiCb.CheckedChanged -= AuthenticationKeyAsciiCb_CheckedChanged;
                        AuthenticationKeyAsciiCb.Checked = false;
                        AuthenticationKeyAsciiCb.CheckedChanged += AuthenticationKeyAsciiCb_CheckedChanged;
                        AuthenticationKeyTB.Text = Device.AuthenticationKey;
                    }
                    this.VerboseModeCB.Checked = Device.Verbose;
                    this.NameTB.Text = Device.Name;
                    SelectedMedia = Device.Media;
                    UseRemoteSerialCB.Checked = Device.UseRemoteSerial;
                    StartProtocolCB.SelectedItem = Device.StartProtocol;
                    PhysicalServerAddressTB.Value = Convert.ToDecimal(Device.PhysicalAddress);
                    LogicalServerAddressTB.Value = Convert.ToDecimal(Device.LogicalAddress);
                    this.ClientAddTB.Value = Convert.ToDecimal(Convert.ToUInt32(Device.ClientAddress));
                    WaitTimeTB.Value = Device.WaitTime;
                    SecurityCB.SelectedItem = Device.Security;
                    InvocationCounterTB.Text = Device.InvocationCounter.ToString();
                    ChallengeTB.Text = GXCommon.ToHex(GXCommon.HexToBytes(Device.Challenge), true);
                    UseUtcTimeZone.Checked = Device.UtcTimeZone;
                }
                ManufacturerCB.DrawMode = MediasCB.DrawMode = DrawMode.OwnerDrawFixed;
                Gurux.Net.GXNet net = new Gurux.Net.GXNet();
                //Initialize network settings.
                if (SelectedMedia is GXNet)
                {
                    this.MediasCB.Items.Add(SelectedMedia);
                    net.Protocol = Gurux.Net.NetworkType.Tcp;
                    this.HostNameTB.Text = ((GXNet)SelectedMedia).HostName;
                    this.PortTB.Text = ((GXNet)SelectedMedia).Port.ToString();
                    NetProtocolCB.SelectedItem = ((GXNet)SelectedMedia).Protocol;
                }
                else
                {
                    NetProtocolCB.SelectedItem = net.Protocol = Gurux.Net.NetworkType.Tcp;
                    this.MediasCB.Items.Add(net);
                }

                //Set maximum baud rate.
                GXSerial serial = new GXSerial();
                foreach (int it in serial.GetAvailableBaudRates(""))
                {
                    if (it != 0)
                    {
                        MaximumBaudRateCB.Items.Add(it);
                    }
                }
                if (Device.MaximumBaudRate == 0)
                {
                    UseMaximumBaudRateCB.Checked = false;
                    UseMaximumBaudRateCB_CheckedChanged(null, null);
                }
                else
                {
                    UseMaximumBaudRateCB.Checked = true;
                    this.MaximumBaudRateCB.SelectedItem = Device.MaximumBaudRate;
                }

                if (SelectedMedia is GXSerial)
                {
                    this.MediasCB.Items.Add(SelectedMedia);
                    string[] ports = GXSerial.GetPortNames();
                    this.SerialPortCB.Items.AddRange(ports);
                    if (ports.Length != 0)
                    {
                        this.SerialPortCB.SelectedItem = ((GXSerial)SelectedMedia).PortName;
                    }
                }
                else
                {
                    //Initialize serial settings.
                    string[] ports = GXSerial.GetPortNames();
                    this.SerialPortCB.Items.AddRange(ports);
                    if (ports.Length != 0)
                    {
                        serial.PortName = ports[0];
                    }
                    if (((GXManufacturer)ManufacturerCB.SelectedItem).StartProtocol == StartProtocolType.DLMS)
                    {
                        serial.BaudRate = 9600;
                        serial.DataBits = 8;
                        serial.Parity = Parity.None;
                        serial.StopBits = StopBits.One;
                    }
                    else
                    {
                        serial.BaudRate = 300;
                        serial.DataBits = 7;
                        serial.Parity = Parity.Even;
                        serial.StopBits = StopBits.One;
                    }
                    this.MediasCB.Items.Add(serial);
                }
                if (SelectedMedia is Gurux.Terminal.GXTerminal)
                {
                    this.MediasCB.Items.Add(SelectedMedia);
                    string[] ports = GXTerminal.GetPortNames();
                    this.TerminalPortCB.Items.AddRange(ports);
                    if (ports.Length != 0)
                    {
                        this.TerminalPortCB.SelectedItem = ((Gurux.Terminal.GXTerminal)SelectedMedia).PortName;
                    }
                    this.TerminalPhoneNumberTB.Text = ((Gurux.Terminal.GXTerminal)SelectedMedia).PhoneNumber;
                }
                else
                {
                    //Initialize terminal settings.
                    Gurux.Terminal.GXTerminal termial = new Gurux.Terminal.GXTerminal();
                    string[] ports = GXTerminal.GetPortNames();
                    this.TerminalPortCB.Items.AddRange(ports);
                    if (ports.Length != 0)
                    {
                        termial.PortName = ports[0];
                    }
                    termial.BaudRate = 9600;
                    termial.DataBits = 8;
                    termial.Parity = Parity.None;
                    termial.StopBits = StopBits.One;
                    this.TerminalPhoneNumberTB.Text = termial.PhoneNumber;
                    //termial.InitializeCommands = "AT+CBST=71,0,1\r\n";
                    this.MediasCB.Items.Add(termial);
                }
                //Select first media if medis is not selected.
                if (SelectedMedia == null)
                {
                    SelectedMedia = (Gurux.Common.IGXMedia)this.MediasCB.Items[0];
                }
                this.MediasCB.SelectedItem = SelectedMedia;
                if (!string.IsNullOrEmpty(Device.Password))
                {
                    PasswordTB.Text = ASCIIEncoding.ASCII.GetString(CryptHelper.Decrypt(Device.Password, Password.Key));
                }
                else if (Device.HexPassword != null)
                {
                    byte[] pw = CryptHelper.Decrypt(Device.HexPassword, Password.Key);
                    PasswordAsciiCb.CheckedChanged -= PasswordAsciiCb_CheckedChanged;
                    PasswordAsciiCb.Checked = false;
                    PasswordAsciiCb.CheckedChanged += PasswordAsciiCb_CheckedChanged;
                    PasswordTB.Text = GXDLMSTranslator.ToHex(pw);
                }
                this.UseLNCB.Checked = Device.UseLogicalNameReferencing;
                ShowConformance(Device.Comm.client.ProposedConformance);
                this.AuthenticationCB.SelectedIndexChanged += new System.EventHandler(this.AuthenticationCB_SelectedIndexChanged);
                bool bConnected = Device.Media != null && Device.Media.IsOpen;
                SerialPortCB.Enabled = AdvancedBtn.Enabled = ManufacturerCB.Enabled = MediasCB.Enabled =
                                           AuthenticationCB.Enabled = UseRemoteSerialCB.Enabled = OKBtn.Enabled = !bConnected;
                HostNameTB.ReadOnly = PortTB.ReadOnly = PasswordTB.ReadOnly = WaitTimeTB.ReadOnly = PhysicalServerAddressTB.ReadOnly = NameTB.ReadOnly = bConnected;
                this.UseLNCB.CheckedChanged += new System.EventHandler(this.UseLNCB_CheckedChanged);
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }
        private void ShowConformance(Conformance c)
        {
            if (UseLNCB.Checked)
            {
                GeneralProtectionCB.Checked = (c & Conformance.GeneralProtection) != 0;
                GeneralBlockTransferCB.Checked = (c & Conformance.GeneralBlockTransfer) != 0;
                Attribute0SetReferencingCB.Checked = (c & Conformance.Attribute0SupportedWithSet) != 0;
                PriorityManagementCB.Checked = (c & Conformance.PriorityMgmtSupported) != 0;
                Attribute0GetReferencingCB.Checked = (c & Conformance.Attribute0SupportedWithGet) != 0;
                GetBlockTransferCB.Checked = (c & Conformance.BlockTransferWithGetOrRead) != 0;
                SetBlockTransferCB.Checked = (c & Conformance.BlockTransferWithSetOrWrite) != 0;
                ActionBlockTransferCB.Checked = (c & Conformance.BlockTransferWithAction) != 0;
                MultipleReferencesCB.Checked = (c & Conformance.MultipleReferences) != 0;
                DataNotificationCB.Checked = (c & Conformance.DataNotification) != 0;
                AccessCB.Checked = (c & Conformance.Access) != 0;
                GetCB.Checked = (c & Conformance.Get) != 0;
                SetCB.Checked = (c & Conformance.Set) != 0;
                SelectiveAccessCB.Checked = (c & Conformance.SelectiveAccess) != 0;
                EventNotificationCB.Checked = (c & Conformance.EventNotification) != 0;
                ActionCB.Checked = (c & Conformance.EventNotification) != 0;
            }
            else
            {
                SNGeneralProtectionCB.Checked = (c & Conformance.GeneralProtection) != 0;
                SNGeneralBlockTransferCB.Checked = (c & Conformance.GeneralBlockTransfer) != 0;
                ReadCB.Checked = (c & Conformance.Read) != 0;
                WriteCB.Checked = (c & Conformance.Write) != 0;
                UnconfirmedWriteCB.Checked = (c & Conformance.UnconfirmedWrite) != 0;
                ReadBlockTransferCB.Checked = (c & Conformance.BlockTransferWithGetOrRead) != 0;
                WriteBlockTransferCB.Checked = (c & Conformance.BlockTransferWithSetOrWrite) != 0;
                SNMultipleReferencesCB.Checked = (c & Conformance.MultipleReferences) != 0;
                InformationReportCB.Checked = (c & Conformance.InformationReport) != 0;
                SNDataNotificationCB.Checked = (c & Conformance.DataNotification) != 0;
                ParameterizedAccessCB.Checked = (c & Conformance.ParameterizedAccess) != 0;
            }
            LNSettings.Visible = UseLNCB.Checked;
            SNSettings.Visible = !UseLNCB.Checked;
        }

        /// <summary>
        /// Show help not available message.
        /// </summary>
        /// <param name="hevent">A HelpEventArgs that contains the event data.</param>
        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            // Get the control where the user clicked
            Control ctl = this.GetChildAtPoint(this.PointToClient(hevent.MousePos));
            string str = GXDLMSDirector.Properties.Resources.HelpNotAvailable;
            // Show as a Help pop-up
            if (str != "")
            {
                Help.ShowPopup(ctl, str, hevent.MousePos);
            }
            // Set flag to show that the Help event as been handled
            hevent.Handled = true;
        }


        private void UpdateConformance()
        {
            Conformance c = (Conformance)0;
            if (UseLNCB.Checked)
            {
                if (GeneralProtectionCB.Checked)
                {
                    c |= Conformance.GeneralProtection;
                }
                if (GeneralBlockTransferCB.Checked)
                {
                    c |= Conformance.GeneralBlockTransfer;
                }
                if (Attribute0SetReferencingCB.Checked)
                {
                    c |= Conformance.Attribute0SupportedWithSet;
                }
                if (PriorityManagementCB.Checked)
                {
                    c |= Conformance.PriorityMgmtSupported;
                }
                if (Attribute0GetReferencingCB.Checked)
                {
                    c |= Conformance.Attribute0SupportedWithGet;
                }
                if (GetBlockTransferCB.Checked)
                {
                    c |= Conformance.BlockTransferWithGetOrRead;
                }
                if (SetBlockTransferCB.Checked)
                {
                    c |= Conformance.BlockTransferWithSetOrWrite;
                }
                if (ActionBlockTransferCB.Checked)
                {
                    c |= Conformance.BlockTransferWithAction;
                }
                if (MultipleReferencesCB.Checked)
                {
                    c |= Conformance.MultipleReferences;
                }
                if (DataNotificationCB.Checked)
                {
                    c |= Conformance.DataNotification;
                }
                if (AccessCB.Checked)
                {
                    c |= Conformance.Access;
                }
                if (GetCB.Checked)
                {
                    c |= Conformance.Get;
                }
                if (SetCB.Checked)
                {
                    c |= Conformance.Set;
                }
                if (SelectiveAccessCB.Checked)
                {
                    c |= Conformance.SelectiveAccess;
                }
                if (EventNotificationCB.Checked)
                {
                    c |= Conformance.EventNotification;
                }
                if (ActionCB.Checked)
                {
                    c |= Conformance.EventNotification;
                }
            }
            else
            {
                if (SNGeneralProtectionCB.Checked)
                {
                    c |= Conformance.GeneralProtection;
                }
                if (SNGeneralBlockTransferCB.Checked)
                {
                    c |= Conformance.GeneralBlockTransfer;
                }
                if (ReadCB.Checked)
                {
                    c |= Conformance.Read;
                }
                if (WriteCB.Checked)
                {
                    c |= Conformance.Write;
                }
                if (UnconfirmedWriteCB.Checked)
                {
                    c |= Conformance.UnconfirmedWrite;
                }
                if (ReadBlockTransferCB.Checked)
                {
                    c |= Conformance.BlockTransferWithGetOrRead;
                }
                if (WriteBlockTransferCB.Checked)
                {
                    c |= Conformance.BlockTransferWithSetOrWrite;
                }
                if (SNMultipleReferencesCB.Checked)
                {
                    c |= Conformance.MultipleReferences;
                }
                if (InformationReportCB.Checked)
                {
                    c |= Conformance.InformationReport;
                }
                if (SNDataNotificationCB.Checked)
                {
                    c |= Conformance.DataNotification;
                }
                if (ParameterizedAccessCB.Checked)
                {
                    c |= Conformance.ParameterizedAccess;
                }
            }
            Device.Comm.client.ProposedConformance = c;
        }

        private void MediasCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedMedia = (Gurux.Common.IGXMedia)MediasCB.SelectedItem;
                this.SerialSettingsGB.Visible = SelectedMedia is GXSerial;
                this.NetworkSettingsGB.Visible = SelectedMedia is GXNet;
                this.TerminalSettingsGB.Visible = SelectedMedia is GXTerminal;
                if (SelectedMedia is GXNet && this.PortTB.Text == "")
                {
                    this.PortTB.Text = "4059";
                }
                UpdateStartProtocol();
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Apply new settings from property pages.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string name = NameTB.Text.Trim();
                if (name.Length == 0)
                {
                    throw new Exception("Invalid name.");
                }
                //Check security settings.
                if ((Security)SecurityCB.SelectedItem != Security.None ||
                    Device.Authentication == Authentication.HighGMAC)
                {
                    if (SystemTitleTB.Text.Trim().Length == 0)
                    {
                        throw new ArgumentException("Invalid system title.");
                    }
                    if (AuthenticationKeyTB.Text.Trim().Length == 0)
                    {
                        throw new ArgumentException("Invalid authentication key.");
                    }
                    if (BlockCipherKeyTB.Text.Trim().Length == 0)
                    {
                        throw new ArgumentException("Invalid block cipher key.");
                    }
                }
                GXServerAddress server = (GXServerAddress)ServerAddressTypeCB.SelectedItem;
                if (server.HDLCAddress == HDLCAddressType.SerialNumber && PhysicalServerAddressTB.Value == 0)
                {
                    throw new Exception("Invalid Serial Number.");
                }
                GXManufacturer man = (GXManufacturer)ManufacturerCB.SelectedItem;
                Device.Authentication = ((GXAuthentication)this.AuthenticationCB.SelectedItem).Type;
                if (Device.Authentication != Authentication.None)
                {
                    if (PasswordAsciiCb.Checked)
                    {
                        Device.Password = CryptHelper.Encrypt(PasswordTB.Text, Password.Key);
                        Device.HexPassword = null;
                    }
                    else
                    {
                        Device.Password = "";
                        Device.HexPassword = CryptHelper.Encrypt(GXDLMSTranslator.HexToBytes(this.PasswordTB.Text), Password.Key);
                    }
                }
                else
                {
                    Device.Password = "";
                }
                Device.Name = name;
                Device.Media = SelectedMedia;
                Device.Manufacturer = man.Identification;
                Device.WaitTime = Convert.ToInt32(WaitTimeTB.Value);
                Device.Verbose = VerboseModeCB.Checked;
                Device.MaximumBaudRate = 0;
                Device.UtcTimeZone = UseUtcTimeZone.Checked;

                if (SelectedMedia is GXSerial)
                {
                    Device.UseRemoteSerial = false;

                    if (this.SerialPortCB.Text.Length == 0)
                    {
                        throw new Exception("Invalid serial port.");
                    }
                    ((GXSerial)SelectedMedia).PortName = this.SerialPortCB.Text;
                    if (UseMaximumBaudRateCB.Checked)
                    {
                        Device.MaximumBaudRate = (int)MaximumBaudRateCB.SelectedItem;
                    }
                }
                else if (SelectedMedia is GXNet)
                {
                    if (this.HostNameTB.Text.Length == 0)
                    {
                        throw new Exception("Invalid host name.");
                    }
                    ((GXNet)SelectedMedia).HostName = this.HostNameTB.Text;
                    int port;
                    if (!Int32.TryParse(this.PortTB.Text, out port))
                    {
                        throw new Exception("Invalid port number.");
                    }
                    ((GXNet)SelectedMedia).Port = port;
                    Device.UseRemoteSerial = UseRemoteSerialCB.Checked;
                    ((GXNet)SelectedMedia).Protocol = (NetworkType)NetProtocolCB.SelectedItem;
                }
                else if (SelectedMedia is Gurux.Terminal.GXTerminal)
                {
                    if (this.TerminalPortCB.Text.Length == 0)
                    {
                        throw new Exception("Invalid serial port.");
                    }
                    if (this.TerminalPhoneNumberTB.Text.Length == 0)
                    {
                        throw new Exception("Invalid phone number.");
                    }
                    Gurux.Terminal.GXTerminal terminal = SelectedMedia as Gurux.Terminal.GXTerminal;
                    terminal.ConfigurableSettings = Gurux.Terminal.AvailableMediaSettings.All & ~Gurux.Terminal.AvailableMediaSettings.Server;
                    Device.UseRemoteSerial = false;
                    terminal.PortName = this.TerminalPortCB.Text;
                    terminal.PhoneNumber = this.TerminalPhoneNumberTB.Text;
                }
                GXAuthentication authentication = (GXAuthentication)AuthenticationCB.SelectedItem;
                Device.HDLCAddressing = ((GXServerAddress)ServerAddressTypeCB.SelectedItem).HDLCAddress;
                Device.ClientAddress = Convert.ToInt32(ClientAddTB.Value);
                if (Device.HDLCAddressing == HDLCAddressType.SerialNumber)
                {
                    Device.PhysicalAddress = PhysicalServerAddressTB.Value;
                }
                else
                {
                    Device.PhysicalAddress = Convert.ChangeType(PhysicalServerAddressTB.Value, server.PhysicalAddress.GetType());
                }
                Device.UseLogicalNameReferencing = this.UseLNCB.Checked;
                Device.LogicalAddress = Convert.ToInt32(LogicalServerAddressTB.Value);
                Device.StartProtocol = (StartProtocolType)this.StartProtocolCB.SelectedItem;
                GXDLMSDirector.Properties.Settings.Default.SelectedManufacturer = man.Name;

                Device.Security = (Security)SecurityCB.SelectedItem;
                Device.SystemTitle = GetAsHex(SystemTitleTB.Text, SystemTitleAsciiCb.Checked);
                Device.BlockCipherKey = GetAsHex(BlockCipherKeyTB.Text, BlockCipherKeyAsciiCb.Checked);
                Device.AuthenticationKey = GetAsHex(AuthenticationKeyTB.Text, AuthenticationKeyAsciiCb.Checked);
                Device.InvocationCounter = UInt32.Parse(InvocationCounterTB.Text);
                Device.Challenge = GXCommon.ToHex(GXCommon.HexToBytes(ChallengeTB.Text), false);
                UpdateConformance();
            }
            catch (Exception Ex)
            {
                this.DialogResult = DialogResult.None;
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        public static string GetAsHex(string value, bool ascii)
        {
            if (ascii)
            {
                return GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(value), false);
            }
            return GXCommon.ToHex(GXCommon.HexToBytes(value), false);
        }

        /// <summary>
        /// Draw media name to media compobox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediasCB_DrawItem(object sender, DrawItemEventArgs e)
        {
            // If the index is invalid then simply exit.
            if (e.Index == -1 || e.Index >= MediasCB.Items.Count)
            {
                return;
            }

            // Draw the background of the item.
            e.DrawBackground();

            // Should we draw the focus rectangle?
            if ((e.State & DrawItemState.Focus) != 0)
            {
                e.DrawFocusRectangle();
            }

            Font f = new Font(e.Font, FontStyle.Regular);
            // Create a new background brush.
            Brush b = new SolidBrush(e.ForeColor);
            // Draw the item.
            Gurux.Common.IGXMedia target = (Gurux.Common.IGXMedia)MediasCB.Items[e.Index];
            if (target == null)
            {
                return;
            }
            string name = target.MediaType;
            SizeF s = e.Graphics.MeasureString(name, f);
            e.Graphics.DrawString(name, f, b, e.Bounds);
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {

        }

        private void AuthenticationCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GXAuthentication authentication = (GXAuthentication)AuthenticationCB.SelectedItem;
                PasswordTB.Enabled = authentication.Type != Authentication.None && authentication.Type != Authentication.HighGMAC && authentication.Type != Authentication.HighECDSA;
                this.ClientAddTB.Value = authentication.ClientAddress;
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void ManufacturerCB_DrawItem(object sender, DrawItemEventArgs e)
        {
            // If the index is invalid then simply exit.
            if (e.Index == -1 || e.Index >= ManufacturerCB.Items.Count)
            {
                return;
            }

            // Draw the background of the item.
            e.DrawBackground();

            // Should we draw the focus rectangle?
            if ((e.State & DrawItemState.Focus) != 0)
            {
                e.DrawFocusRectangle();
            }

            Font f = new Font(e.Font, FontStyle.Regular);
            // Create a new background brush.
            Brush b = new SolidBrush(e.ForeColor);
            // Draw the item.
            GXManufacturer target = (GXManufacturer)ManufacturerCB.Items[e.Index];
            if (target == null)
            {
                return;
            }
            string name = target.Name;
            SizeF s = e.Graphics.MeasureString(name, f);
            e.Graphics.DrawString(name, f, b, e.Bounds);
        }

        /// <summary>
        /// Show Serial port settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdvancedBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ((GXSerial)SelectedMedia).PortName = this.SerialPortCB.Text;
                if (SelectedMedia.Properties(this))
                {
                    this.SerialPortCB.Text = ((GXSerial)SelectedMedia).PortName;
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void AuthenticationCB_DrawItem(object sender, DrawItemEventArgs e)
        {
            // If the index is invalid then simply exit.
            if (e.Index == -1 || e.Index >= AuthenticationCB.Items.Count)
            {
                return;
            }

            // Draw the background of the item.
            e.DrawBackground();

            // Should we draw the focus rectangle?
            if ((e.State & DrawItemState.Focus) != 0)
            {
                e.DrawFocusRectangle();
            }

            Font f = new Font(e.Font, FontStyle.Regular);
            // Create a new background brush.
            Brush b = new SolidBrush(e.ForeColor);
            // Draw the item.
            GXAuthentication authentication = (GXAuthentication)AuthenticationCB.Items[e.Index];
            if (authentication == null)
            {
                return;
            }
            string name = authentication.Type.ToString();
            SizeF s = e.Graphics.MeasureString(name, f);
            e.Graphics.DrawString(name, f, b, e.Bounds);
        }

        void UpdateStartProtocol()
        {
            //If IEC47 is used DLMS is only protocol.
            GXManufacturer man = this.ManufacturerCB.SelectedItem as GXManufacturer;
            this.UseLNCB.Checked = man.UseLogicalNameReferencing;
            if (SelectedMedia is GXNet && man != null)
            {
                StartProtocolCB.Enabled = !man.UseIEC47;
            }
            else
            {
                StartProtocolCB.Enabled = true;
            }
            if (!StartProtocolCB.Enabled)
            {
                StartProtocolCB.SelectedItem = StartProtocolType.DLMS;
            }
        }

        bool IsPrintable(byte[] str)
        {
            if (str != null)
            {
                foreach (char it in str)
                {
                    if (!Char.IsLetterOrDigit(it))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void ManufacturerCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GXManufacturer man = (GXManufacturer)ManufacturerCB.SelectedItem;
                StartProtocolCB.SelectedItem = man.StartProtocol;
                this.ClientAddTB.Value = man.GetActiveAuthentication().ClientAddress;
                AuthenticationCB.Items.Clear();
                foreach (GXAuthentication it in man.Settings)
                {
                    int pos = AuthenticationCB.Items.Add(it);
                    if (it.Type == Device.Authentication)
                    {
                        this.AuthenticationCB.SelectedIndex = pos;
                    }
                }
                ServerAddressTypeCB.Items.Clear();
                HDLCAddressType type = Device.HDLCAddressing;
                //If we are creating new device.
                if (Device.Name == null)
                {
                    type = man.GetActiveServer().HDLCAddress;
                }
                foreach (GXServerAddress it in ((GXManufacturer)ManufacturerCB.SelectedItem).ServerSettings)
                {
                    ServerAddressTypeCB.Items.Add(it);
                    if (it.HDLCAddress == type)
                    {
                        ServerAddressTypeCB.SelectedItem = it;
                    }
                }
                UpdateStartProtocol();
                SecurityCB.SelectedItem = man.Security;
                SystemTitleAsciiCb.CheckedChanged -= SystemTitleAsciiCb_CheckedChanged;
                BlockCipherKeyAsciiCb.CheckedChanged -= BlockCipherKeyAsciiCb_CheckedChanged;
                AuthenticationKeyAsciiCb.CheckedChanged -= AuthenticationKeyAsciiCb_CheckedChanged;

                SystemTitleAsciiCb.Checked = IsAscii(man.SystemTitle);
                if (SystemTitleAsciiCb.Checked)
                {
                    SystemTitleTB.Text = ASCIIEncoding.ASCII.GetString(man.SystemTitle);
                }
                else
                {
                    SystemTitleTB.Text = GXCommon.ToHex(man.SystemTitle, true);
                }

                BlockCipherKeyAsciiCb.Checked = IsAscii(man.BlockCipherKey);
                if (BlockCipherKeyAsciiCb.Checked)
                {
                    SystemTitleTB.Text = ASCIIEncoding.ASCII.GetString(man.BlockCipherKey);
                }
                else
                {
                    BlockCipherKeyTB.Text = GXCommon.ToHex(man.BlockCipherKey, true);
                }

                AuthenticationKeyAsciiCb.Checked = IsAscii(man.AuthenticationKey);
                if (AuthenticationKeyAsciiCb.Checked)
                {
                    SystemTitleTB.Text = ASCIIEncoding.ASCII.GetString(man.AuthenticationKey);
                }
                else
                {
                    AuthenticationKeyTB.Text = GXCommon.ToHex(man.AuthenticationKey, true);
                }

                InvocationCounterTB.Text = "0";
                ChallengeTB.Text = "";

                SystemTitleAsciiCb.CheckedChanged += SystemTitleAsciiCb_CheckedChanged;
                BlockCipherKeyAsciiCb.CheckedChanged += BlockCipherKeyAsciiCb_CheckedChanged;
                AuthenticationKeyAsciiCb.CheckedChanged += AuthenticationKeyAsciiCb_CheckedChanged;
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void StartProtocolCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (object it in this.MediasCB.Items)
                {
                    if (it is GXSerial)
                    {
                        //Initialize serial settings.
                        GXSerial serial = (GXSerial)it;
                        if ((StartProtocolType)StartProtocolCB.SelectedItem == StartProtocolType.DLMS)
                        {
                            serial.BaudRate = 9600;
                            serial.DataBits = 8;
                            serial.Parity = Parity.None;
                            serial.StopBits = StopBits.One;
                        }
                        else
                        {
                            serial.BaudRate = 300;
                            serial.DataBits = 7;
                            serial.Parity = Parity.Even;
                            serial.StopBits = StopBits.One;
                        }
                        break;
                    }
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void TerminalAdvancedBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Gurux.Terminal.GXTerminal terminal = SelectedMedia as Gurux.Terminal.GXTerminal;
                terminal.PortName = this.TerminalPortCB.Text;
                terminal.PhoneNumber = this.TerminalPhoneNumberTB.Text;
                if (SelectedMedia.Properties(this))
                {
                    this.TerminalPortCB.Text = terminal.PortName;
                    this.TerminalPhoneNumberTB.Text = terminal.PhoneNumber;
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void ServerAddressTypeCB_DrawItem(object sender, DrawItemEventArgs e)
        {
            // If the index is invalid then simply exit.
            if (e.Index == -1 || e.Index >= ServerAddressTypeCB.Items.Count)
            {
                return;
            }

            // Draw the background of the item.
            e.DrawBackground();

            // Should we draw the focus rectangle?
            if ((e.State & DrawItemState.Focus) != 0)
            {
                e.DrawFocusRectangle();
            }

            Font f = new Font(e.Font, FontStyle.Regular);
            // Create a new background brush.
            Brush b = new SolidBrush(e.ForeColor);
            // Draw the item.
            GXServerAddress item = (GXServerAddress)ServerAddressTypeCB.Items[e.Index];
            if (item == null)
            {
                return;
            }
            string name = item.HDLCAddress.ToString();
            SizeF s = e.Graphics.MeasureString(name, f);
            e.Graphics.DrawString(name, f, b, e.Bounds);
        }

        private void ServerAddressTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            GXServerAddress server = (GXServerAddress)ServerAddressTypeCB.SelectedItem;
            PhysicalServerAddressTB.Hexadecimal = server.HDLCAddress != HDLCAddressType.SerialNumber;
            this.PhysicalServerAddressTB.Value = Convert.ToDecimal(server.PhysicalAddress);
            this.LogicalServerAddressTB.Value = server.LogicalAddress;
            if (!PhysicalServerAddressTB.Hexadecimal)
            {
                PhysicalServerAddressLbl.Text = "Serial Number:";
            }
            else
            {
                PhysicalServerAddressLbl.Text = "Physical Server:";

            }
        }

        private void InitialSettingsBtn_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.gurux.fi/index.php?q=GXDLMSDirectorExample");
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }

        }

        private void UseMaximumBaudRateCB_CheckedChanged(object sender, EventArgs e)
        {
            MaximumBaudRateCB.Enabled = UseMaximumBaudRateCB.Checked;
            if (MaximumBaudRateCB.SelectedItem == null)
            {
                MaximumBaudRateCB.SelectedItem = 300;
            }
        }

        private void SecurityCB_DrawItem(object sender, DrawItemEventArgs e)
        {
            // If the index is invalid then simply exit.
            if (e.Index == -1 || e.Index >= SecurityCB.Items.Count)
            {
                return;
            }

            // Draw the background of the item.
            e.DrawBackground();

            // Should we draw the focus rectangle?
            if ((e.State & DrawItemState.Focus) != 0)
            {
                e.DrawFocusRectangle();
            }

            Font f = new Font(e.Font, FontStyle.Regular);
            // Create a new background brush.
            Brush b = new SolidBrush(e.ForeColor);
            // Draw the item.
            Security security = (Security)SecurityCB.Items[e.Index];
            string name = security.ToString();
            SizeF s = e.Graphics.MeasureString(name, f);
            e.Graphics.DrawString(name, f, b, e.Bounds);

        }

        private void UseLNCB_CheckedChanged(object sender, EventArgs e)
        {
            Conformance c = GXDLMSClient.GetInitialConformance(UseLNCB.Checked);
            ShowConformance(c);
        }

        public static bool IsAscii(byte[] value)
        {
            if (value == null)
            {
                return false;
            }
            foreach (byte it in value)
            {
                if (it < 0x21 || it > 0x7E)
                {
                    return false;
                }
            }
            return true;
        }

        private void SystemTitleAsciiCb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (SystemTitleAsciiCb.Checked)
                {
                    if (!IsAscii(GXCommon.HexToBytes(SystemTitleTB.Text)))
                    {
                        SystemTitleAsciiCb.CheckedChanged -= SystemTitleAsciiCb_CheckedChanged;
                        SystemTitleAsciiCb.Checked = !SystemTitleAsciiCb.Checked;
                        SystemTitleAsciiCb.CheckedChanged += SystemTitleAsciiCb_CheckedChanged;
                        throw new ArgumentOutOfRangeException(Properties.Resources.InvalidASCII);
                    }
                    SystemTitleTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(SystemTitleTB.Text));
                }
                else
                {
                    SystemTitleTB.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(SystemTitleTB.Text), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void BlockCipherKeyAsciiCb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (BlockCipherKeyAsciiCb.Checked)
                {
                    if (!IsAscii(GXCommon.HexToBytes(BlockCipherKeyTB.Text)))
                    {
                        BlockCipherKeyAsciiCb.CheckedChanged -= BlockCipherKeyAsciiCb_CheckedChanged;
                        BlockCipherKeyAsciiCb.Checked = !BlockCipherKeyAsciiCb.Checked;
                        BlockCipherKeyAsciiCb.CheckedChanged += BlockCipherKeyAsciiCb_CheckedChanged;
                        throw new ArgumentOutOfRangeException(Properties.Resources.InvalidASCII);
                    }
                    BlockCipherKeyTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(BlockCipherKeyTB.Text));
                }
                else
                {
                    BlockCipherKeyTB.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(BlockCipherKeyTB.Text), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void AuthenticationKeyAsciiCb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (AuthenticationKeyAsciiCb.Checked)
                {
                    if (!IsAscii(GXCommon.HexToBytes(AuthenticationKeyTB.Text)))
                    {
                        AuthenticationKeyAsciiCb.CheckedChanged -= AuthenticationKeyAsciiCb_CheckedChanged;
                        AuthenticationKeyAsciiCb.Checked = !AuthenticationKeyAsciiCb.Checked;
                        AuthenticationKeyAsciiCb.CheckedChanged += AuthenticationKeyAsciiCb_CheckedChanged;
                        throw new ArgumentOutOfRangeException(Properties.Resources.InvalidASCII);
                    }
                    AuthenticationKeyTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(AuthenticationKeyTB.Text));
                }
                else
                {
                    AuthenticationKeyTB.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(AuthenticationKeyTB.Text), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void PasswordAsciiCb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (PasswordAsciiCb.Checked)
                {
                    if (!IsAscii(GXCommon.HexToBytes(PasswordTB.Text)))
                    {
                        PasswordAsciiCb.CheckedChanged -= PasswordAsciiCb_CheckedChanged;
                        PasswordAsciiCb.Checked = !PasswordAsciiCb.Checked;
                        PasswordAsciiCb.CheckedChanged += PasswordAsciiCb_CheckedChanged;
                        throw new ArgumentOutOfRangeException(Properties.Resources.InvalidASCII);
                    }
                    PasswordTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(PasswordTB.Text));
                }
                else
                {
                    PasswordTB.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(PasswordTB.Text), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }
    }
}
