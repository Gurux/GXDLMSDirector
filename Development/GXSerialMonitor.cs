using Gurux.Common;
using Gurux.Serial;
using System;
using System.Text;
using System.Windows.Forms;

namespace GXDLMSDirector
{
    /// <summary>
    /// Serial port monitoring window.
    /// </summary>
    public partial class GXSerialMonitor : Form
    {
        GXSerial serial;
        bool showHex = false;
        //Gurux sends \t: to indicate that trace message is sent.
        bool startTrace = false;
        public GXSerialMonitor()
        {
            InitializeComponent();
            try
            {
                serial = new GXSerial();
                serial.Settings = Properties.Settings.Default.SerialMonitorSettings;
                serial.OnMediaStateChange += Serial_OnMediaStateChange;
                serial.OnReceived += Serial_OnReceived;
                foreach (string it in GXSerial.GetPortNames())
                {
                    PortCb.Items.Add(it);
                }
                if (!string.IsNullOrEmpty(serial.PortName))
                {
                    PortCb.SelectedItem = serial.PortName;
                }
                if (PortCb.Items.Count == 0)
                {
                    SettingsBtn.Enabled = OpenBtn.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void Serial_OnReceived(object sender, ReceiveEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ReceivedEventHandler(Serial_OnReceived), sender, e);
            }
            else
            {
                try
                {
                    byte[] data = (byte[])e.Data;
                    if (data != null && data.Length != 0)
                    {
                        if (showHex)
                        {
                            Trace.AppendText(GXCommon.ToHex((byte[])e.Data, true));
                        }
                        else
                        {
                            string tmp = ASCIIEncoding.ASCII.GetString((byte[])e.Data);
                            if (data[0] == '\t' && data.Length != 1 && data[1] == ':')
                            {
                                tmp = DateTime.Now + "\t" + tmp.Substring(2);
                            }
                            else if (data[0] == '\t' && data.Length == 1)
                            {
                                startTrace = true;
                                return;
                            }
                            else if (startTrace)
                            {
                                if (data[0] == ':')
                                {
                                    if (data.Length == 1)
                                    {
                                        return;
                                    }
                                    tmp = tmp.Substring(1);
                                }
                                tmp = DateTime.Now + "\t" + tmp;
                                startTrace = false;
                            }
                            Trace.AppendText(tmp);
                            if (data[data.Length - 1] == 0)
                            {
                                Trace.AppendText(Environment.NewLine);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.AppendText(ex.Message);
                }
            }
        }

        private void Serial_OnMediaStateChange(object sender, MediaStateEventArgs e)
        {
            if (e.State == Gurux.Common.MediaState.Open)
            {
                OpenBtn.Text = "Close";
                PortCb.Enabled = SettingsBtn.Enabled = false;
                startTrace = false;
            }
            else if (e.State == Gurux.Common.MediaState.Closed)
            {
                OpenBtn.Text = "Open";
                PortCb.Enabled = SettingsBtn.Enabled = true;
            }
        }

        private void OpenBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (serial.IsOpen)
                {
                    serial.Close();
                }
                else
                {
                    serial.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (serial.Properties(this))
                {
                    Properties.Settings.Default.SerialMonitorSettings = serial.Settings;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PortCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (PortCb.SelectedItem != null)
                {
                    serial.PortName = PortCb.SelectedItem.ToString();
                    Properties.Settings.Default.SerialMonitorSettings = serial.Settings;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Is received data shown as hex or ASCII.
        /// </summary>
        private void HexCb_CheckedChanged(object sender, EventArgs e)
        {
            showHex = HexCb.Checked;
        }

        /// <summary>
        /// Close serial port connection when user closes the wnd.
        /// </summary>
        private void GXSerialMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                serial.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Clear trace view.
        /// </summary>
        private void ClearBtb_Click(object sender, EventArgs e)
        {
            try
            {
                Trace.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
