//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/GXDLMSDevice.cs $
//
// Version:         $Revision: 9232 $,
//                  $Date: 2017-02-20 09:13:38 +0200 (ma, 20 helmi 2017) $
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
        [System.Xml.Serialization.XmlIgnore()]
        public DeviceState Status
        {
            get
            {
                return m_Status;
            }
        }

        [System.Xml.Serialization.XmlIgnore()]
        public InactivityMode InactivityMode
        {
            get
            {
                return Manufacturers.FindByIdentification(Manufacturer).InactivityMode;
            }
        }

        [System.Xml.Serialization.XmlIgnore()]
        public bool ForceInactivity
        {
            get
            {
                return Manufacturers.FindByIdentification(Manufacturer).ForceInactivity;
            }
        }

        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore()]
        GXDLMSObjectCollection m_Objects;

        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore()]
        public ProgressEventHandler OnProgress;

        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore()]
        public StatusEventHandler OnStatusChanged;

        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore()]
        public MessageTraceEventHandler OnTrace;

        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore()]
        System.Timers.Timer KeepAlive;

        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore()]
        GXDLMSCommunicator communicator;

        public void KeepAliveStart()
        {
            if (InactivityMode != InactivityMode.None)
            {
                KeepAlive.Interval = Manufacturers.FindByIdentification(this.Manufacturer).KeepAliveInterval;
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
        /// Password is used only if authentication is used.
        /// </summary>
        [DefaultValue("")]
        public string Password
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
        [System.Xml.Serialization.XmlIgnore()]
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
        [System.Xml.Serialization.XmlIgnore()]
        public IGXManufacturerExtension Extension
        {
            get;
            internal set;
        }

        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore()]
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
        [System.Xml.Serialization.XmlIgnore()]
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
        /// Save name of the manufacturer.
        /// </summary>
        [Browsable(false)]
        public bool UseIEC47
        {
            get
            {
                return Manufacturers.FindByIdentification(this.Manufacturer).UseIEC47;
            }
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
                    throw new Exception("Invalid media type " + value);
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
                UpdateStatus(DeviceState.Disconnecting);
                if (KeepAlive.Enabled)
                {
                    KeepAlive.Stop();
                }
                communicator.Disconnect();
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

        /// <summary>
        /// Find correct DLMS class by Interface Type from the assembly.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="it"></param>
        /// <returns></returns>
        public static GXDLMSObject ConvertObject2Class(GXDLMSDevice device, ObjectType objectType, string logicalName)
        {
            GXDLMSObject obj = Gurux.DLMS.GXDLMSClient.CreateObject(objectType);
            if (obj != null)
            {
                GXManufacturer m = device.Manufacturers.FindByIdentification(device.Manufacturer);
                GXObisCode item = m.ObisCodes.FindByLN(obj.ObjectType, logicalName, null);
                obj.LogicalName = logicalName;
                if (item != null)
                {
                    obj.Description = item.Description;
                }
            }
            return obj;
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
                Comm.parentForm.Invoke(new UpdateColumnsEventHandler(UpdateColumns), item, man);
                return;
            }
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
                /* TODO:
                if (!m.UseLogicalNameReferencing)
                {
                    GXLogWriter.WriteLog("--- Reading Access rights. ---");
                    try
                    {
                        foreach (GXDLMSAssociationShortName sn in dev.Objects.GetObjects(ObjectType.AssociationShortName))
                        {
                            dev.Comm.Read(sn, 3);
                        }
                    }
                    catch (Exception ex)
                    {
                        GXLogWriter.WriteLog(ex.Message);
                    }
                    GXLogWriter.WriteLog("--- Reading Access rights end. ---");
                }
                 * */
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
                if (this.InactivityMode == InactivityMode.Disconnect)
                {
                    this.Disconnect();
                }
                else if (this.InactivityMode == InactivityMode.KeepAlive)
                {
                    NotifyProgress(this, "Keep Alive", 0, 1);
                    communicator.KeepAlive();
                }
                else if (this.InactivityMode == InactivityMode.Reopen ||
                         this.InactivityMode == InactivityMode.ReopenActive)
                {
                    if (DateTime.Now.Subtract(communicator.connectionStartTime).TotalSeconds > 40)
                    {
                        Disconnect();
                        InitializeConnection();
                        communicator.connectionStartTime = DateTime.Now;
                    }
                }
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
