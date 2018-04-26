//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Version:         $Revision: 10052 $,
//                  $Date: 2018-04-26 10:11:27 +0300 (Thu, 26 Apr 2018) $
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
// More information of Gurux DLMS/COSEM Director: http://www.gurux.org/GXDLMSDirector
//
// This code is licensed under the GNU General Public License v2.
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using Gurux.Serial;
using System.Windows.Forms;
using Gurux.DLMS;
using System.Data;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using GXDLMS.ManufacturerSettings;
using GXDLMS.Common;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Objects;
using Gurux.DLMS.Enums;
using Gurux.Common;
using System.Reflection;

namespace GXDLMSDirector
{
    [Serializable]
    public class GXDLMSDevice
    {
        DeviceState m_Status;

        void UpdateStatus(DeviceState state)
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
        GXDLMSObjectCollection m_Objects;

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
        /// Define how long reply is waited in seconds.
        /// </summary>
        [DefaultValue(5)]
        public int WaitTime
        {
            get;
            set;
        }

        /// <summary>
        /// Maximum used baud rate.
        /// </summary>
        [DefaultValue(0)]
        public int MaximumBaudRate
        {
            get;
            set;
        }

        /// <summary>
        /// Used authentication.
        /// </summary>
        [DefaultValue(Authentication.None)]
        public Authentication Authentication
        {
            get;
            set;
        }

        /// <summary>
        /// Used standard.
        /// </summary>
        [DefaultValue(Standard.DLMS)]
        public Standard Standard
        {
            get;
            set;
        }
        
        /// <summary>
        /// Password is used only if authentication is used.
        /// </summary>
        [DefaultValue("")]
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Password is used only if authentication is used.
        /// </summary>
        [DefaultValue(null)]
        public byte[] HexPassword
        {
            get;
            set;
        }

        /// <summary>
        /// Used communication security.
        /// </summary>
        [DefaultValue(Security.None)]
        public Security Security
        {
            get;
            set;
        }

        /// <summary>
        /// System Title.
        /// </summary>
        [DefaultValue("")]
        public string SystemTitle
        {
            get;
            set;
        }

        /// <summary>
        /// Server System Title.
        /// </summary>
        [DefaultValue("")]
        public string ServerSystemTitle
        {
            get;
            set;
        }

        /// <summary>
        /// Use pre-established application associations.
        /// </summary>
        [DefaultValue(false)]
        public bool PreEstablished
        {
            get;
            set;
        }


        /// <summary>
        /// Block cipher key.
        /// </summary>
        [DefaultValue("")]
        public string BlockCipherKey
        {
            get;
            set;
        }

        /// <summary>
        /// Authentication key.
        /// </summary>
        [DefaultValue("")]
        public string AuthenticationKey
        {
            get;
            set;
        }

        /// <summary>
        /// Invocation counter.
        /// </summary>
        [DefaultValue(0)]
        public UInt32 InvocationCounter
        {
            get;
            set;
        }

        /// <summary>
        /// Frame counter is used to update InvocationCounter automatically.
        /// </summary>
        [DefaultValue(null)]
        public string FrameCounter
        {
            get;
            set;
        }
        


        /// <summary>
        /// Static challenge.
        /// </summary>
        [DefaultValue("")]
        public string Challenge
        {
            get;
            set;
        }

        /// <summary>
        /// Used Physical address.
        /// </summary>
        /// <remarks>
        /// Server HDLC Address (Logical + Physical address)  might be 1,2 or 4 bytes long.
        /// </remarks>
        [DefaultValue(null)]
        public object PhysicalAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Used logical address.
        /// </summary>
        [DefaultValue(null)]
        public int LogicalAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Used logical client ID.
        /// </summary>
        /// <remarks>
        /// This is opsolite. Use ClientAddress.
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
        /// Standard says that Time zone is from normal time to UTC in minutes.
        /// If meter is configured to use UTC time (UTC to normal time) set this to true.
        /// </summary>
        [DefaultValue(false)]
        public bool UtcTimeZone
        {
            get;
            set;
        }

        /// <summary>
        /// USed logical client ID.
        /// </summary>
        /// <remarks>
        /// Client ID is always 1 byte long.
        /// </remarks>
        [DefaultValue(0x10)]
        public int ClientAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Is IEC 62056-21 skipped when using serial port connection.
        /// </summary>
        [DefaultValue(StartProtocolType.IEC)]
        public StartProtocolType StartProtocol
        {
            get;
            set;
        }

        /// <summary>
        /// Is serial port access through TCP/IP or UDP converter.
        /// </summary>
        [DefaultValue(false)]
        public bool UseRemoteSerial
        {
            get;
            set;
        }

