//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Filename:        $HeadURL:  $
//
// Version:         $Revision: $,
//                  $Date: $
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

using Gurux.DLMS;
using System;
using System.Windows.Forms;

namespace GXDLMSDirector
{
    public partial class GXPlcRegisterDlg : Form
    {
        public UInt16 MacAddress;
        public byte[] ActiveInitiatorSystemTitle;
        public byte[] NewSystemTitle;

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXPlcRegisterDlg(byte[] activeInitiatorSystemTitle, byte[] newSystemTitle)
        {
            InitializeComponent();
            ActiveInitiatorTb.Text = GXDLMSTranslator.ToHex(activeInitiatorSystemTitle);
            SystemTitleTb.Text = GXDLMSTranslator.ToHex(newSystemTitle);
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ActiveInitiatorSystemTitle = GXDLMSTranslator.HexToBytes(ActiveInitiatorTb.Text);
                if (ActiveInitiatorSystemTitle.Length != 8 && ActiveInitiatorSystemTitle.Length != 6)
                {
                    throw new Exception("Invalid active initiator system title.");
                }
                NewSystemTitle = GXDLMSTranslator.HexToBytes(SystemTitleTb.Text);
                if (NewSystemTitle.Length != 8 && NewSystemTitle.Length != 6)
                {
                    throw new Exception("Invalid new system title.");
                }
                MacAddress = UInt16.Parse(MACAddressTb.Text);
            }
            catch (Exception ex)
            {
                DialogResult = DialogResult.None;
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
