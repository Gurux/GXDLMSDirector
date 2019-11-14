//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
//
// Version:         $Revision: 10624 $,
//                  $Date: 2019-04-24 13:56:09 +0300 (ke, 24 huhti 2019) $
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
using System.IO;
using System.Diagnostics;
using Gurux.Common;
using Gurux.DLMS;
using Gurux.DLMS.Enums;

namespace GXDLMS.ManufacturerSettings
{
    class GXLogWriter
    {
        /// <summary>
        /// Received trace data.
        /// </summary>
        private static GXByteBuffer receivedTraceData = new GXByteBuffer();
        private static GXDLMSTranslator translator;

        static GXLogWriter()
        {
            translator = new GXDLMSTranslator(TranslatorOutputType.SimpleXml);
            receivedTraceData = new GXByteBuffer();
        }

        static public string LogPath
        {
            get
            {
                string path = string.Empty;
                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    path = System.IO.Path.Combine(path, ".Gurux");
                }
                else
                {
                    //Vista: C:\ProgramData
                    //XP: c:\Program Files\Common Files                
                    //XP = 5.1 & Vista = 6.0
                    if (Environment.OSVersion.Version.Major >= 6)
                    {
                        path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                    }
                    else
                    {
                        path = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
                    }
                    path = System.IO.Path.Combine(path, "Gurux");
                }
                path = System.IO.Path.Combine(path, "GXDLMSDirector");
                path = System.IO.Path.Combine(path, "GXDLMSDirector.log");
                return path;
            }
        }
       
        static public int LogLevel
        {
            get;
            set;
        }

        static public bool Comments
        {
            get
            {
                return translator.Comments;
            }
            set
            {
                translator.Comments = value;
            }
        }

        /// <summary>
        /// Append data to log file.
        /// </summary>
        static public void WriteLog(string data)
        {
            WriteLog(data, GXDLMSDirector.Properties.Settings.Default.LogTime);
        }

        /// <summary>
        /// Append data to log file.
        /// </summary>
        static public void WriteLog(string data, bool addTime)
        {
            if (data == null)
            {
                return;
            }
            if (addTime)
            {
                System.Diagnostics.Trace.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + data.Replace("\r", "<CR>").Replace("\n", "<LF>"));
            }
            else
            {
                System.Diagnostics.Trace.WriteLine(data.Replace("\r", "<CR>").Replace("\n", "<LF>"));
            }
        }

        static public void WriteLog(string text, byte[] value)
        {
            string str;
            if (GXDLMSDirector.Properties.Settings.Default.LogTime)
            {
                str = DateTime.Now.ToLongTimeString() + " " + text;
            }
            else
            {
                str = text;
            }
            //Show data as hex.
            if ((LogLevel & 1) != 0)
            {
                if (value != null)
                {
                    str += "\r\n" + GXCommon.ToHex(value, true);
                }
                System.Diagnostics.Trace.WriteLine(str);
            }
            //Show data as xml.
            if (value != null && (LogLevel & 2) != 0)
            {
                receivedTraceData.Set(value);
                try
                {
                    GXByteBuffer pdu = new GXByteBuffer();
                    InterfaceType type = GXDLMSTranslator.GetDlmsFraming(receivedTraceData);
                    while (translator.FindNextFrame(receivedTraceData, pdu, type))
                    {
                        System.Diagnostics.Trace.WriteLine(translator.MessageToXml(receivedTraceData));
                        receivedTraceData.Trim();
                    }
                }
                catch (Exception)
                {
                    receivedTraceData.Clear();
                }
            }
        }

        /// <summary>
        /// Clear log file.
        /// </summary>
        static public void ClearLog()
        {
            foreach (TraceListener it in System.Diagnostics.Trace.Listeners)
            {
                if (it is TextWriterTraceListener)
                {
                    //Flush and close the output.
                    Trace.Flush();
                    it.Flush();
                    if (((TextWriterTraceListener)it).Writer != null)
                    {
                        ((TextWriterTraceListener)it).Writer.Close();
                    }
                    ((TextWriterTraceListener)it).Writer = new StreamWriter(GXLogWriter.LogPath);
                    GXFileSystemSecurity.UpdateFileSecurity(GXLogWriter.LogPath);
                    break;
                }
            }
            //Get version info
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo info = System.Diagnostics.FileVersionInfo.GetVersionInfo(asm.Location);
            Debug.WriteLine("GXDLMSDirector " + info.FileVersion);
            Debug.WriteLine("Log created " + DateTime.Now.ToLongTimeString());
        }
    }
}
