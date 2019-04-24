//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
//
// Version:         $Revision: 10624 $,
//                  $Date: 2019-04-24 13:56:09 +0300 (Wed, 24 Apr 2019) $
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
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Enums;
using Gurux.Common;
using Gurux.DLMS;

namespace GXDLMSDirector
{
    public partial class ManufacturerForm : Form
    {
        GXManufacturerCollection Manufacturers;
        GXManufacturer Manufacturer;

        void RefreshServer(GXServerAddress server)
        {
            SerialNumberFormulaTB.Text = server.Formula;
            PhysicalServerAddTB.Value = Convert.ToDecimal(server.PhysicalAddress);
            LogicalServerAddTB.Value = server.LogicalAddress;
            PhysicalServerAddTB.Hexadecimal = SerialNumberFormulaTB.ReadOnly = server.HDLCAddress != HDLCAddressType.SerialNumber;
            SizeCb.Items.Add(0);
            SizeCb.Items.Add(1);
            SizeCb.Items.Add(2);
            SizeCb.Items.Add(4);
            SizeCb.SelectedItem = server.Size;
        }

        void UpdateServer(GXServerAddress server)
        {
            server.Formula = SerialNumberFormulaTB.Text;
            server.PhysicalAddress = Convert.ToInt32(PhysicalServerAddTB.Value);
            server.LogicalAddress = Convert.ToInt32(LogicalServerAddTB.Value);
            server.Size = Convert.ToInt32(SizeCb.Text);
        }

        public ManufacturerForm(GXManufacturerCollection manufacturers, GXManufacturer manufacturer)
        {
            InitializeComponent();
            foreach (object it in Enum.GetValues(typeof(Standard)))
            {
                StandardCb.Items.Add(it);
            }
            StandardCb.SelectedItem = manufacturer.Standard;

            SecurityCB.Items.AddRange(new object[] { Security.None, Security.Authentication,
                                      Security.Encryption, Security.AuthenticationEncryption
                                                   });
            Manufacturers = manufacturers;
            Manufacturer = manufacturer;
            if (manufacturer.Settings.Count == 0)
            {
                manufacturer.Settings.Add(new GXAuthentication(Authentication.None, (byte)0x10));
                manufacturer.Settings.Add(new GXAuthentication(Authentication.Low, (byte)0x11));
                manufacturer.Settings.Add(new GXAuthentication(Authentication.High, (byte)0x12));
                manufacturer.Settings.Add(new GXAuthentication(Authentication.HighMD5, (byte)0x13));
                manufacturer.Settings.Add(new GXAuthentication(Authentication.HighSHA1, (byte)0x14));
                manufacturer.Settings.Add(new GXAuthentication(Authentication.HighGMAC, (byte)0x14));
                manufacturer.Settings.Add(new GXAuthentication(Authentication.HighSHA256, (byte)0x14));
                manufacturer.Settings.Add(new GXAuthentication(Authentication.HighECDSA, (byte)0x14));
                GXAuthentication gmac = new GXAuthentication(Authentication.HighGMAC, (byte)0x15);
            }
            GXAuthentication authentication = manufacturer.GetActiveAuthentication();
            foreach (GXAuthentication it in manufacturer.Settings)
            {
                AuthenticationCB.Items.Add(it);
            }
            AuthenticationCB.SelectedItem = authentication;
            this.AuthenticationCB.SelectedIndexChanged += new System.EventHandler(this.AuthenticationCB_SelectedIndexChanged);
            if (manufacturer.ServerSettings.Count == 0)
            {
                manufacturer.ServerSettings.Add(new GXServerAddress(HDLCAddressType.Default, (byte)1, true));
                manufacturer.ServerSettings.Add(new GXServerAddress(HDLCAddressType.SerialNumber, (byte)1, false));
                manufacturer.ServerSettings.Add(new GXServerAddress(HDLCAddressType.Custom, (byte)1, false));
            }
            foreach (GXServerAddress it in manufacturer.ServerSettings)
            {
                ServerAddressTypeCB.Items.Add(it);
            }

            GXServerAddress server = manufacturer.GetActiveServer();
            ServerAddressTypeCB.SelectedItem = server;
            RefreshServer(server);
            this.ServerAddressTypeCB.SelectedIndexChanged += new System.EventHandler(this.ServerAddressTypeCB_SelectedIndexChanged);

            ServerAddressTypeCB.DrawMode = AuthenticationCB.DrawMode = DrawMode.OwnerDrawFixed;
            ClientAddTB.Value = authentication.ClientAddress;

            StartProtocolCB.Items.Add(StartProtocolType.IEC);
            StartProtocolCB.Items.Add(StartProtocolType.DLMS);
            NameTB.Text = manufacturer.Name;
            ManufacturerIdTB.Text = manufacturer.Identification;
            UseLNCB.Checked = manufacturer.UseLogicalNameReferencing;
            UseIEC47CB.Checked = manufacturer.UseIEC47;
            StartProtocolCB.SelectedItem = manufacturer.StartProtocol;
            //Manufacturer ID can not change after creation.
            ManufacturerIdTB.Enabled = string.IsNullOrEmpty(manufacturer.Identification);
            WebAddressTB.Text = Manufacturer.WebAddress;
            if (!string.IsNullOrEmpty(Manufacturer.Info))
            {
                try
                {
                    InfoTB.Text = ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(Manufacturer.Info));
                }
                catch (Exception)
                {
                    InfoTB.Text = "";
                }
            }
            SecurityCB.SelectedItem = manufacturer.Security;
            if (DevicePropertiesForm.IsAscii(manufacturer.SystemTitle))
            {
                SystemTitleAsciiCb.Checked = true;
                SystemTitleTB.Text = ASCIIEncoding.ASCII.GetString((manufacturer.SystemTitle));
            }
            else
            {
                SystemTitleTB.Text = GXCommon.ToHex(manufacturer.SystemTitle, true);
            }
            if (DevicePropertiesForm.IsAscii(manufacturer.BlockCipherKey))
            {
                BlockCipherKeyAsciiCb.Checked = true;
                BlockCipherKeyTB.Text = ASCIIEncoding.ASCII.GetString(manufacturer.BlockCipherKey);
            }
            else
            {
                BlockCipherKeyTB.Text = GXCommon.ToHex(manufacturer.BlockCipherKey, true);
            }
            if (DevicePropertiesForm.IsAscii(manufacturer.AuthenticationKey))
            {
                AuthenticationKeyAsciiCb.Checked = true;
                AuthenticationKeyTB.Text = ASCIIEncoding.ASCII.GetString(manufacturer.AuthenticationKey);
            }
            else
            {
                AuthenticationKeyTB.Text = GXCommon.ToHex(manufacturer.AuthenticationKey, true);
            }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (NameTB.Text.Length == 0)
                {
                    throw new Exception("Invalid manufacturer name.");
                }
                if (ManufacturerIdTB.Text.Length != 3)
                {
                    throw new Exception("Invalid manufacturer identification.");
                }
                if (ManufacturerIdTB.Enabled && Manufacturers.FindByIdentification(ManufacturerIdTB.Text) != null)
                {
                    throw new Exception("Manufacturer identification already exists.");
                }
                if (!SerialNumberFormulaTB.ReadOnly && SerialNumberFormulaTB.Text.Length == 0)
                {
                    throw new Exception("Invalid Serial Number.");
                }
                Manufacturer.Name = NameTB.Text;
                Manufacturer.Identification = ManufacturerIdTB.Text;
                Manufacturer.UseLogicalNameReferencing = UseLNCB.Checked;
                Manufacturer.UseIEC47 = UseIEC47CB.Checked;
                Manufacturer.StartProtocol = (StartProtocolType)StartProtocolCB.SelectedItem;
                Manufacturer.Standard = (Standard)StandardCb.SelectedItem;


