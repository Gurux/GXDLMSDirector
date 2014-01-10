//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GuruxClub/GXDLMSDirector/Development/GXDLMSCommunicator.cs $
//
// Version:         $Revision: 6830 $,
//                  $Date: 2013-12-23 13:42:07 +0200 (ma, 23 joulu 2013) $
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
        internal DateTime LastTransaction = DateTime.MinValue;
        internal DateTime ConnectionStartTime;
        internal GXDLMSDevice Parent;
        public Control ParentForm;
        public Gurux.Common.IGXMedia Media = null;
        internal Gurux.DLMS.GXDLMSClient m_Cosem;

        public GXDLMSCommunicator(GXDLMSDevice parent, Gurux.Common.IGXMedia media)
        {
            Parent = parent;            
            Media = media;
            m_Cosem = new Gurux.DLMS.GXDLMSClient();
        }        

        public ProgressEventHandler OnProgress;
        public ReadEventHandler OnBeforeRead;
        public ReadEventHandler OnAfterRead;

        public byte[] SNRMRequest()
        {
            return m_Cosem.SNRMRequest();
        }

        public void ParseUAResponse(byte[] data)
        {
            m_Cosem.ParseUAResponse(data);
        }

        public byte[][] AARQRequest()
        {
            return m_Cosem.AARQRequest(null);
        }

        public void ParseAAREResponse(byte[] data)
        {
            m_Cosem.ParseAAREResponse(data);
        }                

        public byte[] Read(object data, ObjectType type, int AttributeOrdinal)
        {
            LastTransaction = DateTime.Now;
            if (data is GXDLMSObject)
            {
                GXDLMSObject obj = data as GXDLMSObject;
                data = obj.Name;
            }
            byte[] tmp = m_Cosem.Read(data, type, AttributeOrdinal)[0];
            GXLogWriter.WriteLog(string.Format("Reading object {0} from interface {1}", data.ToString(), type.ToString()), tmp);
            return tmp;
        }

        public byte[] MethodRequest(object name, ObjectType type, int methodIndex)
        {
            return ReadDataBlock(m_Cosem.Method(name, type, methodIndex, null, DataType.None)[0], "");
        }

        public byte[] DisconnectRequest()
        {
            byte[] data = m_Cosem.DisconnectRequest();
            GXLogWriter.WriteLog("Disconnect request", data);
            return data;
        }

        public byte[] DisconnectedModeRequest()
        {
            byte[] data = m_Cosem.DisconnectedModeRequest();
            GXLogWriter.WriteLog("Disconnected Mode request", data);
            return data;
        }

        public bool UseLogicalNameReferencing
        {
            get
            {
                return m_Cosem.UseLogicalNameReferencing;
            }
        }

        /// <summary>
        /// Read DLMS Data from the device.
        /// </summary>
        /// <remarks>
        /// If access is denied return null.
        /// </remarks>
        /// <param name="data">Data to send.</param>
        /// <returns>Received data.</returns>
        public byte[] ReadDLMSPacket(byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            object eop = (byte)0x7E;
            //In network connection terminator is not used.
            if (m_Cosem.InterfaceType == InterfaceType.Net && Media is GXNet && !Parent.UseRemoteSerial)
            {
                eop = null;                
            }
            int pos = 0;
            bool succeeded = false;
            ReceiveParameters<byte[]> p = new ReceiveParameters<byte[]>()
            {
                AllData = true,
                Eop = eop,
                Count = 5,
                WaitTime = Parent.WaitTime * 1000,
            };
            lock (Media.Synchronous)
            {
                if (data != null)
                {
                    Media.Send(data, null);
                }
                while (!succeeded && pos != 3)
                {                    
                    succeeded = Media.Receive(p);
                    if (!succeeded)
                    {
                        //Try to read again...
                        if (++pos != 3)
                        {
                            //If Eop is not set read one byte at time.
                            if (p.Eop == null)
                            {
                                p.Count = 1;
                            }
                            System.Diagnostics.Debug.WriteLine("Data send failed. Try to resend " + pos.ToString() + "/3");
                            Media.Send(data, null);
                            continue;
                        }
                        string err = "Failed to receive reply from the device in given time.";
                        GXLogWriter.WriteLog(err, p.Reply);
                        throw new Exception(err);
                    }
                }
                //Loop until whole Cosem packet is received.                
                while (!m_Cosem.IsDLMSPacketComplete(p.Reply))
                {
                    //If Eop is not set read one byte at time.
                    if (p.Eop == null)
                    {
                        p.Count = 1;
                    }
                    if (!Media.Receive(p))
                    {
                        //Try to read again...
                        if (++pos != 3)
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
            GXLogWriter.WriteLog("Reveived data", p.Reply);
            object errors = m_Cosem.CheckReplyErrors(data, p.Reply);
            if (errors != null)
            {                
                object[,] arr = (object[,])errors;
                int error = (int)arr[0, 0];                
                if (error == -1)
                {
                    //If data is reply to the previous packet sent.
                    //This might happend sometimes.
                    if (m_Cosem.IsPreviousPacket(data, p.Reply))
                    {
                        return ReadDLMSPacket(null);
                    }
                    else
                    {
                        throw new Exception(arr[0, 1].ToString());
                    }
                }
                throw new GXDLMSException(error);
            }
            return p.Reply;
        }

        void InitializeIEC()
        {
            GXManufacturer manufacturer = this.Parent.Manufacturers.FindByIdentification(Parent.Manufacturer);
            if (manufacturer == null)
            {
                throw new Exception("Unknown manufacturer " + Parent.Manufacturer);
            }
            GXSerial serial = Media as GXSerial;            
            byte Terminator = (byte)0x0A;
            if (serial != null && Parent.StartProtocol == StartProtocolType.IEC)
            {
                serial.BaudRate = 300;
                serial.DataBits = 7;
                serial.Parity = Parity.Even;
                serial.StopBits = StopBits.One;
            }
            Media.Open();
            //Query device information.
            if (Media != null && Parent.StartProtocol == StartProtocolType.IEC)
            {
                string data = "/?!\r\n";
                if (this.Parent.HDLCAddressing == HDLCAddressType.SerialNumber)
                {
                    data = "/?" + this.Parent.PhysicalAddress + "!\r\n";
                }
                GXLogWriter.WriteLog("HDLC sending:" + data);
                ReceiveParameters<string> p = new ReceiveParameters<string>()
                {
                    Eop = Terminator,                    
                    WaitTime = Parent.WaitTime * 1000                    
                };
                lock (Media.Synchronous)
                {
                    Media.Send(data, null);
                    if (!Media.Receive(p))
                    {
                        //Try to move away from mode E.
                        try
                        {

                            this.ReadDLMSPacket(this.DisconnectRequest());
                        }
                        catch (Exception ex)
                        { 
                        }
                        data = (char)0x01 + "B0" + (char)0x03 + "\r\n";
                        Media.Send(data, null);
                        p.Count = 1;
                        if (!Media.Receive(p))
                        {
                        }
                        data = "Failed to receive reply from the device in given time.";
                        GXLogWriter.WriteLog(data);
                        throw new Exception(data);
                    }
                    //If echo is used.
                    if (p.Reply == data)
                    {
                        p.Reply = null;
                        if (!Media.Receive(p))
                        {
                            //Try to move away from mode E.
                            this.ReadDLMSPacket(this.DisconnectRequest());
                            if (serial != null)
                            {
                                data = (char)0x01 + "B0" + (char)0x03 + "\r\n";
                                Media.Send(data, null);
                                p.Count = 1;
                                if (!Media.Receive(p))
                                {
                                }
                                serial.DtrEnable = serial.RtsEnable = false;
                                serial.BaudRate = 9600;
                                serial.DtrEnable = serial.RtsEnable = true;
                                data = (char)0x01 + "B0" + (char)0x03 + "\r\n";
                                Media.Send(data, null);
                                p.Count = 1;
                                if (!Media.Receive(p))
                                {
                                }
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
                    Media.Receive(p);
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
                GXLogWriter.WriteLog("BaudRate is : " + BaudRate.ToString());
                //Send ACK
                //Send Protocol control character
                byte controlCharacter = (byte)'2';// "2" HDLC protocol procedure (Mode E)
                //Send Baudrate character
                //Mode control character 
                byte ModeControlCharacter = (byte)'2';//"2" //(HDLC protocol procedure) (Binary mode)
                //Set mode E.
                byte[] arr = new byte[] { 0x06, controlCharacter, (byte)baudrate, ModeControlCharacter, 13, 10 };
                GXLogWriter.WriteLog("Moving to mode E.", arr);
                lock (Media.Synchronous)
                {
                    Media.Send(arr, null);
                    System.Threading.Thread.Sleep(1000);
                    if (serial != null)
                    {
                        serial.DtrEnable = serial.RtsEnable = false;
                        serial.BaudRate = BaudRate;
                        serial.DtrEnable = serial.RtsEnable = true;
                    }
                    p.Reply = null;
                    p.WaitTime = 500;
                    if (!Media.Receive(p))
                    {
                        //Try to move away from mode E.
                        this.ReadDLMSPacket(this.DisconnectRequest());
                        data = "Failed to receive reply from the device in given time.";
                        GXLogWriter.WriteLog(data);
                        throw new Exception(data);
                    }
                    GXLogWriter.WriteLog("Received: " + p.Reply);
                    if (serial != null)
                    {
                        System.Threading.Thread.Sleep(50);
                        serial.DtrEnable = serial.RtsEnable = false;
                        serial.DataBits = 8;
                        serial.Parity = Parity.None;
                        serial.StopBits = StopBits.One;
                        serial.DiscardOutBuffer();
                        serial.DtrEnable = serial.RtsEnable = true;
                        System.Threading.Thread.Sleep(200);
                        serial.ResetSynchronousBuffer();
                    }
                }
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
                if (Parent.UseRemoteSerial)
                {
                    InitializeIEC();
                }
                else
                {
                    Media.Open();
                }
            }
            catch (Exception Ex)
            {
                if (Media != null)
                {
                    Media.Close();
                }
                throw Ex;
            }
        }

        public object ClientID
        {
            get
            {
                return m_Cosem.ClientID;
            }
        }

        public object ServerID
        {
            get
            {
                return m_Cosem.ServerID;
            }
        }      

        public void UpdateManufactureSettings(string id)
        {
            if (!string.IsNullOrEmpty(this.Parent.Manufacturer) && string.Compare(this.Parent.Manufacturer, id, true) != 0)
            {
                throw new Exception(string.Format("Manufacturer type does not match. Manufacturer is {0} and it should be {1}.", id, this.Parent.Manufacturer));
            }
            GXManufacturer manufacturer = this.Parent.Manufacturers.FindByIdentification(id);
            if (manufacturer == null)
            {
                throw new Exception("Unknown manufacturer " + id);
            }
            this.Parent.Manufacturer = manufacturer.Identification;
            m_Cosem.Authentication = this.Parent.Authentication;
            m_Cosem.InterfaceType = InterfaceType.General;
            if (!string.IsNullOrEmpty(this.Parent.Password))
            {
                m_Cosem.Password = CryptHelper.Decrypt(this.Parent.Password, Password.Key);
            }            
            m_Cosem.UseLogicalNameReferencing = this.Parent.UseLogicalNameReferencing;
            //If network media is used check is manufacturer supporting IEC 62056-47
            if (!Parent.UseRemoteSerial && this.Media is GXNet && manufacturer.UseIEC47)
            {                
                m_Cosem.InterfaceType = InterfaceType.Net;
                m_Cosem.ClientID = Convert.ToUInt16(Parent.ClientID);
                m_Cosem.ServerID = Convert.ToUInt16((Parent.LogicalAddress << 9) | Convert.ToUInt16(Parent.PhysicalAddress));
            }
            else
            {
                if (Parent.HDLCAddressing == HDLCAddressType.Custom)
                {
                    m_Cosem.ClientID = Parent.ClientID;
                }
                else
                {
                    m_Cosem.ClientID = (byte) (Convert.ToByte(Parent.ClientID) << 1 | 0x1);                    
                }
                string formula = null;
                GXServerAddress server = manufacturer.GetServer(Parent.HDLCAddressing);
                if (server != null)
                {
                    formula = server.Formula;
                }
                m_Cosem.ServerID = GXManufacturer.CountServerAddress(Parent.HDLCAddressing, formula, Parent.PhysicalAddress, Parent.LogicalAddress);
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
                if (Media != null)
                {
                    Media.Close();
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
                if (Media != null)
                {
                    Media.Close();
                }
                throw Ex;
            }
        }

        public void InitializeConnection()
        {
            if (!string.IsNullOrEmpty(Parent.Manufacturer))
            {
                UpdateManufactureSettings(Parent.Manufacturer);
            }
            if (Media is GXSerial)
            {
                GXLogWriter.WriteLog("Initializing serial connection.");
                InitSerial();
                ConnectionStartTime = DateTime.Now;
            }
            else if (Media is GXNet)
            {
                GXLogWriter.WriteLog("Initializing Network connection.");
                InitNet();
                //Some Electricity meters need some time before first message can be send.
                System.Threading.Thread.Sleep(500);
            }
            else if (Media is Gurux.Terminal.GXTerminal)
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
                byte[] data, reply = null;
                data = SNRMRequest();
                System.Threading.Thread.Sleep(200);
                if (data != null)
                {
                    GXLogWriter.WriteLog("Send SNRM request.", data);
                    reply = ReadDLMSPacket(data);
                    GXLogWriter.WriteLog("Parsing UA reply.", reply);
                    //Has server accepted client.
                    ParseUAResponse(reply);
                    GXLogWriter.WriteLog("Parsing UA reply succeeded.");
                }
                //Generate AARQ request.
                //Split requests to multible packets if needed. 
                //If password is used all data might not fit to one packet.
                foreach (byte[] it in AARQRequest())
                {
                    GXLogWriter.WriteLog("Send AARQ request", it);
                    reply = ReadDLMSPacket(it);
                }
                GXLogWriter.WriteLog("Parsing AARE reply", (byte[])reply);
                try
                {
                    //Parse reply.
                    ParseAAREResponse(reply);
                }
                catch (Exception Ex)
                {
                    ReadDLMSPacket(DisconnectRequest());
                    throw Ex;
                }
            }
            catch (Exception ex)
            {
                if (Media is GXSerial && Parent.StartProtocol == StartProtocolType.IEC)
                {
                    ReceiveParameters<string> p = new ReceiveParameters<string>()
                    {
                        Eop = (byte) 0xA,
                        WaitTime = Parent.WaitTime * 1000
                    };
                    lock (Media.Synchronous)
                    {
                        string data = (char)0x01 + "B0" + (char)0x03 + "\r\n";
                        Media.Send(data, null);
                        Media.Receive(p);
                    }
                }
                throw ex;
            }
            GXLogWriter.WriteLog("Parsing AARE reply succeeded.");
            Parent.KeepAliveStart();
        }

        void NotifyProgress(string description, int current, int maximium)
        {
            if (OnProgress != null)
            {
                OnProgress(this, description, current, maximium);
            }
        }

        byte[] ReadDataBlock(byte[] data, string text)
        {
            return ReadDataBlock(data, text, 1);
        }

        public delegate void DataReceivedEventHandler(object sender, byte[] data);
        public event DataReceivedEventHandler OnDataReceived;
        GXDLMSProfileGeneric CurrentProfileGeneric;

        void OnProfileGenericDataReceived(object sender, byte[] data)
        {
            object value = m_Cosem.TryGetValue(data);            
            if (value != null)
            {
                (CurrentProfileGeneric as IGXDLMSBase).SetValue(2, value, false);                
                if (OnAfterRead != null)
                {
                    OnAfterRead(CurrentProfileGeneric, 2);
                }  
            }
        }

        delegate void ShowRowsEventHandler(Array rows);
        
        private void ShowRows(Array rows)
        {
            /*
            if (ParentForm.InvokeRequired)
            {
                ParentForm.BeginInvoke(new ShowRowsEventHandler(ShowRows), rows);
                return;
            }
            DataType type = DataType.None;
            DataType uiType = DataType.None;
            foreach (object[] row in rows)
            {
                if (row != null)
                {
                    DataRow dr = CurrentProfileGeneric.Buffer.NewRow();
                    object data = null;
                    for (int pos = 0; pos < CurrentProfileGeneric.CaptureObjects.Count; ++pos)
                    {
                        if (Parent.Extension != null)
                        {
                            data = row[pos];
                        }
                        else
                        {
                            int index = 0;
                            IGXDLMSColumnObject obj = CurrentProfileGeneric.CaptureObjects[pos] as IGXDLMSColumnObject;
                            foreach (GXDLMSObject c in CurrentProfileGeneric.CaptureObjects)
                            {
                                IGXDLMSColumnObject obj2 = c as IGXDLMSColumnObject;
                                if (c.ObjectType == CurrentProfileGeneric.CaptureObjects[pos].ObjectType &&
                                    c.LogicalName == CurrentProfileGeneric.CaptureObjects[pos].LogicalName)
                                {
                                    if (obj2.SelectedAttributeIndex == obj.SelectedAttributeIndex)
                                    {
                                        data = row[index];
                                        break;
                                    }
                                }
                                ++index;
                            }
                        }
                        if (data != null)
                        {
                            double scaler = 1;
                            object obj = CurrentProfileGeneric.CaptureObjects[pos];
                            if (obj is GXDLMSRegister)
                            {
                                scaler = ((GXDLMSRegister)obj).Scaler;
                            }
                            if (obj is GXDLMSDemandRegister)
                            {
                                scaler = ((GXDLMSDemandRegister)obj).Scaler;
                            }
                            IGXDLMSColumnObject tmp = CurrentProfileGeneric.CaptureObjects[pos] as IGXDLMSColumnObject;
                            uiType = CurrentProfileGeneric.CaptureObjects[pos].GetUIDataType(tmp.SelectedAttributeIndex);
                            type = CurrentProfileGeneric.CaptureObjects[pos].GetDataType(tmp.SelectedAttributeIndex);
                            if (uiType == DataType.None)
                            {
                                uiType = type;
                            }
                            try
                            {
                                if (!data.GetType().IsArray && scaler != 1)
                                {
                                    if (type == DataType.None)
                                    {
                                        dr[pos] = Convert.ToDouble(data) * scaler;
                                    }
                                    else
                                    {
                                        dr[pos] = Convert.ToDouble(GXDLMS.Common.GXHelpers.ConvertFromDLMS(data, DataType.None, type, true)) * scaler;
                                    }
                                }
                                else
                                {
                                    object tmp2 = GXDLMS.Common.GXHelpers.ConvertFromDLMS(data, type, uiType, true);
                                    if (tmp2 is GXDateTime)
                                    {
                                        tmp2 = (tmp2 as GXDateTime).Value;
                                    }
                                    dr[pos] = tmp2;
                                }
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                    CurrentProfileGeneric.Buffer.Rows.InsertAt(dr, CurrentProfileGeneric.Buffer.Rows.Count);
                }
            } 
             * */
        }
        
        /// <summary>
        /// Read data block from the device.
        /// </summary>
        /// <param name="data">data to send</param>
        /// <param name="text">Progress text.</param>
        /// <param name="multiplier"></param>
        /// <returns>Received data.</returns>
        internal byte[] ReadDataBlock(byte[] data, string text, double multiplier)
        {
            if (Parent.InactivityMode == InactivityMode.ReopenActive && Media is GXSerial && DateTime.Now.Subtract(ConnectionStartTime).TotalSeconds > 40)
            {
                Parent.Disconnect();
                Parent.InitializeConnection();
            }            
            GXLogWriter.WriteLog(text, data);
            byte[] reply = ReadDLMSPacket(data);
            byte[] allData = null;
            RequestTypes moredata = m_Cosem.GetDataFromPacket(reply, ref allData);            
            if (OnDataReceived != null)
            {
                OnDataReceived(this, (byte[])allData);
            }              
            if ((moredata & (RequestTypes.Frame | RequestTypes.DataBlock)) != 0)
            {
                int maxProgress = m_Cosem.GetMaxProgressStatus(allData);
                NotifyProgress(text, 1, maxProgress);
                while (moredata != 0)
                {
                    while ((moredata & RequestTypes.Frame) != 0)
                    {
                        data = m_Cosem.ReceiverReady(RequestTypes.Frame);
                        GXLogWriter.WriteLog("Get next frame: ", (byte[])data);
                        reply = ReadDLMSPacket(data);
                        RequestTypes tmp = m_Cosem.GetDataFromPacket(reply, ref allData);
                        if (OnDataReceived != null)
                        {
                            OnDataReceived(this, (byte[])allData);
                        }
                        //If this was last frame.
                        if ((tmp & RequestTypes.Frame) == 0)
                        {
                            moredata &= ~RequestTypes.Frame;
                            break;
                        }
                        int current = m_Cosem.GetCurrentProgressStatus(allData);
                        //TODO: System.Diagnostics.Debug.Assert(!(current == maxProgress && moredata != RequestTypes.None));
                        NotifyProgress(text, (int)(multiplier * current), maxProgress);
                    }
                    if (Parent.InactivityMode == InactivityMode.ReopenActive && Media is GXSerial && DateTime.Now.Subtract(ConnectionStartTime).TotalSeconds > 40)
                    {
                        Parent.Disconnect();
                        Parent.InitializeConnection();
                    }
                    if ((moredata & RequestTypes.DataBlock) != 0)
                    {
                        //Send Receiver Ready.
                        data = m_Cosem.ReceiverReady(RequestTypes.DataBlock);
                        GXLogWriter.WriteLog("Get Next Data block: ", (byte[])data);
                        reply = ReadDLMSPacket(data);
                        moredata = m_Cosem.GetDataFromPacket(reply, ref allData);
                        if (OnDataReceived != null)
                        {
                            OnDataReceived(this, (byte[])allData);
                        }
                        int current = m_Cosem.GetCurrentProgressStatus(allData);
                        //TODO: System.Diagnostics.Debug.Assert(!(current == maxProgress && moredata != RequestTypes.None));
                        NotifyProgress(text, (int)(multiplier * current), maxProgress);
                    }                    
                }
            }
            return allData;
        }        

        public GXDLMSObjectCollection GetObjects()
        {
            GXLogWriter.WriteLog("--- Collecting objects. ---");
            byte[] allData;
            try
            {
                NotifyProgress("Collecting objects", 0, 1);
                allData = ReadDataBlock(m_Cosem.GetObjectsRequest(), "Collecting objects");
            }
            catch (Exception Ex)
            {
                throw new Exception("GetObjects failed. " + Ex.Message);
            }            
            GXDLMSObjectCollection objs = m_Cosem.ParseObjects(allData, true);
            NotifyProgress("Collecting objects", objs.Count, objs.Count);
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
            foreach (int it in (obj as IGXDLMSBase).GetAttributeIndexToRead())
            {
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
                        if (CurrentProfileGeneric.AccessSelector != AccessRange.Entry)
                        {
                            byte[] tmp = m_Cosem.ReadRowsByRange(CurrentProfileGeneric.Name, CurrentProfileGeneric.CaptureObjects[0].LogicalName, CurrentProfileGeneric.CaptureObjects[0].ObjectType, CurrentProfileGeneric.CaptureObjects[0].Version, Convert.ToDateTime(CurrentProfileGeneric.From), Convert.ToDateTime(CurrentProfileGeneric.To));
                            ReadDataBlock(tmp, "Reading profile generic data", 1);
                        }
                        else
                        {
                            byte[] tmp = m_Cosem.ReadRowsByEntry(CurrentProfileGeneric.Name, Convert.ToInt32(CurrentProfileGeneric.From), Convert.ToInt32(CurrentProfileGeneric.To));
                            ReadDataBlock(tmp, "Reading profile generic data " + CurrentProfileGeneric.Name, 1);
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
                    byte[] data = m_Cosem.Read(obj.Name, obj.ObjectType, it)[0];
                    try
                    {
                        data = ReadDataBlock(data, "Read object type " + obj.ObjectType + " index: " + it);
                    }
                    catch (GXDLMSException ex)
                    {
                        if (ex.ErrorCode == 3) //If read is denied.
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
                        object value = m_Cosem.GetValue(data);
                        DataType type;
                        if (value is byte[] && (type = obj.GetUIDataType(it)) != DataType.None)
                        {
                            value = GXDLMS.Common.GXHelpers.ConvertFromDLMS(value, obj.GetDataType(it), type, true);
                        }
                        (obj as IGXDLMSBase).SetValue(it, value, false);
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
            for (int it = 1; it != (obj as IGXDLMSBase).GetAttributeCount() + 1; ++it)
            {
                object value;
                if (obj.GetDirty(it, out value))
                {
                    //Read DLMS data type if not known.
                    DataType type = obj.GetDataType(it);
                    if (type == DataType.None)
                    {
                        byte[] data = m_Cosem.Read(obj.Name, obj.ObjectType, it)[0];
                        data = ReadDataBlock(data, "Write object type " + obj.ObjectType);
                        type = m_Cosem.GetDLMSDataType(data);
                        if (type == DataType.None)
                        {
                            throw new Exception("Failed to write value. Data type not set.");
                        }
                        obj.SetDataType(it, type);
                    }
                    try
                    {
                        foreach (byte[] tmp in m_Cosem.Write(obj.Name, value, type, obj.ObjectType, it))
                        {
                            ReadDataBlock(tmp, "Write object");
                        }
                        obj.ClearDirty(it);
                        //Read data once again to make sure it is updated.
                        byte[] data = m_Cosem.Read(obj.Name, obj.ObjectType, it)[0];
                        data = ReadDataBlock(data, "Read object " + obj.ObjectType);
                        value = m_Cosem.GetValue(data);
                        obj.SetValue(it, value);
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

        public object ReadValue(object data, ObjectType interfaceClass, int attributeOrdinal, ref DataType type)
        {
            byte[] allData = null;
            object reply;
            allData = Read(data, interfaceClass, attributeOrdinal);
            allData = ReadDataBlock(allData, "Read object");             
            reply = m_Cosem.GetValue(allData);
            //If datatype is unknown
            if (type == DataType.None)
            {
                type = m_Cosem.GetDLMSDataType(allData);
            }
            return reply;
        }

        public GXDLMSObjectCollection GetProfileGenericColumns(object name)
        {
            byte[] allData = ReadDataBlock(Read(name, ObjectType.ProfileGeneric, 3), "Get profile generic columns...");
            if (allData == null)
            {
                return null;
            }
            return m_Cosem.ParseColumns(allData);            
        }

        public void KeepAlive()
        {
            byte[] allData = null, reply = null;
            byte[] data = m_Cosem.GetKeepAlive();
            GXLogWriter.WriteLog("Send Keep Alive", data);
            reply = ReadDLMSPacket(data);
            m_Cosem.GetDataFromPacket(reply, ref allData);

        }      
    }    
}
