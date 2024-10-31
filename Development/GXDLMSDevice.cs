//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Version:         $Revision: 14931 $,
//                  $Date: 2024-10-31 09:29:15 +0200 (Thu, 31 Oct 2024) $
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
// MERCgHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details.
//
// More information of Gurux DLMS/COSEM Director: https://www.gurux.org/GXDLMSDirector
//
// This code is licensed under the GNU General Public License v2.
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using GXDLMS.ManufacturerSettings;
using Gurux.Common;
using System.Reflection;
using Gurux.DLMS;
using Gurux.DLMS.Enums;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Objects;
using System.Text;
using Gurux.Net;
using Gurux.Serial;
using Gurux.DLMS.Objects.Enums;
using Gurux.DLMS.Extension;

namespace GXDLMSDirector
{
    [Serializable]
    public class GXDLMSDevice : GXDLMSMeter
    {
        DeviceState m_Status;

        internal void UpdateStatus(DeviceState state)
        {
            //Clear connecting.
            if (state == DeviceState.Connected)
            {
                state &= ~DeviceState.Connecting;
                state |= DeviceState.Initialized;
            }
            m_Status = state;
            if (OnStatusChanged != null)
            {
                OnStatusChanged(this, m_Status);
            }
        }

        public string GetCommandLineParameters()
        {
            StringBuilder sb = new StringBuilder();
            if (communicator.media is GXNet n)
            {
                if (n.HostName == null)
                {
                    n.Settings = MediaSettings;
                }
                sb.Append("-h " + n.HostName);
                sb.Append(" -p " + n.Port);
            }
            if (communicator.media is GXSerial s)
            {
                if (s.PortName == null)
                {
                    s.Settings = MediaSettings;
                }
                sb.Append("-S " + s.PortName);
                sb.Append(":" + s.BaudRate);
                sb.Append(":" + s.DataBits);
                sb.Append(s.Parity);
                sb.Append(s.StopBits);
            }
            if (!UseLogicalNameReferencing)
            {
                sb.Append(" -r sn");
            }
            if (InterfaceType != InterfaceType.HDLC)
            {
                sb.Append(" -i " + InterfaceType.ToString());
            }
            if (ClientAddress != 16)
            {
                sb.Append(" -c " + ClientAddress);
            }
            if (HDLCAddressing == HDLCAddressType.Default)
            {
                if (PhysicalAddress != 1 || LogicalAddress != 0)
                {
                    if (InterfaceType == InterfaceType.HDLC || InterfaceType == InterfaceType.HdlcWithModeE || InterfaceType == InterfaceType.PlcHdlc)
                    {
                        sb.Append(" -s " + GXDLMSClient.GetServerAddress(LogicalAddress, Convert.ToInt32(PhysicalAddress)));
                    }
                    else
                    {
                        sb.Append(" -s " + communicator.client.ServerAddress);
                    }
                }
            }
            else if (HDLCAddressing == HDLCAddressType.SerialNumber)
            {
                if (PhysicalAddress != 1)
                {
                    sb.Append(" -n " + PhysicalAddress);
                }
            }
            if (Authentication != Authentication.None)
            {
                sb.Append(" -a " + Authentication);
                if (!string.IsNullOrEmpty(Password))
                {
                    string pw = ASCIIEncoding.ASCII.GetString(CryptHelper.Decrypt(Password, GXDLMSDirector.Password.Key));
                    if (!string.IsNullOrEmpty(pw))
                    {
                        sb.Append(" -P " + pw);
                    }
                }
                if (HexPassword != null && HexPassword.Length != 0)
                {
                    sb.Append(" -P 0x" + ASCIIEncoding.ASCII.GetString(CryptHelper.Decrypt(HexPassword, GXDLMSDirector.Password.Key)));
                }
            }
            if (Security != Security.None)
            {
                sb.Append(" -C " + Security);
            }
            if (SecuritySuite != SecuritySuite.Suite0)
            {
                sb.Append(" -V " + SecuritySuite);
            }
            if (Signing != Signing.None)
            {
                sb.Append(" -K " + Signing);
            }
            if (Authentication == Authentication.HighSHA256 ||
                Authentication == Authentication.HighECDSA || Security != Security.None)
            {
                if (!string.IsNullOrEmpty(SystemTitle))
                {
                    sb.Append(" -T " + SystemTitle);
                }
                if (!string.IsNullOrEmpty(ServerSystemTitle))
                {
                    sb.Append(" -M " + ServerSystemTitle);
                }
                if (Security != Security.None)
                {
                    if (!string.IsNullOrEmpty(AuthenticationKey))
                    {
                        sb.Append(" -A " + AuthenticationKey);
                    }
                    if (!string.IsNullOrEmpty(BlockCipherKey))
                    {
                        sb.Append(" -B " + BlockCipherKey);
                    }
                    if (!string.IsNullOrEmpty(DedicatedKey))
                    {
                        sb.Append(" -D " + DedicatedKey);
                    }
                }
                if (!string.IsNullOrEmpty(FrameCounter))
                {
                    sb.Append(" -v " + FrameCounter);
                }
            }
            if (Standard != Standard.DLMS)
            {
                sb.Append(" -d " + Standard);
            }
            if (InterfaceType == InterfaceType.HDLC || InterfaceType == InterfaceType.HdlcWithModeE)
            {
                if (GbtWindowSize != 1)
                {
                    sb.Append(" -W " + GbtWindowSize);
                }
                if (WindowSizeRX != 128)
                {
                    sb.Append(" -w " + WindowSizeRX);
                }
                if (MaxInfoTX != 1)
                {
                    sb.Append(" -f " + MaxInfoRX);
                }
            }
            sb.Append(" -t Verbose");
            return sb.ToString();
        }

