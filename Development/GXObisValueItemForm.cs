//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/GXObisValueItemForm.cs $
//
// Version:         $Revision: 8937 $,
//                  $Date: 2016-11-23 14:03:11 +0200 (ke, 23 marras 2016) $
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
using GXDLMS.ManufacturerSettings;
using Gurux.DLMS.ManufacturerSettings;

namespace GXDLMSDirector
{
public partial class GXObisValueItemForm : Form
{
    List<GXObisValueItem> Items;
    GXObisValueItem Target;
    public GXObisValueItemForm(List<GXObisValueItem> items, GXObisValueItem item)
    {
        InitializeComponent();
        Items = items;
        Target = item;
        UIValueTB.Text = Target.UIValue;
        if (Target.Value != null)
        {
            DeviceValueTB.Text = Target.Value.ToString();
        }
    }

    private void OKBtn_Click(object sender, EventArgs e)
    {
        try
        {
            if (UIValueTB.Text.Trim().Length == 0 ||
                    DeviceValueTB.Text.Trim().Length == 0)
            {
                throw new Exception("Invalid value.");
            }
            if (Items != null)
            {
                foreach (GXObisValueItem it in Items)
                {
                    if (it != Target && (it.Value.Equals(DeviceValueTB.Text) || it.UIValue == UIValueTB.Text))
                    {
                        throw new Exception("Invalid value. Value already exists.");
                    }
                }
            }
            Target.UIValue = UIValueTB.Text;
            Target.Value = DeviceValueTB.Text;
        }
        catch (Exception Ex)
        {
            GXDLMS.Common.Error.ShowError(this, Ex);
            this.DialogResult = DialogResult.None;
        }
    }
}
}
