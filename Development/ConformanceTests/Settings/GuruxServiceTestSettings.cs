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
    public class GuruxServiceTestSettings : IGXConformanceSettings
    {
        [Description("Check Incompatible (Empty) Conformance.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool IncompatibleConformance
        {
            get;
            set;
        }

        /*
        [Description("Check that meter is returning only proposed conformance (Get/Read).")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ProposedConformance
        {
            get;
            set;
        }
        */

        [Description("Conformances read test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool Read
        {
            get;
            set;
        }

        [Description("Conformances write test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool Write
        {
            get;
            set;
        }

        [Description("Conformances Attribute0 supportedWith get test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool Attribute0SupportedWithGet
        {
            get;
            set;
        }

        [Description("Conformances Block transfer with get or read test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool BlockTransferWithGetOrRead
        {
            get;
            set;
        }

        [Description("Conformances Block transfer with set or write test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool BlockTransferWithSetOrWrite
        {
            get;
            set;
        }

        [Description("Conformances Block transfer with action test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool BlockTransferWithAction
        {
            get;
            set;
        }

        [Description("Conformances multiple references test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool MultipleReferences
        {
            get;
            set;
        }

        [Description("Conformances parameterized access test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ParameterizedAccess
        {
            get;
            set;
        }

        [Description("Conformances Get test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool Get
        {
            get;
            set;
        }

        [Description("Conformances Set test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool Set
        {
            get;
            set;
        }

        [Description("Conformances selective access test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool SelectiveAccess
        {
            get;
            set;
        }

        [Description("Conformances action test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool Action
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
