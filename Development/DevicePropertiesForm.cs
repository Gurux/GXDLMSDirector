//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
//
// Version:         $Revision: 11471 $,
//                  $Date: 2020-02-12 13:07:50 +0200 (ke, 12 helmi 2020) $
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

using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gurux.Serial;
using Gurux.DLMS;
using Gurux.Net;
using System.Reflection;
using System.IO.Ports;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.Terminal;
using Gurux.DLMS.Enums;
using Gurux.Common;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

namespace GXDLMSDirector
{
    partial class DevicePropertiesForm : Form
    {
        Form MediaPropertiesForm = null;
        GXManufacturerCollection Manufacturers;
        IGXMedia SelectedMedia = null;
        public GXDLMSMeter Device = null;
        public GXDLMSMeter CopyDevice = null;
        public DevicePropertiesForm(GXManufacturerCollection manufacturers, GXDLMSMeter dev2)
        {
            if (manufacturers.Count == 0)
            {
                throw new Exception(Properties.Resources.ManufacturerSettingsMissing);
            }
            try
            {
                InitializeComponent();
                ServerAddressSizeCb.Items.Add("");
                ServerAddressSizeCb.Items.Add((byte)1);
                ServerAddressSizeCb.Items.Add((byte)2);
                ServerAddressSizeCb.Items.Add((byte)4);
                foreach (object it in Enum.GetValues(typeof(Standard)))
                {
                    StandardCb.Items.Add(it);
                }
                foreach (InterfaceType it in Enum.GetValues(typeof(InterfaceType)))
                {
                    if (it != InterfaceType.PDU)
                    {
                        InterfaceCb.Items.Add(it);
                    }

                }


                PriorityCb.Items.Add(Priority.Normal);
                PriorityCb.Items.Add(Priority.High);
                ServiceClassCb.Items.Add(ServiceClass.UnConfirmed);
                ServiceClassCb.Items.Add(ServiceClass.Confirmed);
                LNSettings.Dock = SNSettings.Dock = DockStyle.Fill;
                SecurityCB.Items.AddRange(new object[] { Security.None, Security.Authentication,
                                      Security.Encryption, Security.AuthenticationEncryption
                                                   });
                NetProtocolCB.Items.AddRange(new object[] { NetworkType.Tcp, NetworkType.Udp });
                this.ServerAddressTypeCB.SelectedIndexChanged += new System.EventHandler(this.ServerAddressTypeCB_SelectedIndexChanged);
                NetworkSettingsGB.Width = this.Width - NetworkSettingsGB.Left;
                CustomSettings.Bounds = SerialSettingsGB.Bounds = TerminalSettingsGB.Bounds = NetworkSettingsGB.Bounds;
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
                if (Device == null || Device.Name == null)
                {
                    Device = new GXDLMSDevice(null);
                    //Select first manufacturer.
                    if (Manufacturers.Count != 0)
                    {
                        ManufacturerCB.SelectedIndex = pos;
                    }
                    Device.Conformance = (int)GXDLMSClient.GetInitialConformance(UseLNCB.Checked);
                    FrameCounterTb.ReadOnly = true;
                    UpdateDeviceSettings(Device);
                }
                else
                {
                    UpdateDeviceSettings(Device);
                }
                ManufacturerCB.DrawMode = MediasCB.DrawMode = DrawMode.OwnerDrawFixed;
                UpdateMediaSettings();
                UseProtectedReleaseCb.Checked = Device.UseProtectedRelease;
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void UpdateMediaSettings()
        {
            this.MediasCB.Items.Clear();
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
                if (ManufacturerCB.SelectedItem != null && ((GXManufacturer)ManufacturerCB.SelectedItem).StartProtocol == StartProtocolType.DLMS)
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
            if (SelectedMedia is GXTerminal)
            {
                this.MediasCB.Items.Add(SelectedMedia);
                string[] ports = GXTerminal.GetPortNames();
                this.TerminalPortCB.Items.AddRange(ports);
                if (ports.Length != 0)
                {
                    this.TerminalPortCB.SelectedItem = ((GXTerminal)SelectedMedia).PortName;
                }
                this.TerminalPhoneNumberTB.Text = ((GXTerminal)SelectedMedia).PhoneNumber;
            }
            else
            {
                //Initialize terminal settings.
                GXTerminal termial = new GXTerminal();
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

            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (a != typeof(GXTerminal).Assembly &&
                        a != typeof(GXSerial).Assembly &&
                    a != typeof(GXNet).Assembly)
                {
                    try
                    {
                        foreach (Type type in a.GetTypes())
                        {
                            if (!type.IsAbstract && type.IsClass && typeof(IGXMedia).IsAssignableFrom(type))
                            {
                                if (SelectedMedia == null || SelectedMedia.GetType() != type)
                                {
                                    MediasCB.Items.Add(a.CreateInstance(type.ToString()));
                                }
                                else
                                {
                                    MediasCB.Items.Add(SelectedMedia);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //It's OK if this fails.
                    }
                }
            }

            //Select first media if media is not selected.
            if (SelectedMedia == null)
            {
                SelectedMedia = (Gurux.Common.IGXMedia)this.MediasCB.Items[0];
            }
            this.MediasCB.SelectedItem = SelectedMedia;
            bool bConnected = (Device is GXDLMSDevice) && (Device as GXDLMSDevice).Media != null && (Device as GXDLMSDevice).Media.IsOpen;
            WaitTimeTB.Enabled = SerialPortCB.Enabled = AdvancedBtn.Enabled = ManufacturerCB.Enabled = MediasCB.Enabled =
                                       AuthenticationCB.Enabled = UseRemoteSerialCB.Enabled = OKBtn.Enabled = !bConnected;
            HostNameTB.ReadOnly = PortTB.ReadOnly = PasswordTB.ReadOnly = ResendTb.ReadOnly = PhysicalServerAddressTB.ReadOnly = NameTB.ReadOnly = bConnected;

        }
        private void UpdateDeviceSettings(GXDLMSMeter device)
        {
            Device = device;
            foreach (GXManufacturer it in this.ManufacturerCB.Items)
            {
                if (string.Compare(it.Identification, device.Manufacturer, true) == 0)
                {
                    this.ManufacturerCB.SelectedItem = it;
                    break;
                }
            }
            if (this.ManufacturerCB.SelectedItem == null)
            {
                throw new Exception("Invalid manufacturer. " + device.Manufacturer);
            }
            StandardCb.SelectedItem = device.Standard;
            if (IsAscii(GXCommon.HexToBytes(device.SystemTitle)))
            {
                SystemTitleAsciiCb.CheckedChanged -= SystemTitleAsciiCb_CheckedChanged;
                SystemTitleAsciiCb.Checked = true;
                SystemTitleAsciiCb.CheckedChanged += SystemTitleAsciiCb_CheckedChanged;
                SystemTitleTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(device.SystemTitle));
            }
            else
            {
                SystemTitleAsciiCb.CheckedChanged -= SystemTitleAsciiCb_CheckedChanged;
                SystemTitleAsciiCb.Checked = false;
                SystemTitleAsciiCb.CheckedChanged += SystemTitleAsciiCb_CheckedChanged;
                SystemTitleTB.Text = device.SystemTitle;
            }
            if (IsAscii(GXCommon.HexToBytes(device.BlockCipherKey)))
            {
                BlockCipherKeyAsciiCb.CheckedChanged -= BlockCipherKeyAsciiCb_CheckedChanged;
                BlockCipherKeyAsciiCb.Checked = true;
                BlockCipherKeyAsciiCb.CheckedChanged += BlockCipherKeyAsciiCb_CheckedChanged;
                BlockCipherKeyTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(device.BlockCipherKey));
            }
            else
            {
                BlockCipherKeyAsciiCb.CheckedChanged -= BlockCipherKeyAsciiCb_CheckedChanged;
                BlockCipherKeyAsciiCb.Checked = false;
                BlockCipherKeyAsciiCb.CheckedChanged += BlockCipherKeyAsciiCb_CheckedChanged;
                BlockCipherKeyTB.Text = device.BlockCipherKey;
            }
            if (IsAscii(GXCommon.HexToBytes(device.AuthenticationKey)))
            {
                AuthenticationKeyAsciiCb.CheckedChanged -= AuthenticationKeyAsciiCb_CheckedChanged;
                AuthenticationKeyAsciiCb.Checked = true;
                AuthenticationKeyAsciiCb.CheckedChanged += AuthenticationKeyAsciiCb_CheckedChanged;
                AuthenticationKeyTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(device.AuthenticationKey));
            }
            else
            {
                AuthenticationKeyAsciiCb.CheckedChanged -= AuthenticationKeyAsciiCb_CheckedChanged;
                AuthenticationKeyAsciiCb.Checked = false;
                AuthenticationKeyAsciiCb.CheckedChanged += AuthenticationKeyAsciiCb_CheckedChanged;
                AuthenticationKeyTB.Text = device.AuthenticationKey;
            }

            if (IsAscii(GXCommon.HexToBytes(device.DedicatedKey)))
            {
                DedicatedKeyAsciiCb.CheckedChanged -= DedicatedKeyAsciiCb_CheckedChanged;
                DedicatedKeyAsciiCb.Checked = true;
                DedicatedKeyAsciiCb.CheckedChanged += DedicatedKeyAsciiCb_CheckedChanged;
                DedicatedKeyTb.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(device.DedicatedKey));
            }
            else
            {
                DedicatedKeyAsciiCb.CheckedChanged -= DedicatedKeyAsciiCb_CheckedChanged;
                DedicatedKeyAsciiCb.Checked = false;
                DedicatedKeyAsciiCb.CheckedChanged += DedicatedKeyAsciiCb_CheckedChanged;
                DedicatedKeyTb.Text = device.DedicatedKey;
            }

            if (IsAscii(GXCommon.HexToBytes(device.ServerSystemTitle)))
            {
                ServerSystemTitleAsciiCb.CheckedChanged -= ServerSystemTitleAsciiCb_CheckedChanged;
                ServerSystemTitleAsciiCb.Checked = true;
                ServerSystemTitleAsciiCb.CheckedChanged += ServerSystemTitleAsciiCb_CheckedChanged;
                ServerSystemTitle.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(device.ServerSystemTitle));
            }
            else
            {
                ServerSystemTitleAsciiCb.CheckedChanged -= ServerSystemTitleAsciiCb_CheckedChanged;
                ServerSystemTitleAsciiCb.Checked = false;
                ServerSystemTitleAsciiCb.CheckedChanged += ServerSystemTitleAsciiCb_CheckedChanged;
                ServerSystemTitle.Text = device.ServerSystemTitle;
            }
            UsePreEstablishedApplicationAssociations.Checked = device.PreEstablished;

            this.VerboseModeCB.Checked = device.Verbose;
            this.NameTB.Text = device.Name;
            if ((Device is GXDLMSDevice))
            {
                SelectedMedia = (Device as GXDLMSDevice).Media;
                if (SelectedMedia != null)
                {
                    SelectedMedia.Settings = device.MediaSettings;
                }
            }
            UseRemoteSerialCB.Checked = device.UseRemoteSerial;
            StartProtocolCB.SelectedItem = device.StartProtocol;
            PhysicalServerAddressTB.Value = Convert.ToDecimal(device.PhysicalAddress);
            LogicalServerAddressTB.Value = Convert.ToDecimal(device.LogicalAddress);
            this.ClientAddTB.Value = Convert.ToDecimal(Convert.ToUInt32(device.ClientAddress));
            WaitTimeTB.Value = new DateTime(2000, 1, 1).AddSeconds(device.WaitTime);
            ResendTb.Value = device.ResendCount;
            SecurityCB.SelectedItem = device.Security;
            InvocationCounterTB.Text = device.InvocationCounter.ToString();
            FrameCounterTb.Text = device.FrameCounter;
            FrameCounterTb.ReadOnly = true;
            InvocationCounterCb.Checked = FrameCounterTb.Text != "";
            ChallengeTB.Text = GXCommon.ToHex(GXCommon.HexToBytes(device.Challenge), true);
            UseUtcTimeZone.Checked = device.UtcTimeZone;
            if (!string.IsNullOrEmpty(device.Password))
            {
                PasswordTB.Text = ASCIIEncoding.ASCII.GetString(CryptHelper.Decrypt(device.Password, Password.Key));
            }
            else if (device.HexPassword != null)
            {
                byte[] pw = CryptHelper.Decrypt(device.HexPassword, Password.Key);
                PasswordAsciiCb.CheckedChanged -= PasswordAsciiCb_CheckedChanged;
                PasswordAsciiCb.Checked = false;
                PasswordAsciiCb.CheckedChanged += PasswordAsciiCb_CheckedChanged;
                PasswordTB.Text = GXDLMSTranslator.ToHex(pw);
            }
            this.UseLNCB.CheckedChanged -= new System.EventHandler(this.UseLNCB_CheckedChanged);
            this.UseLNCB.Checked = device.UseLogicalNameReferencing;
            this.UseLNCB.CheckedChanged += new System.EventHandler(this.UseLNCB_CheckedChanged);
            ShowConformance((Conformance)device.Conformance);

            InterfaceCb.SelectedItem = device.InterfaceType;
            MaxInfoTXTb.Text = device.MaxInfoTX.ToString();
            MaxInfoRXTb.Text = device.MaxInfoRX.ToString();
            WindowSizeTXTb.Text = device.WindowSizeTX.ToString();
            WindowSizeRXTb.Text = device.WindowSizeRX.ToString();
            InactivityTimeoutTb.Text = device.InactivityTimeout.ToString();
            MaxPduTb.Text = device.PduSize.ToString();
            if (device.UserId != -1)
            {
                UserIdTb.Text = device.UserId.ToString();
            }
            PriorityCb.SelectedItem = device.Priority;
            ServiceClassCb.SelectedItem = device.ServiceClass;
            if (device.ServerAddressSize == 0)
            {
                //If server address is not used.
                ServerAddressSizeCb.SelectedIndex = -1;
            }
            else
            {
                //Forse to use server address size.
                ServerAddressSizeCb.SelectedItem = device.ServerAddressSize;
            }
            if (device.PhysicalDeviceAddress != null)
            {
                UseGatewayCb.Checked = true;
                NetworkIDTb.Text = device.NetworkId.ToString();
                if (IsAscii(GXCommon.HexToBytes(device.PhysicalDeviceAddress)))
                {
                    PhysicalDeviceAddressAsciiCb.CheckedChanged -= PhysicalDeviceAddressAsciiCb_CheckedChanged;
                    PhysicalDeviceAddressAsciiCb.Checked = true;
                    PhysicalDeviceAddressAsciiCb.CheckedChanged += PhysicalDeviceAddressAsciiCb_CheckedChanged;
                    PhysicalDeviceAddressTb.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(device.PhysicalDeviceAddress));
                }
                else
                {
                    PhysicalDeviceAddressAsciiCb.CheckedChanged -= PhysicalDeviceAddressAsciiCb_CheckedChanged;
                    PhysicalDeviceAddressAsciiCb.Checked = false;
                    PhysicalDeviceAddressAsciiCb.CheckedChanged += PhysicalDeviceAddressAsciiCb_CheckedChanged;
                    PhysicalDeviceAddressTb.Text = device.PhysicalDeviceAddress;
                }
            }
            else
            {
                UseGatewayCb.Checked = false;
                NetworkIDTb.Text = "0";
            }
            FrameSizeCb.Checked = Device.UseFrameSize;
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
                ActionCB.Checked = (c & Conformance.Action) != 0;
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
                    c |= Conformance.Action;
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
            Device.Conformance = (int)c;
        }

