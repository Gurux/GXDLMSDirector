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
using System.Xml.Serialization;
using Gurux.DLMS.Objects;

namespace GXDLMS.Common
{
    public class GXDLMSObjectSerializer<AbstractType> : IXmlSerializable
    {
        static Dictionary<Type, XmlSerializer> s = new Dictionary<Type, XmlSerializer>();
        // Override the Implicit Conversions Since the XmlSerializer 
        // Casts to/from the required types implicitly. 
        public static implicit operator AbstractType(GXDLMSObjectSerializer<AbstractType> o)
        {
            return o.Data;
        }

        public static implicit operator GXDLMSObjectSerializer<AbstractType>(AbstractType o)
        {
            return o == null ? null : new GXDLMSObjectSerializer<AbstractType>(o);
        }

        private AbstractType _data;
        /// <summary> 
        /// [Concrete] Data to be stored/is stored as XML. 
        /// </summary> 
        public AbstractType Data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary> 
        /// **DO NOT USE** This is only added to enable XML Serialization. 
        /// </summary> 
        /// <remarks>DO NOT USE THIS CONSTRUCTOR</remarks> 
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public GXDLMSObjectSerializer()
        {
            // Default Ctor (Required for Xml Serialization - DO NOT USE) 
        }

        /// <summary> 
        /// Initialises the Serializer to work with the given data. 
        /// </summary> 
        /// <param name="data">Concrete Object of the AbstractType Specified.</param> 
        public GXDLMSObjectSerializer(AbstractType data)
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

            // Ensure the Type was Specified 
            if (typeAttrib == null)
                throw new ArgumentNullException("Unable to Read Xml Data for Abstract Type '" + typeof(AbstractType).Name +
                    "' because no 'type' attribute was specified in the XML.");

            if (typeAttrib.StartsWith("GXDLMSDirector."))
            {
                typeAttrib = typeAttrib.Replace("GXDLMSDirector.", "Gurux.DLMS.Objects.");
            }

            Type type = typeof(Gurux.DLMS.GXDLMSClient).Assembly.GetType(typeAttrib);
            if (type == null)
            {
                type = Type.GetType(typeAttrib);
            }
            //Type type = Type.GetType(typeAttrib);
            // Check the Type is Found. 
            if (type == null)
                throw new InvalidCastException("Unable to Read Xml Data for Abstract Type '" + typeof(AbstractType).Name +
                    "' because the type specified in the XML was not found.");

            // Check the Type is a Subclass of the AbstractType. 
            if (!type.IsSubclassOf(typeof(AbstractType)))
                throw new InvalidCastException("Unable to Read Xml Data for Abstract Type '" + typeof(AbstractType).Name +
                    "' because the Type specified in the XML differs ('" + type.Name + "').");
            // Read the Data, Deserializing based on the (now known) concrete type. 
            reader.ReadStartElement();
            // Exception:  The assembly with display name 'Gurux.GraphView.XmlSerializers'... 
            // is a part of the XmlSerializer's normal operation.  It is expected and will be caught and 
            // handled inside of the Framework code. Just ignore it and continue. 
            // If it bothers you during debugging, set the Visual Studio debugger to only stop on 
            // unhandled exceptions instead of all exceptions.
            XmlSerializer t;
            if (!s.ContainsKey(type))
            {
                t = new XmlSerializer(type, Gurux.DLMS.GXDLMSClient.GetObjectTypes());
                s[type] = t;
            }
            else
            {
                t = s[type];
            }
            this.Data = (AbstractType)t.Deserialize(reader);
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            // Write the Type Name to the XML Element as an Attrib and Serialize 
            Type type = _data.GetType();

            // BugFix: Assembly must be FQN since Types can/are external to current. 
            writer.WriteAttributeString("xsi:type", type.FullName);

            XmlAttributeOverrides overrides = new XmlAttributeOverrides();
            XmlAttributes attribs = new XmlAttributes();
            attribs.XmlIgnore = true;
            overrides.Add(typeof(GXDLMSObject), "Description", attribs);
            XmlSerializer x = new XmlSerializer(type, overrides);
            x.Serialize(writer, _data);
        }

        #endregion
    }
}
