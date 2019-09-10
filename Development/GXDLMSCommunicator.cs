//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Version:         $Revision: 10970 $,
//                  $Date: 2019-09-10 11:12:32 +0300 (ti, 10 syys 2019) $
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

namespace GXDLMSDirector
{
    internal class Password
    {
        public static string Key = "Gurux Ltd.";
    }

    public delegate void ProgressEventHandler(object sender, string description, int current, int maximium);
    public delegate void StatusEventHandler(object sender, DeviceState status);
    public delegate void ReadEventHandler(GXDLMSObject sender, int index, Exception ex);
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

        public GXDLMSCommunicator(GXDLMSDevice parent, Gurux.Common.IGXMedia media)
        {
            this.parent = parent;
            this.media = media;
            client = new GXDLMSSecureClient();
        }

        public ProgressEventHandler OnProgress;
        public ReadEventHandler OnBeforeRead;
        public ReadEventHandler OnAfterRead;
        public ReadEventHandler OnError;

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
                    if (client.InterfaceType == InterfaceType.HDLC)
                    {
                        int t, source;
                        byte type;
                        GXDLMSClient.GetHdlcAddressInfo(new GXByteBuffer(it), out t, out source, out type);
                        client.Limits.SenderFrame = type;
                    }
                    throw ex;
                }
            }
            NotifyProgress(text, 1, 1);
        }

        byte[] ReleaseRequest()
        {
            byte[][] data = client.ReleaseRequest();
            if (data == null)
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
                            (client.InterfaceType == InterfaceType.HDLC && client.Ciphering.Security != Security.None && !parent.PreEstablished))
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
                        if (client.InterfaceType == InterfaceType.HDLC && !parent.PreEstablished)
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
            GXReplyData notify = new GXReplyData();
            reply.Error = 0;
            object eop = (byte)0x7E;
            //In network connection terminator is not used.
            if (client.InterfaceType == InterfaceType.WRAPPER && !(media is GXSerial) && !parent.UseRemoteSerial)
            {
                eop = null;
            }
            int pos = 0;
            bool succeeded = false;
            ReceiveParameters<byte[]> p = new ReceiveParameters<byte[]>()
            {
                AllData = false,
                Eop = eop,
                Count = 5,
                WaitTime = parent.WaitTime * 1000,
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
                int msgPos = 0;
                try
                {
                    pos = 0;
                    //Loop until whole COSEM packet is received.
                    while (!client.GetData(rd, reply, notify))
                    {
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
                                msgPos = rd.Position;
                                notify.Clear();
                                p.Eop = eop;
                            }
                            continue;
                        }
                        //If Eop is not set read one byte at time.
                        if (p.Eop == null)
                        {
                            p.Count = 1;
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
                        rd.Set(p.Reply);
                        rd.Position = msgPos;
                    }
                }
                catch (Exception ex)
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
                    throw ex;
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
            char rate = '5';
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

        void InitializeIEC()
        {
            GXManufacturer manufacturer = this.parent.Manufacturers.FindByIdentification(parent.Manufacturer);
            if (manufacturer == null)
            {
                throw new Exception("Unknown manufacturer " + parent.Manufacturer);
            }
            GXSerial serial = media as GXSerial;
            byte Terminator = (byte)0x0A;
            media.Open();
            if (media is GXSerial)
            {
                //Some meters need a little break.
                Thread.Sleep(1000);
            }
            //Query device information.
            if (serial != null && parent.StartProtocol == StartProtocolType.IEC)
            {
                string data = "/?!\r\n";
                if (this.parent.HDLCAddressing == HDLCAddressType.SerialNumber)
                {
                    data = "/?" + this.parent.PhysicalAddress + "!\r\n";
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
                GXLogWriter.WriteLog("HDLC received: " + p.Reply);
                if (p.Reply[0] != '/')
                {
                    p.WaitTime = 100;
                    media.Receive(p);
                    throw new Exception("Invalid responce.");
                }
                string manufactureID = p.Reply.Substring(1, 3);
                UpdateManufactureSettings(manufactureID);
                char baudrate = p.Reply[4];
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
                    media.Close();
                    serial.BaudRate = BaudRate;
                    serial.DataBits = 8;
                    serial.Parity = Parity.None;
                    serial.StopBits = StopBits.One;
                    media.Open();
                    //Some meters need this sleep. Do not remove.
                    Thread.Sleep(1000);
                }
            }
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
            GXManufacturer manufacturer = this.parent.Manufacturers.FindByIdentification(id);
            if (manufacturer == null)
            {
                throw new Exception("Unknown manufacturer " + id);
            }
            this.parent.Manufacturer = manufacturer.Identification;
            client.Standard = this.parent.Standard;
            client.Authentication = this.parent.Authentication;
            client.InterfaceType = InterfaceType.HDLC;
            if (!string.IsNullOrEmpty(this.parent.Password))
            {
                client.Password = CryptHelper.Decrypt(this.parent.Password, Password.Key);
            }
            else if (this.parent.HexPassword != null)
            {
                client.Password = CryptHelper.Decrypt(this.parent.HexPassword, Password.Key);
            }
            client.UseLogicalNameReferencing = this.parent.UseLogicalNameReferencing;
            client.ProposedConformance = (Conformance)parent.Conformance;
            client.UtcTimeZone = parent.UtcTimeZone;
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

            client.ClientAddress = parent.ClientAddress;
            if (parent.HDLCAddressing == HDLCAddressType.SerialNumber)
            {
                string formula = null;
                GXServerAddress server = manufacturer.GetServer(parent.HDLCAddressing);
                if (server != null)
                {
                    formula = server.Formula;
                }
                client.ServerAddress = GXDLMSClient.GetServerAddress(Convert.ToInt32(parent.PhysicalAddress), formula);
                client.ServerAddressSize = 4;
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
            if (!string.IsNullOrEmpty(this.parent.Password))
            {
                client.Password = CryptHelper.Decrypt(this.parent.Password, Password.Key);
            }
            else if (this.parent.HexPassword != null)
            {
                client.Password = CryptHelper.Decrypt(this.parent.HexPassword, Password.Key);
            }
            client.UseLogicalNameReferencing = this.parent.UseLogicalNameReferencing;
            client.UtcTimeZone = parent.UtcTimeZone;
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
                client.ServerAddress = GXDLMSClient.GetServerAddress(Convert.ToInt32(parent.PhysicalAddress), formula);
                client.ServerAddressSize = 4;
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
            client.Limits.WindowSizeRX = parent.WindowSizeRX;
            client.Limits.WindowSizeTX = parent.WindowSizeTX;
            client.Limits.UseFrameSize = parent.UseFrameSize;
            client.Limits.MaxInfoRX = parent.MaxInfoRX;
            client.Limits.MaxInfoTX = parent.MaxInfoTX;
            client.MaxReceivePDUSize = parent.PduSize;
            client.UserId = parent.UserId;
            client.Priority = parent.Priority;
            client.ServiceClass = parent.ServiceClass;
            if (parent.PreEstablished)
            {
                client.ServerSystemTitle = GXCommon.HexToBytes(parent.ServerSystemTitle);
            }
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
                    media.Open();
                }
            }
            try
            {
                GXReplyData reply = new GXReplyData();
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
                    try
                    {
                        client.ClientAddress = 16;
                        client.Authentication = Authentication.None;
                        client.Ciphering.Security = Security.None;

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
                            GXLogWriter.WriteLog("Parsing UA reply succeeded.");
                            //Has server accepted client.
                            ParseUAResponse(reply.Data);
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
                            client.Ciphering.InvocationCounter = parent.InvocationCounter = Convert.ToUInt32(d.Value);
                            reply.Clear();
                            ReadDataBlock(DisconnectRequest(), "Disconnect request", reply);
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
                    }
                }
                data = SNRMRequest();
                if (data != null)
                {
                    try
                    {
                        reply.Clear();
                        ReadDataBlock(data, "Send SNRM request.", 1, 1, reply);
                    }
                    catch (TimeoutException)
                    {
                        reply.Clear();
                        ReadDataBlock(DisconnectRequest(true), "Send Disconnect request.", 1, 1, reply);
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
                        ReadDLMSPacket(DisconnectRequest(), 1, reply);
                        throw Ex;
                    }
                    //If authentication is required.
                    if (client.Authentication > Authentication.Low)
                    {
                        reply.Clear();
                        ReadDataBlock(client.GetApplicationAssociationRequest(), "Authenticating.", reply);
                        client.ParseApplicationAssociationResponse(reply.Data);
                    }
                }
            }
            catch (Exception ex)
            {
                if (media is GXSerial && parent.StartProtocol == StartProtocolType.IEC)
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
                throw ex;
            }
            parent.KeepAliveStart();
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
                catch (Exception ex)
                {
                    //Update frame ID if meter returns error.
                    if (client.InterfaceType == InterfaceType.HDLC)
                    {
                        int target, source;
                        byte type;
                        GXDLMSClient.GetHdlcAddressInfo(new GXByteBuffer(it), out target, out source, out type);
                        client.Limits.SenderFrame = type;
                    }
                    throw ex;
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
                    OnAfterRead(CurrentProfileGeneric, 2, null);
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
                catch (Exception ex)
                {
                    //Update frame ID if meter returns error.
                    if (client.InterfaceType == InterfaceType.HDLC)
                    {
                        int target, source;
                        byte type;
                        GXDLMSClient.GetHdlcAddressInfo(new GXByteBuffer(it), out target, out source, out type);
                        client.Limits.SenderFrame = type;
                    }
                    throw ex;
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
                    if (client.InterfaceType == InterfaceType.HDLC)
                    {
                        int target, source;
                        byte type;
                        GXDLMSClient.GetHdlcAddressInfo(new GXByteBuffer(it), out target, out source, out type);
                        client.Limits.SenderFrame = type;
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
                    data = client.ReceiverReady(reply.MoreData);
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

        public GXDLMSObjectCollection GetObjects()
        {
            GXLogWriter.WriteLog("--- Collecting objects. ---");
            GXDLMSObjectCollection objs;
            GXReplyData reply = new GXReplyData();
            try
            {
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
                            else if (obj.GetAccess(1) != AccessMode.NoAccess)
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
        /// <param name="InitialValues"></param>
        /// <param name="obj"></param>
        /// <param name="forceRead">Force all attributes read.</param>
        public void Read(object sender, GXDLMSObject obj, bool forceRead)
        {
            GXReplyData reply = new GXReplyData();
            int[] indexes = (obj as IGXDLMSBase).GetAttributeIndexToRead(forceRead);
            foreach (int it in indexes)
            {
                //If reading is not allowed.
                if ((obj.GetAccess(it) & AccessMode.Read) == 0)
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
                        OnBeforeRead(obj, it, null);
                    }
                    try
                    {
                        byte[][] tmp;
                        CurrentProfileGeneric = obj as GXDLMSProfileGeneric;
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
                            tmp = client.ReadRowsByEntry(CurrentProfileGeneric, Convert.ToInt32(CurrentProfileGeneric.From), Convert.ToInt32(CurrentProfileGeneric.To));
                            ReadDataBlock(tmp, "Reading profile generic data " + CurrentProfileGeneric.Name, 1, reply);
                        }
                        else //Read all.
                        {
                            tmp = client.Read(CurrentProfileGeneric, 2);
                            ReadDataBlock(tmp, "Reading profile generic data " + CurrentProfileGeneric.Name, 1, reply);
                        }
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
                            if (OnAfterRead != null)
                            {
                                OnAfterRead(obj, it, ex);
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
                        throw ex;
                    }
                    finally
                    {
                        if (OnAfterRead != null)
                        {
                            OnAfterRead(obj, it, null);
                        }
                        OnDataReceived -= new GXDLMSCommunicator.DataReceivedEventHandler(OnProfileGenericDataReceived);
                    }
                    continue;
                }
                else
                {
                    if (OnBeforeRead != null)
                    {
                        OnBeforeRead(obj, it, null);
                    }
                    byte[] data = client.Read(obj.Name, obj.ObjectType, it)[0];
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
                                OnAfterRead(obj, it, ex);
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
                        throw ex;
                    }
                    if (obj is IGXDLMSBase)
                    {
                        object value = reply.Value;
                        DataType type;
                        if (value is byte[] && (type = obj.GetUIDataType(it)) != DataType.None)
                        {
                            value = GXDLMSClient.ChangeType((byte[])value, type, client.UtcTimeZone);
                        }
                        if (reply.DataType != DataType.None && obj.GetDataType(it) == DataType.None)
                        {
                            obj.SetDataType(it, reply.DataType);
                        }
                        client.UpdateValue(obj, it, value);
                    }
                    if (OnAfterRead != null)
                    {
                        OnAfterRead(obj, it, null);
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
                        }
                        catch (GXDLMSException ex)
                        {
                            if (OnError != null)
                            {
                                OnError(obj, it, ex);
                            }
                            throw ex;
                        }
                        //Read data once again to make sure it is updated.
                        reply.Clear();
                        byte[] data = client.Read(obj, it)[0];
                        ReadDataBlock(data, string.Format("Reading object {0}, interface {1}", obj.LogicalName, obj.ObjectType), reply);
                        val = reply.Value;
                        if (val is byte[] && (type = obj.GetUIDataType(it)) != DataType.None)
                        {
                            val = GXDLMSClient.ChangeType((byte[])val, type, client.UtcTimeZone);
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
            byte[][] data = client.ReadList(list);
            GXReplyData reply = new GXReplyData();
            List<object> values = new List<object>();
            foreach (byte[] it in data)
            {
                ReadDataBlock(it, "", 1, 1, reply);
                //Value is null if data is send in multiple frames.
                if (reply.Value is List<object>)
                {
                    values.AddRange((List<object>)reply.Value);
                }
                reply.Clear();
            }
            if (values.Count != list.Count)
            {
                throw new Exception("Invalid reply. Read items count do not match.");
            }
            client.UpdateValues(list, values);
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
