//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GXDLMSDirector/Development/GXDLMSDevice.cs $
//
// Version:         $Revision: 5903 $,
//                  $Date: 2013-01-08 15:00:16 +0200 (ti, 08 tammi 2013) $
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
            m_Objects = new GXDLMSObjectCollection();
            m_Objects.Parent = this;
            m_Communicator = new GXDLMSCommunicator(this, media);
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
                    byte[] data = (byte[])m_Communicator.DisconnectRequest();
                    m_Communicator.ReadDLMSPacket(data);
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

        DataColumn AddColumn(GXDLMSProfileGeneric item, GXDLMSObject row, DataType type, string text, int pos)
        {
            item.CaptureObjects.Add(row);
            DataColumn col = new DataColumn("O" + pos.ToString());
            col.Caption = text;
            //If event type or status.
            IGXDLMSColumnObject obj = item as IGXDLMSColumnObject;
            IGXDLMSColumnObject obj2 = row as IGXDLMSColumnObject;
            if (obj.SelectedAttributeIndex > 0 && ((obj.SelectedAttributeIndex & (0x8 | 0x10)) != 0x0))
            {                                     
                return col;
            }
            /* TODO: Is nesessry?
             if (obj2.SelectedAttributeIndex > 8) 
                {
                    //Convert value to 32 bit. This is done because event type in register can be UInt8, but event value in column is UInt32.
                    if (type == DataType.UInt8 || type == DataType.UInt16)
                    {
                        type = DataType.UInt32;
                    }
                    if (type == DataType.Int8 || type == DataType.Int16)
                    {
                        type = DataType.Int32;
                    }
                }       
             * //Read col's data type if unknown.
            if (type == DataType.None && item.ObjectType != ObjectType.ProfileGeneric)
            {
                GXDLMSObject tmp = this.FindObject(item.ObjectType, item.LogicalName);
                if (tmp != null)
                {
                    DataType t = DataType.None;
                    this.Comm.ReadValue(tmp.Name, item.ObjectType, obj.SelectedAttributeIndex, ref t);
                    tmp.SetDataType(obj.SelectedAttributeIndex, t);
                    type = t;                    
                }
            }
             */
            if (type != DataType.None)
            {
                Type tp = GXHelpers.FromDLMSDataType(type);
                if (tp != null)
                {
                    col.DataType = tp;
                }
            }            
            return col;
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
            item.Buffer.Columns.Clear();
            item.Buffer.Rows.Clear();
            item.CaptureObjects.Clear();
            int pos = 0;
            GXDLMSObjectCollection cols = null;
            List<DataColumn> columns = new List<DataColumn>();
            if (this.Extension != null)
            {                
                cols = this.Extension.Refresh(item, this.Comm);                
            }
            if (cols == null)
            {
                cols = Comm.GetProfileGenericColumns(item.Name);                
            }
            Comm.DeviceColumns[item] = cols;
            foreach (GXDLMSObject it in cols)
            {
                IGXDLMSColumnObject obj = it as IGXDLMSColumnObject;
                string text = it.Description;
                DataType type = DataType.None;
                if (it.ObjectType == ObjectType.ProfileGeneric)
                {
                    GXDLMSObjectCollection cols2 = null;
                    if (!Comm.DeviceColumns.ContainsKey(it))
                    {
                        if (this.Extension != null)
                        {
                            cols2 = this.Extension.Refresh(it as GXDLMSProfileGeneric, this.Comm);
                        }
                        if (cols2 == null)
                        {
                            cols2 = Comm.GetProfileGenericColumns(it);
                        }
                        Comm.DeviceColumns[it] = cols2;
                    }
                    else
                    {
                        cols2 = Comm.DeviceColumns[it];
                    }
                    int index = 0;
                    foreach (GXDLMSObject it2 in cols2)
                    {                        
                        if (obj.SelectedAttributeIndex == index)
                        {
                            IGXDLMSColumnObject obj2 = it2 as IGXDLMSColumnObject;
                            if (obj2.SelectedAttributeIndex == 0)
                            {
                                Array arr = it2.GetValues();
                                if (arr != null)
                                {
                                    //Skip Logical name.
                                    for (int a = 1; a != arr.Length; ++a)
                                    {
                                        if (!ShouldSkip(it2, a + 1))
                                        {
                                            type = DataType.None;
                                            GXDLMSObject obj3 = MakeColumn(man, it2, ref text, ref type, a + 1);
                                            obj2 = obj3 as IGXDLMSColumnObject;
                                            obj2.SourceLogicalName = it.LogicalName;
                                            obj2.SourceObjectType = it.ObjectType;
                                            columns.Add(AddColumn(item, obj3, type, text, pos++));
                                            obj2.SelectedAttributeIndex = index;
                                            obj2.SelectedDataIndex = a + 1;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                type = DataType.None;
                                GXDLMSObject obj3 = MakeColumn(man, it2, ref text, ref type, obj2.SelectedAttributeIndex);
                                obj3.SelectedAttributeIndex = index;
                                obj2 = obj3 as IGXDLMSColumnObject;
                                obj2.SourceLogicalName = it.LogicalName;
                                obj2.SourceObjectType = it.ObjectType;
                                columns.Add(AddColumn(item, obj3, type, text, pos++));                                
                            }
                            break;
                            
                        }
                        else
                        {
                            ++index;
                        }
                    }
                }
                else
                {
                    if (obj.SelectedAttributeIndex == 0)
                    {
                        Array arr = it.GetValues();
                        if (arr != null)
                        {
                            for (int a = 1; a != arr.Length; ++a)
                            {
                                if (!ShouldSkip(it, a + 1))
                                {
                                    type = DataType.None;
                                    GXDLMSObject obj3 = MakeColumn(man, it, ref text, ref type, a + 1);
                                    obj = obj3 as IGXDLMSColumnObject;
                                    obj.SourceLogicalName = it.LogicalName;
                                    obj.SourceObjectType = it.ObjectType;
                                    columns.Add(AddColumn(item, obj3, type, text, pos++));
                                    obj.SelectedDataIndex = a + 1;
                                }
                            }
                        }
                    }
                    else
                    {                        
                        columns.Add(AddColumn(item, MakeColumn(man, it, ref text, ref type, obj.SelectedAttributeIndex), type, text, pos++));
                    }
                }
            }
            item.Buffer.Columns.AddRange(columns.ToArray());
        }

        private GXDLMSObject MakeColumn(GXManufacturer man, GXDLMSObject it, ref string text, ref DataType type, int index)
        {
            GXObisCode code = man.ObisCodes.FindByLN(it.ObjectType, it.LogicalName, null);
            GXDLMSObject obj = m_Objects.FindByLN(it.ObjectType, it.LogicalName);
            if (obj != null)
            {
                obj = obj.Clone();
                IGXDLMSColumnObject tmp = obj as IGXDLMSColumnObject;
                IGXDLMSColumnObject tmp2 = it as IGXDLMSColumnObject;
                tmp.SelectedAttributeIndex = tmp2.SelectedAttributeIndex;
                tmp.SelectedDataIndex = tmp2.SelectedDataIndex;
                type = obj.GetUIDataType(index);
            }
            else
            {
                obj = it;
                if (code != null && type == DataType.None)
                {
                    IGXDLMSColumnObject tmp = it as IGXDLMSColumnObject;
                    GXDLMSAttributeSettings att = code.Attributes.Find(tmp.SelectedAttributeIndex);
                    if (att != null)
                    {
                        type = att.Type;
                    }
                }
                else
                {
                    type = obj.GetUIDataType(index);
                }
            }            
            if (code == null)
            {
                text = it.LogicalName + " " + it.Description;
            }
            else
            {
                text = it.LogicalName + " " + code.Description;                
            }
            if (string.IsNullOrEmpty(text))
            {
                text = it.LogicalName;
            }
            return obj;
        }

        /// <summary>
        /// After UpdateObjects call objects can be read using Objects property.
        /// </summary>
        public void UpdateObjects()
        {
            try
            {
                GXDLMSObjectCollection objs = Comm.GetObjects();
                int pos = 0;
                GXObisCode code;
                foreach (GXDLMSObject it in objs)
                {                    
                    if ((code = this.ObisCodes.FindByLN(it.ObjectType, it.LogicalName, null)) == null)
                    {
                        code = new GXObisCode(it.LogicalName, it.ObjectType, "");                            
                        this.ObisCodes.Add(code);
                    }               
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
                    //Read OBIS data type if it is unknown.
                    if (it.GetDataType(2) == DataType.None && (it is GXDLMSData || it is GXDLMSRegister))
                    {
                        DataType t = DataType.None;
                        try
                        {
                            this.Comm.ReadValue(it.ToString(), it.ObjectType, 2, ref t);
                            it.SetDataType(2, t);
                            GXDLMSAttributeSettings att = code.Attributes.Find(2);
                            if (att == null)
                            {
                                att = new GXDLMSAttributeSettings(2);
                                code.Attributes.Add(att);
                            }
                            att.Type = t; //TODO: tämä voidaan poistaa myohemmin...
                            m_Objects.Add(it);
                        }
                        catch (GXDLMSException Ex)
                        {
                            //If HW error.
                            if (Ex.ErrorCode == 1)
                            {
                                continue;
                            }
                            //If read is denied.
                            else if (Ex.ErrorCode == 3)
                            {
                                it.SetAccess(2, AccessMode.NoAccess);
                            }
                            else
                            {
                                GXDLMS.Common.Error.ShowError(null, Ex);
                            }
                        }
                        catch (Exception Ex)
                        {
                            GXDLMS.Common.Error.ShowError(null, Ex);
                            break;
                        }
                    }
                    else
                    {
                        m_Objects.Add(it);
                    }
                }
                GXLogWriter.WriteLog("--- Created " + m_Objects.Count.ToString() + " objects. ---");
                int objPos = 0;
                foreach (Gurux.DLMS.Objects.GXDLMSProfileGeneric it in objs.GetObjects(ObjectType.ProfileGeneric))
                {                    
                    ++pos;
                    NotifyProgress(this, "Creating object " + it.LogicalName, pos, objs.Count);
                    it.Buffer.TableName = it.LogicalName + " " + it.Description;
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
