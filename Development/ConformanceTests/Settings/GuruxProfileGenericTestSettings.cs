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
        [ConformanceTest]
        [Description("Try to read by entry using start index 0.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool EntryIndex0
        {
            get;
            set;
        }

        [ConformanceTest]
        [Description("Read first two lines and read by range and check the values.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ReadRowsByRange
        {
            get;
            set;
        }

        [ConformanceTest]
        [Description("Read last row.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ReadLastRow
        {
            get;
            set;
        }

        [ConformanceTest]
        [Description("Read after last row using highest possible entry.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ReadUsingHighestPossibleEntry
        {
            get;
            set;
        }

        [ConformanceTest]
        [Description("Read after last row.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ReadAfterLastRow
        {
            get;
            set;
        }

        [ConformanceTest]
        [Description("Read only first column.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ReadFirstColumn
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
            return GXAppConformanceTests.ToString(this);
        }
    }
}
