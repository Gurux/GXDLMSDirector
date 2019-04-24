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
using System.Text;

namespace GXDLMSDirector
{
    public class GXConformanceApplicationTests
    {
        [Description("Exclude COSEM application layer tests #1.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ExcludeTest1
        {
            get;
            set;
        }
        [Description("Exclude COSEM application layer tests #4.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ExcludeTest4
        {
            get;
            set;
        }
        [Description("Exclude COSEM application layer tests #5.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ExcludeTest5
        {
            get;
            set;
        }
        [Description("Exclude COSEM application layer tests #6.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ExcludeTest6
        {
            get;
            set;
        }
        [Description("Exclude COSEM application layer tests #7.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ExcludeTest7
        {
            get;
            set;
        }
        [Description("Exclude COSEM application layer tests #9.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ExcludeTest9
        {
            get;
            set;
        }
        [Description("Exclude COSEM application layer tests #11.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ExcludeTest11
        {
            get;
            set;
        }

        [Description("Exclude COSEM application layer tests #12.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ExcludeTest12
        {
            get;
            set;
        }

        [Description("Exclude COSEM application layer tests #14.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ExcludeTest14
        {
            get;
            set;
        }

        [Description("Exclude COSEM application layer tests #15.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ExcludeTest15
        {
            get;
            set;
        }

        [Description("Exclude COSEM application layer tests #16.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ExcludeTest16
        {
            get;
            set;
        }


        /// <summary>
        /// Is any of the tests enabled.
        /// </summary>
        public bool IsEnabled()
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(this);
            foreach (PropertyDescriptor it in props)
            {
                if (it.GetValue(this).Equals(true))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Disable or enable all tests.
        /// </summary>
        public void Set(bool value)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(this);
            foreach (PropertyDescriptor it in props)
            {
                if (it.GetValue(this).Equals(false))
                {
                    it.SetValue(this, value);
                }
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
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(this);
            foreach (PropertyDescriptor it in props)
            {
                if (it.GetValue(this).Equals(false))
                {
                    list.Add(it.Name.Substring("ExcludeTest".Length));
                }
            }

            if (props.Count == list.Count)
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
