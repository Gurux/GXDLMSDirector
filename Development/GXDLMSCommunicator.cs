//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Version:         $Revision: 9587 $,
//                  $Date: 2017-10-11 14:53:32 +0300 (ke, 11 loka 2017) $
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


using System;
using System.Collections.Generic;
using System.Text;
using Gurux.DLMS;
using Gurux.Serial;
using Gurux.Net;
using System.Data;
using GXDLMS.ManufacturerSettings;
using System.IO;
using System.ComponentModel;
using System.IO.Ports;
using Gurux.Common;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Objects;
using System.Xml.Serialization;
using System.Reflection;
using System.Linq;
using System.Windows.Forms;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Objects.Enums;
using System.Threading;
using Gurux.DLMS.Secure;

namespace GXDLMSDirector
{
    internal class Password
    {
        public static string Key = "Gurux Ltd.";
    }

    public delegate void ProgressEventHandler(object sender, string description, int current, int maximium);
    public delegate void StatusEventHandler(object sender, DeviceState status);
    public delegate void ReadEventHandler(GXDLMSObject sender, int index);
    public delegate void MessageTraceEventHandler(GXDLMSDevice sender, string trace);

    public class GXDLMSCommunicator
    {
        internal DateTime lastTransaction = DateTime.MinValue;
        internal DateTime connectionStartTime;
        internal GXDLMSDevice parent;
        public Control parentForm;
        public Gurux.Common.IGXMedia media = null;
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

        public byte[] SNRMRequest()
        {
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
            lastTransaction = DateTime.Now;
            byte[] tmp = client.Read(it, attributeOrdinal)[0];
            GXLogWriter.WriteLog(string.Format("Reading object {0} from interface {1}", it.LogicalName, it.ObjectType), tmp);
            return tmp;
        }

        public void MethodRequest(GXDLMSObject target, int methodIndex, object data, GXReplyData reply)
        {
            ReadDataBlock(client.Method(target, methodIndex, data, GXDLMSConverter.GetDLMSDataType(data)), "", reply);
        }

        byte[] ReleaseRequest()
        {
            byte[] data = client.ReleaseRequest()[0];
            GXLogWriter.WriteLog("Release request", data);
            return data;
        }

        byte[] DisconnectRequest()
        {
            byte[] data = client.DisconnectRequest();
            GXLogWriter.WriteLog("Disconnect request", data);
            return data;
        }

        public void Disconnect()
        {
            if (media != null && media.IsOpen)
            {
                try
                {
                    GXReplyData reply = new GXReplyData();
                    try
                    {
                   //     ReadDLMSPacket(ReleaseRequest(), 1, reply);
                    }
                    catch (Exception)
                    {
                        //All meters don't support release.
                    }
                    reply.Clear();
                    ReadDLMSPacket(DisconnectRequest(), 1, reply);
                }
                finally
                {
                    if (media != null)
                    {
                        media.Close();
                        //Restore old settings.
                        if (media is GXSerial)
                        {
                            media.Settings = parent.StartMediaSettings;
                        }
                    }
                }
            }
        }

