//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
//
// Version:         $Revision: 9806 $,
//                  $Date: 2018-01-12 11:44:00 +0200 (pe, 12 tammi 2018) $
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
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace GXDLMSDirector
{
    internal class GXConformanceEditor : UITypeEditor
    {
        System.Windows.Forms.Design.IWindowsFormsEditorService m_EdSvc = null;
        Button button;
        object Target;

        /// <summary>
        /// Shows a dropdown icon in the property editor
        /// </summary>
        /// <remarks>
        /// Search attributes and shpw dropdown list if there are more than one attribute to shown.
        /// </remarks>
        /// <param name="context">The context of the editing control</param>
        /// <returns>Returns <c>UITypeEditorEditStyle.DropDown</c></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// Overrides the method used to provide basic behaviour for selecting editor.
        /// Shows our custom control for editing the value.
        /// </summary>
        /// <param name="context">The context of the editing control</param>
        /// <param name="provider">A valid service provider</param>
        /// <param name="value">The current value of the object to edit</param>
        /// <returns>The new value of the object</returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || context.Instance == null || provider == null)
            {
                return base.EditValue(context, provider, value);
            }
            if ((m_EdSvc = (System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService))) == null)
            {
                return value;
            }
            Target = value;
            // Create a CheckedListBox and populate it with all the propertylist values
            button = new Button();
            button.Text = "Disable All Tests";
            button.Click += new System.EventHandler(OnDisable);
            m_EdSvc.DropDownControl(button);
            return value;
        }


        /// <summary>
        /// Disable or enable all tests.
        /// </summary>
        public static void SetAll(object target, bool value)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(target);
            foreach (PropertyDescriptor it in props)
            {
                object val = it.GetValue(target);
                if (val != null)
                {
                    if (val is IGXConformanceSettings)
                    {
                        SetAll(val, value);
                    }
                    else if (val.Equals(false))
                    {
                        it.SetValue(target, value);
                    }
                }
            }
        }

        private void OnDisable(object sender, EventArgs e)
        {
            if (Target is IGXConformanceSettings)
            {
                SetAll(Target, true);
            }
            if (m_EdSvc != null)
            {
                m_EdSvc.CloseDropDown();
            }
        }


        /// <summary>
        /// Is any of the tests enabled.
        /// </summary>
        public static bool IsEnabled(object target)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(target);
            foreach (PropertyDescriptor it in props)
            {
                object val = it.GetValue(target);
                if (val != null)
                {
                    if (val.Equals(false))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
