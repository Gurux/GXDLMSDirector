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
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace GXDLMSDirector.Macro
{
    public class GXUserActionSettings
    {
        public GXMacro[] Actions;
    }

    public delegate void ExecuteActionEventHandler(GXUserActionSettings settings);

    public delegate void ConnectEventHandler(GXMacro act);
    public delegate void DisconnectEventHandler(GXMacro act);
    public delegate void GetEventHandler(GXMacro act);
    public delegate void SetEventHandler(GXMacro act);


    /// <summary>
    /// Macro.
    /// </summary>
    public class GXMacro
    {
        /// <summary>
        /// Clone macro object.
        /// </summary>
        /// <returns></returns>
        public GXMacro Clone()
        {
            GXMacro target = new GXMacro();
            target.Timestamp = Timestamp;
            target.Disable = Disable;
            target.Verify = Verify;
            target.Type = Type;
            target.Name = Name;
            target.Device = Device;
            target.ObjectType = ObjectType;
            target.ObjectVersion = ObjectVersion;
            target.LogicalName = LogicalName;
            target.Index = Index;
            target.Value = Value;
            target.Data = Data;
            target.Exception = Exception;
            target.DataType = DataType;
            target.UIDataType = UIDataType;
            target.Parameters = Parameters;
            target.External = External;
            return target;
        }

        /// <summary>
        /// Record or excution time.
        /// </summary>
        public DateTime Timestamp
        {
            get;
            set;
        }


        /// <summary>
        /// Is this macro disabled.
        /// </summary>
        [DefaultValue(false)]
        public bool Disable
        {
            get;
            set;
        }

        /// <summary>
        /// Compares the read value with the stored value.
        /// </summary>
        [DefaultValue(false)]
        public bool Verify
        {
            get;
            set;
        }


        /// <summary>
        /// Executed user action type.
        /// </summary>
        public UserActionType Type
        {
            get;
            set;
        }

        /// <summary>
        /// Optional data.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Optional macro description.
        /// </summary>
        public string Description
        {
            get;
            set;
        }        

        /// <summary>
        /// Device name.
        /// </summary>
        public string Device
        {
            get;
            set;
        }


        /// <summary>
        /// Object type.
        /// </summary>
        [DefaultValue(0)]
        public int ObjectType
        {
            get;
            set;
        }

        /// <summary>
        /// Object version.
        /// </summary>
        [DefaultValue(0)]
        public byte ObjectVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Logical name of the target.
        /// </summary>
        public string LogicalName
        {
            get;
            set;
        }

        /// <summary>
        /// Attribute or method index.
        /// </summary>
        [DefaultValue(0)]
        public int Index
        {
            get;
            set;
        }

        /// <summary>
        /// Optional data.
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// Optional data.
        /// </summary>
        public string Data
        {
            get;
            set;
        }

        /// <summary>
        /// Optional parameter(s) data.
        /// </summary>
        /// <remarks>
        /// Ex. Profile Generics start and end time are saved here.
        /// </remarks>
        [DefaultValue(null)]
        public string Parameters
        {
            get;
            set;
        }

        /// <summary>
        /// Optional External data.
        /// </summary>
        /// <remarks>
        /// Ex. capture objects are serialized here.
        /// </remarks>
        [DefaultValue(null)]
        public string External
        {
            get;
            set;
        }

        /// <summary>
        /// Exception message if any.
        /// </summary>
        [DefaultValue(null)]
        public string Exception
        {
            get;
            set;
        }

        /// <summary>
        /// Data type.
        /// </summary>
        [DefaultValue(0)]
        public int DataType
        {
            get;
            set;
        }

        /// <summary>
        /// UI Data type.
        /// </summary>
        [DefaultValue(0)]
        public int UIDataType
        {
            get;
            set;
        }

        /// <summary>
        /// Is macro on run.
        /// </summary>
        [DefaultValue(false)]
        [XmlIgnore()]
        public bool Running
        {
            get;
            set;
        }

        /// <summary>
        /// Last run macro.
        /// </summary>
        [DefaultValue(null)]
        [XmlIgnore()]
        public GXMacro LastRunMacro
        {
            get;
            set;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Running)
            {
                sb.Append("Running ");
            }
            sb.Append(Type);
            sb.Append(" ");
            sb.Append(ObjectType);
            sb.Append(" ");
            sb.Append(LogicalName);
            return sb.ToString();
        }
    }
}
