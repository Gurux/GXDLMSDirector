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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DLMSTranslatorForm));
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.TranslateBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.InterfaceCb = new System.Windows.Forms.ToolStripComboBox();
            this.ShowLbl = new System.Windows.Forms.ToolStripLabel();
            this.ShowCb = new System.Windows.Forms.ToolStripComboBox();
            this.FollowmessagesLbl = new System.Windows.Forms.ToolStripLabel();
            this.FollowmessagesCb = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.TranslateMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.CancelTranslateBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.PduToXmlMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.XmlToPduMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.ConvertMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FindMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.FindNextMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewFrameMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.XmlMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.FrameNumberMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.PduOnlyMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.CompletePduMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.StandardMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.RemoveDuplicatesMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.HexMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.MessagesTab = new System.Windows.Forms.TabPage();
            this.MessageXmlTB = new System.Windows.Forms.RichTextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.MessagePduTB = new System.Windows.Forms.RichTextBox();
            this.PduTab = new System.Windows.Forms.TabPage();
            this.XmlTB = new System.Windows.Forms.TextBox();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.PduTB = new System.Windows.Forms.TextBox();
            this.DataTab = new System.Windows.Forms.TabPage();
            this.DataXml = new System.Windows.Forms.TextBox();
            this.DataPdu = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.MessagesTab.SuspendLayout();
            this.PduTab.SuspendLayout();
            this.DataTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // helpProvider1
            // 
            this.helpProvider1.HelpNamespace = "https://www.gurux.fi/GXDLMSDirector.DeviceProperties";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLbl,
            this.toolStripStatusLabel1,
            this.ProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 568);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(984, 22);
            this.statusStrip1.TabIndex = 69;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLbl
            // 
            this.StatusLbl.Name = "StatusLbl";
            this.StatusLbl.Size = new System.Drawing.Size(867, 17);
            this.StatusLbl.Spring = true;
            this.StatusLbl.Text = "Ready";
            this.StatusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ProgressBar.AutoSize = false;
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.RightToLeftLayout = true;
            this.ProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TranslateBtn,
            this.toolStripSeparator5,
            this.toolStripLabel1,
            this.InterfaceCb,
            this.ShowLbl,
            this.ShowCb,
            this.FollowmessagesLbl,
            this.FollowmessagesCb,
            this.toolStripSeparator4,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(984, 25);
            this.toolStrip1.TabIndex = 80;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // TranslateBtn
            // 
            this.TranslateBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TranslateBtn.Image = ((System.Drawing.Image)(resources.GetObject("TranslateBtn.Image")));
            this.TranslateBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TranslateBtn.Name = "TranslateBtn";
            this.TranslateBtn.Size = new System.Drawing.Size(23, 22);
            this.TranslateBtn.Click += new System.EventHandler(this.ScanBtn_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(56, 22);
            this.toolStripLabel1.Text = "Interface:";
            // 
            // InterfaceCb
            // 
            this.InterfaceCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InterfaceCb.Name = "InterfaceCb";
            this.InterfaceCb.Size = new System.Drawing.Size(121, 25);
            // 
            // ShowLbl
            // 
            this.ShowLbl.Name = "ShowLbl";
            this.ShowLbl.Size = new System.Drawing.Size(93, 22);
            this.ShowLbl.Text = "Show messages:";
            // 
            // ShowCb
            // 
            this.ShowCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ShowCb.Name = "ShowCb";
            this.ShowCb.Size = new System.Drawing.Size(121, 25);
            // 
            // FollowmessagesLbl
            // 
            this.FollowmessagesLbl.Name = "FollowmessagesLbl";
            this.FollowmessagesLbl.Size = new System.Drawing.Size(99, 22);
            this.FollowmessagesLbl.Text = "Follow messages:";
            // 
            // FollowmessagesCb
            // 
            this.FollowmessagesCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FollowmessagesCb.Name = "FollowmessagesCb";
            this.FollowmessagesCb.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "DLMS translator Help...";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMnu,
            this.editToolStripMenuItem,
            this.ViewMnu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 24);
            this.menuStrip1.TabIndex = 79;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FileMnu
            // 
            this.FileMnu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TranslateMnu,
            this.CancelTranslateBtn,
            this.PduToXmlMnu,
            this.XmlToPduMnu,
            this.ConvertMnu,
            this.toolStripSeparator1,
            this.ExitMnu});
            this.FileMnu.Name = "FileMnu";
            this.FileMnu.Size = new System.Drawing.Size(37, 20);
            this.FileMnu.Text = "File";
            // 
            // TranslateMnu
            // 
            this.TranslateMnu.Name = "TranslateMnu";
            this.TranslateMnu.Size = new System.Drawing.Size(138, 22);
            this.TranslateMnu.Text = "&Translate";
            this.TranslateMnu.Click += new System.EventHandler(this.TranslateBtn_Click);
            // 
            // CancelTranslateBtn
            // 
            this.CancelTranslateBtn.Name = "CancelTranslateBtn";
            this.CancelTranslateBtn.Size = new System.Drawing.Size(138, 22);
            this.CancelTranslateBtn.Text = "Cancel";
            this.CancelTranslateBtn.Click += new System.EventHandler(this.CancelTranslateBtn_Click);
            // 
            // PduToXmlMnu
            // 
            this.PduToXmlMnu.Name = "PduToXmlMnu";
            this.PduToXmlMnu.Size = new System.Drawing.Size(138, 22);
            this.PduToXmlMnu.Text = "PDU to XML";
            this.PduToXmlMnu.Click += new System.EventHandler(this.PduToXmlBtn_Click);
            // 
            // XmlToPduMnu
            // 
            this.XmlToPduMnu.Name = "XmlToPduMnu";
            this.XmlToPduMnu.Size = new System.Drawing.Size(138, 22);
            this.XmlToPduMnu.Text = "XML to PDU";
            this.XmlToPduMnu.Click += new System.EventHandler(this.XMLToPduBtn_Click);
            // 
            // ConvertMnu
            // 
            this.ConvertMnu.Name = "ConvertMnu";
            this.ConvertMnu.Size = new System.Drawing.Size(138, 22);
            this.ConvertMnu.Text = "&Convert";
            this.ConvertMnu.Click += new System.EventHandler(this.ConvertMnu_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(135, 6);
            // 
            // ExitMnu
            // 
            this.ExitMnu.Name = "ExitMnu";
            this.ExitMnu.Size = new System.Drawing.Size(138, 22);
            this.ExitMnu.Text = "Exit";
            this.ExitMnu.Click += new System.EventHandler(this.ExitMnu_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FindMnu,
            this.FindNextMnu});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // FindMnu
            // 
            this.FindMnu.Name = "FindMnu";
            this.FindMnu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.FindMnu.Size = new System.Drawing.Size(146, 22);
            this.FindMnu.Text = "&Find...";
            this.FindMnu.Click += new System.EventHandler(this.FindMnu_Click);
            // 
            // FindNextMnu
            // 
            this.FindNextMnu.Name = "FindNextMnu";
            this.FindNextMnu.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.FindNextMnu.Size = new System.Drawing.Size(146, 22);
            this.FindNextMnu.Text = "Find Next";
            this.FindNextMnu.Click += new System.EventHandler(this.FindNextMnu_Click);
            // 
            // ViewMnu
            // 
            this.ViewMnu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowMnu,
            this.PduOnlyMnu,
            this.CompletePduMnu,
            this.StandardMnu,
            this.toolStripSeparator6,
            this.RemoveDuplicatesMnu,
            this.toolStripSeparator2,
            this.HexMnu});
            this.ViewMnu.Name = "ViewMnu";
            this.ViewMnu.Size = new System.Drawing.Size(44, 20);
            this.ViewMnu.Text = "&View";
            // 
            // ShowMnu
            // 
            this.ShowMnu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewFrameMnu,
            this.XmlMnu,
            this.FrameNumberMnu});
            this.ShowMnu.Name = "ShowMnu";
            this.ShowMnu.Size = new System.Drawing.Size(174, 22);
            this.ShowMnu.Text = "Show";
            // 
            // ViewFrameMnu
            // 
            this.ViewFrameMnu.Checked = true;
            this.ViewFrameMnu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewFrameMnu.Name = "ViewFrameMnu";
            this.ViewFrameMnu.Size = new System.Drawing.Size(154, 22);
            this.ViewFrameMnu.Text = "Frame";
            this.ViewFrameMnu.Click += new System.EventHandler(this.ViewFrameMnu_Click);
            // 
            // XmlMnu
            // 
            this.XmlMnu.Checked = true;
            this.XmlMnu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.XmlMnu.Name = "XmlMnu";
            this.XmlMnu.Size = new System.Drawing.Size(154, 22);
            this.XmlMnu.Text = "Xml";
            this.XmlMnu.Click += new System.EventHandler(this.XmlMnu_Click);
            // 
            // FrameNumberMnu
            // 
            this.FrameNumberMnu.Name = "FrameNumberMnu";
            this.FrameNumberMnu.Size = new System.Drawing.Size(154, 22);
            this.FrameNumberMnu.Text = "Frame Number";
            this.FrameNumberMnu.Click += new System.EventHandler(this.FrameNumberMnu_Click);
            // 
            // PduOnlyMnu
            // 
            this.PduOnlyMnu.Name = "PduOnlyMnu";
            this.PduOnlyMnu.Size = new System.Drawing.Size(174, 22);
            this.PduOnlyMnu.Text = "PDU Only";
            this.PduOnlyMnu.Click += new System.EventHandler(this.PduOnlyMnu_Click);
            // 
            // CompletePduMnu
            // 
            this.CompletePduMnu.Name = "CompletePduMnu";
            this.CompletePduMnu.Size = new System.Drawing.Size(174, 22);
            this.CompletePduMnu.Text = "Complate PDU";
            this.CompletePduMnu.Click += new System.EventHandler(this.ComplatePduMnu_Click);
            // 
            // StandardMnu
            // 
            this.StandardMnu.Name = "StandardMnu";
            this.StandardMnu.Size = new System.Drawing.Size(174, 22);
            this.StandardMnu.Text = "Standard";
            this.StandardMnu.Click += new System.EventHandler(this.StandardMnu_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(171, 6);
            // 
            // RemoveDuplicatesMnu
            // 
            this.RemoveDuplicatesMnu.Name = "RemoveDuplicatesMnu";
            this.RemoveDuplicatesMnu.Size = new System.Drawing.Size(174, 22);
            this.RemoveDuplicatesMnu.Text = "Remove duplicates";
            this.RemoveDuplicatesMnu.Click += new System.EventHandler(this.RemoveDuplicatesMnu_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(171, 6);
            // 
            // HexMnu
            // 
            this.HexMnu.Name = "HexMnu";
            this.HexMnu.Size = new System.Drawing.Size(174, 22);
            this.HexMnu.Text = "Hex";
            this.HexMnu.Click += new System.EventHandler(this.HexMnu_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.MessagesTab);
            this.tabControl1.Controls.Add(this.PduTab);
            this.tabControl1.Controls.Add(this.DataTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 49);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(984, 519);
            this.tabControl1.TabIndex = 81;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // MessagesTab
            // 
            this.MessagesTab.Controls.Add(this.MessageXmlTB);
            this.MessagesTab.Controls.Add(this.splitter1);
            this.MessagesTab.Controls.Add(this.MessagePduTB);
            this.MessagesTab.Location = new System.Drawing.Point(4, 22);
            this.MessagesTab.Name = "MessagesTab";
            this.MessagesTab.Padding = new System.Windows.Forms.Padding(3);
            this.MessagesTab.Size = new System.Drawing.Size(976, 493);
            this.MessagesTab.TabIndex = 0;
            this.MessagesTab.Text = "Messages";
            this.MessagesTab.UseVisualStyleBackColor = true;
            // 
            // MessageXmlTB
            // 
            this.MessageXmlTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessageXmlTB.Location = new System.Drawing.Point(263, 3);
            this.MessageXmlTB.Name = "MessageXmlTB";
            this.MessageXmlTB.ReadOnly = true;
            this.MessageXmlTB.Size = new System.Drawing.Size(710, 487);
            this.MessageXmlTB.TabIndex = 6;
            this.MessageXmlTB.Text = "";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(260, 3);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 487);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // MessagePduTB
            // 
            this.MessagePduTB.Dock = System.Windows.Forms.DockStyle.Left;
            this.MessagePduTB.Location = new System.Drawing.Point(3, 3);
            this.MessagePduTB.Name = "MessagePduTB";
            this.MessagePduTB.Size = new System.Drawing.Size(257, 487);
            this.MessagePduTB.TabIndex = 1;
            this.MessagePduTB.Text = resources.GetString("MessagePduTB.Text");
            // 
            // PduTab
            // 
            this.PduTab.Controls.Add(this.XmlTB);
            this.PduTab.Controls.Add(this.splitter2);
            this.PduTab.Controls.Add(this.PduTB);
            this.PduTab.Location = new System.Drawing.Point(4, 22);
            this.PduTab.Name = "PduTab";
            this.PduTab.Padding = new System.Windows.Forms.Padding(3);
            this.PduTab.Size = new System.Drawing.Size(976, 493);
            this.PduTab.TabIndex = 1;
            this.PduTab.Text = "Pdu";
            this.PduTab.UseVisualStyleBackColor = true;
            // 
            // XmlTB
            // 
            this.XmlTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.XmlTB.Location = new System.Drawing.Point(3, 106);
            this.XmlTB.Multiline = true;
            this.XmlTB.Name = "XmlTB";
            this.XmlTB.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.XmlTB.Size = new System.Drawing.Size(970, 384);
            this.XmlTB.TabIndex = 4;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(3, 103);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(970, 3);
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
            this.PduTB.Size = new System.Drawing.Size(970, 100);
            this.PduTB.TabIndex = 2;
            this.PduTB.Text = "C0018100080000010000FF0200";
            // 
            // DataTab
            // 
            this.DataTab.Controls.Add(this.DataXml);
            this.DataTab.Controls.Add(this.DataPdu);
            this.DataTab.Location = new System.Drawing.Point(4, 22);
            this.DataTab.Name = "DataTab";
            this.DataTab.Padding = new System.Windows.Forms.Padding(3);
            this.DataTab.Size = new System.Drawing.Size(976, 493);
            this.DataTab.TabIndex = 4;
            this.DataTab.Text = "Data to XML";
            this.DataTab.UseVisualStyleBackColor = true;
            // 
            // DataXml
            // 
            this.DataXml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataXml.Location = new System.Drawing.Point(171, 3);
            this.DataXml.MaxLength = 1073741823;
            this.DataXml.Multiline = true;
            this.DataXml.Name = "DataXml";
            this.DataXml.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.DataXml.Size = new System.Drawing.Size(802, 487);
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
            this.DataPdu.Size = new System.Drawing.Size(168, 487);
            this.DataPdu.TabIndex = 4;
            // 
            // DLMSTranslatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 590);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DLMSTranslatorForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gurux DLMS Translator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.MessagesTab.ResumeLayout(false);
            this.PduTab.ResumeLayout(false);
            this.PduTab.PerformLayout();
            this.DataTab.ResumeLayout(false);
            this.DataTab.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLbl;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar ProgressBar;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton TranslateBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileMnu;
        private System.Windows.Forms.ToolStripMenuItem TranslateMnu;
        private System.Windows.Forms.ToolStripMenuItem XmlToPduMnu;
        private System.Windows.Forms.ToolStripMenuItem PduToXmlMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ExitMnu;
        private System.Windows.Forms.ToolStripMenuItem ViewMnu;
        private System.Windows.Forms.ToolStripMenuItem PduOnlyMnu;
        private System.Windows.Forms.ToolStripMenuItem CompletePduMnu;
        private System.Windows.Forms.ToolStripMenuItem StandardMnu;
        private System.Windows.Forms.ToolStripMenuItem HexMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem RemoveDuplicatesMnu;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage MessagesTab;
        private System.Windows.Forms.TabPage PduTab;
        private System.Windows.Forms.TextBox XmlTB;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.TextBox PduTB;
        private System.Windows.Forms.TabPage DataTab;
        private System.Windows.Forms.TextBox DataXml;
        private System.Windows.Forms.TextBox DataPdu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem ConvertMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox InterfaceCb;
        private System.Windows.Forms.ToolStripLabel ShowLbl;
        private System.Windows.Forms.ToolStripComboBox ShowCb;
        private System.Windows.Forms.ToolStripLabel FollowmessagesLbl;
        private System.Windows.Forms.ToolStripComboBox FollowmessagesCb;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FindMnu;
        private System.Windows.Forms.ToolStripMenuItem FindNextMnu;
        private System.Windows.Forms.RichTextBox MessageXmlTB;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.RichTextBox MessagePduTB;
        private System.Windows.Forms.ToolStripMenuItem ShowMnu;
        private System.Windows.Forms.ToolStripMenuItem ViewFrameMnu;
        private System.Windows.Forms.ToolStripMenuItem XmlMnu;
        private System.Windows.Forms.ToolStripMenuItem FrameNumberMnu;
        private System.Windows.Forms.ToolStripMenuItem CancelTranslateBtn;
    }
}