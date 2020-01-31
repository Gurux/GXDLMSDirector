namespace GXDLMSDirector
{
    partial class GXSettingsDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXSettingsDlg));
            this.panel1 = new System.Windows.Forms.Panel();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.NotificationsTab = new System.Windows.Forms.TabPage();
            this.NotificationsView = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.BlockCipherKeyAsciiCb = new System.Windows.Forms.CheckBox();
            this.SystemTitleAsciiCb = new System.Windows.Forms.CheckBox();
            this.SystemTitleLbl = new System.Windows.Forms.Label();
            this.SystemTitleTB = new System.Windows.Forms.TextBox();
            this.BlockCipherKeyLbl = new System.Windows.Forms.Label();
            this.BlockCipherKeyTB = new System.Windows.Forms.TextBox();
            this.ExternalMediasTab = new System.Windows.Forms.TabPage();
            this.MediaList = new System.Windows.Forms.ListView();
            this.MediaNameCH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VersionCH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PathCH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NotificationMnu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.NotificationCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.panel2 = new System.Windows.Forms.Panel();
            this.CheckUpdatesBtn = new System.Windows.Forms.Button();
            this.RemoveBtn = new System.Windows.Forms.Button();
            this.AddBtn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.Tabs.SuspendLayout();
            this.NotificationsTab.SuspendLayout();
            this.panel3.SuspendLayout();
            this.ExternalMediasTab.SuspendLayout();
            this.NotificationMnu.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.OKBtn);
            this.panel1.Controls.Add(this.CancelBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 280);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(399, 39);
            this.panel1.TabIndex = 1;
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Location = new System.Drawing.Point(231, 6);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 11;
            this.OKBtn.Text = "&OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(312, 6);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 12;
            this.CancelBtn.Text = "&Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.NotificationsTab);
            this.Tabs.Controls.Add(this.ExternalMediasTab);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Location = new System.Drawing.Point(0, 0);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(399, 280);
            this.Tabs.TabIndex = 2;
            // 
            // NotificationsTab
            // 
            this.NotificationsTab.Controls.Add(this.NotificationsView);
            this.NotificationsTab.Controls.Add(this.panel3);
            this.NotificationsTab.Location = new System.Drawing.Point(4, 22);
            this.NotificationsTab.Name = "NotificationsTab";
            this.NotificationsTab.Padding = new System.Windows.Forms.Padding(3);
            this.NotificationsTab.Size = new System.Drawing.Size(391, 254);
            this.NotificationsTab.TabIndex = 1;
            this.NotificationsTab.Text = "Notifications";
            this.NotificationsTab.UseVisualStyleBackColor = true;
            // 
            // NotificationsView
            // 
            this.NotificationsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NotificationsView.Location = new System.Drawing.Point(3, 73);
            this.NotificationsView.Name = "NotificationsView";
            this.NotificationsView.Size = new System.Drawing.Size(385, 178);
            this.NotificationsView.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.BlockCipherKeyAsciiCb);
            this.panel3.Controls.Add(this.SystemTitleAsciiCb);
            this.panel3.Controls.Add(this.SystemTitleLbl);
            this.panel3.Controls.Add(this.SystemTitleTB);
            this.panel3.Controls.Add(this.BlockCipherKeyLbl);
            this.panel3.Controls.Add(this.BlockCipherKeyTB);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(385, 70);
            this.panel3.TabIndex = 0;
            // 
            // BlockCipherKeyAsciiCb
            // 
            this.BlockCipherKeyAsciiCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BlockCipherKeyAsciiCb.AutoSize = true;
            this.BlockCipherKeyAsciiCb.Location = new System.Drawing.Point(321, 35);
            this.BlockCipherKeyAsciiCb.Name = "BlockCipherKeyAsciiCb";
            this.BlockCipherKeyAsciiCb.Size = new System.Drawing.Size(53, 17);
            this.BlockCipherKeyAsciiCb.TabIndex = 6;
            this.BlockCipherKeyAsciiCb.Text = "ASCII";
            this.BlockCipherKeyAsciiCb.UseVisualStyleBackColor = true;
            this.BlockCipherKeyAsciiCb.CheckedChanged += new System.EventHandler(this.BlockCipherKeyAsciiCb_CheckedChanged);
            // 
            // SystemTitleAsciiCb
            // 
            this.SystemTitleAsciiCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SystemTitleAsciiCb.AutoSize = true;
            this.SystemTitleAsciiCb.Location = new System.Drawing.Point(321, 8);
            this.SystemTitleAsciiCb.Name = "SystemTitleAsciiCb";
            this.SystemTitleAsciiCb.Size = new System.Drawing.Size(53, 17);
            this.SystemTitleAsciiCb.TabIndex = 5;
            this.SystemTitleAsciiCb.Text = "ASCII";
            this.SystemTitleAsciiCb.UseVisualStyleBackColor = true;
            this.SystemTitleAsciiCb.CheckedChanged += new System.EventHandler(this.SystemTitleAsciiCb_CheckedChanged);
            // 
            // SystemTitleLbl
            // 
            this.SystemTitleLbl.AutoSize = true;
            this.SystemTitleLbl.Location = new System.Drawing.Point(5, 10);
            this.SystemTitleLbl.Name = "SystemTitleLbl";
            this.SystemTitleLbl.Size = new System.Drawing.Size(67, 13);
            this.SystemTitleLbl.TabIndex = 3;
            this.SystemTitleLbl.Text = "System Title:";
            // 
            // SystemTitleTB
            // 
            this.SystemTitleTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SystemTitleTB.Location = new System.Drawing.Point(100, 7);
            this.SystemTitleTB.Name = "SystemTitleTB";
            this.SystemTitleTB.Size = new System.Drawing.Size(215, 20);
            this.SystemTitleTB.TabIndex = 2;
            // 
            // BlockCipherKeyLbl
            // 
            this.BlockCipherKeyLbl.AutoSize = true;
            this.BlockCipherKeyLbl.Location = new System.Drawing.Point(5, 36);
            this.BlockCipherKeyLbl.Name = "BlockCipherKeyLbl";
            this.BlockCipherKeyLbl.Size = new System.Drawing.Size(91, 13);
            this.BlockCipherKeyLbl.TabIndex = 1;
            this.BlockCipherKeyLbl.Text = "Block Cipher Key:";
            // 
            // BlockCipherKeyTB
            // 
            this.BlockCipherKeyTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BlockCipherKeyTB.Location = new System.Drawing.Point(101, 32);
            this.BlockCipherKeyTB.Name = "BlockCipherKeyTB";
            this.BlockCipherKeyTB.Size = new System.Drawing.Size(214, 20);
            this.BlockCipherKeyTB.TabIndex = 0;
            // 
            // ExternalMediasTab
            // 
            this.ExternalMediasTab.Controls.Add(this.MediaList);
            this.ExternalMediasTab.Controls.Add(this.panel2);
            this.ExternalMediasTab.Location = new System.Drawing.Point(4, 22);
            this.ExternalMediasTab.Name = "ExternalMediasTab";
            this.ExternalMediasTab.Padding = new System.Windows.Forms.Padding(3);
            this.ExternalMediasTab.Size = new System.Drawing.Size(391, 254);
            this.ExternalMediasTab.TabIndex = 2;
            this.ExternalMediasTab.Text = "External Medias";
            this.ExternalMediasTab.UseVisualStyleBackColor = true;
            // 
            // MediaList
            // 
            this.MediaList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MediaNameCH,
            this.VersionCH,
            this.PathCH});
            this.MediaList.ContextMenuStrip = this.NotificationMnu;
            this.MediaList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MediaList.FullRowSelect = true;
            this.MediaList.HideSelection = false;
            this.MediaList.Location = new System.Drawing.Point(3, 3);
            this.MediaList.MultiSelect = false;
            this.MediaList.Name = "MediaList";
            this.MediaList.Size = new System.Drawing.Size(385, 209);
            this.MediaList.TabIndex = 23;
            this.MediaList.UseCompatibleStateImageBehavior = false;
            this.MediaList.View = System.Windows.Forms.View.Details;
            // 
            // MediaNameCH
            // 
            this.MediaNameCH.Text = "Name";
            this.MediaNameCH.Width = 72;
            // 
            // VersionCH
            // 
            this.VersionCH.Text = "Version";
            this.VersionCH.Width = 72;
            // 
            // PathCH
            // 
            this.PathCH.Text = "Path";
            this.PathCH.Width = 172;
            // 
            // NotificationMnu
            // 
            this.NotificationMnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NotificationCopy,
            this.toolStripSeparator7});
            this.NotificationMnu.Name = "contextMenuStrip1";
            this.NotificationMnu.Size = new System.Drawing.Size(103, 32);
            // 
            // NotificationCopy
            // 
            this.NotificationCopy.Name = "NotificationCopy";
            this.NotificationCopy.Size = new System.Drawing.Size(102, 22);
            this.NotificationCopy.Text = "Copy";
            this.NotificationCopy.Click += new System.EventHandler(this.NotificationCopy_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(99, 6);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.CheckUpdatesBtn);
            this.panel2.Controls.Add(this.RemoveBtn);
            this.panel2.Controls.Add(this.AddBtn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 212);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(385, 39);
            this.panel2.TabIndex = 22;
            // 
            // CheckUpdatesBtn
            // 
            this.CheckUpdatesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CheckUpdatesBtn.Location = new System.Drawing.Point(5, 6);
            this.CheckUpdatesBtn.Name = "CheckUpdatesBtn";
            this.CheckUpdatesBtn.Size = new System.Drawing.Size(75, 23);
            this.CheckUpdatesBtn.TabIndex = 28;
            this.CheckUpdatesBtn.Text = "Check updates";
            this.CheckUpdatesBtn.UseVisualStyleBackColor = true;
            this.CheckUpdatesBtn.Click += new System.EventHandler(this.CheckUpdatesBtn_Click);
            // 
            // RemoveBtn
            // 
            this.RemoveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoveBtn.Location = new System.Drawing.Point(305, 6);
            this.RemoveBtn.Name = "RemoveBtn";
            this.RemoveBtn.Size = new System.Drawing.Size(75, 23);
            this.RemoveBtn.TabIndex = 27;
            this.RemoveBtn.Text = "Remove";
            this.RemoveBtn.UseVisualStyleBackColor = true;
            this.RemoveBtn.Click += new System.EventHandler(this.RemoveBtn_Click);
            // 
            // AddBtn
            // 
            this.AddBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddBtn.Location = new System.Drawing.Point(224, 6);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(75, 23);
            this.AddBtn.TabIndex = 26;
            this.AddBtn.Text = "Add...";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // GXSettingsDlg
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(399, 319);
            this.Controls.Add(this.Tabs);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GXSettingsDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GXDLMSDirector settings";
            this.panel1.ResumeLayout(false);
            this.Tabs.ResumeLayout(false);
            this.NotificationsTab.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ExternalMediasTab.ResumeLayout(false);
            this.NotificationMnu.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage NotificationsTab;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.TabPage ExternalMediasTab;
        private System.Windows.Forms.ListView MediaList;
        private System.Windows.Forms.ColumnHeader MediaNameCH;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button CheckUpdatesBtn;
        private System.Windows.Forms.Button RemoveBtn;
        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.ColumnHeader VersionCH;
        private System.Windows.Forms.ColumnHeader PathCH;
        private System.Windows.Forms.ContextMenuStrip NotificationMnu;
        private System.Windows.Forms.ToolStripMenuItem NotificationCopy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel NotificationsView;
        private System.Windows.Forms.Label BlockCipherKeyLbl;
        private System.Windows.Forms.TextBox BlockCipherKeyTB;
        private System.Windows.Forms.Label SystemTitleLbl;
        private System.Windows.Forms.TextBox SystemTitleTB;
        private System.Windows.Forms.CheckBox BlockCipherKeyAsciiCb;
        private System.Windows.Forms.CheckBox SystemTitleAsciiCb;
    }
}