//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL$
//
// Version:         $Revision$,
//                  $Date$
//                  $Author$
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
// This code is licensed under the GNU General Public License v2. 
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using GXDLMS.ManufacturerSettings;
using Gurux.DLMS;
using GXDLMSDirector;
using GXDLMS.Common;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Objects;
using System.Data;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Objects.Enums;

namespace Extensions
{
    public class Actaris : IGXManufacturerExtension
    {
        GXDLMSProfileGeneric CurrentProfileGeneric;
        System.Windows.Forms.Form MainForm;

        #region IGXManufacturerExtension Members       

        public List<GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>> Refresh(GXDLMSProfileGeneric item, GXDLMSCommunicator comm)
        {
            GXReplyData reply = new GXReplyData();
            if (item.LogicalName.CompareTo("0.0.99.1.2.255") == 0) // LoadProfile1EndOfRecordingData
            {
                List<GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>> items = new List<GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>>();
                //Read profile generic columns.             
                comm.GetProfileGenericColumns(item);
                byte[] data = comm.Read(item, 2);
                comm.ReadDataBlock(data, "Get profile generic columns...", 1, reply);
                object[] values = (object[])reply.Value;
                Array info = values[0] as Array;
                GXDLMSObject obj = new GXDLMSData();
                obj.Description = "DateTime";                
                obj.SetUIDataType(1, DataType.DateTime);
                items.Add(new GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>(obj, new GXDLMSCaptureObject(1, 0)));
                obj = new GXDLMSData();
                obj.Description = "Status";
                items.Add(new GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>(obj, new GXDLMSCaptureObject(2, 0)));
                //Two last items contains Start and end date.
                int cnt = 4;
                for (int pos = 0; pos < info.Length - 2; pos += 2)
                {
                    obj = new GXDLMSData();
                    obj.LogicalName = Convert.ToString(GXDLMSClient.ChangeType((byte[])info.GetValue(pos), DataType.OctetString));
                    object scalerUnit = info.GetValue(pos + 1);
                    obj.Description = "";
                    items.Add(new GXKeyValuePair<GXDLMSObject, GXDLMSCaptureObject>(obj, new GXDLMSCaptureObject(++cnt, 0)));
                }
                LastDateTime = (GXDateTime)GXDLMSClient.ChangeType((byte[]) info.GetValue(info.Length - 1), DataType.DateTime);
                return items;
            }             
            return null;
        }

        void OnProfileGenericDataReceived(object sender, GXReplyData reply)
        {
            if (MainForm.InvokeRequired)
            {
                MainForm.Invoke(new GXDLMSCommunicator.DataReceivedEventHandler(this.OnProfileGenericDataReceived), new object[] { sender, reply });
                return;
            }
            /*
            if (CurrentProfileGeneric != null)
            {               
                GXDLMSCommunicator comm = sender as GXDLMSCommunicator;
                Array value = (Array)comm.m_Cosem.TryGetValue(buff);
                if (value != null && value.Length != 0)
                {
                    object[] rows = (object[])value;
                    bool LoadProfile1 = CurrentProfileGeneric.LogicalName == "0.0.99.1.0.255";
                    bool LoadProfile2 = CurrentProfileGeneric.LogicalName == "0.0.99.1.2.255";
                    foreach (object[] row in rows)
                    {                        
                        if (row != null)
                        {
                            bool skipRow = false;
                            DataRow dr = CurrentProfileGeneric.Buffer.NewRow();
                            object data = null;
                            int index = 0;
                            for (int pos = 0; pos < CurrentProfileGeneric.CaptureObjects.Count; ++pos)
                            {
                                DataType type = DataType.None;
                                DataType uiType = DataType.None;
                                GXDLMSObject target = CurrentProfileGeneric.CaptureObjects[pos];
                                IGXDLMSColumnObject target2 = target as IGXDLMSColumnObject;
                                string name = null;
                                ObjectType oType;
                                int aIndex, dIndex;
                                bool bParent = !string.IsNullOrEmpty(target2.SourceLogicalName);
                                if (bParent)
                                {
                                    name = target2.SourceLogicalName;
                                    oType = target2.SourceObjectType;
                                }
                                else
                                {
                                    name = target.LogicalName;
                                    oType = target.ObjectType;
                                }
                                aIndex = target2.SelectedAttributeIndex;
                                dIndex = target2.SelectedDataIndex;
                                data = null;
                                foreach(GXDLMSObject c in CurrentProfileGeneric.CaptureObjects)
                                {
                                    IGXDLMSColumnObject c2 = c as IGXDLMSColumnObject;
                                    if (c.ObjectType == oType && c.LogicalName == name)
                                    {
                                        if (aIndex == c.SelectedAttributeIndex)
                                        {
                                            //Load profile is a special case.
                                            if (LoadProfile1 && (index < 4) && row[index] != null)
                                            {
                                                data = ((Array)row[index]).GetValue(0);
                                                data = GXDLMS.Common.GXHelpers.ConvertFromDLMS(data, DataType.OctetString, DataType.DateTime, true);
                                            }
                                            else if (LoadProfile2)
                                            {
                                                if (aIndex == 1 || aIndex == 2)
                                                {
                                                    data = ((Array)row[0]).GetValue(0);
                                                }
                                                else
                                                {
                                                    data = ((Array)row[0]).GetValue(aIndex - 1);
                                                }
                                                if (aIndex < 3)
                                                {
                                                    if (data != null)
                                                    {
                                                        data = ((Array)data).GetValue(index);
                                                        if (index == 0)
                                                        {
                                                            data = GXDLMS.Common.GXHelpers.ConvertFromDLMS(data, DataType.OctetString, DataType.DateTime, true);
                                                            LastDateTime = LastDateTime.AddMinutes(25);
                                                        }
                                                    }
                                                    else if (index == 0)
                                                    {
                                                        skipRow = true;
                                                            break;
                                                        //LastDateTime = LastDateTime.AddMinutes(25);
                                                        //data = LastDateTime;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (bParent && aIndex != 0 && row[index] is object[])
                                                {
                                                    data = ((Array)((Array)row[index]).GetValue(0)).GetValue(c2.SelectedAttributeIndex);
                                                }
                                                else
                                                {
                                                    data = row[index];
                                                }

                                                if (dIndex != 0)
                                                {
                                                    data = ((Array)data).GetValue(dIndex - 1);
                                                    uiType = target.GetUIDataType(dIndex);
                                                    type = target.GetDataType(dIndex);
                                                    if (uiType == DataType.None)
                                                    {
                                                        uiType = type;
                                                    }
                                                }
                                                else
                                                {
                                                    uiType = target.GetUIDataType(c2.SelectedAttributeIndex);
                                                    type = target.GetDataType(c2.SelectedAttributeIndex);
                                                    if (uiType == DataType.None)
                                                    {
                                                        uiType = type;
                                                    }
                                                }
                                            }
                                            if (!bParent)
                                            {
                                                ++index;
                                            }
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        ++index;
                                    }
                                }
                                if (skipRow)
                                {
                                    break;
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
                                        dr[pos] = GXDLMS.Common.GXHelpers.ConvertFromDLMS(data, type, uiType, true);
                                    }
                                }
                            }
                            if (!skipRow)
                            {
                                CurrentProfileGeneric.Buffer.Rows.InsertAt(dr, CurrentProfileGeneric.Buffer.Rows.Count);
                            }
                        }                       
                    }
                }            
            }    
              * * */  
        }