        private void MediasCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedMedia = (Gurux.Common.IGXMedia)MediasCB.SelectedItem;
                if (SelectedMedia is GXSerial || SelectedMedia is GXNet || SelectedMedia is GXTerminal)
                {
                    MediaPropertiesForm = null;
                    CustomSettings.Visible = false;
                    SerialSettingsGB.Visible = SelectedMedia is GXSerial;
                    NetworkSettingsGB.Visible = SelectedMedia is GXNet;
                    TerminalSettingsGB.Visible = SelectedMedia is GXTerminal;
                    if (SelectedMedia is GXNet && this.PortTB.Text == "")
                    {
                        this.PortTB.Text = "4059";
                    }
                }
                else
                {
                    SerialSettingsGB.Visible = NetworkSettingsGB.Visible = TerminalSettingsGB.Visible = false;
                    CustomSettings.Visible = true;

                    CustomSettings.Controls.Clear();
                    MediaPropertiesForm = SelectedMedia.PropertiesForm;
                    (MediaPropertiesForm as IGXPropertyPage).Initialize();
                    while (MediaPropertiesForm.Controls.Count != 0)
                    {
                        Control ctr = MediaPropertiesForm.Controls[0];
                        if (ctr is Panel)
                        {
                            if (!ctr.Enabled)
                            {
                                MediaPropertiesForm.Controls.RemoveAt(0);
                                continue;
                            }
                        }
                        CustomSettings.Controls.Add(ctr);
                        ctr.Visible = true;
                    }
                }
                UpdateStartProtocol();
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void UpdateSettings(GXDLMSMeter device, bool validate)
        {
            string name = NameTB.Text.Trim();
            if (validate && name.Length == 0)
            {
                throw new Exception("Invalid name.");
            }
            //Check security settings.
            if (validate && ((Security)SecurityCB.SelectedItem != Security.None ||
                ((GXAuthentication)this.AuthenticationCB.SelectedItem).Type == Authentication.HighGMAC))
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

                if (UsePreEstablishedApplicationAssociations.Checked)
                {
                    if (ServerSystemTitle.Text.Trim().Length == 0)
                    {
                        throw new ArgumentException("Invalid server system title.");
                    }
                }
            }
            GXServerAddress server = (GXServerAddress)ServerAddressTypeCB.SelectedItem;
            if (validate && server.HDLCAddress == HDLCAddressType.SerialNumber && PhysicalServerAddressTB.Value == 0)
            {
                throw new Exception("Invalid Serial Number.");
            }
            GXManufacturer man = (GXManufacturer)ManufacturerCB.SelectedItem;
            device.Authentication = ((GXAuthentication)this.AuthenticationCB.SelectedItem).Type;
            if (device.Authentication != Authentication.None)
            {
                if (PasswordAsciiCb.Checked)
                {
                    device.Password = CryptHelper.Encrypt(PasswordTB.Text, Password.Key);
                    device.HexPassword = null;
                }
                else
                {
                    device.Password = "";
                    device.HexPassword = CryptHelper.Encrypt(GXDLMSTranslator.HexToBytes(this.PasswordTB.Text), Password.Key);
                }
            }
            else
            {
                device.Password = "";
            }
            device.InterfaceType = (InterfaceType)InterfaceCb.SelectedItem;
            if (MaxInfoTXTb.Text == "")
            {
                device.MaxInfoTX = 128;
            }
            else
            {
                device.MaxInfoTX = UInt16.Parse(MaxInfoTXTb.Text);
            }
            if (MaxInfoRXTb.Text == "")
            {
                device.MaxInfoRX = 128;
            }
            else
            {
                device.MaxInfoRX = UInt16.Parse(MaxInfoRXTb.Text);
            }
            if (WindowSizeTXTb.Text == "")
            {
                device.WindowSizeTX = 1;
            }
            else
            {
                device.WindowSizeTX = byte.Parse(WindowSizeTXTb.Text);
            }
            if (WindowSizeRXTb.Text == "")
            {
                device.WindowSizeRX = 1;
            }
            else
            {
                device.WindowSizeRX = byte.Parse(WindowSizeRXTb.Text);
            }
            if (InactivityTimeoutTb.Text == "")
            {
                device.InactivityTimeout = 110;
            }
            else
            {
                device.InactivityTimeout = int.Parse(InactivityTimeoutTb.Text);
            }
            if (MaxPduTb.Text == "")
            {
                device.PduSize = 0xFFFF;
            }
            else
            {
                device.PduSize = UInt16.Parse(MaxPduTb.Text);
            }
            byte v;
            if (byte.TryParse(UserIdTb.Text, out v))
            {
                device.UserId = v;
            }
            else
            {
                device.UserId = -1;
            }
            if (PriorityCb.SelectedItem == null)
            {
                device.Priority = Priority.High;
            }
            else
            {
                device.Priority = (Priority)PriorityCb.SelectedItem;
            }
            if (ServiceClassCb.SelectedItem == null)
            {
                device.ServiceClass = ServiceClass.Confirmed;
            }
            else
            {
                device.ServiceClass = (ServiceClass)ServiceClassCb.SelectedItem;
            }
            if (ServerAddressSizeCb.SelectedItem is string)
            {
                device.ServerAddressSize = 0;
            }
            else
            {
                device.ServerAddressSize = Convert.ToByte(ServerAddressSizeCb.SelectedItem);
            }

