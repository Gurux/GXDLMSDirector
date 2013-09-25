//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GuruxClub/GXDLMSDirector/Development/Views/GXValueField.cs $
//
// Version:         $Revision: 6510 $,
//                  $Date: 2013-08-08 16:24:58 +0300 (to, 08 elo 2013) $
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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GXDLMS.ManufacturerSettings;
using GXDLMS.Common;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS;
using Gurux.DLMS.Objects;
using System.Reflection;

namespace GXDLMSDirector.Views
{
    public enum GXValueFieldType
    {
        TextBox = 1,
        CompoBox = 2,
        ListBox = 3
    }

    public class GXButton : Button
    {
        public int AttributeID
        {
            get;
            set;
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public GXDLMSObject Target
        {
            get;
            set;
        }
    }

    public partial class GXValueField : UserControl
    {
        bool m_Dirty;
        GXValueFieldType m_Type;
        List<GXObisValueItem> Items;        

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXValueField()
        {
            InitializeComponent();
            Type = GXValueFieldType.TextBox;
            comboBox1.Visible = false;
            m_Dirty = false;
        }

        public int AttributeID
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public GXDLMSObject Target
        {
            get;
            set;
        }
        
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public bool Dirty
        {
            get
            {
                return m_Dirty;
            }
            set
            {
                if (m_Dirty != value)
                {
                    m_Dirty = value;
                    SetDirty(true, this.Value);
                }
            }
        }
        
        
        public GXValueFieldType Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                SetDirty(false, null);
                m_Type = value;
                textBox1.Visible = Type == GXValueFieldType.TextBox;
                comboBox1.Visible = Type == GXValueFieldType.CompoBox;
                listBox1.Visible = Type == GXValueFieldType.ListBox;                
                switch (Type)
                {
                    case GXValueFieldType.TextBox:
                        textBox1.KeyUp += new KeyEventHandler(textBox1_KeyUp);
                        break;
                    case GXValueFieldType.CompoBox:
                        comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
                        break;
                }
            }
        }

        void textBox1_TextChanged(object sender, EventArgs e)
        {
            return;
        }

        void SetDirty(bool dirty, object value)
        {
            m_Dirty = dirty;
            if (dirty && AttributeID != 0)
            {
                Target.UpdateDirty(AttributeID, value);
            }
        }