        public void ReadDLMSPacket(byte[] data, GXReplyData reply)
        {
            ReadDLMSPacket(data, 3, reply);
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
            if (data == null)
            {
                return;
            }
            object eop = (byte)0x7E;
            //In network connection terminator is not used.
            if (client.InterfaceType == InterfaceType.WRAPPER && media is GXNet && !parent.UseRemoteSerial)
            {
                eop = null;
            }
            if (parent.OnTrace != null)
            {
                parent.OnTrace(parent, "<- " + DateTime.Now.ToLongTimeString() + " " + GXCommon.ToHex(data, true));
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
            lock (media.Synchronous)
            {
                if (data != null)
                {
                    media.Send(data, null);
                }
                while (!succeeded && pos != 3)
                {
                    succeeded = media.Receive(p);
                    if (!succeeded)
                    {
                        //Try to read again...
                        if (++pos != tryCount)
                        {
                            //If Eop is not set read one byte at time.
                            if (p.Eop == null)
                            {
                                p.Count = 1;
                            }
                            System.Diagnostics.Debug.WriteLine("Data send failed. Try to resend " + pos.ToString() + "/3");
                            media.Send(data, null);
                            continue;
                        }
                        string err = "Failed to receive reply from the device in given time.";
                        GXLogWriter.WriteLog(err, p.Reply);
                        throw new Exception(err);
                    }
                }
                try
                {
                    //Loop until whole COSEM packet is received.
                    while (!client.GetData(p.Reply, reply))
                    {
                        //If Eop is not set read one byte at time.
                        if (p.Eop == null)
                        {
                            p.Count = 1;
                        }
                        if (!media.Receive(p))
                        {
                            //Try to read again...
                            if (++pos != tryCount)
                            {
                                System.Diagnostics.Debug.WriteLine("Data send failed. Try to resend " + pos.ToString() + "/3");
                                continue;
                            }
                            string err = "Failed to receive reply from the device in given time.";
                            GXLogWriter.WriteLog(err, p.Reply);
                            throw new Exception(err);
                        }
                    }
                }
                catch (Exception ex)
                {
                    GXLogWriter.WriteLog("Received data", p.Reply);
                    throw ex;
                }
            }
            if (parent.OnTrace != null)
            {
                parent.OnTrace(parent, "-> " + DateTime.Now.ToLongTimeString() + " " + GXCommon.ToHex(p.Reply, true));
            }
            GXLogWriter.WriteLog("Received data", p.Reply);
            if (reply.Error != 0)
            {
                if (reply.Error == (int)ErrorCode.Rejected)
                {
                    Thread.Sleep(1000);
                    ReadDLMSPacket(data, tryCount, reply);
                }
                else
                {
                    throw new GXDLMSException(reply.Error);
                }
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
            //Query device information.
            if (serial != null && parent.StartProtocol == StartProtocolType.IEC)
            {
                parent.StartMediaSettings = media.Settings;
                string data = "/?!\r\n";
                if (this.parent.HDLCAddressing == HDLCAddressType.SerialNumber)
                {
                    data = "/?" + this.parent.PhysicalAddress + "!\r\n";
                }
                GXLogWriter.WriteLog("HDLC sending:" + data);
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
            if (!parent.UseRemoteSerial && this.media is GXNet && manufacturer.UseIEC47)
            {
                client.InterfaceType = InterfaceType.WRAPPER;
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
                    client.ServerAddress = GXDLMSClient.GetServerAddress(parent.LogicalAddress, Convert.ToInt32(parent.PhysicalAddress));
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
                    //Restore old settings.
                    media.Settings = parent.StartMediaSettings;
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

        public void InitializeConnection()
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
            try
            {
                GXReplyData reply = new GXReplyData();
                byte[] data;
                data = SNRMRequest();
                if (data != null)
                {
                    GXLogWriter.WriteLog("Send SNRM request.", data);
                    ReadDLMSPacket(data, 1, reply);
                    GXLogWriter.WriteLog("Parsing UA reply.\r\n" + reply.Data.ToString());
                    //Has server accepted client.
                    ParseUAResponse(reply.Data);
                    GXLogWriter.WriteLog("Parsing UA reply succeeded.");
                }
                //Generate AARQ request.
                //Split requests to multiple packets if needed.
                //If password is used all data might not fit to one packet.
                reply.Clear();
                ReadDataBlock(AARQRequest(), "", reply);
                try
                {
                    //Parse reply.
                    ParseAAREResponse(reply.Data);
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
                    foreach (byte[] it in client.GetApplicationAssociationRequest())
                    {
                        GXLogWriter.WriteLog("Authenticating", it);
                        reply.Clear();
                        ReadDLMSPacket(it, reply);
                    }
                    client.ParseApplicationAssociationResponse(reply.Data);
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
            GXLogWriter.WriteLog("Parsing AARE reply succeeded.");
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
                reply.Clear();
                ReadDataBlock(it, text, reply);
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
                    OnAfterRead(CurrentProfileGeneric, 2);
                }
            }
        }

        internal void ReadDataBlock(byte[][] data, string text, int multiplier, GXReplyData reply)
        {
            foreach (byte[] it in data)
            {
                reply.Clear();
                ReadDataBlock(it, text, multiplier, reply);
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
            if (parent.InactivityMode == InactivityMode.ReopenActive && media is GXSerial && DateTime.Now.Subtract(connectionStartTime).TotalSeconds > 40)
            {
                parent.Disconnect();
                parent.InitializeConnection();
            }
            GXLogWriter.WriteLog(text, data);
            ReadDLMSPacket(data, reply);
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
                        GXLogWriter.WriteLog("Get next frame: ", data);
                    }
                    else
                    {
                        GXLogWriter.WriteLog("Get Next Data block: ", data);
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

        public GXDLMSObjectCollection GetObjects()
        {
            GXLogWriter.WriteLog("--- Collecting objects. ---");
            GXReplyData reply = new GXReplyData()
            {
            };
            try
            {
                ReadDataBlock(client.GetObjectsRequest(), "Collecting objects", 3, reply);
            }
            catch (Exception Ex)
            {
                throw new Exception("GetObjects failed. " + Ex.Message);
            }
            GXDLMSObjectCollection objs = client.ParseObjects(reply.Data, true);
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
        /// <param name="attribute">Attribute index to read.</param>
        /// <param name="forceRead">Force all attributes read.</param>
        public void Read(object sender, GXDLMSObject obj, int attribute, bool forceRead)
        {
            GXReplyData reply = new GXReplyData();
            if (forceRead)
            {
                obj.ClearReadTime();
            }
            foreach (int it in (obj as IGXDLMSBase).GetAttributeIndexToRead())
            {
                reply.Clear();
                if (obj is GXDLMSProfileGeneric && it == 2)
                {
                    if (OnBeforeRead != null)
                    {
                        OnBeforeRead(obj, it);
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
                    finally
                    {
                        if (OnAfterRead != null)
                        {
                            OnAfterRead(obj, it);
                        }
                        OnDataReceived -= new GXDLMSCommunicator.DataReceivedEventHandler(OnProfileGenericDataReceived);
                    }
                    continue;
                }
                else
                {
                    if (OnBeforeRead != null)
                    {
                        OnBeforeRead(obj, it);
                    }
                    byte[] data = client.Read(obj.Name, obj.ObjectType, it)[0];
                    try
                    {
                        ReadDataBlock(data, "Read object type " + obj.ObjectType + " index: " + it, reply);
                    }
                    catch (GXDLMSException ex)
                    {
                        if (ex.ErrorCode == (int)ErrorCode.ReadWriteDenied ||
                                ex.ErrorCode == (int)ErrorCode.UndefinedObject ||
                                ex.ErrorCode == (int)ErrorCode.UnavailableObject ||
                                //Actaris returns access violation error.
                                ex.ErrorCode == (int)ErrorCode.AccessViolated)
                        {
                            obj.SetAccess(it, AccessMode.NoAccess);
                            continue;
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                    if (obj is IGXDLMSBase)
                    {
                        object value = reply.Value;
                        DataType type;
                        if (value is byte[] && (type = obj.GetUIDataType(it)) != DataType.None)
                        {
                            value = GXDLMSClient.ChangeType((byte[])value, type);
                        }
                        client.UpdateValue(obj, it, value);
                    }
                    if (OnAfterRead != null)
                    {
                        OnAfterRead(obj, it);
                    }
                    obj.SetLastReadTime(it, DateTime.Now);
                    //If only selected attribute is read.
                    if (attribute != 0)
                    {
                        break;
                    }
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
                    //Read DLMS data type if not known.
                    DataType type = obj.GetDataType(it);
                    if (type == DataType.None)
                    {
                        byte[] data = client.Read(obj, it)[0];
                        ReadDataBlock(data, "Write object type " + obj.ObjectType, reply);
                        type = reply.DataType;
                        if (type == DataType.None)
                        {
                            throw new Exception("Failed to write value. Data type not set.");
                        }
                        obj.SetDataType(it, type);
                        reply.Clear();
                    }
                    try
                    {
                        foreach (byte[] tmp in client.Write(obj, it))
                        {
                            ReadDataBlock(tmp, "Write object", reply);
                        }
                        obj.ClearDirty(it);
                        //Read data once again to make sure it is updated.
                        reply.Clear();
                        byte[] data = client.Read(obj, it)[0];
                        ReadDataBlock(data, "Read object " + obj.ObjectType, reply);
                        val = reply.Value;
                        if (val is byte[] && (type = obj.GetUIDataType(it)) != DataType.None)
                        {
                            val = GXDLMSClient.ChangeType((byte[])val, type);
                        }
                        client.UpdateValue(obj, it, val);
                    }
                    catch (GXDLMSException ex)
                    {
                        if (ex.ErrorCode == 3)
                        {
                            throw new Exception("Read/Write Failed.");
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                }
            }
        }

        public void ReadValue(GXDLMSObject it, int attributeOrdinal)
        {
            GXReplyData reply = new GXReplyData();
            ReadDataBlock(Read(it, attributeOrdinal), "Read object", reply);
            //If data type is unknown
            if (it.GetDataType(attributeOrdinal) == DataType.None)
            {
                it.SetDataType(attributeOrdinal, reply.DataType);
            }
            client.UpdateValue(it, attributeOrdinal, reply.Value);
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
            GXLogWriter.WriteLog("Send Keep Alive", data);
            ReadDLMSPacket(data, reply);
        }
    }
}