            device.Name = name;
            if (device is GXDLMSDevice)
            {
                (device as GXDLMSDevice).Media = SelectedMedia;
            }
            device.Manufacturer = man.Identification;
            device.WaitTime = (int)(WaitTimeTB.Value - WaitTimeTB.Value.Date).TotalSeconds;
            device.ResendCount = Convert.ToInt32(ResendTb.Value);
            device.Verbose = VerboseModeCB.Checked;
            device.MaximumBaudRate = 0;
            device.UtcTimeZone = UseUtcTimeZone.Checked;

            if (SelectedMedia is GXSerial)
            {
                device.UseRemoteSerial = false;
                if (validate && this.SerialPortCB.Text.Length == 0)
                {
                    throw new Exception("Invalid serial port.");
                }
                ((GXSerial)SelectedMedia).PortName = this.SerialPortCB.Text;
                if (UseMaximumBaudRateCB.Checked)
                {
                    device.MaximumBaudRate = (int)MaximumBaudRateCB.SelectedItem;
                }
            }
            else if (SelectedMedia is GXNet)
            {
                if (validate && this.HostNameTB.Text.Length == 0)
                {
                    throw new Exception("Invalid host name.");
                }
                ((GXNet)SelectedMedia).HostName = this.HostNameTB.Text;
                int port;
                if (!Int32.TryParse(this.PortTB.Text, out port))
                {
                    if (validate)
                    {
                        port = 0;
                    }
                    else
                    {
                        throw new Exception("Invalid port number.");
                    }
                }
                ((GXNet)SelectedMedia).Port = port;
                device.UseRemoteSerial = UseRemoteSerialCB.Checked;
                ((GXNet)SelectedMedia).Protocol = (NetworkType)NetProtocolCB.SelectedItem;
            }
            else if (SelectedMedia is Gurux.Terminal.GXTerminal)
            {
                if (validate && this.TerminalPortCB.Text.Length == 0)
                {
                    throw new Exception("Invalid serial port.");
                }
                if (validate && this.TerminalPhoneNumberTB.Text.Length == 0)
                {
                    throw new Exception("Invalid phone number.");
                }
                Gurux.Terminal.GXTerminal terminal = SelectedMedia as Gurux.Terminal.GXTerminal;
                terminal.ConfigurableSettings = Gurux.Terminal.AvailableMediaSettings.All & ~Gurux.Terminal.AvailableMediaSettings.Server;
                device.UseRemoteSerial = false;
                terminal.PortName = this.TerminalPortCB.Text;
                terminal.PhoneNumber = this.TerminalPhoneNumberTB.Text;
            }
            else
            {
                if (validate && (MediaPropertiesForm as IGXPropertyPage).Dirty)
                {
                    (MediaPropertiesForm as IGXPropertyPage).Apply();
                }
            }
            if (SelectedMedia != null)
            {
                device.MediaSettings = SelectedMedia.Settings;
            }
            GXAuthentication authentication = (GXAuthentication)AuthenticationCB.SelectedItem;
            device.HDLCAddressing = ((GXServerAddress)ServerAddressTypeCB.SelectedItem).HDLCAddress;
            device.ClientAddress = Convert.ToInt32(ClientAddTB.Value);
            if (device.HDLCAddressing == HDLCAddressType.SerialNumber)
            {
                device.PhysicalAddress = (int)PhysicalServerAddressTB.Value;
            }
            else
            {
                device.PhysicalAddress = (int)PhysicalServerAddressTB.Value;
            }
            device.UseLogicalNameReferencing = this.UseLNCB.Checked;
            device.LogicalAddress = Convert.ToInt32(LogicalServerAddressTB.Value);
            device.StartProtocol = (StartProtocolType)this.StartProtocolCB.SelectedItem;
            GXDLMSDirector.Properties.Settings.Default.SelectedManufacturer = man.Name;

