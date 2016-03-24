//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/GXDLMSCommunicator.cs $
//
// Version:         $Revision: 8315 $,
//                  $Date: 2016-03-24 16:17:17 +0200 (to, 24 maalis 2016) $
//                  $Author: kurumi $
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

namespace GXDLMSDirector
{
    internal class Password
    {
        public static string Key = "Gurux Ltd.";
    }

    public delegate void ProgressEventHandler(object sender, string description, int current, int maximium);
    public delegate void StatusEventHandler(object sender, DeviceState status);
    public delegate void ReadEventHandler(GXDLMSObject sender, int index);

    public class GXDLMSCommunicator
    {
        internal DateTime lastTransaction = DateTime.MinValue;
        internal DateTime connectionStartTime;
        internal GXDLMSDevice parent;
        public Control parentForm;
        public Gurux.Common.IGXMedia media = null;
        internal Gurux.DLMS.GXDLMSClient client;

        public GXDLMSCommunicator(GXDLMSDevice parent, Gurux.Common.IGXMedia media)
        {
            this.parent = parent;
            this.media = media;
            client = new Gurux.DLMS.GXDLMSClient();
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
            ReadDataBlock(client.Method(target, methodIndex, data, DataType.None), "", reply);
        }

        public byte[] DisconnectRequest()
        {
            byte[] data = client.DisconnectRequest();
            GXLogWriter.WriteLog("Disconnect request", data);
            return data;
        }

        public byte[] DisconnectedModeRequest()
        {
            byte[] data = client.DisconnectedModeRequest();
            GXLogWriter.WriteLog("Disconnected Mode request", data);
            return data;
        }

