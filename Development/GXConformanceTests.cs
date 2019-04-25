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
            return Directory.GetFiles(settings.ExternalTests, "*.xml");
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
            GXDLMSXmlClient client = new GXDLMSXmlClient(TranslatorOutputType.SimpleXml);
            string[] list = GetExternalTests(settings);
            foreach (string it in list)
            {
                try
                {
                    client.Load(it);
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
                                output.Errors.Add("<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + indexStr + index + " failed: <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.ErrorCodes?" + e + ">" + e + "</a>)");
                                test.OnTrace(test, e + "\r\n");
                            }
                            else
                            {
                                output.Errors.Add("<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + indexStr + index + " <div class=\"tooltip\">failed:" + ex.Message);
                                output.Errors.Add("<span class=\"tooltiptext\">");
                                output.Errors.Add(ex.ToString());
                                output.Errors.Add("</span></div>");
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
                        output.Errors.Add("<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + indexStr + index + " <div class=\"tooltip\">failed:" + ex.Message);
                        output.Errors.Add("<span class=\"tooltiptext\">");
                        output.Errors.Add(ex.ToString());
                        output.Errors.Add("</span></div>");
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
                                output.Errors.Add(err);
                            }
                        }
                        else
                        {
                            if (lastExternalException != null)
                            {
                                ErrorCode e = (ErrorCode)lastExternalException.ErrorCode;
                                output.Errors.Add("<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + indexStr + index + " failed: <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.ErrorCodes?" + e + ">" + e + "</a>)");
                                test.OnTrace(test, e + "\r\n");
                                lastExternalException = null;
                            }
                            else
                            {
                                output.Errors.Add(" <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + " " + indexStr + " " + index + " is <div class=\"tooltip\">invalid.");
                                output.Errors.Add("<span class=\"tooltiptext\">");
                                output.Errors.Add("Expected:</b><br/>");
                                output.Errors.Add(it.PduAsXml.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br/>"));
                                output.Errors.Add("<br/><b>Actual:</b><br/>");
                                output.Errors.Add(reply.ToString().Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br/>"));
                                output.Errors.Add("</span></div>");
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
                                output.Errors.Add("<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + indexStr + index + " <div class=\"tooltip\">failed:" + ex.Message);
                                output.Errors.Add("<span class=\"tooltiptext\">");
                                output.Errors.Add(ex.ToString());
                                output.Errors.Add("</span></div>");
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
                                str = GXDLMSClient.ChangeType((byte[])value, dt, test.Device.Comm.client.UtcTimeZone).ToString();
                            }
                            else
                            {
                                str = GXCommon.ToHex((byte[])value);
                            }
                        }
                        else if (value is Object[])
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
                output.Errors.Add("<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + " failed: <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.ErrorCodes?" + e + ">" + e + "</a>)");
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
                output.Info.Add(sb.ToString());
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
            if (sender.Comm.client.InterfaceType == InterfaceType.HDLC && payload > sender.Comm.client.Limits.MaxInfoRX)
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
                    dev.Comm.client = new GXDLMSXmlClient(TranslatorOutputType.SimpleXml);
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
                    if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                    {
                        dev.Comm.UpdateSettings();
                        dev.Comm.InitializeConnection(false);
                        HdlcTests(test, settings, dev, output, dev.ResendCount);
                        if (!Continue)
                        {
                            continue;
                        }
                    }
                    //Application tests.
                    if ((dev.Comm.client.ConnectionState & ConnectionState.Dlms) == 0)
                    {
                        dev.Comm.UpdateSettings();
                        dev.Comm.InitializeConnection(false);
                    }
                    CosemApplicationLayerTests(test, settings, dev, output);
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
                    if (dev.Comm.client.InterfaceType == InterfaceType.HDLC && !dev.Comm.client.Limits.UseFrameSize)
                    {
                        if (dev.MaxInfoRX != 128 && dev.MaxInfoRX != dev.Comm.client.Limits.MaxInfoRX)
                        {
                            output.Warnings.Add("Client asked that RX PDU size is " + dev.MaxInfoRX + ". Meter uses " + dev.Comm.client.Limits.MaxInfoRX);
                        }
                        if (dev.MaxInfoTX != 128 && dev.MaxInfoTX != dev.Comm.client.Limits.MaxInfoTX)
                        {
                            output.Warnings.Add("Client asked that TX PDU size is " + dev.MaxInfoTX + ". Meter uses " + dev.Comm.client.Limits.MaxInfoTX);
                        }
                        if (dev.PduSize < dev.Comm.client.MaxReceivePDUSize)
                        {
                            output.Warnings.Add("Client asked that PDU size is " + dev.PduSize + ". Meter uses " + dev.Comm.client.MaxReceivePDUSize);
                        }
                    }
                    int maxframesize = dev.Comm.client.Limits.MaxInfoRX;
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
                                output.Errors.Add("Invalid OBIS code " + it.LogicalName + " for <a target=\"_blank\" href=https://www.gurux.fi/" + it.GetType().FullName + ">" + it.ObjectType + "</a>.");
                                Console.WriteLine("------------------------------------------------------------");
                                Console.WriteLine(it.LogicalName + ": Invalid OBIS code.");
                            }
                        }
                    }
                    if (!settings.ExcludeMeterInfo)
                    {
                        ReadLogicalDeviceName(settings, dev, output);
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
                            output.Info.Add("Firmware version is not available.");
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
                    List<KeyValuePair<string, List<GXDLMSXmlPdu>>> externalTests = new List<KeyValuePair<string, List<GXDLMSXmlPdu>>>();
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
                                    using (StreamReader fs = File.OpenText(it))
                                    {
                                        externalTests.Add(new KeyValuePair<string, List<GXDLMSXmlPdu>>(it, client.Load(fs, ls)));
                                        fs.Close();
                                    }
                                    File.Copy(it, Path.Combine(dir, Path.GetFileName(it)));
                                }
                                catch (Exception e)
                                {
                                    string errStr = "Failed to load exteranal test " + it + ". " + e.Message;
                                    output.Errors.Add(errStr);
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
                        foreach (KeyValuePair<string, List<GXDLMSXmlPdu>> it in externalTests)
                        {
                            if (!Continue)
                            {
                                break;
                            }
                            try
                            {
                                test.OnProgress(test, "Testing " + it.Key, ++i, cnt);
                                Execute(converter, test, it.Key, it.Value, output, settings, true);
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
                                                output.Errors.Add("Write <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + " attribute " + index + " is <div class=\"tooltip\">failed.");
                                                output.Errors.Add("<span class=\"tooltiptext\">");
                                                output.Errors.Add("Expected:</b><br/>");
                                                output.Errors.Add(Convert.ToString(expected).Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br/>"));
                                                output.Errors.Add("<br/><b>Actual:</b><br/>");
                                                output.Errors.Add(Convert.ToString(actual).Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br/>"));
                                                output.Errors.Add("</span></div>");
                                            }
                                            else
                                            {
                                                output.Info.Add("Write" + ot + " " + ln + " attribute " + index + " Succeeded.");
                                            }
                                        }
                                        catch (GXDLMSException ex)
                                        {
                                            if (ex.ErrorCode != 0)
                                            {
                                                ErrorCode e = (ErrorCode)ex.ErrorCode;
                                                output.Errors.Add("<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + " attribute " + index + " failed: <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.ErrorCodes?" + e + ">" + e + "</a>)");
                                            }
                                            else
                                            {
                                                output.Errors.Add("<a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + " attribute " + index + " <div class=\"tooltip\">failed:" + ex.Message);
                                                output.Errors.Add("<span class=\"tooltiptext\">");
                                                output.Errors.Add(ex.ToString().Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br/>"));
                                                output.Errors.Add("</span></div>");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            output.Errors.Add("Write <a target=\"_blank\" href=https://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + " attribute " + index + " <div class=\"tooltip\">failed. " + ex.Message);
                                            output.Errors.Add("<span class=\"tooltiptext\">");
                                            output.Errors.Add(ex.ToString().Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br/>"));
                                            output.Errors.Add("</span></div>");
                                        }
                                    }
                                }
                            }
                        }

                        if (!settings.ExcludeBasicTests)
                        {
                            if ((dev.Comm.client.NegotiatedConformance & Conformance.MultipleReferences) == 0)
                            {
                                output.Info.Add("Meter don't support multiple references.");
                            }
                            else
                            {
                                GXDLMSObjectCollection objs = dev.Objects.GetObjects(ObjectType.Data);
                                if (objs.Count != 0)
                                {
                                    output.Info.Add("Testing multiple references support using " + objs.Count + " data object(s).");
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
                                    output.Info.Add("Multiple references support succeeded.");
                                }
                            }
                        }
                        //Test invalid password.
                        try
                        {
                            TestInvalidPassword(settings, dev, output);
                        }
                        catch (Exception ex)
                        {
                            output.Errors.Add(ex.Message);
                            test.OnError(test, ex);
                        }
                        try
                        {
                            TestImageTransfer(settings, test, dev, output);
                        }
                        catch (Exception ex)
                        {
                            output.Errors.Add(ex.Message);
                            test.OnError(test, ex);
                        }
                    }
                    if (!settings.ExcludeBasicTests)
                    {
                        TestAssociationLn(settings, dev, output);
                    }
#if DEBUG
                    if (!settings.ExcludeClockTests)
                    {
                        TestClock(settings, dev, output);
                    }
#endif //DEBUG

                    if (dev.Comm.payload != 0)
                    {
                        //TODO:   output.Errors.Insert(0, "HDLC payload size is is too high. There are " + dev.Comm.payload + " bytes. Max size is " + dev.Comm.client.Limits.MaxInfoRX + " bytes.");
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
                    output.Errors.Add(ex.Message);
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
            //Read values if not read yet.
            if (settings.ExcludeBasicTests)
            {
                dev.Comm.ReadValue(ln, 4);
                dev.Comm.ReadValue(ln, 5);
                dev.Comm.ReadValue(ln, 6);
                dev.Comm.ReadValue(ln, 8);
            }
            if (ln.XDLMSContextInfo.DlmsVersionNumber != 6)
            {
                output.Errors.Insert(0, "Invalid DLMS version: " + ln.ApplicationContextName.DlmsUA);
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
                if (dev.Comm.client.Ciphering.Security == Security.None && ln.ApplicationContextName.ContextId != ApplicationContextName.LogicalName)
                {
                    output.Errors.Insert(0, "Wrong ApplicationContextName.ContextId: " + ln.ApplicationContextName.ContextId);
                }
                else if (dev.Comm.client.Ciphering.Security != Security.None && ln.ApplicationContextName.ContextId != ApplicationContextName.LogicalNameWithCiphering)
                {
                    output.Errors.Insert(0, "Wrong ApplicationContextName.ContextId: " + ln.ApplicationContextName.ContextId);
                }
            }
            else
            {
                if (dev.Comm.client.Ciphering.Security == Security.None && ln.ApplicationContextName.ContextId != ApplicationContextName.ShortName)
                {
                    output.Errors.Insert(0, "Wrong ApplicationContextName.ContextId: " + ln.ApplicationContextName.ContextId);
                }
                else if (dev.Comm.client.Ciphering.Security != Security.None && ln.ApplicationContextName.ContextId != ApplicationContextName.ShortNameWithCiphering)
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
        }

        /// <summary>
        /// Rerad logical device name and test that meter is returning right command type.
        /// </summary>
        /// <param name="settings">Conformance settings.</param>
        /// <param name="dev">DLMS device.</param>
        /// <param name="output"></param>
        private static void ReadLogicalDeviceName(GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output)
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
                output.Errors.Add("Logical Device Name is not implemented.");
                return;
            }
            try
            {
                if (dev.Comm.client.Ciphering.Security != Security.None)
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

        /// <summary>
        /// Test that meter can handle invalid password.
        /// </summary>
        /// <param name="settings">Conformance settings.</param>
        /// <param name="dev">DLMS device.</param>
        /// <param name="output"></param>
        private static void TestInvalidPassword(GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output)
        {
            if (!string.IsNullOrEmpty(settings.InvalidPassword) && dev.Comm.client.Authentication != Authentication.None)
            {
                dev.Comm.Disconnect();
                string pw = dev.Password;
                byte[] hpw = dev.HexPassword;
                dev.Password = CryptHelper.Encrypt(settings.InvalidPassword, Password.Key);
                dev.HexPassword = null;
                try
                {
                    Thread.Sleep((int)settings.DelayConnection.TotalMilliseconds);
                    dev.InitializeConnection();
                    output.Errors.Insert(0, "Login succeeded with wrong password.");
                    dev.Comm.Disconnect();
                }
                catch (GXDLMSException)
                {
                    output.Info.Insert(0, "Invalid password test succeeded.");
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

        /// <summary>
        /// Send Disc, SNRM and Receiver ready to check that meter is in Normal Response Mode.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_P1
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test1(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #1.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #1. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            //SubTest 1: Move the IUT to NRM
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.SNRMRequest(), "HDLC test #1. SNRM request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                output.Info.Add("SNRM request succeeded. MaxInfoLengthTransmit: " + dev.Comm.client.Limits.MaxInfoTX +
                    " MaxInfoLengthReceive: " + dev.Comm.client.Limits.MaxInfoRX + " WindowSizeTransmit: " +
                    dev.Comm.client.Limits.WindowSizeTX + " WindowSizeReceive: " + dev.Comm.client.Limits.WindowSizeRX);
            }
            catch (Exception)
            {
                passed = false;
            }
            //SubTest 2: Check that the IUT is in NRM
            //Check that meter is Normal Response Mode.
            if (passed)
            {
                try
                {
                    reply.Clear();
                    dev.Comm.client.Limits.SenderFrame = 0x10;
                    dev.Comm.ReadDataBlock(dev.Comm.client.ReceiverReady(RequestTypes.None), "HDLC test #1. Receiver Ready ", 1, tryCount, reply);
                    if ((reply.FrameId & 0xF) != 1)
                    {
                        output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc1\">Test #1 failed</a>. Send sequence number is not 0");
                        passed = false;
                    }
                    if ((reply.FrameId & 0x10) != 0x10)
                    {
                        output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc1\">Test #1 failed</a>. P/F is 0");
                        passed = false;
                    }
                    if ((reply.FrameId & 0xF0) != 0x10)
                    {
                        output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc1\">Test #1 failed</a>. Receive sequence number is not 0");
                        passed = false;
                    }
                }
                catch (Exception)
                {
                    passed = false;
                    output.Errors.Add("Receiver Ready failed.");
                }
            }
            //SubTest 3: Move the IUT to NDM
            //Send disc.
            if (passed)
            {
                try
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #1. Disconnect request", 1, tryCount, reply);
                    dev.Comm.ParseUAResponse(reply.Data);
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                    {
                        output.Info.Add("Meter returns DisconnectMode.");
                    }
                    passed = false;
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            //SubTest 4: Check that the IUT is in NDM
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #1. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc1\">Test #1 failed.</a>");
            }
        }

        /// <summary>
        /// Send SNRM and SNRM again where one byte from CRC is removed. Then check that meter is in Normal Response Mode.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_P2
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test2(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #2.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.SNRMRequest(), "HDLC test #2. SNRM request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                output.Info.Add("SNRM request succeeded. MaxInfoLengthTransmit: " + dev.Comm.client.Limits.MaxInfoTX +
                    " MaxInfoLengthReceive: " + dev.Comm.client.Limits.MaxInfoRX + " WindowSizeTransmit: " +
                    dev.Comm.client.Limits.WindowSizeTX + " WindowSizeReceive: " + dev.Comm.client.Limits.WindowSizeRX);
            }
            catch (Exception ex)
            {
                output.Errors.Add("SNRM request failed. " + ex.Message);
                passed = false;
            }
            try
            {
                reply.Clear();
                GXByteBuffer bb = new GXByteBuffer(dev.Comm.DisconnectRequest(true));
                --bb.Size;
                dev.Comm.ReadDataBlock(bb.Array(), "HDLC test #2. Illegal frame.", 1, tryCount, reply);
                output.Info.Add("Illegal frame failed.");
                passed = false;
            }
            catch (Exception)
            {
                output.Info.Add("Illegal frame succeeded.");
            }
            Thread.Sleep(2000);
            //Check that meter is Normal Response Mode.
            try
            {
                reply.Clear();
                dev.Comm.client.Limits.SenderFrame = 0x10;
                dev.Comm.ReadDataBlock(dev.Comm.client.ReceiverReady(RequestTypes.None), "HDLC test #2. Receiver Ready ", 1, tryCount, reply);
                if (reply.FrameId != 0x11)
                {
                    passed = false;
                    output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc2\">Test #2 failed</a>. Response is not RR.");
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #2. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc2\">Test #2 failed.</a>");
            }
        }

        /// <summary>
        /// Send SNRM and then wait to check that inactivity timeout is working.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_P3
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test3(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            test.OnTrace(test, "Starting HDLC tests #3.\r\n");
            GXDLMSObjectCollection objs = dev.Comm.client.Objects.GetObjects(ObjectType.IecHdlcSetup);
            GXReplyData reply = new GXReplyData();
            if (objs.Count == 0)
            {
                output.PreInfo.Add("Inactivity timeout is not tested.");
                test.OnTrace(test, "Ignored.\r\n");
            }
            else
            {
                bool passed = true;
                try
                {
                    dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #1. Disconnect request", 1, tryCount, reply);
                    dev.Comm.ParseUAResponse(reply.Data);
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                    {
                        output.Info.Add("Meter returns DisconnectMode.");
                    }
                    else
                    {
                        passed = false;
                    }
                }
                catch (Exception)
                {
                    passed = false;
                }
                //SubTest 1: Move the IUT to NRM
                try
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.SNRMRequest(), "HDLC test #1. SNRM request", 1, tryCount, reply);
                    dev.Comm.ParseUAResponse(reply.Data);
                    output.Info.Add("SNRM request succeeded. MaxInfoLengthTransmit: " + dev.Comm.client.Limits.MaxInfoTX +
                        " MaxInfoLengthReceive: " + dev.Comm.client.Limits.MaxInfoRX + " WindowSizeTransmit: " +
                        dev.Comm.client.Limits.WindowSizeTX + " WindowSizeReceive: " + dev.Comm.client.Limits.WindowSizeRX);
                    dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "HDLC test #14. AARQRequest.", 1, tryCount, reply);
                    dev.Comm.client.ParseAAREResponse(reply.Data);
                }
                catch (Exception)
                {
                    passed = false;
                }
                GXDLMSHdlcSetup s = (GXDLMSHdlcSetup)objs[0];
                if (passed)
                {
                    try
                    {
                        dev.Comm.ReadValue(s, 8);
                    }
                    catch (Exception ex)
                    {
                        output.Errors.Add("Failed to read inactivity timeout value." + ex.Message);
                    }
                    output.PreInfo.Add("HDLC Setup default inactivity timeout value is " + s.InactivityTimeout + " seconds.");
                }
                if (s.InactivityTimeout == 0)
                {
                    test.OnTrace(test, "Ignored.\r\n");
                }
                else
                {
                    if (passed)
                    {
                        try
                        {
                            reply.Clear();
                            dev.Comm.ReadDataBlock(dev.Comm.SNRMRequest(), "HDLC test #3. SNRM request", 1, tryCount, reply);
                            dev.Comm.ParseUAResponse(reply.Data);
                            output.Info.Add("Disconect SNRM request succeeded. MaxInfoLengthTransmit: " + dev.Comm.client.Limits.MaxInfoTX +
                                " MaxInfoLengthReceive: " + dev.Comm.client.Limits.MaxInfoRX + " WindowSizeTransmit: " +
                                dev.Comm.client.Limits.WindowSizeTX + " WindowSizeReceive: " + dev.Comm.client.Limits.WindowSizeRX);
                        }
                        catch (Exception ex)
                        {
                            output.Errors.Add("SNRM request failed. " + ex.Message);
                            passed = false;
                        }
                        test.OnTrace(test, "Testing inactivity timeout and sleeping for " + s.InactivityTimeout + " seconds.\r\n");
                        Thread.Sleep((1 + s.InactivityTimeout) * 1000);
                        try
                        {
                            reply.Clear();
                            dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #3. Disconnect request", 1, tryCount, reply);
                            passed = false;
                        }
                        catch (GXDLMSException ex)
                        {
                            if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                            {
                                output.Info.Add("Meter returns DisconnectMode.");
                            }
                            else
                            {
                                passed = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            output.Errors.Add("SNRM request failed. " + ex.Message);
                            passed = false;
                        }
                    }
                    if (passed)
                    {
                        test.OnTrace(test, "Passed.\r\n");
                    }
                    else
                    {
                        test.OnTrace(test, "Failed.\r\n");
                        output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc3\">Test #3 failed.</a>");
                    }
                }
            }
        }

        /// <summary>
        /// Send Disc and then send invalid frames and check that meter is in Normal Disconnected Mode.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_N1
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test4(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #4.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC Test #4. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("HDLC Test #4. Meter returns DisconnectMode.");
                }
                else if (ex.ErrorCode == (int)ErrorCode.Rejected)
                {
                    passed = false;
                    output.Info.Add("HDLC Test #4. Meter returns Rejected.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("HDLC Test #4. DisconnectRequest failed. " + ex.Message);
            }

            //Opening flag missing
            test.OnTrace(test, "HDLC Illecal frame test (Opening flag missing).\r\n");
            try
            {
                reply.Clear();
                GXByteBuffer bb = new GXByteBuffer(dev.Comm.SNRMRequest());
                bb.Move(1, 0, bb.Size - 1);
                dev.Comm.ReadDataBlock(bb.Array(), "HDLC test #4. Opening flag missing.", 1, tryCount, reply);
                output.Errors.Add("HDLC Test #4. Opening flag missing failed.");
                passed = false;
            }
            catch (Exception)
            {
                output.Info.Add("HDLC Test #4. Opening flag missing succeeded.");
            }

            //Closing flag missing.
            test.OnTrace(test, "HDLC Illecal frame test (Closing flag missing).\r\n");
            try
            {
                reply.Clear();
                GXByteBuffer bb = new GXByteBuffer(dev.Comm.SNRMRequest());
                --bb.Size;
                dev.Comm.ReadDataBlock(bb.Array(), "HDLC test #4. Closing flag missing.", 1, tryCount, reply);
                output.Errors.Add("HDLC Test #4. Closing flag missing failed.");
                passed = false;
            }
            catch (Exception)
            {
                output.Info.Add("HDLC Test #4. Closing flag missing succeeded.");
            }

            //Both flags are missing.
            test.OnTrace(test, "HDLC Test #4. HDLC Illecal frame test (Both flags are missing).\r\n");
            try
            {
                reply.Clear();
                GXByteBuffer bb = new GXByteBuffer(dev.Comm.SNRMRequest());
                bb.Move(1, 0, bb.Size - 1);
                --bb.Size;
                dev.Comm.ReadDataBlock(bb.Array(), "HDLC test #4. Both flags missing.", 1, tryCount, reply);
                output.Errors.Add("HDLC Test #4. Both flags missing failed.");
                passed = false;
            }
            catch (Exception)
            {
                output.Info.Add("HDLC Test #4. Both flags missing succeeded.");
            }
            //Check that the IUT is in NDM.
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #4. Disconnect request", 1, tryCount, reply);
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("HDLC Test #4. Meter returns DisconnectMode.");
                }
                else if (ex.ErrorCode == (int)ErrorCode.Rejected)
                {
                    passed = false;
                    output.Info.Add("HDLC Test #4. Meter returns Rejected.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                output.Errors.Add("HDLC Test #4. Disconnect request failed.");
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc4\">Test #4 failed.</a>");
            }
        }

        /// <summary>
        /// Send Disc and then send invalid frames and SNRM and wait UA.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_N2
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test5(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #5.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #5. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else if (ex.ErrorCode == (int)ErrorCode.Rejected)
                {
                    output.Info.Add("Meter rejects Disconnect Request.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                output.Errors.Add("DisconnectRequest failed. " + ex.Message);
            }

            //Remove content one byte at the time.
            UInt16 rx = dev.Comm.client.Limits.MaxInfoRX, tx = dev.Comm.client.Limits.MaxInfoTX;
            dev.Comm.client.Limits.MaxInfoRX = dev.Comm.client.Limits.MaxInfoTX = 128;
            try
            {
                GXByteBuffer bb = new GXByteBuffer(dev.Comm.SNRMRequest());
                while (bb.Size - 4 > 0)
                {
                    bb.Move(4, 3, bb.Size - 4);
                    --bb.Data[2];
                    try
                    {
                        reply.Clear();
                        dev.Comm.ReadDataBlock(bb.Array(), "HDLC test #5. Invalid frame.", 1, tryCount, reply);
                        passed = false;
                        break;
                    }
                    catch (TimeoutException)
                    {
                    }
                    catch (Exception)
                    {
                        passed = false;
                    }
                }
            }
            finally
            {
                dev.Comm.client.Limits.MaxInfoRX = rx;
                dev.Comm.client.Limits.MaxInfoTX = tx;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.SNRMRequest(), "HDLC test #5. SNRM request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                output.Info.Add("Disconect SNRM request succeeded. MaxInfoLengthTransmit: " + dev.Comm.client.Limits.MaxInfoTX +
                    " MaxInfoLengthReceive: " + dev.Comm.client.Limits.MaxInfoRX + " WindowSizeTransmit: " +
                    dev.Comm.client.Limits.WindowSizeTX + " WindowSizeReceive: " + dev.Comm.client.Limits.WindowSizeRX);
            }
            catch (Exception ex)
            {
                output.Errors.Add("SNRM request failed. " + ex.Message);
                passed = false;
            }

            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc5\">Test #5 failed.</a>");
            }
        }

        /// <summary>
        /// Send Disc and then send invalid SNRM frame and Disc and check that meter is in Normal Disconnected Mode.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_N3
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test6(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #6.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #6. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
            }
            catch (Exception ex)
            {
                output.Errors.Add("Disconnect request failed. " + ex.Message);
                passed = false;
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.SNRMRequest();
                data[1] = 0;
                dev.Comm.ReadDataBlock(data, "HDLC test #6. SNRM request", 1, tryCount, reply);
                output.Errors.Add("SNRM request failed.");
                passed = false;
            }
            catch (TimeoutException)
            {
                output.Info.Add("SNRM request succeeded.");
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #6. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("SNRM request failed. " + ex.Message);
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc6\">Test #6 failed.</a>");
            }
        }

        /// <summary>
        /// Send Disc and then send invalid SNRM frame where length is too long. Send Disc and check that meter is in Normal Disconnected Mode.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_N4
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test7(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #7.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #7. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.SNRMRequest();
                ++data[2];
                dev.Comm.ReadDataBlock(data, "HDLC test #7. SNRM request", 1, tryCount, reply);
                output.Errors.Add("SNRM request failed. ");
                passed = false;
            }
            catch (TimeoutException)
            {
                output.Info.Add("Illegal frame succeeded.");
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #7. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }

            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc7\">Test #7 failed.</a>");
            }
        }

        /// <summary>
        /// Send SNRM and then send Unknown command identifier.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_N5
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test8(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #8.\r\n");
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #8. SNRM request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }
            //SubTest 1: Unknown command identifier
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.CustomHdlcFrameRequest(0xFF, null);
                dev.Comm.ReadDataBlock(data, "HDLC test #8. Unknown command", 1, tryCount, reply);
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.UnacceptableFrame)
                {
                    output.Info.Add("Unknown command succeeded. " + ex.Message);
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            // SubTest 2: Check that the HDLC layer can be initialised
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #8. SNRM request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #8. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }

            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc8\">Test #8 failed.</a>");
            }
        }

        /// <summary>
        /// Send Disc and then send illegal frame. Send Disc and check that meter is in Normal Disconnected Mode.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_N7
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test9(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #9.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #9. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                GXByteBuffer bb = new GXByteBuffer(new byte[] { 0x81, 0x80, 0x12, 0x05, 0x01, 0x80, 0x06, 0x01, 0x80, 0x07, 0x04, 0x00, 0x00, 0x00, 0x01, 0x08, 0x04, 0x00, 0x00, 0x00, 0x01 });
                byte[] data = dev.Comm.client.CustomHdlcFrameRequest(0x94, bb);
                dev.Comm.ReadDataBlock(data, "HDLC test #9. Unknown command", 1, tryCount, reply);
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.Rejected)
                {
                    output.Info.Add("Unknown command succeeded. (Meter rejects the frame)");
                }
                else if (ex.ErrorCode == (int)ErrorCode.UnacceptableFrame)
                {
                    output.Info.Add("Unknown command succeeded. (Unacceptable Frame)");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                output.Info.Add("Unknown command succeeded (timeout).");
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #9. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else if (ex.ErrorCode == (int)ErrorCode.Rejected)
                {
                    output.Info.Add("Meter rejects the frame.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                output.Info.Add("Disconnect request failed (timeout).");
                passed = false;
            }

            try
            {
                reply.Clear();
                GXByteBuffer bb = new GXByteBuffer(new byte[] { 0x81, 0x80, 0x12, 0x05, 0x01, 0x80, 0x06, 0x01, 0x80, 0x07, 0x04, 0x00, 0x00, 0x00, 0x01, 0x08, 0x04, 0x00, 0x00, 0x00, 0x01 });
                byte[] data = dev.Comm.client.CustomHdlcFrameRequest(0x94, bb);
                for (byte pos = 0; pos != data.Length; ++pos)
                {
                    if (data[pos] == 0x94)
                    {
                        data[pos] = 0x93;
                        break;
                    }
                }
                dev.Comm.ReadDataBlock(data, "HDLC test #9. Illecal frame.", 1, tryCount, reply);
                passed = false;
            }
            catch (TimeoutException)
            {
                output.Info.Add("Unknown command succeeded (timeout).");
            }
            catch (Exception ex)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc9\">Test #9 failed.</a>");
            }
        }

        /// <summary>
        /// Send Disc and then send SNRM frame where CRC is wrong. Then send Disc and check that meter is in Normal Disconnected Mode.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_N8
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test10(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #10.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #10. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.SNRMRequest();
                ++data[data.Length - 2];
                dev.Comm.ReadDataBlock(data, "HDLC test #10. Unknown command", 1, tryCount, reply);
                passed = false;
            }
            catch (TimeoutException)
            {
                output.Info.Add("Invalid frame succeeded..");
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #10. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc10\">Test #10 failed.</a>");
            }
        }

        /// <summary>
        /// Send frame where server address is three bytes.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_HDLC_ADDRESS_N7.
        /// </remarks>
        private static void Test11(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #11.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #10. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = Gurux.Common.GXCommon.HexToBytes("7EA0090002052193AFD07E");
                dev.Comm.ReadDataBlock(data, "HDLC test #11. SNRM", 1, tryCount, reply);
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.UnacceptableFrame)
                {
                    output.Info.Add("Meter returns Unacceptable Frame.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (TimeoutException)
            {
                //This should happened.
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #11. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc11\">Test #11 failed.</a>");
            }
        }

        /// <summary>
        /// Try to connect using max frame size 2030
        /// </summary>
        /// <remarks>
        /// This is DLMS CCT T_HDLC_NDM2NRM_P1.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test12(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #12.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #12. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                dev.Comm.client.Limits.MaxInfoRX = dev.Comm.client.Limits.MaxInfoTX = 2030;
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #12. max frame size.", 1, tryCount, reply);
                dev.Comm.client.ParseUAResponse(reply.Data);
            }
            catch (Exception ex)
            {
                passed = false;
            }
            finally
            {
                dev.Comm.client.Limits.MaxInfoRX = dev.Comm.client.Limits.MaxInfoTX = 128;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #12. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc12\">Test #12 failed.</a>");
            }
        }

        /// <summary>
        /// Try to connect using window size 4
        /// </summary>
        /// <remarks>
        /// This is DLMS CCT T_HDLC_NDM2NRM_P2.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test13(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #13.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #13. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                dev.Comm.client.Limits.WindowSizeRX = dev.Comm.client.Limits.WindowSizeTX = 4;
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #13. Window size 4.", 1, tryCount, reply);
                dev.Comm.client.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            finally
            {
                dev.Comm.client.Limits.WindowSizeRX = dev.Comm.client.Limits.WindowSizeTX = 1;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #13. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc13\">Test #13 failed.</a>");
            }
        }

        /// <summary>
        /// Send AARQ.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_HDLC_INFO_P1.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test14(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #14.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #14. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #14. SNRMRequest.", 1, tryCount, reply);
                dev.Comm.client.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                try
                {
                    dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "HDLC test #14. AARQRequest.", 1, tryCount, reply);
                    dev.Comm.client.ParseAAREResponse(reply.Data);
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #14. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc14\">Test #14 failed.</a>");
            }
        }

        /// <summary>
        /// Send AARQ in segments.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_HDLC_INFO_P1.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test15(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #15.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #15. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #15. SNRMRequest.", 1, tryCount, reply);
                dev.Comm.client.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                try
                {
                    dev.Comm.client.Limits.MaxInfoTX = 4;
                    dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "HDLC test #15. AARQRequest.", 1, tryCount, reply);
                    dev.Comm.client.ParseAAREResponse(reply.Data);
                }
                catch (Exception)
                {
                    passed = false;
                }
                finally
                {
                    dev.Comm.client.Limits.MaxInfoTX = 128;
                }
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #15. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc15\">Test #15 failed.</a>");
            }
        }

        /// <summary>
        /// Send frame that don't fit to HDLC frame.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_HDLC_INFO_N1.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test16(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #16.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #16. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #16. SNRMRequest.", 1, tryCount, reply);
                dev.Comm.client.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                try
                {
                    reply.Clear();
                    GXByteBuffer bb = new GXByteBuffer();
                    bb.Capacity = dev.Comm.client.Limits.MaxInfoTX + 1;
                    bb.Size = bb.Capacity;
                    byte[] data = dev.Comm.client.CustomHdlcFrameRequest(0x10, bb);
                    dev.Comm.ReadDataBlock(data, "HDLC test #16. AARQRequest.", 1, tryCount, reply);
                    passed = false;
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode != (int)ErrorCode.UnacceptableFrame)
                    {
                        passed = false;
                    }
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            try
            {
                if (passed)
                {
                    reply.Clear();
                    byte[] data = dev.Comm.client.SNRMRequest();
                    dev.Comm.ReadDataBlock(data, "HDLC test #16. SNRMRequest.", 1, tryCount, reply);
                    dev.Comm.client.ParseUAResponse(reply.Data);
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #16. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc16\">Test #16 failed.</a>");
            }
        }

        /// <summary>
        /// Send SNRM and then Receiver ready with wrong sequence number.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_HDLC_INFO_N2.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test17(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #17.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #17. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #17. SNRMRequest.", 1, tryCount, reply);
                dev.Comm.client.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                try
                {
                    reply.Clear();
                    byte[] data = dev.Comm.client.CustomHdlcFrameRequest(0x31, null);
                    dev.Comm.ReadDataBlock(data, "HDLC test #17. ReceiverReady.", 1, tryCount, reply);
                    passed = false;
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode != (int)ErrorCode.UnacceptableFrame)
                    {
                        passed = false;
                    }
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            try
            {
                if (passed)
                {
                    reply.Clear();
                    byte[] data = dev.Comm.client.SNRMRequest();
                    dev.Comm.ReadDataBlock(data, "HDLC test #17. SNRMRequest.", 1, tryCount, reply);
                    dev.Comm.client.ParseUAResponse(reply.Data);
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #17. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc17\">Test #17 failed.</a>");
            }
        }

        /// <summary>
        /// Send wrong sequence number.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_HDLC_INFO_N3.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test18(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #18.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #18. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #18. SNRMRequest.", 1, tryCount, reply);
                dev.Comm.client.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                try
                {
                    reply.Clear();
                    GXByteBuffer bb = new GXByteBuffer();
                    bb.SetHexString("E6E600601DA109060760857405080101BE10040E01000000065F1F040060FE9FFFFF");
                    byte[] data = dev.Comm.client.CustomHdlcFrameRequest(0x12, bb);
                    dev.Comm.ReadDataBlock(data, "HDLC test #18. Wrong N(S) sequence number.", 1, tryCount, reply);
                    //Meter should reply RR.
                    if (reply.Data.Size != 0 || reply.FrameId != 0x11)
                    {
                        passed = false;
                    }
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode != (int)ErrorCode.UnacceptableFrame)
                    {
                        passed = false;
                    }
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            try
            {
                if (passed)
                {
                    reply.Clear();
                    byte[] data = dev.Comm.client.SNRMRequest();
                    dev.Comm.ReadDataBlock(data, "HDLC test #18. SNRMRequest.", 1, tryCount, reply);
                    dev.Comm.client.ParseUAResponse(reply.Data);
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #18. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc18\">Test #18 failed.</a>");
            }
        }

        /// <summary>
        /// Start communicating without sending SNRM before AARQ.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_HDLC_NDMOP_N1.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test19(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #19.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #19. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "HDLC test #19. AARQ request.", 1, tryCount, reply);
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (TimeoutException)
            {
                output.Info.Add("Unknown command succeeded (timeout).");
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #19. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc19\">Test #19 failed.</a>");
            }
        }

        /// <summary>
        /// Send frame where client address is two bytes.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_HDLC_ADDRESS_N1.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test20(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #20.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #20. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();

                byte[] data = Gurux.Common.GXCommon.HexToBytes("7EA00B0002040100219361807E");
                dev.Comm.ReadDataBlock(data, "HDLC test #20. SNRM request.", 1, tryCount, reply);
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.UnacceptableFrame)
                {
                    output.Info.Add("Meter returns UnacceptableFrame.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (TimeoutException)
            {
                output.Info.Add("Invalid CRC succeeded (timeout).");
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #20. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc20\">Test 20 failed.</a>");
            }
        }

        /// <summary>
        /// Send unknown destination.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_HDLC_ADDRESS_N4.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test21(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #21.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #21. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }
            int serverAddress = dev.Comm.client.ServerAddress;
            try
            {
                reply.Clear();
                dev.Comm.client.ServerAddress = GXDLMSClient.GetServerAddress(16, Convert.ToInt32(dev.PhysicalAddress));
                dev.Comm.ReadDataBlock(dev.Comm.SNRMRequest(), "HDLC test #21. SNRM request.", 1, tryCount, reply);
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.UnacceptableFrame)
                {
                    output.Info.Add("Meter returns UnacceptableFrame.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (TimeoutException)
            {
                output.Info.Add("Invalid destination succeeded (timeout).");
            }
            catch (Exception)
            {
                passed = false;
            }
            //SubTest 1. Unknown destination address on one byte
            if (passed)
            {
                try
                {
                    reply.Clear();
                    dev.Comm.client.ServerAddress = 2;
                    dev.Comm.ReadDataBlock(dev.Comm.SNRMRequest(), "HDLC test #21. SNRM request.", 1, tryCount, reply);
                    passed = false;
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.UnacceptableFrame)
                    {
                        output.Info.Add("Meter returns UnacceptableFrame.");
                    }
                    else
                    {
                        passed = false;
                    }
                }
                catch (TimeoutException)
                {
                    output.Info.Add("Invalid destination succeeded (timeout).");
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            dev.Comm.client.ServerAddress = serverAddress;
            if (!passed)
            {
                try
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #21. Disconnect request", 1, tryCount, reply);
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                    {
                        output.Info.Add("Meter returns DisconnectMode.");
                    }
                    else
                    {
                        passed = false;
                    }
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc21\">Test 21 failed.</a>");
            }
        }

        /// <summary>
        /// Send same HDLC packet twice and check that meter can hanle this. Then read next data.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test101(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #101.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #101. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #101. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "HDLC test #101. AARQ", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
                reply.Clear();
                GXDLMSData ldn = new GXDLMSData("0.0.42.0.0.255");
                data = dev.Comm.Read(ldn, 1);
                dev.Comm.ReadDataBlock(data, "HDLC test #101. Read LDN #1", 1, reply);
                reply.Clear();
                //RR
                dev.Comm.ReadDataBlock(data, "HDLC test #101. Read LDN #2", 1, reply);
                if ((reply.FrameId & 0xC) != 0)
                {
                    output.Info.Add("Meter Don't return ReceiveReady.");
                    passed = false;
                }
                reply.Clear();
                //Read value again.
                data = dev.Comm.Read(ldn, 1);
                dev.Comm.ReadDataBlock(data, "HDLC test #101. Read LDN #3", 1, reply);
                reply.Clear();
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.UnacceptableFrame)
                {
                    output.Info.Add("Meter returns Unacceptable Frame.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                //This should happened.
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #11. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc101\">Test #101 failed.</a>");
            }
        }

        private static void HdlcTests(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            if (!Continue)
            {
                return;
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest1)
            {
                Test1(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest2)
            {
                Test2(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }

            if (!settings.ExcludedHdlcTests.ExcludeTest3)
            {
                Test3(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest4)
            {
                Test4(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }

            if (!settings.ExcludedHdlcTests.ExcludeTest5)
            {
                Test5(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest6)
            {
                Test6(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest7)
            {
                Test7(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest8)
            {
                Test8(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest9)
            {
                Test9(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest10)
            {
                Test10(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest11)
            {
                Test11(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest12)
            {
                Test12(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest13)
            {
                Test13(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest14)
            {

                Test14(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest15)
            {

                Test15(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest16)
            {

                Test16(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest17)
            {

                Test17(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest18)
            {

                Test18(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest19)
            {
                Test19(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest20)
            {
                Test20(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest21)
            {
                Test21(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedHdlcTests.ExcludeTest101)
            {
                Test101(test, settings, dev, output, tryCount);
            }
        }

        /// <summary>
        /// Appl_01: Connection establishment.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_APPL_IDLE_N1.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void AppTest1(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting COSEM Application tests #1.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(), "HDLC test #1. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #5 failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #1. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
                GXDLMSAssociationLogicalName av = new GXDLMSAssociationLogicalName("0.0.40.0.0.255");
                data = dev.Comm.Read(av, 1);
                (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                dev.Comm.ReadDataBlock(data, "COSEM Application test #1. Read logical name", 1, reply);
                reply.Clear();
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode != (int)ErrorCode.UnacceptableFrame)
                {
                    passed = false;
                }
            }
            catch (GXDLMSConfirmedServiceError ex)
            {
                if (ex.ConfirmedServiceError != ConfirmedServiceError.InitiateError ||
                    ex.ServiceError != ServiceError.Service ||
                    ex.ServiceErrorValue != 2)
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("COSEM Application tests #1 failed. " + ex.Message);
            }
            (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = false;

            //T_APPL_OPEN_1
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "COSEM Application test #1. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                Thread.Sleep(1000);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            //SubTest 1.Establish an AA using the parameters declared
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #1. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "COSEM Application test #1. AARQ", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
                reply.Clear();
                if (dev.Comm.client.Authentication > Authentication.Low)
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.client.GetApplicationAssociationRequest(), "Authenticating.", 1, tryCount, reply);
                    dev.Comm.client.ParseApplicationAssociationResponse(reply.Data);
                }
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                passed = false;
            }
            //SubTest 2.Check that the AA has been established
            if (passed)
            {
                try
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.client.Read("0.0.40.0.0.255", ObjectType.AssociationLogicalName, 1), "COSEM Application test #1. AARQ", 1, reply);
                    string ln = GXDLMSConverter.ToLogicalName(reply.Value);
                    if (ln != "0.0.40.0.0.255")
                    {
                        output.Info.Add("Check Associated State: Unexpected Data value. Expected: 0.0.40.0.0.255, actual: " + ln);
                        passed = false;
                    }
                    reply.Clear();
                }
                catch (Exception ex)
                {
                    passed = false;
                    output.Errors.Add("COSEM Application tests #1 failed. " + ex.Message);
                }
                //SubTest 3.Release the AA
                if (passed)
                {
                    try
                    {
                        dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #4. Disconnect request", 1, tryCount, reply);
                        dev.Comm.ParseUAResponse(reply.Data);
                    }
                    catch (GXDLMSException ex)
                    {
                        if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                        {
                            output.Info.Add("Meter returns DisconnectMode.");
                        }
                        else
                        {
                            passed = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        passed = false;
                        output.Info.Add("COSEM Application tests #1 failed. " + ex.Message);
                    }
                    if (passed)
                    {
                        try
                        {
                            reply.Clear();
                            byte[] data = dev.Comm.client.SNRMRequest();
                            dev.Comm.ReadDataBlock(data, "COSEM Application test #1. SNRM", 1, tryCount, reply);
                            dev.Comm.ParseUAResponse(reply.Data);
                            reply.Clear();
                        }
                        catch (GXDLMSException ex)
                        {
                            if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                            {
                                output.Info.Add("Meter returns DisconnectMode.");
                            }
                            else
                            {
                                passed = false;
                            }
                        }
                        catch (Exception)
                        {
                            passed = false;
                        }
                    }
                }
            }

            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                output.Info.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app1\">COSEM Application test #1 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app1\">COSEM Application test #1 failed.</a>");
            }
        }

        /// <summary>
        /// Appl_04: Connection establishment : Protocol-version
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void AppTest4(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting COSEM Application tests #4.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #4. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #4 failed. " + ex.Message);
            }
            //Protocol-version present and containing the default value.
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #4. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
                try
                {
                    dev.Comm.client.ProtocolVersion = "100001";
                    dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "COSEM Application test #4. AARQ", 1, tryCount, reply);
                    dev.Comm.client.ProtocolVersion = null;
                    dev.Comm.ParseAAREResponse(reply.Data);
                    reply.Clear();
                    if (dev.Comm.client.ProtocolVersion != "100001")
                    {
                        throw new Exception("Protocol-version test failed.");
                    }
                }
                finally
                {
                    dev.Comm.client.ProtocolVersion = null;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #4 failed. " + ex.Message);
            }
            if (passed)
            {
                try
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #4. Disconnect request", 1, tryCount, reply);
                    dev.Comm.ParseUAResponse(reply.Data);
                    reply.Clear();
                }
                catch (Exception ex)
                {
                    passed = false;
                }
                //SubTest 2: Protocol-version is present but not containing the default value
                try
                {
                    reply.Clear();
                    byte[] data = dev.Comm.client.SNRMRequest();
                    dev.Comm.ReadDataBlock(data, "COSEM Application test #4. SNRM", 1, tryCount, reply);
                    dev.Comm.ParseUAResponse(reply.Data);
                    reply.Clear();
                    try
                    {
                        dev.Comm.client.ProtocolVersion = "010001";
                        dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "COSEM Application test #4. AARQ", 1, tryCount, reply);
                        dev.Comm.ParseAAREResponse(reply.Data);
                        reply.Clear();
                        throw new Exception("Protocol-version test failed.");
                    }
                    finally
                    {
                        dev.Comm.client.ProtocolVersion = null;
                    }
                }
                catch (GXDLMSException ex)
                {
                    if (ex.Result == AssociationResult.TransientRejected && ex.Diagnostic == (byte)AcseServiceProvider.NoCommonAcseVersion)
                    {
                        output.Info.Add("COSEM Application tests #4 Invalid Protocol-version succeeded. " + ex.Message);
                    }
                    else
                    {
                        passed = false;
                        output.Errors.Add("COSEM Application tests #4 failed. " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    passed = false;
                    output.Errors.Add("COSEM Application tests #4 failed. " + ex.Message);
                }
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #4. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
            }

            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                output.Info.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app4\">COSEM Application test #4 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app4\">COSEM Application test #4 failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_OPEN_5: Unknown application context
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void AppTest5(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = true;
            test.OnTrace(test, "Starting COSEM Application tests #5.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #5. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #5 failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #5. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
                GXByteBuffer bb = new GXByteBuffer();
                bb.SetHexString("E6E600601DA10906075F857504070203BE10040E01000000065F1F04007C1BA0FFFF");
                dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0x10, bb), "COSEM Application test #5. AARQ", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
                reply.Clear();
                throw new Exception("UNKNOWN ApplicationContextName failed.");
            }
            catch (GXDLMSException ex)
            {
                if (ex.Result == AssociationResult.PermanentRejected && ex.Diagnostic == (byte)SourceDiagnostic.ApplicationContextNameNotSupported)
                {
                    output.Info.Add("COSEM Application tests #5 UNKNOWN ApplicationContextName succeeded. " + ex.Message);
                }
                else
                {
                    passed = false;
                    output.Errors.Add("COSEM Application tests #5 failed. " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("COSEM Application tests #5 failed. " + ex.Message);
            }
            /*
            //SubTest 1: Unused AARQ fields are present with a dummy value
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #5. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #5 failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #5. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #5 failed. " + ex.Message);
            }
            try
            {
                GXByteBuffer bb = new GXByteBuffer();
                bb.SetHexString("6036A109060760857405080101A203040144A303040144A403020100A503020100A803020100BE10040E01000000065F1F040060FE9FFFFF");
                dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0x10, bb), "COSEM Application test #5. AARQ", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #5. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
            }

            //SubTest 4: AARQ.calling-AE-invocation-id present when client user identification is not supported.
            dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #5. Disconnect request", 1, tryCount, reply);
            dev.Comm.ParseUAResponse(reply.Data);
            reply.Clear();

            data = dev.Comm.client.SNRMRequest();
            dev.Comm.ReadDataBlock(data, "COSEM Application test #5. SNRM", 1, tryCount, reply);
            dev.Comm.ParseUAResponse(reply.Data);
            reply.Clear();
            try
            {
                GXByteBuffer bb = new GXByteBuffer();
                bb.SetHexString("6059A109060760857405080101A203040144A303040144A403020100A503020100A703040144A803020100A9030201018A0207808B0760857405080201AC0A80083132333435363738BE10040E01000000065F1F040060FE9FFFFF");
                dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0x10, bb), "COSEM Application test #5. AARQ", 1, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
            }
            */
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #5. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
            }

            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                output.Info.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app5\">COSEM Application test #5 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app5\">COSEM Application test #5 failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_OPEN_6:
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void AppTest6(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = true;
            test.OnTrace(test, "Starting COSEM Application tests #6.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #6. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                    output.Info.Add("COSEM Application tests #6 failed. " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #6 failed. " + ex.Message);
            }
            //SubTest 1: Unused AARQ fields are present with a dummy value
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #6. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
                GXByteBuffer bb = new GXByteBuffer();
                bb.SetHexString("E6E6006036A109060760857405080101A203040144A303040144A403020100A503020100A803020100BE10040E01000000065F1F040060FEDFFFFF");
                dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0x10, bb), "COSEM Application test #6. AARQ", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
                GXDLMSTranslator t = new GXDLMSTranslator(TranslatorOutputType.SimpleXml);
                reply.Data.Position = 0;
                string str = t.PduToXml(reply.Data);
                if (str.Contains("RespondingAeInvocationId"))
                {
                    throw new Exception("Responding AE Invocation ID present.");
                }
                if (str.Contains("RespondingAPTitle"))
                {
                    throw new Exception("Responding AP title present.");
                }
                reply.Clear();
            }
            catch (GXDLMSException ex)
            {
                if (ex.Result == AssociationResult.PermanentRejected && ex.Diagnostic != (byte)SourceDiagnostic.None)
                {
                    output.Info.Add("COSEM Application tests #5 UNKNOWN ApplicationContextName succeeded. " + ex.Message);
                }
                else
                {
                    passed = false;
                    output.Errors.Add("COSEM Application tests #6 failed. " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("COSEM Application tests #6 failed. " + ex.Message);
            }
            //SubTest 2: AARQ.calling-AP-title too short
            if (passed && (dev.Comm.client.Authentication == Authentication.HighGMAC ||
                (dev.Comm.client.Ciphering != null && dev.Comm.client.Ciphering.Security != Security.None)))
            {
                try
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #6. Disconnect request", 1, tryCount, reply);
                    dev.Comm.ParseUAResponse(reply.Data);
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                    {
                        output.Info.Add("Meter returns DisconnectMode.");
                    }
                    else
                    {
                        passed = false;
                    }
                }
                catch (Exception ex)
                {
                    passed = false;
                    output.Info.Add("COSEM Application tests #6 failed. " + ex.Message);
                }
                byte[] st = dev.Comm.client.Ciphering.SystemTitle;
                try
                {
                    reply.Clear();
                    data = dev.Comm.client.SNRMRequest();
                    dev.Comm.ReadDataBlock(data, "COSEM Application test #6. SNRM", 1, tryCount, reply);
                    dev.Comm.ParseUAResponse(reply.Data);
                    reply.Clear();
                    dev.Comm.client.Ciphering.SystemTitle = new byte[] { 0x44 };
                    dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "COSEM Application test #6. AARQ", 1, tryCount, reply);
                    dev.Comm.ParseAAREResponse(reply.Data);
                    reply.Clear();
                    throw new Exception("AARQ.calling-AP-title too short.");
                }
                catch (GXDLMSException ex)
                {
                    if (ex.Result == AssociationResult.PermanentRejected && ex.Diagnostic == (byte)SourceDiagnostic.CallingApTitleNotRecognized)
                    {
                        output.Info.Add("COSEM Application tests #6 AARQ.calling-AP-title too short succeeded. " + ex.Message);
                    }
                    else
                    {
                        passed = false;
                        output.Errors.Add("COSEM Application tests #6 AARQ.calling-AP-title too short failed. " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    passed = false;
                    output.Errors.Add("COSEM Application tests #6 failed. " + ex.Message);
                }
                dev.Comm.client.Ciphering.SystemTitle = st;
            }
            //SubTest 3: AARQ.calling-AP-title too long
            if (passed && (dev.Comm.client.Authentication == Authentication.HighGMAC ||
                (dev.Comm.client.Ciphering != null && dev.Comm.client.Ciphering.Security != Security.None)))
            {
                try
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #6. Disconnect request", 1, tryCount, reply);
                    dev.Comm.ParseUAResponse(reply.Data);
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                    {
                        output.Info.Add("Meter returns DisconnectMode.");
                    }
                    else
                    {
                        passed = false;
                    }
                }
                catch (Exception ex)
                {
                    passed = false;
                    output.Info.Add("COSEM Application tests #6 failed. " + ex.Message);
                }
                byte[] st = dev.Comm.client.Ciphering.SystemTitle;
                try
                {
                    reply.Clear();
                    data = dev.Comm.client.SNRMRequest();
                    dev.Comm.ReadDataBlock(data, "COSEM Application test #6. SNRM", 1, tryCount, reply);
                    dev.Comm.ParseUAResponse(reply.Data);
                    reply.Clear();
                    dev.Comm.client.Ciphering.SystemTitle = new byte[] { 0x43, 0x54, 0x54, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30 };
                    dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "COSEM Application test #6. AARQ", 1, tryCount, reply);
                    dev.Comm.ParseAAREResponse(reply.Data);
                    reply.Clear();
                    throw new Exception("AARQ.calling-AP-title too long.");
                }
                catch (GXDLMSException ex)
                {
                    if (ex.Result == AssociationResult.PermanentRejected && ex.Diagnostic == (byte)SourceDiagnostic.CallingApTitleNotRecognized)
                    {
                        output.Info.Add("COSEM Application tests #6 AARQ.calling-AP-title too long succeeded. " + ex.Message);
                    }
                    else
                    {
                        passed = false;
                        output.Errors.Add("COSEM Application tests #6 AARQ.calling-AP-title too long failed. " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    passed = false;
                    output.Errors.Add("COSEM Application tests #6 AARQ.calling-AP-title too long failed. " + ex.Message);
                }
                dev.Comm.client.Ciphering.SystemTitle = st;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #6. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #6 failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #6. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #6 failed. " + ex.Message);
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                output.Info.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app6\">COSEM Application test #6 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app6\">COSEM Application test #6 failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_OPEN_7:
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void AppTest7(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = true;
            test.OnTrace(test, "Starting COSEM Application tests #7.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #7. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                    output.Info.Add("COSEM Application tests #7 failed. " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #7 failed. " + ex.Message);
            }
            //Unused AARQ fields are present with a dummy value
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #7. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
                GXByteBuffer bb = new GXByteBuffer();
                bb.SetHexString("E6E6006036A109060760857405080101A203040144A303040144A403020100A503020100A803020100BE10040E01000000065F1F040060FE9FFFFF");
                dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0x10, bb), "COSEM Application test #7. AARQ", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("COSEM Application tests #7 failed. " + ex.Message);
            }
            //SubTest 1: Unused AARQ fields are present with a dummy value. Authentication is not used.
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #7. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #7 failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #7. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #7 failed. " + ex.Message);
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                output.Info.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app7\">COSEM Application test #7 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app7\">COSEM Application test #7 failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_OPEN_9. Test for dedicated key.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void AppTest9(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = true;
            test.OnTrace(test, "Starting COSEM Application tests #9.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #9. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                    output.Info.Add("COSEM Application tests #9 failed. " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #9 failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #9. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                dev.Comm.client.Ciphering.DedicatedKey = GXCommon.HexToBytes("000102030405060708090A0B0C0D0E0F");
                dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "COSEM Application test #9. AARQ", 1, tryCount, reply);
                dev.Comm.client.Ciphering.DedicatedKey = null;
                dev.Comm.ParseAAREResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
                output.Errors.Add("COSEM Application tests #9 failed. " + ex.Message);
            }
            finally
            {
                dev.Comm.client.Ciphering.DedicatedKey = null;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                output.Info.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app9\">COSEM Application test #9 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app9\">COSEM Application test #9 failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_OPEN_11. Check Quality of service.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void AppTest11(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = true;
            test.OnTrace(test, "Starting COSEM Application tests #11.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #11. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #11. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #11 failed. " + ex.Message);
            }
            reply.Clear();
            try
            {
                dev.Comm.client.QualityOfService = 1;
                dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "COSEM Application test #9. AARQ", 1, tryCount, reply);
                dev.Comm.client.QualityOfService = 0;
                dev.Comm.ParseAAREResponse(reply.Data);
                if (dev.Comm.client.QualityOfService != 1)
                {
                    passed = false;
                    output.Info.Add("COSEM Application tests #11 failed. Quality of service field not found.");
                }
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
            }
            dev.Comm.client.QualityOfService = 0;
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #11. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                output.Info.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app11\">COSEM Application test #11 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app11\">COSEM Application test #11 failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_OPEN_12. Check DLMS version number. Try to use version 5 and 7.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void AppTest12(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = true;
            test.OnTrace(test, "Starting COSEM Application tests #12.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #12. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #12 failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #12. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #12 failed. " + ex.Message);
            }
            reply.Clear();
            try
            {
                GXByteBuffer bb = new GXByteBuffer();
                bb.SetHexString("E6E600601DA109060760857405080101BE10040E01000000055F1F040060FE9FFFFF");
                dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0x10, bb), "COSEM Application test #12. AARQ", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
            }
            catch (GXDLMSConfirmedServiceError ex)
            {
                if (ex.ServiceError == ServiceError.Initiate && ex.ServiceErrorValue == 1)
                {
                    output.Info.Add("COSEM Application tests #12 succeeded with DMLS version 5.");
                }
                else
                {
                    output.Info.Add("COSEM Application tests #12 failed. " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #12. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #12. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #12 failed. " + ex.Message);
            }
            reply.Clear();
            try
            {
                GXByteBuffer bb = new GXByteBuffer();
                bb.SetHexString("E6E600601DA109060760857405080101BE10040E01000000075F1F040060FE9FFFFF");
                dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0x10, bb), "COSEM Application test #12. AARQ", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
            }
            catch (GXDLMSConfirmedServiceError ex)
            {
                if (ex.ServiceError == ServiceError.Initiate && ex.ServiceErrorValue == 1)
                {
                    output.Info.Add("COSEM Application tests #12 succeeded with DMLS version 7.");
                }
                else
                {
                    output.Info.Add("COSEM Application tests #12 failed. " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #12. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                output.Info.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app12\">COSEM Application test #12 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app12\">COSEM Application test #12 failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_OPEN_14. Try to connect with invalid PDU size.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void AppTest14(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = true;
            test.OnTrace(test, "Starting COSEM Application tests #14.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #14. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #14 failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #14. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #14 failed. " + ex.Message);
            }
            reply.Clear();
            try
            {
                GXByteBuffer bb = new GXByteBuffer();
                bb.SetHexString("E6E600601DA109060760857405080101BE10040E01000000065F1F040060FE9F000B");
                dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0x10, bb), "COSEM Application test #14. AARQ", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
                passed = false;
                output.Errors.Add("COSEM Application tests #14 failed with PDU size 11.");
            }
            catch (GXDLMSConfirmedServiceError ex)
            {
                if (ex.ServiceError == ServiceError.Initiate && ex.ServiceErrorValue == 3)
                {
                    output.Info.Add("COSEM Application tests #14 succeeded with PDU size 11.");
                }
                else
                {
                    output.Info.Add("COSEM Application tests #14 failed. " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #14. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #14. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #14 failed. " + ex.Message);
            }
            reply.Clear();
            try
            {
                GXByteBuffer bb = new GXByteBuffer();
                bb.SetHexString("E6E600601DA109060760857405080101BE10040E01000000065F1F040060FE9FFFFF");
                dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0x10, bb), "COSEM Application test #14. AARQ", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
            }
            catch (Exception ex)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                output.Info.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app14\">COSEM Application test #14 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app14\">COSEM Application test #14 failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_DATA_LN_N1. Get-Request with unknown tag and Get-Request with missing elements.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void AppTest15(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = true;
            test.OnTrace(test, "Starting COSEM Application tests #15.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #15. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #15 failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #15. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #15 failed. " + ex.Message);
            }
            reply.Clear();

            try
            {
                GXByteBuffer bb = new GXByteBuffer();
                dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "HDLC test #15. AARQRequest.", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
                reply.Clear();
                if (dev.Comm.client.Authentication > Authentication.Low)
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.client.GetApplicationAssociationRequest(), "Authenticating.", 1, tryCount, reply);
                    dev.Comm.client.ParseApplicationAssociationResponse(reply.Data);
                }
            }
            catch (Exception ex)
            {
                passed = false;
            }
            //T_APPL_DATA_LN_N1. Get-Request with unknown tag.
            if (passed)
            {
                try
                {
                    reply.Clear();
                    GXByteBuffer bb = new GXByteBuffer();
                    if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                    {
                        bb.SetHexString("E6E600");
                    }
                    bb.SetHexString("C004C1000F0000280000FF0100");
                    (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                    dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0, bb), "COSEM Application test #15. Invalid Get request.", 1, tryCount, reply);
                    passed = false;
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.ReadWriteDenied)
                    {
                        output.Info.Add("COSEM Application tests #15 Invalid Get request succeeded.");
                    }
                    else
                    {
                        output.Errors.Add("COSEM Application tests #15 failed. " + ex.Message);
                        passed = false;
                    }
                }
                catch (Exception ex)
                {
                    passed = false;
                }
            }
            //Get-Request with missing elements
            if (passed)
            {
                try
                {
                    reply.Clear();
                    GXByteBuffer bb = new GXByteBuffer();
                    if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                    {
                        bb.SetHexString("E6E600");
                    }
                    bb.SetHexString("C001C1000028000100");
                    (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                    dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0, bb), "COSEM Application test #15. Invalid Get request.", 1, tryCount, reply);
                    passed = false;
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.ReadWriteDenied)
                    {
                        output.Info.Add("COSEM Application tests #15 Invalid Get request succeeded.");
                    }
                    else
                    {
                        output.Errors.Add("COSEM Application tests #15 failed. " + ex.Message);
                        passed = false;
                    }
                }
                catch (Exception ex)
                {
                    passed = false;
                }
            }
            (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = false;
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #15. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            //Send Read request.
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #15. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "HDLC test #15. AARQRequest.", 1, tryCount, reply);
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #15 failed. " + ex.Message);
            }
            reply.Clear();
            try
            {

                GXByteBuffer bb = new GXByteBuffer();
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    bb.SetHexString("E6E600");
                }
                bb.SetHexString("050102FA00");
                (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0x32, bb), "COSEM Application test #15. Read service", 1, tryCount, reply);
                passed = false;
                output.Errors.Add("COSEM Application tests #15 failed using read service.");
            }
            catch (GXDLMSConfirmedServiceError ex)
            {
                if (ex.ServiceError == ServiceError.Service && ex.ServiceErrorValue == 2)
                {
                    output.Info.Add("COSEM Application tests #15 succeeded when Short Name referencing is used.");
                }
                else
                {
                    output.Info.Add("COSEM Application tests #15 failed for Short Name referencing. " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                passed = false;
            }
             (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = false;
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                output.Info.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app15\">COSEM Application test #15 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app15\">COSEM Application test #15 failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_DATA_LN_N3. Set-Request with unknown tag and set-Request with missing elements.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void AppTest16(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = true;
            test.OnTrace(test, "Starting COSEM Application tests #16.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #16. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    output.Info.Add("Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #16 failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #16. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
                output.Info.Add("COSEM Application tests #16 failed. " + ex.Message);
            }
            reply.Clear();

            try
            {
                GXByteBuffer bb = new GXByteBuffer();
                dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "HDLC test #16. AARQRequest.", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
                if (dev.Comm.client.Authentication > Authentication.Low)
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.client.GetApplicationAssociationRequest(), "Authenticating.", 1, tryCount, reply);
                    dev.Comm.client.ParseApplicationAssociationResponse(reply.Data);
                }
                reply.Clear();
            }
            catch (Exception)
            {
                passed = false;
            }
            //T_APPL_DATA_LN_N1. Set-Request with unknown tag.
            if (passed)
            {
                try
                {
                    reply.Clear();
                    GXByteBuffer bb = new GXByteBuffer();
                    if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                    {
                        bb.SetHexString("E6E600");
                    }
                    bb.SetHexString("C106C1000F0000280000FF010009060000280000FF");
                    (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                    dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0, bb), "COSEM Application test #16. Invalid Set request.", 1, tryCount, reply);
                    passed = false;
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.ReadWriteDenied)
                    {
                        output.Info.Add("COSEM Application tests #16 Invalid Set request succeeded.");
                    }
                    else
                    {
                        output.Errors.Add("COSEM Application tests #16 failed. " + ex.Message);
                        passed = false;
                    }
                }
                catch (Exception ex)
                {
                    passed = false;
                }
            }
            //Set-Request with missing data
            if (passed)
            {
                try
                {
                    reply.Clear();
                    GXByteBuffer bb = new GXByteBuffer();
                    if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                    {
                        bb.SetHexString("E6E600");
                    }
                    bb.SetHexString("C101C1000F0000280000");
                    (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                    dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0, bb), "COSEM Application test #16. Invalid Set request.", 1, tryCount, reply);
                    passed = false;
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.ReadWriteDenied)
                    {
                        output.Info.Add("COSEM Application tests #16 Invalid Set request succeeded.");
                    }
                    else
                    {
                        output.Errors.Add("COSEM Application tests #16 failed. " + ex.Message);
                        passed = false;
                    }
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = false;
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #16. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                output.Info.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app16\">COSEM Application test #16 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#app16\">COSEM Application test #16 failed.</a>");
            }
        }

        /// <summary>
        /// COSEM application layer tests
        /// </summary>
        private static void CosemApplicationLayerTests(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output)
        {
            int tryCount = 1;
            if (!Continue)
            {
                return;
            }
            if (!settings.ExcludedApplicationTests.ExcludeTest1)
            {
                AppTest1(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.ExcludeTest4)
            {
                AppTest4(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.ExcludeTest5)
            {
                AppTest5(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.ExcludeTest6)
            {
                AppTest6(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.ExcludeTest7)
            {
                AppTest7(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.ExcludeTest9)
            {
                AppTest9(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.ExcludeTest11)
            {
                AppTest11(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.ExcludeTest12)
            {
                AppTest12(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.ExcludeTest14)
            {
                AppTest14(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.ExcludeTest15)
            {
                AppTest15(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.ExcludeTest16)
            {
                AppTest16(test, settings, dev, output, tryCount);
                if (!Continue)
                {
                    return;
                }
            }
        }

        private static void TestClock(GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output)
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
                        try
                        {
                            if (dev.Comm.client.UtcTimeZone)
                            {
                                it.TimeZone = (int)TimeZoneInfo.Local.BaseUtcOffset.TotalMinutes;
                            }
                            else
                            {
                                it.TimeZone = -(int)TimeZoneInfo.Local.BaseUtcOffset.TotalMinutes;
                            }
                            output.Info.Add("Set new Time zone:" + it.TimeZone);
                            dev.Comm.Write(it, 3);
                            dev.Comm.Write(it, 2);
                            newTime.Skip |= DateTimeSkips.Second;
                            dev.Comm.ReadValue(it, 2);
                            time = it.Time;
                            if (newTime.Compare(it.Time.Value.Add(DateTime.Now - start).LocalDateTime) != 0)
                            {
                                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock1\">Clock test #1 failed</a>. Failed to set new time using current time zone. Expected: " + newTime + " Actual: " + it.Time.Value.Add(DateTime.Now - start).LocalDateTime);
                                start = time = DateTime.Now;
                            }
                            else
                            {
                                output.Info.Add("Setting new time succeeded using current time zone.");
                            }
                        }
                        catch (Exception ex)
                        {
                            output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock1\">Clock test #1 failed</a>. Failed to set new time using current time zone. Meter returns exception. " + ex.Message);
                            start = time = DateTime.Now;
                        }
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
                                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock2\">Clock test #2 failed</a>. Failed to set new time using UTC time. Expected: " + newTime + " Actual: " + it.Time.Value.Add(DateTime.Now - start).LocalDateTime);
                            }
                            else
                            {
                                output.Info.Add("Setting new time succeeded using UTC time zone.");
                            }
                        }
                        catch (Exception ex)
                        {
                            output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock2\">Clock test #2 failed</a>. Failed to set new time using UTC time zone. Meter returns exception. " + ex.Message);
                        }

                        /*
                        //Update new time without timezone.
                        it.Time.Skip |= DateTimeSkips.Deviation;
                        dev.Comm.Write(it, 2);
                        dev.Comm.ReadValue(it, 2);
                        if (newTime.Compare(it.Time.Value.Add(DateTime.Now - start).DateTime) != 0)
                        {
                            output.Errors.Add("Failed to set new time using without time zone. Expected: " + newTime + " Actual: " + it.Time.Value.Add(DateTime.Now - start).DateTime);
                        }
                        */

                        //Check DST.
                        if (it.GetAccess(8) == AccessMode.ReadWrite && (it.GetAccess(7) & AccessMode.Read) != 0)
                        {
                            dev.Comm.ReadValue(it, 8);
                            dev.Comm.ReadValue(it, 7);
                            bool dst = it.Enabled;
                            int deviation = it.Deviation;
                            if (dst)
                            {
                                output.Info.Add("DST is in use and deviation is " + deviation + ".");
                            }
                            else
                            {
                                output.Info.Add("DST is not in use. Devitation is " + deviation + ".");
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
                                    output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock3\">Clock test #3 failed</a>. Clock test failed. Failed to enable DST.");
                                }
                                //Read time.
                                dev.Comm.ReadValue(it, 2);
                                GXDateTime tmp = new GXDateTime(it.Time);
                                tmp.Skip |= DateTimeSkips.Second;
                                if (tmp.Compare(time.Add(DateTime.Now - start)) != 0)
                                {
                                    //Setting current time
                                    output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock3\">Clock test #3 failed</a>. Clock test failed. Time is not valid if DST is changed. Expected: " + time.Add(DateTime.Now - start) + " Actual: " + tmp);
                                }
                                else
                                {
                                    output.Info.Add("Meter can change DST and time is updated correctly.");
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
                                        output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock3\">Clock test #3 failed</a>. Clock test failed. Failed to set DST.");
                                    }
                                }
                            }
                            //Change time and check is DST flag set.
                            if ((it.GetAccess(5) & AccessMode.Read) != 0 && (it.GetAccess(6) & AccessMode.Read) != 0)
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
                                    output.Info.Add("Meter is in DST time.");
                                    if ((it.Status & ClockStatus.DaylightSavingActive) == 0)
                                    {
                                        output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock4\">Clock test #4 failed</a>. Meter is in DST, but DST status flag is not set.");
                                    }
                                    //Move meter to normal time.
                                    it.Time = new GXDateTime(end.Value.AddDays(7));
                                    dst1 = false;
                                }
                                else
                                {
                                    output.Info.Add("Meter is in normal time.");
                                    if ((it.Status & ClockStatus.DaylightSavingActive) != 0)
                                    {
                                        output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock4\">Clock test #4 failed</a>. Meter is in normal time, but DST status flag is set.");
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
                                        output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock4\">Clock test #4 failed</a>. Meter is in DST, but DST status flag is not set.");
                                    }
                                    else
                                    {
                                        output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock4\">Clock test #4 failed</a>. Meter is in normal time, but DST status flag is set.");
                                    }
                                }
                                else
                                {
                                    if (dst1)
                                    {
                                        output.Info.Add("Time is changed to DST time, and DST status flag is set.");
                                    }
                                    else
                                    {
                                        output.Info.Add("Time is changed to normal time and DST status flag is not set.");
                                    }
                                }
                                //Move meter to current time.
                                it.Time = new GXDateTime(time.Add(DateTime.Now - start));
                                dev.Comm.Write(it, 2);
                            }
                            else
                            {
                                output.Info.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock4\">Clock test #4 is not tested</a>. Changind DST begin and end time is not tested.");
                            }
                            //Return DST back.
                            it.Enabled = dst;
                            try
                            {
                                dev.Comm.Write(it, 8);
                            }
                            catch (Exception)
                            {
                                output.Errors.Add("Clock test failed. Failed to set DST.");
                            }
                        }
                        //Change time zone to UTC.
                        if (it.TimeZone != 0)
                        {
                            output.Info.Add("Time zone of the meter:" + it.TimeZone);
                            it.TimeZone = 0;
                            dev.Comm.Write(it, 3);
                            //Read time.
                            dev.Comm.ReadValue(it, 2);
                            GXDateTime tmp = new GXDateTime(it.Time);
                            tmp.Skip |= DateTimeSkips.Second;
                            if (tmp.Compare(time.Add(DateTime.Now - start)) != 0)
                            {
                                //Setting UTC time
                                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock5\">Clock test #5 failed</a>. Clock test failed. Time is not valid if time zone is changed to UTC. Expected: " + time.Add(DateTime.Now - start) + " Actual: " + tmp);
                            }
                            else
                            {
                                output.Info.Add("Meter can change time zone to UTC and time is updated correctly.");
                            }
                        }
                        else
                        {
                            it.TimeZone = (int)TimeZoneInfo.Utc.GetUtcOffset(DateTime.Now).TotalMinutes;
                            output.Info.Add("Time zone of the meter is UTC. Try to set it to " + it.TimeZone);
                            dev.Comm.Write(it, 3);
                            //Read time.
                            dev.Comm.ReadValue(it, 2);
                            GXDateTime tmp = new GXDateTime(it.Time);
                            tmp.Skip |= DateTimeSkips.Second;
                            if (tmp.Compare(time.Add(DateTime.Now - start)) != 0)
                            {
                                //Setting current time
                                output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock5\">Clock test #5 failed</a>.  Clock test failed. Time is not valid if time zone is changed from UTC. Expected: " + time.Add(DateTime.Now - start) + " Actual: " + tmp);
                            }
                            else
                            {
                                output.Info.Add("Meter can change time zone from UTC and time is updated correctly.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        output.Errors.Add("<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#clock1\">Clock test failed</a>. " + ex.Message);
                    }
                    it.TimeZone = timeZone;
                    dev.Comm.Write(it, 3);
                }
                else
                {
                    output.Info.Add("Clock time access for " + it.LogicalName + " is " + it.GetAccess(2));
                    output.Info.Add("Clock time zone access for " + it.LogicalName + " is " + it.GetAccess(3));
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
                        output.Info.Add("Image block size is " + img.ImageBlockSize + " bytes.");
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
                                    output.Errors.Add("Image transferred blocks status is wrong. It's " + img.ImageTransferredBlocksStatus + " and it shoud be zilled with 0.");
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
                            output.Errors.Add("Image transfer status is wrong. It's " + img.ImageTransferStatus + " and it shoud be TransferInitiated.");
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
                            output.Errors.Add("Image first not transferred block number wrong. It's " + img.ImageFirstNotTransferredBlockNumber + " and it shoud be 0.");
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
                            output.Errors.Add("Image activate info is not reset.");
                        }
                        if (!error)
                        {
                            output.Info.Add("Image activation Step 2 succeeded.");
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
                            output.Info.Add("Image transfer (Step 3) succeeded.");
                            output.Info.Add("Image transfer takes " + (DateTime.Now - start).ToString(@"hh\:mm\:ss"));
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
                                output.Errors.Add("Image first not transferred block number wrong. It's " + img.ImageFirstNotTransferredBlockNumber + " and it shoud be " + blocks.Length + ".");
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
                            output.Errors.Add("Image Transferred blocks status is not implemented.");
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
                                    output.Errors.Add("Image Transferred blocks status is wrong. Amount of bits is different than block size. (" + img.ImageTransferredBlocksStatus.Length + "/" + blocks.Length + ")");
                                }
                            }
                            foreach (char it in img.ImageTransferredBlocksStatus)
                            {
                                if (it != '1')
                                {
                                    error = true;
                                    output.Errors.Add("Image transferred blocks status is not set.");
                                    break;
                                }
                            }
                        }
                        if (!error)
                        {
                            output.Info.Add("Image completeness Step 4 succeeded.");
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
                            } while (img.ImageTransferStatus != ImageTransferStatus.TransferInitiated);
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
                                    output.Errors.Add("Image verification failed.");
                                    return;
                                }
                                if (img.ImageTransferStatus != ImageTransferStatus.VerificationSuccessful)
                                {
                                    test.OnProgress(test, "Image verification is on progress. Waiting...", 1, 1);
                                    Thread.Sleep((int)settings.ImageVerifyWaitTime.TotalMilliseconds);
                                }
                            } while ((img.ImageTransferStatus != ImageTransferStatus.VerificationSuccessful));
                        }
                        output.Info.Add("Image verify succeeded (Step 5).");
                        output.Info.Add("Verify takes " + (DateTime.Now - start).ToString(@"hh\:mm\:ss"));

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
                            output.Info.Add("Image activation succeeded (Step 6).");
                            output.Info.Add("Activation takes " + (DateTime.Now - start).ToString(@"hh\:mm\:ss"));

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
