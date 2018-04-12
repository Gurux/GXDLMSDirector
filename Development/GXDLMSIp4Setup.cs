//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: https://146.185.146.169/Projects/GuruxClub/GXDLMSDirector/Development/GXDLMSIp4Setup.cs $
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
using GXDLMS.Common;
using System.Drawing.Design;

namespace GXDLMSDirector
{
    [GXDLMSViewAttribute(typeof(Views.GXDLMSIp4SetupView))]
    public class GXDLMSIp4Setup : GXDLMSBase
    {
        /// <summary> 
        /// Constructor.
        /// </summary> 
        public GXDLMSIp4Setup()
            : base(ObjectType.Ip4Setup)
        {
        }

        [XmlIgnore()]
        [ReadOnly(false)]
        [GXDLMSAttributeIndex(2, DataType.OctetString)]
        public string DataLinkLayerReference
        {
            get;
            set;
        }

        [XmlIgnore()]
        [ReadOnly(false)]
        [GXDLMSAttributeIndex(3, DataType.String)]
        public UInt64 IPAddress
        {
            get;
            set;
        }

        [XmlIgnore()]
        [ReadOnly(false)]
        [GXDLMSAttributeIndex(4)]
        public object MulticastIPAddress
        {
            get;
            set;
        }

        [XmlIgnore()]
        [ReadOnly(false)]
        [GXDLMSAttributeIndex(5)]
        public object IPOptions
        {
            get;
            set;
        }

        [XmlIgnore()]        
        [ReadOnly(false)]
        [GXDLMSAttributeIndex(6, DataType.String)]
        public UInt64 SubnetMask
        {
            get;
            set;
        }

        [XmlIgnore()]
        [ReadOnly(false)]
        [GXDLMSAttributeIndex(7)]
        public UInt64 GatewayIPAddress
        {
            get;
            set;
        }

        [XmlIgnore()]        
        [ReadOnly(false)]
        [GXDLMSAttributeIndex(8)]        
        public string UseDHCP
        {
            get;
            set;
        }

        [XmlIgnore()]
        [ReadOnly(false)]
        [GXDLMSAttributeIndex(9)]
        public UInt64 PrimaryDNSAddress
        {
            get;
            set;
        }

        [XmlIgnore()]
        [ReadOnly(false)]
        [GXDLMSAttributeIndex(10)]        
        public UInt64 SecondaryDNSAddress
        {
            get;
            set;
        }

    }
}