        object GetValue(object target, int attributeID, List<object> UpdatedObjects)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(target);
            foreach (PropertyDescriptor it in properties)
            {
                GXDLMSAttributeSettings index = it.Attributes[typeof(GXDLMSAttributeSettings)] as GXDLMSAttributeSettings;
                if (index != null && index.Index == AttributeID)
                {
                    return it.GetValue(target);
                }
                else if (it.PropertyType.IsClass)
                {
                    if (it.PropertyType == typeof(string))
                    {
                        continue;
                    }
                    //If component is not already searched.
                    if (!UpdatedObjects.Contains(target))
                    {
                        UpdatedObjects.Add(target);
                        GetValue(it.GetValue(target), attributeID, UpdatedObjects);
                    }
                }
            }
            return null;
        }       

        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                object value = null;
                foreach (GXObisValueItem it in Items)
                {
                    if (it.UIValue == comboBox1.Text)
                    {
                        value = it.Value;
                        break;
                    }
                }
                SetDirty(true, value);
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        void textBox1_LostFocus(object sender, EventArgs e)
        {
            try
            {
               SetDirty(true, textBox1.Text);
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        void textBox1_KeyUp(object sender, KeyEventArgs e)
        {        
            try
            {
                if (!e.Handled)
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        SetDirty(true, textBox1.Text);
                    }
                    e.Handled = true;
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }             
        }

        delegate void UpdateValueItemsEventHandler(GXDLMSObject target, int index);

        public void UpdateValueItems(GXDLMSObject target, int index)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new UpdateValueItemsEventHandler(UpdateValueItems), target, index);
            }
            else
            {
                GXDLMSAttributeSettings tmp = GXDLMSClient.GetAttributeInfo(target, index);
                if (tmp != null)
                {
                    Items = tmp.Values;
                }
                else
                {
                    Items = null;
                }
                if (this.Type == GXValueFieldType.TextBox)
                {
                    this.Type = Items == null || Items.Count == 0 ? GXValueFieldType.TextBox : GXValueFieldType.CompoBox;
                }
                comboBox1.Items.Clear();
                if (Items != null)
                {
                    foreach (GXObisValueItem it in Items)
                    {
                        comboBox1.Items.Add(it.UIValue);
                    }
                }
                ReadOnly = (target.GetAccess(index) & Gurux.DLMS.AccessMode.Write) == 0;
            }
        }

        [Browsable(true), DefaultValue(false)]        
        public bool ReadOnly
        {
            get
            {
                if (Type == GXValueFieldType.TextBox)
                {
                    return this.textBox1.ReadOnly;
                }
                else if (Type == GXValueFieldType.CompoBox)
                {
                    return this.comboBox1.Enabled;
                }
                else if (Type == GXValueFieldType.ListBox)
                {
                    return this.listBox1.Enabled;
                }
                else if (Type == GXValueFieldType.TextBox)
                {
                    return true;
                }
                throw new InvalidExpressionException();
            }
            set
            {
                if (Type == GXValueFieldType.TextBox)
                {
                    this.textBox1.ReadOnly = value;
                }
                else if (Type == GXValueFieldType.CompoBox)
                {
                    this.comboBox1.Enabled = !value;
                }
                else if (Type == GXValueFieldType.ListBox)
                {
                    this.listBox1.Enabled = !value;
                }
                else
                {
                    throw new InvalidExpressionException();
                }
            }
        }

        delegate void UpdateValueEventHandler(object value);

        void OnUpdateValue(object value)
        {
            string str;
            if (value != null && value.GetType().IsArray)
            {
                str = Convert.ToString(GXDLMS.Common.GXHelpers.ConvertFromDLMS(value, DataType.None, DataType.None, true));
            }
            else
            {
                str = GXHelpers.ConvertDLMS2String(value);
            }
            if (Type == GXValueFieldType.TextBox)
            {
                textBox1.TextChanged -= new EventHandler(textBox1_TextChanged);
                this.textBox1.Text = str;
                textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
            }
            else if (Type == GXValueFieldType.CompoBox)
            {
                if (comboBox1.Items.Count != 0)
                {
                    if (value != null)
                    {
                        foreach (GXObisValueItem it in Items)
                        {
                            if (it.Value.Equals(Convert.ChangeType(value, it.Value.GetType())))
                            {
                                int pos = comboBox1.Items.IndexOf(it.UIValue);
                                if (pos != -1)
                                {
                                    comboBox1.SelectedIndexChanged -= new EventHandler(comboBox1_SelectedIndexChanged);
                                    comboBox1.SelectedIndex = pos;
                                    comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        comboBox1.SelectedIndexChanged -= new EventHandler(comboBox1_SelectedIndexChanged);
                        comboBox1.SelectedIndex = -1;
                        comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
                        return;
                    }
                }
                this.comboBox1.Text = str;
            }
            else if (Type == GXValueFieldType.ListBox)
            {
                this.listBox1.Items.Clear();
                if (value is Array)
                {
                    foreach (Array it in (Array)value)
                    {
                        List<byte> arr = new List<byte>();
                        foreach (object item in it)
                        {
                            if (item is Array)
                            {
                                foreach (byte b in (Array)item)
                                {
                                    arr.Add(b);
                                }
                            }
                            else
                            {
                                arr.Add((byte)item);
                            }
                        }
                        this.listBox1.Items.Add(ASCIIEncoding.ASCII.GetString((byte[])arr.ToArray()));
                    }
                }
            }
            else if (Type != GXValueFieldType.TextBox)
            {
                throw new InvalidExpressionException();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public object Value
        {
            get
            {
                if (Type == GXValueFieldType.TextBox)
                {
                    return this.textBox1.Text;
                }
                else if (Type == GXValueFieldType.CompoBox)
                {
                    return this.comboBox1.Text;
                }
                else if (Type == GXValueFieldType.ListBox)
                {
                    return this.listBox1.Text;
                }
                throw new InvalidExpressionException();
            }
            set
            {
                if (InvokeRequired)
                {
                    this.BeginInvoke(new UpdateValueEventHandler(OnUpdateValue), value);
                }
                else
                {
                    OnUpdateValue(value);
                }
            }
        }
    }
}
