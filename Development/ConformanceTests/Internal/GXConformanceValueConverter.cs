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

namespace GXDLMSDirector
{
    public class GXConformanceValueConverter : TypeConverter
    {
        /// <summary>
        /// Allows us to display the + symbol near the property name
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
        /// <returns>true if System.ComponentModel.TypeConverter.GetProperties(System.Object) should be called to find the properties of this object; otherwise, false.</returns>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            GXConformanceValueDescriptorCollection props = new GXConformanceValueDescriptorCollection(null);
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(value, attributes);
            foreach (PropertyDescriptor pd in pdc)
            {
                props.Add(pd);
            }
            return props;
        }
    }
}
