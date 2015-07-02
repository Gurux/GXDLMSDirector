//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/GXDLMSModemConfiguration.cs $
//
// Version:         $Revision: 5551 $,
//                  $Date: 2012-07-31 13:18:30 +0300 (ti, 31 heinä 2012) $
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
using System.Text;
using Gurux.DLMS;
using System.ComponentModel;
using System.Xml.Serialization;
using GXDLMS.ManufacturerSettings;
using System.Drawing.Design;
using GXDLMS.Common;
using Gurux.DLMS.ManufacturerSettings;

namespace GXDLMSDirector
{
    [GXDLMSViewAttribute(typeof(Views.GXDLMSModemConfigurationView))]
    public class GXDLMSModemConfiguration : GXDLMSBase
    {
        /// <summary> 
        /// Constructor.
        /// </summary> 
        public GXDLMSModemConfiguration()
            : base(ObjectType.ModemConfiguration)
        {
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="values"></param>
        public override void UpdateDefaultValueItems(GXAttributeCollection attributes)
        {
            GXAttribute att = new GXAttribute();
            att.Index = 2;
            att.Values.Add(new GXObisValueItem(0, "300"));
            att.Values.Add(new GXObisValueItem(1, "600"));
            att.Values.Add(new GXObisValueItem(2, "1200"));
            att.Values.Add(new GXObisValueItem(3, "2400"));
            att.Values.Add(new GXObisValueItem(4, "4800"));
            att.Values.Add(new GXObisValueItem(5, "9600"));
            att.Values.Add(new GXObisValueItem(6, "19200"));
            att.Values.Add(new GXObisValueItem(7, "38400"));
            att.Values.Add(new GXObisValueItem(8, "57600"));
            att.Values.Add(new GXObisValueItem(9, "115200"));
            attributes.Add(att);
        }

        [XmlIgnore()]
        [DefaultValue(0)]
        [ReadOnly(false)]
        [GXDLMSAttributeIndex(2)]
        [TypeConverter(typeof(GXOBISValueItemConverter))]
        [Editor(typeof(GXOBISValueItemEditor), typeof(UITypeEditor))]
        public int CommunicationSpeed
        {
            get;
            set;
        }

        [XmlIgnore()]
        [ReadOnly(false)]
        [GXDLMSAttributeIndex(3, DataType.Array)]
        public object InitialisationStrings
        {
            get;
            set;
        }

        [XmlIgnore()]
        [ReadOnly(false)]
        [GXDLMSAttributeIndex(4, DataType.Array)]
        public object ModemProfile
        {
            get;
            set;
        }
    }
}
