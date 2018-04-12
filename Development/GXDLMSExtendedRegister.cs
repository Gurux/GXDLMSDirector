//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: https://146.185.146.169/Projects/GuruxClub/GXDLMSDirector/Development/GXDLMSExtendedRegister.cs $
//
// Version:         $Revision: 4781 $,
//                  $Date: 2012-03-19 10:23:38 +0200 (ma, 19 maalis 2012) $
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
using System.ComponentModel;
using Gurux.DLMS;
using GXDLMS.ManufacturerSettings;
using System.Xml.Serialization;

namespace GXDLMSDirector
{
    [GXDLMSViewAttribute(typeof(Views.GXDLMSExtendedRegisterView))]
    public class GXDLMSExtendedRegister : GXDLMSRegister
    {
        /// <summary> 
        /// Constructor.
        /// </summary> 
        public GXDLMSExtendedRegister()
            : base(ObjectType.ExtendedRegister)
        {
        }

        /// <summary>
        /// Scaler of COSEM Register object.
        /// </summary>
        [XmlIgnore()]
        [ReadOnly(true)]
        [GXDLMSAttributeIndex(4)]
        public string Status
        {
            get;
            set;
        }

        /// <summary>
        /// Scaler of COSEM Register object.
        /// </summary>
        [XmlIgnore()]
        [ReadOnly(true)]
        [GXDLMSAttributeIndex(5, DataType.DateTime)]
        [TypeConverter(typeof(GXDateTimeConverter))]
        public DateTime CaptureTime
        {
            get;
            set;
        }
    }
}
