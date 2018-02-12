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
// More information of Gurux DLMS/COSEM Director: http://www.gurux.org/GXDLMSDirector
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
using GXDLMS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace GXDLMSDirector
{
    partial class GXConformanceDlg : Form
    {
        public GXConformanceDlg()
        {
            InitializeComponent();
            ConcurrentReadingCb.Checked = Properties.Settings.Default.ConformanceConcurrent;
            ShowValuesCb.Checked = Properties.Settings.Default.ConformanceShowValues;
            ReReadCb.Checked = Properties.Settings.Default.ConformanceReadAssociationView;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ConformanceConcurrent = ConcurrentReadingCb.Checked;
            Properties.Settings.Default.ConformanceShowValues = ShowValuesCb.Checked;
            Properties.Settings.Default.ConformanceReadAssociationView = ReReadCb.Checked;
        }


        /// <summary>
        /// Get tests for COSEM objects.
        /// </summary>
        /// <returns>COSEM object tests.</returns>
        private static string[] GetTests()
        {
            return typeof(GXConformanceDlg).Assembly.GetManifestResourceNames()
                .Where(r => r.StartsWith("GXDLMSDirector.ConformanceTests") && r.EndsWith(".xml"))
                .ToArray();
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

        private static void Execute(GXDLMSConverter converter, GXConformanceTest test, string name, List<GXDLMSXmlPdu> actions, GXOutput output)
        {
            GXReplyData reply = new GXReplyData();
            string ln = null;
            int index = 0;
            ObjectType ot = ObjectType.None;
            List<KeyValuePair<ObjectType, string>> succeeded = new List<KeyValuePair<ObjectType, string>>();
            foreach (GXDLMSXmlPdu it in actions)
            {
                if (it.Command == Command.Snrm && test.Device.Comm.client.InterfaceType == InterfaceType.WRAPPER)
                {
                    continue;
                }
                if (it.Command == Command.DisconnectRequest && test.Device.Comm.client.InterfaceType == InterfaceType.WRAPPER)
                {
                    break;
                }
                //Send
                if (it.IsRequest())
                {
                    XmlNode i = it.XmlNode.SelectNodes("GetRequestNormal")[0];
                    if (i == null)
                    {
                        ot = ObjectType.None;
                        index = 0;
                        ln = null;
                    }
                    else
                    {
                        ot = (ObjectType)int.Parse(i.SelectNodes("AttributeDescriptor/ClassId")[0].Attributes["Value"].Value);
                        index = int.Parse(i.SelectNodes("AttributeDescriptor/AttributeId")[0].Attributes["Value"].Value);
                        ln = (i.SelectNodes("AttributeDescriptor/InstanceId")[0].Attributes["Value"].Value);
                        ln = GetLogicalName(ln);
                        test.OnTrace(null, ot + " " + ln + ":" + index + "\r\n");
                    }
                    reply.Clear();
                    try
                    {
                        byte[][] tmp = (test.Device.Comm.client as GXDLMSXmlClient).PduToMessages(it);
                        test.Device.Comm.ReadDataBlock(tmp, "", 1, reply);
                    }
                    catch (Exception ex)
                    {
                        output.Errors.Add("<a target=\"_blank\" href=http://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + " attribute " + index + " is <div class=\"tooltip\">failed:" + ex.Message);
                        output.Errors.Add("<span class=\"tooltiptext\">");
                        output.Errors.Add(ex.ToString());
                        output.Errors.Add("</span></div>");
                    }
                }
                else if (reply.Data.Size != 0)
                {
                    List<string> list = it.Compare(reply.ToString());
                    if (list.Count != 0)
                    {
                        if (ot == ObjectType.None)
                        {
                            foreach (string err in list)
                            {
                                output.Errors.Add(err);
                            }
                        }
                        else
                        {
                            output.Errors.Add("<a target=\"_blank\" href=http://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a> " + ln + " attribute " + index + " is <div class=\"tooltip\">invalid.");
                            output.Errors.Add("<span class=\"tooltiptext\">");
                            output.Errors.Add("Expected:</b><br/>");
                            output.Errors.Add(it.PduAsXml.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br/>"));
                            output.Errors.Add("<br/><b>Actual:</b><br/>");
                            output.Errors.Add(reply.ToString().Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r\n", "<br/>"));
                            output.Errors.Add("</span></div>");
                        }
                        Console.WriteLine("------------------------------------------------------------");
                        Console.WriteLine("Test" + name + "failed. Invalid reply: " + string.Join("\n", list.ToArray()));
                    }
                    else
                    {
                        if (ot == ObjectType.None)
                        {
                            output.Info.Add(name + " succeeded.");
                        }
                        else
                        {
                            if (Properties.Settings.Default.ConformanceShowValues)
                            {
                                if (reply.Value is byte[])
                                {
                                    succeeded.Add(new KeyValuePair<ObjectType, string>(ot, index.ToString() + ":" + GXCommon.ToHex((byte[])reply.Value)));
                                }
                                else if (reply.Value is Object[])
                                {
                                    succeeded.Add(new KeyValuePair<ObjectType, string>(ot, index.ToString() + ":" + GXHelpers.GetArrayAsString(reply.Value)));
                                }
                                else
                                {
                                    succeeded.Add(new KeyValuePair<ObjectType, string>(ot, index.ToString() + ":" + Convert.ToString(reply.Value)));
                                }
                            }
                            else
                            {
                                succeeded.Add(new KeyValuePair<ObjectType, string>(ot, index.ToString()));
                            }
                        }
                    }
                }
            }
            if (succeeded.Count != 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<div class=\"tooltip\">" + ln);
                sb.Append("<span class=\"tooltiptext\">");
                foreach (var it in succeeded)
                {
                    sb.Append("Index " + it.Value + "<br/>");
                }
                sb.Append("</span></div>");
                sb.Append("&nbsp;" + converter.GetDescription(ln, succeeded[0].Key)[0] + "&nbsp;" + "<a target=\"_blank\" href=http://www.gurux.fi/Gurux.DLMS.Objects.GXDLMS" + ot + ">" + ot + "</a>.");
                output.Info.Add(sb.ToString());
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

        private static void OnMessageTrace(GXDLMSDevice sender, string trace, byte[] data, string path)
        {
            if (path != null)
            {
                using (FileStream fs = File.Open(path, FileMode.Append))
                {
                    using (TextWriter writer = new StreamWriter(fs))
                    {
                        writer.WriteLine(trace + " " + GXCommon.ToHex(data));
                    }
                }
            }
        }

        /// <summary>
        /// Continue conformance tests.
        /// </summary>
        public static bool Continue = true;


        private static object ConformanceLock = new object();

        /// <summary>
        /// Read data from the meter.
        /// </summary>
        public static void ReadXmlMeter(object data)
        {
            object[] tmp2 = (object[])data;
            List<GXConformanceTest> tests = (List<GXConformanceTest>)tmp2[0];
            GXConformanceTest test;
            GXDLMSDevice dev = null;
            GXDLMSConverter converter = new GXDLMSConverter();
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
                    dev = CloneDevice(test.Device);
                    dev.OnTrace = OnMessageTrace;
                    dev.Comm.LogFile = test.LogFile;
                    GXDLMSClient cl = dev.Comm.client;
                    dev.Comm.client = new GXDLMSXmlClient(TranslatorOutputType.SimpleXml);
                    cl.CopyTo(dev.Comm.client);
                    test.Device = dev;
                    output = new GXOutput(tests[0].ResultFile, dev.Name);
                    tests.RemoveAt(0);
                }
                IGXMedia media = dev.Media;
                GXDLMSXmlClient client = (GXDLMSXmlClient)dev.Comm.client;
                List<string> files = new List<string>();
                try
                {
                    media.Open();
                    dev.InitializeConnection();
                    if (Properties.Settings.Default.ConformanceReadAssociationView)
                    {
                        dev.Comm.GetObjects();
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
                    output.PreInfo.Add("Total amount of objects: " + dev.Objects.Count.ToString());
                    //Check OBIS codes.
                    foreach (GXDLMSObject it in dev.Objects)
                    {
                        if (it.Description == "Invalid")
                        {
                            output.Errors.Add("Invalid OBIS code " + it.LogicalName + " for <a target=\"_blank\" href=http://www.gurux.fi/" + it.GetType().FullName + ">" + it.ObjectType + "</a>.");
                            Console.WriteLine("------------------------------------------------------------");
                            Console.WriteLine(it.LogicalName + ": Invalid OBIS code.");
                        }
                    }

                    //Read structures of Cosem objects.
                    List<KeyValuePair<string, List<GXDLMSXmlPdu>>> cosemTests = new List<KeyValuePair<string, List<GXDLMSXmlPdu>>>();
                    GXDLMSTranslator translator = new GXDLMSTranslator(TranslatorOutputType.SimpleXml);
                    lock (ConformanceLock)
                    {
                        foreach (string it in GetTests())
                        {
                            using (Stream stream = typeof(Program).Assembly.GetManifestResourceStream(it))
                            using (StreamReader sr = new StreamReader(stream))
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(sr.ReadToEnd());
                                XmlNodeList list = doc.SelectNodes("/Messages/GetRequest/GetRequestNormal");
                                ObjectType ot = ObjectType.None;
                                foreach (XmlNode node in list)
                                {
                                    ot = (ObjectType)int.Parse(node.SelectNodes("AttributeDescriptor/ClassId")[0].Attributes["Value"].Value);
                                    int index = int.Parse(node.SelectNodes("AttributeDescriptor/AttributeId")[0].Attributes["Value"].Value);
                                    //Update logical name.
                                    foreach (GXDLMSObject obj in dev.Objects.GetObjects(ot))
                                    {
                                        if ((obj.GetAccess(index) & AccessMode.Read) != 0)
                                        {
                                            string tmp = GXCommon.ToHex(LogicalNameToBytes(obj.LogicalName), false);
                                            foreach (XmlNode n in list)
                                            {
                                                XmlAttribute ln = n.SelectNodes("AttributeDescriptor/InstanceId")[0].Attributes["Value"];
                                                ln.Value = tmp;
                                            }
                                            cosemTests.Add(new KeyValuePair<string, List<GXDLMSXmlPdu>>(ot.ToString(), client.LoadXml(doc.InnerXml)));
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    foreach (KeyValuePair<string, List<GXDLMSXmlPdu>> it in cosemTests)
                    {
                        try
                        {
                            Execute(converter, test, it.Key, it.Value, output);
                        }
                        catch (Exception ex)
                        {
                            test.OnError(test, ex);
                        }
                        test.OnObjectTestCompleated(test);
                    }
                    List<ObjectType> unknownDataTypes = new List<ObjectType>();
                    foreach (GXDLMSObject o in dev.Objects)
                    {
                        if (!unknownDataTypes.Contains(o.ObjectType))
                        {
                            bool found = false;
                            foreach (KeyValuePair<string, List<GXDLMSXmlPdu>> t in cosemTests)
                            {
                                if (o.ObjectType.ToString() == t.Key)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                unknownDataTypes.Add(o.ObjectType);
                                output.Warnings.Add("<a target=\"_blank\" href=http://www.gurux.fi/" + o.GetType().FullName + ">" + o.ObjectType + "</a> is not tested.");
                            }
                        }
                    }
                    test.OnReady(test);
                }
                catch (Exception ex)
                {
                    test.OnError(test, ex);
                }
                finally
                {
                    output.MakeReport();
                    output.writer.Flush();
                    output.writer.Close();
                    if (dev != null)
                    {
                        dev.Comm.Disconnect();
                    }
                    test.Done.Set();
                }
            }
        }

    }
}
