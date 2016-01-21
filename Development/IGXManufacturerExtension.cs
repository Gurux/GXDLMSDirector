//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL$
//
// Version:         $Revision$,
//                  $Date$
//                  $Author$
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
// This code is licensed under the GNU General Public License v2. 
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Objects;
using Gurux.DLMS;

namespace GXDLMSDirector
{
    /// <summary>
    /// With this interface it is possible to implement manufacturer spesific transactions.
    /// </summary>
    public interface IGXManufacturerExtension
    {
        /// <summary>
        /// Handles custom itema after read.
        /// </summary>
        bool Read(object sender, GXDLMSObject item, GXDLMSObjectCollection columns, int attribute, GXDLMSCommunicator comm);

        List<GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>> Refresh(GXDLMSProfileGeneric item, GXDLMSCommunicator comm);
    }
}
