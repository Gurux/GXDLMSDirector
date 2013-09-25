//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GuruxClub/GXDLMSDirector/Development/ManufacturerForm.cs $
//
// Version:         $Revision: 5618 $,
//                  $Date: 2012-08-24 09:15:04 +0300 (pe, 24 elo 2012) $
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GXDLMS.ManufacturerSettings;
using Gurux.DLMS.ManufacturerSettings;

namespace GXDLMSDirector
{
    public partial class ManufacturerForm : Form
    {                        
        void RefreshServer(GXServerAddress server)
        {
            SerialNumberFormulaTB.Text = server.Formula;
            if (server.PhysicalAddress != null)
            {
                ServerAddTypeCB.SelectedItem = server.PhysicalAddress.GetType();
            }
            PhysicalServerAddTB.Value = Convert.ToDecimal(server.PhysicalAddress);
            LogicalServerAddTB.Value = server.LogicalAddress;
            PhysicalServerAddTB.Hexadecimal = SerialNumberFormulaTB.ReadOnly = server.HDLCAddress != HDLCAddressType.SerialNumber;            
        }

        void UpdateServer(GXServerAddress server)
        {
            server.Formula = SerialNumberFormulaTB.Text;
            server.PhysicalAddress = Convert.ChangeType(PhysicalServerAddTB.Value, (Type)ServerAddTypeCB.SelectedItem);
            server.LogicalAddress = Convert.ToInt32(LogicalServerAddTB.Value);
        }

        void RefreshAuthentication(GXAuthentication authentication)
        {
            if (authentication.ClientID != null)
            {
                ClientAddTypeCB.SelectedItem = authentication.ClientID.GetType();
                ClientAddTB.Value = Convert.ToDecimal(authentication.ClientID);
            }
            else
            {
                ClientAddTypeCB.SelectedItem = typeof(byte);
            }
        }

        void UpdateAuthentication(GXAuthentication authentication)
        {
            object value = Convert.ChangeType(this.ClientAddTB.Value, (Type)this.ClientAddTypeCB.SelectedItem);         
            authentication.ClientID = value;
        }

        GXManufacturerCollection Manufacturers;
        GXManufacturer Manufacturer;
        public ManufacturerForm(GXManufacturerCollection manufacturers, GXManufacturer manufacturer)
        {
            InitializeComponent();
            Manufacturers = manufacturers;
            Manufacturer = manufacturer;
            if (manufacturer.Settings.Count == 0)
            {
                manufacturer.Settings.Add(new GXAuthentication(Gurux.DLMS.Authentication.None, (byte)0x10));
                manufacturer.Settings.Add(new GXAuthentication(Gurux.DLMS.Authentication.Low, (byte)0x11));
                manufacturer.Settings.Add(new GXAuthentication(Gurux.DLMS.Authentication.High, (byte)0x12));
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
                manufacturer.ServerSettings.Add(new GXServerAddress(HDLCAddressType.Default, (byte) 1, true));
                manufacturer.ServerSettings.Add(new GXServerAddress(HDLCAddressType.SerialNumber, (byte) 1, false));
                manufacturer.ServerSettings.Add(new GXServerAddress(HDLCAddressType.Custom, (byte)1, false));
            }            
            foreach (GXServerAddress it in manufacturer.ServerSettings)
            {
                ServerAddressTypeCB.Items.Add(it);
            }

            ServerAddTypeCB.Items.Add(typeof(byte));
            ServerAddTypeCB.Items.Add(typeof(ushort));
            ServerAddTypeCB.Items.Add(typeof(uint));
            GXServerAddress server = manufacturer.GetActiveServer();
            ServerAddressTypeCB.SelectedItem = server;
            RefreshServer(server);
            this.ServerAddressTypeCB.SelectedIndexChanged += new System.EventHandler(this.ServerAddressTypeCB_SelectedIndexChanged);
            
            ServerAddressTypeCB.DrawMode = AuthenticationCB.DrawMode = ClientAddTypeCB.DrawMode = ServerAddTypeCB.DrawMode = DrawMode.OwnerDrawFixed;
            ClientAddTypeCB.Items.Add(typeof(byte));
            ClientAddTypeCB.Items.Add(typeof(ushort));
            ClientAddTypeCB.Items.Add(typeof(uint));
            if (authentication.ClientID != null)
            {
                ClientAddTB.Value = Convert.ToDecimal(authentication.ClientID);
                ClientAddTypeCB.SelectedItem = authentication.ClientID.GetType();
            }
            RefreshAuthentication(authentication);            

            InactivityModeCB.Items.Add(InactivityMode.None);
            InactivityModeCB.Items.Add(InactivityMode.KeepAlive);
            InactivityModeCB.Items.Add(InactivityMode.Reopen);
            InactivityModeCB.Items.Add(InactivityMode.ReopenActive);
            InactivityModeCB.Items.Add(InactivityMode.Disconnect);
            StartProtocolCB.Items.Add(StartProtocolType.IEC);
            StartProtocolCB.Items.Add(StartProtocolType.DLMS);
            NameTB.Text = manufacturer.Name;
            ManufacturerIdTB.Text = manufacturer.Identification;
            ForceKeepAliveCB.Checked = manufacturer.ForceInactivity;
            UseLNCB.Checked = manufacturer.UseLogicalNameReferencing;
            UseIEC47CB.Checked = manufacturer.UseIEC47;
            StartProtocolCB.SelectedItem = manufacturer.StartProtocol;
            InactivityModeCB.SelectedItem = Manufacturer.InactivityMode;
            //Manufacturer ID can not change after creation.
            ManufacturerIdTB.Enabled = string.IsNullOrEmpty(manufacturer.Identification);
            KeepAliveIntervalTB.Value = Manufacturer.KeepAliveInterval / 1000;
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
                Manufacturer.InactivityMode = (InactivityMode)InactivityModeCB.SelectedItem;
                Manufacturer.ForceInactivity = ForceKeepAliveCB.Checked;
                GXAuthentication authentication = Manufacturer.GetActiveAuthentication();                
                Manufacturer.KeepAliveInterval = Convert.ToInt32(KeepAliveIntervalTB.Value) * 1000;
                UpdateAuthentication(authentication);
                //Save server values.
                UpdateServer((GXServerAddress) ServerAddressTypeCB.SelectedItem);
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
                UpdateAuthentication(authentication);                
                authentication = ((GXAuthentication)AuthenticationCB.SelectedItem);
                authentication.Selected = true;
                this.RefreshAuthentication(authentication);                
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void ClientAddTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ClientAddTypeCB.SelectedItem == typeof(byte))
            {
                ClientAddTB.Maximum = 0xFF;
            }
            else if (ClientAddTypeCB.SelectedItem == typeof(ushort))
            {
                ClientAddTB.Maximum = 0xFFFF;
            }
            else if (ClientAddTypeCB.SelectedItem == typeof(uint))
            {
                ClientAddTB.Maximum = 0xFFFFFFFF;
            }
        }

        private void ServerAddTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            uint value = 0;
            if (ServerAddTypeCB.SelectedItem == typeof(byte))
            {
                value = 0xFF;
            }
            else if (ServerAddTypeCB.SelectedItem == typeof(ushort))
            {
                value = 0xFFFF;
            }
            else if (ServerAddTypeCB.SelectedItem == typeof(uint))
            {
                value = 0xFFFFFFFF;
            }
            PhysicalServerAddTB.Maximum = value;
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