        /// <summary>
        /// Is wrapper used.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public bool UseWrapper
        {
            get;
            set;
        }


        /// <summary>
        /// The maximum information field length in transmit.
        /// </summary>
        /// <remarks>
        /// DefaultValue is 128. Minimum value is 32 and max value is 128.
        /// </remarks>
        [DefaultValue(128)]
        public UInt16 MaxInfoTX
        {
            get;
            set;
        }

        /// <summary>
        /// The maximum information field length in receive.
        /// </summary>
        /// <remarks>
        /// DefaultValue is 128. Minimum value is 32 and max value is 128.
        /// </remarks>
        [DefaultValue(128)]
        public UInt16 MaxInfoRX
        {
            get;
            set;
        }

        /// <summary>
        /// The window size in transmit.
        /// </summary>
        /// <remarks>
        /// DefaultValue is 1.
        /// </remarks>
        public byte WindowSizeTX
        {
            get;
            set;
        }

        /// <summary>
        /// The window size in receive.
        /// </summary>
        /// <remarks>
        /// DefaultValue is 1.
        /// </remarks>
        public byte WindowSizeRX
        {
            get;
            set;
        }


        /// <summary>
        /// Proposed maximum size of PDU.
        /// </summary>
        /// <remarks>
        /// DefaultValue is 1.
        /// </remarks>
        [DefaultValue(0xFFFF)]
        public UInt16 PduSize
        {
            get;
            set;
        }

        /// <summary>
        /// User Id.
        /// </summary>
        /// <remarks>
        /// In default user id is not used.
        /// </remarks>
        [DefaultValue(-1)]
        public short UserId
        {
            get;
            set;
        }


        /// <summary>
        ///Inactivity timeout.
        /// </summary>
        /// <remarks>
        /// DefaultValue is 120 second.
        /// </remarks>
        public int InactivityTimeout
        {
            get;
            set;
        }

        /// <summary>
        /// Used ServiceClass.
        /// </summary>
        [DefaultValue(ServiceClass.Confirmed)]
        public ServiceClass ServiceClass
        {
            get;
            set;
        }

        /// <summary>
        /// Used priority.
        /// </summary>
        [DefaultValue(Priority.High)]
        public Priority Priority
        {
            get;
            set;
        }


        /// <summary>
        /// Server address size.
        /// </summary>
        [DefaultValue(0)]
        public byte ServerAddressSize
        {
            get;
            set;
        }

        void NotifyProgress(object sender, string description, int current, int maximium)
        {
            if (OnProgress != null)
            {
                OnProgress(sender, description, current, maximium);
            }
        }

        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Is media verbose mode used.
        /// </summary>
        public bool Verbose
        {
            get;
            set;
        }

        /// <summary>
        /// Used Conformance.
        /// </summary>
        public int Conformance
        {
            get
            {
                return (int)communicator.client.ProposedConformance;
            }
            set
            {
                communicator.client.ProposedConformance = (Conformance)value;
            }
        }

