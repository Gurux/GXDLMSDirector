//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
//
// Version:         $Revision: 10624 $,
//                  $Date: 2019-04-24 13:56:09 +0300 (ke, 24 huhti 2019) $
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GXDLMS.ManufacturerSettings;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Enums;
using Gurux.DLMS;

namespace GXDLMSDirector
{
    public partial class OBISCodesForm : Form
    {
        System.Collections.Hashtable Items = new System.Collections.Hashtable();
        public OBISCodesForm(GXManufacturerCollection manufacturers, string selectedManufacturer, ObjectType Interface, string ln)
        {
            InitializeComponent();
            ManufacturerNameCH.Width = -2;
            NewBtn.Enabled = manufacturers.Count != 0;
            EditBtn.Enabled = RemoveBtn.Enabled = false;
            bool bSelected = false;
            //Add manufacturers
            foreach (GXManufacturer it in manufacturers)
            {
                if (!it.Removed)
                {
                    ListViewItem item = AddManufacturer(it);
                    if (it.Identification == selectedManufacturer)
                    {
                        bSelected = item.Selected = true;
                    }
                }
            }
            //Select first item
            if (!bSelected && ManufacturersList.Items.Count != 0)
            {
                ManufacturersList.Items[0].Selected = true;
            }
            //Add OBIS Codes.
            ManufacturersList_SelectedIndexChanged(null, null);
            //Select OBIS code by Logical name.
            if (ManufacturersList.SelectedItems.Count == 1)
            {
                ShowOBISCOdes(((GXManufacturer)ManufacturersList.SelectedItems[0].Tag).ObisCodes, Interface, ln);
            }
            this.ManufacturersList.SelectedIndexChanged += new System.EventHandler(this.ManufacturersList_SelectedIndexChanged);
        }

        ListViewItem AddManufacturer(GXManufacturer manufacturer)
        {
            ListViewItem it = ManufacturersList.Items.Add(manufacturer.Name);
            it.Tag = manufacturer;
            return it;
        }

        private void NewBtn_Click(object sender, EventArgs e)
        {
            try
            {
                GXObisCode item = new GXObisCode();
                GXManufacturer man = (GXManufacturer)ManufacturersList.SelectedItems[0].Tag;
                OBISCodeForm dlg = new OBISCodeForm(new GXDLMSConverter(man.Standard), null, man.ObisCodes, item);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    AddItem(item);
                    ((GXManufacturer)ManufacturersList.SelectedItems[0].Tag).ObisCodes.Add(item);
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            try
            {
                GXObisCode item = (GXObisCode)OBISCodesList.SelectedItems[0].Tag;
                GXManufacturer man = (GXManufacturer) ManufacturersList.SelectedItems[0].Tag;                
                OBISCodeForm dlg = new OBISCodeForm(new GXDLMSConverter(man.Standard), null, man.ObisCodes, item);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    OBISCodesList.SelectedItems[0].Text = item.LogicalName + " " + item.Description;
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this, GXDLMSDirector.Properties.Resources.RemoveObjectConfirmation, GXDLMSDirector.Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                {
                    return;
                }
                foreach (ListViewItem it in OBISCodesList.SelectedItems)
                {
                    GXObisCode item = (GXObisCode)it.Tag;
                    ((GXManufacturer)ManufacturersList.SelectedItems[0].Tag).ObisCodes.Remove(item);
                    Items.Remove(item);
                    it.Remove();
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Add item to OBIS list.
        /// </summary>
        /// <param name="item"></param>
        ListViewItem AddItem(GXObisCode item)
        {
            ListViewItem it = OBISCodesList.Items.Add(item.LogicalName + " " + item.Description);
            it.Tag = item;
            Items[item] = it;
            return it;
        }

        static int CompareOBISKeys(KeyValuePair<string, GXObisCode> a, KeyValuePair<string, GXObisCode> b)
        {
            if (a.Key == null || b.Key == null)
            {
                return 0;
            }
            string[] keyA = a.Key.Split('.');
            string[] keyB = b.Key.Split('.');
            if (keyA.Length != 6 || keyB.Length != 6)
            {
                return -1;
            }
            for (int pos = 0; pos != keyA.Length; ++pos)
            {
                if (keyA[pos] != keyB[pos])
                {
                    if (int.Parse(keyA[pos]) < int.Parse(keyB[pos]))
                    {
                        return -1;
                    }
                    return 1;
                }
            }
            return 0;
        }

        void ShowOBISCOdes(GXObisCodeCollection collection, ObjectType Interface, string selectedLN)
        {
            Items.Clear();
            this.OBISCodesList.Items.Clear();
            if (collection != null)
            {
                List<KeyValuePair<string, GXObisCode>> list = new List<KeyValuePair<string, GXObisCode>>();
                foreach (GXObisCode it in collection)
                {
                    if (!string.IsNullOrEmpty(it.LogicalName))
                    {
                        list.Add(new KeyValuePair<string, GXObisCode>(it.LogicalName, it));
                    }
                }
                try
                {
                    list.Sort(CompareOBISKeys);
                }
                catch
                {
                    //This fails if there is empty key. Remove key.
                }
                bool bSelected = false;
                if (collection != null)
                {
                    foreach (KeyValuePair<string, GXObisCode> it in list)
                    {
                        ListViewItem item = AddItem(it.Value);
                        if (!bSelected && Interface == it.Value.ObjectType && it.Value.LogicalName == selectedLN)
                        {
                            bSelected = item.Selected = true;
                        }
                    }
                }

                bool bEnabled = this.OBISCodesList.Items.Count != 0;
                EditBtn.Enabled = RemoveBtn.Enabled = bEnabled;
                if (!bSelected && bEnabled)
                {
                    this.OBISCodesList.Items[0].Selected = true;
                }
            }
            this.OBISCodesList.Select();
        }

        /// <summary>
        /// Show manufacturer's OBIS Codes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManufacturersList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ManufacturersList.SelectedItems.Count == 1)
            {
                ShowOBISCOdes(((GXManufacturer)ManufacturersList.SelectedItems[0].Tag).ObisCodes, 0, null);
            }
            else
            {
                ShowOBISCOdes(null, 0, null);
            }
        }

        /// <summary>
        /// Update Edit and Remove buttons when new item is selected from the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OBISCodesList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            EditBtn.Enabled = RemoveBtn.Enabled = OBISCodesList.SelectedItems.Count == 1;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
        }

        protected static ScrollBars GetVisibleScrollbars(Control ctl)
        {
            int wndStyle = Gurux.Win32.GetWindowLong(ctl.Handle, Gurux.Win32.GWL_STYLE);
            bool hsVisible = (wndStyle & Gurux.Win32.WS_HSCROLL) != 0;
            bool vsVisible = (wndStyle & Gurux.Win32.WS_VSCROLL) != 0;
            if (hsVisible)
            {
                return vsVisible ? ScrollBars.Both : ScrollBars.Horizontal;
            }
            return vsVisible ? ScrollBars.Vertical : ScrollBars.None;
        }

        /// <summary>
        /// Edit item when user double clicks item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OBISCodesList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EditBtn_Click(null, null);
        }
    }
}
