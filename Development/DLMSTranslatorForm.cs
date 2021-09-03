using Gurux.Common;
using Gurux.DLMS;
using Gurux.DLMS.ASN;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Objects.Enums;
using Gurux.DLMS.UI;
using Gurux.DLMS.UI.Ecdsa;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace GXDLMSDirector
{
    public partial class DLMSTranslatorForm : Form
    {
        GXCipheringSettings Ciphering;
        GXDLMSTranslator translator = new GXDLMSTranslator(TranslatorOutputType.SimpleXml);
        public DLMSTranslatorForm()
        {
            InitializeComponent();
            InterfaceTypeCb.Items.Add("");
            foreach (InterfaceType it in Enum.GetValues(typeof(InterfaceType)))
            {
                if (it != InterfaceType.HdlcWithModeE)
                {
                    InterfaceTypeCb.Items.Add(it);
                }
            }
            translator.OnKeys += Translator_OnKeys;
            InterfaceTypeCb.SelectedIndex = 0;
            translator.Comments = true;
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
            try
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GXDLMSDirector");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string certificates = Path.Combine(path, "Certificates");
                if (!Directory.Exists(certificates))
                {
                    Directory.CreateDirectory(certificates);
                }
                string keys = Path.Combine(path, "Keys");
                if (!Directory.Exists(keys))
                {
                    Directory.CreateDirectory(keys);
                }
                try
                {
                    translator.SecuritySuite = (SecuritySuite)Properties.Settings.Default.SecuritySuite;
                    translator.Security = (Security)Enum.Parse(typeof(Security), Properties.Settings.Default.Security);
                    translator.SystemTitle = GXDLMSTranslator.HexToBytes(Properties.Settings.Default.NotifySystemTitle);
                    translator.ServerSystemTitle = GXDLMSTranslator.HexToBytes(Properties.Settings.Default.ServerSystemTitle);
                    translator.BlockCipherKey = GXDLMSTranslator.HexToBytes(Properties.Settings.Default.NotifyBlockCipherKey);
                    translator.AuthenticationKey = GXDLMSTranslator.HexToBytes(Properties.Settings.Default.AuthenticationKey);
                    translator.InvocationCounter = (UInt32)Properties.Settings.Default.InvocationCounter;
                    translator.DedicatedKey = GXDLMSTranslator.HexToBytes(Properties.Settings.Default.DedicatedKey);
                }
                catch (Exception)
                {
                    //Set default settings if settings are corrupted.
                    translator.Security = Security.None;
                    translator.SystemTitle = GXDLMSTranslator.HexToBytes(Properties.Settings.Default.NotifySystemTitle);
                    translator.ServerSystemTitle = GXDLMSTranslator.HexToBytes(Properties.Settings.Default.ServerSystemTitle);
                    translator.BlockCipherKey = GXDLMSTranslator.HexToBytes(Properties.Settings.Default.NotifyBlockCipherKey);
                    translator.AuthenticationKey = GXDLMSTranslator.HexToBytes(Properties.Settings.Default.AuthenticationKey);
                    translator.InvocationCounter = 0;
                    translator.DedicatedKey = GXDLMSTranslator.HexToBytes(Properties.Settings.Default.DedicatedKey);
                }
                Ciphering = new GXCipheringSettings(translator, keys, certificates,
                            Properties.Settings.Default.ClientAgreementKey,
                            Properties.Settings.Default.ClientSigningKey,
                            Properties.Settings.Default.ServerAgreementKey,
                            Properties.Settings.Default.ServerSigningKey);
                Ciphering.Challenge = GXDLMSTranslator.HexToBytes(Properties.Settings.Default.Challenge);
                tabControl1.TabPages.Add(Ciphering.GetCiphetingTab());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Find correct key.
        /// </summary>
        private void Translator_OnKeys(object sender, GXCryptoKeyParameter args)
        {
            if (args.Encrypt)
            {
                //Private key is used when data is encrypted.
                args.PrivateKey = Ciphering.FindPrivateKey(args.SecuritySuite, args.CertificateType, args.SystemTitle);
            }
            else
            {
                //Public key is used for data decrypt.
                args.PublicKey = Ciphering.FindPublicKey(args.SecuritySuite, args.CertificateType, args.SystemTitle);
            }
        }

        private void UpdateSecuritySettings()
        {
            translator.SecuritySuite = Ciphering.SecuritySuite;
            translator.Security = Ciphering.Security;
            translator.SystemTitle = Ciphering.SystemTitle;
            translator.ServerSystemTitle = Ciphering.ServerSystemTitle;
            translator.BlockCipherKey = Ciphering.BlockCipherKey;
            translator.AuthenticationKey = Ciphering.AuthenticationKey;
            translator.InvocationCounter = Ciphering.InvocationCounter;
            translator.DedicatedKey = Ciphering.DedicatedKey;
            translator.Keys.Clear();
            translator.Keys.AddRange(Ciphering.KeyPairs);
        }

        /// <summary>
        /// Convert PDU to XML.
        /// </summary>
        private void PduToXmlBtn_Click(object sender, EventArgs e)
        {
            string xml = null;
            UpdateSecuritySettings();
            try
            {
                if (tabControl1.SelectedTab == tabPage5)
                {
                    translator.DataToXml(DataPdu.Text, out xml);
                }
                else
                {
                    XmlTB.Text = translator.PduToXml(RemoveComments(PduTB.Text));
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
            foreach (string it in data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                string tmp = it;
                int pos = tmp.IndexOf("//");
                if (pos != -1)
                {
                    tmp = tmp.Substring(0, pos);
                }
                if (!tmp.StartsWith("#"))
                {
                    if (InterfaceTypeCb.SelectedItem is InterfaceType && (InterfaceType)InterfaceTypeCb.SelectedItem == InterfaceType.WRAPPER)
                    {
                        pos = tmp.IndexOf("0001");
                        if (pos != -1)
                        {
                            sb.Append(tmp.Substring(pos));
                            sb.Append(Environment.NewLine);
                        }
                    }
                    else
                    {
                        sb.Append(tmp);
                        sb.Append(Environment.NewLine);
                    }
                }
            }
            if (sb.Length != 0)
            {
                sb.Length = sb.Length - 2;
            }
            return sb.ToString();
        }

        private static bool UpdateSystemTitle(Form parent, string title, byte[] data, byte[] original)
        {
            if (data != null)
            {
                string st = GXDLMSTranslator.ToHex(data);
                if (GXDLMSTranslator.ToHex(original) != st)
                {
                    if (MessageBox.Show(parent, string.Format(title, GXDLMSTranslator.ToHex(original), st), Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Convert message to XML.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TranslateBtn_Click(object sender, EventArgs e)
        {
            UpdateSecuritySettings();
            MessageXmlTB.Text = "";
            if (translator.BlockCipherKey != null)
            {
                MessageXmlTB.Text = "BlockCipher key: " + GXDLMSTranslator.ToHex(translator.BlockCipherKey) + Environment.NewLine;
            }
            if (translator.AuthenticationKey != null)
            {
                MessageXmlTB.Text += "Authentication Key:" + GXDLMSTranslator.ToHex(translator.AuthenticationKey) + Environment.NewLine;
            }
            StringBuilder sb = new StringBuilder();
            GXByteBuffer bb = new GXByteBuffer();
            //TODO: This can remove later.
            Security s = translator.Security;
            try
            {
                translator.Clear();
                translator.PduOnly = PduOnlyCB.Checked;
                GXByteBuffer pdu = new GXByteBuffer();
                bb.Set(GXDLMSTranslator.HexToBytes(RemoveComments(MessagePduTB.Text)));
                InterfaceType type;
                if (InterfaceTypeCb.SelectedItem is string)
                {
                    type = GXDLMSTranslator.GetDlmsFraming(bb);
                }
                else
                {
                    type = (InterfaceType)InterfaceTypeCb.SelectedItem;
                }
                int cnt = 1;
                string last = "";
                while (translator.FindNextFrame(bb, pdu, type))
                {
                    int start = bb.Position;
                    GXDLMSTranslatorMessage msg = new GXDLMSTranslatorMessage();
                    msg.Message = bb;
                    translator.MessageToXml(msg);
                    //Remove duplicate messages.
                    if (RemoveDuplicatesCb.Checked)
                    {
                        if (last == msg.Xml)
                        {
                            continue;
                        }
                    }
                    last = msg.Xml;
                    if (msg.Command == Command.Aarq)
                    {
                        if (msg.SystemTitle != null && msg.SystemTitle.Length == 8)
                        {
                            if (UpdateSystemTitle(this, "Current System title \"{0}\" is different in the parsed \"{1}\". Do you want to start using parsed one?",
                                msg.SystemTitle, translator.SystemTitle))
                            {
                                translator.SystemTitle = msg.SystemTitle;
                                Ciphering.SystemTitle = msg.SystemTitle;
                            }
                        }
                        if (msg.DedicatedKey != null && msg.DedicatedKey.Length == 16)
                        {
                            if (UpdateSystemTitle(this, "Current dedicated key \"{0}\" is different in the parsed \"{1}\". Do you want to start using parsed one?",
                                msg.DedicatedKey, translator.DedicatedKey))
                            {
                                translator.DedicatedKey = msg.DedicatedKey;
                                Ciphering.DedicatedKey = msg.DedicatedKey;
                            }
                        }
                    }
                    if (msg.Command == Command.Aare && msg.SystemTitle != null && msg.SystemTitle.Length == 8)
                    {
                        if (UpdateSystemTitle(this, "Current Server System title \"{0}\" is different in the parsed \"{1}\". Do you want to start using parsed one?",
                            msg.SystemTitle, translator.ServerSystemTitle))
                        {
                            translator.ServerSystemTitle = msg.SystemTitle;
                            Ciphering.ServerSystemTitle = msg.SystemTitle;
                        }
                    }
                    if (!AllRb.Checked)
                    {
                        switch (msg.Command)
                        {
                            case Command.None:
                                break;
                            case Command.InitiateRequest:
                            case Command.ReadRequest:
                            case Command.WriteRequest:
                            case Command.GetRequest:
                            case Command.SetRequest:
                            case Command.MethodRequest:
                            case Command.Snrm:
                            case Command.Aarq:
                            case Command.ReleaseRequest:
                            case Command.DisconnectRequest:
                            case Command.AccessRequest:
                            case Command.GloGetRequest:
                            case Command.GloSetRequest:
                            case Command.GloMethodRequest:
                            case Command.GloInitiateRequest:
                            case Command.GloReadRequest:
                            case Command.GloWriteRequest:
                            case Command.DedInitiateRequest:
                            case Command.DedReadRequest:
                            case Command.DedWriteRequest:
                            case Command.DedGetRequest:
                            case Command.DedSetRequest:
                            case Command.DedMethodRequest:
                            case Command.GatewayRequest:
                                if (ReceivedRb.Checked)
                                {
                                    continue;
                                }
                                break;
                            case Command.InitiateResponse:
                            case Command.ReadResponse:
                            case Command.WriteResponse:
                            case Command.GetResponse:
                            case Command.SetResponse:
                            case Command.MethodResponse:
                            case Command.Ua:
                            case Command.Aare:
                            case Command.ReleaseResponse:
                            case Command.AccessResponse:
                            case Command.GloGetResponse:
                            case Command.GloSetResponse:
                            case Command.GloMethodResponse:
                            case Command.GloInitiateResponse:
                            case Command.GloReadResponse:
                            case Command.GloWriteResponse:
                            case Command.DedInitiateResponse:
                            case Command.DedReadResponse:
                            case Command.DedWriteResponse:
                            case Command.DedGetResponse:
                            case Command.DedSetResponse:
                            case Command.DedMethodResponse:
                            case Command.GatewayResponse:
                                if (SentRb.Checked)
                                {
                                    continue;
                                }
                                break;
                            case Command.DisconnectMode:
                            case Command.UnacceptableFrame:
                            case Command.ConfirmedServiceError:
                            case Command.ExceptionResponse:
                            case Command.GeneralBlockTransfer:
                            case Command.DataNotification:
                            case Command.GloEventNotification:
                            case Command.GloConfirmedServiceError:
                            case Command.GeneralGloCiphering:
                            case Command.GeneralDedCiphering:
                            case Command.GeneralCiphering:
                            case Command.GeneralSigning:
                            case Command.InformationReport:
                            case Command.EventNotification:
                            case Command.DedConfirmedServiceError:
                            case Command.DedUnconfirmedWriteRequest:
                            case Command.DedInformationReport:
                            case Command.DedEventNotification:
                                break;
                        }
                    }
                    sb.AppendLine(cnt + ": " + bb.ToHex(true, start, bb.Position - start));
                    ++cnt;
                    sb.Append(msg.Xml);
                    pdu.Clear();
                }
                MessageXmlTB.Text += sb.ToString();
                translator.Security = s;
            }
            catch (Exception ex)
            {
                translator.Security = s;
                MessageXmlTB.AppendText(sb.ToString());
                MessageXmlTB.AppendText(Environment.NewLine);
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

            Properties.Settings.Default.SecuritySuite = (int)translator.SecuritySuite;
            Properties.Settings.Default.Security = translator.Security.ToString();
            Properties.Settings.Default.NotifySystemTitle = GXDLMSTranslator.ToHex(translator.SystemTitle);
            Properties.Settings.Default.ServerSystemTitle = GXDLMSTranslator.ToHex(translator.ServerSystemTitle);
            Properties.Settings.Default.NotifyBlockCipherKey = GXDLMSTranslator.ToHex(translator.BlockCipherKey);
            Properties.Settings.Default.AuthenticationKey = GXDLMSTranslator.ToHex(translator.AuthenticationKey);
            Properties.Settings.Default.DedicatedKey = GXDLMSTranslator.ToHex(translator.DedicatedKey);
            Properties.Settings.Default.InvocationCounter = translator.InvocationCounter;
            Properties.Settings.Default.Challenge = GXDLMSTranslator.ToHex(Ciphering.Challenge);
            Properties.Settings.Default.ClientAgreementKey = Ciphering.ClientAgreementKey;
            Properties.Settings.Default.ClientSigningKey = Ciphering.ClientSigningKey;
            Properties.Settings.Default.ServerAgreementKey = Ciphering.ServerAgreementKey;
            Properties.Settings.Default.ServerSigningKey = Ciphering.ServerSigningKey;

            Properties.Settings.Default.Data = DataPdu.Text;
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GXDLMSDirector");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string certificates = Path.Combine(path, "Certificates");
            if (!Directory.Exists(certificates))
            {
                Directory.CreateDirectory(certificates);
            }
            Properties.Settings.Default.Save();
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
    }
}
