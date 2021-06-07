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
    public class CttApplicationSettings : IGXConformanceSettings
    {
        [Description("Exclude COSEM application layer tests T_APPL_IDLE_N1.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool T_APPL_IDLE_N1
        {
            get;
            set;
        }

        [Description("Exclude COSEM application layer tests T_APPL_OPEN_1.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool T_APPL_OPEN_1
        {
            get;
            set;
        }

        [Description("Exclude COSEM application layer tests T_APPL_OPEN_3.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool T_APPL_OPEN_3
        {
            get;
            set;
        }

        [Description("Exclude COSEM application layer tests #4.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool Appl_04
        {
            get;
            set;
        }
        [Description("Exclude COSEM application layer tests T_APPL_OPEN_5.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool T_APPL_OPEN_5
        {
            get;
            set;
        }
        [Description("Exclude COSEM application layer tests T_APPL_OPEN_6.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool T_APPL_OPEN_6
        {
            get;
            set;
        }
        [Description("Exclude COSEM application layer tests T_APPL_OPEN_7.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool T_APPL_OPEN_7
        {
            get;
            set;
        }
        [Description("Exclude COSEM application layer tests T_APPL_OPEN_9.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool T_APPL_OPEN_9
        {
            get;
            set;
        }
        [Description("Exclude COSEM application layer tests T_APPL_OPEN_11.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool T_APPL_OPEN_11
        {
            get;
            set;
        }

        [Description("Exclude COSEM application layer tests T_APPL_OPEN_12.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool T_APPL_OPEN_12
        {
            get;
            set;
        }

        [Description("Exclude COSEM application layer tests T_APPL_OPEN_14.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool T_APPL_OPEN_14
        {
            get;
            set;
        }

        [Description("Exclude COSEM application layer tests T_APPL_DATA_LN_N1.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool T_APPL_DATA_LN_N1
        {
            get;
            set;
        }

        [Description("Exclude COSEM application layer tests T_APPL_DATA_LN_N3.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool T_APPL_DATA_LN_N3
        {
            get;
            set;
        }

        [Description("Exclude COSEM application layer tests T_APPL_DATA_LN_N4.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool T_APPL_DATA_LN_N4
        {
            get;
            set;
        }

        [Description("Exclude application layer tests APPL_REL_P1.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool T_APPL_REL_P1
        {
            get;
            set;
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
                    list.Add(it.Name);
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
