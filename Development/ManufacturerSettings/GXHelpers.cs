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
using Gurux.DLMS;
using GXDLMS.ManufacturerSettings;

namespace GXDLMS.Common
{
    class GXHelpers
    {
        static public object ConvertFromDLMS(object data, DataType from, DataType type, bool arrayAsString)
        {
            if (type == DataType.Array)
            {
                return data;
            }
            if (type == DataType.None)
            {
                if (arrayAsString && data != null && data.GetType().IsArray)
                {
                    data = GXHelpers.GetArrayAsString(data);
                }
                return data;
            }
            //Show Octet string...
            if (from == DataType.OctetString && type == DataType.OctetString)
            {
                if (data is byte[])
                {
                    string str = "";
                    byte[] arr = (byte[])data;
                    if (arr.Length == 0)
                    {
                        data = string.Empty;
                    }
                    else
                    {
                        foreach (byte it in arr)
                        {
                            str += it.ToString() + ".";
                        }
                        data = str.Substring(0, str.Length - 1);
                    }
                }
            }
            //Convert DLMS octect string to Windows string.
            else if (from == DataType.OctetString && type == DataType.String)
            {
                if (data is string)
                {
                    return data;
                }
                else if (data is byte[])
                {
                    byte[] arr = (byte[])data;
                    data = System.Text.Encoding.ASCII.GetString(arr);
                }
            }
            //Convert DLMS datetime to Windows Time.
            else if (type == DataType.DateTime)
            {
                if (data is byte[])
                {
                    return GXDLMSClient.ChangeType((byte[])data, DataType.DateTime);
                }
                return data;
            }
            //Convert DLMS datetime to Windows Date.
            else if (type == DataType.Date)
            {                
                if (data is DateTime)
                {
                    return data;
                }
                if (data is string)
                {
                    return data;
                }
                if (!data.GetType().IsArray || ((Array)data).Length < 5)
                {
                    throw new Exception("DateTime conversion failed. Invalid DLMS format.");
                }                
                return GXDLMSClient.ChangeType((byte[])data, DataType.Date);
            }
            else if (data is byte[])
            {
                if (type == DataType.String)
                {
                    data = System.Text.Encoding.ASCII.GetString((byte[])data);
                }
                else
                {
                    data = ToHexString(data);
                }
            }
            else if (data is Array)
            {
                data = ArrayToString(data);
            }
            return data;
        }

        /// <summary>
        /// Converts string to byte[].
        /// format: AB ba 01 1
        /// </summary>
        /// <param name="hexString">Hex string to convert.</param>
        /// <returns>Byte array.</returns>
        public static byte[] StringToByteArray(string hexString)
        {
            //if hex string is octect string.
            bool isOctetString = hexString.Contains(".");
            if (string.IsNullOrEmpty(hexString))
            {
                return null;
            }
            string[] splitted = hexString.Split(isOctetString ? '.': ' ');
            byte[] retVal = new byte[splitted.Length];
            int i = -1;
            foreach (string hexStr in splitted)
            {
                retVal[++i] = Convert.ToByte(hexStr, isOctetString ? 10 : 16);
            }
            return retVal;
        }

        /// <summary>
        /// Converts data to hex string.
        /// </summary>
        /// <param name="data">Data to convert.</param>
        /// <returns>Hex string.</returns>
        public static string ToHexString(object data)
        {
            string hex = string.Empty;
            if (data is Array)
            {
                Array arr = (Array)data;
                for (long pos = 0; pos != arr.Length; ++pos)
                {
                    long val = Convert.ToInt64(arr.GetValue(pos));
                    hex += Convert.ToString(val, 16) + " ";
                }
                return hex.TrimEnd();
            }
            hex = Convert.ToString(Convert.ToInt64(data), 16);
            return hex;
        }

        static string ArrayToString(object data)
        {
            string str = "";
            if (data is Array)
            {
                Array arr = (Array)data;
                for (long pos = 0; pos != arr.Length; ++pos)
                {
                    object tmp = arr.GetValue(pos);
                    if (tmp is Array)
                    {
                        str += "{ " + ArrayToString(tmp) + " }";
                    }
                    else
                    {
                        str += "{ " + Convert.ToString(tmp) + " }";
                    }
                }
            }
            return str;
        }

