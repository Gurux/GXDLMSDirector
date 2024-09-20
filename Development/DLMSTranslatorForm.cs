using Gurux.Common;
using Gurux.DLMS;
using Gurux.DLMS.ASN;
using Gurux.DLMS.Ecdsa;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Objects.Enums;
using Gurux.DLMS.UI;
using Gurux.DLMS.UI.Ecdsa;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GXDLMSDirector
{
    enum ShowMessages
    {
        All,
        Sent,
        Received,
        Failed
    }

    /// <summary>
    /// How frames are followed.
    /// </summary>
    [Flags]
    enum Follow
    {
        /// <summary>
        /// Frames are not followed and all the frames ar shown.
        /// </summary>
        None = 0,
        /// <summary>
        /// Frames are followed by server address.
        /// </summary>
        Meter = 1,
        /// <summary>
        /// Frames are followed by client address.
        /// </summary>
        Client = 2,
        /// <summary>
        /// Frames are followed by client and server address.
        /// </summary>
        Both = 3
    }

    public partial class DLMSTranslatorForm : Form
    {
        string searchText = null;
        GXCipheringSettings Ciphering;
        GXDLMSTranslator translator = new GXDLMSTranslator(TranslatorOutputType.SimpleXml);

        public DLMSTranslatorForm()
        {
            InitializeComponent();
            InterfaceCb.Items.Add("");
            foreach (InterfaceType it in Enum.GetValues(typeof(InterfaceType)))
            {
                if (it != InterfaceType.HdlcWithModeE &&
                    it != InterfaceType.LPWAN &&
                    it != InterfaceType.WiSUN &&
                    it != InterfaceType.PlcPrime)
                {
                    InterfaceCb.Items.Add(it);
                }
            }
            foreach (ShowMessages it in Enum.GetValues(typeof(ShowMessages)))
            {
                ShowCb.Items.Add(it);
            }
            foreach (Follow it in Enum.GetValues(typeof(Follow)))
            {
                FollowmessagesCb.Items.Add(it);
            }
            try
            {
                InterfaceCb.SelectedIndex = Properties.Settings.Default.TranslatorInterface;
            }
            catch (Exception)
            {
                InterfaceCb.SelectedIndex = 0;
            }
            ShowCb.SelectedIndex = Properties.Settings.Default.TranslatorShow;
            ViewFrameMnu.Checked = Properties.Settings.Default.TranslatorFrame;
            XmlMnu.Checked = Properties.Settings.Default.TranslatorXml;
            FrameNumberMnu.Checked = Properties.Settings.Default.FrameNumber;
            FollowmessagesCb.SelectedIndex = Properties.Settings.Default.TranslatorFollow;
            translator.OnKeys += Translator_OnKeys;
            translator.Comments = true;
            DataPdu.Text = Properties.Settings.Default.Data;
            tabControl1_SelectedIndexChanged(null, null);
            StandardMnu.Checked = Properties.Settings.Default.TranslatorStandard;
            HexMnu.Checked = Properties.Settings.Default.TranslatorHex;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Pdu))
            {
                PduTB.Text = Properties.Settings.Default.Pdu;
            }
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Message))
            {
                MessagePduTB.Text = Properties.Settings.Default.Message;
            }
            string certificates = null, keys = null;
            try
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GXDLMSDirector");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                certificates = Path.Combine(path, "Certificates");
                if (!Directory.Exists(certificates))
                {
                    Directory.CreateDirectory(certificates);
                }
                keys = Path.Combine(path, "Keys");
                if (!Directory.Exists(keys))
                {
                    Directory.CreateDirectory(keys);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                translator.SecuritySuite = SecuritySuite.Suite0;
                translator.SystemTitle = GXDLMSTranslator.HexToBytes(Properties.Settings.Default.NotifySystemTitle);
                try
                {
                    translator.ServerSystemTitle = GXDLMSTranslator.HexToBytes(Properties.Settings.Default.ServerSystemTitle);
                    translator.BlockCipherKey = GXDLMSTranslator.HexToBytes(Properties.Settings.Default.NotifyBlockCipherKey);
                    translator.AuthenticationKey = GXDLMSTranslator.HexToBytes(Properties.Settings.Default.AuthenticationKey);
                    translator.DedicatedKey = GXDLMSTranslator.HexToBytes(Properties.Settings.Default.DedicatedKey);
                }
                catch (Exception)
                {
                    translator.BlockCipherKey = null;
                    translator.AuthenticationKey = null;
                    translator.DedicatedKey = null;
                }
                translator.InvocationCounter = 0;
            }
            Ciphering = new GXCipheringSettings(translator, keys, certificates,
                        Properties.Settings.Default.ClientAgreementKey,
                        Properties.Settings.Default.ClientSigningKey,
                        Properties.Settings.Default.ServerAgreementKey,
                        Properties.Settings.Default.ServerSigningKey);
            Ciphering.Challenge = GXDLMSTranslator.HexToBytes(Properties.Settings.Default.Challenge);
            tabControl1.TabPages.Add(Ciphering.GetCiphetingTab());
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

        private KeyValuePair<GXPkcs8, GXx509Certificate> FindKey(string SerialNumber)
        {
            BigInteger value = BigInteger.Parse(SerialNumber);
            foreach (var it in Ciphering.KeyPairs)
            {
                if (it.Value.SerialNumber == value)
                {
                    return it;
                }
            }
            return new KeyValuePair<GXPkcs8, GXx509Certificate>();
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
            if (!string.IsNullOrEmpty(Ciphering.ClientSigningKey))
            {
                KeyValuePair<GXPkcs8, GXx509Certificate> it = FindKey(Ciphering.ClientSigningKey);
                GXPrivateKey pk = null;
                if (it.Key != null)
                {
                    pk = it.Key.PrivateKey;
                    pk.SystemTitle = it.Value != null ? GXAsn1Converter.SystemTitleFromSubject(it.Value.Subject) : null;
                }
                translator.SigningKeyPair = new KeyValuePair<GXPublicKey, GXPrivateKey>(translator.SigningKeyPair.Key, pk);
            }
            if (!string.IsNullOrEmpty(Ciphering.ServerSigningKey))
            {
                KeyValuePair<GXPkcs8, GXx509Certificate> it = FindKey(Ciphering.ServerSigningKey);
                GXPublicKey pub = null;
                if (it.Value != null)
                {
                    pub = it.Value.PublicKey;
                    pub.SystemTitle = it.Value != null ? GXAsn1Converter.SystemTitleFromSubject(it.Value.Subject) : null;
                }
                translator.SigningKeyPair = new KeyValuePair<GXPublicKey, GXPrivateKey>(pub, translator.SigningKeyPair.Value);
            }
        }

        /// <summary>
        /// Convert PDU to XML.
        /// </summary>
        private void PduToXmlBtn_Click(object sender, EventArgs e)
        {
            UpdateSecuritySettings();
            try
            {
                XmlTB.Text = translator.PduToXml(RemoveComments(PduTB.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
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
        private String RemoveComments(string data)
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
                    sb.Append(tmp);
                    sb.Append(Environment.NewLine);
                }
            }
            if (sb.Length != 0)
            {
                sb.Length = sb.Length - 2;
            }
            return sb.ToString();
        }

        delegate bool UpdateSystemTitleEventHandler(Form parent, string title, byte[] data, byte[] original);

        private static bool UpdateSystemTitle(Form parent, string title, byte[] data, byte[] original)
        {
            if (parent.InvokeRequired)
            {
                return (bool)parent.Invoke(new UpdateSystemTitleEventHandler(UpdateSystemTitle), parent, title, data, original);
            }
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

        delegate void AppendMessageEventHandler(string text, Color color);

        int FindNextTag(string text, int startIndex, out string tag)
        {
            tag = null;
            int pos, index = -1;
            //Find tags.
            List<string> tags = new List<string>();
            //Add comment start tag.
            tags.Add("<!--");
            //Add comment end tag.
            tags.Add("-->");
            tags.Add("<ExceptionResponse>");
            tags.Add("Manufacturer Code:");
            tags.Add("<ConformanceBit Name=\"");
            foreach (string it in tags)
            {
                pos = text.IndexOf(it, startIndex);
                if (pos != -1)
                {
                    if ((pos < index || index == -1))
                    {
                        index = pos;
                        tag = it;
                    }
                }
            }
            return index;
        }

        void OnAppendMessage(string text)
        {
            OnAppendMessage(text, Color.White);
        }

        void UpdateConformanceBits(string text, bool insideofComment, ref int startIndex, ref int lastIndex)
        {
            string tag = "\" />" + Environment.NewLine;
            int end = text.IndexOf(tag, startIndex);
            if (end != -1)
            {
                Color old = MessageXmlTB.SelectionColor;
                startIndex += "<ConformanceBit Name=\"".Length;
                if (insideofComment)
                {
                    MessageXmlTB.SelectionColor = Color.Green;
                }
                MessageXmlTB.AppendText(text.Substring(lastIndex, startIndex - lastIndex));
                MessageXmlTB.SelectionStart = MessageXmlTB.TextLength;
                MessageXmlTB.SelectionLength = 0;
                MessageXmlTB.SelectionColor = Color.Blue;
                MessageXmlTB.AppendText(text.Substring(startIndex, end - startIndex));
                MessageXmlTB.SelectionColor = old;
                MessageXmlTB.SelectionStart = MessageXmlTB.TextLength;
                MessageXmlTB.SelectionLength = 0;
                MessageXmlTB.AppendText(text.Substring(end, tag.Length));
                end += tag.Length;
                lastIndex = end;
                startIndex += tag.Length;
            }
        }

        void PreComment(string text, string tag, int commentEnd, ref int startIndex, ref int lastIndex)
        {
            MessageXmlTB.AppendText(text.Substring(lastIndex, startIndex - lastIndex));
            MessageXmlTB.SelectionStart = MessageXmlTB.TextLength;
            MessageXmlTB.SelectionLength = 0;
            Color old = MessageXmlTB.SelectionColor;
            MessageXmlTB.SelectionColor = Color.Green;
            MessageXmlTB.AppendText(text.Substring(startIndex, commentEnd - startIndex));
            MessageXmlTB.SelectionColor = old;
            lastIndex = commentEnd;
            startIndex = commentEnd;
            startIndex -= tag.Length;
        }

        void PostComment(string text, string tag, ref int startIndex, ref int lastIndex)
        {
            startIndex += tag.Length;
            Color old = MessageXmlTB.SelectionColor;
            MessageXmlTB.SelectionColor = Color.Green;
            MessageXmlTB.AppendText(text.Substring(lastIndex, startIndex - lastIndex));
            MessageXmlTB.SelectionColor = old;
            lastIndex = startIndex;
        }

        void OnAppendMessage(string text, Color color)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AppendMessageEventHandler(OnAppendMessage), text, color).AsyncWaitHandle.WaitOne();
            }
            else
            {
                int lastIndex = 0;
                int startIndex = 0;
                string tag;
                bool insideofComment = false;
                while ((startIndex = FindNextTag(text, startIndex, out tag)) != -1)
                {
                    if (tag == "<!--")
                    {
                        insideofComment = true;
                        int commentEnd = FindNextTag(text, startIndex + tag.Length, out tag);
                        if (commentEnd != -1)
                        {
                            commentEnd += tag.Length;
                            if (tag == "<ConformanceBit Name=\"")
                            {
                                //Update comment.
                                PreComment(text, tag, commentEnd, ref startIndex, ref lastIndex);
                                UpdateConformanceBits(text, true, ref startIndex, ref lastIndex);
                            }
                            else if (tag == "Manufacturer Code:")
                            {
                                //Update comment.
                                PreComment(text, tag, commentEnd, ref startIndex, ref lastIndex);
                                //Add link.
                                Color old = MessageXmlTB.SelectionColor;
                                MessageXmlTB.SelectionColor = Color.Blue;
                                MessageXmlTB.AppendText(text.Substring(commentEnd, 4));
                                MessageXmlTB.SelectionColor = old;
                                //Flag ID is 3 chars.
                                commentEnd += 4;
                                lastIndex = commentEnd;
                                startIndex = commentEnd;
                            }
                            else
                            {
                                insideofComment = false;
                                MessageXmlTB.AppendText(text.Substring(lastIndex, startIndex - lastIndex));
                                MessageXmlTB.SelectionStart = MessageXmlTB.TextLength;
                                MessageXmlTB.SelectionLength = 0;
                                Color old = MessageXmlTB.SelectionColor;
                                MessageXmlTB.SelectionColor = Color.Green;
                                MessageXmlTB.AppendText(text.Substring(startIndex, commentEnd - startIndex));
                                MessageXmlTB.SelectionColor = old;
                                lastIndex = commentEnd;
                                startIndex = commentEnd;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if (tag == "-->")
                    {
                        insideofComment = false;
                        PostComment(text, tag, ref startIndex, ref lastIndex);
                    }
                    else if (tag == "<ExceptionResponse>")
                    {
                        tag = "</ExceptionResponse>";
                        int end = text.IndexOf(tag, startIndex);
                        if (end != -1)
                        {
                            end += tag.Length;
                            MessageXmlTB.AppendText(text.Substring(lastIndex, startIndex - lastIndex));
                            MessageXmlTB.SelectionStart = MessageXmlTB.TextLength;
                            MessageXmlTB.SelectionLength = 0;
                            Color old = MessageXmlTB.SelectionColor;
                            MessageXmlTB.SelectionColor = Color.Red;
                            MessageXmlTB.AppendText(text.Substring(startIndex, end - startIndex));
                            MessageXmlTB.SelectionColor = old;
                            lastIndex = end;
                            startIndex += tag.Length;
                        }
                    }
                    else if (tag == "<ConformanceBit Name=\"")
                    {
                        UpdateConformanceBits(text, insideofComment, ref startIndex, ref lastIndex);
                    }
                }
                if (color != Color.White)
                {
                    MessageXmlTB.SelectionStart = MessageXmlTB.TextLength;
                    MessageXmlTB.SelectionLength = 0;
                    Color old = MessageXmlTB.SelectionColor;
                    MessageXmlTB.SelectionColor = color;
                    MessageXmlTB.AppendText(text.Substring(lastIndex));
                    MessageXmlTB.SelectionColor = old;
                }
                else
                {
                    MessageXmlTB.AppendText(text.Substring(lastIndex));
                }
            }
        }

        delegate void UpdateProgressBarEventHandler(int value, int count);

        void UpdateProgress(int value, int count)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateProgressBarEventHandler(UpdateProgress), value, count);
            }
            else
            {
                ProgressBar.Value = value;
                StatusLbl.Text = "Searching... " + count + " frames found";
            }
        }

        delegate void UpdateProgressBarMaxEventHandler(int value, int count);

        void UpdateMaxProgress(int value, int count)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateProgressBarMaxEventHandler(UpdateMaxProgress), value, count);
            }
            else
            {
                if (value == 0)
                {
                    if (count != 0)
                    {
                        StatusLbl.Text = "Searching... " + count + " frames found";
                    }
                    else
                    {
                        StatusLbl.Text = "Ready";
                    }
                    ProgressBar.Value = 0;
                    ProgressBar.Visible = false;
                }
                else
                {
                    ProgressBar.Maximum = value;
                }
            }
        }

        ManualResetEvent CancelTranslate = new ManualResetEvent(false);

        /// <summary>
        /// Convert message to XML.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TranslateBtn_Click(object sender, EventArgs e)
        {
            if (MessagePduTB.ReadOnly)
            {
                StatusLbl.Text = "Cancelling translate.";
                CancelTranslate.Set();
                return;
            }
            TranslateBtn.Checked = true;
            MessagePduTB.ReadOnly = true;
            Follow follow = (Follow)FollowmessagesCb.SelectedItem;
            ShowMessages show = (ShowMessages)ShowCb.SelectedItem;
            MessageXmlTB.Text = null;
            ProgressBar.Visible = true;
            object selectedInterface = InterfaceCb.SelectedItem;
            UpdateSecuritySettings();
            StatusLbl.Text = "Finding frames";
            GXByteBuffer bb = new GXByteBuffer();
            bb.Set(GXDLMSTranslator.HexToBytes(RemoveComments(string.Join(Environment.NewLine, MessagePduTB.Lines))));
            System.Threading.Tasks.Task.Run(async () =>
            {
                if (translator.BlockCipherKey != null)
                {
                    OnAppendMessage("BlockCipher key: " +
                        GXDLMSTranslator.ToHex(translator.BlockCipherKey) + Environment.NewLine, Color.Green);
                }
                if (translator.AuthenticationKey != null)
                {
                    OnAppendMessage("Authentication Key:" +
                        GXDLMSTranslator.ToHex(translator.AuthenticationKey) + Environment.NewLine, Color.Green);
                }
                StringBuilder sb = new StringBuilder();
                Security s = translator.Security;
                int count = 1;
                try
                {
                    translator.Clear();
                    translator.PduOnly = PduOnlyMnu.Checked;
                    GXByteBuffer pdu = new GXByteBuffer();
                    UpdateProgress(0, 0);
                    UpdateMaxProgress(bb.Size, 0);
                    GXDLMSTranslatorMessage frame = new GXDLMSTranslatorMessage();
                    frame.Message = bb;
                    if (selectedInterface is string)
                    {
                        frame.InterfaceType = GXDLMSTranslator.GetDlmsFraming(bb);
                        BeginInvoke((Action)(() =>
                        {
                            InterfaceCb.SelectedItem = frame.InterfaceType;
                        }));
                    }
                    else
                    {
                        frame.InterfaceType = (InterfaceType)selectedInterface;
                    }
                    string last = "";
                    int clientAddress = 0, serverAddress = 0;
                    while (translator.FindNextFrame(frame, pdu, clientAddress, serverAddress))
                    {
                        //Translate is cancelled.
                        if (CancelTranslate.WaitOne(1))
                        {
                            UpdateMaxProgress(0, 0);
                            BeginInvoke((Action)(() =>
                            {
                                MessagePduTB.ReadOnly = false;
                                TranslateBtn.Checked = false;
                            }));
                            CancelTranslate.Reset();
                            return;
                        }
                        int start = bb.Position;
                        UpdateProgress(start, count);
                        GXDLMSTranslatorMessage msg = new GXDLMSTranslatorMessage();
                        msg.Message = bb;
                        msg.InterfaceType = frame.InterfaceType;
                        translator.MessageToXml(msg);
                        if (follow != Follow.None)
                        {
                            if ((follow & Follow.Client) != 0 && clientAddress == 0)
                            {
                                clientAddress = msg.SourceAddress;
                            }
                            if ((follow & Follow.Meter) != 0 && serverAddress == 0)
                            {
                                serverAddress = msg.TargetAddress;
                            }
                        }
                        //Remove duplicate messages.
                        if (RemoveDuplicatesMnu.Checked)
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
                                    BeginInvoke((Action)(() =>
                                    {
                                        Ciphering.SystemTitle = msg.SystemTitle;
                                    }));
                                }
                            }
                            if (msg.DedicatedKey != null && msg.DedicatedKey.Length == 16)
                            {
                                if (UpdateSystemTitle(this, "Current dedicated key \"{0}\" is different in the parsed \"{1}\". Do you want to start using parsed one?",
                                    msg.DedicatedKey, translator.DedicatedKey))
                                {
                                    translator.DedicatedKey = msg.DedicatedKey;
                                    BeginInvoke((Action)(() =>
                                    {
                                        Ciphering.DedicatedKey = msg.DedicatedKey;
                                    }));
                                }
                            }
                        }
                        if (msg.Command == Command.Aare && msg.SystemTitle != null && msg.SystemTitle.Length == 8)
                        {
                            if (UpdateSystemTitle(this, "Current Server System title \"{0}\" is different in the parsed \"{1}\". Do you want to start using parsed one?",
                                msg.SystemTitle, translator.ServerSystemTitle))
                            {
                                translator.ServerSystemTitle = msg.SystemTitle;
                                BeginInvoke((Action)(() =>
                                {
                                    Ciphering.ServerSystemTitle = msg.SystemTitle;
                                }));
                            }
                        }
                        if (show != ShowMessages.All)
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
                                    if (show == ShowMessages.Received)
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
                                    if (show == ShowMessages.Sent)
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
                        if (Properties.Settings.Default.TranslatorFrame)
                        {
                            if (Properties.Settings.Default.FrameNumber)
                            {
                                sb.Append(count + ": ");
                            }
                            sb.AppendLine(bb.ToHex(true, start, bb.Position - start));
                        }
                        if (Properties.Settings.Default.TranslatorXml)
                        {
                            sb.Append(msg.Xml);
                        }
                        if (msg.Exception != null)
                        {
                            ++count;
                            OnAppendMessage(sb.ToString(), Color.Red);
                        }
                        else if (show != ShowMessages.Failed)
                        {
                            ++count;
                            OnAppendMessage(sb.ToString());
                        }
                        sb.Clear();
                        pdu.Clear();
                    }
                    OnAppendMessage(sb.ToString());
                    translator.Security = s;
                    //Update UI.
                    await System.Threading.Tasks.Task.Delay(1);
                }
                catch (Exception ex)
                {
                    translator.Security = s;
                    OnAppendMessage(sb.ToString());
                    OnAppendMessage(Environment.NewLine);
                    OnAppendMessage(bb.RemainingHexString(true));
                    MessageBox.Show(ex.Message);
                }
                //Count starts from 1.
                UpdateMaxProgress(0, count - 1);
                BeginInvoke((Action)(() =>
                {
                    MessagePduTB.ReadOnly = false;
                    TranslateBtn.Checked = false;
                }));
            });
        }

        /// <summary>
        /// User changes tab. Update UI.
        /// </summary>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CancelTranslateBtn.Visible = InterfaceCb.Enabled = ShowCb.Enabled = FollowmessagesCb.Enabled = CompletePduMnu.Visible = PduOnlyMnu.Visible = TranslateMnu.Visible = tabControl1.SelectedTab == MessagesTab;
            PduToXmlMnu.Visible = XmlToPduMnu.Visible = tabControl1.SelectedTab == PduTab;
            ConvertMnu.Visible = tabControl1.SelectedTab == DataTab;
            TranslateBtn.Enabled = tabControl1.SelectedTab == MessagesTab || tabControl1.SelectedTab == PduTab || tabControl1.SelectedTab == DataTab;
        }

        /// <summary>
        /// Show value in Hex.
        /// </summary>
        private void HexCB_CheckedChanged(object sender, EventArgs e)
        {
            translator.Hex = HexMnu.Checked;
            if (tabControl1.SelectedIndex == 0)
            {
                TranslateBtn_Click(null, null);
            }
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
            Properties.Settings.Default.TranslatorInterface = InterfaceCb.SelectedIndex;
            Properties.Settings.Default.TranslatorShow = ShowCb.SelectedIndex;
            Properties.Settings.Default.TranslatorFollow = FollowmessagesCb.SelectedIndex;

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

        private void ExitMnu_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Show only PDU.
        /// </summary>
        private void PduOnlyMnu_Click(object sender, EventArgs e)
        {
            PduOnlyMnu.Checked = !PduOnlyMnu.Checked;
            translator.PduOnly = PduOnlyMnu.Checked;
            TranslateBtn_Click(null, null);
        }

        private void ComplatePduMnu_Click(object sender, EventArgs e)
        {
            CompletePduMnu.Checked = !CompletePduMnu.Checked;
            translator.CompletePdu = CompletePduMnu.Checked;
            TranslateBtn_Click(null, null);
        }

        private void StandardMnu_Click(object sender, EventArgs e)
        {
            StandardMnu.Checked = !StandardMnu.Checked;
            Properties.Settings.Default.TranslatorStandard = StandardMnu.Checked;
            if (StandardMnu.Checked)
            {
                translator = new GXDLMSTranslator(TranslatorOutputType.StandardXml);
            }
            else
            {
                translator = new GXDLMSTranslator(TranslatorOutputType.SimpleXml);
            }
            translator.PduOnly = PduOnlyMnu.Checked;
            translator.Comments = true;
            translator.Hex = HexMnu.Checked;
            if (tabControl1.SelectedIndex == 0)
            {
                TranslateBtn_Click(null, null);
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                PduToXmlBtn_Click(null, null);
            }
        }

        private void HexMnu_Click(object sender, EventArgs e)
        {
            HexMnu.Checked = !HexMnu.Checked;
            Properties.Settings.Default.TranslatorHex = HexMnu.Checked;

            translator.Hex = HexMnu.Checked;
            if (tabControl1.SelectedIndex == 0)
            {
                TranslateBtn_Click(null, null);
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                PduToXmlBtn_Click(null, null);
            }
        }

        private void RemoveDuplicatesMnu_Click(object sender, EventArgs e)
        {
            RemoveDuplicatesMnu.Checked = !RemoveDuplicatesMnu.Checked;
            TranslateBtn_Click(null, null);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://www.gurux.fi/GXDLMSTranslatorHelp");
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void ScanBtn_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == MessagesTab)
            {
                TranslateBtn_Click(null, null);
            }
            else if (tabControl1.SelectedTab == PduTab)
            {
                PduToXmlBtn_Click(null, null);
            }
            else if (tabControl1.SelectedTab == DataTab)
            {
                ConvertMnu_Click(null, null);
            }
        }

        /// <summary>
        /// Convert value data to XML.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConvertMnu_Click(object sender, EventArgs e)
        {
            string xml = null;
            UpdateSecuritySettings();
            try
            {
                translator.DataToXml(DataPdu.Text, out xml);
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

        private void FindMnu_Click(object sender, EventArgs e)
        {
            try
            {
                GXFindParameters p = new GXFindParameters();
                p.Hide = SearchDialogHidden.Obis;
                p.Text = searchText;
                if (GXDlmsUi.Find(this, p))
                {
                    searchText = p.Text;
                    FindNextMnu.Enabled = true;
                    FindNextMnu_Click(null, null);
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void FindNextMnu_Click(object sender, EventArgs e)
        {
            try
            {
                if (searchText == null)
                {
                    FindMnu_Click(sender, e);
                    return;
                }
                int pos = MessageXmlTB.Text.IndexOf(searchText, MessageXmlTB.SelectionStart + 1, StringComparison.InvariantCultureIgnoreCase);
                if (pos != -1)
                {
                    MessageXmlTB.Select(pos, searchText.Length);
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Is frame shown on output.
        /// </summary>
        private void ViewFrameMnu_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TranslatorFrame = ViewFrameMnu.Checked = !ViewFrameMnu.Checked;
        }

        /// <summary>
        /// Is XML shown on output.
        /// </summary>
        private void XmlMnu_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TranslatorXml = XmlMnu.Checked = !XmlMnu.Checked;
        }

        private void FrameNumberMnu_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.FrameNumber = FrameNumberMnu.Checked = !FrameNumberMnu.Checked;
        }

        /// <summary>
        /// Cancel translate.
        /// </summary>
        private void CancelTranslateBtn_Click(object sender, EventArgs e)
        {
            StatusLbl.Text = "Cancelling translate.";
            CancelTranslate.Set();
        }
    }
}