                GXAuthentication authentication = Manufacturer.GetActiveAuthentication();
                authentication.ClientAddress = Convert.ToInt32(this.ClientAddTB.Value);
                //Save server values.
                UpdateServer((GXServerAddress)ServerAddressTypeCB.SelectedItem);
                Manufacturer.WebAddress = WebAddressTB.Text;
                if (!string.IsNullOrEmpty(InfoTB.Text))
                {
                    Manufacturer.Info = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes(InfoTB.Text));
                }
                else
                {
                    Manufacturer.Info = null;
                }
                Manufacturer.Security = (Security)SecurityCB.SelectedItem;
                Manufacturer.SystemTitle = GXCommon.HexToBytes(DevicePropertiesForm.GetAsHex(SystemTitleTB.Text, SystemTitleAsciiCb.Checked));
                Manufacturer.BlockCipherKey = GXCommon.HexToBytes(DevicePropertiesForm.GetAsHex(BlockCipherKeyTB.Text, BlockCipherKeyAsciiCb.Checked));
                Manufacturer.AuthenticationKey = GXCommon.HexToBytes(DevicePropertiesForm.GetAsHex(AuthenticationKeyTB.Text, AuthenticationKeyAsciiCb.Checked));
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
                this.DialogResult = DialogResult.None;
            }
        }

        /// <summary>
        /// Draw device ID types.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Address_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox Sender = (ComboBox)sender;
            // If the index is invalid then simply exit.
            if (e.Index == -1 || e.Index >= Sender.Items.Count)
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
            Type target = (Type)Sender.Items[e.Index];
            if (target == null)
            {
                return;
            }
            string name = target.Name;
            SizeF s = e.Graphics.MeasureString(name, f);
            e.Graphics.DrawString(name, f, b, e.Bounds);
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

        private void AuthenticationCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GXAuthentication authentication = Manufacturer.GetActiveAuthentication();
                authentication.Selected = false;
                //Save old values.
                authentication.ClientAddress = Convert.ToInt32(this.ClientAddTB.Value);
                authentication = ((GXAuthentication)AuthenticationCB.SelectedItem);
                authentication.Selected = true;
                ClientAddTB.Value = authentication.ClientAddress;
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
            try
            {
                GXServerAddress server = Manufacturer.GetActiveServer();
                server.Selected = false;
                //Save old values.
                UpdateServer(server);

                server = ((GXServerAddress)ServerAddressTypeCB.SelectedItem);
                server.Selected = true;
                RefreshServer(server);
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }
    }
}
