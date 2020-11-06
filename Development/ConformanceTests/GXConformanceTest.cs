//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
//
// Version:         $Revision: 9442 $,
//                  $Date: 2017-05-23 15:21:03 +0300 (ti, 23 touko 2017) $
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

using Gurux.DLMS.Objects;
using System;
using System.Threading;

namespace GXDLMSDirector
{
    public delegate void ConformanceReadyEvent(GXConformanceTest r);

    public delegate void ConformanceErrorEvent(GXConformanceTest r, Exception e);

    public delegate void ConformanceTraceEvent(GXConformanceTest sender, string data);

    public class GXConformanceTest
    {
        /// <summary>
        /// Tested device.
        /// </summary>
        public GXDLMSDevice Device
        {
            get;
            set;
        }

        /// <summary>
        /// Name of results folder.
        /// </summary>
        public string Results
        {
            get;
            set;
        }

        /// <summary>
        /// Occurred exception.
        /// </summary>
        public Exception Exception
        {
            get;
            set;
        }

        /// <summary>
        /// Return error level.
        /// </summary>
        public int ErrorLevel
        {
            get;
            set;
        }

        /// <summary>
        /// Show trace.
        /// </summary>
        public ConformanceTraceEvent OnTrace
        {
            get;
            set;
        }

        public ProgressEventHandler OnProgress
        {
            get;
            set;
        }

        /// <summary>
        /// Occurred exception.
        /// </summary>
        public ConformanceErrorEvent OnError
        {
            get;
            set;
        }
        /// <summary>
        /// Occurred exception.
        /// </summary>
        public ConformanceReadyEvent OnReady
        {
            get;
            set;
        }

        /// <summary>
        /// Read values from the meter.
        /// </summary>
        public GXDLMSObjectCollection Values
        {
            get;
            set;
        }
    }
}
