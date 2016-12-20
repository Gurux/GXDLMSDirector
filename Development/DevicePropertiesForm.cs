//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/DevicePropertiesForm.cs $
//
// Version:         $Revision: 9048 $,
//                  $Date: 2016-12-20 16:35:34 +0200 (ti, 20 joulu 2016) $
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
        public DevicePropertiesForm(GXManufacturerCollection manufacturers, GXDLMSDevice dev)
        {
            try
            {
                InitializeComponent();
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
                //Show supported services tab only when they are read.
                if (dev == null || dev.Comm.client.SNSettings == null && dev.Comm.client.LNSettings == null)
                {
                    DeviceTab.TabPages.Remove(SupportedServicesTab);
                }
                else
                {
                    object settings = null;
                    if (dev.Comm.client.UseLogicalNameReferencing)
                    {
                        settings = dev.Comm.client.LNSettings;
                    }
                    else
                    {
                        settings = dev.Comm.client.SNSettings;
                    }
                    if (settings != null)
                    {
                        SupportedServicesGrid.SelectedObject = settings;
                        foreach (PropertyDescriptor it in TypeDescriptor.GetProperties(settings))
                        {
                            ReadOnlyAttribute att = (ReadOnlyAttribute)it.Attributes[typeof(ReadOnlyAttribute)];
                            if (att != null)
                            {
                                FieldInfo[] f = att.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                                f[0].SetValue(att, true);
                            }
                        }
                    }
                }
                Device = dev;
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
                    this.VerboseModeCB.Checked = dev.Verbose;
                    this.NameTB.Text = dev.Name;
                    SelectedMedia = dev.Media;
                    UseRemoteSerialCB.Checked = Device.UseRemoteSerial;
                    StartProtocolCB.SelectedItem = Device.StartProtocol;
                    PhysicalServerAddressTB.Value = Convert.ToDecimal(Device.PhysicalAddress);
                    LogicalServerAddressTB.Value = Convert.ToDecimal(Device.LogicalAddress);
                    this.ClientAddTB.Value = Convert.ToDecimal(Convert.ToUInt32(Device.ClientAddress));
                    WaitTimeTB.Value = Device.WaitTime;
                    SecurityCB.SelectedItem = dev.Security;
                    SystemTitleTB.Text = dev.SystemTitle;
                    BlockCipherKeyTB.Text = dev.BlockCipherKey;
                    AuthenticationKeyTB.Text = dev.AuthenticationKey;
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
                    this.PasswordTB.Text = ASCIIEncoding.ASCII.GetString(CryptHelper.Decrypt(Device.Password, Password.Key));
                }
                if (dev != null)
                {
                    this.UseLNCB.Checked = dev.UseLogicalNameReferencing;
                }
                this.AuthenticationCB.SelectedIndexChanged += new System.EventHandler(this.AuthenticationCB_SelectedIndexChanged);
                bool bConnected = Device.Media != null && Device.Media.IsOpen;
                SerialPortCB.Enabled = AdvancedBtn.Enabled = ManufacturerCB.Enabled = MediasCB.Enabled =
                                           AuthenticationCB.Enabled = UseRemoteSerialCB.Enabled = OKBtn.Enabled = !bConnected;
                HostNameTB.ReadOnly = PortTB.ReadOnly = PasswordTB.ReadOnly = WaitTimeTB.ReadOnly = PhysicalServerAddressTB.ReadOnly = NameTB.ReadOnly = bConnected;
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
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
                GXServerAddress server = (GXServerAddress)ServerAddressTypeCB.SelectedItem;
                if (server.HDLCAddress == HDLCAddressType.SerialNumber && PhysicalServerAddressTB.Value == 0)
                {
                    throw new Exception("Invalid Serial Number.");
                }
                GXManufacturer man = (GXManufacturer)ManufacturerCB.SelectedItem;
                Device.Authentication = ((GXAuthentication)this.AuthenticationCB.SelectedItem).Type;
                if (Device.Authentication != Authentication.None)
                {
                    Device.Password = CryptHelper.Encrypt(this.PasswordTB.Text, Password.Key);
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
                Device.SystemTitle = SystemTitleTB.Text;
                Device.BlockCipherKey = BlockCipherKeyTB.Text;
                Device.AuthenticationKey = AuthenticationKeyTB.Text;
            }
            catch (Exception Ex)
            {
                this.DialogResult = DialogResult.None;
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
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
                PasswordTB.Enabled = authentication.Type != Authentication.None;
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
                if (man.SystemTitle != null || man.BlockCipherKey != null ||
                        man.AuthenticationKey != null)
                {
                    SystemTitleTB.Text = GXCommon.ToHex(man.SystemTitle, true);
                    BlockCipherKeyTB.Text = GXCommon.ToHex(man.BlockCipherKey, true);
                    AuthenticationKeyTB.Text = GXCommon.ToHex(man.AuthenticationKey, true);
                    if (!DeviceTab.TabPages.Contains(CipheringTab))
                    {
                        DeviceTab.TabPages.Add(CipheringTab);
                        if (DeviceTab.TabPages.Contains(SupportedServicesTab))
                        {
                            DeviceTab.TabPages.Remove(SupportedServicesTab);
                            DeviceTab.TabPages.Add(SupportedServicesTab);
                        }
                    }
                    if (!IsPrintable(man.SystemTitle) ||
                            !IsPrintable(man.BlockCipherKey) ||
                            !IsPrintable(man.AuthenticationKey))
                    {
                        AsciiRB.Enabled = false;
                    }
                    else
                    {
                        AsciiRB.Enabled = true;
                    }
                }
                else if (DeviceTab.TabPages.Contains(CipheringTab))
                {
                    DeviceTab.TabPages.Remove(CipheringTab);
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

        private void HexRB_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (HexRB.Checked)
                {
                    SystemTitleTB.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(SystemTitleTB.Text), true);
                    BlockCipherKeyTB.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(BlockCipherKeyTB.Text), true);
                    AuthenticationKeyTB.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(AuthenticationKeyTB.Text), true);
                }
                else
                {
                    SystemTitleTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(SystemTitleTB.Text));
                    BlockCipherKeyTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(BlockCipherKeyTB.Text));
                    AuthenticationKeyTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(AuthenticationKeyTB.Text));
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void ValidateHex(object sender, KeyPressEventArgs e)
        {
            if (HexRB.Checked)
            {
                if (!Char.IsNumber(e.KeyChar) &&
                        (Char.IsLetter(e.KeyChar) && !(e.KeyChar >= 'a' && e.KeyChar <= 'f' ||
                                                       e.KeyChar >= 'A' && e.KeyChar <= 'F')))
                {
                    e.Handled = true;
                }
                if (AsciiRB.Enabled && !Char.IsLetterOrDigit(e.KeyChar))
                {
                    AsciiRB.Enabled = false;
                }
            }
        }
    }
}