        static public string GetArrayAsString(object data)
        {
            Array arr = (Array)data;
            string str = null;
            foreach (object it in arr)
            {
                if (str == null)
                {
                    str = "{";
                }
                else
                {
                    str += ", ";
                }
                if (it != null && it.GetType().IsArray)
                {
                    str += GetArrayAsString(it);
                }
                else
                {
                    str += Convert.ToString(it);
                }
            }
            if (str == null)
            {
                str = "";
            }
            else
            {
                str += "}";
            }
            return str;
        }

        static public string ConvertDLMS2String(object data)
        {
            if (data is DateTime)
            {
                DateTime dt = (DateTime)data;
                if (dt == DateTime.MinValue)
                {
                    return "";
                }
                return dt.ToString();
            }
            if (data is byte[])
            {
                return BitConverter.ToString((byte[])data).Replace("-", " ");
            }
            return Convert.ToString(data);
        }

        static public Type FromDLMSDataType(DataType type)
        {
            switch (type)
            {
                case DataType.CompactArray:
                case DataType.Array:
                    return typeof(byte[]);
                case DataType.Boolean:
                    return typeof(bool);
                case DataType.Date:                    
                case DataType.DateTime:
                case DataType.Time:
                    return typeof(DateTime);
                case DataType.Float32:
                    return typeof(float);
                case DataType.Float64:
                    return typeof(double);
                case DataType.Int16:
                    return typeof(Int16);
                case DataType.Int32:
                    return typeof(Int32);
                case DataType.Int64:
                    return typeof(Int64);
                case DataType.Int8:
                    return typeof(Int16);
                case DataType.String:
                    return typeof(string);
                case DataType.UInt16:
                    return typeof(UInt16);
                case DataType.UInt32:
                    return typeof(UInt32);
                case DataType.UInt64:
                    return typeof(UInt64);
                case DataType.UInt8:
                    return typeof(byte);
                case DataType.None:                    
                case DataType.BinaryCodedDesimal:
                case DataType.BitString:
                case DataType.Enum:
                case DataType.OctetString:
                case DataType.Structure:
                default:
                    return null;
            }
        }

        static public bool IsNumeric(DataType type)
        {
            switch (type)
            {
                case DataType.Float32:
                case DataType.Float64:
                case DataType.Int16:
                case DataType.Int32:
                case DataType.Int64:
                case DataType.Int8:
                case DataType.String:
                case DataType.UInt16:
                case DataType.UInt32:
                case DataType.UInt64:
                case DataType.UInt8:
                    return true;
            }
            return false;
        }

        static public DataType GetDLMSDataType(Type type)
        {
            //If expected type is not given return property type.
            if (type == typeof(Int32))
            {
                return DataType.Int32;
            }
            if (type == typeof(UInt32))
            {
                return DataType.UInt32;
            }
            if (type == typeof(String))
            {
                return DataType.String;
            }
            if (type == typeof(byte))
            {
                return DataType.UInt8;
            }
            if (type == typeof(sbyte))
            {
                return DataType.Int8;
            }
            if (type == typeof(Int16))
            {
                return DataType.Int16;
            }
            if (type == typeof(UInt16))
            {
                return DataType.UInt16;
            }
            if (type == typeof(Int64))
            {
                return DataType.Int64;
            }
            if (type == typeof(UInt64))
            {
                return DataType.UInt64;
            }
            if (type == typeof(float))
            {
                return DataType.Float32;
            }
            if (type == typeof(double))
            {
                return DataType.Float64;
            }
            if (type == typeof(DateTime))
            {
                return DataType.DateTime;
            }
            if (type == typeof(Boolean) || type == typeof(bool))
            {
                return DataType.Boolean;
            }
            throw new Exception("Failed to convert data type to DLMS data type. Unknown data type.");
        }
    }
}
