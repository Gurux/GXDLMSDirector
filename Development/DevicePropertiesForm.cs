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
using Gurux.DLMS.ASN;
using Gurux.DLMS.UI.Ecdsa;

namespace GXDLMSDirector
{
    partial class DevicePropertiesForm : Form
    {
        GXDeviceCipheringSettings ciphering;
        Form MediaPropertiesForm = null;
        GXManufacturerCollection Manufacturers;
        IGXMedia SelectedMedia = null;
        public GXDLMSMeter Device = null;
        public GXDLMSMeter CopyDevice = null;
        public DevicePropertiesForm(GXManufacturerCollection manufacturers, GXDLMSMeter dev)
        {
            if (manufacturers.Count == 0)
            {
                throw new Exception(Properties.Resources.ManufacturerSettingsMissing);
            }
            try
            {
                InitializeComponent();
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GXDLMSDirector");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string certificates = Path.Combine(path, "Certificates");
                if (!Directory.Exists(certificates))
                {
                    Directory.CreateDirectory(certificates);
                }
                string keys = Path.Combine(path, "Keys");
                if (!Directory.Exists(keys))
                {
                    Directory.CreateDirectory(keys);
                }
                Device = dev;
                ciphering = new GXDeviceCipheringSettings(Properties.Settings.Default.GeneratorAddress,
                    Properties.Resources.GXDLMSDirectorTxt, keys, certificates);
                TabPage tab = ciphering.GetCiphetingTab();
                ServerAddressSizeCb.Items.Add("");
                ServerAddressSizeCb.Items.Add((byte)1);
                ServerAddressSizeCb.Items.Add((byte)2);
                ServerAddressSizeCb.Items.Add((byte)4);
                ChallengeSizeCb.Items.Add("Random");
                ChallengeSizeCb.Items.Add((byte)8);
                ChallengeSizeCb.Items.Add((byte)16);
                ChallengeSizeCb.Items.Add((byte)32);
                ChallengeSizeCb.Items.Add((byte)64);
                foreach (object it in Enum.GetValues(typeof(Standard)))
                {
                    StandardCb.Items.Add(it);
                }
                PriorityCb.Items.Add(Priority.Normal);
                PriorityCb.Items.Add(Priority.High);
                ServiceClassCb.Items.Add(ServiceClass.UnConfirmed);
                ServiceClassCb.Items.Add(ServiceClass.Confirmed);
                LNSettings.Dock = SNSettings.Dock = DockStyle.Fill;
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
                int pos = 0;
                foreach (GXManufacturer it in Manufacturers)
                {
                    int index = this.ManufacturerCB.Items.Add(it);
                    if (it.Name == Properties.Settings.Default.SelectedManufacturer)
                    {
                        pos = index;
                    }
                }
                if (Device == null || Device.Name == null)
                {
                    Device = new GXDLMSDevice(null);
                    GXManufacturer man = null;
                    //Select first manufacturer.
                    if (Manufacturers.Count != 0)
                    {
                        ManufacturerCB.SelectedIndex = pos;
                        man = (GXManufacturer)ManufacturerCB.SelectedItem;
                    }
                    Device.Conformance = (int)GXDLMSClient.GetInitialConformance(UseLNCB.Checked);
                    FrameCounterTb.ReadOnly = true;
                    if (man != null)
                    {
                        Device.Security = man.Security;
                        Device.Signing = (Signing)Properties.Settings.Default.Signing;
                        Device.SystemTitle = GXCommon.ToHex(man.SystemTitle, false);
                        Device.ServerSystemTitle = GXCommon.ToHex(man.ServerSystemTitle, false);
                        Device.BlockCipherKey = GXCommon.ToHex(man.BlockCipherKey, false);
                        Device.AuthenticationKey = GXCommon.ToHex(man.AuthenticationKey, false);
                        InvocationCounterTB.Text = Properties.Settings.Default.InvocationCounter.ToString();
                        Device.DedicatedKey = Properties.Settings.Default.DedicatedKey;
                    }
                    UpdateDeviceSettings(Device);
                }
                else
                {
                    UpdateDeviceSettings(Device);
                }
                ManufacturerCB.DrawMode = MediasCB.DrawMode = DrawMode.OwnerDrawFixed;
                UpdateMediaSettings();
                UseProtectedReleaseCb.Checked = Device.UseProtectedRelease;
                SecurityChangeCheckCb.Checked = Device.SecurityChangeCheck;
                while (tab.Controls.Count != 0)
                {
                    Control ctr = tab.Controls[0];
                    SettingsPanel.Controls.Add(ctr);
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(null, Ex);
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
                net.Protocol = NetworkType.Tcp;
                this.HostNameTB.Text = ((GXNet)SelectedMedia).HostName;
                this.PortTB.Text = ((GXNet)SelectedMedia).Port.ToString();
                NetProtocolCB.SelectedItem = ((GXNet)SelectedMedia).Protocol;
            }
            else
            {
                NetProtocolCB.SelectedItem = net.Protocol = NetworkType.Tcp;
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
            ciphering.Signing = device.Signing;
            ciphering.SignInitiateRequestResponse = device.SignInitiateRequestResponse;
            ciphering.SecuritySuite = device.SecuritySuite;
            ciphering.Security = device.Security;
            if (!string.IsNullOrEmpty(device.SystemTitle) && IsAscii(GXCommon.HexToBytes(device.SystemTitle)))
            {
                ciphering.SystemTitleAscii = true;
                ciphering.SystemTitle = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(device.SystemTitle));
            }
            else
            {
                ciphering.SystemTitleAscii = false;
                ciphering.SystemTitle = device.SystemTitle;
            }
            if (!string.IsNullOrEmpty(device.BlockCipherKey) && IsAscii(GXCommon.HexToBytes(device.BlockCipherKey)))
            {
                ciphering.BlockCipherKeyAscii = true;
                ciphering.BlockCipherKey = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(device.BlockCipherKey));
            }
            else
            {
                ciphering.BlockCipherKeyAscii = false;
                ciphering.BlockCipherKey = device.BlockCipherKey;
            }
            if (!string.IsNullOrEmpty(device.AuthenticationKey) && IsAscii(GXCommon.HexToBytes(device.AuthenticationKey)))
            {
                ciphering.AuthenticationKeyAscii = true;
                ciphering.AuthenticationKey = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(device.AuthenticationKey));
            }
            else
            {
                ciphering.AuthenticationKeyAscii = false;
                ciphering.AuthenticationKey = device.AuthenticationKey;
            }
            if (!string.IsNullOrEmpty(device.BroadcastKey) && IsAscii(GXCommon.HexToBytes(device.BroadcastKey)))
            {
                ciphering.BroadcastKeyAscii = true;
                ciphering.BroadcastKey = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(device.BroadcastKey));
            }
            else
            {
                ciphering.BroadcastKeyAscii = false;
                ciphering.BroadcastKey = device.BroadcastKey;
            }
            if (!string.IsNullOrEmpty(device.DedicatedKey) && IsAscii(GXCommon.HexToBytes(device.DedicatedKey)))
            {
                ciphering.DedicatedKeyAscii = true;
                ciphering.DedicatedKey = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(device.DedicatedKey));
            }
            else
            {
                ciphering.DedicatedKeyAscii = false;
                ciphering.DedicatedKey = device.DedicatedKey;
            }
            ciphering.ServerSystemTitle = device.ServerSystemTitle;
            ciphering.Signing = device.Signing;
            ciphering.SignInitiateRequestResponse = device.SignInitiateRequestResponse;
            ciphering.ClientSigningKey = device.ClientSigningKey;
            ciphering.ClientAgreementKey = device.ClientAgreementKey;
            ciphering.ServerSigningKey = device.ServerSigningKey;
            ciphering.ServerAgreementKey = device.ServerAgreementKey;
            ciphering.UpdateKeys();
            ciphering.PreEstablishedApplicationAssociations = device.PreEstablished;
            ciphering.IgnoreSNRMWithPreEstablished = device.IgnoreSNRMWithPreEstablished;
            VerboseModeCB.Checked = device.Verbose;
            NameTB.Text = device.Name;
            if ((Device is GXDLMSDevice))
            {
                SelectedMedia = (Device as GXDLMSDevice).Media;
                if (SelectedMedia != null)
                {
                    SelectedMedia.Settings = device.MediaSettings;
                }
            }
            UseRemoteSerialCB.Checked = device.UseRemoteSerial;
            BroadcastCb.Checked = device.Broadcast;
            PhysicalServerAddressTB.Value = Convert.ToDecimal(device.PhysicalAddress);
            LogicalServerAddressTB.Value = Convert.ToDecimal(device.LogicalAddress);
            this.ClientAddTB.Value = Convert.ToDecimal(Convert.ToUInt32(device.ClientAddress));
            WaitTimeTB.Value = new DateTime(2000, 1, 1).AddSeconds(device.WaitTime);
            ResendTb.Value = device.ResendCount;
            InvocationCounterTB.Text = device.InvocationCounter.ToString();
            FrameCounterTb.Text = device.FrameCounter;
            FrameCounterTb.ReadOnly = true;
            InvocationCounterCb.Checked = FrameCounterTb.Text != "";
            ciphering.Challenge = GXCommon.ToHex(GXCommon.HexToBytes(device.Challenge), true);
            UseUtcTimeZone.Checked = device.UtcTimeZone;
            IgnoreTimeZoneCb.Checked = (device.DateTimeSkips & DateTimeSkips.Deviation) != 0;
            IgnoreTimeStatusCb.Checked = (device.DateTimeSkips & DateTimeSkips.Status) != 0;
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
            PduWaitTimeTb.Text = device.PduWaitTime.ToString();
            MACSourceAddressTb.Value = device.MACSourceAddress;
            MACTargetAddressTb.Value = device.MacDestinationAddress;
            WindowSizeTXTb.Text = device.WindowSizeTX.ToString();
            WindowSizeRXTb.Text = device.WindowSizeRX.ToString();
            InactivityTimeoutTb.Text = device.InactivityTimeout.ToString();
            FrameDelayTb.Text = device.FrameDelay.ToString();
            ObjectDelayTb.Text = device.ObjectDelay.ToString();
            MaxPduTb.Text = device.PduSize.ToString();
            GBTWindowSizeTb.Text = device.GbtWindowSize.ToString();
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

            if (device.ChallengeSize == 0)
            {
                ChallengeSizeCb.SelectedIndex = 0;
            }
            else
            {
                //Forse to use server address size.
                ChallengeSizeCb.SelectedItem = device.ChallengeSize;
            }
            IncludePublicKeyCb.Checked = device.PublicKeyInInitialize;
            SignInitiateRequestResponseCb.Checked = device.SignInitiateRequestResponse;
            OverwriteAttributeAccessRightsCb.Checked = device.OverwriteAttributeAccessRights;
            IncreaseInvocationCounterForGMacAuthenticationCB.Checked = device.IncreaseInvocationCounterForGMacAuthentication;
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
                DeltaValueEncodingCb.Checked = (c & Conformance.DeltaValueEncoding) != 0;
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
                SNDeltaValueEncodingCb.Checked = (c & Conformance.DeltaValueEncoding) != 0;
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
                if (DeltaValueEncodingCb.Checked)
                {
                    c |= Conformance.DeltaValueEncoding;
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
                if (SNDeltaValueEncodingCb.Checked)
                {
                    c |= Conformance.DeltaValueEncoding;
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
            GXServerAddress server = (GXServerAddress)ServerAddressTypeCB.SelectedItem;
            if (validate && server.HDLCAddress == HDLCAddressType.SerialNumber && PhysicalServerAddressTB.Value == 0)
            {
                throw new Exception("Invalid Serial Number.");
            }
            GXManufacturer man = (GXManufacturer)ManufacturerCB.SelectedItem;
            GXAuthentication auth = ((GXAuthentication)AuthenticationCB.SelectedItem);
            if (auth == null)
            {
                AuthenticationCB.SelectedIndex = 0;
                auth = ((GXAuthentication)AuthenticationCB.SelectedItem);
            }
            device.Authentication = auth.Type;
            device.AuthenticationName = auth.Name;
            if (device.Authentication != Authentication.None)
            {
                if (device.Authentication == Authentication.HighECDSA &&
                    string.IsNullOrEmpty(ciphering.ClientSigningKey))
                {
                    throw new Exception("ECDSA needs client signing key. Set it in secured Connections tab.");
                }
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
            if (FrameDelayTb.Text == "")
            {
                device.FrameDelay = 0;
            }
            else
            {
                device.FrameDelay = int.Parse(FrameDelayTb.Text);
            }
            if (ObjectDelayTb.Text == "")
            {
                device.ObjectDelay = 0;
            }
            else
            {
                device.ObjectDelay = int.Parse(ObjectDelayTb.Text);
            }
            if (MACSourceAddressTb.Text == "")
            {
                device.MACSourceAddress = 0xC00;
            }
            else
            {
                device.MACSourceAddress = Convert.ToUInt16(MACSourceAddressTb.Value);
            }

            if (MACTargetAddressTb.Text == "")
            {
                device.MacDestinationAddress = 0;
            }
            else
            {
                device.MacDestinationAddress = Convert.ToUInt16(MACTargetAddressTb.Value);
            }
            if (MaxPduTb.Text == "")
            {
                device.PduSize = 0xFFFF;
            }
            else
            {
                device.PduSize = UInt16.Parse(MaxPduTb.Text);
            }
            device.GbtWindowSize = Byte.Parse(GBTWindowSizeTb.Text);
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

            if (ChallengeSizeCb.SelectedItem is string)
            {
                device.ChallengeSize = 0;
            }
            else
            {
                device.ChallengeSize = Convert.ToByte(ChallengeSizeCb.SelectedItem);
            }
            device.PublicKeyInInitialize = IncludePublicKeyCb.Checked;
            device.SignInitiateRequestResponse = SignInitiateRequestResponseCb.Checked;
            device.OverwriteAttributeAccessRights = OverwriteAttributeAccessRightsCb.Checked;
            device.IncreaseInvocationCounterForGMacAuthentication = IncreaseInvocationCounterForGMacAuthenticationCB.Checked;
            if (PduWaitTimeTb.Text != "")
            {
                device.PduWaitTime = int.Parse(PduWaitTimeTb.Text);
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
            device.DateTimeSkips = DateTimeSkips.None;
            if (IgnoreTimeZoneCb.Checked)
            {
                device.DateTimeSkips |= DateTimeSkips.Deviation;
            }
            if (IgnoreTimeStatusCb.Checked)
            {
                device.DateTimeSkips |= DateTimeSkips.Status;
            }
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
            device.Broadcast = BroadcastCb.Checked;
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
            Properties.Settings.Default.SelectedManufacturer = man.Name;
            device.Security = ciphering.Security;
            device.SecuritySuite = ciphering.SecuritySuite;
            device.SystemTitle = ciphering.SystemTitle;
            device.BlockCipherKey = ciphering.BlockCipherKey;
            device.AuthenticationKey = ciphering.AuthenticationKey;
            device.BroadcastKey = ciphering.BroadcastKey;
            device.ServerSystemTitle = ciphering.ServerSystemTitle;
            device.DedicatedKey = ciphering.DedicatedKey;
            device.ClientSigningKey = ciphering.ClientSigningKey;
            device.Signing = ciphering.Signing;
            device.SignInitiateRequestResponse = ciphering.SignInitiateRequestResponse;
            device.ClientAgreementKey = ciphering.ClientAgreementKey;
            device.ServerSigningKey = ciphering.ServerSigningKey;
            device.ServerAgreementKey = ciphering.ServerAgreementKey;
            device.PreEstablished = ciphering.PreEstablishedApplicationAssociations;
            device.IgnoreSNRMWithPreEstablished = ciphering.IgnoreSNRMWithPreEstablished;
            device.UseProtectedRelease = UseProtectedReleaseCb.Checked;
            device.SecurityChangeCheck = SecurityChangeCheckCb.Checked;

            //Check security settings.
            if (validate && (device.Security != Security.None ||
                ((GXAuthentication)this.AuthenticationCB.SelectedItem).Type == Authentication.HighGMAC))
            {
                if (!string.IsNullOrEmpty(device.SystemTitle) && device.SystemTitle.Length != 16)
                {
                    throw new ArgumentException("Invalid system title. System title must be 8 bytes long.");
                }
                if (!string.IsNullOrEmpty(device.AuthenticationKey) && device.AuthenticationKey.Length != 32 && device.AuthenticationKey.Length != 64)
                {
                    throw new ArgumentException("Invalid authentication key. Authentication key must be 16 or 32 bytes long.");
                }
                if (!string.IsNullOrEmpty(device.BlockCipherKey) && device.BlockCipherKey.Length != 32 && device.BlockCipherKey.Length != 64)
                {
                    throw new ArgumentException("Invalid block cipher key. Block cipher key must be 16 or 32 bytes long.");
                }
                if (!string.IsNullOrEmpty(device.DedicatedKey) && device.DedicatedKey.Length != 32 && device.DedicatedKey.Length != 64)
                {
                    throw new ArgumentException("Invalid dedicated key. Dedicated key must be 16 or 32 bytes long.");
                }
                if (!string.IsNullOrEmpty(device.ServerSystemTitle) && device.ServerSystemTitle.Length != 16)
                {
                    throw new ArgumentException("Invalid server system title. Server system title must be 8 bytes long.");
                }
            }
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
            device.Challenge = ciphering.Challenge;
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
                PasswordAsciiCb.Visible = PasswordTB.Visible = authentication.Type != Authentication.HighECDSA;
                SigningKeyCb.Visible = authentication.Type == Authentication.HighECDSA;
                if (authentication.Type == Authentication.HighECDSA)
                {
                    PasswordLbl.Text = "Signing key:";
                }
                else
                {
                    PasswordLbl.Text = "Password:";
                }
                GXPkcs8 pk = null;
                if (!string.IsNullOrEmpty(Device.ClientSigningKey))
                {
                    pk = GXPkcs8.FromDer(Device.ClientSigningKey);
                }
                SignInitiateRequestResponseCb.Enabled = IncludePublicKeyCb.Enabled = authentication.Type == Authentication.HighECDSA;
                if (authentication.Type == Authentication.HighECDSA)
                {
                    ChallengeSizeCb.SelectedItem = (byte)32;
                    SigningKeyCb.Items.Clear();
                    string st = ciphering.SystemTitle;
                    if (st == "4142434445464748")
                    {
                        st = null;
                    }
                    foreach (var it in ciphering.GetClientKeys(st))
                    {
                        int pos = SigningKeyCb.Items.Add(it);
                        if (it.Key.Equals(pk))
                        {
                            SigningKeyCb.SelectedIndex = pos;
                        }
                    }
                }
                else
                {
                    ChallengeSizeCb.SelectedItem = (byte)16;
                }
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
            string name;
            if (string.IsNullOrEmpty(authentication.Name))
            {
                name = authentication.Type.ToString();
            }
            else
            {
                name = authentication.Name;
            }
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
                    InterfaceType selected;
                    if (InterfaceCb.SelectedItem != null)
                    {
                        selected = (InterfaceType)InterfaceCb.SelectedItem;
                    }
                    else
                    {
                        selected = Device.InterfaceType;
                    }
                    InterfaceCb.Items.Clear();
                    if (man.SupporterdInterfaces != 0)
                    {
                        foreach (InterfaceType it in Enum.GetValues(typeof(InterfaceType)))
                        {
                            if ((man.SupporterdInterfaces & (1 << (int)it)) != 0)
                            {
                                InterfaceCb.Items.Add(it);
                            }
                        }
                    }
                    else
                    {
                        InterfaceCb.Items.Add(InterfaceType.HDLC);
                        InterfaceCb.Items.Add(InterfaceType.HdlcWithModeE);
                        InterfaceCb.Items.Add(InterfaceType.WRAPPER);
                    }
                    InterfaceCb.SelectedItem = selected;
                    //Select first item if interface is not available.
                    if (InterfaceCb.SelectedItem == null)
                    {
                        InterfaceCb.SelectedItem = InterfaceCb.Items[0];
                    }

                    this.ClientAddTB.Value = man.GetActiveAuthentication().ClientAddress;
                    AuthenticationCB.Items.Clear();
                    foreach (GXAuthentication it in man.Settings)
                    {
                        bool empty = string.IsNullOrEmpty(Device.AuthenticationName);
                        int pos = AuthenticationCB.Items.Add(it);
                        if ((empty && it.Type == Device.Authentication) ||
                                (!empty && it.Name == Device.AuthenticationName))
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
                    ciphering.Security = man.Security;
                    ciphering.Signing = man.Signing;

                    ciphering.SystemTitleAscii = IsAscii(man.SystemTitle);
                    if (ciphering.SystemTitleAscii)
                    {
                        ciphering.SystemTitle = ASCIIEncoding.ASCII.GetString(man.SystemTitle);
                    }
                    else
                    {
                        ciphering.SystemTitle = GXCommon.ToHex(man.SystemTitle, true);
                    }

                    ciphering.BlockCipherKeyAscii = IsAscii(man.BlockCipherKey);
                    if (ciphering.BlockCipherKeyAscii)
                    {
                        ciphering.BlockCipherKey = ASCIIEncoding.ASCII.GetString(man.BlockCipherKey);
                    }
                    else
                    {
                        ciphering.BlockCipherKey = GXCommon.ToHex(man.BlockCipherKey, true);
                    }

                    ciphering.AuthenticationKeyAscii = man.AuthenticationKey == null || IsAscii(man.AuthenticationKey);
                    if (ciphering.AuthenticationKeyAscii)
                    {
                        if (man.AuthenticationKey == null)
                        {
                            ciphering.AuthenticationKey = "";
                        }
                        else
                        {
                            ciphering.AuthenticationKey = ASCIIEncoding.ASCII.GetString(man.AuthenticationKey);
                        }
                    }
                    else
                    {
                        ciphering.AuthenticationKey = GXCommon.ToHex(man.AuthenticationKey, true);
                    }
                    ciphering.ServerSystemTitle = GXCommon.ToHex(man.ServerSystemTitle, true);
                    InvocationCounterTB.Text = "0";
                    ciphering.Challenge = "";
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
                System.Diagnostics.Process.Start("https://www.gurux.fi/GXDLMSDirectorExample");
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
            if (type == InterfaceType.Plc || type == InterfaceType.PlcHdlc)
            {
                if (DeviceTab.TabPages.Contains(PduFrame))
                {
                    DeviceTab.TabPages.Remove(PduFrame);
                }
                if (DeviceTab.TabPages.Contains(HdlcFrameTab))
                {
                    DeviceTab.TabPages.Remove(HdlcFrameTab);
                }
                if (!DeviceTab.TabPages.Contains(PlcFrame))
                {
                    DeviceTab.TabPages.Insert(2, PlcFrame);
                }
            }
            else if (type == InterfaceType.HDLC ||
                type == InterfaceType.HdlcWithModeE)
            {
                if (DeviceTab.TabPages.Contains(PduFrame))
                {
                    DeviceTab.TabPages.Remove(PduFrame);
                }
                if (DeviceTab.TabPages.Contains(PlcFrame))
                {
                    DeviceTab.TabPages.Remove(PlcFrame);
                }
                if (!DeviceTab.TabPages.Contains(HdlcFrameTab))
                {
                    DeviceTab.TabPages.Insert(2, HdlcFrameTab);
                }
                foreach (object it in this.MediasCB.Items)
                {
                    if (it is GXSerial)
                    {
                        //Initialize serial settings.
                        GXSerial serial = (GXSerial)it;
                        if (type == InterfaceType.HDLC)
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
            else if (type == InterfaceType.PDU)
            {
                if (DeviceTab.TabPages.Contains(PlcFrame))
                {
                    DeviceTab.TabPages.Remove(PlcFrame);
                }
                if (DeviceTab.TabPages.Contains(HdlcFrameTab))
                {
                    DeviceTab.TabPages.Remove(HdlcFrameTab);
                }
                if (!DeviceTab.TabPages.Contains(PduFrame))
                {
                    DeviceTab.TabPages.Insert(2, PduFrame);
                }
            }
            else
            {
                if (DeviceTab.TabPages.Contains(HdlcFrameTab))
                {
                    DeviceTab.TabPages.Remove(HdlcFrameTab);
                }
                if (DeviceTab.TabPages.Contains(PlcFrame))
                {
                    DeviceTab.TabPages.Remove(PlcFrame);
                }
            }
        }

        private void StandardCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ciphering.Standard = (Standard)StandardCb.SelectedItem;
                switch (ciphering.Standard)
                {
                    case Standard.India:
                    case Standard.Italy:
                    case Standard.SaudiArabia:
                        UseUtcTimeZone.Checked = true;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                //Ignore all exceptions.
            }
        }
        private void ShowAsHex_CheckedChanged(object sender, EventArgs e)
        {
            MACTargetAddressTb.Hexadecimal = MACSourceAddressTb.Hexadecimal = PhysicalServerAddressTB.Hexadecimal =
                LogicalServerAddressTB.Hexadecimal = ClientAddTB.Hexadecimal = ShowAsHex.Checked;
        }

        /// <summary>
        /// Generate private key and certificate for the client.
        /// </summary>
        private void PrivatekeyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GXDLMSDirector");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string certificates = Path.Combine(path, "Certificates");
                if (!Directory.Exists(certificates))
                {
                    Directory.CreateDirectory(certificates);
                }
                string keys = Path.Combine(path, "Keys");
                if (!Directory.Exists(keys))
                {
                    Directory.CreateDirectory(keys);
                }
                GXEcdsaKeysDlg dlg = new GXEcdsaKeysDlg(Properties.Settings.Default.GeneratorAddress, keys,
                    certificates,
                    Properties.Resources.GXDLMSDirectorTxt,
                    ciphering.SecuritySuite,
                    GXDLMSTranslator.HexToBytes(ciphering.SystemTitle));
                dlg.Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void SigningKeyCb_Format(object sender, ListControlConvertEventArgs e)
        {
            if (e.Value is KeyValuePair<GXPkcs8, GXx509Certificate> kp)
            {
                if (kp.Value != null)
                {
                    e.Value = kp.Value.Subject + " #" + kp.Value.SerialNumber;
                }
                else
                {
                    e.Value = kp.Key.PrivateKey.ToHex();
                }
            }
        }

        /// <summary>
        /// User selects new signing key.
        /// </summary>
        private void SigningKeyCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SigningKeyCb.SelectedItem is KeyValuePair<GXPkcs8, GXx509Certificate> kp)
            {
                string certificateSt = GXDLMSTranslator.ToHex(GXAsn1Converter.SystemTitleFromSubject(kp.Value.Subject), false);
                if (ciphering.SystemTitle != certificateSt &&
                    //ciphering.SystemTitle is not set on load.
                    Device.SystemTitle != certificateSt)
                {
                    if (MessageBox.Show(Parent, string.Format("System title '{0}' of the client is different than in the certificate '{1}'. Do you want to update the system title from the certificate?", ciphering.SystemTitle, certificateSt), "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        return;
                    }
                    ciphering.SystemTitleAscii = false;
                    ciphering.SystemTitle = certificateSt;
                }
                ciphering.ClientSigningKey = kp.Key.ToDer();
            }
            else
            {
                Device.ClientSigningKey = null;
                Device.ServerSigningKey = null;
            }
        }
    }
}
