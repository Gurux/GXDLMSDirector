//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GuruxClub/GXDLMSDirector/Development/GXDLMSDevice.cs $
//
// Version:         $Revision: 7706 $,
//                  $Date: 2014-12-04 12:50:37 +0200 (to, 04 joulu 2014) $
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

        /// <summary>
        /// This method is used to solve Column's data type in Profile Generic table.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="attributeIndex"></param>
        /// <returns></returns>
        internal static DataType GetAttributeType(GXDLMSObject component, int attributeIndex)
        {
            if (attributeIndex != 0)
            {
                if (attributeIndex > 0x10)
                {
                    attributeIndex = 2;
                }
                GXDLMSAttributeSettings att2 = component.Attributes.Find(attributeIndex);
                if (att2 != null && att2.Type != DataType.None)
                {
                    return att2.Type;
                }
                PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(component);
                foreach (PropertyDescriptor pd in pdc)
                {
                    GXDLMSAttributeSettings att = (GXDLMSAttributeSettings)pd.Attributes[typeof(GXDLMSAttributeSettings)];
                    if (att != null)
                    {
                        if (att.Index == attributeIndex)
                        {
                            if (att.UIType != DataType.None)
                            {
                                return att.UIType;
                            }
                            //If expected type is not given return property type.
                            if (pd.PropertyType == typeof(Int32))
                            {
                                return DataType.Int32;
                            }
                            if (pd.PropertyType == typeof(UInt32))
                            {
                                return DataType.UInt32;
                            }
                            if (pd.PropertyType == typeof(String))
                            {
                                return DataType.String;
                            }
                            if (pd.PropertyType == typeof(byte))
                            {
                                return DataType.UInt8;
                            }
                            if (pd.PropertyType == typeof(sbyte))
                            {
                                return DataType.Int8;
                            }
                            if (pd.PropertyType == typeof(Int16))
                            {
                                return DataType.Int16;
                            }
                            if (pd.PropertyType == typeof(UInt16))
                            {
                                return DataType.UInt16;
                            }
                            if (pd.PropertyType == typeof(Int64))
                            {
                                return DataType.Int64;
                            }
                            if (pd.PropertyType == typeof(UInt64))
                            {
                                return DataType.UInt64;
                            }
                            if (pd.PropertyType == typeof(float))
                            {
                                return DataType.Float32;
                            }
                            if (pd.PropertyType == typeof(double))
                            {
                                return DataType.Float64;
                            }
                            if (pd.PropertyType == typeof(DateTime))
                            {
                                return DataType.DateTime;
                            }
                            if (pd.PropertyType == typeof(Boolean) || pd.PropertyType == typeof(bool))
                            {
                                return DataType.Boolean;
                            }
                            if (pd.PropertyType == typeof(object))
                            {
                                return DataType.None;
                            }
                        }
                    }
                }
            }
            return DataType.None;
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
        System.Timers.Timer KeepAlive;

        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore()]
        GXDLMSCommunicator m_Communicator;

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
        /// USed logical client ID.
        /// </summary>
        /// <remarks>
        /// Client ID is always 1 byte long.
        /// </remarks>
        [DefaultValue(0x10)]
        public object ClientID
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

        public void InitializeConnection()
        {
            try
            {
                UpdateStatus(DeviceState.Connecting);
                m_Communicator.InitializeConnection();
                UpdateStatus(DeviceState.Connected);
            }
            catch (Exception Ex)
            {
                UpdateStatus(DeviceState.Initialized);
                if (Media != null)
                {
                    Media.Close();
                }
                throw Ex;
            }
        }

        public GXDLMSCommunicator Comm
        {
            get
            {
                return m_Communicator;
            }
        }

        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore()]
        public Gurux.DLMS.ManufacturerSettings.GXObisCodeCollection ObisCodes
        {
            get
            {
                return m_Communicator.m_Cosem.ObisCodes;
            }
            set
            {
                m_Communicator.m_Cosem.ObisCodes = value;
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
                return m_Communicator.Media;
            }
            set
            {
                m_Communicator.Media = value;
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
        /// Save name of the manufacturer.
        /// </summary>
        [Browsable(false)]
        public string Manufacturer
        {
            get;
            set;
        }

        /// <summary>
        /// What HSLC Addressing is used.
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
            ClientID = 0x10; // Public client (lowest security level).
            PhysicalAddress = 1;
            Password = "";
            Authentication = Authentication.None;
            m_Communicator = new GXDLMSCommunicator(this, media);
            m_Objects = m_Communicator.m_Cosem.Objects;
            m_Objects.Tag = this;
            m_Communicator.OnProgress += new ProgressEventHandler(this.NotifyProgress);
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
                return m_Communicator.Media.GetType().ToString();
            }
            set
            {
                Type type = Type.GetType(value);
                if (type == null)
                {
                    throw new Exception("Invalid media type " + value);
                }
                m_Communicator.Media = (Gurux.Common.IGXMedia)Activator.CreateInstance(type);
            }
        }        

        /// <summary>
        /// Media settings as a string.
        /// </summary>
        public string MediaSettings
        {
            get
            {
                return m_Communicator.Media.Settings;
            }
            set
            {               
                m_Communicator.Media.Settings = value;         
            }
        }

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
                if (m_Communicator.Media.IsOpen)
                {
                    m_Communicator.ReadDLMSPacket(m_Communicator.DisconnectRequest(), 1);
                }
            }
            catch (Exception Ex)
            {
                //Do not show error occurred in disconnect. Write error only to the log file.
                GXLogWriter.WriteLog(Ex.Message);
            }
            finally
            {
                if (m_Communicator.Media != null)
                {
                    m_Communicator.Media.Close();
                }
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
        public static GXDLMSObject ConvertObject2Class(GXDLMSDevice device, Gurux.DLMS.ObjectType objectType, string logicalName)
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
            if (Comm.ParentForm.InvokeRequired)
            {
                Comm.ParentForm.Invoke(new UpdateColumnsEventHandler(UpdateColumns), item, man);
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
                cols = Comm.GetProfileGenericColumns(item.Name);                
            }
            item.CaptureObjects = cols;
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
                    //Profile Generic objects are added later.
                    if (it.ObjectType == ObjectType.ProfileGeneric)
                    {
                        continue;
                    }
                    if (it.GetType() == typeof(GXDLMSObject))
                    {
                        continue;
                    }
                    ++pos;
                    NotifyProgress(this, "Creating object " + it.LogicalName, pos, objs.Count);
                    m_Objects.Add(it);                    
                }
                GXLogWriter.WriteLog("--- Created " + m_Objects.Count.ToString() + " objects. ---");
                int objPos = 0;

                //Read registers units and scalers.
                int cnt = Objects.Count;
                GXLogWriter.WriteLog("--- Reading scalers and units. ---");
                for (pos = 0; pos != cnt; ++pos)
                {
                    GXDLMSObject it = Objects[pos];
                    it.UpdateDefaultValueItems();
                    this.OnProgress(this, "Reading scalers and units.", pos + 1, cnt);
                    if (it is GXDLMSRegister)
                    {
                        object data = it.ShortName;
                        if (it.ShortName == 0)
                        {
                            data = it.LogicalName;
                        }
                        //Read scaler first.
                        DataType type = DataType.None;
                        try
                        {
                            data = Comm.ReadValue(data, it.ObjectType, 3, ref type);
                            object tmp = GXHelpers.ConvertFromDLMS(data, DataType.None, DataType.None, false);
                            //Actaris ACE 6000 is returning wrong value here.
                            if (tmp is object[])
                            {
                                object[] scalerUnit = (object[])tmp;
                                ((GXDLMSRegister)it).Scaler = Math.Pow(10, Convert.ToInt32(scalerUnit.GetValue(0)));
                                ((GXDLMSRegister)it).Unit = (Unit)Convert.ToInt32(scalerUnit.GetValue(1));
                            }
                        }
                        catch (Exception ex)
                        {
                            GXLogWriter.WriteLog(ex.Message);                            
                        }
                    }
                    if (it is GXDLMSDemandRegister)
                    {
                        object name = it.ShortName;
                        object data;
                        if (it.ShortName == 0)
                        {
                            name = it.LogicalName;
                        }
                        //Read scaler first.
                        DataType type = DataType.None;
                        byte attributeOrder = 4;
                        try
                        {
                            data = Comm.ReadValue(name, it.ObjectType, attributeOrder, ref type);
                            Array scalerUnit = (Array)GXHelpers.ConvertFromDLMS(data, DataType.None, DataType.None, false);
                            ((GXDLMSDemandRegister)it).Scaler = Math.Pow(10, Convert.ToInt32(scalerUnit.GetValue(0)));
                            ((GXDLMSDemandRegister)it).Unit = (Unit)Convert.ToInt32(scalerUnit.GetValue(1));
                            //Read Period
                            data = Comm.ReadValue(name, it.ObjectType, 8, ref type);
                            ((GXDLMSDemandRegister)it).Period = Convert.ToUInt64(data);
                            //Read number of periods
                            data = Comm.ReadValue(name, it.ObjectType, 9, ref type);
                            ((GXDLMSDemandRegister)it).NumberOfPeriods = Convert.ToUInt32(data);
                        }
                        catch (Exception ex)
                        {
                            GXLogWriter.WriteLog(ex.Message);                            
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
                this.OnProgress(this, "Reading scalers and units.", cnt, cnt);
                foreach (Gurux.DLMS.Objects.GXDLMSProfileGeneric it in objs.GetObjects(ObjectType.ProfileGeneric))
                {                    
                    ++pos;
                    NotifyProgress(this, "Creating object " + it.LogicalName, pos, objs.Count);
                    //Read Profile Generic Columns.                
                    try
                    {
                        NotifyProgress(this, "Get profile generic columns", ++objPos, objs.Count);                    
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
                    m_Objects.Add(it);
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
                    m_Communicator.KeepAlive();
                }
                else if (this.InactivityMode == InactivityMode.Reopen || 
                    this.InactivityMode == InactivityMode.ReopenActive)
                {
                    if (DateTime.Now.Subtract(m_Communicator.ConnectionStartTime).TotalSeconds > 40)
                    {
                        Disconnect();
                        InitializeConnection();
                        m_Communicator.ConnectionStartTime = DateTime.Now;
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
