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
        [ConformanceTest]
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

        [ConformanceTest]
        [Description("Conformances read test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool Read
        {
            get;
            set;
        }

        [ConformanceTest]
        [Description("Conformances write test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool Write
        {
            get;
            set;
        }

        [ConformanceTest]
        [Description("Conformances Attribute0 supportedWith get test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool Attribute0SupportedWithGet
        {
            get;
            set;
        }

        [ConformanceTest]
        [Description("Conformances Block transfer with get or read test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool BlockTransferWithGetOrRead
        {
            get;
            set;
        }

        [ConformanceTest]
        [Description("Conformances Block transfer with set or write test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool BlockTransferWithSetOrWrite
        {
            get;
            set;
        }

        [ConformanceTest]
        [Description("Conformances Block transfer with action test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool BlockTransferWithAction
        {
            get;
            set;
        }

        [ConformanceTest]
        [Description("Conformances multiple references test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool MultipleReferences
        {
            get;
            set;
        }

        [ConformanceTest]
        [Description("Conformances parameterized access test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ParameterizedAccess
        {
            get;
            set;
        }

        [ConformanceTest]
        [Description("Conformances Get test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool Get
        {
            get;
            set;
        }

        [ConformanceTest]
        [Description("Conformances Set test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool Set
        {
            get;
            set;
        }

        [ConformanceTest]
        [Description("Conformances selective access test is excluded.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool SelectiveAccess
        {
            get;
            set;
        }

        [ConformanceTest]
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
            return GXAppConformanceTests.ToString(this);
        }
    }
}
