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
using Gurux.Serial;
using Gurux.Common;
using Gurux.Net;
using GXDLMSDirector.Properties;
using System.Threading;
using Gurux.DLMS.ManufacturerSettings;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;

namespace GXDLMSDirector
{
    public partial class GXHdlcAddressResolver : Form
    {
        IGXMedia media;
        GXAsyncWork ScanWork;
        GXManufacturerCollection Manufacturers;

        public GXHdlcAddressResolver()
        {
            InitializeComponent();
            try
            {
                GXSerial s = new GXSerial();
                List<ToolStripItem> rates = new List<ToolStripItem>();
                List<string> list = new List<string>(Settings.Default.HdlcAddressBaudRates.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                bool addAll = list.Count == 0;
                foreach (int rate in s.GetAvailableBaudRates(null))
                {
                    if (rate != 0)
                    {
                        ToolStripMenuItem it = new ToolStripMenuItem();
                        it.Checked = list.Count == 0 || list.Contains(rate.ToString());
                        it.CheckOnClick = true;
                        it.CheckedChanged += It_CheckedChanged;
                        it.Text = it.Name = rate.ToString();
                        rates.Add(it);
                        if (addAll)
                        {
                            Settings.Default.HdlcAddressBaudRates += ";" + rate.ToString();
                        }
                    }
                }
                ScanBaudRateMnu.DropDownItems.AddRange(rates.ToArray());
                if (Settings.Default.HdlcAddressMedia == "GXSerial" && GXSerial.GetPortNames().Length != 0)
                {
                    OnMediaTypeChanged(SerialMnu, null);
                }
                else
                {
                    OnMediaTypeChanged(NetworkMnu, null);
                }
                if (Settings.Default.HdlcAddressUseOpticalProbe && media is GXSerial)
                {
                    UseOpticalProbeMnu_Click(null, null);
                }
                if (Settings.Default.HdlcAddressScanBaudRates)
                {
                    ScanBaudRateMnu_Click(null, null);
                }
                Manufacturers = new GXManufacturerCollection();
                GXManufacturerCollection.ReadManufacturerSettings(Manufacturers);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void It_CheckedChanged(object sender, EventArgs e)
        {
            List<string> list = new List<string>(Settings.Default.HdlcAddressBaudRates.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            if ((sender as ToolStripMenuItem).Checked)
            {
                list.Add((sender as ToolStripMenuItem).Text);
            }
            else
            {
                list.Remove((sender as ToolStripMenuItem).Text);
            }
            Settings.Default.HdlcAddressBaudRates = string.Join(";", list);
        }

        private void OnReceived(object sender, ReceiveEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ReceivedEventHandler(OnReceived), sender, e);
            }
            else
            {
                try
                {
                    TraceView.AppendText(GXCommon.ToHex((byte[])e.Data));
                }
                catch (Exception ex)
                {
                    TraceView.AppendText(ex.Message);
                }
            }
        }

        void OnAsyncStateChange(object sender, GXAsyncWork work, object[] parameters, AsyncState state, string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AsyncStateChangeEventHandler(OnAsyncStateChange), sender, work, parameters, state, text);
            }
            else
            {
                if (state != AsyncState.Start)
                {
                    media.Close();
                    BeginInvoke(new ResetProgressEventHandler(OnResetProgress), 0, true);
                }
            }
        }

