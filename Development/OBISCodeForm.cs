//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/OBISCodeForm.cs $
//
// Version:         $Revision: 8991 $,
//                  $Date: 2016-12-02 13:54:21 +0200 (pe, 02 joulu 2016) $
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
using Gurux.DLMS;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GXDLMS.Common;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Objects;
using Gurux.DLMS.Enums;

namespace GXDLMSDirector
{
    public partial class OBISCodeForm : Form
    {
        System.Collections.Hashtable Items = new System.Collections.Hashtable();
        GXObisCodeCollection ObisCodeCollection;
        GXObisCode Target;
        GXObisCode OriginalTarget;

        public OBISCodeForm(GXObisCodeCollection collection, GXObisCode item)
        {
            InitializeComponent();
            OriginalTarget = item;
            //Create clone from original items.
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, item);
            ms.Position = 0;
            Target = (GXObisCode)bf.Deserialize(ms);
            ms.Close();
            ObisCodeCollection = collection;
            ShowInterfaces();
            if (Target.ObjectType == ObjectType.None)
            {
                InterfaceCB.SelectedIndex = 0;
            }
            else
            {
                InterfaceCB.SelectedItem = Target.ObjectType;
            }
            UpdateTarget();
        }

        void UpdateTarget()
        {
            //If logical name is not given set default value.
            if (string.IsNullOrEmpty(Target.LogicalName))
            {
                Target.LogicalName = "1.2.3.4.5";
            }
            ObisPropertyGrid.SelectedObject = Target;
            InterfaceCB.SelectedItem = Target;
        }

        /// <summary>
        /// Show all interfaces
        /// </summary>
        void ShowInterfaces()
        {
            InterfaceCB.Items.Clear();
            foreach (Type type in Gurux.DLMS.GXDLMSClient.GetObjectTypes())
            {
                GXDLMSObject obj = Activator.CreateInstance(type) as GXDLMSObject;
                InterfaceCB.Items.Add(obj.ObjectType);
            }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!GXDLMSObject.ValidateLogicalName(Target.LogicalName))
                {
                    throw new Exception("Invalid logical name.");
                }
                OriginalTarget.Description = Target.Description;
                OriginalTarget.ObjectType = Target.ObjectType;
                OriginalTarget.LogicalName = Target.LogicalName;
                OriginalTarget.Append = Target.Append;
                OriginalTarget.Attributes.Clear();
                OriginalTarget.Attributes.AddRange(Target.Attributes);
                if (ObisCodeCollection.FindByLN(OriginalTarget.ObjectType, OriginalTarget.LogicalName, OriginalTarget) != null)
                {
                    throw new Exception("OBIS code already exists.");
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
                DialogResult = DialogResult.None;
            }
        }

        void UpdateObisValueText(ListViewItem item, GXObisValueItem obj)
        {
            item.Text = obj.Value + " / " + obj.UIValue;
        }

        private void InterfaceCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bool bChange = Target.ObjectType != (ObjectType)InterfaceCB.SelectedItem;
                Target.ObjectType = (ObjectType)InterfaceCB.SelectedItem;
                if (bChange)
                {
                    Target.Attributes.Clear();
                }
                UpdateTarget();
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }
    }
}
