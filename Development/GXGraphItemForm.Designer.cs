namespace GXDLMSDirector
{
    partial class GXGraphItemForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXGraphItemForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.GraphItemList = new System.Windows.Forms.ListView();
            this.ManufacturerNameCH = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.GraphItemEditor = new System.Windows.Forms.PropertyGrid();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.OKBtn);
            this.panel1.Controls.Add(this.CancelBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 333);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(405, 46);
            this.panel1.TabIndex = 17;
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Location = new System.Drawing.Point(237, 11);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 7;
            this.OKBtn.Text = "&OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(318, 11);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 8;
            this.CancelBtn.Text = "&Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // GraphItemList
            // 
            this.GraphItemList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ManufacturerNameCH});
            this.GraphItemList.Dock = System.Windows.Forms.DockStyle.Left;
            this.GraphItemList.FullRowSelect = true;
            this.GraphItemList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.GraphItemList.HideSelection = false;
            this.GraphItemList.Location = new System.Drawing.Point(0, 0);
            this.GraphItemList.Name = "GraphItemList";
            this.GraphItemList.Size = new System.Drawing.Size(185, 333);
            this.GraphItemList.SmallImageList = this.imageList1;
            this.GraphItemList.TabIndex = 18;
            this.GraphItemList.UseCompatibleStateImageBehavior = false;
            this.GraphItemList.View = System.Windows.Forms.View.Details;
            this.GraphItemList.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.GraphItemList_DrawItem);
            this.GraphItemList.SelectedIndexChanged += new System.EventHandler(this.GraphItemList_SelectedIndexChanged);
            // 
            // ManufacturerNameCH
            // 
            this.ManufacturerNameCH.Text = "Name";
            this.ManufacturerNameCH.Width = 50;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList1.Images.SetKeyName(0, "GraphNone.bmp");
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(185, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 333);
            this.splitter1.TabIndex = 19;
            this.splitter1.TabStop = false;
            // 
            // GraphItemEditor
            // 
            this.GraphItemEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GraphItemEditor.HelpVisible = false;
            this.GraphItemEditor.Location = new System.Drawing.Point(188, 0);
            this.GraphItemEditor.Name = "GraphItemEditor";
            this.GraphItemEditor.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.GraphItemEditor.Size = new System.Drawing.Size(217, 333);
            this.GraphItemEditor.TabIndex = 20;
            this.GraphItemEditor.ToolbarVisible = false;
            this.GraphItemEditor.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.GraphItemEditor_PropertyValueChanged);
            this.GraphItemEditor.SizeChanged += new System.EventHandler(this.GraphItemEditor_SizeChanged);
            // 
            // GXGraphItemForm
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(405, 379);
            this.Controls.Add(this.GraphItemEditor);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.GraphItemList);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GXGraphItemForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Graph Items";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.ListView GraphItemList;
        private System.Windows.Forms.ColumnHeader ManufacturerNameCH;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.PropertyGrid GraphItemEditor;
        private System.Windows.Forms.ImageList imageList1;

    }
}