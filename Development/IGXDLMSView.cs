//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GXDLMSDirector/Development/IGXDLMSView.cs $
//
// Version:         $Revision: 5795 $,
//                  $Date: 2012-10-02 13:22:54 +0300 (ti, 02 loka 2012) $
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
using Gurux.DLMS.Objects;
using GXDLMS.ManufacturerSettings;
using Gurux.DLMS;

namespace GXDLMSDirector
{
    public interface IGXDLMSView
    {
        GXDLMSObject Target
        {
            get;
            set;
        }
        
        /// <summary>
        /// Called after value has changed and if Attribute ID is not set to GXValueField.
        /// </summary>
        /// <param name="attributeID"></param>                
        void OnValueChanged(int attributeID, object value);

        /// <summary>
        /// Called after access rights changed and if  Attribute ID is not set to GXValueField.
        /// </summary>
        /// <param name="attributeID"></param>
        /// <param name="access"></param>
        void OnAccessRightsChange(int attributeID, AccessMode access);

        /// <summary>
        /// Called to update UI after value has change.
        /// </summary>
        /// <param name="attributeID"></param>
        /// <param name="Dirty"></param>
        void OnDirtyChange(int attributeID, bool Dirty);

        System.Windows.Forms.ErrorProvider ErrorProvider
        {
            get;
        }

        /// <summary>
        /// Update Desctiption.
        /// </summary>
        string Description
        {
            get;
            set;
        }
    }
}
