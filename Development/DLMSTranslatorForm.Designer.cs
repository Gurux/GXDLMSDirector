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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RemoveDuplicatesCb = new System.Windows.Forms.CheckBox();
            this.ReceivedRb = new System.Windows.Forms.RadioButton();
            this.SentRb = new System.Windows.Forms.RadioButton();
            this.AllRb = new System.Windows.Forms.RadioButton();
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
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.DataXml = new System.Windows.Forms.TextBox();
            this.DataPdu = new System.Windows.Forms.TextBox();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
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
            this.panel2.Size = new System.Drawing.Size(924, 47);
            this.panel2.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RemoveDuplicatesCb);
            this.groupBox1.Controls.Add(this.ReceivedRb);
            this.groupBox1.Controls.Add(this.SentRb);
            this.groupBox1.Controls.Add(this.AllRb);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(586, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(338, 47);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Show messages";
            // 
            // RemoveDuplicatesCb
            // 
            this.RemoveDuplicatesCb.AutoSize = true;
            this.RemoveDuplicatesCb.Location = new System.Drawing.Point(6, 17);
            this.RemoveDuplicatesCb.Name = "RemoveDuplicatesCb";
            this.RemoveDuplicatesCb.Size = new System.Drawing.Size(117, 17);
            this.RemoveDuplicatesCb.TabIndex = 14;
            this.RemoveDuplicatesCb.Text = "Remove duplicates";
            this.RemoveDuplicatesCb.UseVisualStyleBackColor = true;
            this.RemoveDuplicatesCb.Click += new System.EventHandler(this.TranslateBtn_Click);
            // 
            // ReceivedRb
            // 
            this.ReceivedRb.AutoSize = true;
            this.ReceivedRb.Location = new System.Drawing.Point(262, 16);
            this.ReceivedRb.Name = "ReceivedRb";
            this.ReceivedRb.Size = new System.Drawing.Size(71, 17);
            this.ReceivedRb.TabIndex = 13;
            this.ReceivedRb.Text = "Received";
            this.ReceivedRb.UseVisualStyleBackColor = true;
            this.ReceivedRb.Click += new System.EventHandler(this.TranslateBtn_Click);
            // 
            // SentRb
            // 
            this.SentRb.AutoSize = true;
            this.SentRb.Location = new System.Drawing.Point(205, 16);
            this.SentRb.Name = "SentRb";
            this.SentRb.Size = new System.Drawing.Size(47, 17);
            this.SentRb.TabIndex = 12;
            this.SentRb.Text = "Sent";
            this.SentRb.UseVisualStyleBackColor = true;
            this.SentRb.Click += new System.EventHandler(this.TranslateBtn_Click);
            // 
            // AllRb
            // 
            this.AllRb.AutoSize = true;
            this.AllRb.Checked = true;
            this.AllRb.Location = new System.Drawing.Point(150, 16);
            this.AllRb.Name = "AllRb";
            this.AllRb.Size = new System.Drawing.Size(36, 17);
            this.AllRb.TabIndex = 10;
            this.AllRb.TabStop = true;
            this.AllRb.Text = "All";
            this.AllRb.UseVisualStyleBackColor = true;
            this.AllRb.Click += new System.EventHandler(this.TranslateBtn_Click);
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
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 47);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(924, 490);
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
            this.tabPage1.Size = new System.Drawing.Size(916, 464);
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
            this.MessageXmlTB.Size = new System.Drawing.Size(739, 458);
            this.MessageXmlTB.TabIndex = 3;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(171, 3);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 458);
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
            this.MessagePduTB.Size = new System.Drawing.Size(168, 458);
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
            this.tabPage2.Size = new System.Drawing.Size(916, 464);
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
            this.XmlTB.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.XmlTB.Size = new System.Drawing.Size(910, 355);
            this.XmlTB.TabIndex = 4;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(3, 103);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(910, 3);
            this.splitter2.TabIndex = 3;
            this.splitter2.TabStop = false;
            // 
            // PduTB
            // 
            this.PduTB.Dock = System.Windows.Forms.DockStyle.Top;
            this.PduTB.Location = new System.Drawing.Point(3, 3);
            this.PduTB.Multiline = true;
            this.PduTB.Name = "PduTB";
            this.PduTB.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.PduTB.Size = new System.Drawing.Size(910, 100);
            this.PduTB.TabIndex = 2;
            this.PduTB.Text = "C0018100080000010000FF0200";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.DataXml);
            this.tabPage5.Controls.Add(this.DataPdu);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(916, 464);
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
            this.DataXml.Size = new System.Drawing.Size(742, 458);
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
            this.DataPdu.Size = new System.Drawing.Size(168, 458);
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
            this.ClientSize = new System.Drawing.Size(924, 537);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DLMSTranslatorForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gurux DLMS Translator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
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
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TextBox DataXml;
        private System.Windows.Forms.TextBox DataPdu;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton ReceivedRb;
        private System.Windows.Forms.RadioButton SentRb;
        private System.Windows.Forms.RadioButton AllRb;
        private System.Windows.Forms.CheckBox RemoveDuplicatesCb;
    }
}