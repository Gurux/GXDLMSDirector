//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
//
// Version:         $Revision: 9442 $,
//                  $Date: 2017-05-23 15:21:03 +0300 (ti, 23 touko 2017) $
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
using System.Windows.Forms;

namespace GXDLMSDirector
{
    /// <summary>
    /// Conformance tests.
    /// </summary>
    partial class GXConformanceDlg : Form
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXConformanceDlg(GXConformanceSettings settings)
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = settings;
        }

        /// <summary>
        /// Accept changes.
        /// </summary>
        private void OKBtn_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        /// <summary>
        /// Reset to default value.
        /// </summary>
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            try
            {
                propertyGrid1.ResetSelectedProperty();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        /// <summary>
        /// Reset all values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(propertyGrid1.SelectedObject);
                foreach (PropertyDescriptor pg in props)
                {
                    if (pg.IsBrowsable)
                    {
                        AttributeCollection attributes = pg.Attributes;
                        DefaultValueAttribute myAttribute = (DefaultValueAttribute)attributes[typeof(DefaultValueAttribute)];
                        pg.SetValue(propertyGrid1.SelectedObject, Convert.ChangeType(myAttribute.Value, pg.PropertyType));
                    }
                }
                propertyGrid1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }
    }
}
