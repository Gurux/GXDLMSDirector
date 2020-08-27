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
    public class GuruxProfileGenericTestSettings : IGXConformanceSettings
    {
        [Description("Try to read by entry using start index 0.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool EntryIndex0
        {
            get;
            set;
        }

        [Description("Read first two lines and read by range and check the values.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ReadRowsByRange
        {
            get;
            set;
        }

        [Description("Read last row.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ReadLastRow
        {
            get;
            set;
        }

        [Description("Read after last row using highest possible entry.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ReadUsingHighestPossibleEntry
        {
            get;
            set;
        }

        [Description("Read after last row.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ReadAfterLastRow
        {
            get;
            set;
        }

        [Description("Read only first column.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ReadFirstColumn
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
                    if (it.Name.StartsWith("ExcludeTest"))
                    {
                        list.Add(it.Name.Substring("ExcludeTest".Length));
                    }
                    else
                    {
                        list.Add(it.Name);
                    }
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
