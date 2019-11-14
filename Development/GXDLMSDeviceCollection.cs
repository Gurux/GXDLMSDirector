//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
//
// Version:         $Revision: 10624 $,
//                  $Date: 2019-04-24 13:56:09 +0300 (ke, 24 huhti 2019) $
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
using System.Collections.Generic;

namespace GXDLMSDirector
{
    /// <summary>
    /// List of devices.
    /// </summary>
    [Serializable]
    public class GXDLMSDeviceCollection : List<GXDLMSDevice>
    {
    }

    /// <summary>
    /// List of meters.
    /// </summary>
    [Serializable]
    public class GXDLMSMeterCollection : List<GXDLMSMeter>
    {
    }
}
