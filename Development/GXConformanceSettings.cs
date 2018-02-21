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
// More information of Gurux DLMS/COSEM Director: http://www.gurux.org/GXDLMSDirector
//
// This code is licensed under the GNU General Public License v2. 
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------

using System.ComponentModel;

namespace GXDLMSDirector
{
    /// <summary>
    /// Conformance properties.
    /// </summary>
    public class GXConformanceSettings
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXConformanceSettings()
        {
            ShowValues = true;
        }

        [Description("Are meters reading concurrently.")]
        [DefaultValue(false)]
        [Category("Behavior")]
        public bool ConcurrentTesting
        {
            get;
            set;
        }

        [Description("Are values show on report.")]
        [DefaultValue(true)]
        [Category("Appereance")]
        public bool ShowValues
        {
            get;
            set;
        }

        [Description("Re-read Association View.")]
        [DefaultValue(false)]
        [Category("Behavior")]
        public bool ReReadAssociationView
        {
            get;
            set;
        }

        [Description("Is write tested.")]
        [DefaultValue(false)]
        [Category("Behavior")]
        public bool Write
        {
            get;
            set;
        }

        [Description("Exclude Basic Tests.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ExcludeBasicTests
        {
            get;
            set;

        }

        [Description("Delay between tests in seconds.")]
        [DefaultValue(0)]
        [Category("Accessibility")]
        public uint Delay
        {
            get;
            set;

        }

        [Description("External tests")]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("Accessibility")]
        public string ExternalTests
        {
            get;
            set;
        }

        [Description("Invalid password.")]
        [Category("Connection")]
        [DefaultValue("")]
        public string InvalidPassword
        {
            get;
            set;
        }

        [Description("Delay between connections.")]
        [Category("Connection")]
        [DefaultValue(0)]
        public int DelayConnection
        {
            get;
            set;
        }     
    }
}
