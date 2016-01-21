//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL$
//
// Version:         $Revision$,
//                  $Date$
//                  $Author$
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
using GXDLMS.Common;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Objects;
using Gurux.DLMS;

namespace GXDLMSDirector
{
    public partial class GXGraphItemForm : Form
    {
        private List<string> GetColors()
        {
            //create a generic list of strings
            List<string> colors = new List<string>();
            //get the color names from the Known color enum
            string[] colorNames = Enum.GetNames(typeof(KnownColor));
            //iterate thru each string in the colorNames array
            foreach (string colorName in colorNames)
            {
                //cast the colorName into a KnownColor
                KnownColor knownColor = (KnownColor)Enum.Parse(typeof(KnownColor), colorName);
                //check if the knownColor variable is a System color
                if (knownColor > KnownColor.Transparent)
                {
                    //add it to our list
                    colors.Add(colorName);
                }
            }
            //return the color list
            return colors;
        }
        GXGraphItemCollection Items;
        public GXGraphItemForm(GXGraphItemCollection items, List<GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>> columns, GXDLMSDevice device)
        {
            InitializeComponent();
            GraphItemEditor_SizeChanged(null, null);
            GraphItemList.OwnerDraw = true;            
            Items = items;
            int pos = 0;
            List<string> colors = GetColors();
            GXManufacturer man = device.Manufacturers.FindByIdentification(device.Manufacturer);
            foreach (var it in columns)
            {
                GXDLMSObject obj = it.Key;
                int index = it.Value.AttributeIndex;
                if (!GXHelpers.IsNumeric(obj.GetDataType(index)) || (index > 0 && ((index & 0x8) != 0 || (index & 0x10) != 0)))
                {
                    continue;
                }
                GXGraphItem item = items.Find(obj.LogicalName, index);
                if (item == null)
                {
                    item = new GXGraphItem();
                    item.LogicalName = obj.LogicalName;
                    item.Color = Color.FromName(colors[pos++]);
                    item.AttributeIndex = index;
                    items.Add(item);
                }
                string desc = obj.Description;
                GXObisCode code = man.ObisCodes.FindByLN(obj.ObjectType, obj.LogicalName, null);
                if (code != null)
                {
                    desc = code.Description;
                }
                ListViewItem tmp = GraphItemList.Items.Add(obj.LogicalName + " " + desc);
                tmp.Tag = item;
            }
        }

        private void GraphItemList_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if (GraphItemList.SelectedItems.Count != 0)
            {
                object[] items = new object[GraphItemList.SelectedItems.Count];
                for(int pos = 0; pos != GraphItemList.SelectedItems.Count; ++pos)
                {
                    items[pos] = GraphItemList.SelectedItems[pos].Tag;
                }
                GraphItemEditor.SelectedObjects = items;
            }
            else
            {
                GraphItemEditor.SelectedObject = null;
            }
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

        private void GraphItemEditor_SizeChanged(object sender, EventArgs e)
        {
            int w = GraphItemList.Width - 6;
            if ((GetVisibleScrollbars(GraphItemList) & ScrollBars.Horizontal) != 0)
            {
                w -= SystemInformation.VerticalScrollBarWidth;
            }
            ManufacturerNameCH.Width = w;
        }       
        private void GraphItemList_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            GXGraphItem item = GraphItemList.Items[e.ItemIndex].Tag as GXGraphItem;
            if (item.Enabled)
            {
                // Draw the background for an unselected item.
                using (SolidBrush brush = new SolidBrush(item.Color))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds.Left + 5, e.Bounds.Top + 1, 15, 15);
                }                
            }
            else
            {
                e.Graphics.DrawImage(GraphItemList.SmallImageList.Images[0], e.Bounds.Left + 5, e.Bounds.Top + 1, 15, 15);
            }
            e.DrawDefault = true;
        }

        private void GraphItemEditor_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            try
            {
            if (GraphItemList.SelectedItems.Count != 0 && e.ChangedItem.Label == "Enabled")
            {
                int start = GraphItemList.SelectedItems[0].Index;
                int end = GraphItemList.SelectedItems[GraphItemList.SelectedItems.Count -1].Index;
                GraphItemList.RedrawItems(start, end, false);
            }
            }
            catch
            {
                return;
            }
        }
    }
}
