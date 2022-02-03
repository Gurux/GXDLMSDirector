namespace GXDLMSDirector.Macro
{
    partial class GXMacroDelayDlg
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
            this.DelayLbl = new System.Windows.Forms.Label();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.DelayTb = new System.Windows.Forms.DateTimePicker();
            this.NameTb = new System.Windows.Forms.TextBox();
            this.NameLbl = new System.Windows.Forms.Label();
            this.DescriptionLbl = new System.Windows.Forms.Label();
            this.DescriptionTb = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // DelayLbl
            // 
            this.DelayLbl.AutoSize = true;
            this.DelayLbl.Location = new System.Drawing.Point(12, 38);
            this.DelayLbl.Name = "DelayLbl";
            this.DelayLbl.Size = new System.Drawing.Size(37, 13);
            this.DelayLbl.TabIndex = 38;
            this.DelayLbl.Text = "Delay:";
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Location = new System.Drawing.Point(167, 146);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 39;
            this.OKBtn.Text = "&OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(248, 146);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 40;
            this.CancelBtn.Text = "&Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // DelayTb
            // 
            this.DelayTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DelayTb.CustomFormat = "HH:mm:ss";
            this.DelayTb.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DelayTb.Location = new System.Drawing.Point(95, 38);
            this.DelayTb.Name = "DelayTb";
            this.DelayTb.ShowUpDown = true;
            this.DelayTb.Size = new System.Drawing.Size(228, 20);
            this.DelayTb.TabIndex = 41;
            this.DelayTb.Value = new System.DateTime(2000, 1, 1, 0, 0, 1, 0);
            // 
            // NameTb
            // 
            this.NameTb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NameTb.Location = new System.Drawing.Point(95, 12);
            this.NameTb.Name = "NameTb";
            this.NameTb.Size = new System.Drawing.Size(228, 20);
            this.NameTb.TabIndex = 42;
            // 
            // NameLbl
            // 
            this.NameLbl.AutoSize = true;
            this.NameLbl.Location = new System.Drawing.Point(13, 14);
            this.NameLbl.Name = "NameLbl";
            this.NameLbl.Size = new System.Drawing.Size(38, 13);
            this.NameLbl.TabIndex = 43;
            this.NameLbl.Text = "Name:";
            // 
            // DescriptionLbl
            // 
            this.DescriptionLbl.AutoSize = true;
            this.DescriptionLbl.Location = new System.Drawing.Point(13, 67);
            this.DescriptionLbl.Name = "DescriptionLbl";
            this.DescriptionLbl.Size = new System.Drawing.Size(63, 13);
            this.DescriptionLbl.TabIndex = 48;
            this.DescriptionLbl.Text = "Description:";
            // 
            // DescriptionTb
            // 
            this.DescriptionTb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DescriptionTb.Location = new System.Drawing.Point(95, 64);
            this.DescriptionTb.Multiline = true;
            this.DescriptionTb.Name = "DescriptionTb";
            this.DescriptionTb.Size = new System.Drawing.Size(228, 76);
            this.DescriptionTb.TabIndex = 47;
            // 
            // GXMacroDelayDlg
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(335, 181);
            this.ControlBox = false;
            this.Controls.Add(this.DescriptionLbl);
            this.Controls.Add(this.DescriptionTb);
            this.Controls.Add(this.NameTb);
            this.Controls.Add(this.NameLbl);
            this.Controls.Add(this.DelayTb);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.DelayLbl);
            this.Name = "GXMacroDelayDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Macro delay";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label DelayLbl;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.DateTimePicker DelayTb;
        private System.Windows.Forms.TextBox NameTb;
        private System.Windows.Forms.Label NameLbl;
        private System.Windows.Forms.Label DescriptionLbl;
        private System.Windows.Forms.TextBox DescriptionTb;
    }
}