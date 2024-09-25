//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Version:         $Revision: 14889 $,
//                  $Date: 2024-09-25 14:55:55 +0300 (Wed, 25 Sep 2024) $
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
using Gurux.DLMS;
using Gurux.Serial;
using Gurux.Net;
using GXDLMS.ManufacturerSettings;
using System.IO.Ports;
using Gurux.Common;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Objects;
using System.Reflection;
using System.Windows.Forms;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Objects.Enums;
using System.Threading;
using Gurux.DLMS.Secure;
using System.Collections.Generic;
using System.Xml;
using Gurux.DLMS.ASN;
using Gurux.DLMS.Ecdsa;
using System.IO;
using Gurux.DLMS.Extension;
using System.Linq;

namespace GXDLMSDirector
{
    internal class Password
    {
        public static string Key = "Gurux Ltd.";
    }

    public delegate void ProgressEventHandler(object sender, string description, int current, int maximium);
    public delegate void StatusEventHandler(object sender, DeviceState status);
    public delegate void ErrorEventHandler(GXDLMSObject sender, int index, object data, Exception ex);
    public delegate void ReadEventHandler(GXDLMSClient client, GXDLMSObject sender, int index, object data, object parameters, Exception ex);
    public delegate void WriteEventHandler(GXDLMSClient client, GXDLMSObject sender, int index, object data, Exception ex);
    public delegate void MessageTraceEventHandler(DateTime time, GXDLMSDevice sender, string trace, byte[] data, int payload, string path, int duration);

    public class GXDLMSCommunicator
    {
        /// <summary>
        /// Log file.
        /// </summary>
        internal string LogFile;

        /// <summary>
        /// Maximum payload size.
        /// </summary>
        internal int payload;

        internal DateTime lastTransaction = DateTime.MinValue;
        internal DateTime connectionStartTime;
        internal GXDLMSDevice parent;
        public Control parentForm;
        public IGXMedia media = null;
        internal GXDLMSSecureClient client;
        internal static IGXDLMSExtension Extension;
        //Frame send time.
        DateTime FrameSendTime = DateTime.MinValue;
        //Object transaction time.
        DateTime ObjectTransactionTime = DateTime.MinValue;


        public GXDLMSCommunicator(GXDLMSDevice parent, IGXMedia media)
        {
            this.parent = parent;
            this.media = media;
            client = new GXDLMSSecureClient();
            //Get ECDSA keys when needed.
            client.OnKeys += Client_OnKeys;
            if (Extension != null)
            {
                client.OnCrypto += Client_OnCrypto;
            }
        }

        private void Client_OnCrypto(object sender, GXCryptoKeyParameter args)
        {
            Extension.Crypt(sender, args);
        }

