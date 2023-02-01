//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Filename:        $HeadURL$
//
// Version:         $Revision$,
//                  $Date$
//                  $Author$
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
// More information of Gurux products: https://www.gurux.org
//
// This code is licensed under the GNU General Public License v2.
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------

using Gurux.Common;
using Gurux.Serial;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GXDLMSDirector
{
    public partial class GXHdlcAddressesResolverDlg : Form
    {
        Form MediaPropertiesForm;
        public GXHdlcAddressesResolverDlg(IGXMedia media)
        {
            InitializeComponent();
            if (Properties.Settings.Default.HdlcAddressScanBaudRates && (media is GXSerial s))
            {
                List<string> list = new List<string>(Properties.Settings.Default.HdlcAddressBaudRates.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                foreach (int rate in s.GetAvailableBaudRates(null))
                {
                    if (rate != 0)
                    {
                        bool c = list.Contains(rate.ToString());
                        BaudRatesCl.Items.Add(rate.ToString(), c);
                    }
                }
            }
            else
            {
                BaudRatesPanel.Visible = false;
            }
            CustomSettings.Controls.Clear();
            MediaPropertiesForm = media.PropertiesForm;
            (MediaPropertiesForm as IGXPropertyPage).Initialize();
            while (MediaPropertiesForm.Controls.Count != 0)
            {
                Control ctr = MediaPropertiesForm.Controls[0];
                if (ctr is Panel)
                {
                    if (!ctr.Enabled)
                    {
                        MediaPropertiesForm.Controls.RemoveAt(0);
                        continue;
                    }
                }
                CustomSettings.Controls.Add(ctr);
                ctr.Visible = true;
            }
            TestFoundMetersFirstCb.Checked = Properties.Settings.Default.TestFoundMetersFirst;
            TestFailedClientsLastCb.Checked = Properties.Settings.Default.TestFailedClientsLast;
            ServerAddressesTb.Text = Properties.Settings.Default.HdlcServerAddresses.Replace(",", Environment.NewLine);
            ClientAddressesTb.Text = Properties.Settings.Default.HdlcClientAddresses.Replace(",", Environment.NewLine);
            InitializeWaitTimeTb.Text = Properties.Settings.Default.HdlcSearchInitialWaitTime.ToString();
            SearchWaitTimeTb.Text = Properties.Settings.Default.HdlcSearchWaitTime.ToString();
            ConnectionDelayTb.Text = Properties.Settings.Default.HdlcConnectionDelay.ToString();
        }

        private void UpdateAddresses(bool hex, List<int> serverAddresses, List<int> clientAddresses)
        {
            NumberStyles style = hex ? NumberStyles.HexNumber : NumberStyles.Integer;
            //Restore default values if not set.
            if (ServerAddressesTb.Text == "")
            {
                //127 is broadcast address for one byte.
                //16383 is broadcast address for two byte.
                ServerAddressesTb.Text = "1,145,28672,127,16383".Replace(",", Environment.NewLine);
            }
            if (ClientAddressesTb.Text == "")
            {
                ClientAddressesTb.Text = "16,1,100".Replace(",", Environment.NewLine);
            }
            serverAddresses.AddRange(ServerAddressesTb.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(q => int.Parse(q, style)).ToArray());
            if (serverAddresses.Count == 0)
            {
                throw new Exception("There must be at least one server address.");
            }
            clientAddresses.AddRange(ClientAddressesTb.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(q => int.Parse(q, style)).ToArray());
            if (clientAddresses.Count == 0)
            {
                throw new Exception("There must be at least one client address.");
            }
        }

        private void OKBtn_Click(object sender, System.EventArgs e)
        {
            try
            {
                Properties.Settings.Default.HdlcSearchWaitTime = int.Parse(SearchWaitTimeTb.Text);
                if (Properties.Settings.Default.HdlcSearchWaitTime < 0)
                {
                    throw new Exception("Invalid search wait time value.");
                }
                Properties.Settings.Default.HdlcSearchInitialWaitTime = int.Parse(InitializeWaitTimeTb.Text);
                if (Properties.Settings.Default.HdlcSearchInitialWaitTime < 0)
                {
                    throw new Exception("Invalid initial wait time value.");
                }
                Properties.Settings.Default.HdlcConnectionDelay = int.Parse(ConnectionDelayTb.Text);
                if (Properties.Settings.Default.HdlcConnectionDelay < 0)
                {
                    throw new Exception("Invalid HDLC connection delay.");
                }
                List<int> serverAddresses = new List<int>();
                List<int> clientAddresses = new List<int>();
                UpdateAddresses(HexCb.Checked, serverAddresses, clientAddresses);
                Properties.Settings.Default.HdlcServerAddresses = string.Join(",", serverAddresses);
                Properties.Settings.Default.HdlcClientAddresses = string.Join(",", clientAddresses);
                Properties.Settings.Default.TestFoundMetersFirst = TestFoundMetersFirstCb.Checked;
                Properties.Settings.Default.TestFailedClientsLast = TestFailedClientsLastCb.Checked;
                if (BaudRatesPanel.Visible)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var it in BaudRatesCl.CheckedItems)
                    {
                        if (sb.Length != 0)
                        {
                            sb.Append(';');
                        }
                        sb.Append(it);
                    }
                    Properties.Settings.Default.HdlcAddressBaudRates = sb.ToString();
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
                DialogResult = DialogResult.None;
            }
        }

        private void HexCb_CheckedChanged(object sender, EventArgs e)
        {
            List<int> serverAddresses = new List<int>();
            List<int> clientAddresses = new List<int>();
            bool showHex = HexCb.Checked;
            UpdateAddresses(!showHex, serverAddresses, clientAddresses);
            StringBuilder sb = new StringBuilder();
            if (serverAddresses != null)
            {
                foreach (int it in serverAddresses)
                {
                    if (showHex)
                    {
                        sb.AppendLine(it.ToString("X"));
                    }
                    else
                    {
                        sb.AppendLine(it.ToString());
                    }
                }
                ServerAddressesTb.Text = sb.ToString();
            }
            sb.Length = 0;
            if (clientAddresses != null)
            {
                foreach (int it in clientAddresses)
                {
                    if (showHex)
                    {
                        sb.AppendLine(it.ToString("X"));
                    }
                    else
                    {
                        sb.AppendLine(it.ToString());
                    }
                }
                ClientAddressesTb.Text = sb.ToString();
            }
        }

        /// <summary>
        /// Generate all posible server address variables.
        /// </summary>
        private void ServerGenerateAllBtn_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            for (int pos = 1; pos <= 16383; ++pos)
            {
                sb.AppendLine(pos.ToString());
            }
            ServerAddressesTb.Text = sb.ToString();
        }

        /// <summary>
        /// Generate all posible client address variables.
        /// </summary>
        private void ClientGenerateAllBtn_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            for (int pos = 1; pos <= 127; ++pos)
            {
                sb.AppendLine(pos.ToString());
            }
            ClientAddressesTb.Text = sb.ToString();
        }

        /// <summary>
        /// Show help not available message.
        /// </summary>
        /// <param name="hevent">A HelpEventArgs that contains the event data.</param>
        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            try
            {
                string str = "https://www.gurux.fi/GXDLMSHdlcAddressResolverHelp";
                // Show online help.
                Process.Start(str);
                // Set flag to show that the Help event as been handled
                hevent.Handled = true;
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }
    }
}
