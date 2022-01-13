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

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;

namespace GXDLMSDirector
{
    public class GXConformanceGuruxSettings : IGXConformanceSettings
    {
        GuruxServiceTestSettings serviceTests;
        GuruxClockTestSettings clockTests;
        GuruxProfileGenericTestSettings profileGenericTest;
        GuruxAuthenticationTestSettings authenticationTests;

        public GXConformanceGuruxSettings()
        {
            serviceTests = new GuruxServiceTestSettings();
            clockTests = new GuruxClockTestSettings();
            profileGenericTest = new GuruxProfileGenericTestSettings();
            authenticationTests = new GuruxAuthenticationTestSettings();
        }

        [Description("Gurux Clock tests.")]
        [Category("Accessibility")]
        [TypeConverter(typeof(GXConformanceValueConverter))]
        [DefaultValue(null)]
        [Editor(typeof(GXConformanceEditor), typeof(UITypeEditor))]
        [RefreshPropertiesAttribute(RefreshProperties.All)]
        public GuruxClockTestSettings ClockTests
        {
            get
            {
                return clockTests;
            }
            set
            {
                //If user reset values.
                if (value == null)
                {
                    value = new GuruxClockTestSettings();
                }
                clockTests = value;
            }
        }

        [Description("Gurux Profile Generic tests.")]
        [Category("Accessibility")]
        [TypeConverter(typeof(GXConformanceValueConverter))]
        [DefaultValue(null)]
        [Editor(typeof(GXConformanceEditor), typeof(UITypeEditor))]
        [RefreshPropertiesAttribute(RefreshProperties.All)]
        public GuruxProfileGenericTestSettings ProfileGenericTests
        {
            get
            {
                return profileGenericTest;
            }
            set
            {
                //If user reset values.
                if (value == null)
                {
                    value = new GuruxProfileGenericTestSettings();
                }
                profileGenericTest = value;
            }
        }

        [Description("Gurux Conformance tests.")]
        [Category("Accessibility")]
        [TypeConverter(typeof(GXConformanceValueConverter))]
        [DefaultValue(null)]
        [Editor(typeof(GXConformanceEditor), typeof(UITypeEditor))]
        [RefreshPropertiesAttribute(RefreshProperties.All)]
        public GuruxServiceTestSettings ServiceTests
        {
            get
            {
                return serviceTests;
            }
            set
            {
                //If user reset values.
                if (value == null)
                {
                    value = new GuruxServiceTestSettings();
                }
                serviceTests = value;
            }
        }

        [Description("Gurux Authentication tests.")]
        [Category("Accessibility")]
        [TypeConverter(typeof(GXConformanceValueConverter))]
        [DefaultValue(null)]
        [Editor(typeof(GXConformanceEditor), typeof(UITypeEditor))]
        [RefreshPropertiesAttribute(RefreshProperties.All)]
        public GuruxAuthenticationTestSettings AuthenticationTests
        {
            get
            {
                return authenticationTests;
            }
            set
            {
                //If user reset values.
                if (value == null)
                {
                    value = new GuruxAuthenticationTestSettings();
                }
                authenticationTests = value;
            }
        }

        /// <summary>
        /// Return included tests.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            List<string> list = new List<string>();
            bool all = true;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(this);
            foreach (PropertyDescriptor it in props)
            {
                if (it.GetValue(this) is IGXConformanceSettings v)
                {
                    PropertyDescriptorCollection props2 = TypeDescriptor.GetProperties(v);
                    foreach (PropertyDescriptor it2 in props2)
                    {
                        if (it2.Attributes[typeof(ConformanceTestAttribute)] != null)
                        {
                            if (it2.GetValue(v).Equals(false))
                            {
                                list.Add(it.Name);
                            }
                            else
                            {
                                all = false;
                            }
                        }
                    }
                }
                else if (it.GetValue(this).Equals(false))
                {
                    list.Add(it.Name);
                }
            }
            if (all)
            {
                sb.Append("Running all tests.");
            }
            else if (list.Count == 0)
            {
                sb.Append("All tests are excluded.");
            }
            else
            {
                sb.Append("Running tests: ");
                foreach (string it in list)
                {
                    sb.Append(it);
                    sb.Append(", ");
                }
                sb.Length -= 2;
            }
            return sb.ToString();
        }
    }
}
