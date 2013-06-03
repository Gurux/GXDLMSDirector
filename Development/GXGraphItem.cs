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
using System.Text;
using System.ComponentModel;

namespace GXDLMSDirector
{
    public class GXGraphItem
    {
        public GXGraphItem()
        {
            Enabled = true;
        }

        [System.Xml.Serialization.XmlIgnore()]
        public System.Drawing.Color Color
        {
            get;
            set;
        }

        /// <summary>
        /// Fix color serialization bug.
        /// </summary>
        [Browsable(false)]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public int Argb
        {
            get
            {
                return Color.ToArgb();
            }
            set
            {
                if (value == 0)
                {
                    Color = System.Drawing.Color.Empty;
                }
                else
                {
                    Color = System.Drawing.Color.FromArgb(value);
                }
            }
        }

        [DefaultValue(true)]
        public bool Enabled
        {
            get;
            set;
        }
        
        [Browsable(false)]
        public string LogicalName
        {
            get;
            set;
        }

        [Browsable(false)]
        public int AttributeIndex
        {
            get;
            set;
        }
    }
}
