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

using Gurux.DLMS;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace GXDLMSDirector
{
    /// <summary>
    /// Use this converter to use selected names with numbers (Zero = 0).
    /// </summary>	
    public class GXByteConverter : StringConverter
    {
        /// <summary>
        /// In-place editing is not supported.
        /// </summary>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type t)
        {
            return false;           
        }

        /// <summary>
        /// Checks if this converter can convert the object to given type.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="destinationType">The type to convert to.</param>
        /// <returns>True, if the converter can convert the object to given type.</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Checks if the user can type in a value that is not in the drop-down list.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <returns>False.</returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        /// <summary>
        /// Converts given value to the type of this converter.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="info">The culture info.</param>
        /// <param name="value">Object to convert.</param>
        /// <returns>Converted type.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo info, object value)
        {
            try
            {              
                return base.ConvertFrom(context, info, value);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Converts given object to given type.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="culture">Culture info.</param>
        /// <param name="value">The object to convert.</param>
        /// <param name="destType">The type to convert to.</param>
        /// <returns>Converted type of the object.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            try
            {
                if (value is byte[])
                {
                    if (GXByteBuffer.IsAsciiString((byte[]) value))
                    {
                        return ASCIIEncoding.ASCII.GetString((byte[]) value);
                    }
                    else
                    {
                        return Gurux.Common.GXCommon.ToHex((byte[])value);
                    }
                }
                return base.ConvertTo(context, culture, value, destType);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            return null;
        }
    }

}