        public void InitializeConnection()
        {
            try
            {
                UpdateStatus(DeviceState.Connecting);
                communicator.InitializeConnection();
                UpdateStatus(DeviceState.Connected);
            }
            catch (Exception Ex)
            {
                UpdateStatus(DeviceState.Initialized);
                if (Media != null)
                {
                    Media.Close();
                    if (Media is GXSerial)
                    {
                        Media.Settings = StartMediaSettings;
                    }
                }
                throw Ex;
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
        public Gurux.DLMS.ManufacturerSettings.GXObisCodeCollection ObisCodes
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
        public IGXManufacturerExtension Extension
        {
            get;
            internal set;
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
        /// Name of the manufacturer.
        /// </summary>
        [Browsable(false)]
        public string Manufacturer
        {
            get;
            set;
        }

        /// <summary>
        /// What HDLC Addressing is used.
        /// </summary>
        [Browsable(false)]
        public HDLCAddressType HDLCAddressing
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSDevice(Gurux.Common.IGXMedia media)
        {
            StartProtocol = StartProtocolType.IEC;
            ClientAddress = 0x10; // Public client (lowest security level).
            PhysicalAddress = 1;
            Password = "";
            Authentication = Authentication.None;
            communicator = new GXDLMSCommunicator(this, media);
            m_Objects = communicator.client.Objects;
            m_Objects.Tag = this;
            communicator.OnProgress += new ProgressEventHandler(this.NotifyProgress);
            this.KeepAlive = new System.Timers.Timer();
            this.KeepAlive.Interval = 40000;
            this.KeepAlive.Elapsed += new System.Timers.ElapsedEventHandler(KeepAlive_Elapsed);
            m_Status = DeviceState.Initialized;
            WaitTime = 5;
            InactivityTimeout = 120;
            WindowSizeRX = WindowSizeTX = 1;
            MaxInfoRX = MaxInfoTX = 128;
            PduSize = 0xFFFF;
            ServiceClass = ServiceClass.Confirmed;
            Priority = Priority.High;
            UserId = -1;
        }

        /// <summary>
        /// Constructor for serializing.
        /// </summary>
        public GXDLMSDevice() : this(null)
        {
        }

        [Browsable(false)]
        public string MediaType
        {
            get
            {
                return communicator.media.GetType().ToString();
            }
            set
            {
                if (string.Compare(value, typeof(Gurux.Net.GXNet).FullName, true) == 0)
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
                    communicator.media = (IGXMedia)Activator.CreateInstance(type);
                }
            }
        }

        /// <summary>
        /// Media settings as a string.
        /// </summary>
        public string MediaSettings
        {
            get
            {
                return communicator.media.Settings;
            }
            set
            {
                communicator.media.Settings = value;
            }
        }

        /// <summary>
        /// Media settings are changed for serial port connection.
        /// </summary>
        public string StartMediaSettings;

        /// <summary>
        /// Is Logical name referencing used.
        /// </summary>
        [DefaultValue(false)]
        [XmlElement("UseLN")]
        public bool UseLogicalNameReferencing
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [XmlArray("Objects2")]
        public GXDLMSObjectCollection Objects
        {
            get
            {
                return m_Objects;
            }
        }

        /// <summary>
        /// Do not use this. This is obsolete.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [XmlArray("Objects")]
        [XmlArrayItem("Object", Type = typeof(GXDLMSObjectSerializer<GXDLMSObject>))]
        //[Obsolete("Use Objects", true)]
        public GXDLMSObjectCollection ObsoleteObjects
        {
            get
            {
                return m_Objects;
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
                    UpdateStatus(DeviceState.Disconnecting);
                    communicator.Disconnect();
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
            foreach (GXDLMSObject it in m_Objects)
            {
                if (type == it.ObjectType && it.LogicalName == logicalName)
                {
                    return it;
                }
            }
            return null;
        }

        bool ShouldSkip(GXDLMSObject it, int index)
        {
            //Skip Scaler and unit.
            if (it is GXDLMSRegister && index == 3)
            {
                return true;
            }
            return false;
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
                if (this.Extension != null)
                {
                    cols = this.Extension.Refresh(item, this.Comm);
                }
                if (cols == null)
                {
                    Comm.GetProfileGenericColumns(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                    m_Objects.Add(it);
                }
                GXLogWriter.WriteLog("--- Created " + m_Objects.Count.ToString() + " objects. ---");
                //Read registers units and scalers.
                int cnt = Objects.Count;
                GXLogWriter.WriteLog("--- Reading scalers and units. ---");
                for (pos = 0; pos != cnt; ++pos)
                {
                    GXDLMSObject it = Objects[pos];
                    this.OnProgress(this, "Reading scalers and units.", cnt + pos + 1, 3 * cnt);
                    if (it is GXDLMSRegister)
                    {
                        //Read scaler first.
                        try
                        {
                            Comm.ReadValue(it, 3);
                        }
                        catch (Exception ex)
                        {
                            GXLogWriter.WriteLog(ex.Message);
                            UpdateError(it, 3, ex);
                            if (ex is GXDLMSException)
                            {
                                continue;
                            }
                            throw ex;
                        }
                    }
                    if (it is GXDLMSDemandRegister)
                    {
                        try
                        {
                            //Read scaler first.
                            Comm.ReadValue(it, 4);
                            //Read Period
                            Comm.ReadValue(it, 8);
                            //Read number of periods
                            Comm.ReadValue(it, 9);
                        }
                        catch (Exception ex)
                        {
                            GXLogWriter.WriteLog(ex.Message);
                            UpdateError(it, 3, ex);
                            if (ex is GXDLMSException)
                            {
                                continue;
                            }
                            throw ex;
                        }
                    }
                }
                GXLogWriter.WriteLog("--- Reading scalers and units end. ---");
                if (!UseLogicalNameReferencing)
                {
                    GXLogWriter.WriteLog("--- Reading Access rights. ---");
                    try
                    {
                        foreach (GXDLMSAssociationShortName sn in Objects.GetObjects(ObjectType.AssociationShortName))
                        {
                            Comm.ReadValue(sn, 3);
                        }
                    }
                    catch (Exception ex)
                    {
                        GXLogWriter.WriteLog(ex.Message);
                    }
                    GXLogWriter.WriteLog("--- Reading Access rights end. ---");
                }
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