            device.Security = (Security)SecurityCB.SelectedItem;
            device.SystemTitle = GetAsHex(SystemTitleTB.Text, SystemTitleAsciiCb.Checked);
            device.BlockCipherKey = GetAsHex(BlockCipherKeyTB.Text, BlockCipherKeyAsciiCb.Checked);
            device.AuthenticationKey = GetAsHex(AuthenticationKeyTB.Text, AuthenticationKeyAsciiCb.Checked);
            device.ServerSystemTitle = GetAsHex(ServerSystemTitle.Text, ServerSystemTitleAsciiCb.Checked);
            device.DedicatedKey = GetAsHex(DedicatedKeyTb.Text, DedicatedKeyAsciiCb.Checked);
            device.PreEstablished = UsePreEstablishedApplicationAssociations.Checked;
            device.UseProtectedRelease = UseProtectedReleaseCb.Checked;
            if (InvocationCounterTB.Text != "")
            {
                device.InvocationCounter = UInt32.Parse(InvocationCounterTB.Text);
            }
            else
            {
                device.InvocationCounter = 0;
            }
            if (InvocationCounterCb.Checked && FrameCounterTb.Text != "")
            {
                GXDLMSConverter.LogicalNameToBytes(FrameCounterTb.Text);
                device.FrameCounter = FrameCounterTb.Text;
            }
            else
            {
                device.FrameCounter = null;
            }
            device.Challenge = GXCommon.ToHex(GXCommon.HexToBytes(ChallengeTB.Text), false);
            UpdateConformance();
            device.Standard = (Standard)StandardCb.SelectedItem;

