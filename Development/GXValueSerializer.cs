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


using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace GXDLMSDirector
{
    public class GXValueSerializer : IXmlSerializable
    {
        // Override the Implicit Conversions Since the XmlSerializer 
        // Casts to/from the required types implicitly. 
        public static implicit operator string(GXValueSerializer o)
        {
            return o.Data.ToString();
        }

        public static implicit operator GXValueSerializer(string o)
        {
            return o == null ? null : new GXValueSerializer(o);
        }

        private object _data;
        /// <summary> 
        /// [Concrete] Data to be stored/is stored as XML. 
        /// </summary> 
        public object Data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary> 
        /// **DO NOT USE** This is only added to enable XML Serialization. 
        /// </summary> 
        /// <remarks>DO NOT USE THIS CONSTRUCTOR</remarks> 
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public GXValueSerializer()
        {
            // Default Ctor (Required for Xml Serialization - DO NOT USE) 
        }

        /// <summary> 
        /// Initialises the Serializer to work with the given data. 
        /// </summary> 
        /// <param name="data">Concrete Object of the AbstractType Specified.</param> 
        public GXValueSerializer(string data)
        {
            _data = data;
        }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null; // this is fine as schema is unknown. 
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            // Cast the Data back from the Abstract Type. 
            string typeAttrib = reader.GetAttribute("xsi:type");
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            // Write the Type Name to the XML Element as an Attrib and Serialize 
            Type type = _data.GetType();

            // BugFix: Assembly must be FQN since Types can/are external to current. 
            writer.WriteAttributeString("xsi:type", type.FullName);
            new XmlSerializer(type).Serialize(writer, _data);
        }

        #endregion
    }
}
