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

using System;
using System.Windows.Forms;
using Gurux.DLMS;
using System.Text;

namespace GXDLMSDirector
{
    public partial class GXByteEditor : Form
    {
        public GXByteEditor(byte[] value)
        {
            InitializeComponent();
            if (value != null)
            {
                if (GXByteBuffer.IsAsciiString(value))
                {
                    ValueTb.Text = ASCIIEncoding.ASCII.GetString(value);
                    AsciiCb.Checked = true;
                }
                else
                {
                    ValueTb.Text = Gurux.Common.GXCommon.ToHex(value);
                }
            }
        }

        public byte[] Value
        {
            get;
            private set;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (AsciiCb.Checked)
                {
                    Value = ASCIIEncoding.ASCII.GetBytes(ValueTb.Text);
                }
                else
                {
                    Value = Gurux.Common.GXCommon.HexToBytes(ValueTb.Text);
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
                DialogResult = DialogResult.None;
            }
        }
    }
}