        public bool UseLogicalNameReferencing
        {
            get
            {
                return client.UseLogicalNameReferencing;
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
            if (media != null && parent.StartProtocol == StartProtocolType.IEC)
            {
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
                        data = (char)0x01 + "B0" + (char)0x03 + "\r\n";
                        media.Send(data, null);
                        p.Count = 1;
                        media.Receive(p);
                        data = "Failed to receive reply from the device in given time.";
                        GXLogWriter.WriteLog(data);
                        throw new Exception(data);
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
                                data = (char)0x01 + "B0" + (char)0x03 + "\r\n";
                                media.Send(data, null);
                                p.Count = 1;
                                if (!media.Receive(p))
                                {
                                }
                                serial.DtrEnable = serial.RtsEnable = false;
                                serial.BaudRate = 9600;
                                serial.DtrEnable = serial.RtsEnable = true;
                                data = (char)0x01 + "B0" + (char)0x03 + "\r\n";
                                media.Send(data, null);
                                p.Count = 1;
                                media.Receive(p);
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
                    GXLogWriter.WriteLog("Maximum BaudRate is set to : " + BaudRate.ToString());
                }
                GXLogWriter.WriteLog("BaudRate is : " + BaudRate.ToString());
                //Send ACK
                //Send Protocol control character
                byte controlCharacter = (byte)'2';// "2" HDLC protocol procedure (Mode E)
                //Send Baud rate character
                //Mode control character 
                byte ModeControlCharacter = (byte)'2';//"2" //(HDLC protocol procedure) (Binary mode)
                //Set mode E.
                byte[] arr = new byte[] { 0x06, controlCharacter, (byte)baudrate, ModeControlCharacter, 13, 10 };
                GXLogWriter.WriteLog("Moving to mode E.", arr);
                lock (media.Synchronous)
                {
                    media.Send(arr, null);
                    System.Threading.Thread.Sleep(500);
                    serial.BaudRate = BaudRate;
                    p.Reply = null;
                    p.WaitTime = 100;
                    //Note! All meters do not echo this.
                    media.Receive(p);
                    if (p.Reply != null)
                    {
                        GXLogWriter.WriteLog("Received: " + p.Reply);
                    }
                    serial.Close();
                    serial.DataBits = 8;
                    serial.Parity = Parity.None;
                    serial.StopBits = StopBits.One;
                    serial.Open();
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
            client.UseLogicalNameReferencing = this.parent.UseLogicalNameReferencing;
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
                client.ServerAddress = GXDLMSClient.GetServerAddress(parent.LogicalAddress, Convert.ToInt32(parent.PhysicalAddress));
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
                throw new Exception("Unknown media type.");
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
                foreach (byte[] it in AARQRequest())
                {
                    GXLogWriter.WriteLog("Send AARQ request", it);
                    reply.Clear();
                    ReadDLMSPacket(it, reply);
                }
                GXLogWriter.WriteLog("Parsing AARE reply\r\n" + reply.Data.ToString());
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
                if (client.IsAuthenticationRequired)
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
                        Eop = (byte) 0xA,
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
                    reply.Value = null;
                }
                if (OnAfterRead != null)
                {
                    OnAfterRead(CurrentProfileGeneric, 2);
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
            GXReplyData reply = new GXReplyData(){Peek = true};
            try
            {
                ReadDataBlock(client.GetObjectsRequest(), "Collecting objects", 3, reply);
            }
            catch (Exception Ex)
            {
                throw new Exception("GetObjects failed. " + Ex.Message);
            }
            GXDLMSObjectCollection objs = client.ParseObjects(reply.Data, true);
            //Update OBIS code description.
            GXDLMSConverter c = new GXDLMSConverter();
            c.UpdateOBISCodeInformation(objs);
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
        /// <param name="attribute"></param>
        public void Read(object sender, GXDLMSObject obj, int attribute)
        {
            GXReplyData reply = new GXReplyData();
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
                        CurrentProfileGeneric = obj as GXDLMSProfileGeneric;
                        OnDataReceived += new GXDLMSCommunicator.DataReceivedEventHandler(this.OnProfileGenericDataReceived);
                        if (CurrentProfileGeneric.AccessSelector == AccessRange.Range ||
                            CurrentProfileGeneric.AccessSelector == AccessRange.Last)
                        {
                            byte[][] tmp = client.ReadRowsByRange(CurrentProfileGeneric, Convert.ToDateTime(CurrentProfileGeneric.From), Convert.ToDateTime(CurrentProfileGeneric.To));
                            ReadDataBlock(tmp[0], "Reading profile generic data", 1, reply);
                        }
                        else if (CurrentProfileGeneric.AccessSelector == AccessRange.Entry)
                        {
                            byte[][] tmp = client.ReadRowsByEntry(CurrentProfileGeneric, Convert.ToInt32(CurrentProfileGeneric.From), Convert.ToInt32(CurrentProfileGeneric.To));
                            ReadDataBlock(tmp[0], "Reading profile generic data " + CurrentProfileGeneric.Name, 1, reply);
                        }
                        else //Read all.
                        {
                            byte[] tmp = client.Read(CurrentProfileGeneric, 2)[0];
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
                        if (ex.ErrorCode == 3 ||  //If read is denied.
                            ex.ErrorCode == 4 || // Undefined object.
                            ex.ErrorCode == 13) //Actaris returns access violation error.
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
       
        public void Write(GXDLMSObject obj, object target, int index, List<object> UpdatedObjects)
        {
            object value;
            GXReplyData reply = new GXReplyData();
            for (int it = 1; it != (obj as IGXDLMSBase).GetAttributeCount() + 1; ++it)
            {
                reply.Clear();
                if (obj.GetDirty(it, out value))
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
                    }
                    try
                    {
                        foreach (byte[] tmp in client.Write(obj.Name, value, type, obj.ObjectType, it))
                        {
                            ReadDataBlock(tmp, "Write object", reply);
                        }
                        obj.ClearDirty(it);
                        //Read data once again to make sure it is updated.
                        byte[] data = client.Read(obj, it)[0];
                        ReadDataBlock(data, "Read object " + obj.ObjectType, reply);

                        value = reply.Value;
                        if (value is byte[] && (type = obj.GetUIDataType(it)) != DataType.None)
                        {
                            value = GXDLMSClient.ChangeType((byte[])value, type);
                        }
                        client.UpdateValue(obj, it, value);
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
