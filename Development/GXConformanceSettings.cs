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

using Gurux.DLMS.UI;
using System;
using System.ComponentModel;
using System.Drawing.Design;
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
        private string imageFile;
        GXConformanceHdlcTests excludedHdlcTests;
        GXConformanceApplicationTests excludedApplicationTests;

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
            WarningBeforeStart = true;
            excludedApplicationTests = new GXConformanceApplicationTests();
            excludedHdlcTests = new GXConformanceHdlcTests();
            ResendCount = -1;
        }

        [Description("Resend count. If resend count is -1 meter's resend count is used.")]
        [DefaultValue(-1)]
        [Category("Behavior")]
        public int ResendCount
        {
            get;
            set;
        }

        [XmlIgnore]
        [Description("Wait time. If waittime is zero meter's wait time is used.")]
        [DefaultValue(typeof(TimeSpan), "00:00:00")]
        [Category("Behavior")]
        public TimeSpan WaitTime
        {
            get;
            set;
        }

        [Browsable(false)]
        [DefaultValue("00:00:00")]
        public string WaitTimeAsString
        {
            get
            {
                return XmlConvert.ToString(Delay);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    WaitTime = XmlConvert.ToTimeSpan(value);
                }
                else
                {
                    WaitTime = new TimeSpan(0, 0, 0);
                }
            }
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
                else
                {
                    Delay = new TimeSpan(0, 0, 0);
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
                else
                {
                    DelayConnection = new TimeSpan(0, 0, 0);
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
                if (value != null && (imageFile != value && ImageIdentifier == null || ImageIdentifier.Length == 0))
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
                else
                {
                    ImageVerifyWaitTime = new TimeSpan(0, 0, 10);
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
        [Description("How long is waited before image is activated. Image activation status is not checked if value is 00:00:00")]
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
                else
                {
                    ImageActivateWaitTime = new TimeSpan(0, 0, 10);
                }
            }
        }

        [Description("Excluded HDLC framing tests. HDLC tests are executed only when HDLC framing is used.")]
        [Category("Accessibility")]
        [TypeConverter(typeof(GXConformanceValueConverter))]
        [DefaultValue(null)]
        [Editor(typeof(GXConformanceEditor), typeof(UITypeEditor))]
        [RefreshPropertiesAttribute(RefreshProperties.All)]
        public GXConformanceHdlcTests ExcludedHdlcTests
        {
            get
            {
                return excludedHdlcTests;
            }
            set
            {
                //If user reset values.
                if (value == null)
                {
                    value = new GXConformanceHdlcTests();
                }
                excludedHdlcTests = value;
            }
        }

        /// <summary>
        /// Old functionality. This is removed later.
        /// </summary>
        [DefaultValue(false)]
        [Browsable(false)]
        public bool ExcludeApplicationTests
        {
            get
            {
                return false;
            }
            set
            {
                if (value)
                {
                    ExcludedApplicationTests.Set(true);
                }
            }
        }

        [Description("Exclude COSEM application layer tests.")]
        [Category("Accessibility")]
        [TypeConverter(typeof(GXConformanceValueConverter))]
        [DefaultValue(null)]
        [Editor(typeof(GXConformanceEditor), typeof(UITypeEditor))]
        [RefreshPropertiesAttribute(RefreshProperties.All)]
        public GXConformanceApplicationTests ExcludedApplicationTests
        {
            get
            {
                return excludedApplicationTests;
            }
            set
            {
                //If user reset values.
                if (value == null)
                {
                    value = new GXConformanceApplicationTests();
                }
                excludedApplicationTests = value;
            }
        }

        [Description("Meter information is not read. Logical Device Name, Firmware version and time are not read from the meter.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ExcludeMeterInfo
        {
            get;
            set;
        }

        [Description("Warning is shown before tests are actual started.")]
        [DefaultValue(true)]
        [Category("Accessibility")]
        public bool WarningBeforeStart
        {
            get;
            set;
        }

        string readLastDays;

        [Description("Override Profile Generic's Read last days.")]
        [DefaultValue(null)]
        [Category("Accessibility")]
        public string ReadLastDays
        {
            get
            {
                return readLastDays;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    int.Parse(value);
                }
                readLastDays = value;
            }
        }

        /// <summary>
        /// Is application closed after tests are run.
        /// </summary>
        /// <remarks>
        /// This can be used when tests are run from command line.
        /// </remarks>
        [Browsable(false)]
        [XmlIgnore]
        public CloseApp CloseApplication
        {
            get;
            set;
        }

#if DEBUG
        [Description("Exclude clock Tests.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ExcludeClockTests
        {
            get;
            set;

        }
#endif //DEBUG
    }

    public enum CloseApp
    {
        /// <summary>
        /// Application is newer close.
        /// </summary>
        Never,
        /// <summary>
        /// Application is always closed.
        /// </summary>
        Always,
        /// <summary>
        /// Application is closed if all tests succeeded.
        /// </summary>
        Success
    }
}