        /// <summary>
        /// Returns collection of manufacturer Obis codes to implement custom read.
        /// </summary>
        /// <param name="name">Short or Logical Name.</param>
        /// <param name="type">Interface type.</param>        
        /// <returns>True, if data read is handled.</returns>
        public bool Read(object sender, GXDLMSObject item, GXDLMSObjectCollection columns, int attribute, GXDLMSCommunicator comm)
        {
            MainForm = sender as System.Windows.Forms.Form;
            if (!(item is GXDLMSProfileGeneric))
            {
                return false;
            }
            //Actaris do not support other than index 2.
            if (attribute != 0 && attribute != 2)
            {
                return true;
            }
            if (comm.OnBeforeRead != null)
            {
                comm.OnBeforeRead(item, attribute);
            }
            CurrentProfileGeneric = item as GXDLMSProfileGeneric;
            if (item is GXDLMSProfileGeneric)
            {
                GXDLMSProfileGeneric pg = item as GXDLMSProfileGeneric;
                GXReplyData reply = new GXReplyData();
                byte[][] data;
                try
                {
                    comm.OnDataReceived += new GXDLMSCommunicator.DataReceivedEventHandler(this.OnProfileGenericDataReceived);
                    //Read profile generic columns.                
                    if (pg.AccessSelector == AccessRange.Range ||
                        pg.AccessSelector == AccessRange.Last)
                    {
                        data = comm.client.ReadRowsByRange(pg, Convert.ToDateTime(pg.From).Date, Convert.ToDateTime(pg.To).Date);
                        comm.ReadDataBlock(data[0], "Reading profile generic data", 1, reply);
                    }
                    else if (pg.AccessSelector == AccessRange.Entry)
                    {
                        data = comm.client.ReadRowsByEntry(pg, Convert.ToInt32(pg.From), Convert.ToInt32(pg.To));
                        comm.ReadDataBlock(data[0], "Reading profile generic data " + pg.Name, 1, reply);
                    }
                    else //Read All.
                    {
                        data = comm.client.Read(pg, 2);
                        comm.ReadDataBlock(data[0], "Reading profile generic data " + pg.Name, 1, reply);
                    }
                }
                finally
                {
                    CurrentProfileGeneric = null;
                    comm.OnDataReceived -= new GXDLMSCommunicator.DataReceivedEventHandler(this.OnProfileGenericDataReceived);
                }
                return true;
            }
            return false;
        }

        DateTime LastDateTime;
        #endregion
    }
}
