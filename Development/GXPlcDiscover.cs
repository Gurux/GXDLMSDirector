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
using System.Collections.Generic;
using System.Diagnostics;
using Gurux.DLMS.Enums;
using System.Collections;
using Gurux.DLMS.Plc;
using Gurux.DLMS.Plc.Enums;

namespace GXDLMSDirector
{
    public partial class GXPlcDiscover : Form
    {
        MainForm _parent;
        IGXMedia media;
        GXAsyncWork ScanWork;

        public GXPlcDiscover(MainForm parent)
        {
            InitializeComponent();
            MetersView_SelectedIndexChanged(null, null);
            _parent = parent;
            try
            {
                if (Settings.Default.PlcInterface == (int)InterfaceType.PlcHdlc)
                {
                    SFskMnu.Checked = true;
                }
                else
                {
                    LlcMnu.Checked = true;
                }
                if (Settings.Default.PlcMedia == "GXSerial" && GXSerial.GetPortNames().Length != 0)
                {
                    OnMediaTypeChanged(SerialMnu, null);
                }
                else
                {
                    OnMediaTypeChanged(NetworkMnu, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
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
                    RegisterMnu.Enabled = true;
                    media.Close();
                    BeginInvoke(new ResetProgressEventHandler(OnResetProgress), 0, true);
                }
                else
                {
                    RegisterMnu.Enabled = false;
                }
                MetersView_SelectedIndexChanged(null, null);
            }
        }

        void OnError(object sender, Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        delegate void IncreaseProgressEventHandler();

        void OnIncreaseProgress()
        {
            ++ProgressBar.Value;
        }

        delegate void AppendMeterEventHandler(List<GXDLMSPlcMeterInfo> list);

        Hashtable _meters = new Hashtable();


        void OnAppendMeter(List<GXDLMSPlcMeterInfo> list)
        {
            foreach (GXDLMSPlcMeterInfo it in list)
            {
                string str = "";
                if (it.SourceAddress == (UInt16)PlcSourceAddress.New)
                {
                    str = "New meter";
                }
                else if (it.AlarmDescriptor == 0x82)
                {
                    str = "Registered";
                }
                else if (it.AlarmDescriptor != 0)
                {
                    str = it.AlarmDescriptor.ToString();
                }
                ListViewItem li = (ListViewItem)_meters[GXCommon.ToHex(it.SystemTitle)];
                string sa;
                if (it.SourceAddress == (UInt16)PlcSourceAddress.New)
                {
                    sa = "New (0x" + it.SourceAddress.ToString("X") + ")";
                }
                else if (it.SourceAddress == (UInt16)PlcSourceAddress.Initiator)
                {
                    sa = "Initiator (0x" + it.SourceAddress.ToString("X") + ")";
                }
                else
                {
                    sa = "0x" + it.SourceAddress.ToString("X");
                }
                if (li == null)
                {
                    li = new ListViewItem(new string[] { GXCommon.ToHex(it.SystemTitle), str, sa});
                    li.Tag = it;
                    _meters.Add(li.Text, li);
                    MetersView.Items.Add(li);
                }
                else
                {
                    li.SubItems[1].Text = str;
                    li.SubItems[2].Text = sa;
                }
            }
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

        /// <summary>
        /// Try to find client and server address from the meter.
        /// </summary>
        private void DiscoverMeters(object sender, GXAsyncWork work, object[] parameters)
        {
            GXDLMSClient cl = new GXDLMSClient(true, 16, 1, Authentication.None, null, (InterfaceType)Settings.Default.PlcInterface);
            byte[] data = cl.Plc.DiscoverRequest();
            ReceiveParameters<byte[]> p = new ReceiveParameters<byte[]>()
            {
                AllData = false,
                Count = 8,
                WaitTime = 10000,
            };
            DateTime start = DateTime.Now;
            GXByteBuffer rd = new GXByteBuffer();
            lock (media.Synchronous)
            {
                if (data != null)
                {
                    media.Send(data, null);
                    start = DateTime.Now;
                }
                GXReplyData reply = new GXReplyData();
                rd = new GXByteBuffer(p.Reply);
                try
                {
                    while (!work.IsCanceled)
                    {
                        p.Reply = null;
                        //If Eop is not set read one byte at time.
                        if (p.Eop == null)
                        {
                            p.Count = cl.GetFrameSize(rd);
                        }
                        if (!media.IsOpen)
                        {
                            throw new InvalidOperationException("Media is closed.");
                        }
                        if (!media.Receive(p))
                        {
                            //It's OK if there is no reply. Read again
                            continue;
                        }
                        rd.Position = 0;
                        rd.Set(p.Reply);
                        if (cl.GetData(rd, reply))
                        {
                            List<GXDLMSPlcMeterInfo> list = cl.Plc.ParseDiscover(reply.Data, (UInt16)reply.TargetAddress, (UInt16)reply.SourceAddress);
                            BeginInvoke(new AppendMeterEventHandler(OnAppendMeter), list);
                        }
                    }
                }
                catch (Exception)
                {
                    //Throw original exception.
                    throw;
                }
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
                    if (e.State == Gurux.Common.MediaState.Opening)
                    {
                        DiscoverBtn.Checked = true;
                        MediaSettingsBtn.Enabled = MediaMnu.Enabled = MediaSettingsMnu.Enabled = false;
                        TraceView.Text = "";
                        DiscoverMnu.Text = "Stop";
                        UpdateStatus("Discovering.");
                    }
                    else if (e.State == Gurux.Common.MediaState.Closed)
                    {
                        DiscoverBtn.Checked = false;
                        MediaSettingsBtn.Enabled = MediaMnu.Enabled = MediaSettingsMnu.Enabled = true;
                        DiscoverMnu.Text = "Discover";
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
                    media.Settings = Settings.Default.PlcSerialSettings;
                    media.OnMediaStateChange += OnMediaStateChange;
                    media.OnReceived += OnReceived;
                    media.OnTrace += MediaOnTrace;
                    if (GXSerial.GetPortNames().Length == 0)
                    {
                        MediaSettingsMnu.Enabled = MediaSettingsMnu.Enabled = false;
                    }
                    Settings.Default.PlcMedia = "GXSerial";
                }
                else
                {
                    media = new GXNet();
                    (media as GXNet).ConfigurableSettings = Gurux.Net.AvailableMediaSettings.All & ~Gurux.Net.AvailableMediaSettings.Server;
                    media.Settings = Settings.Default.PlcNetworkSettings;
                    media.OnMediaStateChange += OnMediaStateChange;
                    media.OnReceived += OnReceived;
                    media.OnTrace += MediaOnTrace;
                    MediaSettingsMnu.Enabled = MediaSettingsMnu.Enabled = true;
                    Settings.Default.PlcMedia = "GXNet";
                }
                media.Trace = TraceLevel.Verbose;
                UpdateStatus("Ready.");
                SerialBtn.Checked = SerialMnu.Checked = isSerial;
                NetworkBtn.Checked = NetworkMnu.Checked = !isSerial;
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
            string str;
            if ((InterfaceType)Settings.Default.PlcInterface == InterfaceType.Plc)
            {
                str = "PLC LLC";
            }
            else
            {
                str = "PLC HDLC";
            }
            StatusLbl.Text = text + " Using " + str + ". " + media.ToString();
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
                }
                if (media.Properties(this))
                {
                    if (media is GXNet)
                    {
                        Settings.Default.PlcNetworkSettings = media.Settings;
                    }
                    else if (!Settings.Default.HdlcAddressUseOpticalProbe)
                    {
                        Settings.Default.PlcSerialSettings = media.Settings;
                    }
                    UpdateStatus("Ready.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DiscoverMnu_Click(object sender, EventArgs e)
        {
            try
            {
                if (media.IsOpen)
                {
                    ScanWork.Cancel();
                    media.Close();
                }
                else
                {
                    ClearMnu_Click(null, null);
                    media.Open();
                    ScanWork = new GXAsyncWork(this, OnAsyncStateChange, DiscoverMeters, OnError, null, new object[] { sender });
                    ScanWork.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GXPlcDiscovery_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (ScanWork != null)
                {
                    ScanWork.Cancel();
                }
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
                Process.Start("https://www.gurux.fi/GXDLMSDirector");
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Register selected meters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterMnu_Click(object sender, EventArgs e)
        {
            try
            {
                if (MetersView.SelectedItems.Count == 1)
                {
                    ListViewItem li = MetersView.SelectedItems[0];
                    GXDLMSPlcMeterInfo mi = (GXDLMSPlcMeterInfo)li.Tag;
                    GXPlcRegisterDlg dlg = new GXPlcRegisterDlg(GXDLMSTranslator.HexToBytes(Settings.Default.PlcSerialSettingsActiveInitiator), mi.SystemTitle);
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        GXDLMSDevice dev = new GXDLMSDevice(media);
                        dev.Comm.client.InterfaceType = (InterfaceType)Settings.Default.PlcInterface;
                        media.Open();
                        GXReplyData reply = new GXReplyData();
                        dev.Comm.client.Plc.MacSourceAddress = dlg.MacAddress;
                        Settings.Default.PlcSerialSettingsActiveInitiator = GXDLMSTranslator.ToHex(dlg.ActiveInitiatorSystemTitle);
                        byte[] data = dev.Comm.client.Plc.RegisterRequest(dlg.ActiveInitiatorSystemTitle, dlg.NewSystemTitle);
                        dev.Comm.ReadDLMSPacket(data, reply);
                        data = dev.Comm.client.Plc.DiscoverRequest();
                        dev.Comm.ReadDLMSPacket(data, reply);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                try
                {
                    media.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Interface type is changed.
        /// </summary>
        private void InterfaceTypeChanged(object sender, EventArgs e)
        {
            try
            {
                LlcMnu.Checked = sender == LlcMnu;
                SFskMnu.Checked = sender == SFskMnu;
                Settings.Default.PlcInterface = (int)(LlcMnu.Checked ? InterfaceType.Plc : InterfaceType.PlcHdlc);
                UpdateStatus("Ready.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearMnu_Click(object sender, EventArgs e)
        {
            try
            {

                _meters.Clear();
                MetersView.Items.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DiscoveryMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (media.IsOpen)
            {
                DiscoverMnu2.Text = "Stop";
            }
            else
            {
                DiscoverMnu2.Text = "Discover";
            }
        }

        private void CreateDeviceMnu_Click(object sender, EventArgs e)
        {
            try
            {
                if (MetersView.SelectedItems.Count == 1)
                {
                    ListViewItem li = MetersView.SelectedItems[0];
                    GXDLMSPlcMeterInfo mi = (GXDLMSPlcMeterInfo)li.Tag;
                    GXDLMSDevice dev = new GXDLMSDevice();
                    //Set empty name or new device is created.
                    dev.Name = "";
                    if (media is GXNet)
                    {
                        dev.Media = new GXNet();
                    }
                    else if (media is GXSerial)
                    {
                        dev.Media = new GXSerial();
                    }
                    dev.Media.Settings = media.Settings;
                    if (!string.IsNullOrEmpty(Settings.Default.SelectedManufacturer))
                    {
                        foreach (var it in _parent.Manufacturers)
                        {
                            if (it.Name == Settings.Default.SelectedManufacturer)
                            {
                                dev.Manufacturer = it.Identification;
                                dev.UseLogicalNameReferencing = it.UseLogicalNameReferencing;
                                break;
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(dev.Manufacturer))
                    {
                        //Select Gurux as default manufacturer.
                        dev.Manufacturer = "grx";
                        dev.UseLogicalNameReferencing = true;
                    }
                    dev.Conformance = (int)GXDLMSClient.GetInitialConformance(dev.UseLogicalNameReferencing);
                    dev.MacDestinationAddress = mi.SourceAddress;
                    dev.MACSourceAddress = mi.DestinationAddress;
                    dev.InterfaceType = (InterfaceType)Settings.Default.PlcInterface;
                    _parent.AddDevice(dev);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_parent, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MetersView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (media != null && !media.IsOpen && MetersView.SelectedItems.Count == 1)
            {
                ListViewItem li = MetersView.SelectedItems[0];
                GXDLMSPlcMeterInfo mi = (GXDLMSPlcMeterInfo)li.Tag;
                RegisterMnu2.Enabled = RegisterMnu.Enabled = mi.SourceAddress == (UInt16)PlcSourceAddress.New;
                CreateDeviceMnu.Enabled = !RegisterMnu2.Enabled;
            }
            else
            {
                RegisterMnu2.Enabled = RegisterMnu.Enabled = CreateDeviceMnu.Enabled = false;
            }
        }
    }
}
