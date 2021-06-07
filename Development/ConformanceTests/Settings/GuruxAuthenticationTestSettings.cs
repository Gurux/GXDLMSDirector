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
    public class GuruxAuthenticationTestSettings : IGXConformanceSettings
    {
        [Description("Try to connect using wrong password.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool WrongPassword
        {
            get;
            set;
        }

        [Description("Try to connect using wrong password.")]
        [Category("Connection")]
        [DefaultValue(null)]
        public string InvalidPassword
        {
            get;
            set;
        }


        [Description("DLMS version number is read from the association object and it must be 8.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool DLMSVersionNumber
        {
            get;
            set;
        }


        [Description("Client SAP is read from the association object and it must be equal to client address.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ClientSAP
        {
            get;
            set;
        }

        [Description("Server SAP is read from the association object and it must be equal to server address.")]
        [DefaultValue(false)]
        [Category("Accessibility")]
        public bool ServerSAP
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
                object value = it.GetValue(this);
                if (value != null && value.Equals(false))
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
