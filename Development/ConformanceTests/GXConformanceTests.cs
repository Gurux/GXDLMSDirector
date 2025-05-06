//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
//
// Version:         $Revision: 9442 $,
//                  $Date: 2017-05-23 15:21:03 +0300 (ti, 23 touko 2017) $
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

using Gurux.Common;
using Gurux.DLMS;
using Gurux.DLMS.Conformance.Test;
using Gurux.DLMS.Enums;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Objects;
using Gurux.DLMS.Objects.Enums;
using Gurux.DLMS.UI;
using GXDLMS.Common;
using GXDLMSDirector.Macro;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace GXDLMSDirector
{
    /// <summary>
    /// This class implements Gurux Conformance tests.
    /// </summary>
    public class GXConformanceTests
    {
        /// <summary>
        /// Only one trace can write for a log at the time.
        /// </summary>
        static readonly object traceLock = new object();

        /// <summary>
        /// Continue conformance tests.
        /// </summary>
        public static bool Continue = true;

        /// <summary>
        /// Lock tests so they are read only one thread.
        /// </summary>
        private static object ConformanceLock = new object();

        /// <summary>
        /// Get basic tests for COSEM objects.
        /// </summary>
        /// <returns>COSEM object tests.</returns>
        private static string[] GetBasicTests()
        {
            return typeof(GXConformanceDlg).Assembly.GetManifestResourceNames()
                .Where(r => r.StartsWith("GXDLMSDirector.ConformanceTests") && r.EndsWith(".xml"))
                .ToArray();
        }

        /// <summary>
        /// Get external tests.
        /// </summary>
        /// <returns>External tests.</returns>
        private static string[] GetExternalTests(GXConformanceSettings settings)
        {
            if (string.IsNullOrEmpty(settings.ExternalTests))
            {
                return new string[0];
            }
            //If single file.
            if (File.Exists(settings.ExternalTests))
            {
                return new string[] { settings.ExternalTests };
            }
            List<string> files = new List<string>();
            files.AddRange(Directory.GetFiles(settings.ExternalTests, "*.xml"));
            files.AddRange(Directory.GetFiles(settings.ExternalTests, "*.gxm"));
            return files.ToArray();
        }

        /// <summary>
        /// Load xml test and validate them.
        /// </summary>
        public static void ValidateTests(GXConformanceSettings settings)
        {
            //Load basic tests.
            if (!settings.ExcludeBasicTests)
            {
                List<string> tests = new List<string>(GetBasicTests());
                foreach (string it in tests)
                {
                    try
                    {
                        using (Stream stream = typeof(Program).Assembly.GetManifestResourceStream(it))
                        {
                            GetTests(it, stream, null, null);
                            stream.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Failed to load basic test " + it + ".", ex);
                    }
                }
            }
            //Load external tests.
            GXDLMSXmlClient client = new GXDLMSXmlClient(TranslatorOutputType.SimpleXml, true);
            string[] list = GetExternalTests(settings);
            foreach (string it in list)
            {
                try
                {
                    if (it.EndsWith("gxm"))
                    {
                        using (XmlReader reader = XmlReader.Create(it))
                        {
                            XmlSerializer x = new XmlSerializer(typeof(GXMacro[]));
                            GXMacro[] macros = (GXMacro[])x.Deserialize(reader);
                            reader.Close();
                        }
                    }
                    else
                    {
                        client.Load(it);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to load external test " + it + "." + Environment.NewLine + ex.ToString());
                }
            }
        }

        /// <summary>
        /// Get logical name as byte array.
        /// </summary>
        /// <param name="value">LN as string.</param>
        /// <returns>LN as byte array.</returns>
        static byte[] LogicalNameToBytes(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new byte[6];
            }
            string[] items = value.Split('.');
            // If data is string.
            if (items.Length != 6)
            {
                throw new ArgumentException("Invalid Logical Name");
            }
            byte[] buff = new byte[6];
            byte pos = 0;
            foreach (string it in items)
            {
                buff[pos] = Convert.ToByte(it);
                ++pos;
            }
            return buff;
        }

        /// <summary>
        /// Convert hex LN to dotted LN.
        /// </summary>
        /// <param name="ln"></param>
        /// <returns></returns>
        private static string GetLogicalName(string ln)
        {
            byte[] buff = GXCommon.HexToBytes(ln);
            return (buff[0] & 0xFF) + "." + (buff[1] & 0xFF) + "." + (buff[2] & 0xFF) + "." +
                   (buff[3] & 0xFF) + "." + (buff[4] & 0xFF) + "." + (buff[5] & 0xFF);
        }

        private static void Execute(
            GXDLMSConverter converter,
            GXConformanceTest test,
            GXMacro[] macros,
            GXOutput output,
            GXConformanceSettings settings)
        {
            GXDLMSObject obj;
            foreach (GXMacro macro in macros)
            {
                List<KeyValuePair<ObjectType, string>> succeeded = new List<KeyValuePair<ObjectType, string>>();
                if (settings.Delay.TotalSeconds != 0)
                {
                    Thread.Sleep((int)settings.Delay.TotalMilliseconds);
                }
                switch (macro.Type)
                {
                    case UserActionType.None:
                        throw new Exception("Invalid macro type.");
                    case UserActionType.Delay:
                        Thread.Sleep(int.Parse(macro.Value));
                        break;
                    case UserActionType.Connect:
                        break;
                    case UserActionType.Disconnecting:
                        break;
                    case UserActionType.Get:
                    case UserActionType.Set:
                    case UserActionType.Action:
                        obj = GXDLMSClient.CreateObject((ObjectType)macro.ObjectType, macro.ObjectVersion);
                        if (obj != null)
                        {
                            GXReplyData reply = new GXReplyData();
                            obj.LogicalName = macro.LogicalName;
                            obj.SetAccess(macro.Index, AccessMode.ReadWrite);
                            obj.SetMethodAccess(macro.Index, MethodAccessMode.Access);
                            if (obj is GXDLMSProfileGeneric && macro.Index == 2 && macro.External != null)
                            {
                                //Update capture objects.
                                test.Device.Comm.client.UpdateValue(obj, 3, GXDLMSTranslator.XmlToValue(macro.External));
                            }
                            if (macro.Type == UserActionType.Get)
                            {
                                test.Device.Comm.ReadDataBlock(test.Device.Comm.Read(obj, macro.Index, macro.Parameters), null, 1, reply);
                                object value = reply.Value;
                                test.Device.Comm.client.UpdateValue(obj, macro.Index, value);
                                macro.DataType = (int)obj.GetDataType(macro.Index);
                                macro.UIDataType = (int)obj.GetUIDataType(macro.Index);
                                string expected = macro.Data;
                                macro.Data = GXDLMSTranslator.ValueToXml(value);
                                if (macro.Data != null)
                                {
                                    macro.Data = macro.Data.Replace("\r\n", "\n");
                                }
                                string str = null;
                                if (macro.Data != null)
                                {
                                    str = macro.Data.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br/>");
                                }
                                if (macro.Verify)
                                {
                                    if (expected != null)
                                    {
                                        expected = expected.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br/>");
                                    }
                                    if (expected != str)
                                    {
                                        AddError(test, null, output.Errors, " <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + (ObjectType)macro.ObjectType + ">" + (ObjectType)macro.ObjectType + "</a> " + macro.LogicalName + " Get " + macro.Index + " is <div class=\"tooltip\">invalid.");
                                        AddError(test, null, output.Errors, "<span class=\"tooltiptext\">");
                                        AddError(test, null, output.Errors, "Expected:</b><br/>");
                                        AddError(test, null, output.Errors, expected);
                                        AddError(test, null, output.Errors, "<br/><b>Actual:</b><br/>");
                                        AddError(test, null, output.Errors, str);
                                        AddError(test, null, output.Errors, "</span></div>");
                                    }
                                    else
                                    {
                                        succeeded.Add(new KeyValuePair<ObjectType, string>((ObjectType)macro.ObjectType, macro.Name + "<br/>Equals:<br/>" + expected));
                                    }
                                }
                                else
                                {
                                    succeeded.Add(new KeyValuePair<ObjectType, string>((ObjectType)macro.ObjectType, macro.Name + "<br/>Actual:<br/>" + str));
                                }
                            }
                            else if (macro.Type == UserActionType.Set)
                            {
                                obj.SetDataType(macro.Index, (DataType)macro.DataType);
                                obj.SetUIDataType(macro.Index, (DataType)macro.UIDataType);
                                ValueEventArgs e = new ValueEventArgs(obj, macro.Index, 0, null);
                                e.Value = GXDLMSTranslator.XmlToValue(macro.Data);
                                (obj as IGXDLMSBase).SetValue(test.Device.Comm.client.Settings, e);
                                try
                                {
                                    test.Device.Comm.Write(obj, macro.Index);
                                    if (!string.IsNullOrEmpty(macro.Exception))
                                    {
                                        //If exception is expected.
                                        AddError(test, null, output.Errors, " <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + obj.ObjectType + ">" + obj.ObjectType + "</a> " + obj.LogicalName + " " + " Set Attribute " + " " + macro.Index + " <div class=\"tooltip\">failed.");
                                        AddError(test, null, output.Errors, "<span class=\"tooltiptext\">");
                                        AddError(test, null, output.Errors, "Expected exception:</b><br/>");
                                        AddError(test, null, output.Errors, macro.Exception.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br/>"));
                                        AddError(test, null, output.Errors, "<br/><b>Actual:</b><br/>");
                                        AddError(test, null, output.Errors, "Set succeeded.");
                                        AddError(test, null, output.Errors, "</span></div>");
                                    }
                                    else
                                    {
                                        succeeded.Add(new KeyValuePair<ObjectType, string>((ObjectType)macro.ObjectType, macro.Name + "Set succceded."));
                                    }
                                }
                                catch (GXDLMSException ex)
                                {
                                    if (string.IsNullOrEmpty(macro.Exception))
                                    {
                                        //If exception is not expected.
                                        if (ex.ErrorCode != 0)
                                        {
                                            ErrorCode e2 = (ErrorCode)ex.ErrorCode;
                                            AddError(test, null, output.Errors, "<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + obj.ObjectType + ">" + obj.ObjectType + "</a> " + obj.LogicalName + " Set Attribute " + macro.Index + " failed: <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.ErrorCodes?" + e2 + ">" + e2 + "</a>");
                                            test.OnTrace(test, e2 + "\r\n");
                                        }
                                        else
                                        {
                                            AddError(test, null, output.Errors, "<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + obj.ObjectType + ">" + obj.ObjectType + "</a> " + obj.LogicalName + " Set Attribute " + macro.Index + " <div class=\"tooltip\">failed:" + ex.Message);
                                            AddError(test, null, output.Errors, "<span class=\"tooltiptext\">");
                                            AddError(test, null, output.Errors, ex.ToString());
                                            AddError(test, null, output.Errors, "</span></div>");
                                            test.OnTrace(test, ex.Message + "\r\n");
                                        }
                                    }
                                    else
                                    {
                                        succeeded.Add(new KeyValuePair<ObjectType, string>((ObjectType)macro.ObjectType, macro.Name + " Set succceded."));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (string.IsNullOrEmpty(macro.Exception))
                                    {
                                        AddError(test, null, output.Errors, "<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + obj.ObjectType + ">" + obj.ObjectType + "</a> " + obj.LogicalName + " Attribute " + macro.Index + " <div class=\"tooltip\">failed:" + ex.Message);
                                        AddError(test, null, output.Errors, "<span class=\"tooltiptext\">");
                                        AddError(test, null, output.Errors, ex.ToString());
                                        AddError(test, null, output.Errors, "</span></div>");
                                        test.OnTrace(test, ex.Message + "\r\n");
                                    }
                                    else
                                    {
                                        succeeded.Add(new KeyValuePair<ObjectType, string>((ObjectType)macro.ObjectType, macro.Name + " Set succceded."));
                                    }
                                }
                            }
                            else if (macro.Type == UserActionType.Action)
                            {
                                try
                                {
                                    GXReplyData reply2 = new GXReplyData();
                                    test.Device.Comm.MethodRequest(obj, macro.Index, GXDLMSTranslator.XmlToValue(macro.Data), "", reply2);
                                    if (!string.IsNullOrEmpty(macro.Exception))
                                    {
                                        //If exception is expected.
                                        AddError(test, null, output.Errors, " <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + obj.ObjectType + ">" + obj.ObjectType + "</a> " + obj.LogicalName + " " + " invoke Attribute " + " " + macro.Index + " <div class=\"tooltip\">failed.");
                                        AddError(test, null, output.Errors, "<span class=\"tooltiptext\">");
                                        AddError(test, null, output.Errors, "Expected exception:</b><br/>");
                                        AddError(test, null, output.Errors, macro.Exception.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br/>"));
                                        AddError(test, null, output.Errors, "<br/><b>Actual:</b><br/>");
                                        AddError(test, null, output.Errors, "Invoke succeeded.");
                                        AddError(test, null, output.Errors, "</span></div>");
                                    }
                                    else
                                    {
                                        succeeded.Add(new KeyValuePair<ObjectType, string>((ObjectType)macro.ObjectType, macro.Name + " Invoke succceded."));
                                    }
                                }
                                catch (GXDLMSException ex)
                                {
                                    if (string.IsNullOrEmpty(macro.Exception))
                                    {
                                        //If exception is not expected.
                                        if (ex.ErrorCode != 0)
                                        {
                                            ErrorCode e2 = (ErrorCode)ex.ErrorCode;
                                            AddError(test, null, output.Errors, "<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + obj.ObjectType + ">" + obj.ObjectType + "</a> " + obj.LogicalName + " invoke Attribute " + macro.Index + " failed: <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.ErrorCodes?" + e2 + ">" + e2 + "</a>");
                                            test.OnTrace(test, e2 + "\r\n");
                                        }
                                        else
                                        {
                                            AddError(test, null, output.Errors, "<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + obj.ObjectType + ">" + obj.ObjectType + "</a> " + obj.LogicalName + " invoke Attribute " + macro.Index + " <div class=\"tooltip\">failed:" + ex.Message);
                                            AddError(test, null, output.Errors, "<span class=\"tooltiptext\">");
                                            AddError(test, null, output.Errors, ex.ToString());
                                            AddError(test, null, output.Errors, "</span></div>");
                                            test.OnTrace(test, ex.Message + "\r\n");
                                        }
                                    }
                                    else
                                    {
                                        succeeded.Add(new KeyValuePair<ObjectType, string>((ObjectType)macro.ObjectType, macro.Name + " Invoke succceded."));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (string.IsNullOrEmpty(macro.Exception))
                                    {
                                        AddError(test, null, output.Errors, "<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + obj.ObjectType + ">" + obj.ObjectType + "</a> " + obj.LogicalName + " Attribute " + macro.Index + " <div class=\"tooltip\">failed:" + ex.Message);
                                        AddError(test, null, output.Errors, "<span class=\"tooltiptext\">");
                                        AddError(test, null, output.Errors, ex.ToString());
                                        AddError(test, null, output.Errors, "</span></div>");
                                        test.OnTrace(test, ex.Message + "\r\n");
                                    }
                                }
                            }
                        }
                        break;
                }
                if (succeeded.Count != 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div class=\"tooltip\">" + macro.LogicalName);
                    sb.Append("<span class=\"tooltiptext\">");
                    foreach (var it in succeeded)
                    {
                        sb.Append(it.Value + "<br/>");
                    }
                    sb.Append("</span></div>");
                    sb.Append("&nbsp;" + converter.GetDescription(macro.LogicalName, (ObjectType)macro.ObjectType)[0] + "&nbsp;" + "<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + (ObjectType)macro.ObjectType + ">" + (ObjectType)macro.ObjectType + "</a>" + "&nbsp;" + " Index:" + macro.Index + ".");
                    AddInfo(test, null, output.Info, sb.ToString());
                }
            }
        }

        private static void Execute(
            GXDLMSConverter converter,
            GXConformanceTest test,
            object target,
            List<GXDLMSXmlPdu> actions,
            GXOutput output,
            GXConformanceSettings settings,
            bool external)
        {
            GXReplyData reply = new GXReplyData();
            string ln = null;
            int index = 0;
            ObjectType ot = ObjectType.None;
            List<KeyValuePair<ObjectType, string>> succeeded = new List<KeyValuePair<ObjectType, string>>();
            GXDLMSObject obj = null;
            if (target is GXDLMSObject)
            {
                obj = target as GXDLMSObject;
            }
            GXDLMSException lastExternalException = null;
            foreach (GXDLMSXmlPdu it in actions)
            {
                if (!Continue)
                {
                    break;
                }

                if (it.Command == Command.Snrm && test.Device.Comm.client.InterfaceType == InterfaceType.WRAPPER)
                {
                    continue;
                }
                if (it.Command == Command.DisconnectRequest && test.Device.Comm.client.InterfaceType == InterfaceType.WRAPPER)
                {
                    break;
                }
                //Send
                string indexStr = " attribute ";
                if (it.IsRequest())
                {
                    lastExternalException = null;
                    if (settings.Delay.TotalSeconds != 0)
                    {
                        Thread.Sleep((int)settings.Delay.TotalMilliseconds);
                    }
                    indexStr = " attribute ";
                    XmlNode i = null;
                    switch (it.Command)
                    {
                        case Command.GetRequest:
                            i = it.XmlNode.SelectNodes("GetRequestNormal")[0];
                            break;
                        case Command.SetRequest:
                            i = it.XmlNode.SelectNodes("SetRequestNormal")[0];
                            break;
                        case Command.MethodRequest:
                            i = it.XmlNode.SelectNodes("ActionRequestNormal")[0];
                            indexStr = " method ";
                            break;
                    }
                    if (i == null)
                    {
                        ot = ObjectType.None;
                        index = 0;
                        ln = null;
                    }
                    else
                    {
                        if (it.Command != Command.MethodRequest)
                        {
                            ot = (ObjectType)int.Parse(i.SelectNodes("AttributeDescriptor/ClassId")[0].Attributes["Value"].Value);
                            index = int.Parse(i.SelectNodes("AttributeDescriptor/AttributeId")[0].Attributes["Value"].Value);
                            ln = (i.SelectNodes("AttributeDescriptor/InstanceId")[0].Attributes["Value"].Value);
                            //If attribute is not implement on this version.
                            if (obj != null && index > (obj as IGXDLMSBase).GetAttributeCount())
                            {
                                break;
                            }
                            if (obj != null && (obj.GetAccess(index) & AccessMode.Read) == 0)
                            {
                                reply.Clear();
                                continue;
                            }
                        }
                        else
                        {
                            ot = (ObjectType)int.Parse(i.SelectNodes("MethodDescriptor/ClassId")[0].Attributes["Value"].Value);
                            index = int.Parse(i.SelectNodes("MethodDescriptor/MethodId")[0].Attributes["Value"].Value);
                            ln = (i.SelectNodes("MethodDescriptor/InstanceId")[0].Attributes["Value"].Value);
                            //If method is not implement on this version.
                            if (obj != null && index > (obj as IGXDLMSBase).GetMethodCount())
                            {
                                break;
                            }
                            if (obj != null && obj.GetMethodAccess(index) == MethodAccessMode.NoAccess)
                            {
                                continue;
                            }
                        }
                        ln = GetLogicalName(ln);
                        test.OnTrace(test, ot + " " + ln + ":" + index + "\t");
                    }
                    reply.Clear();
                    //Skip association view and profile generic buffer.
                    if (obj != null)
                    {
                        if ((!external && (obj.ObjectType == ObjectType.AssociationLogicalName || obj.ObjectType == ObjectType.ProfileGeneric) && index == 2))
                        {
                            continue;
                        }
                    }
                    try
                    {
                        DateTime s = DateTime.Now;
                        byte[][] tmp = (test.Device.Comm.client as GXDLMSXmlClient).PduToMessages(it);
                        test.Device.Comm.ReadDataBlock(tmp, "", 1, reply);
                    }
                    catch (GXDLMSException ex)
                    {
                        using (Stream stream = File.Open(Path.Combine(test.Results, "error.txt"), FileMode.Append))
                        {
                            using (TextWriter writer = new StreamWriter(stream))
                            {
                                writer.WriteLine(DateTime.Now + ";" + ot + ";" + ln + ";" + indexStr + ";" + index + ";" + ex.Message);
                            }
                        }
                        //Error is not shown for external tests.
                        if (obj != null)
                        {
                            if (ex.ErrorCode != 0)
                            {
                                ErrorCode e = (ErrorCode)ex.ErrorCode;
                                AddError(test, null, output.Errors, "<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + indexStr + index + " failed: <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.ErrorCodes?" + e + ">" + e + "</a>)");
                                test.OnTrace(test, e + "\r\n");
                            }
                            else
                            {
                                AddError(test, null, output.Errors, "<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + indexStr + index + " <div class=\"tooltip\">failed:" + ex.Message);
                                AddError(test, null, output.Errors, "<span class=\"tooltiptext\">");
                                AddError(test, null, output.Errors, ex.ToString());
                                AddError(test, null, output.Errors, "</span></div>");
                                test.OnTrace(test, ex.Message + "\r\n");
                            }
                        }
                        else
                        {
                            //Don't check result for external tests.
                            //External test might that it fails.
                            lastExternalException = ex;
                        }
                    }
                    catch (Exception ex)
                    {
                        using (Stream stream = File.Open(Path.Combine(test.Results, "error.txt"), FileMode.Append))
                        {
                            using (TextWriter writer = new StreamWriter(stream))
                            {
                                writer.WriteLine(DateTime.Now + ";" + ot + ";" + ln + ";" + indexStr + ";" + index + ";" + ex.Message);
                            }
                        }
                        AddError(test, null, output.Errors, "<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + indexStr + index + " <div class=\"tooltip\">failed:" + ex.Message);
                        AddError(test, null, output.Errors, "<span class=\"tooltiptext\">");
                        AddError(test, null, output.Errors, ex.ToString());
                        AddError(test, null, output.Errors, "</span></div>");
                        test.OnTrace(test, ex.Message + "\r\n");
                    }
                }
                else if (reply.Data.Size != 0)
                {
                    indexStr = "Index ";
                    switch (it.Command)
                    {
                        case Command.GetResponse:
                            indexStr = "Get";
                            break;
                        case Command.SetResponse:
                            indexStr = "Set";
                            break;
                        case Command.MethodResponse:
                            indexStr = "Action";
                            break;
                    }
                    List<string> list = it.Compare(reply.ToString());
                    if (list.Count != 0)
                    {
                        //Association Logical Name attribute 4 and 6 might be also byte array.
                        if (ot == ObjectType.AssociationLogicalName && (index == 4 || index == 6) && reply.Value is byte[])
                        {
                            continue;
                        }
                        if (ot == ObjectType.None)
                        {
                            foreach (string err in list)
                            {
                                AddError(test, null, output.Errors, err);
                            }
                        }
                        else
                        {
                            if (lastExternalException != null)
                            {
                                ErrorCode e = (ErrorCode)lastExternalException.ErrorCode;
                                AddError(test, null, output.Errors, "<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + indexStr + index + " failed: <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.ErrorCodes?" + e + ">" + e + "</a>)");
                                test.OnTrace(test, e + "\r\n");
                                lastExternalException = null;
                            }
                            else
                            {
                                AddError(test, null, output.Errors, " <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + " " + indexStr + " " + index + " is <div class=\"tooltip\">invalid.");
                                AddError(test, null, output.Errors, "<span class=\"tooltiptext\">");
                                AddError(test, null, output.Errors, "Expected:</b><br/>");
                                AddError(test, null, output.Errors, it.PduAsXml.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br/>"));
                                AddError(test, null, output.Errors, "<br/><b>Actual:</b><br/>");
                                AddError(test, null, output.Errors, reply.ToString().Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br/>"));
                                AddError(test, null, output.Errors, "</span></div>");
                            }
                        }
                    }
                    else if (it.Command == Command.GetResponse)
                    {
                        indexStr += " Index ";
                        if (obj == null)
                        {
                            obj = GXDLMSClient.CreateObject(ot);
                            obj.LogicalName = ln;
                        }
                        ValueEventArgs e = new ValueEventArgs(obj, index, 0, null);
                        object value;
                        string name = (obj as IGXDLMSBase).GetNames()[index - 1];
                        if (target is GXDLMSAssociationLogicalName && index == 2)
                        {
                            value = reply.Value;
                        }
                        else
                        {
                            try
                            {
                                e.Value = reply.Value;
                                (obj as IGXDLMSBase).SetValue(test.Device.Comm.client.Settings, e);
                                value = obj.GetValues()[index - 1];
                            }
                            catch (Exception ex)
                            {
                                using (Stream stream = File.Open(Path.Combine(test.Results, "error.txt"), FileMode.Append))
                                {
                                    using (TextWriter writer = new StreamWriter(stream))
                                    {
                                        writer.WriteLine(DateTime.Now + ";" + ot + ";" + ln + ";" + indexStr + ";" + index + ";" + ex.Message);
                                    }
                                }
                                AddError(test, null, output.Errors, "<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + indexStr + index + " <div class=\"tooltip\">failed:" + ex.Message);
                                AddError(test, null, output.Errors, "<span class=\"tooltiptext\">");
                                AddError(test, null, output.Errors, ex.ToString());
                                AddError(test, null, output.Errors, "</span></div>");
                                test.OnTrace(test, ex.Message + "\r\n");
                                continue;
                            }
                        }
                        string str;
                        if (value is byte[])
                        {
                            DataType dt = obj.GetUIDataType(index);
                            if (dt == DataType.String)
                            {
                                str = ASCIIEncoding.ASCII.GetString((byte[])value);
                            }
                            else if (dt == DataType.DateTime || dt == DataType.Date || dt == DataType.Time)
                            {
                                str = GXDLMSClient.ChangeType((byte[])value, dt, test.Device.Comm.client.UseUtc2NormalTime).ToString();
                            }
                            else
                            {
                                str = GXCommon.ToHex((byte[])value);
                            }
                        }
                        else if (value is object[])
                        {
                            //TODO: str = GXDLMSTranslator.ValueToXml(value); Profile Generic capture objects causes problem at the moment.
                            str = GXHelpers.GetArrayAsString(value);
                        }
                        else if (value is System.Collections.IList)
                        {
                            //TODO: str = GXDLMSTranslator.ValueToXml(value);
                            str = GXHelpers.GetArrayAsString(value);
                        }
                        else
                        {
                            str = Convert.ToString(value);
                        }
                        test.OnTrace(test, str + "\r\n");
                        if (settings.ShowValues)
                        {
                            succeeded.Add(new KeyValuePair<ObjectType, string>(ot, indexStr + index.ToString() + ":" + name + "<br/>" + str));
                        }
                        else
                        {
                            if (it.Command == Command.GetResponse && settings.ShowValues)
                            {
                                succeeded.Add(new KeyValuePair<ObjectType, string>(ot, indexStr + index.ToString()));
                            }
                            else
                            {
                                succeeded.Add(new KeyValuePair<ObjectType, string>(ot, indexStr + index.ToString()));
                            }
                        }
                    }
                    else
                    {
                        succeeded.Add(new KeyValuePair<ObjectType, string>(ot, "Test: " + Path.GetFileNameWithoutExtension((string)target) + " " + indexStr + index.ToString()));
                    }
                }
            }
            if (lastExternalException != null)
            {
                ErrorCode e = (ErrorCode)lastExternalException.ErrorCode;
                AddError(test, null, output.Errors, "<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + " failed: <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.ErrorCodes?" + e + ">" + e + "</a>)");
                test.OnTrace(test, e + "\r\n");
                lastExternalException = null;
            }
            if (succeeded.Count != 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<div class=\"tooltip\">" + ln);
                sb.Append("<span class=\"tooltiptext\">");
                foreach (var it in succeeded)
                {
                    sb.Append(it.Value + "<br/>");
                }
                sb.Append("</span></div>");
                sb.Append("&nbsp;" + converter.GetDescription(ln, succeeded[0].Key)[0] + "&nbsp;" + "<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a>.");
                AddInfo(test, null, output.Info, sb.ToString());
            }
            if (obj != null)
            {
                test.Values.Add(obj);
            }
        }

        /// <summary>
        /// Make clone from the device.
        /// </summary>
        /// <param name="dev"></param>
        /// <returns></returns>
        public static GXDLMSDevice CloneDevice(GXDLMSDevice dev)
        {
            //Create clone from original items.
            using (MemoryStream ms = new MemoryStream())
            {
                List<Type> types = new List<Type>(GXDLMSClient.GetObjectTypes());
                types.Add(typeof(GXDLMSAttributeSettings));
                types.Add(typeof(GXDLMSAttribute));
                XmlAttributeOverrides overrides = new XmlAttributeOverrides();
                XmlAttributes attribs = new XmlAttributes();
                attribs.XmlIgnore = true;
                overrides.Add(typeof(GXDLMSDevice), "ObsoleteObjects", attribs);
                overrides.Add(typeof(GXDLMSAttributeSettings), attribs);
                XmlSerializer x = new XmlSerializer(typeof(GXDLMSDevice), overrides, types.ToArray(), null, "Gurux1");
                using (TextWriter writer = new StreamWriter(ms))
                {
                    x.Serialize(writer, dev);
                    ms.Position = 0;
                    using (XmlReader reader = XmlReader.Create(ms))
                    {
                        GXDLMSDevice dev2 = (GXDLMSDevice)x.Deserialize(reader);
                        dev2.Manufacturers = dev.Manufacturers;
                        dev = dev2;
                    }
                }
                ms.Close();
            }
            return dev;
        }

        private static void OnMessageTrace(DateTime time, GXDLMSDevice sender, string trace, byte[] data, int payload, string path, int duration)
        {
            //Save highest frame size.
            if (sender.Comm.client.InterfaceType == InterfaceType.HDLC && payload > sender.Comm.client.HdlcSettings.MaxInfoRX)
            {
                if (sender.Comm.payload < payload)
                {
                    sender.Comm.payload = payload;
                }
            }
            if (path != null)
            {
                lock (traceLock)
                {
                    using (FileStream fs = File.Open(path, FileMode.Append))
                    {
                        using (TextWriter writer = new StreamWriter(fs))
                        {
                            if (data == null)
                            {
                                writer.WriteLine(time.ToString("HH:mm:ss"));
                            }
                            else
                            {
                                writer.Write(time.ToString("HH:mm:ss") + " ");
                            }
                            writer.WriteLine(trace + " " + GXCommon.ToHex(data));
                            if (duration != 0 && Properties.Settings.Default.TraceDuration)
                            {
                                writer.WriteLine("Duration: " + duration);
                            }
                            writer.Flush();
                        }
                    }
                    if (duration != 0)
                    {
                        try
                        {
                            using (FileStream fs = File.Open(Path.Combine(Path.GetDirectoryName(path), "Durations.txt"), FileMode.Append))
                            {
                                using (TextWriter writer = new StreamWriter(fs))
                                {
                                    writer.WriteLine(time.ToString(CultureInfo.InvariantCulture) + ";" + duration);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            //User is opening the file. Try to write again.
                            Thread.Sleep(500);
                            using (FileStream fs = File.Open(Path.Combine(Path.GetDirectoryName(path), "Durations.txt"), FileMode.Append))
                            {
                                using (TextWriter writer = new StreamWriter(fs))
                                {
                                    writer.WriteLine(time.ToString(CultureInfo.InvariantCulture) + ";" + duration);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Load tests.
        /// </summary>
        /// <param name="stream">Stream where tests are read.</param>
        /// <param name="dev">DLMS device</param>
        /// <param name="tests">tests.</param>
        private static void GetTests(string name, Stream stream, GXDLMSDevice dev, List<KeyValuePair<GXDLMSObject, List<GXDLMSXmlPdu>>> tests)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);
            XmlNodeList list = doc.SelectNodes("/Messages/GetRequest/GetRequestNormal");
            ObjectType ot = ObjectType.None;
            name = name.Remove(0, "GXDLMSDirector.ConformanceTests.".Length);
            foreach (XmlNode node in list)
            {
                ot = (ObjectType)int.Parse(node.SelectNodes("AttributeDescriptor/ClassId")[0].Attributes["Value"].Value);
                if (dev != null)
                {
                    //Update logical name.
                    foreach (GXDLMSObject obj in dev.Objects.GetObjects(ot))
                    {
                        if (name.StartsWith("DLMS.v0.") ||
                            name.StartsWith("DLMS.v1.") ||
                            name.StartsWith("DLMS.v2.") ||
                            name.StartsWith("DLMS.v3."))
                        {
                            if (!name.StartsWith("DLMS.v" + obj.Version + "."))
                            {
                                break;
                            }
                        }
                        string tmp = GXCommon.ToHex(LogicalNameToBytes(obj.LogicalName), false);
                        foreach (XmlNode n in list)
                        {
                            XmlAttribute ln = n.SelectNodes("AttributeDescriptor/InstanceId")[0].Attributes["Value"];
                            ln.Value = tmp;
                        }
                        if (tests != null)
                        {
                            string standard = dev.Standard.ToString();
                            //Check if there is contry spesific test available.
                            foreach (var it in tests)
                            {
                                if (it.Key == obj)
                                {
                                    //Remove DLMS standard test and replace it with country spesific test.
                                    if (name.StartsWith(standard))
                                    {
                                        tests.Remove(it);
                                    }
                                    else
                                    {
                                        return;
                                    }
                                    break;
                                }
                            }
                            KeyValuePair<GXDLMSObject, List<GXDLMSXmlPdu>> tmp2 = new KeyValuePair<GXDLMSObject, List<GXDLMSXmlPdu>>(obj, (dev.Comm.client as GXDLMSXmlClient).LoadXml(doc.InnerXml));
                            tests.Add(tmp2);
                        }
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Read data from the meter.
        /// </summary>
        public static void ReadXmlMeter(object data)
        {
            GXDLMSConverter converter;
            object[] tmp2 = (object[])data;
            List<GXConformanceTest> tests = (List<GXConformanceTest>)tmp2[0];
            GXConformanceSettings settings = (GXConformanceSettings)tmp2[1];
            GXConformanceParameters cp = null;
            if (tmp2.Length > 3)
            {
                cp = (GXConformanceParameters)tmp2[3];
            }
            GXConformanceTest test;
            GXDLMSDevice dev = null;
            GXOutput output;
            while (Continue)
            {
                lock (tests)
                {
                    if (tests.Count == 0)
                    {
                        return;
                    }
                    test = tests[0];
                    test.Values = new GXDLMSObjectCollection();
                    dev = test.Device;
                    dev.InactivityTimeout = 0;
                    dev.OnTrace = OnMessageTrace;
                    dev.Comm.LogFile = Path.Combine(test.Results, "Trace.txt");
                    GXDLMSClient cl = dev.Comm.client;
                    converter = new GXDLMSConverter(dev.Standard);
                    dev.Comm.client = new GXDLMSXmlClient(TranslatorOutputType.SimpleXml, true);
                    dev.Comm.client.Ciphering.TestMode = true;
                    cl.CopyTo(dev.Comm.client);
                    test.Device = dev;
                    if (settings.ResendCount != -1)
                    {
                        dev.ResendCount = settings.ResendCount;
                    }
                    if (settings.WaitTime.TotalSeconds != 0)
                    {
                        dev.WaitTime = (int)settings.WaitTime.TotalSeconds;
                    }
                    output = new GXOutput(Path.Combine(test.Results, "Results.html"), dev.Name);
                    //Disable keep alive.
                    dev.InactivityTimeout = 0;
                    tests.RemoveAt(0);
                }
                IGXMedia media = dev.Media;
                GXDLMSXmlClient client = (GXDLMSXmlClient)dev.Comm.client;
                List<string> files = new List<string>();
                DateTime start = DateTime.Now;
                test.OnTrace(test, "Starting to execute test for " + dev.Name + ".\r\n");
                try
                {
                    output.PreInfo.Add("<a target=\"_blank\" href=\"https://www.gurux.fi/gurux.dlms.ctt.tests\">Gurux Conformance Tests</a>");

                    output.PreInfo.Add("Start Time: " + start.ToString());
                    output.PreInfo.Add("<hr>");
                    //HDLC tests.
                    if (GXConformanceEditor.IsEnabled(settings.ExcludedHdlcTests) && dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                    {
                        dev.Comm.UpdateSettings();
                        dev.Comm.InitializeConnection(false);
                        GXHDLCConformanceTests.HdlcTests(test, settings, dev, output, dev.ResendCount);
                        if (!Continue)
                        {
                            continue;
                        }
                    }
                    //Application tests.
                    if (GXConformanceEditor.IsEnabled(settings.ExcludedApplicationTests) && (dev.Comm.client.ConnectionState & ConnectionState.Dlms) == 0)
                    {
                        dev.Comm.UpdateSettings();
                        dev.Comm.InitializeConnection(false);
                    }
                    GXAppConformanceTests.CosemApplicationLayerTests(test, settings, dev, output);
                    if (!Continue)
                    {
                        continue;
                    }
                    if ((dev.Comm.client.ConnectionState & ConnectionState.Dlms) == 0)
                    {
                        dev.Comm.UpdateSettings();
                        dev.Comm.InitializeConnection(false);
                    }
                    if (client.Ciphering.InvocationCounter != 0)
                    {
                        output.PreInfo.Add("InvocationCounter: " + client.Ciphering.InvocationCounter);
                    }
                    if (dev.Comm.client.InterfaceType == InterfaceType.HDLC && !dev.Comm.client.HdlcSettings.UseFrameSize)
                    {
                        if (dev.MaxInfoRX != 128 && dev.MaxInfoRX != dev.Comm.client.HdlcSettings.MaxInfoRX)
                        {
                            output.Warnings.Add("Client asked that RX PDU size is " + dev.MaxInfoRX + ". Meter uses " + dev.Comm.client.HdlcSettings.MaxInfoRX);
                        }
                        if (dev.MaxInfoTX != 128 && dev.MaxInfoTX != dev.Comm.client.HdlcSettings.MaxInfoTX)
                        {
                            output.Warnings.Add("Client asked that TX PDU size is " + dev.MaxInfoTX + ". Meter uses " + dev.Comm.client.HdlcSettings.MaxInfoTX);
                        }
                        if (dev.PduSize < dev.Comm.client.MaxReceivePDUSize)
                        {
                            output.Warnings.Add("Client asked that PDU size is " + dev.PduSize + ". Meter uses " + dev.Comm.client.MaxReceivePDUSize);
                        }
                    }
                    int maxframesize = dev.Comm.client.HdlcSettings.MaxInfoRX;
                    if (settings.ReReadAssociationView)
                    {
                        test.OnTrace(test, "Re-reading association view.\r\n");
                        dev.Objects.Clear();
                        GXDLMSObjectCollection objs = dev.Comm.GetObjects();
                        while (objs.Count != 0)
                        {
                            GXDLMSObject obj = objs[0];
                            objs.Remove(obj);
                            dev.Objects.Add(obj);
                        }
                    }
                    if (client.UseLogicalNameReferencing)
                    {
                        output.PreInfo.Add("Testing using Logical Name referencing.");
                    }
                    else
                    {
                        output.PreInfo.Add("Testing using Short Name referencing.");
                    }
                    output.PreInfo.Add("Authentication level: " + dev.Authentication);
                    StringBuilder sb = new StringBuilder();
                    foreach (Conformance it in Enum.GetValues(typeof(Conformance)))
                    {
                        if (((int)it & (int)client.ProposedConformance) != 0)
                        {
                            sb.Append("<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Conformance?" + it + ">" + it + "</a>, ");
                        }
                    }
                    if (sb.Length != 0)
                    {
                        sb.Length -= 2;
                    }
                    output.PreInfo.Add("Proposed services:");
                    output.PreInfo.Add(sb.ToString());
                    sb.Clear();
                    foreach (Conformance it in Enum.GetValues(typeof(Conformance)))
                    {
                        if (((int)it & (int)client.NegotiatedConformance) != 0)
                        {
                            sb.Append("<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Conformance?" + it + ">" + it + "</a>, ");
                        }
                    }
                    if (sb.Length != 0)
                    {
                        sb.Length -= 2;
                    }
                    output.PreInfo.Add("Supported services:");
                    output.PreInfo.Add(sb.ToString());

                    if (!settings.ExcludeBasicTests)
                    {
                        output.PreInfo.Add("Total amount of objects: " + dev.Objects.Count.ToString());
                        //Check OBIS codes.
                        foreach (GXDLMSObject it in dev.Objects)
                        {
                            if (it.Description == "Invalid")
                            {
                                AddError(test, dev, output.Errors, "Invalid OBIS code " + it.LogicalName + " for <a target=\"_blank\" href=https://www.gurux.fi/" + it.GetType().FullName + ">" + it.ObjectType + "</a>.");
                                Console.WriteLine("------------------------------------------------------------");
                                Console.WriteLine(it.LogicalName + ": Invalid OBIS code.");
                            }
                        }
                    }
                    if (!settings.ExcludeMeterInfo)
                    {
                        ReadLogicalDeviceName(test, settings, dev, output);
                        GXDLMSData firmware = new GXDLMSData("1.0.0.2.0.255");
                        try
                        {
                            dev.Comm.ReadValue(firmware, 2);
                            object v = firmware.Value;
                            if (v is byte[])
                            {
                                v = ASCIIEncoding.ASCII.GetString((byte[])v);
                            }
                            output.PreInfo.Add("Firmware version is: " + Convert.ToString(v) + ".");
                        }
                        catch (Exception)
                        {
                            AddInfo(test, dev, output.Info, "Firmware version is not available.");
                        }
                        GXDLMSClock time = new GXDLMSClock("0.0.1.0.0.255");
                        try
                        {
                            dev.Comm.ReadValue(time, 2);
                            output.PreInfo.Add("Meter time: " + Convert.ToString(time.Time) + ".");
                        }
                        catch (Exception)
                        {
                            //It's OK if this fails.
                        }
                    }
                    else
                    {
                        test.OnTrace(test, "Meter information is not read.\r\n");
                        output.PreInfo.Add("Meter information is not read.");
                    }
                    //Read structures of Cosem objects.
                    List<KeyValuePair<GXDLMSObject, List<GXDLMSXmlPdu>>> cosemTests = new List<KeyValuePair<GXDLMSObject, List<GXDLMSXmlPdu>>>();
                    List<object> externalTests = new List<object>();
                    GXDLMSTranslator translator = new GXDLMSTranslator(TranslatorOutputType.SimpleXml);
                    lock (ConformanceLock)
                    {
                        //Load basic tests.
                        if (!settings.ExcludeBasicTests)
                        {
                            foreach (string it in GetBasicTests())
                            {
                                using (Stream stream = typeof(Program).Assembly.GetManifestResourceStream(it))
                                {
                                    GetTests(it, stream, dev, cosemTests);
                                    stream.Close();
                                }
                            }
                        }
                        else
                        {
                            test.OnTrace(test, "Basic tests are ignored.\r\n");
                            output.PreInfo.Add("Basic tests are ignored.");
                        }
                        //Load external tests.
                        string[] list = GetExternalTests(settings);
                        if (list.Length != 0)
                        {
                            string dir = Path.Combine(test.Results, "External");
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }

                            string str = "External tests: " + list.Length;
                            test.OnTrace(test, str + ".\r\n");
                            output.PreInfo.Add(str);
                            GXXmlLoadSettings ls = null;
                            if (!string.IsNullOrEmpty(settings.ReadLastDays))
                            {
                                ls = new GXXmlLoadSettings();
                                ls.End = DateTime.Now;
                                ls.Start = DateTime.Now.Date.AddDays(-Convert.ToInt32(settings.ReadLastDays)).Date;
                                ls.End = DateTime.Now.AddDays(1).Date;
                            }
                            foreach (string it in list)
                            {
                                try
                                {
                                    if (it.EndsWith("gxm"))
                                    {
                                        using (XmlReader reader = XmlReader.Create(it))
                                        {
                                            XmlSerializer x = new XmlSerializer(typeof(GXMacro[]));
                                            externalTests.Add((GXMacro[])x.Deserialize(reader));
                                            reader.Close();
                                        }
                                    }
                                    else
                                    {
                                        using (StreamReader fs = File.OpenText(it))
                                        {
                                            externalTests.Add(new KeyValuePair<string, List<GXDLMSXmlPdu>>(it, client.Load(fs, ls)));
                                            fs.Close();
                                        }
                                    }
                                    File.Copy(it, Path.Combine(dir, Path.GetFileName(it)));
                                }
                                catch (Exception e)
                                {
                                    string errStr = "Failed to load exteranal test " + it + ". " + e.Message;
                                    AddError(test, dev, output.Errors, errStr);
                                    test.OnTrace(test, errStr + "\r\n");
                                }

                            }
                        }
                    }
                    if (settings.Amount != 1)
                    {
                        string str = "Tests are run " + settings.Amount + " times.\r\n";
                        test.OnTrace(test, str);
                        output.PreInfo.Add(str);
                    }

                    for (int pos = 0; pos != settings.Amount; ++pos)
                    {
                        int i = 0, cnt = cosemTests.Count;
                        foreach (KeyValuePair<GXDLMSObject, List<GXDLMSXmlPdu>> it in cosemTests)
                        {
                            if (!Continue)
                            {
                                break;
                            }
                            try
                            {
                                test.OnProgress(test, "Testing " + it.Key.LogicalName, ++i, cnt);
                                Execute(converter, test, it.Key, it.Value, output, settings, false);
                            }
                            catch (Exception ex)
                            {
                                test.OnError(test, ex);
                            }
                        }
                        i = 0;
                        cnt = externalTests.Count;
                        foreach (object it in externalTests)
                        {
                            if (!Continue)
                            {
                                break;
                            }
                            try
                            {
                                if (it is GXMacro[] macros)
                                {
                                    Execute(converter, test, macros, output, settings);
                                }
                                else if (it is KeyValuePair<string, List<GXDLMSXmlPdu>> kv)
                                {
                                    test.OnProgress(test, "Testing " + kv.Key, ++i, cnt);
                                    Execute(converter, test, kv.Key, kv.Value, output, settings, true);
                                }
                            }
                            catch (Exception ex)
                            {
                                test.OnError(test, ex);
                            }
                        }
                        //Check this only once.
                        if (!settings.ExcludeBasicTests && pos == 0)
                        {
                            List<ObjectType> unknownDataTypes = new List<ObjectType>();
                            foreach (GXDLMSObject o in dev.Objects)
                            {
                                if (!unknownDataTypes.Contains(o.ObjectType))
                                {
                                    bool found = false;
                                    foreach (KeyValuePair<GXDLMSObject, List<GXDLMSXmlPdu>> t in cosemTests)
                                    {
                                        if (o.ObjectType == t.Key.ObjectType)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                    if (!found)
                                    {
                                        unknownDataTypes.Add(o.ObjectType);
                                        output.Warnings.Add("<a target=\"_blank\" href=https://www.gurux.fi/" + o.GetType().FullName + ">" + o.ObjectType + "</a> is not tested.");
                                    }
                                }
                            }
                        }
                        if (settings.Write)
                        {
                            test.OnTrace(test, "Write tests started\r\n");
                            foreach (GXDLMSObject obj in dev.Objects)
                            {
                                for (int index = 1; index != (obj as IGXDLMSBase).GetAttributeCount(); ++index)
                                {
                                    if ((obj.GetAccess(index) & AccessMode.Read) != 0 && (obj.GetAccess(index) & AccessMode.Write) != 0)
                                    {
                                        ObjectType ot = obj.ObjectType;
                                        string ln = obj.LogicalName;
                                        try
                                        {
                                            test.OnTrace(test, ot + " " + ln + ":" + index + "\r\n");
                                            object expected = obj.GetValues()[index - 1];
                                            dev.Comm.Write(obj, index);
                                            object actual = obj.GetValues()[index - 1];
                                            //Check that value is not changed.
                                            if (Convert.ToString(expected) != Convert.ToString(actual))
                                            {
                                                AddError(test, dev, output.Errors, "Write <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + " attribute " + index + " is <div class=\"tooltip\">failed.");
                                                AddError(test, dev, output.Errors, "<span class=\"tooltiptext\">");
                                                AddError(test, dev, output.Errors, "Expected:</b><br/>");
                                                AddError(test, dev, output.Errors, Convert.ToString(expected).Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br/>"));
                                                AddError(test, dev, output.Errors, "<br/><b>Actual:</b><br/>");
                                                AddError(test, dev, output.Errors, Convert.ToString(actual).Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br/>"));
                                                AddError(test, dev, output.Errors, "</span></div>");
                                            }
                                            else
                                            {
                                                AddInfo(test, dev, output.Info, "Write" + ot + " " + ln + " attribute " + index + " Succeeded.");
                                            }
                                        }
                                        catch (GXDLMSException ex)
                                        {
                                            if (ex.ErrorCode != 0)
                                            {
                                                ErrorCode e = (ErrorCode)ex.ErrorCode;
                                                AddError(test, dev, output.Errors, "<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + " attribute " + index + " failed: <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.ErrorCodes?" + e + ">" + e + "</a>)");
                                            }
                                            else
                                            {
                                                AddError(test, dev, output.Errors, "<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + " attribute " + index + " <div class=\"tooltip\">failed:" + ex.Message);
                                                AddError(test, dev, output.Errors, "<span class=\"tooltiptext\">");
                                                AddError(test, dev, output.Errors, ex.ToString().Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br/>"));
                                                AddError(test, dev, output.Errors, "</span></div>");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            AddError(test, dev, output.Errors, "Write <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + " attribute " + index + " <div class=\"tooltip\">failed. " + ex.Message);
                                            AddError(test, dev, output.Errors, "<span class=\"tooltiptext\">");
                                            AddError(test, dev, output.Errors, ex.ToString().Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br/>"));
                                            AddError(test, dev, output.Errors, "</span></div>");
                                        }
                                    }
                                }
                            }
                        }

                        if (!settings.ExcludeBasicTests)
                        {
                            if ((dev.Comm.client.NegotiatedConformance & Conformance.MultipleReferences) == 0)
                            {
                                AddInfo(test, dev, output.Info, "Meter don't support multiple references.");
                            }
                            else
                            {
                                GXDLMSObjectCollection objs = dev.Objects.GetObjects(ObjectType.Data);
                                if (objs.Count != 0)
                                {
                                    AddInfo(test, dev, output.Info, "Testing multiple references support using " + objs.Count + " data object(s).");
                                    List<KeyValuePair<GXDLMSObject, int>> list = new List<KeyValuePair<GXDLMSObject, int>>();
                                    foreach (GXDLMSObject it in objs)
                                    {
                                        list.Add(new KeyValuePair<GXDLMSObject, int>(it, 2));
                                    }
                                    (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                                    try
                                    {
                                        dev.Comm.ReadList(list);
                                    }
                                    finally
                                    {
                                        (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = false;
                                    }
                                    AddInfo(test, dev, output.Info, "Multiple references support succeeded.");
                                }
                            }
                        }
                        try
                        {
                            TestImageTransfer(settings, test, dev, output);
                        }
                        catch (Exception ex)
                        {
                            AddError(test, dev, output.Errors, ex.Message);
                            test.OnError(test, ex);
                        }
                    }
                    if (!settings.ExcludeBasicTests)
                    {
                        TestAssociationLn(settings, dev, output);

                    }
                    if (GXConformanceEditor.IsEnabled(settings.ExcludedGuruxTests.ClockTests))
                    {
                        (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                        TestClock(test, settings, dev, output);
                        (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = false;
                    }
                    if (GXConformanceEditor.IsEnabled(settings.ExcludedGuruxTests.ProfileGenericTests))
                    {
                        (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                        TestProfileGeneric(test, settings, dev, output);
                        (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = false;
                    }
                    if (GXConformanceEditor.IsEnabled(settings.ExcludedGuruxTests.ServiceTests))
                    {
                        (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                        TestServices(test, settings, dev, output);
                        (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = false;
                    }
                    if (GXConformanceEditor.IsEnabled(settings.ExcludedGuruxTests.AuthenticationTests))
                    {
                        (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                        TestAuthentications(test, settings, dev, output);
                        (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = false;
                    }

                    if (dev.Comm.payload != 0)
                    {
                        //TODO:   output.Errors.Insert(0, "HDLC payload size is is too high. There are " + dev.Comm.payload + " bytes. Max size is " + dev.Comm.client.HdlcSettings.MaxInfoRX + " bytes.");
                    }
                    if (output.Errors.Count != 0)
                    {
                        test.ErrorLevel = 2;
                    }
                    else if (output.Warnings.Count != 0)
                    {
                        test.ErrorLevel = 1;
                    }
                    else
                    {
                        test.ErrorLevel = 0;
                    }
                    test.Values.Save(Path.Combine(test.Results, "values.xml"), new GXXmlWriterSettings());
                    test.OnReady(test);
                }
                catch (Exception ex)
                {
                    AddError(test, dev, output.Errors, ex.Message);
                    test.ErrorLevel = 2;
                    test.OnError(test, ex);
                    using (Stream stream = File.Open(Path.Combine(test.Results, "error.txt"), FileMode.Append))
                    {
                        using (TextWriter writer = new StreamWriter(stream))
                        {
                            writer.WriteLine(DateTime.Now + ";" + ex.Message);
                        }
                    }
                }
                finally
                {
                    output.PreInfo.Insert(1, "Ran for " + (DateTime.Now - start).ToString());
                    output.MakeReport();
                    output.writer.Flush();
                    output.writer.Close();
                    if (dev != null)
                    {
                        dev.Comm.Disconnect();
                    }
                    if (cp != null && Interlocked.Decrement(ref cp.numberOfTasks) == 0)
                    {
                        cp.finished.Set();
                    }
                }
            }
        }

        private static void ReadRowsByEntry(GXDLMSDevice dev, GXDLMSProfileGeneric pg, UInt32 index, UInt32 count, List<GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>> columns)
        {
            GXReplyData reply = new GXReplyData();
            byte[][] data = dev.Comm.client.ReadRowsByEntry(pg, index, count, columns);
            dev.Comm.ReadDataBlock(data, "Read rows by entry", 2, reply);
            dev.Comm.client.UpdateValue(pg, 2, reply.Value);
        }

        private static void ReadRowsByRange(GXDLMSDevice dev, GXDLMSProfileGeneric pg, DateTime start, DateTime end,
                                        List<GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>> columns)
        {
            GXReplyData reply = new GXReplyData();
            byte[][] data = dev.Comm.client.ReadRowsByRange(pg, start, end, columns);
            dev.Comm.ReadDataBlock(data, "Read rows by range", 2, reply);
            dev.Comm.client.UpdateValue(pg, 2, reply.Value);
        }

        /// <summary>
        /// Test authentications.
        /// </summary>
        /// <param name="settings">Conformance settings.</param>
        /// <param name="dev">DLMS device.</param>
        /// <param name="output"></param>
        private static void TestAuthentications(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output)
        {
            if (!settings.ExcludedGuruxTests.AuthenticationTests.WrongPassword)
            {
                try
                {
                    TestInvalidPassword(test, settings, dev, output);
                }
                catch (Exception ex)
                {
                    AddError(test, dev, output.Errors, ex.Message);
                    test.OnError(test, ex);
                }
            }
        }

        /// <summary>
        /// Test services.
        /// </summary>
        /// <param name="settings">Conformance settings.</param>
        /// <param name="dev">DLMS device.</param>
        /// <param name="output"></param>
        private static void TestServices(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output)
        {
            bool useLogicalNameReferencing = dev.Comm.client.UseLogicalNameReferencing;
            int conformance = dev.Conformance;
            if (!settings.ExcludedGuruxTests.ServiceTests.IncompatibleConformance)
            {
                dev.Comm.Disconnect();
                try
                {
                    dev.Conformance = 0;
                    Thread.Sleep((int)settings.DelayConnection.TotalMilliseconds);
                    dev.InitializeConnection();
                    AddError(test, dev, output.Errors, "Empty service test failed.");
                }
                catch (GXDLMSConfirmedServiceError ex)
                {
                    AddInfo(test, dev, output.Info, "Empty service test succeeded. " + ex.Message);
                }
                catch (Exception ex)
                {
                    AddError(test, dev, output.Errors, "Empty service test failed. " + ex.Message);
                }
                finally
                {
                    dev.Conformance = conformance;
                }
            }

            dev.Comm.Disconnect();
            try
            {
                if (dev.UseLogicalNameReferencing)
                {
                    dev.Conformance = (int)Conformance.Get;
                }
                else
                {
                    dev.Conformance = (int)Conformance.Read;
                }
                if (dev.Authentication > Authentication.Low)
                {
                    dev.Conformance |= (int)Conformance.Action;
                }
                Thread.Sleep((int)settings.DelayConnection.TotalMilliseconds);
                dev.InitializeConnection();
                if (dev.Comm.client.NegotiatedConformance != (Conformance)dev.Conformance)
                {
                    AddError(test, dev, output.Errors, "Proposed conformance service test failed.");
                }
            }
            catch (GXDLMSException)
            {
                AddError(test, dev, output.Errors, "Proposed conformance service test failed.");
            }
            if ((Conformance.GeneralProtection & dev.Comm.client.NegotiatedConformance) == 0)
            {
                //This cannot be tested.
            }
            if ((Conformance.GeneralBlockTransfer & dev.Comm.client.NegotiatedConformance) == 0)
            {
                //This cannot be tested.
            }
            if (!settings.ExcludedGuruxTests.ServiceTests.Read && (Conformance.Read & dev.Comm.client.NegotiatedConformance) == 0)
            {
                try
                {
                    GXReplyData reply = new GXReplyData();
                    dev.Comm.client.UseLogicalNameReferencing = false;
                    dev.Comm.ReadDataBlock(dev.Comm.client.Read((ushort)0xFA00, ObjectType.AssociationShortName, 1), "Read service test.", 1, 0, reply);
                    AddError(test, dev, output.Errors, "Read service test failed. Meter replied.");
                }
                catch (GXDLMSExceptionResponse ex)
                {
                    AddInfo(test, dev, output.Info, "Read service test failed. " + ex.ExceptionServiceError);
                }
                catch (TimeoutException)
                {
                    AddInfo(test, dev, output.Info, "Read service test. Meter didn't reply.");
                }
                catch (Exception ex)
                {
                    AddError(test, dev, output.Errors, "Read service test failed. " + ex.Message);
                }
                finally
                {
                    dev.Comm.client.UseLogicalNameReferencing = useLogicalNameReferencing;
                }
            }
            if (!settings.ExcludedGuruxTests.ServiceTests.Write && (Conformance.Write & dev.Comm.client.NegotiatedConformance) == 0)
            {
                try
                {
                    //Find writable object and write it.
                    int index = 0;
                    GXDLMSObject obj = null;
                    foreach (GXDLMSObject it in dev.Objects)
                    {
                        for (int pos = 1; pos != (it as IGXDLMSBase).GetAttributeCount(); ++pos)
                        {
                            if (it.GetAccess(pos) == AccessMode.ReadWrite)
                            {
                                obj = it;
                                index = pos;
                                break;
                            }
                        }
                    }
                    if (obj == null)
                    {
                        AddInfo(test, dev, output.Info, "Write service test skipped. There are no writable objects.");
                    }
                    else
                    {
                        GXReplyData reply = new GXReplyData();
                        dev.Comm.client.UseLogicalNameReferencing = false;
                        GXDLMSAssociationShortName sn = new GXDLMSAssociationShortName();
                        dev.Comm.ReadDataBlock(dev.Comm.client.Write(sn, 1), "Read service test", 1, 0, reply);
                        AddError(test, dev, output.Errors, "Write service test failed");
                    }
                }
                catch (GXDLMSExceptionResponse ex)
                {
                    AddInfo(test, dev, output.Info, "Write service test succeeded. " + ex.ExceptionServiceError);
                }
                catch (TimeoutException)
                {
                    AddInfo(test, dev, output.Info, "Write service test succeeded. Meter didn't reply.");
                }
                catch (Exception ex)
                {
                    AddError(test, dev, output.Errors, "Write service test failed. " + ex.Message);
                }
                finally
                {
                    dev.Comm.client.UseLogicalNameReferencing = useLogicalNameReferencing;
                }
            }
            if ((Conformance.UnconfirmedWrite & dev.Comm.client.NegotiatedConformance) == 0)
            {
                //This cannot be tested.
            }
            if ((Conformance.Attribute0SupportedWithSet & dev.Comm.client.NegotiatedConformance) == 0)
            {
                //This cannot be tested.
            }
            if (!settings.ExcludedGuruxTests.ServiceTests.Attribute0SupportedWithGet && (Conformance.Attribute0SupportedWithGet & dev.Comm.client.NegotiatedConformance) == 0)
            {
                try
                {
                    //Find Data object and read it.
                    GXDLMSObject obj = null;
                    foreach (GXDLMSObject it in dev.Objects.GetObjects(ObjectType.Data))
                    {
                        obj = it;
                        break;
                    }
                    if (obj == null)
                    {
                        AddError(test, dev, output.Errors, "Attribute0SupportedWithGet service test skipped. There are no data objects.");
                    }
                    else
                    {
                        GXReplyData reply = new GXReplyData();
                        dev.Comm.ReadDataBlock(dev.Comm.Read(obj, 0), "Attribute0SupportedWithGet service test", 1, 0, reply);
                        AddError(test, dev, output.Errors, "Attribute0SupportedWithGet service test failed.");
                    }
                }
                catch (GXDLMSExceptionResponse ex)
                {
                    AddInfo(test, dev, output.Info, "Attribute0SupportedWithGet service test succeeded. " + ex.ExceptionServiceError);
                }
                catch (TimeoutException)
                {
                    AddInfo(test, dev, output.Info, "Attribute0SupportedWithGet service test succeeded. Meter didn't reply.");
                }
                catch (Exception ex)
                {
                    AddError(test, dev, output.Errors, "Attribute0SupportedWithGet service test failed. " + ex.Message);
                }
            }
            if (!settings.ExcludedGuruxTests.ServiceTests.BlockTransferWithGetOrRead && (Conformance.BlockTransferWithGetOrRead & dev.Comm.client.NegotiatedConformance) == 0)
            {
                try
                {
                    GXReplyData reply = new GXReplyData();
                    dev.Comm.client.ForceToBlocks = true;
                    dev.Comm.ReadDataBlock(dev.Comm.Read(dev.Objects[0], 1), "BlockTransferWithGetOrRead service test", 1, 0, reply);
                    AddError(test, dev, output.Errors, "BlockTransferWithGetOrRead service failed.");
                }
                catch (GXDLMSExceptionResponse ex)
                {
                    AddInfo(test, dev, output.Info, "BlockTransferWithGetOrRead service succeeded. " + ex.ExceptionServiceError);
                }
                catch (TimeoutException)
                {
                    AddInfo(test, dev, output.Info, "BlockTransferWithGetOrRead service succeeded. Meter didn't reply.");
                }
                catch (Exception ex)
                {
                    AddError(test, dev, output.Errors, "BlockTransferWithGetOrRead service test failed. " + ex.Message);
                }
                finally
                {
                    dev.Comm.client.ForceToBlocks = false;
                }
            }
            if (!settings.ExcludedGuruxTests.ServiceTests.BlockTransferWithSetOrWrite && (Conformance.BlockTransferWithSetOrWrite & dev.Comm.client.NegotiatedConformance) == 0)
            {
                try
                {
                    //Find writable object and write it.
                    int index = 0;
                    GXDLMSObject obj = null;
                    foreach (GXDLMSObject it in dev.Objects)
                    {
                        for (int pos = 1; pos != (it as IGXDLMSBase).GetAttributeCount(); ++pos)
                        {
                            if (it.GetAccess(pos) == AccessMode.ReadWrite)
                            {
                                obj = it;
                                index = pos;
                                break;
                            }
                        }
                    }
                    if (obj == null)
                    {
                        AddInfo(test, dev, output.Info, "BlockTransferWithSetOrWrite service test skipped. There are no writable objects.");
                    }
                    else
                    {
                        GXReplyData reply = new GXReplyData();
                        dev.Comm.client.ForceToBlocks = true;
                        dev.Comm.ReadDataBlock(dev.Comm.Read(obj, index), "Attribute0SupportedWithGet service BlockTransferWithSetOrWrite", 0, reply);
                        dev.Comm.Write(obj, index);
                        AddError(test, dev, output.Errors, "BlockTransferWithSetOrWrite service test failed.");
                    }
                }
                catch (GXDLMSExceptionResponse ex)
                {
                    AddInfo(test, dev, output.Info, "BlockTransferWithSetOrWrite service test succeeded. " + ex.ExceptionServiceError);
                }
                catch (TimeoutException)
                {
                    AddInfo(test, dev, output.Info, "Attribute0SupportedBlockTransferWithSetOrWriteWithGet service test succeeded. Meter didn't reply.");
                }
                catch (Exception ex)
                {
                    AddError(test, dev, output.Errors, "BlockTransferWithSetOrWrite service test failed. " + ex.Message);
                }
                finally
                {
                    dev.Comm.client.ForceToBlocks = false;
                }
            }
            if (!settings.ExcludedGuruxTests.ServiceTests.BlockTransferWithAction && (Conformance.BlockTransferWithAction & dev.Comm.client.NegotiatedConformance) == 0)
            {
                //This cannot be tested.
            }
            if (!settings.ExcludedGuruxTests.ServiceTests.MultipleReferences && (Conformance.MultipleReferences & dev.Comm.client.NegotiatedConformance) == 0)
            {
                try
                {
                    List<KeyValuePair<GXDLMSObject, int>> list = new List<KeyValuePair<GXDLMSObject, int>>();
                    foreach (GXDLMSObject it in dev.Objects.GetObjects(ObjectType.Data))
                    {
                        list.Add(new KeyValuePair<GXDLMSObject, int>(it, 1));
                        if (list.Count == 2)
                        {
                            break;
                        }
                    }
                    dev.Comm.client.NegotiatedConformance |= Conformance.MultipleReferences;
                    dev.Comm.ReadList(list);
                    AddError(test, dev, output.Errors, "MultipleReferences service test failed.");
                }
                catch (GXDLMSExceptionResponse ex)
                {
                    AddInfo(test, dev, output.Info, "MultipleReferences service test succeeded. " + ex.ExceptionServiceError);
                }
                catch (TimeoutException)
                {
                    AddInfo(test, dev, output.Info, "MultipleReferences service test succeeded. Meter didn't reply.");
                }
                catch (Exception ex)
                {
                    AddError(test, dev, output.Errors, "MultipleReferences service test failed. " + ex.Message);
                }
            }

            if ((Conformance.Access & dev.Comm.client.NegotiatedConformance) == 0)
            {
                //This cannot be tested at the moment.
            }
            if (!settings.ExcludedGuruxTests.ServiceTests.ParameterizedAccess && (Conformance.ParameterizedAccess & dev.Comm.client.NegotiatedConformance) == 0)
            {
                //Not implemented.
            }
            if (!settings.ExcludedGuruxTests.ServiceTests.Get && (Conformance.Get & dev.Comm.client.NegotiatedConformance) == 0)
            {
                try
                {
                    GXReplyData reply = new GXReplyData();
                    dev.Comm.ReadDataBlock(dev.Comm.Read(dev.Objects[0], 1), "Get service test", 1, 0, reply);
                    AddError(test, dev, output.Errors, "Get service test failed.");
                }
                catch (GXDLMSExceptionResponse ex)
                {
                    AddInfo(test, dev, output.Info, "Get service test succeeded. " + ex.ExceptionServiceError);
                }
                catch (TimeoutException)
                {
                    AddInfo(test, dev, output.Info, "Get service test succeeded. Meter didn't reply.");
                }
                catch (Exception ex)
                {
                    AddError(test, dev, output.Errors, "Get service test failed. " + ex.Message);
                }
            }
            if (!settings.ExcludedGuruxTests.ServiceTests.Set && (Conformance.Set & dev.Comm.client.NegotiatedConformance) == 0)
            {
                try
                {
                    //Find writable object and write it.
                    int index = 0;
                    GXDLMSObject obj = null;
                    foreach (GXDLMSObject it in dev.Objects)
                    {
                        for (int pos = 1; pos != (it as IGXDLMSBase).GetAttributeCount(); ++pos)
                        {
                            if (it.GetAccess(pos) == AccessMode.ReadWrite)
                            {
                                obj = it;
                                index = pos;
                                break;
                            }
                        }
                    }
                    if (obj == null)
                    {
                        AddInfo(test, dev, output.Info, "Set service test skipped. There are no writable objects.");
                    }
                    else
                    {
                        GXReplyData reply = new GXReplyData();
                        dev.Comm.ReadDataBlock(dev.Comm.Read(obj, index), "Set service test", 0, reply);
                        dev.Comm.Write(obj, index);
                        AddError(test, dev, output.Errors, "Set service test failed.");
                    }
                }
                catch (GXDLMSExceptionResponse ex)
                {
                    AddInfo(test, dev, output.Info, "Set service test succeeded. " + ex.ExceptionServiceError);
                }
                catch (TimeoutException)
                {
                    AddInfo(test, dev, output.Info, "Set service test succeeded. Meter didn't reply.");
                }
                catch (Exception ex)
                {
                    AddError(test, dev, output.Errors, "Set service test test failed. " + ex.Message);
                }
            }
            if (!settings.ExcludedGuruxTests.ServiceTests.SelectiveAccess && (Conformance.SelectiveAccess & dev.Comm.client.NegotiatedConformance) == 0)
            {
                try
                {
                    //Find executed object and execute it.
                    int index = 0;
                    GXDLMSObject obj = null;
                    foreach (GXDLMSObject it in dev.Objects.GetObjects(ObjectType.ProfileGeneric))
                    {
                        obj = it;
                        index = 2;
                        break;
                    }
                    if (obj == null)
                    {
                        AddInfo(test, dev, output.Info, "SelectiveAccess service test skipped. There are no profile generic objects to test.");
                    }
                    else
                    {
                        GXReplyData reply = new GXReplyData();
                        dev.Comm.ReadDataBlock(dev.Comm.Read(obj, index), "SelectiveAccess service test", 0, reply);
                        dev.Comm.Write(obj, index);
                        AddError(test, dev, output.Errors, "SelectiveAccess service test failed.");
                    }
                }
                catch (GXDLMSExceptionResponse ex)
                {
                    AddInfo(test, dev, output.Info, "SelectiveAccess service test succeeded. " + ex.ExceptionServiceError);
                }
                catch (GXDLMSException ex)
                {
                    AddInfo(test, dev, output.Info, "SelectiveAccess service test succeeded. " + ex.Message);
                }
                catch (TimeoutException)
                {
                    AddInfo(test, dev, output.Info, "SelectiveAccess service test succeeded. Meter didn't reply.");
                }
                catch (Exception ex)
                {
                    AddError(test, dev, output.Errors, "SelectiveAccess service test failed. " + ex.Message);
                }
            }
            if (!settings.ExcludedGuruxTests.ServiceTests.Action && (Conformance.Action & dev.Comm.client.NegotiatedConformance) == 0)
            {
                try
                {
                    //Find executed object and execute it.
                    int index = 0;
                    GXDLMSObject obj = null;
                    foreach (GXDLMSObject it in dev.Objects)
                    {
                        for (int pos = 1; pos != (it as IGXDLMSBase).GetMethodCount(); ++pos)
                        {
                            if (it.GetMethodAccess(pos) == MethodAccessMode.Access)
                            {
                                obj = it;
                                index = pos;
                                break;
                            }
                        }
                    }
                    if (obj == null)
                    {
                        AddInfo(test, dev, output.Info, "Action service test skipped. There are no actions to execute.");
                    }
                    else
                    {
                        GXReplyData reply = new GXReplyData();
                        dev.Comm.ReadDataBlock(dev.Comm.Read(obj, index), "Action service test", 0, reply);
                        dev.Comm.Write(obj, index);
                        AddError(test, dev, output.Errors, "Action service test failed.");
                    }
                }
                catch (GXDLMSExceptionResponse ex)
                {
                    AddInfo(test, dev, output.Info, "Action service test succeeded. " + ex.ExceptionServiceError);
                }
                catch (GXDLMSException ex)
                {
                    AddInfo(test, dev, output.Info, "Action service test succeeded. " + ex.Message);
                }
                catch (TimeoutException)
                {
                    AddInfo(test, dev, output.Info, "Action service test succeeded. Meter didn't reply.");
                }
                catch (Exception ex)
                {
                    AddError(test, dev, output.Errors, "Action service test failed. " + ex.Message);
                }
            }
            dev.Conformance = conformance;
        }

        /// <summary>
        /// Test profile generic.
        /// </summary>
        /// <param name="settings">Conformance settings.</param>
        /// <param name="dev">DLMS device.</param>
        /// <param name="output"></param>
        private static void TestProfileGeneric(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output)
        {
            GXDLMSObjectCollection objects = dev.Comm.client.Objects.GetObjects(ObjectType.ProfileGeneric);
            foreach (GXDLMSProfileGeneric pg in objects)
            {
                //Check that capture objects are equal
                if (pg.CaptureObjects.Count != 0)
                {
                    List<GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>> catureObjects = new List<GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>>();
                    catureObjects.AddRange(pg.CaptureObjects);
                    dev.Comm.ReadValue(pg, 3);
                    if (pg.CaptureObjects.Count != catureObjects.Count)
                    {
                        AddError(test, dev, output.Errors, "Profile generic " + pg.LogicalName + " failed. Amount of the capture objects is different.");
                    }
                    else
                    {
                        IEnumerator<GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>> e = catureObjects.GetEnumerator();
                        foreach (GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject> it in pg.CaptureObjects)
                        {
                            e.MoveNext();
                            if (e.Current.Key.LogicalName != it.Key.LogicalName)
                            {
                                AddError(test, dev, output.Errors, "Profile generic " + pg.LogicalName + " failed. Logical name is " + it.Key.LogicalName + " and it should be " + e.Current.Key.LogicalName);
                            }
                            else if (e.Current.Key.GetType() != it.Key.GetType())
                            {
                                AddError(test, dev, output.Errors, "Profile generic " + pg.LogicalName + " failed. Logical name is " + it.Key.GetType() + " and it should be " + e.Current.Key.GetType());
                            }
                            else if (e.Current.Value.AttributeIndex != it.Value.AttributeIndex)
                            {
                                AddError(test, dev, output.Errors, "Profile generic " + pg.LogicalName + " failed. Attribute index is " + it.Value.AttributeIndex + " and it should be " + e.Current.Value.AttributeIndex);
                            }
                            else if (e.Current.Value.DataIndex != it.Value.DataIndex)
                            {
                                AddError(test, dev, output.Errors, "Profile generic " + pg.LogicalName + " failed. Data index is " + it.Value.DataIndex + " and it should be " + e.Current.Value.DataIndex);
                            }
                        }
                    }
                }
                bool passed = true;
                //Read EntriesInUse
                dev.Comm.ReadValue(pg, 7);
                UInt32 entriesInUse = pg.EntriesInUse;
                if (entriesInUse == 0)
                {
                    output.Warnings.Add("Failed to test Profile Generic " + pg.LogicalName + " because buffer is empty.");
                }
                else
                {
                    try
                    {
                        dev.Comm.ReadValue(pg, 3);
                    }
                    catch (Exception)
                    {
                        AddError(test, dev, output.Errors, "Profile generic " + pg.LogicalName + " failed. Failed to read capture objects.");
                        continue;
                    }
                    if (pg.CaptureObjects.Count == 0)
                    {
                        AddInfo(test, dev, output.Info, "Profile generic " + pg.LogicalName + " failed. Capture objects is empty.");
                        continue;
                    }
                    if (settings.ExcludedGuruxTests.ProfileGenericTests.EntryIndex0)
                    {
                        try
                        {
                            //Numbering of entries and selected values starts from 1.
                            ReadRowsByEntry(dev, pg, 0, 1, null);
                            passed = false;
                        }
                        catch (Exception)
                        {
                            //This should fail.
                        }
                        if (passed)
                        {
                            AddInfo(test, dev, output.Info, "Testing Profile Generic " + pg.LogicalName + " read by entry #0 succeeded.");
                        }
                        else
                        {
                            AddError(test, dev, output.Errors, "Testing Profile Generic " + pg.LogicalName + " read by entry #0 failed.");
                        }
                    }
                    //Read first two lines and read by range and check the values.
                    if (settings.ExcludedGuruxTests.ProfileGenericTests.ReadRowsByRange)
                    {
                        try
                        {
                            if (entriesInUse < 3)
                            {
                                AddInfo(test, dev, output.Info, "Profile generic " + pg.LogicalName + " skipped. Amount ot the rows is too small.");
                            }
                            else if (pg.CaptureObjects[0].Key is GXDLMSClock)
                            {
                                ReadRowsByEntry(dev, pg, 1, 2, null);
                                List<object[]> rows = pg.Buffer;
                                GXDateTime start = (GXDateTime)rows[0][0];
                                GXDateTime end = (GXDateTime)rows[1][0];
                                ReadRowsByRange(dev, pg, start, end, null);
                                if (pg.Buffer.Count < 2)
                                {
                                    AddError(test, dev, output.Errors, "Profile generic " + pg.LogicalName + " failed. ReadRowsByRange returned wrong amount of rows.");
                                }
                                else
                                {
                                    foreach (object[] it in pg.Buffer)
                                    {
                                        GXDateTime dt = (GXDateTime)it[0];
                                        if (dt.Value < start.Value || dt.Value > end.Value)
                                        {
                                            AddInfo(test, dev, output.Info, "Profile generic " + pg.LogicalName + " failed. ReadRowsByRange returned wrong item. Start time: " + start + " End time: " + end + " Actual time: " + dt);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            AddError(test, dev, output.Errors, "Testing Profile Generic " + pg.LogicalName + " " + ex.Message);
                        }
                    }
                    //Read last row.
                    if (settings.ExcludedGuruxTests.ProfileGenericTests.ReadLastRow)
                    {
                        try
                        {
                            if (pg.CaptureObjects[0].Key is GXDLMSClock)
                            {
                                ReadRowsByEntry(dev, pg, entriesInUse, entriesInUse, null);
                                List<object[]> rows = pg.Buffer;
                                GXDateTime start = (GXDateTime)rows[0][0];
                                ReadRowsByRange(dev, pg, start, DateTime.MaxValue, null);
                                if (pg.Buffer.Count != 1)
                                {
                                    AddError(test, dev, output.Errors, "Profile generic " + pg.LogicalName + " failed. ReadRowsByRange returned invalid amount of rows when last one was read.");
                                }
                                else
                                {
                                    GXDateTime dt = (GXDateTime)pg.Buffer[0][0];
                                    if (dt.Value != start.Value)
                                    {
                                        AddError(test, dev, output.Errors, "Profile generic " + pg.LogicalName + " failed. ReadRowsByRange returned invalid row when last one was read.");
                                        break;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            AddError(test, dev, output.Errors, "Testing Profile Generic " + pg.LogicalName + " " + ex.Message);
                        }
                    }
                    //Read last row using end index #0.
                    if (settings.ExcludedGuruxTests.ProfileGenericTests.ReadUsingHighestPossibleEntry)
                    {
                        try
                        {
                            if (pg.CaptureObjects[0].Key is GXDLMSClock)
                            {
                                ReadRowsByEntry(dev, pg, entriesInUse, 0, null);
                                if (pg.Buffer.Count != 1)
                                {
                                    AddError(test, dev, output.Errors, "Profile generic " + pg.LogicalName + " failed. ReadRowsByRange returned invalid amount of rows when last one was read using Zero as end index.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            AddError(test, dev, output.Errors, "Testing Profile Generic " + pg.LogicalName + " " + ex.Message);
                        }
                    }
                    //Read after last row.
                    if (settings.ExcludedGuruxTests.ProfileGenericTests.ReadLastRow)
                    {
                        try
                        {
                            if (pg.CaptureObjects[0].Key is GXDLMSClock)
                            {
                                ReadRowsByEntry(dev, pg, entriesInUse, entriesInUse, null);
                                List<object[]> rows = pg.Buffer;
                                GXDateTime start = (GXDateTime)rows[0][0];
                                ReadRowsByRange(dev, pg, start.Value.LocalDateTime.AddHours(1), DateTime.MaxValue, null);
                                if (pg.Buffer.Count != 0)
                                {
                                    AddError(test, dev, output.Errors, "Profile generic " + pg.LogicalName + " failed. ReadRowsByRange returned invalid amount of rows when last one was read.");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            AddError(test, dev, output.Errors, "Testing Profile Generic " + pg.LogicalName + " " + ex.Message);
                        }
                    }
                    //Read only first column.
                    if (settings.ExcludedGuruxTests.ProfileGenericTests.ReadFirstColumn)
                    {
                        if (entriesInUse < 1)
                        {
                            AddInfo(test, dev, output.Info, "Profile generic " + pg.LogicalName + " skipped. Amount ot the rows is too small.");
                        }
                        else
                        {
                            List<GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>> columns = new List<GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>>();
                            columns.Add(pg.CaptureObjects[0]);
                            try
                            {
                                ReadRowsByEntry(dev, pg, 1, 1, columns);
                                if (pg.Buffer.Count != 0 || pg.Buffer[0].Length != 1)
                                {
                                    AddError(test, dev, output.Errors, "Profile generic " + pg.LogicalName + " failed. ReadRowsByEntry returned invalid amount of columns when only first one was read.");
                                }
                            }
                            catch (Exception ex)
                            {
                                AddError(test, dev, output.Errors, "Testing Profile Generic " + pg.LogicalName + " " + ex.Message);
                            }
                            try
                            {
                                if (pg.CaptureObjects[0].Key is GXDLMSClock)
                                {
                                    ReadRowsByEntry(dev, pg, 1, 1, null);
                                    List<object[]> rows = pg.Buffer;
                                    GXDateTime start = (GXDateTime)rows[0][0];
                                    ReadRowsByRange(dev, pg, start.Value.LocalDateTime, start.Value.LocalDateTime.AddHours(1), columns);
                                    if (pg.Buffer.Count != 0)
                                    {
                                        AddError(test, dev, output.Errors, "Profile generic " + pg.LogicalName + " failed. ReadRowsByRange returned invalid amount of columns when only first one was read.");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                AddError(test, dev, output.Errors, "Testing Profile Generic " + pg.LogicalName + " " + ex.Message);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Test current association.
        /// </summary>
        /// <param name="settings">Conformance settings.</param>
        /// <param name="dev">DLMS device.</param>
        /// <param name="output"></param>
        private static void TestAssociationLn(GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output)
        {
            GXDLMSAssociationLogicalName ln = (GXDLMSAssociationLogicalName)dev.Comm.client.Objects.FindByLN(ObjectType.AssociationLogicalName, "0.0.40.0.0.255");
            if (ln == null)
            {
                if (ln == null)
                {
                    GXDLMSObjectCollection objects = dev.Comm.client.Objects.GetObjects(ObjectType.AssociationLogicalName);
                    foreach (GXDLMSAssociationLogicalName it in objects)
                    {
                        if (it.AuthenticationMechanismName.MechanismId == dev.Authentication)
                        {
                            ln = (GXDLMSAssociationLogicalName)it;
                            break;
                        }
                    }
                }
                if (ln == null)
                {
                    return;
                }
            }
            //Read values.
            dev.Comm.ReadValue(ln, 4);
            dev.Comm.ReadValue(ln, 5);
            dev.Comm.ReadValue(ln, 6);
            dev.Comm.ReadValue(ln, 8);
            if (ln.XDLMSContextInfo.DlmsVersionNumber != 6)
            {
                output.Errors.Insert(0, "Invalid DLMS version: " + ln.XDLMSContextInfo.DlmsVersionNumber);
            }

            if (ln.ApplicationContextName.JointIsoCtt != 2 && ln.ApplicationContextName.JointIsoCtt != 96)
            {
                output.Errors.Insert(0, "Wrong ApplicationContextName.JointIsoCtt: " + ln.ApplicationContextName.JointIsoCtt);
            }
            if (ln.ApplicationContextName.Country != 16 && ln.ApplicationContextName.Country != 133)
            {
                output.Errors.Insert(0, "Wrong ApplicationContextName.Country: " + ln.ApplicationContextName.Country);
            }
            if (ln.ApplicationContextName.CountryName != 756 && ln.ApplicationContextName.CountryName != 116)
            {
                output.Errors.Insert(0, "Wrong ApplicationContextName.CountryName: " + ln.ApplicationContextName.CountryName);
            }
            if (ln.ApplicationContextName.IdentifiedOrganization != 5)
            {
                output.Errors.Insert(0, "Wrong ApplicationContextName.IdentifiedOrganization: " + ln.ApplicationContextName.IdentifiedOrganization);
            }
            if (ln.ApplicationContextName.DlmsUA != 8)
            {
                output.Errors.Insert(0, "Wrong ApplicationContextName.DlmsUA: " + ln.ApplicationContextName.DlmsUA);
            }
            if (ln.ApplicationContextName.ApplicationContext != 1)
            {
                output.Errors.Insert(0, "Wrong ApplicationContextName.ApplicationContext: " + ln.ApplicationContextName.ApplicationContext);
            }
            if (dev.Comm.client.UseLogicalNameReferencing)
            {
                if (dev.Comm.client.Ciphering.Security == (byte)Security.None && ln.ApplicationContextName.ContextId != ApplicationContextName.LogicalName)
                {
                    output.Errors.Insert(0, "Wrong ApplicationContextName.ContextId: " + ln.ApplicationContextName.ContextId);
                }
                else if (dev.Comm.client.Ciphering.Security != (byte)Security.None && ln.ApplicationContextName.ContextId != ApplicationContextName.LogicalNameWithCiphering)
                {
                    output.Errors.Insert(0, "Wrong ApplicationContextName.ContextId: " + ln.ApplicationContextName.ContextId);
                }
            }
            else
            {
                if (dev.Comm.client.Ciphering.Security == (byte)Security.None && ln.ApplicationContextName.ContextId != ApplicationContextName.ShortName)
                {
                    output.Errors.Insert(0, "Wrong ApplicationContextName.ContextId: " + ln.ApplicationContextName.ContextId);
                }
                else if (dev.Comm.client.Ciphering.Security != (byte)Security.None && ln.ApplicationContextName.ContextId != ApplicationContextName.ShortNameWithCiphering)
                {
                    output.Errors.Insert(0, "Wrong ApplicationContextName.ContextId: " + ln.ApplicationContextName.ContextId);
                }
            }

            if (ln.AuthenticationMechanismName.JointIsoCtt != 2 && ln.AuthenticationMechanismName.JointIsoCtt != 96)
            {
                output.Errors.Insert(0, "Wrong AuthenticationMechanismName.JointIsoCtt: " + ln.AuthenticationMechanismName.JointIsoCtt);
            }
            if (ln.AuthenticationMechanismName.Country != 16 && ln.AuthenticationMechanismName.Country != 133)
            {
                output.Errors.Insert(0, "Wrong AuthenticationMechanismName.Country: " + ln.AuthenticationMechanismName.Country);
            }
            if (ln.AuthenticationMechanismName.CountryName != 756 && ln.AuthenticationMechanismName.CountryName != 116)
            {
                output.Errors.Insert(0, "Wrong AuthenticationMechanismName.CountryName: " + ln.AuthenticationMechanismName.CountryName);
            }
            if (ln.AuthenticationMechanismName.IdentifiedOrganization != 5)
            {
                output.Errors.Insert(0, "Wrong AuthenticationMechanismName.IdentifiedOrganization: " + ln.AuthenticationMechanismName.IdentifiedOrganization);
            }
            if (ln.AuthenticationMechanismName.DlmsUA != 8)
            {
                output.Errors.Insert(0, "Wrong AuthenticationMechanismName.DlmsUA: " + ln.AuthenticationMechanismName.DlmsUA);
            }
            if (ln.AuthenticationMechanismName.AuthenticationMechanismName != 2)
            {
                output.Errors.Insert(0, "Wrong AuthenticationMechanismName.AuthenticationMechanismName:0 " + ln.AuthenticationMechanismName.AuthenticationMechanismName);
            }
            if (ln.AuthenticationMechanismName.MechanismId != dev.Comm.client.Authentication)
            {
                output.Errors.Insert(0, "Wrong AuthenticationMechanismName.MechanismId: " + ln.AuthenticationMechanismName.MechanismId);
            }

            if (ln.AssociationStatus != Gurux.DLMS.Objects.Enums.AssociationStatus.Associated)
            {
                output.Errors.Insert(0, "Invalid AssociationStatus: " + ln.AssociationStatus);
            }
            if (ln.XDLMSContextInfo.Conformance == Conformance.None)
            {
                output.Errors.Insert(0, "Invalid Conformance: " + ln.XDLMSContextInfo.Conformance);
            }
            if (ln.XDLMSContextInfo.MaxReceivePduSize == 0)
            {
                output.Errors.Insert(0, "Invalid MaxReceivePduSize: " + ln.XDLMSContextInfo.MaxReceivePduSize);
            }
            if (ln.XDLMSContextInfo.MaxSendPduSize == 0)
            {
                output.Errors.Insert(0, "Invalid MaxSendPduSize: " + ln.XDLMSContextInfo.MaxSendPduSize);
            }
            if (ln.ClientSAP != dev.ClientAddress)
            {
                output.Errors.Insert(0, string.Format("Invalid Client SAP. Actual:{0} expected: {1} ", ln.ClientSAP, dev.ClientAddress));
            }
            if (ln.ServerSAP != dev.Comm.client.ServerAddress)
            {
                output.Errors.Insert(0, string.Format("Invalid Server SAP. Actual:{0} expected: {1} ", ln.ServerSAP, dev.Comm.client.ServerAddress));
            }
        }

        /// <summary>
        /// Rerad logical device name and test that meter is returning right command type.
        /// </summary>
        /// <param name="settings">Conformance settings.</param>
        /// <param name="dev">DLMS device.</param>
        /// <param name="output"></param>
        private static void ReadLogicalDeviceName(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output)
        {
            GXReplyData reply = new GXReplyData();
            try
            {
                GXDLMSData ldn = new GXDLMSData("0.0.42.0.0.255");
                byte[] data = dev.Comm.Read(ldn, 2);
                dev.Comm.ReadDataBlock(data, "Read Logical Device Name", 2, reply);
                object v = reply.Value;
                if (v is byte[])
                {
                    v = ASCIIEncoding.ASCII.GetString((byte[])v);
                }
                output.PreInfo.Add("Logical Device Name is: " + Convert.ToString(v + "."));
            }
            catch (Exception)
            {
                AddError(test, dev, output.Errors, "Logical Device Name is not implemented.");
                return;
            }
            try
            {
                if (dev.Comm.client.Ciphering.Security != (byte)Security.None)
                {
                    bool ded = dev.Comm.client.Ciphering.DedicatedKey != null;
                    if ((dev.Comm.client.ConnectionState & ConnectionState.Dlms) == 0 ||
                        (dev.Comm.client.NegotiatedConformance & Conformance.GeneralProtection) != 0)
                    {
                        if (ded)
                        {
                            if (reply.CipheredCommand != Command.GeneralDedCiphering)
                            {
                                throw new Exception("Reply data is not send using general ded ciphering.");
                            }
                        }
                        else
                        {
                            if (reply.CipheredCommand != Command.GeneralGloCiphering)
                            {
                                throw new Exception("Reply data is not send using general glo ciphering.");
                            }
                        }
                    }
                    else
                    {
                        if (ded)
                        {
                            if (reply.CipheredCommand != Command.DedGetResponse)
                            {
                                throw new Exception("Reply data is not send using Ded get response.");
                            }
                        }
                        else
                        {
                            if (reply.CipheredCommand != Command.GloGetResponse)
                            {
                                throw new Exception("Reply data is not send using Glo get response.");
                            }
                        }
                    }
                }
            }
            catch (GXDLMSException)
            {
                output.Errors.Insert(0, "Login failed after wrong password.");
            }
        }

        public static void AddError(GXConformanceTest test, GXDLMSDevice dev, List<string> list, string str)
        {
            if (dev != null && dev.OnTrace != null)
            {
                dev.OnTrace(DateTime.Now, dev, str, null, 0, dev.Comm.LogFile, 0);
            }
            test.OnTrace(test, str);
            list.Add(str);
        }

        public static void AddInfo(GXConformanceTest test, GXDLMSDevice dev, List<string> list, string str)
        {
            if (dev != null && dev.OnTrace != null)
            {
                dev.OnTrace(DateTime.Now, dev, str, null, 0, dev.Comm.LogFile, 0);
            }
            list.Add(str);
        }

        /// <summary>
        /// Test that meter can handle invalid password.
        /// </summary>
        /// <param name="settings">Conformance settings.</param>
        /// <param name="dev">DLMS device.</param>
        /// <param name="output"></param>
        private static void TestInvalidPassword(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output)
        {
            if (!string.IsNullOrEmpty(settings.ExcludedGuruxTests.AuthenticationTests.InvalidPassword) && dev.Comm.client.Authentication != Authentication.None)
            {
                dev.Comm.Disconnect();
                string pw = dev.Password;
                byte[] hpw = dev.HexPassword;
                dev.Password = CryptHelper.Encrypt(settings.ExcludedGuruxTests.AuthenticationTests.InvalidPassword, Password.Key);
                dev.HexPassword = null;
                try
                {
                    Thread.Sleep((int)settings.DelayConnection.TotalMilliseconds);
                    dev.InitializeConnection();
                    output.Errors.Insert(0, "Login succeeded with wrong password.");
                    dev.Comm.Disconnect();
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode != 0)
                    {
                        AddInfo(test, dev, output.Info, "Invalid password test succeeded. Meter returned " + ex.Message);
                    }
                    else
                    {
                        AddError(test, dev, output.Errors, "Invalid password test failed. Meter didn't return an error.");
                    }
                    dev.Comm.Disconnect();
                }
                //Try to connect again.
                dev.Password = pw;
                dev.HexPassword = hpw;
                try
                {
                    Thread.Sleep((int)settings.DelayConnection.TotalMilliseconds);
                    dev.InitializeConnection();
                }
                catch (GXDLMSException)
                {
                    output.Errors.Insert(0, "Login failed after wrong password.");
                }
            }
        }

        private static void TestClock(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output)
        {
            GXDLMSObjectCollection objects = dev.Comm.client.Objects.GetObjects(ObjectType.Clock);
            foreach (GXDLMSClock it in objects)
            {
                //If settings time and time zone is allowed.
                if (it.GetAccess(2) == AccessMode.ReadWrite && it.GetAccess(3) == AccessMode.ReadWrite)
                {
                    //Read old values.
                    dev.Comm.ReadValue(it, 3);
                    dev.Comm.ReadValue(it, 2);
                    DateTime time = DateTime.Now;
                    int timeZone = it.TimeZone;
                    try
                    {
                        //Update new time using current time.
                        GXDateTime newTime = new GXDateTime(DateTime.Now);
                        it.Time = newTime;
                        DateTime start = DateTime.Now;
                        if (!settings.ExcludedGuruxTests.ClockTests.SetLocalTime)
                        {
                            try
                            {
                                if (dev.Comm.client.UseUtc2NormalTime)
                                {
                                    it.TimeZone = (int)TimeZoneInfo.Local.BaseUtcOffset.TotalMinutes;
                                }
                                else
                                {
                                    it.TimeZone = -(int)TimeZoneInfo.Local.BaseUtcOffset.TotalMinutes;
                                }
                                AddInfo(test, dev, output.Info, "Set new Time zone:" + it.TimeZone);
                                dev.Comm.Write(it, 3);
                                dev.Comm.Write(it, 2);
                                newTime.Skip |= DateTimeSkips.Second;
                                dev.Comm.ReadValue(it, 2);
                                time = it.Time;
                                if (newTime.Compare(it.Time.Value.Add(DateTime.Now - start).LocalDateTime) != 0)
                                {
                                    AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock1\">Clock test #1 failed</a>. Failed to set new time using current time zone. Expected: " + newTime + " Actual: " + it.Time.Value.Add(DateTime.Now - start).LocalDateTime);
                                    start = time = DateTime.Now;
                                }
                                else
                                {
                                    AddInfo(test, dev, output.Info, "Setting new time succeeded using current time zone.");
                                }
                            }
                            catch (Exception ex)
                            {
                                AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock1\">Clock test #1 failed</a>. Failed to set new time using current time zone. Meter returns exception. " + ex.Message);
                                start = time = DateTime.Now;
                            }
                        }
                        if (!settings.ExcludedGuruxTests.ClockTests.SetEpochTime)
                        {
                            //Update new time using UTC time.
                            newTime = new GXDateTime(DateTime.Now.ToUniversalTime());
                            newTime.Skip |= DateTimeSkips.Second;
                            it.Time = newTime;
                            try
                            {
                                dev.Comm.Write(it, 2);
                                dev.Comm.ReadValue(it, 2);
                                if (newTime.Compare(it.Time.Value.Add(DateTime.Now - start).LocalDateTime) != 0)
                                {
                                    AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock2\">Clock test #2 failed</a>. Failed to set new time using UTC time. Expected: " + newTime + " Actual: " + it.Time.Value.Add(DateTime.Now - start).LocalDateTime);
                                }
                                else
                                {
                                    AddInfo(test, dev, output.Info, "Setting new time succeeded using UTC time zone.");
                                }
                            }
                            catch (Exception ex)
                            {
                                AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock2\">Clock test #2 failed</a>. Failed to set new time using UTC time zone. Meter returns exception. " + ex.Message);
                            }
                        }

                        if (!settings.ExcludedGuruxTests.ClockTests.SetTimeWithoutTimeZone)
                        {
                            //Update new time without timezone.
                            it.Time.Skip |= DateTimeSkips.Deviation;
                            dev.Comm.Write(it, 2);
                            dev.Comm.ReadValue(it, 2);
                            if (newTime.Compare(it.Time.Value.Add(DateTime.Now - start).DateTime) != 0)
                            {
                                AddError(test, dev, output.Errors, "Failed to set new time using without time zone. Expected: " + newTime + " Actual: " + it.Time.Value.Add(DateTime.Now - start).DateTime);
                            }
                        }

                        //Check DST.
                        if (it.GetAccess(8) == AccessMode.ReadWrite && (it.GetAccess(7) & AccessMode.Read) != 0)
                        {
                            dev.Comm.ReadValue(it, 8);
                            dev.Comm.ReadValue(it, 7);
                            bool dst = it.Enabled;
                            if (!settings.ExcludedGuruxTests.ClockTests.FlipDST)
                            {
                                int deviation = it.Deviation;
                                if (dst)
                                {
                                    AddInfo(test, dev, output.Info, "DST is in use and deviation is " + deviation + ".");
                                }
                                else
                                {
                                    AddInfo(test, dev, output.Info, "DST is not in use. Devitation is " + deviation + ".");
                                }
                                if (deviation != 0)
                                {
                                    //Flip DST.
                                    it.Enabled = !dst;
                                    try
                                    {
                                        dev.Comm.Write(it, 8);
                                    }
                                    catch (Exception)
                                    {
                                        AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock3\">Clock test #3 failed</a>. Clock test failed. Failed to enable DST.");
                                    }
                                    //Read time.
                                    dev.Comm.ReadValue(it, 2);
                                    GXDateTime tmp = new GXDateTime(it.Time);
                                    tmp.Skip |= DateTimeSkips.Second;
                                    if (tmp.Compare(time.Add(DateTime.Now - start)) != 0)
                                    {
                                        //Setting current time
                                        AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock3\">Clock test #3 failed</a>. Clock test failed. Time is not valid if DST is changed. Expected: " + time.Add(DateTime.Now - start) + " Actual: " + tmp);
                                    }
                                    else
                                    {
                                        AddInfo(test, dev, output.Info, "Meter can change DST and time is updated correctly.");
                                    }
                                    //Enable DST back.
                                    if (!it.Enabled)
                                    {
                                        it.Enabled = dst;
                                        try
                                        {
                                            dev.Comm.Write(it, 8);
                                        }
                                        catch (Exception)
                                        {
                                            AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock3\">Clock test #3 failed</a>. Clock test failed. Failed to set DST.");
                                        }
                                    }
                                }
                            }
                            //Change time and check is DST flag set.
                            if ((it.GetAccess(5) & AccessMode.Read) != 0 && (it.GetAccess(6) & AccessMode.Read) != 0)
                            {
                                if (!settings.ExcludedGuruxTests.ClockTests.CheckDST)
                                {
                                    dev.Comm.ReadValue(it, 4);
                                    dev.Comm.ReadValue(it, 5);
                                    dev.Comm.ReadValue(it, 6);
                                    GXDateTime begin = it.Begin;
                                    GXDateTime end = it.End;
                                    bool dst1;
                                    //Read time.
                                    dev.Comm.ReadValue(it, 2);
                                    GXDateTime tmp = new GXDateTime(it.Time);
                                    if (begin.Compare(tmp) != 1 && end.Compare(tmp) != -1)
                                    {
                                        AddInfo(test, dev, output.Info, "Meter is in DST time.");
                                        if ((it.Status & ClockStatus.DaylightSavingActive) == 0)
                                        {
                                            AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock4\">Clock test #4 failed</a>. Meter is in DST, but DST status flag is not set.");
                                        }
                                        //Move meter to normal time.
                                        it.Time = new GXDateTime(end.Value.AddDays(7));
                                        dst1 = false;
                                    }
                                    else
                                    {
                                        AddInfo(test, dev, output.Info, "Meter is in normal time.");
                                        if ((it.Status & ClockStatus.DaylightSavingActive) != 0)
                                        {
                                            AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock4\">Clock test #4 failed</a>. Meter is in normal time, but DST status flag is set.");
                                        }
                                        //Move meter to DST time.
                                        it.Time = new GXDateTime(begin.Value.AddDays(7));
                                        dst1 = true;
                                    }
                                    //Write new time.
                                    dev.Comm.Write(it, 2);
                                    //Check that clock status is changed.
                                    dev.Comm.ReadValue(it, 4);
                                    if (((it.Status & ClockStatus.DaylightSavingActive) != 0 && !dst1) ||
                                         ((it.Status & ClockStatus.DaylightSavingActive) == 0 && dst1))
                                    {
                                        if (dst1)
                                        {
                                            AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock4\">Clock test #4 failed</a>. Meter is in DST, but DST status flag is not set.");
                                        }
                                        else
                                        {
                                            AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock4\">Clock test #4 failed</a>. Meter is in normal time, but DST status flag is set.");
                                        }
                                    }
                                    else
                                    {
                                        if (dst1)
                                        {
                                            AddInfo(test, dev, output.Info, "Time is changed to DST time, and DST status flag is set.");
                                        }
                                        else
                                        {
                                            AddInfo(test, dev, output.Info, "Time is changed to normal time and DST status flag is not set.");
                                        }
                                    }
                                    //Move meter to current time.
                                    it.Time = new GXDateTime(time.Add(DateTime.Now - start));
                                    dev.Comm.Write(it, 2);
                                }
                                else
                                {
                                    AddInfo(test, dev, output.Info, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock4\">Clock test #4 is not tested</a>. Changind DST begin and end time is not tested.");
                                }
                                //Return DST back.
                                it.Enabled = dst;
                                try
                                {
                                    dev.Comm.Write(it, 8);
                                }
                                catch (Exception)
                                {
                                    AddError(test, dev, output.Errors, "Clock test failed. Failed to set DST.");
                                }
                            }
                        }
                        //Change time zone to UTC.
                        if (!settings.ExcludedGuruxTests.ClockTests.ChangeTimeZone)
                        {
                            if (it.TimeZone != 0)
                            {
                                AddInfo(test, dev, output.Info, "Time zone of the meter:" + it.TimeZone);
                                it.TimeZone = 0;
                                dev.Comm.Write(it, 3);
                                //Read time.
                                dev.Comm.ReadValue(it, 2);
                                GXDateTime tmp = new GXDateTime(it.Time);
                                tmp.Skip |= DateTimeSkips.Second;
                                if (tmp.Compare(time.Add(DateTime.Now - start)) != 0)
                                {
                                    //Setting UTC time
                                    AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock5\">Clock test #5 failed</a>. Clock test failed. Time is not valid if time zone is changed to UTC. Expected: " + time.Add(DateTime.Now - start) + " Actual: " + tmp);
                                }
                                else
                                {
                                    AddInfo(test, dev, output.Info, "Meter can change time zone to UTC and time is updated correctly.");
                                }
                            }
                            else
                            {
                                it.TimeZone = (int)TimeZoneInfo.Utc.GetUtcOffset(DateTime.Now).TotalMinutes;
                                AddInfo(test, dev, output.Info, "Time zone of the meter is UTC. Try to set it to " + it.TimeZone);
                                dev.Comm.Write(it, 3);
                                //Read time.
                                dev.Comm.ReadValue(it, 2);
                                GXDateTime tmp = new GXDateTime(it.Time);
                                tmp.Skip |= DateTimeSkips.Second;
                                if (tmp.Compare(time.Add(DateTime.Now - start)) != 0)
                                {
                                    //Setting current time
                                    AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock5\">Clock test #5 failed</a>.  Clock test failed. Time is not valid if time zone is changed from UTC. Expected: " + time.Add(DateTime.Now - start) + " Actual: " + tmp);
                                }
                                else
                                {
                                    AddInfo(test, dev, output.Info, "Meter can change time zone from UTC and time is updated correctly.");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock1\">Clock test failed</a>. " + ex.Message);
                    }
                    it.TimeZone = timeZone;
                    dev.Comm.Write(it, 3);
                }
                else
                {
                    AddInfo(test, dev, output.Info, "Clock time access for " + it.LogicalName + " is " + it.GetAccess(2));
                    AddInfo(test, dev, output.Info, "Clock time zone access for " + it.LogicalName + " is " + it.GetAccess(3));
                }
            }
        }

        /// <summary>
        /// Test image transfer.
        /// </summary>
        /// <param name="settings">Conformance settings.</param>
        /// <param name="dev">DLMS device.</param>
        /// <param name="output"></param>
        private static void TestImageTransfer(GXConformanceSettings settings, GXConformanceTest test, GXDLMSDevice dev, GXOutput output)
        {
            if (!string.IsNullOrEmpty(settings.ImageFile) && settings.ImageIdentifier != null && settings.ImageIdentifier.Length != 0)
            {
                GXDLMSObjectCollection objects = dev.Comm.client.Objects.GetObjects(ObjectType.ImageTransfer);
                if (objects.Count != 0)
                {
                    output.PreInfo.Add("Testing Image transfer.");
                    GXDLMSImageTransfer img = (GXDLMSImageTransfer)objects[0];
                    dev.Comm.ReadValue(img, 5);
                    if (!img.ImageTransferEnabled)
                    {
                        output.Errors.Insert(0, "Image transfer is not enabled.");
                    }
                    else
                    {
                        //Step 1. BB: 4.4.6.4
                        dev.Comm.ReadValue(img, 2);
                        AddInfo(test, dev, output.Info, "Image block size is " + img.ImageBlockSize + " bytes.");
                        byte[] image = null;
                        if (string.Compare(Path.GetExtension(settings.ImageFile), ".xml", true) == 0)
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(settings.ImageFile);
                            GXImageDlg.GetImage(doc.ChildNodes, ref image);
                        }
                        else
                        {
                            image = File.ReadAllBytes(settings.ImageFile);
                        }
                        if (!Continue)
                        {
                            return;
                        }
                        //Step 2. BB: 4.4.6.4
                        bool error = false;
                        GXReplyData reply = new GXReplyData();
                        dev.Comm.ReadDataBlock(img.ImageTransferInitiate(dev.Comm.client, settings.ImageIdentifier, image.Length), "", 1, reply);
                        reply.Clear();
                        int cnt1 = 0;
                        do
                        {
                            dev.Comm.ReadValue(img, 6);
                            if (++cnt1 > 10)
                            {
                                throw new Exception("Failed to read Image transfer status after image transfer initiate.");
                            }
                            if (img.ImageTransferStatus == ImageTransferStatus.NotInitiated)
                            {
                                Thread.Sleep(2000);
                            }
                        } while (img.ImageTransferStatus == ImageTransferStatus.NotInitiated);
                        //Check ImageTransferredBlocksStatus.
                        dev.Comm.ReadValue(img, 3);
                        if (img.ImageTransferredBlocksStatus != null)
                        {
                            foreach (char it in img.ImageTransferredBlocksStatus)
                            {
                                if (it != '0')
                                {
                                    error = true;
                                    AddError(test, dev, output.Errors, "Image transferred blocks status is wrong. It's " + img.ImageTransferredBlocksStatus + " and it shoud be zilled with 0.");
                                }
                            }
                        }

                        //Check ImageTransferStatus.
                        if (!Continue)
                        {
                            return;
                        }
                        if (img.ImageTransferStatus != ImageTransferStatus.TransferInitiated)
                        {
                            error = true;
                            AddError(test, dev, output.Errors, "Image transfer status is wrong. It's " + img.ImageTransferStatus + " and it shoud be TransferInitiated.");
                        }
                        //Check ImageFirstNotTransferredBlockNumber.
                        if (!Continue)
                        {
                            return;
                        }
                        dev.Comm.ReadValue(img, 4);
                        if (img.ImageFirstNotTransferredBlockNumber != 0)
                        {
                            error = true;
                            AddError(test, dev, output.Errors, "Image first not transferred block number wrong. It's " + img.ImageFirstNotTransferredBlockNumber + " and it shoud be 0.");
                        }
                        //Check ImageActivateInfo.
                        if (!Continue)
                        {
                            return;
                        }
                        dev.Comm.ReadValue(img, 7);
                        if (img.ImageActivateInfo != null && img.ImageActivateInfo.Length != 0)
                        {
                            error = true;
                            AddError(test, dev, output.Errors, "Image activate info is not reset.");
                        }
                        if (!error)
                        {
                            AddInfo(test, dev, output.Info, "Image activation Step 2 succeeded.");
                        }
                        //Step 3. BB: 4.4.6.4
                        DateTime start = DateTime.Now;
                        error = false;
                        byte[][] blocks = img.GetImageBlocks(image);
                        int pos = 0, cnt = blocks.Length;
                        reply.Clear();
                        foreach (byte[] b in blocks)
                        {
                            if (!Continue)
                            {
                                return;
                            }
                            try
                            {
                                dev.Comm.ReadDataBlock(dev.Comm.client.Method(img, 2, b, DataType.Array), "", 1, reply);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Image block transfer failed. " + ex.Message);
                            }
                            if (reply.Error != 0)
                            {
                                output.Errors.Insert(0, "Image transfer failed. Error code: " + reply.Error);
                                return;
                            }
                            reply.Clear();
                            test.OnProgress(test, "Image block transfer...", ++pos, cnt);
                        }
                        if (!error)
                        {
                            AddInfo(test, dev, output.Info, "Image transfer (Step 3) succeeded.");
                            AddInfo(test, dev, output.Info, "Image transfer takes " + (DateTime.Now - start).ToString(@"hh\:mm\:ss"));
                        }
                        //Step 4. BB: 4.4.6.4
                        error = false;
                        test.OnProgress(test, "Checking completeness of the Image...", 1, 1);
                        //Check ImageFirstNotTransferredBlockNumber.
                        if (!Continue)
                        {
                            return;
                        }
                        dev.Comm.ReadValue(img, 4);
                        if (img.ImageFirstNotTransferredBlockNumber != blocks.Length)
                        {
                            if (img.ImageFirstNotTransferredBlockNumber > blocks.Length)
                            {
                                output.Warnings.Add("Image first not transferred block number wrong. It's " + img.ImageFirstNotTransferredBlockNumber + " and it shoud be " + blocks.Length + ".");
                            }
                            else
                            {
                                error = true;
                                AddError(test, dev, output.Errors, "Image first not transferred block number wrong. It's " + img.ImageFirstNotTransferredBlockNumber + " and it shoud be " + blocks.Length + ".");
                            }
                        }
                        //Check ImageTransferredBlocksStatus.
                        if (!Continue)
                        {
                            return;
                        }
                        dev.Comm.ReadValue(img, 3);
                        if (img.ImageTransferredBlocksStatus == null)
                        {
                            error = true;
                            AddError(test, dev, output.Errors, "Image Transferred blocks status is not implemented.");
                        }
                        else
                        {
                            if (img.ImageTransferredBlocksStatus.Length != blocks.Length)
                            {
                                if (img.ImageTransferredBlocksStatus.Length > blocks.Length)
                                {
                                    output.Warnings.Add("Image Transferred blocks status is wrong. Amount of bits is different than block size. (" + img.ImageTransferredBlocksStatus.Length + "/" + blocks.Length + ")");
                                }
                                else
                                {
                                    error = true;
                                    AddError(test, dev, output.Errors, "Image Transferred blocks status is wrong. Amount of bits is different than block size. (" + img.ImageTransferredBlocksStatus.Length + "/" + blocks.Length + ")");
                                }
                            }
                            foreach (char it in img.ImageTransferredBlocksStatus)
                            {
                                if (it != '1')
                                {
                                    error = true;
                                    AddError(test, dev, output.Errors, "Image transferred blocks status is not set.");
                                    break;
                                }
                            }
                        }
                        if (!error)
                        {
                            AddInfo(test, dev, output.Info, "Image completeness Step 4 succeeded.");
                        }

                        if (settings.ImageVerify)
                        {
                            if (!Continue)
                            {
                                return;
                            }
                            start = DateTime.Now;
                            //Step 5. BB: 4.4.6.4
                            reply.Clear();
                            test.OnProgress(test, "Verifying image...", 1, 1);
                            do
                            {
                                dev.Comm.ReadValue(img, 6);
                                if (img.ImageTransferStatus != ImageTransferStatus.TransferInitiated)
                                {
                                    test.OnProgress(test, "Transfering image on progress. Waiting...", 1, 1);
                                    Thread.Sleep((int)settings.ImageVerifyWaitTime.TotalMilliseconds);
                                }
                            }
                            while (img.ImageTransferStatus != ImageTransferStatus.TransferInitiated);
                            try
                            {
                                dev.Comm.ReadDataBlock(img.ImageVerify(dev.Comm.client), "", 1, reply);
                            }
                            catch (GXDLMSException ex)
                            {
                                reply.Error = (short)ex.ErrorCode;
                            }
                            if (reply.Error == (short)ErrorCode.TemporaryFailure)
                            {
                                test.OnProgress(test, "Check is image verify ready...", 1, 1);
                                dev.Comm.ReadValue(img, 6);
                                if (img.ImageTransferStatus == ImageTransferStatus.VerificationInitiated)
                                {
                                    test.OnProgress(test, "Still verifying...", 1, 1);
                                }
                                Thread.Sleep((int)settings.ImageVerifyWaitTime.TotalMilliseconds);
                            }
                            else if (reply.Error != 0)
                            {
                                output.Errors.Insert(0, "Image verification failed. Error code: " + reply.Error);
                                return;
                            }
                            //Wait until image is verified.
                            do
                            {
                                dev.Comm.ReadValue(img, 6);
                                if (img.ImageTransferStatus == ImageTransferStatus.VerificationFailed)
                                {
                                    AddError(test, dev, output.Errors, "Image verification failed.");
                                    return;
                                }
                                if (img.ImageTransferStatus != ImageTransferStatus.VerificationSuccessful)
                                {
                                    test.OnProgress(test, "Image verification is on progress. Waiting...", 1, 1);
                                    Thread.Sleep((int)settings.ImageVerifyWaitTime.TotalMilliseconds);
                                }
                            } while ((img.ImageTransferStatus != ImageTransferStatus.VerificationSuccessful));
                        }
                        AddInfo(test, dev, output.Info, "Image verify succeeded (Step 5).");
                        AddInfo(test, dev, output.Info, "Verify takes " + (DateTime.Now - start).ToString(@"hh\:mm\:ss"));

                        reply.Clear();
                        if (settings.ImageActivate)
                        {
                            if (!Continue)
                            {
                                return;
                            }
                            start = DateTime.Now;
                            //Step 7. BB: 4.4.6.4
                            test.OnProgress(test, "Activating image...", 1, 1);
                            try
                            {
                                reply.Clear();
                                dev.Comm.ReadDataBlock(img.ImageActivate(dev.Comm.client), "", 1, reply);
                            }
                            catch (GXDLMSException ex)
                            {
                                reply.Error = (short)ex.ErrorCode;
                            }
                            //Image activate is not checked if image activate timeout is zero.
                            if (settings.ImageActivateWaitTime.TotalSeconds == 0)
                            {
                                //Disconnect is not send because meter is restarted and not answering.
                                dev.Comm.media.Close();
                            }
                            else
                            {
                                if (reply.Error == (short)ErrorCode.TemporaryFailure)
                                {
                                    test.OnProgress(test, "Check is image activation ready...", 1, 1);
                                    dev.Comm.ReadValue(img, 6);
                                    if (img.ImageTransferStatus == ImageTransferStatus.ActivationInitiated)
                                    {
                                        test.OnProgress(test, "Still activating...", 1, 1);
                                    }
                                    Thread.Sleep((int)settings.ImageActivateWaitTime.TotalMilliseconds);
                                }
                                else if (reply.Error != 0)
                                {
                                    output.Errors.Insert(0, "Image activation failed. Error code: " + reply.Error);
                                    return;
                                }
                                //Wait until image is activated.
                                do
                                {
                                    try
                                    {
                                        dev.Comm.ReadValue(img, 6);
                                        if (img.ImageTransferStatus == ImageTransferStatus.ActivationFailed)
                                        {
                                            throw new Exception("Image activation failed.");
                                        }
                                        if (img.ImageTransferStatus != ImageTransferStatus.ActivationSuccessful)
                                        {
                                            test.OnProgress(test, "Image activation is on progress. Waiting...", 1, 1);
                                            Thread.Sleep((int)settings.ImageVerifyWaitTime.TotalMilliseconds);
                                        }
                                    }
                                    catch (TimeoutException)
                                    {
                                        Thread.Sleep((int)settings.ImageVerifyWaitTime.TotalMilliseconds);
                                        dev.Comm.UpdateSettings();
                                        dev.Comm.InitializeConnection(false);
                                        dev.Comm.ReadValue(img, 6);
                                    }
                                } while ((img.ImageTransferStatus != ImageTransferStatus.ActivationSuccessful));
                            }
                            AddInfo(test, dev, output.Info, "Image activation succeeded (Step 6).");
                            AddInfo(test, dev, output.Info, "Activation takes " + (DateTime.Now - start).ToString(@"hh\:mm\:ss"));

                        }
                    }
                }
            }
        }
    }

    class GXConformanceParameters
    {
        /// <summary>
        /// Number ofTasks
        /// </summary>
        public int numberOfTasks;

        //Called when last talk is executed.
        public ManualResetEvent finished = new ManualResetEvent(false);
    }
}
