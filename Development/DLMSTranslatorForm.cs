using Gurux.Common;
using Gurux.DLMS;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Secure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GXDLMSDirector
{
    public partial class DLMSTranslatorForm : Form
    {
        GXDLMSTranslator translator = new GXDLMSTranslator(TranslatorOutputType.SimpleXml);

        public DLMSTranslatorForm()
        {
            InitializeComponent();
            translator.Comments = true;
            SecurityCB.Items.AddRange(new object[] { Security.None, Security.Authentication,
                                      Security.Encryption, Security.AuthenticationEncryption});
            SecurityCB.SelectedItem = Enum.Parse(typeof(Security), Properties.Settings.Default.Security);
            SystemTitleTB.Text = Properties.Settings.Default.NotifySystemTitle;
            ServerSystemTitleTB.Text = Properties.Settings.Default.ServerSystemTitle;
            BlockCipherKeyTB.Text = Properties.Settings.Default.NotifyBlockCipherKey;
            AuthenticationKeyTB.Text = Properties.Settings.Default.AuthenticationKey;
            InvocationCounterTB.Text = Properties.Settings.Default.InvocationCounter.ToString();
            ChallengeTb.Text = Properties.Settings.Default.Challenge;
            DedicatedKeyTb.Text = Properties.Settings.Default.DedicatedKey;

            DataPdu.Text = Properties.Settings.Default.Data;
            tabControl1_SelectedIndexChanged(null, null);
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Pdu))
            {
                PduTB.Text = Properties.Settings.Default.Pdu;
            }
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Message))
            {
                MessagePduTB.Text = Properties.Settings.Default.Message;
            }
        }

        /// <summary>
        /// Convert PDU to XML.
        /// </summary>
        private void PduToXmlBtn_Click(object sender, EventArgs e)
        {
            string xml = null;
            try
            {
                if (tabControl1.SelectedIndex == 4)
                {
                    translator.DataToXml(DataPdu.Text, out xml);
                }
                else
                {
                    UpdateSecurity();
                    XmlTB.Text = translator.PduToXml(PduTB.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
            finally
            {
                DataXml.Text = xml;
            }
        }


        /// <summary>
        /// Convert XML to PDU.
        /// </summary>
        private void XMLToPduBtn_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateSecurity();
                if (tabControl1.SelectedIndex == 4)
                {
                    DataPdu.Text = GXCommon.ToHex(translator.XmlToData(DataXml.Text));
                }
                else
                {
                    PduTB.Text = GXCommon.ToHex(translator.XmlToPdu(XmlTB.Text), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }

        }

        /// <summary>
        /// Remove comments.
        /// </summary>
        /// <param name="data">Input.</param>
        /// <returns>Data where comments are removed.</returns>
        private String RemoveComments(String data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (String it in data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!it.StartsWith("#"))
                {
                    if (WrapperCb.Checked)
                    {
                        int pos = it.IndexOf("0001");
                        if (pos != -1)
                        {
                            sb.Append(it.Substring(pos));
                            sb.Append("\r\n");
                        }
                    }
                    else
                    {
                        sb.Append(it);
                        sb.Append("\r\n");
                    }
                }
            }
            if (sb.Length != 0)
            {
                sb.Length = sb.Length - 2;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Update security settings to the translator.
        /// </summary>
        private void UpdateSecurity()
        {
            translator.Security = (Security)Enum.Parse(typeof(Security), SecurityCB.SelectedItem.ToString());
            translator.SystemTitle = GXCommon.HexToBytes(GetAsHex(SystemTitleTB.Text, SystemTitleAsciiCb.Checked));
            translator.BlockCipherKey = GXCommon.HexToBytes(GetAsHex(BlockCipherKeyTB.Text, BlockCipherKeyAsciiCb.Checked));
            translator.AuthenticationKey = GXCommon.HexToBytes(GetAsHex(AuthenticationKeyTB.Text, AuthenticationKeyAsciiCb.Checked));
            translator.InvocationCounter = UInt32.Parse(InvocationCounterTB.Text);
            translator.ServerSystemTitle = GXCommon.HexToBytes(GetAsHex(ServerSystemTitleTB.Text, ServerSystemTitleAsciiCb.Checked));
            translator.DedicatedKey = GXCommon.HexToBytes(GetAsHex(DedicatedKeyTb.Text, DedicatedKeyAsciiCb.Checked));
            translator.DedicatedKey = GXCommon.HexToBytes(GetAsHex(DedicatedKeyTb.Text, DedicatedKeyAsciiCb.Checked));
        }

        /// <summary>
        /// Convert message to XML.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TranslateBtn_Click(object sender, EventArgs e)
        {
            MessageXmlTB.Text = "";
            StringBuilder sb = new StringBuilder();
            GXByteBuffer bb = new GXByteBuffer();
            //TODO: This can remove later.
            Security s = translator.Security;
            try
            {
                translator.Clear();
                UpdateSecurity();
                translator.Security = Security.Authentication;

                translator.PduOnly = PduOnlyCB.Checked;
                GXByteBuffer pdu = new GXByteBuffer();
                bb.Set(GXDLMSTranslator.HexToBytes(RemoveComments(MessagePduTB.Text)));
                InterfaceType type = GXDLMSTranslator.GetDlmsFraming(bb);
                int cnt = 1;
                while (translator.FindNextFrame(bb, pdu, type))
                {
                    int start = bb.Position;
                    string tmp = translator.MessageToXml(bb);
                    sb.AppendLine(cnt + ": " + bb.ToHex(true, start, bb.Position - start));
                    ++cnt;
                    sb.Append(tmp);
                    pdu.Clear();
                }
                MessageXmlTB.Text = sb.ToString();
                translator.Security = s;
            }
            catch (Exception ex)
            {
                translator.Security = s;
                MessageXmlTB.AppendText(sb.ToString());
                MessageXmlTB.AppendText("\r\n");
                MessageXmlTB.AppendText(bb.RemainingHexString(true));

                MessageBox.Show(this, ex.Message);
            }
        }

        /// <summary>
        /// User changes tab. Update UI.
        /// </summary>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool page1 = tabControl1.SelectedIndex == 0;
            PduOnlyCB.Visible = CompletePDUCb.Visible = TranslateBtn.Visible = page1;
            PduToXmlBtn.Visible = XMLToPduBtn.Visible = !page1;
        }

        /// <summary>
        /// Show value in Hex.
        /// </summary>
        private void HexCB_CheckedChanged(object sender, EventArgs e)
        {
            translator.Hex = HexCB.Checked;
            if (tabControl1.SelectedIndex == 0)
            {
                TranslateBtn_Click(null, null);
            }
        }

        /// <summary>
        /// Show only PDU.
        /// </summary>
        private void PduOnlyCB_CheckedChanged(object sender, EventArgs e)
        {
            translator.PduOnly = PduOnlyCB.Checked;
            TranslateBtn_Click(null, null);
        }

        private void CompletePDUCb_CheckedChanged(object sender, EventArgs e)
        {
            translator.CompletePdu = CompletePDUCb.Checked;
            TranslateBtn_Click(null, null);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Pdu = PduTB.Text;
            Properties.Settings.Default.Message = MessagePduTB.Text;

            Properties.Settings.Default.Security = SecurityCB.SelectedItem.ToString();
            Properties.Settings.Default.NotifySystemTitle = GetAsHex(SystemTitleTB.Text, SystemTitleAsciiCb.Checked);
            Properties.Settings.Default.ServerSystemTitle = GetAsHex(ServerSystemTitleTB.Text, ServerSystemTitleAsciiCb.Checked);
            Properties.Settings.Default.NotifyBlockCipherKey = GetAsHex(BlockCipherKeyTB.Text, BlockCipherKeyAsciiCb.Checked);
            Properties.Settings.Default.AuthenticationKey = GetAsHex(AuthenticationKeyTB.Text, AuthenticationKeyAsciiCb.Checked);
            Properties.Settings.Default.DedicatedKey = GetAsHex(DedicatedKeyTb.Text, DedicatedKeyAsciiCb.Checked);
            Properties.Settings.Default.InvocationCounter = ulong.Parse(InvocationCounterTB.Text);
            Properties.Settings.Default.Challenge = GetAsHex(ChallengeTb.Text, ChallengeAsciiCb.Checked);
            Properties.Settings.Default.Data = DataPdu.Text;
            Properties.Settings.Default.Save();
        }

        private static bool IsAscii(byte[] value)
        {
            if (value != null)
            {
                foreach (byte it in value)
                {
                    if (it < 0x21 || it > 0x7E)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static string GetAsHex(string value, bool ascii)
        {
            if (ascii)
            {
                return GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(value), false);
            }
            return GXCommon.ToHex(GXCommon.HexToBytes(value), false);
        }

        private void SystemTitleAsciiCb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (SystemTitleAsciiCb.Checked)
                {
                    if (!IsAscii(GXCommon.HexToBytes(SystemTitleTB.Text)))
                    {
                        SystemTitleAsciiCb.CheckedChanged -= SystemTitleAsciiCb_CheckedChanged;
                        SystemTitleAsciiCb.Checked = !SystemTitleAsciiCb.Checked;
                        SystemTitleAsciiCb.CheckedChanged += SystemTitleAsciiCb_CheckedChanged;
                        throw new ArgumentOutOfRangeException("There are non ASCII chars.");
                    }
                    SystemTitleTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(SystemTitleTB.Text));
                }
                else
                {
                    SystemTitleTB.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(SystemTitleTB.Text), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void BlockCipherKeyAsciiCb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (BlockCipherKeyAsciiCb.Checked)
                {
                    if (!IsAscii(GXCommon.HexToBytes(BlockCipherKeyTB.Text)))
                    {
                        BlockCipherKeyAsciiCb.CheckedChanged -= BlockCipherKeyAsciiCb_CheckedChanged;
                        BlockCipherKeyAsciiCb.Checked = !BlockCipherKeyAsciiCb.Checked;
                        BlockCipherKeyAsciiCb.CheckedChanged += BlockCipherKeyAsciiCb_CheckedChanged;
                        throw new ArgumentOutOfRangeException("There are non ASCII chars.");
                    }
                    BlockCipherKeyTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(BlockCipherKeyTB.Text));
                }
                else
                {
                    BlockCipherKeyTB.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(BlockCipherKeyTB.Text), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void AuthenticationKeyAsciiCb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (AuthenticationKeyAsciiCb.Checked)
                {
                    if (!IsAscii(GXCommon.HexToBytes(AuthenticationKeyTB.Text)))
                    {
                        AuthenticationKeyAsciiCb.CheckedChanged -= AuthenticationKeyAsciiCb_CheckedChanged;
                        AuthenticationKeyAsciiCb.Checked = !AuthenticationKeyAsciiCb.Checked;
                        AuthenticationKeyAsciiCb.CheckedChanged += AuthenticationKeyAsciiCb_CheckedChanged;
                        throw new ArgumentOutOfRangeException("There are non ASCII chars.");
                    }
                    AuthenticationKeyTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(AuthenticationKeyTB.Text));
                }
                else
                {
                    AuthenticationKeyTB.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(AuthenticationKeyTB.Text), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void ChallengeAsciiCb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ChallengeAsciiCb.Checked)
                {
                    if (!IsAscii(GXCommon.HexToBytes(ChallengeTb.Text)))
                    {
                        ChallengeAsciiCb.CheckedChanged -= ChallengeAsciiCb_CheckedChanged;
                        ChallengeAsciiCb.Checked = false;
                        ChallengeAsciiCb.CheckedChanged += ChallengeAsciiCb_CheckedChanged;
                        throw new ArgumentOutOfRangeException("There are non ASCII chars.");
                    }
                    ChallengeTb.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(ChallengeTb.Text));
                }
                else
                {
                    ChallengeTb.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(ChallengeTb.Text), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }


        private void StandardCB_CheckedChanged(object sender, EventArgs e)
        {
            if (StandardCB.Checked)
            {
                translator = new GXDLMSTranslator(TranslatorOutputType.StandardXml);
            }
            else
            {
                translator = new GXDLMSTranslator(TranslatorOutputType.SimpleXml);
            }
            translator.PduOnly = PduOnlyCB.Checked;
            translator.Comments = true;
            translator.Hex = HexCB.Checked;
            if (tabControl1.SelectedIndex == 0)
            {
                TranslateBtn_Click(null, null);
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                PduToXmlBtn_Click(null, null);
            }
        }

        private void PasswordBtn_Click(object sender, EventArgs e)
        {
            try
            {
                GXCiphering c = new GXCiphering(null);
                c.SystemTitle = GXCommon.HexToBytes(GetAsHex(SystemTitleTB.Text, SystemTitleAsciiCb.Checked));
                c.BlockCipherKey = GXCommon.HexToBytes(GetAsHex(BlockCipherKeyTB.Text, BlockCipherKeyAsciiCb.Checked));
                c.AuthenticationKey = GXCommon.HexToBytes(GetAsHex(AuthenticationKeyTB.Text, AuthenticationKeyAsciiCb.Checked));
                c.InvocationCounter = UInt32.Parse(InvocationCounterTB.Text);
                MessageBox.Show(this, GXCommon.ToHex(c.GenerateGmacPassword(GXCommon.HexToBytes(GetAsHex(ChallengeTb.Text, ChallengeAsciiCb.Checked))), true));
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void ServerSystemTitleAsciiCb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ServerSystemTitleAsciiCb.Checked)
                {
                    if (!IsAscii(GXCommon.HexToBytes(ServerSystemTitleTB.Text)))
                    {
                        ServerSystemTitleAsciiCb.CheckedChanged -= ServerSystemTitleAsciiCb_CheckedChanged;
                        ServerSystemTitleAsciiCb.Checked = !ServerSystemTitleAsciiCb.Checked;
                        ServerSystemTitleAsciiCb.CheckedChanged += ServerSystemTitleAsciiCb_CheckedChanged;
                        throw new ArgumentOutOfRangeException("There are non ASCII chars.");
                    }
                    ServerSystemTitleTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(ServerSystemTitleTB.Text));
                }
                else
                {
                    ServerSystemTitleTB.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(ServerSystemTitleTB.Text), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void DedicatedKeyAsciiCb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (DedicatedKeyAsciiCb.Checked)
                {
                    if (!IsAscii(GXCommon.HexToBytes(DedicatedKeyTb.Text)))
                    {
                        DedicatedKeyAsciiCb.CheckedChanged -= DedicatedKeyAsciiCb_CheckedChanged;
                        DedicatedKeyAsciiCb.Checked = !DedicatedKeyAsciiCb.Checked;
                        DedicatedKeyAsciiCb.CheckedChanged += DedicatedKeyAsciiCb_CheckedChanged;
                        throw new ArgumentOutOfRangeException("There are non ASCII chars.");
                    }
                    DedicatedKeyTb.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(DedicatedKeyTb.Text));
                }
                else
                {
                    DedicatedKeyTb.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(DedicatedKeyTb.Text), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }
    }
}