        void OnError(object sender, Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        delegate void AppendTextEventHandler(string text);

        void OnAppendText(string text)
        {
            ProgressTb.AppendText(text);
        }

        delegate void IncreaseProgressEventHandler();

        void OnIncreaseProgress()
        {
            ++ProgressBar.Value;
        }

        delegate void ResetProgressEventHandler(int count, bool reset);

        void OnResetProgress(int count, bool reset)
        {
            ProgressBar.Maximum = count;
            if (reset)
            {
                ProgressBar.Value = 0;
            }
        }

        private bool CheckSnrm(GXDLMSDevice dev, int count, bool reset)
        {
            if (media.IsOpen)
            {
                List<int> serverAddresses = new List<int>();
                //Management Logical Device address.
                serverAddresses.Add(1);
                //One byte broadcast address.
                serverAddresses.Add(0x7f);
                //Two byte broadcast address.
                serverAddresses.Add(0x3FFF);
                BeginInvoke(new ResetProgressEventHandler(OnResetProgress), count * serverAddresses.Count, reset);
                foreach (int address in serverAddresses)
                {
                    BeginInvoke(new IncreaseProgressEventHandler(OnIncreaseProgress));
                    BeginInvoke(new AppendTextEventHandler(OnAppendText),
                        "Try with server address " + address + " (0x" + address.ToString("X") + ")" + Environment.NewLine);

                    dev.Comm.client.ClientAddress = 16;
                    dev.Comm.client.ServerAddress = address;
                    byte[] data = dev.Comm.client.SNRMRequest();
                    GXReplyData reply = new GXReplyData();
                    try
                    {
                        //Accept all data.
                        dev.Comm.client.ClientAddress = 0;
                        dev.Comm.client.ServerAddress = 0x7f;
                        dev.Comm.ReadDataBlock(data, "Send SNRM request.", 0, 0, reply);
                        if (media is GXSerial)
                        {
                            BeginInvoke(new AppendTextEventHandler(OnAppendText),
                                "++++++++++++++++++++++++++++++++++++" + Environment.NewLine +
                                "Used baud rate: " + (media as GXSerial).BaudRate + Environment.NewLine +
                                "Client address: " + reply.ClientAddress + " (0x" + reply.ClientAddress.ToString("X") + ")" + Environment.NewLine +
                                "Server address: " + reply.ServerAddress + " (0x" + reply.ServerAddress.ToString("X") + ")" + Environment.NewLine);
                        }
                        else
                        {
                            BeginInvoke(new AppendTextEventHandler(OnAppendText),
                                "++++++++++++++++++++++++++++++++++++" + Environment.NewLine +
                                "Client address: " + reply.ClientAddress + " (0x" + reply.ClientAddress.ToString("X") + ")" + Environment.NewLine +
                                "Server address: " + reply.ServerAddress + " (0x" + reply.ServerAddress.ToString("X") + ")" + Environment.NewLine);
                        }
                        return true;
                    }
                    catch (TimeoutException)
                    {
                        reply.Clear();
                        try
                        {
                            dev.Comm.ReadDataBlock(dev.Comm.client.DisconnectRequest(true), "Send Disconnect request.", 0, 0, reply);
                        }
                        catch (Exception)
                        {
                            reply.Clear();
                        }
                    }
                    catch (Exception)
                    {
                        reply.Clear();
                        try
                        {
                            dev.Comm.ReadDataBlock(dev.Comm.client.DisconnectRequest(true), "Send Disconnect request.", 0, 0, reply);
                        }
                        catch (Exception)
                        {
                            reply.Clear();
                        }
                    }
                    if (!media.IsOpen)
                    {
                        break;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Try to find client and server address from the meter.
        /// </summary>
        private void FindSettings(object sender, GXAsyncWork work, object[] parameters)
        {
            GXDLMSDevice dev = new GXDLMSDevice(media);
            dev.WaitTime = 1;
            if ((media is GXSerial serial) && Settings.Default.HdlcAddressUseOpticalProbe)
            {
                try
                {
                    string manufactureID;
                    dev.StartProtocol = StartProtocolType.IEC;
                    media.OnMediaStateChange -= OnMediaStateChange;
                    try
                    {
                        dev.Manufacturers = Manufacturers;
                        manufactureID = dev.Comm.InitializeIEC();
                    }
                    finally
                    {
                        media.OnMediaStateChange += OnMediaStateChange;
                    }
                    BeginInvoke(new AppendTextEventHandler(OnAppendText),
                           "++++++++++++++++++++++++++++++++++++" + Environment.NewLine +
                           "Connect to the meter using IEC handshaking." + Environment.NewLine +
                           "Manufacturer is " + manufactureID + Environment.NewLine +
                           "Baud rate is " + serial.BaudRate + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    BeginInvoke(new AppendTextEventHandler(OnAppendText),
                        "Failed to receive reply from the optical probe." + Environment.NewLine +
                           "++++++++++++++++++++++++++++++++++++" + Environment.NewLine +
                           "Checking device with baud rate " + ex.Message + Environment.NewLine);
                    return;
                }
            }
            if (media is GXSerial && Settings.Default.HdlcAddressScanBaudRates && !Settings.Default.HdlcAddressUseOpticalProbe)
            {
                int original = (media as GXSerial).BaudRate;
                List<string> rates = new List<string>(Settings.Default.HdlcAddressBaudRates.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                int count = rates.Count;
                bool first = true;
                foreach (string it in rates)
                {
                    int rate = int.Parse(it);
                    if (rate != 0)
                    {
                        (media as GXSerial).BaudRate = rate;
                        Thread.Sleep(1000);
                        BeginInvoke(new AppendTextEventHandler(OnAppendText),
                            "++++++++++++++++++++++++++++++++++++" + Environment.NewLine +
                            "Checking device with baud rate " + rate + Environment.NewLine);
                        if (CheckSnrm(dev, rates.Count, first))
                        {
                            break;
                        }
                        first = false;
                    }
                    --count;
                }
                (media as GXSerial).BaudRate = original;
            }
            else
            {
                CheckSnrm(dev, 1, true);
            }
        }

        private void OnMediaStateChange(object sender, MediaStateEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MediaStateChangeEventHandler(OnMediaStateChange), sender, e);
            }
            else
            {
                try
                {
                    if (e.State == Gurux.Common.MediaState.Open)
                    {
                        ScanBtn.Checked = true;
                        MediaSettingsBtn.Enabled = MediaMnu.Enabled = MediaSettingsMnu.Enabled = false;
                        ProgressTb.Text = "";
                        TraceView.Text = "";
                        ScanMnu.Text = "Stop";
                        UpdateStatus("Scanning");
                        ScanWork = new GXAsyncWork(this, OnAsyncStateChange, FindSettings, OnError, null, new object[] { sender });
                        ScanWork.Start();
                    }
                    else if (e.State == Gurux.Common.MediaState.Closed)
                    {
                        ScanBtn.Checked = false;
                        MediaSettingsBtn.Enabled = MediaMnu.Enabled = MediaSettingsMnu.Enabled = true;
                        ScanMnu.Text = "Scan";
                        UpdateStatus("Ready.");
                        if (ScanWork != null)
                        {
                            ScanWork.Cancel();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Media type is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMediaTypeChanged(object sender, EventArgs e)
        {
            try
            {
                if (media != null)
                {
                    media.OnMediaStateChange -= OnMediaStateChange;
                    media.OnReceived -= OnReceived;
                    media.OnTrace -= MediaOnTrace;
                }
                bool isSerial = sender == SerialMnu || sender == SerialBtn;
                if (isSerial)
                {
                    media = new GXSerial();
                    media.Settings = Settings.Default.HdlcAddressSerialSettings;
                    media.OnMediaStateChange += OnMediaStateChange;
                    media.OnReceived += OnReceived;
                    media.OnTrace += MediaOnTrace;
                    media.Trace = TraceLevel.Verbose;
                    if (GXSerial.GetPortNames().Length == 0)
                    {
                        MediaSettingsMnu.Enabled = MediaSettingsMnu.Enabled = false;
                    }
                    Settings.Default.HdlcAddressMedia = "GXSerial";
                }
                else
                {
                    media = new GXNet();
                    (media as GXNet).ConfigurableSettings = Gurux.Net.AvailableMediaSettings.All & ~Gurux.Net.AvailableMediaSettings.Server;
                    media.Settings = Settings.Default.HdlcAddressNetworkSettings;
                    media.OnMediaStateChange += OnMediaStateChange;
                    media.OnReceived += OnReceived;
                    media.OnTrace += MediaOnTrace;
                    media.Trace = TraceLevel.Verbose;
                    MediaSettingsMnu.Enabled = MediaSettingsMnu.Enabled = true;
                    Settings.Default.HdlcAddressMedia = "GXNet";
                }
                UpdateStatus("Ready.");
                SerialBtn.Checked = UseOpticalProbeMnu.Enabled = SerialMnu.Checked = isSerial;
                NetworkBtn.Checked = NetworkMnu.Checked = !isSerial;
                ScanBaudRatesBtn.Enabled = ScanBaudRateMnu.Enabled = OpticalProbeBtn.Enabled = UseOpticalProbeMnu.Enabled = isSerial;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MediaOnTrace(object sender, TraceEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new TraceEventHandler(MediaOnTrace), sender, e);
            }
            else
            {
                TraceView.AppendText(e.ToString() + Environment.NewLine);
            }
        }

        private void UpdateStatus(string text)
        {
            if (Settings.Default.HdlcAddressScanBaudRates)
            {
                StatusLbl.Text = text + " " + (media as GXSerial).PortName + " with multiple baud rates.";
            }
            else if (Settings.Default.HdlcAddressUseOpticalProbe)
            {
                StatusLbl.Text = text + " " + media.ToString() + " with optical probe.";
            }
            else
            {
                StatusLbl.Text = text + " " + media.ToString();
            }
        }

        /// <summary>
        /// Show media settings.
        /// </summary>
        private void MediaSettingsMnu_Click(object sender, EventArgs e)
        {
            try
            {
                if (media is GXNet)
                {
                    (media as GXNet).ConfigurableSettings = Gurux.Net.AvailableMediaSettings.All & ~Gurux.Net.AvailableMediaSettings.Server;
                }
                else if (media is GXSerial)
                {
                    if (Settings.Default.HdlcAddressScanBaudRates)
                    {
                        (media as GXSerial).ConfigurableSettings = Gurux.Serial.AvailableMediaSettings.All & ~Gurux.Serial.AvailableMediaSettings.BaudRate;
                    }
                    else
                    {
                        (media as GXSerial).ConfigurableSettings = Gurux.Serial.AvailableMediaSettings.All;
                    }
                }
                if (media.Properties(this))
                {
                    if (media is GXNet)
                    {
                        Settings.Default.HdlcAddressNetworkSettings = media.Settings;
                    }
                    else if (!Settings.Default.HdlcAddressUseOpticalProbe)
                    {
                        Settings.Default.HdlcAddressSerialSettings = media.Settings;
                    }
                    UpdateStatus("Ready.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ScanMnu_Click(object sender, EventArgs e)
        {
            try
            {
                if (media.IsOpen)
                {
                    media.Close();
                }
                else
                {
                    media.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GXHdlcAddressResolver_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (media != null)
                {
                    media.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Is optical probe used.
        /// </summary>
        private void UseOpticalProbeMnu_Click(object sender, EventArgs e)
        {
            OpticalProbeBtn.Checked = UseOpticalProbeMnu.Checked = !UseOpticalProbeMnu.Checked;
            Settings.Default.HdlcAddressUseOpticalProbe = UseOpticalProbeMnu.Checked;
            ScanBaudRatesBtn.Enabled = ScanBaudRateMnu.Enabled = !UseOpticalProbeMnu.Checked;
            if (media is GXSerial serial)
            {
                if (Settings.Default.HdlcAddressUseOpticalProbe)
                {
                    serial.BaudRate = 300;
                    serial.DataBits = 7;
                    serial.Parity = Parity.Even;
                    serial.StopBits = StopBits.One;
                }
                else
                {
                    media.Settings = Settings.Default.HdlcAddressSerialSettings;
                }
            }
            UpdateStatus("Ready.");
        }

        /// <summary>
        /// If baud rate is scanned.
        /// </summary>
        private void ScanBaudRateMnu_Click(object sender, EventArgs e)
        {
            ScanBaudRatesBtn.Checked = ScanBaudRateMnu.Checked = !ScanBaudRateMnu.Checked;
            Settings.Default.HdlcAddressScanBaudRates = ScanBaudRateMnu.Checked;
            OpticalProbeBtn.Enabled = UseOpticalProbeMnu.Enabled = !ScanBaudRateMnu.Checked;
            UpdateStatus("Ready.");
        }

        /// <summary>
        /// Close HDLC address resolver.
        /// </summary>
        private void ExitMnu_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://www.gurux.fi/index.php?q=GXDLMSDirectorHelp");
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }
    }
}