        [Browsable(false)]
        [XmlIgnore()]
        public DeviceState Status
        {
            get
            {
                return m_Status;
            }
        }

        [Browsable(false)]
        [XmlIgnore()]
        public ProgressEventHandler OnProgress;

        [Browsable(false)]
        [XmlIgnore()]
        public StatusEventHandler OnStatusChanged;

        [Browsable(false)]
        [XmlIgnore()]
        public MessageTraceEventHandler OnTrace;

        /// <summary>
        /// Meter sends event notification.
        /// </summary>
        [Browsable(false)]
        [XmlIgnore()]
        public ReceivedEventHandler OnEvent;


        [Browsable(false)]
        [XmlIgnore()]
        System.Timers.Timer KeepAlive;

        [Browsable(false)]
        [XmlIgnore()]
        GXDLMSCommunicator communicator;

        public void KeepAliveStart()
        {
            if (InactivityTimeout != 0 && Media.IsOpen)
            {
                KeepAlive.Interval = InactivityTimeout * 1000;
                KeepAlive.Start();
            }
        }

        public void KeepAliveStop()
        {
            KeepAlive.Stop();
        }

        /// <summary>
        /// Used logical client ID.
        /// </summary>
        /// <remarks>
        /// This is obsolete. Use ClientAddress.
        /// </remarks>
        [DefaultValue(null)]
        public object ClientID
        {
            get
            {
                return null;
            }
            set
            {
                ClientAddress = Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Is wrapper used.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public bool UseWrapper
        {
            get
            {
                return false;
            }
            set
            {
                if (value)
                {
                    InterfaceType = InterfaceType.WRAPPER;
                }
            }
        }

        void NotifyProgress(object sender, string description, int current, int maximium)
        {
            if (OnProgress != null)
            {
                OnProgress(sender, description, current, maximium);
            }
        }

        public void InitializeConnection()
        {
            try
            {
                UpdateStatus(DeviceState.Connecting);
                communicator.InitializeConnection(true);
                UpdateStatus(DeviceState.Connected);
            }
            catch (Exception)
            {
                UpdateStatus(DeviceState.Initialized);
                if (Media != null)
                {
                    Media.Close();
                }
                throw;
            }
        }

        public GXDLMSCommunicator Comm
        {
            get
            {
                return communicator;
            }
        }

        [Browsable(false)]
        [XmlIgnore()]
        public GXObisCodeCollection ObisCodes
        {
            get
            {
                return communicator.client.CustomObisCodes;
            }
            set
            {
                communicator.client.CustomObisCodes = value;
            }
        }

        [Browsable(false)]
        [XmlIgnore()]
        public Gurux.Common.IGXMedia Media
        {
            get
            {
                return communicator.media;
            }
            set
            {
                communicator.media = value;
            }
        }

        [Browsable(false)]
        [XmlIgnore()]
        public GXManufacturerCollection Manufacturers
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSDevice(IGXMedia media) : base()
        {
            communicator = new GXDLMSCommunicator(this, media);
            Objects = communicator.client.Objects;
            Objects.Tag = this;
            communicator.OnProgress += new ProgressEventHandler(this.NotifyProgress);
            this.KeepAlive = new System.Timers.Timer();
            this.KeepAlive.Interval = 40000;
            this.KeepAlive.Elapsed += new System.Timers.ElapsedEventHandler(KeepAlive_Elapsed);
            m_Status = DeviceState.Initialized;
        }

        /// <summary>
        /// Constructor for serializing.
        /// </summary>
        public GXDLMSDevice() : this(null)
        {
        }

        [Browsable(false)]
        public override string MediaType
        {
            get
            {
                if (communicator.media == null)
                {
                    return null;
                }
                return communicator.media.GetType().ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    communicator.media = null;
                }
                else if (string.Compare(value, typeof(Gurux.Net.GXNet).FullName, true) == 0)
                {
                    communicator.media = new Gurux.Net.GXNet();
                }
                else if (string.Compare(value, typeof(Gurux.Serial.GXSerial).FullName, true) == 0)
                {
                    communicator.media = new Gurux.Serial.GXSerial();
                }
                else if (string.Compare(value, typeof(Gurux.Terminal.GXTerminal).FullName, true) == 0)
                {
                    communicator.media = new Gurux.Terminal.GXTerminal();
                }
                else
                {
                    Type type = Type.GetType(value);
                    if (type == null)
                    {
                        string ns = "";
                        int pos = value.LastIndexOf('.');
                        if (pos != -1)
                        {
                            ns = value.Substring(0, pos);
                        }
                        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            if (assembly.GetName().Name == ns)
                            {
                                if (assembly.GetType(value, false, true) != null)
                                {
                                    type = assembly.GetType(value);
                                }
                            }
                        }
                    }
                    if (type == null)
                    {
                        throw new Exception("Invalid media type: " + value);
                    }
                    communicator.media = (IGXMedia)Activator.CreateInstance(type);
                }
            }
        }

