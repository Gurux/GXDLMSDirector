namespace GXDLMSDirector
{
    partial class GXDataConcentratorUI
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDataConcentratorUI));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SearchBtn = new System.Windows.Forms.Button();
            this.SearchTb = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ReadCMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.AddDeviceCMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.Line1CMnu = new System.Windows.Forms.ToolStripSeparator();
            this.DeleteCMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.PropertiesCMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.Devices = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            this.imageList1.Images.SetKeyName(7, "DeviceProperty.bmp");
            this.imageList1.Images.SetKeyName(8, "DeviceTable.bmp");
            this.imageList1.Images.SetKeyName(9, "DeviceDirty.bmp");
            this.imageList1.Images.SetKeyName(10, "PropertyDirty.bmp");
            this.imageList1.Images.SetKeyName(11, "DeviceCategory.bmp");
            this.imageList1.Images.SetKeyName(12, "settings.bmp");
            // 
            // SearchBtn
            // 
            this.SearchBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchBtn.Location = new System.Drawing.Point(154, 3);
            this.SearchBtn.Name = "SearchBtn";
            this.SearchBtn.Size = new System.Drawing.Size(75, 23);
            this.SearchBtn.TabIndex = 1;
            this.SearchBtn.Text = "Search";
            this.SearchBtn.UseVisualStyleBackColor = true;
            this.SearchBtn.Click += new System.EventHandler(this.SearchBtn_Click);
            // 
            // SearchTb
            // 
            this.SearchTb.Location = new System.Drawing.Point(3, 3);
            this.SearchTb.Name = "SearchTb";
            this.SearchTb.Size = new System.Drawing.Size(145, 20);
            this.SearchTb.TabIndex = 2;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ReadCMnu,
            this.AddDeviceCMnu,
            this.Line1CMnu,
            this.DeleteCMnu,
            this.toolStripMenuItem5,
            this.PropertiesCMnu});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 126);
            // 
            // ReadCMnu
            // 
            this.ReadCMnu.Name = "ReadCMnu";
            this.ReadCMnu.Size = new System.Drawing.Size(180, 22);
            this.ReadCMnu.Text = "Read";
            this.ReadCMnu.Click += new System.EventHandler(this.ReadCMnu_Click);
            // 
            // AddDeviceCMnu
            // 
            this.AddDeviceCMnu.Name = "AddDeviceCMnu";
            this.AddDeviceCMnu.Size = new System.Drawing.Size(134, 22);
            this.AddDeviceCMnu.Text = "&Add Device";
            this.AddDeviceCMnu.Click += new System.EventHandler(this.AddDeviceCMnu_Click);
            // 
            // Line1CMnu
            // 
            this.Line1CMnu.Name = "Line1CMnu";
            this.Line1CMnu.Size = new System.Drawing.Size(131, 6);
            // 
            // DeleteCMnu
            // 
            this.DeleteCMnu.Name = "DeleteCMnu";
            this.DeleteCMnu.Size = new System.Drawing.Size(134, 22);
            this.DeleteCMnu.Text = "Delete";
            this.DeleteCMnu.Click += new System.EventHandler(this.DeleteCMnu_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(131, 6);
            // 
            // PropertiesCMnu
            // 
            this.PropertiesCMnu.Name = "PropertiesCMnu";
            this.PropertiesCMnu.Size = new System.Drawing.Size(134, 22);
            this.PropertiesCMnu.Text = "&Properties";
            this.PropertiesCMnu.Click += new System.EventHandler(this.PropertiesCMnu_Click);
            // 
            // Devices
            // 
            this.Devices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Devices.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Devices.ContextMenuStrip = this.contextMenuStrip1;
            this.Devices.FullRowSelect = true;
            this.Devices.HideSelection = false;
            this.Devices.ImageIndex = 0;
            this.Devices.ImageList = this.imageList1;
            this.Devices.Location = new System.Drawing.Point(3, 32);
            this.Devices.Name = "Devices";
            this.Devices.SelectedImageIndex = 0;
            this.Devices.Size = new System.Drawing.Size(226, 220);
            this.Devices.TabIndex = 4;
            // 
            // GXDataConcentratorUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.Devices);
            this.Controls.Add(this.SearchTb);
            this.Controls.Add(this.SearchBtn);
            this.Name = "GXDataConcentratorUI";
            this.Size = new System.Drawing.Size(232, 260);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button SearchBtn;
        private System.Windows.Forms.TextBox SearchTb;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ReadCMnu;
        private System.Windows.Forms.ToolStripMenuItem AddDeviceCMnu;
        private System.Windows.Forms.ToolStripSeparator Line1CMnu;
        private System.Windows.Forms.ToolStripMenuItem DeleteCMnu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem PropertiesCMnu;
        private System.Windows.Forms.TreeView Devices;
    }
}
