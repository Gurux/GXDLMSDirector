//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GuruxClub/GXDLMSDirector/Development/OBISCodeForm.cs $
//
// Version:         $Revision: 5618 $,
//                  $Date: 2012-08-24 09:15:04 +0300 (pe, 24 elo 2012) $
//                  $Author: kurumi $
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GXDLMS.ManufacturerSettings;
using Gurux.DLMS;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GXDLMS.Common;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Objects;
using Gurux.Common;

namespace GXDLMSDirector
{
    public partial class AuthenticationForm : Form
    {
        GXAuthentication Target;

        public AuthenticationForm(GXAuthentication target)
        {
            InitializeComponent();
            Target = target;
            this.Text = Target.Type.ToString() + " authentication settings";
            this.SecretTB.Text = GXCommon.ToHex(target.SharedSecret, true);
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (HexRB.Checked)
                {
                    Target.SharedSecret = GXCommon.HexToBytes(this.SecretTB.Text, true);
                }
                else
                {
                    Target.SharedSecret = ASCIIEncoding.ASCII.GetBytes(this.SecretTB.Text);
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
                DialogResult = DialogResult.None;
            }
        }

        private void OnChange(object sender, EventArgs e)
        {
            try
            {
                byte[] data;
                if (HexRB.Checked)
                {
                    data = ASCIIEncoding.ASCII.GetBytes(this.SecretTB.Text);
                    this.SecretTB.Text = GXCommon.ToHex(data, true);
                }
                else
                {
                    data = GXCommon.HexToBytes(this.SecretTB.Text, true);
                    this.SecretTB.Text = ASCIIEncoding.ASCII.GetString(data);
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }       
    }
}
