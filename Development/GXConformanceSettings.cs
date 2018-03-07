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

using Gurux.DLMS.UI;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

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
            Amount = 1;
            ImageVerifyWaitTime = new TimeSpan(0, 0, 10);
            ImageActivateWaitTime = new TimeSpan(0, 0, 10);
            DelayConnection = Delay = new TimeSpan(0, 0, 0);
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

        [XmlIgnore]
        [Description("Delay between tests.")]
        [DefaultValue(typeof(TimeSpan), "00:00:00")]
        public TimeSpan Delay
        {
            get;
            set;
        }

        [Browsable(false)]
        [DefaultValue("00:00:00")]
        public string DelayAsString
        {
            get
            {
                return XmlConvert.ToString(Delay);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Delay = XmlConvert.ToTimeSpan(value);
                }
            }
        }

        [Description("External tests")]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue(null)]
        [Category("Accessibility")]
        public string ExternalTests
        {
            get;
            set;
        }

        [Description("Invalid password.")]
        [Category("Connection")]
        [DefaultValue(null)]
        public string InvalidPassword
        {
            get;
            set;
        }

        [XmlIgnore]
        [Description("Delay between connections.")]
        [Category("Connection")]
        [DefaultValue(typeof(TimeSpan), "00:00:00")]
        public TimeSpan DelayConnection
        {
            get;
            set;
        }

        [Browsable(false)]
        [DefaultValue("00:00:00")]
        public string DelayConnectionAsString
        {
            get
            {
                return XmlConvert.ToString(DelayConnection);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    DelayConnection = XmlConvert.ToTimeSpan(value);
                }
            }
        }

        [Description("How many times the tests are executed.")]
        [Category("Accessibility")]
        [DefaultValue(1)]
        public UInt16 Amount
        {
            get;
            set;
        }

        private string imageFile;

        [Description("Updated image file.")]
        [Category("Image")]
        [DefaultValue(null)]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ImageFile
        {
            get
            {
                return imageFile;
            }
            set
            {
                if (imageFile != value && ImageIdentifier == null || ImageIdentifier.Length == 0)
                {
                    if (string.Compare(Path.GetExtension(value), ".xml", true) == 0)
                    {
                        try
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(value);
                            ImageIdentifier = GXImageDlg.GetIdentification(doc.ChildNodes);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            value = null;
                        }
                    }
                }
                imageFile = value;
            }
        }

        [Description("Image Identifier.")]
        [Category("Image")]
        [DefaultValue(null)]
        [TypeConverter(typeof(GXByteConverter))]
        [Editor(typeof(UITextTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public byte[] ImageIdentifier
        {
            get;
            set;
        }
        /*
        [Description("Send image in reversed order. This can be used to simulate that blocks are lost and they are re-sent.")]
        [Category("Image")]
        [DefaultValue(false)]
        public bool ImageReverse
        {
            get;
            set;
        }
        */

        [Description("Is image verified.")]
        [Category("Image")]
        [DefaultValue(false)]
        public bool ImageVerify
        {
            get;
            set;
        }

        [XmlIgnore]
        [Description("How long is waited before image is verified.")]
        [Category("Image")]
        [DefaultValue(typeof(TimeSpan), "00:00:10")]
        public TimeSpan ImageVerifyWaitTime
        {
            get;
            set;
        }

        [Browsable(false)]
        [DefaultValue("00:00:10")]
        public string ImageVerifyWaitTimeAsString
        {
            get
            {
                return XmlConvert.ToString(ImageVerifyWaitTime);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    ImageVerifyWaitTime = XmlConvert.ToTimeSpan(value);
                }
            }
        }


        [Description("Is image activated.")]
        [Category("Image")]
        [DefaultValue(false)]
        public bool ImageActivate
        {
            get;
            set;
        }

        [XmlIgnore]
        [Description("How long is waited before image is activated.")]
        [Category("Image")]
        [DefaultValue(typeof(TimeSpan), "00:00:10")]
        public TimeSpan ImageActivateWaitTime
        {
            get;
            set;
        }

        [Browsable(false)]
        [DefaultValue("00:00:10")]
        public string ImageActivateWaitTimeAsString
        {
            get
            {
                return XmlConvert.ToString(ImageActivateWaitTime);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    ImageActivateWaitTime = XmlConvert.ToTimeSpan(value);
                }
            }
        }
    }
}