        public void Disconnect()
        {
            try
            {
                if (KeepAlive.Enabled)
                {
                    KeepAlive.Stop();
                }
                if (Comm.media.IsOpen && m_Status != DeviceState.Disconnecting)
                {
                    if (Comm.media is IGXMedia2)
                    {
                        if (((IGXMedia2)Comm.media).AsyncWaitHandle != null)
                        {
                            ((IGXMedia2)Comm.media).AsyncWaitHandle.Set();
                        }
                    }
                    UpdateStatus(DeviceState.Disconnecting);
                    communicator.Disconnect();
                }
                else
                {
                    Comm.media.Close();
                }
            }
            catch (Exception Ex)
            {
                //Do not show error occurred in disconnect. Write error only to the log file.
                GXLogWriter.WriteLog(Ex.Message);
            }
            finally
            {
                UpdateStatus(DeviceState.Initialized);
            }
        }

        GXDLMSObject FindObject(ObjectType type, string logicalName)
        {
            foreach (GXDLMSObject it in Objects)
            {
                if (type == it.ObjectType && it.LogicalName == logicalName)
                {
                    return it;
                }
            }
            return null;
        }

        delegate void UpdateColumnsEventHandler(GXDLMSProfileGeneric item, GXManufacturer man);

        public void UpdateColumns(GXDLMSProfileGeneric item, GXManufacturer man)
        {
            if (Comm.parentForm.InvokeRequired)
            {
                try
                {
                    Comm.parentForm.Invoke(new UpdateColumnsEventHandler(UpdateColumns), item, man);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                return;
            }
            try
            {
                item.Buffer.Clear();
                item.CaptureObjects.Clear();
                List<GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>> cols = null;
                List<DataColumn> columns = new List<DataColumn>();
                try
                {
                    Comm.GetProfileGenericColumns(item);
                    if (Standard == Standard.Italy && item.CaptureObjects.Count == 0)
                    {
                        cols = GetColumns(Comm.client.Objects, Comm.client.CustomObisCodes, item.LogicalName, 0);
                        GXDLMSConverter c = new GXDLMSConverter(Standard);
                        foreach (var it in cols)
                        {
                            c.UpdateOBISCodeInformation(it.Key);
                        }
                        item.CaptureObjects.AddRange(cols);
                    }
                }
                catch (GXDLMSException ex)
                {
                    if (Standard == Standard.Italy)
                    {
                        GXDLMSData obj = Comm.client.Objects.FindByLN(ObjectType.Data, "0.0.96.1.3.255") as GXDLMSData;
                        int type = 0;
                        if (obj != null)
                        {
                            if (obj.Value == null)
                            {
                                try
                                {
                                    Comm.ReadValue(obj, 2);
                                    type = Convert.ToInt16(obj.Value);
                                }
                                catch (Exception)
                                {
                                    type = 0;
                                }
                            }
                            else
                            {
                                type = Convert.ToInt16(obj.Value);
                            }
                        }
                        cols = GetColumns(Comm.client.Objects, Comm.client.CustomObisCodes, item.LogicalName, type);
                        item.CaptureObjects.Clear();
                        GXDLMSConverter c = new GXDLMSConverter(Standard);
                        foreach (var it in cols)
                        {
                            c.UpdateOBISCodeInformation(it.Key);
                        }
                        item.CaptureObjects.AddRange(cols);
                    }
                    if (cols == null || cols.Count == 0)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject> CreateColumn(GXDLMSObjectCollection objects, GXObisCodeCollection obisCodes, ObjectType ot, string ln, int index)
        {
            return CreateColumn(objects, obisCodes, ot, ln, index, DataType.None);
        }

        private static GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject> CreateColumn(GXDLMSObjectCollection objects, GXObisCodeCollection obisCodes, ObjectType ot, string ln, int index, DataType dt)
        {
            GXDLMSObject obj = objects.FindByLN(ot, ln);
            if (obj == null)
            {
                GXObisCode code = obisCodes.FindByLN(ot, ln, null);
                obj = GXDLMSClient.CreateObject(ot);
                obj.LogicalName = ln;
                if (code != null)
                {
                    GXDLMSAttributeSettings s = code.Attributes.Find(index);
                    if (s != null)
                    {
                        obj.SetDataType(index, s.Type);
                        obj.SetUIDataType(index, s.UIType);
                        obj.SetValues(index, s.Values);
                    }
                }
                else
                {
                    obj.SetUIDataType(index, dt);
                }
            }
            return new GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>(obj, new GXDLMSCaptureObject(index, 0));
        }

        private static List<GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>> GetColumns(GXDLMSObjectCollection objects, GXObisCodeCollection obisCodes, string ln, int type)
        {
            List<GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>> list = new List<GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>>();
            //Event Logbook
            if (ln == "7.0.99.98.0.255")
            {
                //If meter.
                if (type == 2)
                {
                    list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.1.1.0.255", 2));
                    list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.96.15.2.255", 2));
                    list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.96.11.2.255", 2));
                    list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "7.1.96.5.1.255", 2));
                    list.Add(CreateColumn(objects, obisCodes, ObjectType.AssociationLogicalName, "0.0.40.0.0.255", 11));
                }
                else
                {
                    //If concentrator.
                    list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.1.1.0.255", 2));
                    list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.96.15.2.255", 2));
                    list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.96.11.2.255", 2));
                    list.Add(CreateColumn(objects, obisCodes, ObjectType.AssociationLogicalName, "0.0.40.0.0.255", 11));
                    list.Add(CreateColumn(objects, obisCodes, ObjectType.GSMDiagnostic, "0.0.25.6.0.255", 5));
                    list.Add(CreateColumn(objects, obisCodes, ObjectType.GSMDiagnostic, "0.0.25.6.0.255", 6));
                }
            }
            else if (ln == "7.0.99.99.3.255")
            {
                //If meter.
                if (type == 2)
                {
                    list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.1.1.0.255", 2));
                    list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "7.1.96.5.1.255", 2));
                    list.Add(CreateColumn(objects, obisCodes, ObjectType.Register, "7.0.13.2.0.255", 2));
                    list.Add(CreateColumn(objects, obisCodes, ObjectType.Register, "7.0.12.2.0.255", 2));
                }
                else
                {
                    //If concentrator.
                    list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.1.1.0.255", 2));
                    list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "7.1.96.5.1.255", 2));
                }
            }
            else if (ln == "7.0.99.98.1.255")
            {
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.1.1.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.96.11.1.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.96.15.1.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.AssociationLogicalName, "0.0.40.0.0.255", 11));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Register, "7.0.13.2.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "7.1.96.5.1.255", 2));
            }
            else if (ln == "7.0.99.98.6.255")
            {
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.1.1.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.96.15.3.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "7.0.96.5.1.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.96.5.4.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.MacAddressSetup, "0.1.25.2.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.24.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.23.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.13.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.15.255", 2));
            }
            else if (ln == "7.0.99.16.0.255")
            {
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.96.15.1.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.ParameterMonitor, "0.0.16.2.0.255", 2));
            }
            else if (ln == "7.0.98.11.0.255")
            {
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.1.1.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "7.0.96.5.1.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "7.0.96.10.2.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "7.0.0.1.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Register, "7.0.13.2.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Register, "7.0.13.2.1.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Register, "7.0.13.2.2.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Register, "7.0.13.2.3.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Register, "7.0.12.2.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.ExtendedRegister, "7.0.43.45.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.ExtendedRegister, "7.0.43.45.0.255", 5));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.TariffPlan, "0.0.94.39.21.255", 2));
            }
            //Not Enrolled Detected List
            else if (ln == "0.0.21.0.1.255" ||
                //Enrolled Detected List
                ln == "0.1.21.0.1.255")
            {
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.1.1.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.36.255", 2, DataType.DateTime));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.MacAddressSetup, "0.1.25.2.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.35.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.23.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.34.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.38.255", 2));
            }
            // White list
            else if (ln == "0.0.21.0.2.255")
            {
                list.Add(CreateColumn(objects, obisCodes, ObjectType.MacAddressSetup, "0.1.25.2.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.13.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.15.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.39.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.1.1.0.255", 2));
            }
            // Bloack list
            else if (ln == "0.0.21.0.3.255")
            {
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.1.1.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.MacAddressSetup, "0.1.25.2.0.255", 2));
            }
            // Command Queue
            else if (ln == "0.0.21.0.10.255")
            {
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.1.1.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.MacAddressSetup, "0.1.25.2.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.45.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.27.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.50.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.28.255", 2));
            }
            // Directory event logbook
            else if (ln == "7.0.99.98.3.255")
            {
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.1.1.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.96.15.4.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.96.11.4.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.1.25.2.0.255", 2));
            }
            //Push Data Queue.
            else if (ln == "0.0.98.1.0.255")
            {
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.1.1.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.MacAddressSetup, "0.1.25.2.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.50.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.23.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.24.255", 2));
            }
            //Response Queue
            else if (ln == "0.0.21.0.11.255")
            {
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.1.1.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.MacAddressSetup, "0.1.25.2.0.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.27.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.29.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.50.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.23.255", 2));
                list.Add(CreateColumn(objects, obisCodes, ObjectType.Data, "0.0.94.39.24.255", 2));
            }
            return list;
        }

        private static void UpdateError(GXDLMSObject it, int attributeIndex, Exception ex)
        {
            GXDLMSException t = ex as GXDLMSException;
            if (t != null)
            {
                if (t.ErrorCode == 1 || t.ErrorCode == 3)
                {
                    it.SetAccess(attributeIndex, AccessMode.NoAccess);
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                throw ex;
            }
        }

        /// <summary>
        /// After UpdateObjects call objects can be read using Objects property.
        /// </summary>
        public void UpdateObjects()
        {
            try
            {
                GXDLMSObjectCollection objs = Comm.GetObjects();
                objs.Tag = this;
                int pos = 0;
                foreach (GXDLMSObject it in objs)
                {
                    ++pos;
                    NotifyProgress(this, "Creating object " + it.LogicalName, pos, objs.Count);
                    Objects.Add(it);
                }
                GXLogWriter.WriteLog("--- Created " + Objects.Count.ToString() + " objects. ---");
                //Read registers units and scalers.
                int cnt = Objects.Count;
                if (!UseLogicalNameReferencing)
                {
                    GXLogWriter.WriteLog("--- Reading Access rights. ---");
                    try
                    {
                        foreach (GXDLMSAssociationShortName sn in Objects.GetObjects(ObjectType.AssociationShortName))
                        {
                            if (sn.Version > 1)
                            {
                                Comm.ReadValue(sn, 3);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GXLogWriter.WriteLog(ex.Message);
                    }
                    GXLogWriter.WriteLog("--- Reading Access rights end. ---");
                }
                GXLogWriter.WriteLog("--- Reading scalers and units. ---");
                this.OnProgress(this, "Reading scalers and units.", cnt + pos + 1, 3 * cnt);
                if ((Comm.client.NegotiatedConformance & Gurux.DLMS.Enums.Conformance.MultipleReferences) != 0)
                {
                    List<KeyValuePair<GXDLMSObject, int>> list = new List<KeyValuePair<GXDLMSObject, int>>();
                    foreach (GXDLMSObject it in Objects)
                    {
                        if (it is GXDLMSRegister && Comm.client.CanRead(it, 3))
                        {
                            list.Add(new KeyValuePair<GXDLMSObject, int>(it, 3));
                        }
                        if (it is GXDLMSDemandRegister && Comm.client.CanRead(it, 4))
                        {
                            list.Add(new KeyValuePair<GXDLMSObject, int>(it, 4));
                        }
                    }
                    if (list.Count != 0)
                    {
                        try
                        {
                            Comm.ReadList(list);
                        }
                        catch (Exception)
                        {
                            //Show error.
                        }
                    }
                }
                else
                {
                    for (pos = 0; pos != cnt; ++pos)
                    {
                        GXDLMSObject it = Objects[pos];
                        if (it is GXDLMSRegister)
                        {
                            //Read scaler first.
                            try
                            {
                                if (Comm.client.CanRead(it, 3))
                                {
                                    Comm.ReadValue(it, 3);
                                }
                            }
                            catch (Exception ex)
                            {
                                GXLogWriter.WriteLog(ex.Message);
                                it.SetAccess(3, AccessMode.NoAccess);
                                if (ex is GXDLMSException)
                                {
                                    continue;
                                }
                                throw ex;
                            }
                        }
                        if (it is GXDLMSDemandRegister)
                        {
                            //Read scaler first.
                            try
                            {
                                if (Comm.client.CanRead(it, 4))
                                {
                                    Comm.ReadValue(it, 4);
                                }
                            }
                            catch (Exception ex)
                            {
                                GXLogWriter.WriteLog(ex.Message);
                                UpdateError(it, 4, ex);
                                if (ex is GXDLMSException)
                                {
                                    continue;
                                }
                                throw ex;
                            }
                            //Read Period
                            try
                            {
                                if (Comm.client.CanRead(it, 8))
                                {
                                    Comm.ReadValue(it, 8);
                                }
                            }
                            catch (Exception ex)
                            {
                                GXLogWriter.WriteLog(ex.Message);
                                UpdateError(it, 8, ex);
                                if (ex is GXDLMSException)
                                {
                                    continue;
                                }
                                throw ex;
                            }
                            //Read number of periods
                            try
                            {
                                if (Comm.client.CanRead(it, 9))
                                {
                                    Comm.ReadValue(it, 9);
                                }
                            }
                            catch (Exception ex)
                            {
                                GXLogWriter.WriteLog(ex.Message);
                                UpdateError(it, 9, ex);
                                if (ex is GXDLMSException)
                                {
                                    continue;
                                }
                                throw ex;
                            }
                        }
                    }
                }
                GXLogWriter.WriteLog("--- Reading scalers and units end. ---");
                this.OnProgress(this, "Reading profile generic columns.", cnt, cnt);
                foreach (Gurux.DLMS.Objects.GXDLMSProfileGeneric it in objs.GetObjects(ObjectType.ProfileGeneric))
                {
                    ++pos;
                    //Read Profile Generic Columns.
                    try
                    {
                        NotifyProgress(this, "Get profile generic columns", (2 * cnt) + pos, 3 * objs.Count);
                        UpdateColumns(it, Manufacturers.FindByIdentification(Manufacturer));
                        if (it.CaptureObjects == null || it.CaptureObjects.Count == 0)
                        {
                            continue;
                        }
                    }
                    catch
                    {
                        GXLogWriter.WriteLog(string.Format("Failed to read Profile Generic {0} columns.", it.LogicalName));
                        continue;
                    }
                }
            }
            finally
            {
                NotifyProgress(this, "", 0, 0);
            }
        }

        /// <summary>
        /// Serial port connection needs keep alive messages every 40 second...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeepAlive_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                NotifyProgress(this, "Keep Alive", 0, 1);
                communicator.KeepAlive();
            }
            catch (Exception Ex)
            {
                this.Disconnect();
                GXDLMS.Common.Error.ShowError(null, Ex);
            }
            finally
            {
                NotifyProgress(this, "Ready", 1, 1);
            }
        }
    }
}
