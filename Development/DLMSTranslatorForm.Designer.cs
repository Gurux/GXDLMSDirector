namespace GXDLMSDirector
{
    partial class DLMSTranslatorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DLMSTranslatorForm));
            this.panel2 = new System.Windows.Forms.Panel();
            this.WrapperCb = new System.Windows.Forms.CheckBox();
            this.StandardCB = new System.Windows.Forms.CheckBox();
            this.CompletePDUCb = new System.Windows.Forms.CheckBox();
            this.PduOnlyCB = new System.Windows.Forms.CheckBox();
            this.HexCB = new System.Windows.Forms.CheckBox();
            this.XMLToPduBtn = new System.Windows.Forms.Button();
            this.PduToXmlBtn = new System.Windows.Forms.Button();
            this.TranslateBtn = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.MessageXmlTB = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.MessagePduTB = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.XmlTB = new System.Windows.Forms.TextBox();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.PduTB = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.DedicatedKeyAsciiCb = new System.Windows.Forms.CheckBox();
            this.DedicatedKeyTb = new System.Windows.Forms.TextBox();
            this.DedicatedKeyLbl = new System.Windows.Forms.Label();
            this.ServerSystemTitleAsciiCb = new System.Windows.Forms.CheckBox();
            this.ServerSystemTitleTB = new System.Windows.Forms.TextBox();
            this.ServerSystemtitleLbl = new System.Windows.Forms.Label();
            this.ChallengeAsciiCb = new System.Windows.Forms.CheckBox();
            this.ChallengeTb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.PasswordBtn = new System.Windows.Forms.Button();
            this.InvocationCounterTB = new System.Windows.Forms.TextBox();
            this.InvocationCounterLbl = new System.Windows.Forms.Label();
            this.SystemTitleAsciiCb = new System.Windows.Forms.CheckBox();
            this.BlockCipherKeyAsciiCb = new System.Windows.Forms.CheckBox();
            this.AuthenticationKeyAsciiCb = new System.Windows.Forms.CheckBox();
            this.SecurityCB = new System.Windows.Forms.ComboBox();
            this.AuthenticationKeyTB = new System.Windows.Forms.TextBox();
            this.BlockCipherKeyTB = new System.Windows.Forms.TextBox();
            this.SystemTitleTB = new System.Windows.Forms.TextBox();
            this.AuthenticationKeyLbl = new System.Windows.Forms.Label();
            this.BlockCipherKeyLbl = new System.Windows.Forms.Label();
            this.SystemtitleLbl = new System.Windows.Forms.Label();
            this.SecurityLbl = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.DataXml = new System.Windows.Forms.TextBox();
            this.DataPdu = new System.Windows.Forms.TextBox();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.WrapperCb);
            this.panel2.Controls.Add(this.StandardCB);
            this.panel2.Controls.Add(this.CompletePDUCb);
            this.panel2.Controls.Add(this.PduOnlyCB);
            this.panel2.Controls.Add(this.HexCB);
            this.panel2.Controls.Add(this.XMLToPduBtn);
            this.panel2.Controls.Add(this.PduToXmlBtn);
            this.panel2.Controls.Add(this.TranslateBtn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 47);
            this.panel2.TabIndex = 3;
            // 
            // WrapperCb
            // 
            this.WrapperCb.AutoSize = true;
            this.WrapperCb.Location = new System.Drawing.Point(517, 12);
            this.WrapperCb.Name = "WrapperCb";
            this.WrapperCb.Size = new System.Drawing.Size(67, 17);
            this.WrapperCb.TabIndex = 7;
            this.WrapperCb.Text = "Wrapper";
            this.WrapperCb.UseVisualStyleBackColor = true;
            // 
            // StandardCB
            // 
            this.StandardCB.AutoSize = true;
            this.StandardCB.Location = new System.Drawing.Point(429, 12);
            this.StandardCB.Name = "StandardCB";
            this.StandardCB.Size = new System.Drawing.Size(69, 17);
            this.StandardCB.TabIndex = 6;
            this.StandardCB.Text = "Standard";
            this.StandardCB.UseVisualStyleBackColor = true;
            this.StandardCB.CheckedChanged += new System.EventHandler(this.StandardCB_CheckedChanged);
            // 
            // CompletePDUCb
            // 
            this.CompletePDUCb.AutoSize = true;
            this.CompletePDUCb.Location = new System.Drawing.Point(261, 12);
            this.CompletePDUCb.Name = "CompletePDUCb";
            this.CompletePDUCb.Size = new System.Drawing.Size(96, 17);
            this.CompletePDUCb.TabIndex = 5;
            this.CompletePDUCb.Text = "Complete PDU";
            this.CompletePDUCb.UseVisualStyleBackColor = true;
            this.CompletePDUCb.CheckedChanged += new System.EventHandler(this.CompletePDUCb_CheckedChanged);
            // 
            // PduOnlyCB
            // 
            this.PduOnlyCB.AutoSize = true;
            this.PduOnlyCB.Location = new System.Drawing.Point(178, 12);
            this.PduOnlyCB.Name = "PduOnlyCB";
            this.PduOnlyCB.Size = new System.Drawing.Size(71, 17);
            this.PduOnlyCB.TabIndex = 4;
            this.PduOnlyCB.Text = "PDU only";
            this.PduOnlyCB.UseVisualStyleBackColor = true;
            this.PduOnlyCB.CheckedChanged += new System.EventHandler(this.PduOnlyCB_CheckedChanged);
            // 
            // HexCB
            // 
            this.HexCB.AutoSize = true;
            this.HexCB.Checked = true;
            this.HexCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HexCB.Location = new System.Drawing.Point(370, 12);
            this.HexCB.Name = "HexCB";
            this.HexCB.Size = new System.Drawing.Size(45, 17);
            this.HexCB.TabIndex = 3;
            this.HexCB.Text = "Hex";
            this.HexCB.UseVisualStyleBackColor = true;
            this.HexCB.CheckedChanged += new System.EventHandler(this.HexCB_CheckedChanged);
            // 
            // XMLToPduBtn
            // 
            this.XMLToPduBtn.Location = new System.Drawing.Point(86, 12);
            this.XMLToPduBtn.Name = "XMLToPduBtn";
            this.XMLToPduBtn.Size = new System.Drawing.Size(75, 23);
            this.XMLToPduBtn.TabIndex = 1;
            this.XMLToPduBtn.Text = "XML To Pdu";
            this.XMLToPduBtn.UseVisualStyleBackColor = true;
            this.XMLToPduBtn.Click += new System.EventHandler(this.XMLToPduBtn_Click);
            // 
            // PduToXmlBtn
            // 
            this.PduToXmlBtn.Location = new System.Drawing.Point(5, 12);
            this.PduToXmlBtn.Name = "PduToXmlBtn";
            this.PduToXmlBtn.Size = new System.Drawing.Size(75, 23);
            this.PduToXmlBtn.TabIndex = 0;
            this.PduToXmlBtn.Text = "Pdu To XML";
            this.PduToXmlBtn.UseVisualStyleBackColor = true;
            this.PduToXmlBtn.Click += new System.EventHandler(this.PduToXmlBtn_Click);
            // 
            // TranslateBtn
            // 
            this.TranslateBtn.Location = new System.Drawing.Point(5, 12);
            this.TranslateBtn.Name = "TranslateBtn";
            this.TranslateBtn.Size = new System.Drawing.Size(75, 23);
            this.TranslateBtn.TabIndex = 0;
            this.TranslateBtn.Text = "Translate";
            this.TranslateBtn.UseVisualStyleBackColor = true;
            this.TranslateBtn.Click += new System.EventHandler(this.TranslateBtn_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 47);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 403);
            this.tabControl1.TabIndex = 4;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.MessageXmlTB);
            this.tabPage1.Controls.Add(this.splitter1);
            this.tabPage1.Controls.Add(this.MessagePduTB);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 377);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Messages";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // MessageXmlTB
            // 
            this.MessageXmlTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessageXmlTB.Location = new System.Drawing.Point(174, 3);
            this.MessageXmlTB.MaxLength = 1073741823;
            this.MessageXmlTB.Multiline = true;
            this.MessageXmlTB.Name = "MessageXmlTB";
            this.MessageXmlTB.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.MessageXmlTB.Size = new System.Drawing.Size(615, 371);
            this.MessageXmlTB.TabIndex = 3;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(171, 3);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 371);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // MessagePduTB
            // 
            this.MessagePduTB.Dock = System.Windows.Forms.DockStyle.Left;
            this.MessagePduTB.Location = new System.Drawing.Point(3, 3);
            this.MessagePduTB.MaxLength = 1073741823;
            this.MessagePduTB.Multiline = true;
            this.MessagePduTB.Name = "MessagePduTB";
            this.MessagePduTB.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.MessagePduTB.Size = new System.Drawing.Size(168, 371);
            this.MessagePduTB.TabIndex = 1;
            this.MessagePduTB.Text = resources.GetString("MessagePduTB.Text");
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.XmlTB);
            this.tabPage2.Controls.Add(this.splitter2);
            this.tabPage2.Controls.Add(this.PduTB);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 377);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Pdu";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // XmlTB
            // 
            this.XmlTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.XmlTB.Location = new System.Drawing.Point(3, 106);
            this.XmlTB.Multiline = true;
            this.XmlTB.Name = "XmlTB";
            this.XmlTB.Size = new System.Drawing.Size(786, 268);
            this.XmlTB.TabIndex = 4;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(3, 103);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(786, 3);
            this.splitter2.TabIndex = 3;
            this.splitter2.TabStop = false;
            // 
            // PduTB
            // 
            this.PduTB.Dock = System.Windows.Forms.DockStyle.Top;
            this.PduTB.Location = new System.Drawing.Point(3, 3);
            this.PduTB.Multiline = true;
            this.PduTB.Name = "PduTB";
            this.PduTB.Size = new System.Drawing.Size(786, 100);
            this.PduTB.TabIndex = 2;
            this.PduTB.Text = "C0018100080000010000FF0200";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.DedicatedKeyAsciiCb);
            this.tabPage3.Controls.Add(this.DedicatedKeyTb);
            this.tabPage3.Controls.Add(this.DedicatedKeyLbl);
            this.tabPage3.Controls.Add(this.ServerSystemTitleAsciiCb);
            this.tabPage3.Controls.Add(this.ServerSystemTitleTB);
            this.tabPage3.Controls.Add(this.ServerSystemtitleLbl);
            this.tabPage3.Controls.Add(this.ChallengeAsciiCb);
            this.tabPage3.Controls.Add(this.ChallengeTb);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.PasswordBtn);
            this.tabPage3.Controls.Add(this.InvocationCounterTB);
            this.tabPage3.Controls.Add(this.InvocationCounterLbl);
            this.tabPage3.Controls.Add(this.SystemTitleAsciiCb);
            this.tabPage3.Controls.Add(this.BlockCipherKeyAsciiCb);
            this.tabPage3.Controls.Add(this.AuthenticationKeyAsciiCb);
            this.tabPage3.Controls.Add(this.SecurityCB);
            this.tabPage3.Controls.Add(this.AuthenticationKeyTB);
            this.tabPage3.Controls.Add(this.BlockCipherKeyTB);
            this.tabPage3.Controls.Add(this.SystemTitleTB);
            this.tabPage3.Controls.Add(this.AuthenticationKeyLbl);
            this.tabPage3.Controls.Add(this.BlockCipherKeyLbl);
            this.tabPage3.Controls.Add(this.SystemtitleLbl);
            this.tabPage3.Controls.Add(this.SecurityLbl);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(792, 377);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Ciphering";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // DedicatedKeyAsciiCb
            // 
            this.DedicatedKeyAsciiCb.AutoSize = true;
            this.DedicatedKeyAsciiCb.Location = new System.Drawing.Point(425, 118);
            this.DedicatedKeyAsciiCb.Name = "DedicatedKeyAsciiCb";
            this.DedicatedKeyAsciiCb.Size = new System.Drawing.Size(53, 17);
            this.DedicatedKeyAsciiCb.TabIndex = 78;
            this.DedicatedKeyAsciiCb.Text = "ASCII";
            this.DedicatedKeyAsciiCb.UseVisualStyleBackColor = true;
            this.DedicatedKeyAsciiCb.CheckedChanged += new System.EventHandler(this.DedicatedKeyAsciiCb_CheckedChanged);
            // 
            // DedicatedKeyTb
            // 
            this.DedicatedKeyTb.Location = new System.Drawing.Point(114, 116);
            this.DedicatedKeyTb.Name = "DedicatedKeyTb";
            this.DedicatedKeyTb.Size = new System.Drawing.Size(297, 20);
            this.DedicatedKeyTb.TabIndex = 77;
            // 
            // DedicatedKeyLbl
            // 
            this.DedicatedKeyLbl.AutoSize = true;
            this.DedicatedKeyLbl.Location = new System.Drawing.Point(9, 119);
            this.DedicatedKeyLbl.Name = "DedicatedKeyLbl";
            this.DedicatedKeyLbl.Size = new System.Drawing.Size(80, 13);
            this.DedicatedKeyLbl.TabIndex = 79;
            this.DedicatedKeyLbl.Text = "Dedicated Key:";
            // 
            // ServerSystemTitleAsciiCb
            // 
            this.ServerSystemTitleAsciiCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ServerSystemTitleAsciiCb.AutoSize = true;
            this.ServerSystemTitleAsciiCb.Location = new System.Drawing.Point(425, 267);
            this.ServerSystemTitleAsciiCb.Name = "ServerSystemTitleAsciiCb";
            this.ServerSystemTitleAsciiCb.Size = new System.Drawing.Size(53, 17);
            this.ServerSystemTitleAsciiCb.TabIndex = 67;
            this.ServerSystemTitleAsciiCb.Text = "ASCII";
            this.ServerSystemTitleAsciiCb.UseVisualStyleBackColor = true;
            this.ServerSystemTitleAsciiCb.CheckedChanged += new System.EventHandler(this.ServerSystemTitleAsciiCb_CheckedChanged);
            // 
            // ServerSystemTitleTB
            // 
            this.ServerSystemTitleTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ServerSystemTitleTB.Location = new System.Drawing.Point(112, 264);
            this.ServerSystemTitleTB.Name = "ServerSystemTitleTB";
            this.ServerSystemTitleTB.Size = new System.Drawing.Size(299, 20);
            this.ServerSystemTitleTB.TabIndex = 65;
            // 
            // ServerSystemtitleLbl
            // 
            this.ServerSystemtitleLbl.AutoSize = true;
            this.ServerSystemtitleLbl.Location = new System.Drawing.Point(7, 268);
            this.ServerSystemtitleLbl.Name = "ServerSystemtitleLbl";
            this.ServerSystemtitleLbl.Size = new System.Drawing.Size(97, 13);
            this.ServerSystemtitleLbl.TabIndex = 66;
            this.ServerSystemtitleLbl.Text = "Server System title:";
            // 
            // ChallengeAsciiCb
            // 
            this.ChallengeAsciiCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ChallengeAsciiCb.AutoSize = true;
            this.ChallengeAsciiCb.Location = new System.Drawing.Point(425, 191);
            this.ChallengeAsciiCb.Name = "ChallengeAsciiCb";
            this.ChallengeAsciiCb.Size = new System.Drawing.Size(53, 17);
            this.ChallengeAsciiCb.TabIndex = 64;
            this.ChallengeAsciiCb.Text = "ASCII";
            this.ChallengeAsciiCb.UseVisualStyleBackColor = true;
            this.ChallengeAsciiCb.CheckedChanged += new System.EventHandler(this.ChallengeAsciiCb_CheckedChanged);
            // 
            // ChallengeTb
            // 
            this.ChallengeTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChallengeTb.Location = new System.Drawing.Point(112, 192);
            this.ChallengeTb.Name = "ChallengeTb";
            this.ChallengeTb.Size = new System.Drawing.Size(299, 20);
            this.ChallengeTb.TabIndex = 62;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 63;
            this.label2.Text = "Challenge:";
            // 
            // PasswordBtn
            // 
            this.PasswordBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PasswordBtn.Location = new System.Drawing.Point(285, 218);
            this.PasswordBtn.Name = "PasswordBtn";
            this.PasswordBtn.Size = new System.Drawing.Size(126, 23);
            this.PasswordBtn.TabIndex = 61;
            this.PasswordBtn.Text = "Generate password";
            this.PasswordBtn.UseVisualStyleBackColor = true;
            this.PasswordBtn.Click += new System.EventHandler(this.PasswordBtn_Click);
            // 
            // InvocationCounterTB
            // 
            this.InvocationCounterTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InvocationCounterTB.Location = new System.Drawing.Point(112, 154);
            this.InvocationCounterTB.Name = "InvocationCounterTB";
            this.InvocationCounterTB.Size = new System.Drawing.Size(299, 20);
            this.InvocationCounterTB.TabIndex = 59;
            // 
            // InvocationCounterLbl
            // 
            this.InvocationCounterLbl.AutoSize = true;
            this.InvocationCounterLbl.Location = new System.Drawing.Point(7, 157);
            this.InvocationCounterLbl.Name = "InvocationCounterLbl";
            this.InvocationCounterLbl.Size = new System.Drawing.Size(96, 13);
            this.InvocationCounterLbl.TabIndex = 60;
            this.InvocationCounterLbl.Text = "Invocation counter";
            // 
            // SystemTitleAsciiCb
            // 
            this.SystemTitleAsciiCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SystemTitleAsciiCb.AutoSize = true;
            this.SystemTitleAsciiCb.Location = new System.Drawing.Point(425, 40);
            this.SystemTitleAsciiCb.Name = "SystemTitleAsciiCb";
            this.SystemTitleAsciiCb.Size = new System.Drawing.Size(53, 17);
            this.SystemTitleAsciiCb.TabIndex = 58;
            this.SystemTitleAsciiCb.Text = "ASCII";
            this.SystemTitleAsciiCb.UseVisualStyleBackColor = true;
            this.SystemTitleAsciiCb.CheckedChanged += new System.EventHandler(this.SystemTitleAsciiCb_CheckedChanged);
            // 
            // BlockCipherKeyAsciiCb
            // 
            this.BlockCipherKeyAsciiCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BlockCipherKeyAsciiCb.AutoSize = true;
            this.BlockCipherKeyAsciiCb.Location = new System.Drawing.Point(425, 67);
            this.BlockCipherKeyAsciiCb.Name = "BlockCipherKeyAsciiCb";
            this.BlockCipherKeyAsciiCb.Size = new System.Drawing.Size(53, 17);
            this.BlockCipherKeyAsciiCb.TabIndex = 57;
            this.BlockCipherKeyAsciiCb.Text = "ASCII";
            this.BlockCipherKeyAsciiCb.UseVisualStyleBackColor = true;
            this.BlockCipherKeyAsciiCb.CheckedChanged += new System.EventHandler(this.BlockCipherKeyAsciiCb_CheckedChanged);
            // 
            // AuthenticationKeyAsciiCb
            // 
            this.AuthenticationKeyAsciiCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AuthenticationKeyAsciiCb.AutoSize = true;
            this.AuthenticationKeyAsciiCb.Location = new System.Drawing.Point(425, 93);
            this.AuthenticationKeyAsciiCb.Name = "AuthenticationKeyAsciiCb";
            this.AuthenticationKeyAsciiCb.Size = new System.Drawing.Size(53, 17);
            this.AuthenticationKeyAsciiCb.TabIndex = 56;
            this.AuthenticationKeyAsciiCb.Text = "ASCII";
            this.AuthenticationKeyAsciiCb.UseVisualStyleBackColor = true;
            this.AuthenticationKeyAsciiCb.CheckedChanged += new System.EventHandler(this.AuthenticationKeyAsciiCb_CheckedChanged);
            // 
            // SecurityCB
            // 
            this.SecurityCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SecurityCB.Location = new System.Drawing.Point(112, 12);
            this.SecurityCB.Name = "SecurityCB";
            this.SecurityCB.Size = new System.Drawing.Size(299, 21);
            this.SecurityCB.TabIndex = 46;
            // 
            // AuthenticationKeyTB
            // 
            this.AuthenticationKeyTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AuthenticationKeyTB.Location = new System.Drawing.Point(112, 90);
            this.AuthenticationKeyTB.Name = "AuthenticationKeyTB";
            this.AuthenticationKeyTB.Size = new System.Drawing.Size(299, 20);
            this.AuthenticationKeyTB.TabIndex = 49;
            // 
            // BlockCipherKeyTB
            // 
            this.BlockCipherKeyTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BlockCipherKeyTB.Location = new System.Drawing.Point(112, 64);
            this.BlockCipherKeyTB.Name = "BlockCipherKeyTB";
            this.BlockCipherKeyTB.Size = new System.Drawing.Size(299, 20);
            this.BlockCipherKeyTB.TabIndex = 48;
            // 
            // SystemTitleTB
            // 
            this.SystemTitleTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SystemTitleTB.Location = new System.Drawing.Point(112, 37);
            this.SystemTitleTB.Name = "SystemTitleTB";
            this.SystemTitleTB.Size = new System.Drawing.Size(299, 20);
            this.SystemTitleTB.TabIndex = 47;
            // 
            // AuthenticationKeyLbl
            // 
            this.AuthenticationKeyLbl.AutoSize = true;
            this.AuthenticationKeyLbl.Location = new System.Drawing.Point(7, 93);
            this.AuthenticationKeyLbl.Name = "AuthenticationKeyLbl";
            this.AuthenticationKeyLbl.Size = new System.Drawing.Size(99, 13);
            this.AuthenticationKeyLbl.TabIndex = 55;
            this.AuthenticationKeyLbl.Text = "Authentication Key:";
            // 
            // BlockCipherKeyLbl
            // 
            this.BlockCipherKeyLbl.AutoSize = true;
            this.BlockCipherKeyLbl.Location = new System.Drawing.Point(7, 67);
            this.BlockCipherKeyLbl.Name = "BlockCipherKeyLbl";
            this.BlockCipherKeyLbl.Size = new System.Drawing.Size(91, 13);
            this.BlockCipherKeyLbl.TabIndex = 54;
            this.BlockCipherKeyLbl.Text = "Block Cipher Key:";
            // 
            // SystemtitleLbl
            // 
            this.SystemtitleLbl.AutoSize = true;
            this.SystemtitleLbl.Location = new System.Drawing.Point(7, 41);
            this.SystemtitleLbl.Name = "SystemtitleLbl";
            this.SystemtitleLbl.Size = new System.Drawing.Size(63, 13);
            this.SystemtitleLbl.TabIndex = 53;
            this.SystemtitleLbl.Text = "System title:";
            // 
            // SecurityLbl
            // 
            this.SecurityLbl.AutoSize = true;
            this.SecurityLbl.Location = new System.Drawing.Point(7, 15);
            this.SecurityLbl.Name = "SecurityLbl";
            this.SecurityLbl.Size = new System.Drawing.Size(48, 13);
            this.SecurityLbl.TabIndex = 50;
            this.SecurityLbl.Text = "Security:";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.DataXml);
            this.tabPage5.Controls.Add(this.DataPdu);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(792, 377);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Data to XML";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // DataXml
            // 
            this.DataXml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataXml.Location = new System.Drawing.Point(171, 3);
            this.DataXml.MaxLength = 1073741823;
            this.DataXml.Multiline = true;
            this.DataXml.Name = "DataXml";
            this.DataXml.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.DataXml.Size = new System.Drawing.Size(618, 371);
            this.DataXml.TabIndex = 5;
            // 
            // DataPdu
            // 
            this.DataPdu.Dock = System.Windows.Forms.DockStyle.Left;
            this.DataPdu.Location = new System.Drawing.Point(3, 3);
            this.DataPdu.MaxLength = 1073741823;
            this.DataPdu.Multiline = true;
            this.DataPdu.Name = "DataPdu";
            this.DataPdu.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.DataPdu.Size = new System.Drawing.Size(168, 371);
            this.DataPdu.TabIndex = 4;
            // 
            // helpProvider1
            // 
            this.helpProvider1.HelpNamespace = "https://www.gurux.fi/GXDLMSDirector.DeviceProperties";
            // 
            // DLMSTranslatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel2);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DLMSTranslatorForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gurux DLMS Transfer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox WrapperCb;
        private System.Windows.Forms.CheckBox StandardCB;
        private System.Windows.Forms.CheckBox CompletePDUCb;
        private System.Windows.Forms.CheckBox PduOnlyCB;
        private System.Windows.Forms.CheckBox HexCB;
        private System.Windows.Forms.Button XMLToPduBtn;
        private System.Windows.Forms.Button PduToXmlBtn;
        private System.Windows.Forms.Button TranslateBtn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox MessageXmlTB;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TextBox MessagePduTB;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox XmlTB;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.TextBox PduTB;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox DedicatedKeyAsciiCb;
        private System.Windows.Forms.TextBox DedicatedKeyTb;
        private System.Windows.Forms.Label DedicatedKeyLbl;
        private System.Windows.Forms.CheckBox ServerSystemTitleAsciiCb;
        private System.Windows.Forms.TextBox ServerSystemTitleTB;
        private System.Windows.Forms.Label ServerSystemtitleLbl;
        private System.Windows.Forms.CheckBox ChallengeAsciiCb;
        private System.Windows.Forms.TextBox ChallengeTb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox InvocationCounterTB;
        private System.Windows.Forms.Label InvocationCounterLbl;
        private System.Windows.Forms.CheckBox SystemTitleAsciiCb;
        private System.Windows.Forms.CheckBox BlockCipherKeyAsciiCb;
        private System.Windows.Forms.CheckBox AuthenticationKeyAsciiCb;
        private System.Windows.Forms.ComboBox SecurityCB;
        private System.Windows.Forms.TextBox AuthenticationKeyTB;
        private System.Windows.Forms.TextBox BlockCipherKeyTB;
        private System.Windows.Forms.TextBox SystemTitleTB;
        private System.Windows.Forms.Label AuthenticationKeyLbl;
        private System.Windows.Forms.Label BlockCipherKeyLbl;
        private System.Windows.Forms.Label SystemtitleLbl;
        private System.Windows.Forms.Label SecurityLbl;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TextBox DataXml;
        private System.Windows.Forms.TextBox DataPdu;
        private System.Windows.Forms.Button PasswordBtn;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}