            if (UseGatewayCb.Checked)
            {
                device.NetworkId = byte.Parse(NetworkIDTb.Text);
                device.PhysicalDeviceAddress = GetAsHex(PhysicalDeviceAddressTb.Text, PhysicalDeviceAddressAsciiCb.Checked);
            }
            else
            {
                device.NetworkId = 0;
                device.PhysicalDeviceAddress = null;
            }
            device.UseFrameSize = FrameSizeCb.Checked;
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
                UpdateSettings(Device, true);
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
            if (Device.Name == null)
            {
                //If IEC47 is used DLMS is only protocol.
                GXManufacturer man = this.ManufacturerCB.SelectedItem as GXManufacturer;
                if (man != null)
                {
                    UseLNCB.Checked = Device.UseLogicalNameReferencing = man.UseLogicalNameReferencing;
                    if (SelectedMedia is GXNet)
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
                if (man != null)
                {
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
                        if (man.UseIEC47)
                        {
                            Device.InterfaceType = InterfaceType.WRAPPER;
                        }
                        Device.Standard = man.Standard;
                        Device.UtcTimeZone = man.UtcTimeZone;
                        StandardCb.SelectedItem = man.Standard;
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
                        BlockCipherKeyTB.Text = ASCIIEncoding.ASCII.GetString(man.BlockCipherKey);
                    }
                    else
                    {
                        BlockCipherKeyTB.Text = GXCommon.ToHex(man.BlockCipherKey, true);
                    }

                    AuthenticationKeyAsciiCb.Checked = man.AuthenticationKey == null || IsAscii(man.AuthenticationKey);
                    if (AuthenticationKeyAsciiCb.Checked)
                    {
                        if (man.AuthenticationKey == null)
                        {
                            AuthenticationKeyTB.Text = "";
                        }
                        else
                        {
                            AuthenticationKeyTB.Text = ASCIIEncoding.ASCII.GetString(man.AuthenticationKey);
                        }
                    }
                    else
                    {
                        AuthenticationKeyTB.Text = GXCommon.ToHex(man.AuthenticationKey, true);
                    }
                    ServerSystemTitleAsciiCb.Checked = IsAscii(man.ServerSystemTitle);
                    if (ServerSystemTitleAsciiCb.Checked)
                    {
                        ServerSystemTitle.Text = ASCIIEncoding.ASCII.GetString(man.ServerSystemTitle);
                    }
                    else
                    {
                        ServerSystemTitle.Text = GXCommon.ToHex(man.ServerSystemTitle, true);
                    }

                    InvocationCounterTB.Text = "0";
                    ChallengeTB.Text = "";

                    SystemTitleAsciiCb.CheckedChanged += SystemTitleAsciiCb_CheckedChanged;
                    BlockCipherKeyAsciiCb.CheckedChanged += BlockCipherKeyAsciiCb_CheckedChanged;
                    AuthenticationKeyAsciiCb.CheckedChanged += AuthenticationKeyAsciiCb_CheckedChanged;
                }
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

            if (server.Size == 0)
            {
                //If server address is not used.
                ServerAddressSizeCb.SelectedIndex = -1;
            }
            else
            {
                //Forse to use server address size.
                ServerAddressSizeCb.SelectedItem = Convert.ToByte(server.Size);
            }

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
                System.Diagnostics.Process.Start("https://www.gurux.fi/index.php?q=GXDLMSDirectorExample");
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

        private void InactivityTimeoutTb_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Enable server system title when Pre-Established Application Associations are used.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UsePreEstablishedApplicationAssociations_CheckedChanged(object sender, EventArgs e)
        {
            ServerSystemTitle.ReadOnly = !UsePreEstablishedApplicationAssociations.Checked;
        }

        private void ServerSystemTitleAsciiCb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ServerSystemTitleAsciiCb.Checked)
                {
                    if (!IsAscii(GXCommon.HexToBytes(ServerSystemTitle.Text)))
                    {
                        ServerSystemTitleAsciiCb.CheckedChanged -= ServerSystemTitleAsciiCb_CheckedChanged;
                        ServerSystemTitleAsciiCb.Checked = !ServerSystemTitleAsciiCb.Checked;
                        ServerSystemTitleAsciiCb.CheckedChanged += ServerSystemTitleAsciiCb_CheckedChanged;
                        throw new ArgumentOutOfRangeException(Properties.Resources.InvalidASCII);
                    }
                    ServerSystemTitle.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(ServerSystemTitle.Text));
                }
                else
                {
                    ServerSystemTitle.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(ServerSystemTitle.Text), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void InvocationCounterCb_CheckedChanged(object sender, EventArgs e)
        {
            bool c = InvocationCounterCb.Checked;
            InvocationCounterTB.ReadOnly = c;
            FrameCounterTb.ReadOnly = !c;
        }

        private void DeviceTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DeviceTab.SelectedTab == XmlTab)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        List<Type> types = new List<Type>(GXDLMSClient.GetObjectTypes());
                        types.Add(typeof(GXDLMSAttributeSettings));
                        types.Add(typeof(GXDLMSAttribute));
                        XmlAttributeOverrides overrides = new XmlAttributeOverrides();
                        XmlAttributes attribs = new XmlAttributes
                        {
                            XmlIgnore = true
                        };
                        overrides.Add(typeof(GXDLMSDevice), "ObsoleteObjects", attribs);
                        overrides.Add(typeof(GXDLMSAttributeSettings), attribs);
                        overrides.Add(typeof(GXDLMSDevice), "Objects", attribs);
                        XmlSerializer x = new XmlSerializer(typeof(GXDLMSDevice), overrides, types.ToArray(), null, "Gurux1");
                        using (TextWriter writer = new StreamWriter(ms))
                        {
                            using (TextReader reader = new StreamReader(ms))
                            {
                                if (CopyDevice == null)
                                {
                                    x.Serialize(writer, Device);
                                    ms.Position = 0;
                                    CopyDevice = (GXDLMSDevice)x.Deserialize(reader);
                                    ms.Position = 0;
                                }
                                UpdateSettings(CopyDevice, false);
                                x.Serialize(writer, CopyDevice);
                                ms.Position = 0;
                                textBox1.Text = reader.ReadToEnd();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message);
                }
            }
        }

        /// <summary>
        /// Apply xml settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                List<Type> types = new List<Type>();
                types.Add(typeof(GXDLMSAttributeSettings));
                types.Add(typeof(GXDLMSAttribute));
                XmlAttributeOverrides overrides = new XmlAttributeOverrides();
                XmlAttributes attribs = new XmlAttributes
                {
                    XmlIgnore = true
                };
                overrides.Add(typeof(GXDLMSDevice), "ObsoleteObjects", attribs);
                overrides.Add(typeof(GXDLMSAttributeSettings), attribs);
                overrides.Add(typeof(GXDLMSDevice), "Objects", attribs);
                //Version is added to namespace.
                XmlSerializer x = new XmlSerializer(typeof(GXDLMSDevice), overrides, types.ToArray(), null, "Gurux1");
                using (MemoryStream ms = new MemoryStream())
                {
                    using (TextWriter writer = new StreamWriter(ms))
                    {
                        writer.Write(textBox1.Text);
                        writer.Flush();
                        ms.Position = 0;
                        using (XmlReader reader = XmlReader.Create(ms))
                        {
                            CopyDevice = (GXDLMSDevice)x.Deserialize(reader);
                            UpdateDeviceSettings(CopyDevice);
                            UpdateMediaSettings();
                        }
                    }
                }
                MessageBox.Show(this, Properties.Resources.ReadyTxt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        /// <summary>
        /// Copy xml to clipboard.
        /// </summary>
        private void CopyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        /// <summary>
        /// Paste xml from Clipboard.
        /// </summary>
        private void PasteFromClipboardBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    textBox1.Text = Clipboard.GetText();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void DedicatedKeyAsciiCb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (DedicatedKeyAsciiCb.Checked)
                {
                    if (!IsAscii(GXCommon.HexToBytes(DedicatedKeyTb.Text)))
                    {
                        DedicatedKeyAsciiCb.CheckedChanged -= DedicatedKeyAsciiCb_CheckedChanged;
                        DedicatedKeyAsciiCb.Checked = !DedicatedKeyAsciiCb.Checked;
                        DedicatedKeyAsciiCb.CheckedChanged += DedicatedKeyAsciiCb_CheckedChanged;
                        throw new ArgumentOutOfRangeException(Properties.Resources.InvalidASCII);
                    }
                    DedicatedKeyTb.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(DedicatedKeyTb.Text));
                }
                else
                {
                    DedicatedKeyTb.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(DedicatedKeyTb.Text), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void PhysicalDeviceAddressAsciiCb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (PhysicalDeviceAddressAsciiCb.Checked)
                {
                    if (!IsAscii(GXCommon.HexToBytes(PhysicalDeviceAddressTb.Text)))
                    {
                        PhysicalDeviceAddressAsciiCb.CheckedChanged -= PhysicalDeviceAddressAsciiCb_CheckedChanged;
                        PhysicalDeviceAddressAsciiCb.Checked = !PhysicalDeviceAddressAsciiCb.Checked;
                        PhysicalDeviceAddressAsciiCb.CheckedChanged += PhysicalDeviceAddressAsciiCb_CheckedChanged;
                        throw new ArgumentOutOfRangeException(Properties.Resources.InvalidASCII);
                    }
                    PhysicalDeviceAddressTb.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(PhysicalDeviceAddressTb.Text));
                }
                else
                {
                    PhysicalDeviceAddressTb.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(PhysicalDeviceAddressTb.Text), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        /// <summary>
        /// Is gateway used.
        /// </summary>
        private void UseGatewayCb_CheckedChanged(object sender, EventArgs e)
        {
            NetworkIDTb.ReadOnly = PhysicalDeviceAddressTb.ReadOnly = !UseGatewayCb.Checked;
        }

        private void FrameSizeCb_CheckedChanged(object sender, EventArgs e)
        {
            if (FrameSizeCb.Checked)
            {
                MaxInfoTXLbl.Text = "Max frame size in transmit";
                MaxInfoRXLbl.Text = "Max frame size in receive";
            }
            else
            {
                MaxInfoTXLbl.Text = "Max payload size in transmit";
                MaxInfoRXLbl.Text = "Max payload size in receive";
            }
        }

        private void InterfaceCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            InterfaceType type = (InterfaceType)InterfaceCb.SelectedItem;
            if (type == InterfaceType.WRAPPER)
            {
                PhysicalServerAddressLbl.Text = "Logical device:";
            }
            else
            {
                PhysicalServerAddressLbl.Text = "Physical Server:";
            }
            LogicalServerAddressLbl.Visible = LogicalServerAddressTB.Visible = type == InterfaceType.HDLC;

        }
    }
}