        /// <summary>
        /// Return correct path.
        /// </summary>
        /// <param name="securitySuite">Security Suite.</param>
        /// <param name="type">Certificate type.</param>
        /// <param name="path">Folder.</param>
        /// <param name="systemTitle">System title.</param>
        /// <returns>Path to the certificate file or folder if system title is not given.</returns>
        private static string GetPath(SecuritySuite securitySuite, CertificateType type, string path, byte[] systemTitle)
        {
            string pre;
            if (securitySuite == SecuritySuite.Suite2)
            {
                path = Path.Combine(path, "384");
            }
            if (systemTitle == null)
            {
                return path;
            }
            switch (type)
            {
                case CertificateType.DigitalSignature:
                    pre = "D";
                    break;
                case CertificateType.KeyAgreement:
                    pre = "A";
                    break;
                default:
                    throw new Exception("Invalid type.");
            }
            return Path.Combine(path, pre + GXDLMSTranslator.ToHex(systemTitle, false) + ".pem");
        }
        private void Client_OnKeys(object sender, GXCryptoKeyParameter args)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GXDLMSDirector");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string certificates = Path.Combine(path, "Certificates");
            if (!Directory.Exists(certificates))
            {
                Directory.CreateDirectory(certificates);
            }
            string keys = Path.Combine(path, "Keys");
            if (!Directory.Exists(keys))
            {
                Directory.CreateDirectory(keys);
            }
            if (args.Encrypt)
            {
                //Find private key.
                path = GetPath(args.SecuritySuite, args.CertificateType, keys, args.SystemTitle);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    args.PrivateKey = GXPkcs8.Load(path).PrivateKey;
                }
            }
            else
            {
                //Find public key.
                path = GetPath(args.SecuritySuite, args.CertificateType, certificates, null);
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    GXx509Certificate[] certs = GXx509Certificate.Search(path, CertificateType.DigitalSignature, args.SystemTitle);
                    if (certs.Length == 0)
                    {
                        throw new Exception("Failed to find meter certificate.");
                    }
                    if (certs.Length != 1)
                    {
                        throw new Exception("Meter have multiple certificates.");
                    }
                    args.PublicKey = certs[0].PublicKey;
                }
            }
        }

        public ProgressEventHandler OnProgress;
        public ReadEventHandler OnBeforeRead;
        public ReadEventHandler OnAfterRead;
        public ErrorEventHandler OnError;
        public WriteEventHandler OnAfterWrite;

        public byte[] SNRMRequest()
        {
            payload = 0;
            return client.SNRMRequest();
        }

        public void ParseUAResponse(GXByteBuffer data)
        {
            client.ParseUAResponse(data);
        }

        public byte[][] AARQRequest()
        {
            return client.AARQRequest();
        }

        public void ParseAAREResponse(GXByteBuffer data)
        {
            client.ParseAAREResponse(data);
        }

        public byte[] Read(GXDLMSObject it, int attributeOrdinal)
        {
            byte[] tmp = client.Read(it, attributeOrdinal)[0];
            GXLogWriter.WriteLog(string.Format("Reading object {0}, interface {1}", it.LogicalName, it.ObjectType), tmp);
            return tmp;
        }

        /// <summary>
        /// Read or write multiple items with one request.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="list"></param>
        public void AccessRequest(DateTime time, List<GXDLMSAccessItem> list)
        {
            GXReplyData reply = new GXReplyData();
            ReadDataBlock(client.AccessRequest(time, list), "Access Request", reply);
            client.ParseAccessResponse(list, reply.Data);
        }

        public byte[][] Read(GXDLMSObject it, int attributeOrdinal, string parameters)
        {
            //Some meters expect a small delay between COSEM object reads.
            if (parent.ObjectDelay > 0)
            {
                double delay = (DateTime.Now - ObjectTransactionTime).TotalMilliseconds;
                if (delay < parent.ObjectDelay)
                {
                    Thread.Sleep(parent.ObjectDelay - (int)delay);
                }
                ObjectTransactionTime = DateTime.Now;
            }
            if (it is GXDLMSProfileGeneric && attributeOrdinal == 2 && parameters != null)
            {
                GXStructure p = (GXStructure)GXDLMSTranslator.XmlToValue(parameters);
                if ((int)p[0] == 1)
                {
                    GXStructure arr = (GXStructure)p[1];
                    GXDateTime start = (GXDateTime)client.ChangeType(new GXByteBuffer((byte[])arr[1]), DataType.DateTime);
                    GXDateTime end = (GXDateTime)client.ChangeType(new GXByteBuffer((byte[])arr[2]), DataType.DateTime);
                    return client.ReadRowsByRange((GXDLMSProfileGeneric)it, start, end);
                }
                if ((int)p[0] == 2)
                {
                    GXStructure arr = (GXStructure)p[1];
                    UInt32 index = (UInt32)arr[0];
                    UInt32 count = (UInt32)arr[1] - index + 1;
                    return client.ReadRowsByEntry((GXDLMSProfileGeneric)it, index, count);
                }
            }
            return client.Read(it, attributeOrdinal);
        }

        public void MethodRequest(GXDLMSObject target, int methodIndex, object data, string text, GXReplyData reply)
        {
            byte[][] tmp;
            if (data is byte[])
            {
                tmp = client.Method(target, methodIndex, data, DataType.Array);
            }
            else
            {
                tmp = client.Method(target, methodIndex, data, GXDLMSConverter.GetDLMSDataType(data));
            }
            reply.Broadcast = client.Broacast;
            int pos = 0;
            string str = string.Format("Method object {0}, interface {1}", target.LogicalName, target.ObjectType);
            foreach (byte[] it in tmp)
            {
                reply.Clear();
                if (tmp.Length != 1)
                {
                    ++pos;
                    NotifyProgress(text, pos, tmp.Length);
                }
                try
                {
                    ReadDataBlock(it, str, reply);
                }
                catch (Exception ex)
                {
                    //Update frame ID if meter returns error.
                    if (client.InterfaceType == InterfaceType.HDLC ||
                        client.InterfaceType == InterfaceType.HdlcWithModeE ||
                        client.InterfaceType == InterfaceType.PlcHdlc)
                    {
                        int t, source;
                        byte type;
                        GXDLMSClient.GetHdlcAddressInfo(new GXByteBuffer(it), out t, out source, out type);
                        client.HdlcSettings.SenderFrame = type;
                    }
                    throw ex;
                }
            }
            NotifyProgress(text, 1, 1);
        }

        byte[] ReleaseRequest()
        {
            byte[][] data = client.ReleaseRequest();
            if (data == null || data.Length == 0)
            {
                return null;
            }
            GXLogWriter.WriteLog("Release request", data[0]);
            return data[0];
        }

        public byte[] DisconnectRequest()
        {
            return DisconnectRequest(false);
        }

        public byte[] DisconnectRequest(bool force)
        {
            byte[] data = client.DisconnectRequest(force);
            if (data == null)
            {
                return null;
            }
            GXLogWriter.WriteLog("Disconnect request");
            return data;
        }

        public void Disconnect()
        {
            try
            {
                if (media != null && media.IsOpen)
                {
                    GXReplyData reply = new GXReplyData();
                    try
                    {
                        //Release is call only for secured connections.
                        //All meters are not supporting Release and it's causing problems.
                        if (client.InterfaceType == InterfaceType.WRAPPER ||
                            (client.Ciphering.Security != (byte)Security.None &&
                            !parent.PreEstablished))
                        {
                            byte[] data = ReleaseRequest();
                            if (data != null)
                            {
                                ReadDataBlock(data, "Release request", reply);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //All meters don't support release.
                    }
                    try
                    {
                        reply.Clear();
                        if ((client.InterfaceType == InterfaceType.HDLC ||
                            client.InterfaceType == InterfaceType.HdlcWithModeE ||
                            client.InterfaceType == InterfaceType.PlcHdlc))
                        {
                            ReadDataBlock(DisconnectRequest(true), "Disconnect request", reply);
                        }
                    }
                    catch (Exception)
                    {
                        //All meters don't support release.
                    }
                }
            }
            finally
            {
                if (media != null)
                {
                    media.Close();
                }
            }
        }

        public void ReadDLMSPacket(byte[] data, GXReplyData reply)
        {
            ReadDLMSPacket(data, parent.ResendCount, reply);
        }

        /// <summary>
        /// Read DLMS Data from the device.
        /// </summary>
        /// <remarks>
        /// If access is denied return null.
        /// </remarks>
        /// <param name="data">Data to send.</param>
        /// <returns>Received data.</returns>
        public void ReadDLMSPacket(byte[] data, int tryCount, GXReplyData reply)
        {
            if ((data == null || data.Length == 0) && !reply.IsStreaming())
            {
                return;
            }
            if (parent.FrameDelay > 0)
            {
                double delay = (DateTime.Now - FrameSendTime).TotalMilliseconds;
                if (delay < parent.FrameDelay)
                {
                    Thread.Sleep(parent.FrameDelay - (int) delay);
                }
                FrameSendTime = DateTime.Now;
            }
            GXReplyData notify = new GXReplyData();
            reply.Error = 0;
            object eop = (byte)0x7E;
            //In network connection terminator is not used.
            if (client.InterfaceType != InterfaceType.HDLC &&
                client.InterfaceType != InterfaceType.HdlcWithModeE &&
                client.InterfaceType != InterfaceType.PlcHdlc)
            {
                eop = null;
            }
            int pos = 0;
            bool succeeded = false;
            ReceiveParameters<byte[]> p = new ReceiveParameters<byte[]>()
            {
                AllData = client.InterfaceType == InterfaceType.PDU,
                Eop = eop,
                Count = eop == null ? 8 : 5,
                //If wait time is negative it's ms.
                WaitTime = parent.WaitTime > 0 ? parent.WaitTime * 1000 : -parent.WaitTime,
            };
            DateTime start = DateTime.Now;
            GXByteBuffer rd = new GXByteBuffer();
            lock (media.Synchronous)
            {
                if (!media.IsOpen)
                {
                    throw new InvalidOperationException("Media is closed.");
                }
                if (data != null)
                {
                    media.Send(data, null);
                    start = DateTime.Now;
                }
                do
                {
                    if (!media.IsOpen)
                    {
                        throw new InvalidOperationException("Media is closed.");
                    }
                    if (reply.Broadcast)
                    {
                        return;
                    }
                    succeeded = media.Receive(p);
                    if (!succeeded)
                    {
                        //Try to read again...
                        if (++pos < tryCount)
                        {
                            //If Eop is not set read one byte at time.
                            if (p.Eop == null)
                            {
                                p.Count = 1;
                            }
                            string log = "Data send failed. Try to resend " + pos.ToString() + "/" + tryCount;
                            GXLogWriter.WriteLog(log);
                            if (parent.OnTrace != null)
                            {
                                parent.OnTrace(DateTime.Now, parent, log, p.Reply, 0, LogFile, 0);
                            }
                            media.Send(data, null);
                            continue;
                        }
                        string err = "Failed to receive reply from the device in given time.";
                        GXLogWriter.WriteLog(err, p.Reply);
                        if (parent.OnTrace != null)
                        {
                            parent.OnTrace(DateTime.Now, parent, err, p.Reply, 0, LogFile, 0);
                        }
                        throw new TimeoutException(err);
                    }
                }
                while (!succeeded && pos != tryCount);
                rd = new GXByteBuffer(p.Reply);
                try
                {
                    pos = 0;
                    if (client.InterfaceType == InterfaceType.PDU)
                    {
                        p.WaitTime = client.Pdu.WaitTime;
                        p.Reply = null;
                        p.Count = 1;
                        media.Receive(p);
                    }
                    while (rd.GetUInt8(0) == '\t')
                    {
                        pos = 1;
                        while (pos < rd.Size)
                        {
                            if (rd.GetUInt8(pos) == 0)
                            {
                                ++pos;
                                if (parent.OnEvent != null)
                                {
                                    parent.OnEvent(media, new ReceiveEventArgs(rd.SubArray(0, pos), media.ToString()));
                                }
                                rd.Position = pos;
                                rd.Trim();
                                break;
                            }
                            ++pos;
                        }
                        pos = 0;
                    }

                    //Loop until whole COSEM packet is received.
                    while (!client.GetData(rd, reply, notify))
                    {
                        int framePosition = rd.Position;
                        rd.Position = 0;
                        if (rd.Compare(data))
                        {
                            rd.Clear();
                        }
                        else
                        {
                            rd.Position = framePosition;
                        }
                        p.Reply = null;
                        if (notify.Data.Size != 0)
                        {
                            // Handle notify.
                            if (!notify.IsMoreData)
                            {
                                if (parent.OnEvent != null)
                                {
                                    parent.OnEvent(media, new ReceiveEventArgs(rd.Array(), media.ToString()));
                                }
                                rd.Trim();
                                notify.Clear();
                                p.Eop = eop;
                            }
                            continue;
                        }
                        //If Eop is not set read one byte at time.
                        if (p.Eop == null)
                        {
                            p.Count = client.GetFrameSize(rd);
                        }
                        if (!media.IsOpen)
                        {
                            throw new InvalidOperationException("Media is closed.");
                        }
                        if (!media.Receive(p))
                        {
                            string err;
                            //Try to read again...
                            if (++pos <= tryCount)
                            {
                                err = "Data send failed. Try to resend " + pos.ToString() + "/3";
                                System.Diagnostics.Debug.WriteLine(err);
                                GXLogWriter.WriteLog(err, data);
                                if (parent.OnTrace != null)
                                {
                                    parent.OnTrace(DateTime.Now, parent, err, rd.Array(), 0, LogFile, 0);
                                }
                                p.Reply = null;
                                media.Send(data, null);
                                continue;
                            }
                            err = "Failed to receive reply from the device in given time.";
                            GXLogWriter.WriteLog(err, rd.Array());
                            if (parent.OnTrace != null)
                            {
                                parent.OnTrace(DateTime.Now, parent, err, rd.Array(), 0, LogFile, 0);
                            }
                            throw new TimeoutException(err);
                        }
                        rd.Position = 0;
                        rd.Set(p.Reply);
                    }
                }
                catch (Exception)
                {
                    if (rd.Size != 0)
                    {
                        GXLogWriter.WriteLog(null, rd.Array());
                        if (parent.OnTrace != null)
                        {
                            int size = 0;
                            if (reply.Data != null)
                            {
                                size = reply.Data.Size;
                            }
                            if (Properties.Settings.Default.TraceTime)
                            {
                                parent.OnTrace(DateTime.Now, parent, "\r\nRX:\t", rd.Array(), size, LogFile, (int)(DateTime.Now - start).TotalMilliseconds);
                            }
                            else
                            {
                                parent.OnTrace(DateTime.Now, parent, "RX:\t", rd.Array(), size, LogFile, (int)(DateTime.Now - start).TotalMilliseconds);
                            }
                        }
                        if (Properties.Settings.Default.LogDuration)
                        {
                            GXLogWriter.WriteLog("Duration: " + ((int)(DateTime.Now - start).TotalMilliseconds).ToString(), false);
                        }
                    }
                    //Throw original exception.
                    throw;
                }
            }
            GXLogWriter.WriteLog(null, rd.Array());
            if (parent.OnTrace != null)
            {
                int size = 0;
                if (reply.Data != null)
                {
                    size = reply.Data.Size;
                }
                if (Properties.Settings.Default.TraceTime)
                {
                    parent.OnTrace(DateTime.Now, parent, "\r\nRX:\t", rd.Array(), size, LogFile, (int)(DateTime.Now - start).TotalMilliseconds);
                }
                else
                {
                    parent.OnTrace(DateTime.Now, parent, "RX:\t", rd.Array(), size, LogFile, (int)(DateTime.Now - start).TotalMilliseconds);
                }
            }
            if (Properties.Settings.Default.LogDuration)
            {
                GXLogWriter.WriteLog("Duration: " + ((int)(DateTime.Now - start).TotalMilliseconds).ToString(), false);
            }

            if (reply.Error != 0)
            {
                throw new GXDLMSException(reply.Error);
            }
        }

        private char GetIecBaudRate(int baudrate)
        {
            char rate;
            switch (baudrate)
            {
                case 300:
                    rate = '0';
                    break;
                case 600:
                    rate = '1';
                    break;
                case 1200:
                    rate = '2';
                    break;
                case 2400:
                    rate = '3';
                    break;
                case 4800:
                    rate = '4';
                    break;
                case 9600:
                    rate = '5';
                    break;
                case 19200:
                    rate = '6';
                    break;
                case 38400:
                    rate = '7';
                    break;
                case 57600:
                    rate = '8';
                    break;
                case 115200:
                    rate = '9';
                    break;
                default:
                    throw new Exception("Unknown baud rate.");
            }
            return rate;
        }

        /// <summary>
        /// Send IEC disconnect message.
        /// </summary>
        void DiscIEC()
        {
            ReceiveParameters<string> p = new ReceiveParameters<string>()
            {
                AllData = false,
                Eop = (byte)0x0A,
                WaitTime = parent.WaitTime * 1000
            };
            string data = (char)0x01 + "B0" + (char)0x03 + "\r\n";
            media.Send(data, null);
            p.Count = 1;
            media.Receive(p);
        }

        internal string InitializeIEC()
        {
            GXManufacturer manufacturer = null;
            string manufactureID = null;
            if (parent.Manufacturer != null)
            {
                manufacturer = this.parent.Manufacturers.FindByIdentification(parent.Manufacturer);
                if (manufacturer == null)
                {
                    throw new Exception("Unknown manufacturer " + parent.Manufacturer);
                }
            }
            GXSerial serial = media as GXSerial;
            byte Terminator = (byte)0x0A;
            if (!media.IsOpen)
            {
                media.Open();
            }
            if (media is GXSerial)
            {
                //Some meters need a little break.
                Thread.Sleep(1000);
            }
            //Query device information.
            if (parent.InterfaceType == InterfaceType.HdlcWithModeE)
            {
                string data = "/?!\r\n";
                if (manufacturer != null && !string.IsNullOrEmpty(manufacturer.IecAddress))
                {
                    data = manufacturer.IecAddress + "\r\n";
                }
                else
                {
                    if (this.parent.HDLCAddressing == HDLCAddressType.SerialNumber)
                    {
                        data = "/?" + this.parent.PhysicalAddress + "!\r\n";
                    }
                }
                GXLogWriter.WriteLog("IEC Sending:" + data);
                ReceiveParameters<string> p = new ReceiveParameters<string>()
                {
                    AllData = false,
                    Eop = Terminator,
                    WaitTime = parent.WaitTime * 1000
                };
                lock (media.Synchronous)
                {
                    media.Send(data, null);
                    if (!media.Receive(p))
                    {
                        //Try to move away from mode E.
                        try
                        {
                            GXReplyData reply = new GXReplyData();
                            this.ReadDLMSPacket(this.DisconnectRequest(), 1, reply);
                        }
                        catch (Exception)
                        {
                        }
                        DiscIEC();
                        string str = "Failed to receive reply from the device in given time.";
                        GXLogWriter.WriteLog(str);
                        media.Send(data, null);
                        if (!media.Receive(p))
                        {
                            throw new Exception(str);
                        }
                    }
                    //If echo is used.
                    if (p.Reply == data)
                    {
                        p.Reply = null;
                        if (!media.Receive(p))
                        {
                            //Try to move away from mode E.
                            GXReplyData reply = new GXReplyData();
                            this.ReadDLMSPacket(this.DisconnectRequest(), 1, reply);
                            if (serial != null)
                            {
                                DiscIEC();
                                serial.DtrEnable = serial.RtsEnable = false;
                                serial.BaudRate = 9600;
                                serial.DtrEnable = serial.RtsEnable = true;
                                DiscIEC();
                            }
                            data = "Failed to receive reply from the device in given time.";
                            GXLogWriter.WriteLog(data);
                            throw new Exception(data);
                        }
                    }
                }
                int pos = 0;
                //With some meters there might be some extra invalid chars. Remove them.
                if (p.Reply != null)
                {
                    p.Reply = p.Reply.Replace("?", "");
                }
                GXLogWriter.WriteLog("HDLC received: " + p.Reply);
                if (p.Reply[pos] != '/')
                {
                    p.WaitTime = 100;
                    media.Receive(p);
                    throw new Exception("Invalid responce.");
                }
                manufactureID = p.Reply.Substring(pos + 1, 3);
                UpdateManufactureSettings(manufactureID);
                char baudrate = p.Reply[pos + 4];
                int BaudRate = 0;
                switch (baudrate)
                {
                    case '0':
                        BaudRate = 300;
                        break;
                    case '1':
                        BaudRate = 600;
                        break;
                    case '2':
                        BaudRate = 1200;
                        break;
                    case '3':
                        BaudRate = 2400;
                        break;
                    case '4':
                        BaudRate = 4800;
                        break;
                    case '5':
                        BaudRate = 9600;
                        break;
                    case '6':
                        BaudRate = 19200;
                        break;
                    case '7':
                        BaudRate = 38400;
                        break;
                    case '8':
                        BaudRate = 57600;
                        break;
                    case '9':
                        BaudRate = 115200;
                        break;
                    default:
                        throw new Exception("Unknown baud rate.");
                }
                if (parent.MaximumBaudRate != 0)
                {
                    BaudRate = parent.MaximumBaudRate;
                    baudrate = GetIecBaudRate(BaudRate);
                    GXLogWriter.WriteLog("Maximum BaudRate is set to : " + BaudRate.ToString());
                }
                GXLogWriter.WriteLog("BaudRate is : " + BaudRate.ToString());
                //Send ACK
                //Send Protocol control character
                // "2" HDLC protocol procedure (Mode E)
                byte controlCharacter = (byte)'2';
                //Send Baud rate character
                //Mode control character
                byte ModeControlCharacter = (byte)'2';
                //"2" //(HDLC protocol procedure) (Binary mode)
                //Set mode E.
                byte[] arr = new byte[] { 0x06, controlCharacter, (byte)baudrate, ModeControlCharacter, 13, 10 };
                GXLogWriter.WriteLog("Moving to mode E.", arr);
                lock (media.Synchronous)
                {
                    p.Reply = null;
                    media.Send(arr, null);
                    p.WaitTime = 2000;
                    //Note! All meters do not echo this.
                    media.Receive(p);
                    if (p.Reply != null)
                    {
                        GXLogWriter.WriteLog("Received: " + p.Reply);
                    }
                    if (serial != null)
                    {
                        media.Close();
                        serial.BaudRate = BaudRate;
                        serial.DataBits = 8;
                        serial.Parity = Parity.None;
                        serial.StopBits = StopBits.One;
                        media.Open();
                    }
                    //Some meters need this sleep. Do not remove.
                    Thread.Sleep(1000);
                }
            }
            return manufactureID;
        }

        void Media_OnTrace(object sender, TraceEventArgs e)
        {
            GXLogWriter.WriteLog(e.ToString());
            if (parent.OnTrace != null)
            {
                parent.OnTrace(DateTime.Now, parent, e.ToString(), null, 0, LogFile, 0);
            }
        }

        /// <summary>
        /// Initialize network connection settings.
        /// </summary>
        /// <returns></returns>
        void InitNet()
        {
            try
            {
                if (parent.UseRemoteSerial)
                {
                    InitializeIEC();
                }
                else
                {
                    media.Open();
                }
            }
            catch (Exception Ex)
            {
                if (media != null)
                {
                    media.Close();
                }
                throw Ex;
            }
        }

        public int ClientAddress
        {
            get
            {
                return client.ClientAddress;
            }
        }

        public int ServerAddress
        {
            get
            {
                return client.ServerAddress;
            }
        }

        public void UpdateManufactureSettings(string id)
        {
            if (!media.IsOpen)
            {
                media.Settings = parent.MediaSettings;
            }
            if (!string.IsNullOrEmpty(this.parent.Manufacturer) && string.Compare(this.parent.Manufacturer, id, true) != 0)
            {
                throw new Exception(string.Format("Manufacturer type does not match. Manufacturer is {0} and it should be {1}.", id, this.parent.Manufacturer));
            }
            GXManufacturer manufacturer = null;
            if (!string.IsNullOrEmpty(this.parent.Manufacturer))
            {
                manufacturer = this.parent.Manufacturers.FindByIdentification(parent.Manufacturer);
                if (manufacturer == null)
                {
                    throw new Exception("Unknown manufacturer " + id);
                }
                parent.Manufacturer = manufacturer.Identification;
            }
            client.Standard = this.parent.Standard;
            client.Authentication = this.parent.Authentication;
            client.InterfaceType = InterfaceType.HDLC;
            client.UseProtectedRelease = parent.UseProtectedRelease;
            if (!string.IsNullOrEmpty(this.parent.Password))
            {
                client.Password = CryptHelper.Decrypt(this.parent.Password, Password.Key);
            }
            else if (this.parent.HexPassword != null)
            {
                client.Password = CryptHelper.Decrypt(this.parent.HexPassword, Password.Key);
            }
            //Update client signing key.
            GXPkcs8 pk = null;
            if ((parent.Authentication == Authentication.HighECDSA
                || parent.Signing != Signing.None)
                && client.Ciphering.SigningKeyPair.Value == null
                && !string.IsNullOrEmpty(parent.ClientSigningKey))
            {
                if (client.Ciphering.SigningKeyPair.Key == null && !string.IsNullOrEmpty(parent.ServerSigningKey))
                {
                    GXx509Certificate cert = GXx509Certificate.FromDer(parent.ServerSigningKey);
                    client.Ciphering.SigningKeyPair = new KeyValuePair<GXPublicKey, GXPrivateKey>(cert.PublicKey, null);
                }
                pk = GXPkcs8.FromDer(parent.ClientSigningKey);
                client.Ciphering.SigningKeyPair = new KeyValuePair<GXPublicKey, GXPrivateKey>(client.Ciphering.SigningKeyPair.Key, pk.PrivateKey);
            }
            client.UseLogicalNameReferencing = this.parent.UseLogicalNameReferencing;
            client.ProposedConformance = (Conformance)parent.Conformance;
            client.UseUtc2NormalTime = parent.UtcTimeZone;
            client.DateTimeSkips = parent.DateTimeSkips;
            //Show media verbose.
            if (this.parent.Verbose && media.Trace != System.Diagnostics.TraceLevel.Verbose)
            {
                media.Trace = System.Diagnostics.TraceLevel.Verbose;
                media.OnTrace += new TraceEventHandler(Media_OnTrace);
            }
            else if (!this.parent.Verbose && media.Trace == System.Diagnostics.TraceLevel.Verbose)
            {
                media.Trace = System.Diagnostics.TraceLevel.Off;
                media.OnTrace -= new TraceEventHandler(Media_OnTrace);
            }

            //If network media is used check is manufacturer supporting IEC 62056-47
            if (client.InterfaceType == InterfaceType.WRAPPER && (parent.UseRemoteSerial || this.media is GXSerial))
            {
                client.InterfaceType = InterfaceType.HDLC;
            }

            client.Broacast = parent.Broadcast;
            client.ClientAddress = parent.ClientAddress;
            if (parent.HDLCAddressing == HDLCAddressType.SerialNumber && manufacturer != null)
            {
                string formula = null;
                GXServerAddress server = manufacturer.GetServer(parent.HDLCAddressing);
                if (server != null)
                {
                    formula = server.Formula;
                }
                client.ServerAddressSize = 4;
                if (client.InterfaceType == InterfaceType.HDLC || client.InterfaceType == InterfaceType.HdlcWithModeE)
                {
                    client.ServerAddress = GXDLMSClient.GetServerAddressFromSerialNumber(Convert.ToInt32(parent.PhysicalAddress), parent.LogicalAddress, formula);
                }
                else
                {
                    client.ServerAddress = GXDLMSClient.GetServerAddressFromSerialNumber(Convert.ToInt32(parent.PhysicalAddress), 0, formula);
                }
            }
            else
            {
                if (client.InterfaceType == InterfaceType.WRAPPER)
                {
                    client.ServerAddress = Convert.ToInt32(parent.PhysicalAddress);
                }
                else
                {
                    client.ServerAddress = GXDLMSClient.GetServerAddress(parent.LogicalAddress, Convert.ToInt32(parent.PhysicalAddress), parent.ServerAddressSize);
                    client.ServerAddressSize = parent.ServerAddressSize;
                }
            }
            client.Ciphering.Signing = parent.Signing;
            client.Ciphering.Security = parent.Security;
            client.Ciphering.SecuritySuite = parent.SecuritySuite;
            if (client.Authentication == Authentication.HighECDSA ||
                client.Ciphering.Signing == Signing.OnePassDiffieHellman ||
                client.Ciphering.Signing == Signing.StaticUnifiedModel)
            {
                GXx509Certificate pub;
                if ((client.Ciphering.Signing == Signing.OnePassDiffieHellman ||
                    client.Ciphering.Signing == Signing.StaticUnifiedModel) &&
                    string.IsNullOrEmpty(parent.ClientAgreementKey))
                {
                    throw new Exception("Client agreement key is not set.");
                }
                if (parent.ClientAgreementKey != null && parent.ServerAgreementKey != null)
                {
                    pk = GXPkcs8.FromDer(parent.ClientAgreementKey);
                    pub = GXx509Certificate.FromDer(parent.ServerAgreementKey);
                    client.Ciphering.KeyAgreementKeyPair = new KeyValuePair<GXPublicKey, GXPrivateKey>(pub.PublicKey, pk.PrivateKey);
                }
                if (parent.ClientSigningKey != null)
                {
                    pk = GXPkcs8.FromDer(parent.ClientSigningKey);
                    if (string.IsNullOrEmpty(parent.ServerSigningKey))
                    {
                        pub = null;
                    }
                    else
                    {
                        pub = GXx509Certificate.FromDer(parent.ServerSigningKey);
                    }
                    client.Ciphering.SigningKeyPair = new KeyValuePair<GXPublicKey, GXPrivateKey>(pub != null ? pub.PublicKey : null, pk.PrivateKey);
                }
            }
            if (parent.SystemTitle != null && parent.BlockCipherKey != null && parent.AuthenticationKey != null)
            {
                client.Ciphering.SystemTitle = GXCommon.HexToBytes(parent.SystemTitle);
                client.Ciphering.BlockCipherKey = GXCommon.HexToBytes(parent.BlockCipherKey);
                client.Ciphering.AuthenticationKey = GXCommon.HexToBytes(parent.AuthenticationKey);
                client.Ciphering.BroadcastBlockCipherKey = GXCommon.HexToBytes(parent.BroadcastKey);
                client.Ciphering.InvocationCounter = parent.InvocationCounter;
            }
            else
            {
                client.Ciphering.SystemTitle = null;
                client.Ciphering.BlockCipherKey = null;
                client.Ciphering.AuthenticationKey = null;
                client.Ciphering.BroadcastBlockCipherKey = null;
                client.Ciphering.InvocationCounter = 0;
            }

            if (!string.IsNullOrEmpty(parent.Challenge))
            {
                client.CtoSChallenge = GXCommon.HexToBytes(parent.Challenge);
            }
            else
            {
                client.CtoSChallenge = null;
            }

            if (!string.IsNullOrEmpty(parent.PhysicalDeviceAddress))
            {
                client.Gateway = new GXDLMSGateway();
                client.Gateway.NetworkId = parent.NetworkId;
                client.Gateway.PhysicalDeviceAddress = GXCommon.HexToBytes(parent.PhysicalDeviceAddress);
            }
            else
            {
                client.Gateway = null;
            }
        }

        /// <summary>
        /// Initialize serial port connection to COSEM/DLMS device.
        /// </summary>
        /// <returns></returns>
        void InitSerial()
        {
            try
            {
                InitializeIEC();
            }
            catch (Exception Ex)
            {
                if (media != null)
                {
                    media.Close();
                }
                throw Ex;
            }
        }

        /// <summary>
        /// Initialize serial port connection to COSEM/DLMS device.
        /// </summary>
        /// <returns></returns>
        void InitTerminal()
        {
            try
            {
                InitializeIEC();
            }
            catch (Exception Ex)
            {
                if (media != null)
                {
                    media.Close();
                }
                throw Ex;
            }
        }

        public void UpdateSettings()
        {
            if (!media.IsOpen)
            {
                media.Settings = parent.MediaSettings;
            }
            client.Authentication = this.parent.Authentication;
            client.InterfaceType = parent.InterfaceType;
            client.Plc.MacSourceAddress = parent.MACSourceAddress;
            client.Plc.MacDestinationAddress = parent.MacDestinationAddress;
            if (!string.IsNullOrEmpty(this.parent.Password))
            {
                client.Password = CryptHelper.Decrypt(this.parent.Password, Password.Key);
            }
            else if (this.parent.HexPassword != null)
            {
                client.Password = CryptHelper.Decrypt(this.parent.HexPassword, Password.Key);
            }
            client.UseLogicalNameReferencing = this.parent.UseLogicalNameReferencing;
            client.UseUtc2NormalTime = parent.UtcTimeZone;
            //Show media verbose.
            if (this.parent.Verbose && media.Trace != System.Diagnostics.TraceLevel.Verbose)
            {
                media.Trace = System.Diagnostics.TraceLevel.Verbose;
                media.OnTrace += new TraceEventHandler(Media_OnTrace);
            }
            else if (!this.parent.Verbose && media.Trace == System.Diagnostics.TraceLevel.Verbose)
            {
                media.Trace = System.Diagnostics.TraceLevel.Off;
                media.OnTrace -= new TraceEventHandler(Media_OnTrace);
            }
            client.ClientAddress = parent.ClientAddress;
            if (parent.HDLCAddressing == HDLCAddressType.SerialNumber)
            {
                string formula = null;
                GXManufacturer manufacturer = this.parent.Manufacturers.FindByIdentification(parent.Manufacturer);
                GXServerAddress server = manufacturer.GetServer(parent.HDLCAddressing);
                if (server != null)
                {
                    formula = server.Formula;
                }
                if (client.InterfaceType == InterfaceType.HDLC || client.InterfaceType == InterfaceType.HdlcWithModeE)
                {
                    client.ServerAddress = GXDLMSClient.GetServerAddressFromSerialNumber(Convert.ToInt32(parent.PhysicalAddress), parent.LogicalAddress, formula);
                }
                else
                {
                    client.ServerAddress = GXDLMSClient.GetServerAddressFromSerialNumber(Convert.ToInt32(parent.PhysicalAddress), 0, formula);
                }
                client.ServerAddressSize = 4;
            }
            else
            {
                if (client.InterfaceType == InterfaceType.HDLC || client.InterfaceType == InterfaceType.HdlcWithModeE || client.InterfaceType == InterfaceType.PlcHdlc)
                {
                    client.ServerAddress = GXDLMSClient.GetServerAddress(parent.LogicalAddress, Convert.ToInt32(parent.PhysicalAddress), parent.ServerAddressSize);
                    client.ServerAddressSize = parent.ServerAddressSize;
                }
                else
                {
                    client.ServerAddress = Convert.ToInt32(parent.PhysicalAddress);
                }
            }
            client.Ciphering.Security = parent.Security;
            if (parent.SystemTitle != null && parent.BlockCipherKey != null && parent.AuthenticationKey != null)
            {
                client.Ciphering.SystemTitle = GXCommon.HexToBytes(parent.SystemTitle);
                client.Ciphering.BlockCipherKey = GXCommon.HexToBytes(parent.BlockCipherKey);
                client.Ciphering.AuthenticationKey = GXCommon.HexToBytes(parent.AuthenticationKey);
                client.Ciphering.InvocationCounter = parent.InvocationCounter;
            }
            else
            {
                client.Ciphering.SystemTitle = null;
                client.Ciphering.BlockCipherKey = null;
                client.Ciphering.AuthenticationKey = null;
                client.Ciphering.InvocationCounter = 0;
            }

            if (!string.IsNullOrEmpty(parent.Challenge))
            {
                client.CtoSChallenge = GXCommon.HexToBytes(parent.Challenge);
            }
            else
            {
                client.CtoSChallenge = null;
            }
            if (!string.IsNullOrEmpty(parent.DedicatedKey))
            {
                client.Ciphering.DedicatedKey = GXCommon.HexToBytes(parent.DedicatedKey);
            }
            else
            {
                client.Ciphering.DedicatedKey = null;
            }
            client.HdlcSettings.WindowSizeRX = parent.WindowSizeRX;
            client.HdlcSettings.WindowSizeTX = parent.WindowSizeTX;
            client.HdlcSettings.UseFrameSize = parent.UseFrameSize;
            client.HdlcSettings.MaxInfoRX = parent.MaxInfoRX;
            client.HdlcSettings.MaxInfoTX = parent.MaxInfoTX;
            client.MaxReceivePDUSize = parent.PduSize;
            client.GbtWindowSize = parent.GbtWindowSize;
            client.UserId = parent.UserId;
            client.Priority = parent.Priority;
            client.ServiceClass = parent.ServiceClass;
            if (parent.PreEstablished)
            {
                client.ServerSystemTitle = GXCommon.HexToBytes(parent.ServerSystemTitle);
            }
            client.ChallengeSize = parent.ChallengeSize;
            client.OverwriteAttributeAccessRights = parent.OverwriteAttributeAccessRights;
            client.IncreaseInvocationCounterForGMacAuthentication = parent.IncreaseInvocationCounterForGMacAuthentication;
        }

        public void InitializeConnection(bool force)
        {
            if (force || !media.IsOpen)
            {
                if (!string.IsNullOrEmpty(parent.Manufacturer))
                {
                    UpdateManufactureSettings(parent.Manufacturer);
                }
                if (media is GXSerial)
                {
                    GXLogWriter.WriteLog("Initializing serial connection.");
                    InitSerial();
                    connectionStartTime = DateTime.Now;
                }
                else if (media is GXNet)
                {
                    GXLogWriter.WriteLog("Initializing Network connection.");
                    InitNet();
                    //Some Electricity meters need some time before first message can be send.
                    System.Threading.Thread.Sleep(500);
                }
                else if (media is Gurux.Terminal.GXTerminal)
                {
                    GXLogWriter.WriteLog("Initializing Terminal connection.");
                    InitTerminal();
                }
                else
                {
                    if (media is IGXMedia2)
                    {
                        ((IGXMedia2)media).AsyncWaitTime = (uint)parent.WaitTime;
                    }
                    media.Open();
                }
            }
            GXReplyData reply = new GXReplyData();
            try
            {
                client.ManufacturerId = parent.Manufacturer;
                byte[] data;
                UpdateSettings();
                //Read frame counter if GeneralProtection is used.
                if (!string.IsNullOrEmpty(parent.FrameCounter) && client.Ciphering != null && client.Ciphering.Security != Security.None)
                {
                    reply.Clear();
                    int add = client.ClientAddress;
                    Authentication auth = client.Authentication;
                    Security security = client.Ciphering.Security;
                    byte[] challenge = client.CtoSChallenge;
                    Signing signing = client.Ciphering.Signing;
                    byte[] serverSystemTitle = client.ServerSystemTitle;
                    try
                    {
                        client.ClientAddress = 16;
                        client.Authentication = Authentication.None;
                        client.Ciphering.Security = Security.None;
                        client.Ciphering.Signing = Signing.None;
                        client.ServerSystemTitle = null;
                        data = SNRMRequest();
                        if (data != null)
                        {
                            try
                            {
                                ReadDataBlock(data, "Send SNRM request.", 1, 1, reply);
                            }
                            catch (TimeoutException)
                            {
                                reply.Clear();
                                ReadDataBlock(DisconnectRequest(true), "Send Disconnect request.", 1, 1, reply);
                                reply.Clear();
                                ReadDataBlock(data, "Send SNRM request.", 1, 1, reply);
                            }
                            catch (Exception e)
                            {
                                reply.Clear();
                                ReadDataBlock(DisconnectRequest(), "Send Disconnect request.", 1, 1, reply);
                                throw e;
                            }
                            //Has server accepted client.
                            ParseUAResponse(reply.Data);
                            GXLogWriter.WriteLog("Parsing UA reply succeeded.");
                        }
                        ReadDataBlock(AARQRequest(), "Send AARQ request.", reply);
                        try
                        {
                            //Parse reply.
                            ParseAAREResponse(reply.Data);
                            GXLogWriter.WriteLog("Parsing AARE reply succeeded.");
                            reply.Clear();
                            GXDLMSData d = new GXDLMSData(parent.FrameCounter);
                            ReadDLMSPacket(Read(d, 2), reply);
                            client.UpdateValue(d, 2, reply.Value);
                            client.Ciphering.InvocationCounter = parent.InvocationCounter = 1 + Convert.ToUInt32(d.Value);
                            reply.Clear();
                            if (parent.InterfaceType == InterfaceType.HdlcWithModeE)
                            {
                                Disconnect();
                                //Initialize IEC again for optical port connection.
                                media.Settings = parent.MediaSettings;
                                InitializeIEC();
                            }
                            else
                            {
                                ReadDataBlock(DisconnectRequest(), "Disconnect request", reply);
                            }
                        }
                        catch (Exception Ex)
                        {
                            reply.Clear();
                            ReadDataBlock(DisconnectRequest(), "Disconnect request", reply);
                            throw Ex;
                        }
                    }
                    finally
                    {
                        client.ClientAddress = add;
                        client.Authentication = auth;
                        client.Ciphering.Security = security;
                        client.CtoSChallenge = challenge;
                        client.Ciphering.Signing = signing;
                        client.ServerSystemTitle = serverSystemTitle;
                    }
                }
                if (!parent.IgnoreSNRMWithPreEstablished)
                {
                    data = SNRMRequest();
                    if (data != null)
                    {
                        try
                        {
                            reply.Clear();
                            ReadDataBlock(data, "Send SNRM request.", 1, parent.ResendCount, reply);
                        }
                        catch (TimeoutException)
                        {
                            reply.Clear();
                            ReadDataBlock(DisconnectRequest(true), "Send Disconnect request.", 1, parent.ResendCount, reply);
                            reply.Clear();
                            ReadDataBlock(data, "Send SNRM request.", reply);
                        }
                        catch (Exception e)
                        {
                            reply.Clear();
                            ReadDataBlock(DisconnectRequest(), "Send Disconnect request.", reply);
                            throw e;
                        }
                        GXLogWriter.WriteLog("Parsing UA reply succeeded.");
                        //Has server accepted client.
                        ParseUAResponse(reply.Data);
                    }
                }
                if (!parent.PreEstablished)
                {
                    //Generate AARQ request.
                    //Split requests to multiple packets if needed.
                    //If password is used all data might not fit to one packet.
                    reply.Clear();
                    ReadDataBlock(AARQRequest(), "Send AARQ request.", reply);
                    try
                    {
                        //Parse reply.
                        ParseAAREResponse(reply.Data);
                        GXLogWriter.WriteLog("Parsing AARE reply succeeded.");
                    }
                    catch (Exception Ex)
                    {
                        reply.Clear();
                        try
                        {
                            ReadDLMSPacket(DisconnectRequest(), 1, reply);
                        }
                        catch (Exception)
                        {
                            //It's OK if disconnect fails.
                        }
                        throw;
                    }
                    //If authentication is required.
                    if (client.Authentication > Authentication.Low)
                    {
                        reply.Clear();
                        ReadDataBlock(client.GetApplicationAssociationRequest(), "Authenticating.", reply);
                        client.ParseApplicationAssociationResponse(reply.Data);
                    }
                    parent.KeepAliveStart();
                }
            }
            catch (Exception)
            {
                reply.Clear();
                ReadDLMSPacket(ReleaseRequest(), 1, reply);
                reply.Clear();
                ReadDLMSPacket(DisconnectRequest(), 1, reply);
                if (media is GXSerial && parent.InterfaceType == InterfaceType.HdlcWithModeE)
                {
                    ReceiveParameters<string> p = new ReceiveParameters<string>()
                    {
                        Eop = (byte)0xA,
                        WaitTime = parent.WaitTime * 1000
                    };
                    lock (media.Synchronous)
                    {
                        string data = (char)0x01 + "B0" + (char)0x03 + "\r\n";
                        media.Send(data, null);
                        media.Receive(p);
                    }
                }
                throw;
            }
        }

        void NotifyProgress(string description, int current, int maximium)
        {
            if (OnProgress != null)
            {
                OnProgress(this, description, current, maximium);
            }
        }

        void ReadDataBlock(byte[][] data, string text, GXReplyData reply)
        {
            foreach (byte[] it in data)
            {
                reply.Clear(); try
                {
                    ReadDataBlock(it, text, reply);
                }
                catch (Exception)
                {
                    //Update frame ID if meter returns error.
                    if (client.InterfaceType == InterfaceType.HDLC ||
                        client.InterfaceType == InterfaceType.HdlcWithModeE ||
                        client.InterfaceType == InterfaceType.PlcHdlc)
                    {
                        int target, source;
                        byte type;
                        GXDLMSClient.GetHdlcAddressInfo(new GXByteBuffer(it), out target, out source, out type);
                        client.HdlcSettings.SenderFrame = type;
                    }
                    throw;
                }
            }
        }

        void ReadDataBlock(byte[] data, string text, GXReplyData reply)
        {
            ReadDataBlock(data, text, 1, reply);
        }

        public delegate void DataReceivedEventHandler(object sender, GXReplyData reply);
        public event DataReceivedEventHandler OnDataReceived;
        GXDLMSProfileGeneric CurrentProfileGeneric;

        void OnProfileGenericDataReceived(object sender, GXReplyData reply)
        {
            if (reply.Value != null)
            {
                lock (reply)
                {
                    client.UpdateValue(CurrentProfileGeneric, 2, reply.Value);
                    reply.Value = new Object[0];
                }
                if (OnAfterRead != null)
                {
                    OnAfterRead(null, CurrentProfileGeneric, 2, null, null, null);
                }
            }
        }

        internal void ReadDataBlock(byte[][] data, string text, int multiplier, int tryCount, GXReplyData reply)
        {
            foreach (byte[] it in data)
            {
                reply.Clear();
                try
                {
                    ReadDataBlock(it, text, multiplier, tryCount, reply);
                }
                catch (Exception)
                {
                    //Update frame ID if meter returns error.
                    if (client.InterfaceType == InterfaceType.HDLC ||
                        client.InterfaceType == InterfaceType.HdlcWithModeE ||
                        client.InterfaceType == InterfaceType.PlcHdlc)
                    {
                        int target, source;
                        byte type;
                        GXDLMSClient.GetHdlcAddressInfo(new GXByteBuffer(it), out target, out source, out type);
                        client.HdlcSettings.SenderFrame = type;
                    }
                    throw;
                }
            }
        }

        internal void ReadDataBlock(byte[][] data, string text, int multiplier, GXReplyData reply)
        {
            foreach (byte[] it in data)
            {
                reply.Clear();
                try
                {
                    ReadDataBlock(it, text, multiplier, reply);
                }
                catch (Exception ex)
                {
                    //Update frame ID if meter returns error.
                    if (client.InterfaceType == InterfaceType.HDLC ||
                        client.InterfaceType == InterfaceType.HdlcWithModeE ||
                        client.InterfaceType == InterfaceType.PlcHdlc)
                    {
                        int target, source;
                        byte type;
                        GXDLMSClient.GetHdlcAddressInfo(new GXByteBuffer(it), out target, out source, out type);
                        client.HdlcSettings.SenderFrame = type;
                    }
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Read data block from the device.
        /// </summary>
        /// <param name="data">data to send</param>
        /// <param name="text">Progress text.</param>
        /// <param name="multiplier"></param>
        /// <returns>Received data.</returns>
        internal void ReadDataBlock(byte[] data, string text, int multiplier, GXReplyData reply)
        {
            if (data == null)
            {
                return;
            }
            ReadDataBlock(data, text, multiplier, parent.ResendCount, reply);
        }

        /// <summary>
        /// Read data block from the device.
        /// </summary>
        /// <param name="data">data to send</param>
        /// <param name="text">Progress text.</param>
        /// <param name="multiplier"></param>
        /// <returns>Received data.</returns>
        internal void ReadDataBlock(byte[] data, string text, int multiplier, int tryCount, GXReplyData reply)
        {
            lastTransaction = DateTime.Now;
            GXLogWriter.WriteLog(text, data);
            if (parent.OnTrace != null)
            {
                parent.OnTrace(lastTransaction, parent, text + "\r\nTX:\t", data, 0, LogFile, 0);
            }
            //Synchronous lock is needed because GBT might cause that there are multiple replies and 
            //we don't want to handle them as notifications.
            lock (media.Synchronous)
            {
                ReadDLMSPacket(data, tryCount, reply);
                if (OnDataReceived != null)
                {
                    OnDataReceived(this, reply);
                }
                if (reply.IsMoreData)
                {
                    if (reply.TotalCount != 1)
                    {
                        NotifyProgress(text, 1, multiplier * reply.TotalCount);
                    }
                    while (reply.IsMoreData)
                    {
                        if (!reply.IsStreaming())
                        {
                            data = client.ReceiverReady(reply);
                            if ((reply.MoreData & RequestTypes.Frame) != 0)
                            {
                                GXLogWriter.WriteLog("Get next frame.");
                            }
                            else
                            {
                                GXLogWriter.WriteLog("Get Next Data block.");
                            }
                            if (parent.OnTrace != null)
                            {
                                parent.OnTrace(DateTime.Now, parent, "\r\nTX:\t", data, 0, LogFile, 0);
                            }
                            GXLogWriter.WriteLog(text, data);
                        }
                        else
                        {
                            data = null;
                        }
                        ReadDLMSPacket(data, reply);
                        if (OnDataReceived != null)
                        {
                            OnDataReceived(this, reply);
                        }
                        if (reply.TotalCount != 1)
                        {
                            NotifyProgress(text, reply.Count, multiplier * reply.TotalCount);
                        }
                    }
                }
            }
        }

        public GXDLMSObjectCollection GetObjects()
        {
            GXLogWriter.WriteLog("--- Collecting objects. ---");
            GXDLMSObjectCollection objs;
            GXReplyData reply = new GXReplyData();
            try
            {
                if (OnBeforeRead != null)
                {
                    GXDLMSObject target;
                    if (client.UseLogicalNameReferencing)
                    {
                        target = new GXDLMSAssociationLogicalName();
                    }
                    else
                    {
                        target = new GXDLMSAssociationShortName();
                    }
                    OnBeforeRead(client, target, 2, null, null, null);
                }
                ReadDataBlock(client.GetObjectsRequest(), "Collecting objects", 3, 1, reply);
            }
            catch (Exception Ex)
            {
                if (parent.Standard == Standard.Italy)
                {
                    GXDLMSObject obj;
                    objs = new GXDLMSObjectCollection();
                    GXObisCode[] tmp = GXDLMSConverter.GetObjects(Standard.Italy);
                    foreach (GXObisCode it in tmp)
                    {
                        try
                        {
                            obj = GXDLMSClient.CreateObject(it.ObjectType);
                            obj.LogicalName = it.LogicalName;
                            obj.Description = it.Description;
                            obj.Version = it.Version;
                            ReadValue(obj, 1);
                            if (string.IsNullOrEmpty(obj.LogicalName))
                            {
                                //Some meters are returning invalid data here.
                            }
                            else if (client.CanRead(obj, 1))
                            {
                                objs.Add(obj);
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            //Media is closed.
                            break;
                        }
                        catch (Exception)
                        {
                            //This is not implemented read next.
                        }
                    }
                    obj = objs.FindByLN(ObjectType.AssociationLogicalName, "0.0.40.0.0.255");
                    if (obj != null)
                    {
                        obj.SetAccess(2, AccessMode.NoAccess);
                    }
                    return objs;
                }
                else
                {
                    throw new Exception("GetObjects failed. " + Ex.Message);
                }
            }
            objs = client.ParseObjects(reply.Data, true);
            if (OnAfterRead != null)
            {
                if (client.UseLogicalNameReferencing)
                {
                    GXDLMSObject ln = client.Objects.FindByLN(ObjectType.AssociationLogicalName, "0.0.40.0.0.255");
                    //All meters don't add default association.
                    if (ln == null)
                    {
                        ln = client.Objects.GetObjects(ObjectType.AssociationLogicalName)[0];
                    }
                    OnAfterRead(client, ln, 2, reply.Data, null, null);
                }
                else
                {
                    OnAfterRead(client, client.Objects.FindBySN(0xFA00), 2, reply.Data, null, null);
                }
            }
            GXLogWriter.WriteLog("--- Collecting " + objs.Count.ToString() + " objects. ---");
            return objs;
        }

        class GXAttributeRead
        {
            public PropertyInfo Info;
            public GXDLMSAttribute Attribute;

            public GXAttributeRead(PropertyInfo info, GXDLMSAttribute attribute)
            {
                Info = info;
                Attribute = attribute;
            }
        }

        delegate void ClearProfileGenericDataEventHandler();

        /// <summary>
        /// Read object.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="obj">Object to read.</param>
        /// <param name="forceRead">Force all attributes read.</param>
        public void Read(object sender, GXDLMSObject obj, bool forceRead)
        {
            GXReplyData reply = new GXReplyData();
            int[] indexes = (obj as IGXDLMSBase).GetAttributeIndexToRead(forceRead);
            foreach (int it in indexes)
            {
                //Logical name is not read again.
                if (it == 1)
                {
                    continue;
                }
                //If reading is not allowed.
                if (!client.CanRead(obj, it))
                {
                    obj.ClearStatus(it);
                    continue;
                }

                //If object is static and it's already read.
                if (!forceRead && obj.GetStatic(it) && obj.GetLastReadTime(it) != DateTime.MinValue)
                {
                    continue;
                }
                //Profile generic capture objects is not read here.
                if (forceRead && obj is GXDLMSProfileGeneric && it == 3)
                {
                    continue;
                }
                obj.ClearStatus(it);
                reply.Clear();
                if (obj is GXDLMSProfileGeneric && it == 2)
                {
                    if (OnBeforeRead != null)
                    {
                        OnBeforeRead(null, obj, it, null, null, null);
                    }
                    object parameters = null;
                    PduEventHandler p = new PduEventHandler(delegate (object sender1, byte[] value)
                    {
                        try
                        {
                            GXDLMSTranslator t = new GXDLMSTranslator();
                            t.Hex = false;
                            string xml = null;
                            xml = t.PduToXml(value);
                            XmlDocument doc2 = new XmlDocument();
                            doc2.LoadXml(xml);
                            var tags = doc2.GetElementsByTagName("AccessParameters");
                            if (tags != null && tags.Count != 0)
                            {
                                xml = tags[0].InnerXml;
                                int type = int.Parse(doc2.GetElementsByTagName("AccessSelector")[0].Attributes[0].Value);
                                parameters = new GXStructure() { type, GXDLMSTranslator.XmlToValue(xml) };
                            }
                        }
                        catch (Exception)
                        {
                            //Ignore error.
                        }
                    });
                    try
                    {
                        byte[][] tmp;
                        CurrentProfileGeneric = obj as GXDLMSProfileGeneric;
                        client.OnPdu += p;
                        OnDataReceived += new GXDLMSCommunicator.DataReceivedEventHandler(this.OnProfileGenericDataReceived);
                        if (CurrentProfileGeneric.AccessSelector == AccessRange.Range ||
                                CurrentProfileGeneric.AccessSelector == AccessRange.Last)
                        {
                            GXDateTime start = CurrentProfileGeneric.From as GXDateTime;
                            if (start == null)
                            {
                                start = Convert.ToDateTime(CurrentProfileGeneric.From);
                            }
                            GXDateTime end = CurrentProfileGeneric.To as GXDateTime;
                            if (end == null)
                            {
                                end = Convert.ToDateTime(CurrentProfileGeneric.To);
                            }
                            //Set seconds to zero.
                            start.Value = start.Value.AddSeconds(-start.Value.Second);
                            end.Value = end.Value.AddSeconds(-end.Value.Second);
                            tmp = client.ReadRowsByRange(CurrentProfileGeneric, start, end);
                            ReadDataBlock(tmp, "Reading profile generic data", 1, reply);
                        }
                        else if (CurrentProfileGeneric.AccessSelector == AccessRange.Entry)
                        {
                            tmp = client.ReadRowsByEntry(CurrentProfileGeneric, Convert.ToUInt32(CurrentProfileGeneric.From), Convert.ToUInt32(CurrentProfileGeneric.To));
                            ReadDataBlock(tmp, "Reading profile generic data " + CurrentProfileGeneric.Name, 1, reply);
                        }
                        else //Read all.
                        {
                            tmp = client.Read(CurrentProfileGeneric, 2);
                            ReadDataBlock(tmp, "Reading profile generic data " + CurrentProfileGeneric.Name, 1, reply);
                        }
                        OnAfterRead?.Invoke(client, obj, it, reply.Data, parameters, null);
                    }
                    catch (GXDLMSException ex)
                    {
                        obj.SetLastError(it, ex);
                        if (ex.ErrorCode == (int)ErrorCode.ReadWriteDenied ||
                                ex.ErrorCode == (int)ErrorCode.UndefinedObject ||
                                ex.ErrorCode == (int)ErrorCode.UnavailableObject ||
                                //Actaris returns access violation error.
                                ex.ErrorCode == (int)ErrorCode.AccessViolated ||
                                ex.ErrorCode == (int)ErrorCode.OtherReason)
                        {
                            //Some meters return OtherReason if Profile Generic buffer is try to read with selective access.
                            if (!(obj is GXDLMSProfileGeneric && it == 2 && ex.ErrorCode == (int)ErrorCode.OtherReason))
                            {
                                obj.SetAccess(it, AccessMode.NoAccess);
                            }
                            OnAfterRead?.Invoke(null, obj, it, null, null, ex);
                            continue;
                        }
                        else
                        {
                            OnAfterRead?.Invoke(null, obj, it, null, null, ex);
                            throw ex;
                        }
                    }
                    catch (Exception ex)
                    {
                        OnAfterRead?.Invoke(null, obj, it, null, null, ex);
                        obj.SetLastError(it, ex);
                        throw ex;
                    }
                    finally
                    {
                        client.OnPdu -= p;
                        OnDataReceived -= new GXDLMSCommunicator.DataReceivedEventHandler(OnProfileGenericDataReceived);
                    }
                    continue;
                }
                else
                {
                    if (OnBeforeRead != null)
                    {
                        OnBeforeRead(client, obj, it, null, null, null);
                    }
                    byte[][] data = client.Read(obj, it);
                    try
                    {
                        ReadDataBlock(data, "Read object type " + obj.ObjectType + " index: " + it, reply);
                    }
                    catch (GXDLMSException ex)
                    {
                        obj.SetLastError(it, ex);
                        if (ex.ErrorCode == (int)ErrorCode.ReadWriteDenied ||
                                ex.ErrorCode == (int)ErrorCode.UndefinedObject ||
                                ex.ErrorCode == (int)ErrorCode.UnavailableObject ||
                                //Actaris returns access violation error.
                                ex.ErrorCode == (int)ErrorCode.AccessViolated ||
                                ex.ErrorCode == (int)ErrorCode.OtherReason ||
                                ex.ErrorCode == (int)ErrorCode.InconsistentClass)
                        {
                            obj.SetAccess(it, AccessMode.NoAccess);
                            if (OnAfterRead != null)
                            {
                                OnAfterRead(client, obj, it, null, null, ex);
                            }
                            continue;
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                    catch (Exception ex)
                    {
                        obj.SetLastError(it, ex);
                        throw;
                    }
                    if (obj is IGXDLMSBase)
                    {
                        object value = reply.Value;
                        DataType type;
                        if (value is byte[] && (type = obj.GetUIDataType(it)) != DataType.None)
                        {
                            value = GXDLMSClient.ChangeType((byte[])value, type, client.UseUtc2NormalTime);
                        }
                        if (reply.DataType != DataType.None && obj.GetDataType(it) == DataType.None)
                        {
                            obj.SetDataType(it, reply.DataType);
                        }
                        client.UpdateValue(obj, it, value);
                    }
                    if (OnAfterRead != null)
                    {
                        OnAfterRead(client, obj, it, reply.Value, null, null);
                    }
                    obj.SetLastReadTime(it, DateTime.Now);
                }
            }
        }

        public void Write(GXDLMSObject obj, int index)
        {
            object val;
            GXReplyData reply = new GXReplyData();
            for (int it = 1; it != (obj as IGXDLMSBase).GetAttributeCount() + 1; ++it)
            {
                reply.Clear();
                if (it == index || (index == 0 && obj.GetDirty(it, out val)))
                {
                    bool forced = false;
                    GXDLMSAttributeSettings att = obj.Attributes.Find(it);
                    //Read DLMS data type if not known.
                    DataType type = obj.GetDataType(it);
                    if (type == DataType.None)
                    {
                        byte[] data = client.Read(obj, it)[0];
                        ReadDataBlock(data, "Read object type " + obj.ObjectType, reply);
                        type = reply.DataType;
                        if (type != DataType.None)
                        {
                            obj.SetDataType(it, type);
                        }
                        reply.Clear();
                    }
                    try
                    {
                        if (att != null && att.ForceToBlocks)
                        {
                            forced = client.ForceToBlocks = true;
                        }
                        try
                        {
                            ReadDataBlock(client.Write(obj, it), string.Format("Writing object {0}, interface {1}", obj.LogicalName, obj.ObjectType), reply);
                            ValueEventArgs e1 = new ValueEventArgs(obj, it, 0, null);
                            string xml = GXDLMSTranslator.ValueToXml(((IGXDLMSBase)obj).GetValue(client.Settings, e1));
                            OnAfterWrite?.Invoke(client, obj, it, xml, null);
                        }
                        catch (GXDLMSException ex)
                        {
                            OnError?.Invoke(obj, it, null, ex);
                            OnAfterWrite?.Invoke(client, obj, it, null, ex);
                            throw ex;
                        }
                        //Read data once again to make sure it is updated.
                        reply.Clear();
                        byte[] data = client.Read(obj, it)[0];
                        ReadDataBlock(data, string.Format("Reading object {0}, interface {1}", obj.LogicalName, obj.ObjectType), reply);
                        val = reply.Value;
                        if (val is byte[] && (type = obj.GetUIDataType(it)) != DataType.None)
                        {
                            val = GXDLMSClient.ChangeType((byte[])val, type, client.UseUtc2NormalTime);
                        }
                        client.UpdateValue(obj, it, val);
                    }
                    finally
                    {
                        if (forced)
                        {
                            client.ForceToBlocks = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Read list of attributes.
        /// </summary>
        public void ReadList(List<KeyValuePair<GXDLMSObject, int>> list)
        {
            if (OnBeforeRead != null)
            {
                foreach (var it in list)
                {
                    OnBeforeRead(client, it.Key, it.Value, null, null, null);
                }
            }
            byte[][] data = client.ReadList(list);
            GXReplyData reply = new GXReplyData();
            List<object> values = new List<object>();
            foreach (byte[] it in data)
            {
                ReadDataBlock(it, "", 1, 1, reply);
                //Value is null if data is send in multiple frames.
                if (reply.Value is IEnumerable<object>)
                {
                    values.AddRange((IEnumerable<object>)reply.Value);
                }
                reply.Clear();
            }
            if (values.Count != list.Count)
            {
                throw new Exception("Invalid reply. Read items count do not match.");
            }
            client.UpdateValues(list, values);
            if (OnAfterRead != null)
            {
                int pos = 0;
                foreach(var it in list)
                {
                    OnAfterRead(client, it.Key, it.Value, values[pos], null, null);
                    ++pos;
                }
            }
        }

        public object ReadValue(GXDLMSObject it, int attributeOrdinal)
        {
            GXReplyData reply = new GXReplyData();
            string str = string.Format("Reading object {0}, interface {1}", it.LogicalName, it.ObjectType);
            ReadDataBlock(client.Read(it, attributeOrdinal), str, reply);
            //If data type is unknown
            if (it.GetDataType(attributeOrdinal) == DataType.None)
            {
                it.SetDataType(attributeOrdinal, reply.DataType);
            }
            client.UpdateValue(it, attributeOrdinal, reply.Value);
            return reply.Value;
        }

        public void GetProfileGenericColumns(GXDLMSProfileGeneric item)
        {
            GXReplyData reply = new GXReplyData();
            ReadDataBlock(Read(item, 3), "Get profile generic columns...", reply);
            client.UpdateValue(item, 3, reply.Value);
        }

        public void KeepAlive()
        {
            GXReplyData reply = new GXReplyData();
            byte[] data = client.GetKeepAlive();
            ReadDataBlock(data, "Send Keep Alive", reply);
        }
    }
}
