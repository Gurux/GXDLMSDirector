namespace GXDLMSDirector
{
partial class AuthenticationGmacForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuthenticationGmacForm));
        this.SystemTitleTB = new System.Windows.Forms.TextBox();
        this.SecurityLbl = new System.Windows.Forms.Label();
        this.OKBtn = new System.Windows.Forms.Button();
        this.CancelBtn = new System.Windows.Forms.Button();
        this.SystemtitleLbl = new System.Windows.Forms.Label();
        this.BlockCipherKeyTB = new System.Windows.Forms.TextBox();
        this.BlockCipherKeyLbl = new System.Windows.Forms.Label();
        this.AuthenticationKeyTB = new System.Windows.Forms.TextBox();
        this.AuthenticationKeyLbl = new System.Windows.Forms.Label();
        this.SecurityCB = new System.Windows.Forms.ComboBox();
        this.HexRB = new System.Windows.Forms.RadioButton();
        this.AsciiRB = new System.Windows.Forms.RadioButton();
        this.SuspendLayout();
        //
        // SystemTitleTB
        //
        this.SystemTitleTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                     | System.Windows.Forms.AnchorStyles.Right)));
        this.SystemTitleTB.Location = new System.Drawing.Point(110, 31);
        this.SystemTitleTB.Name = "SystemTitleTB";
        this.SystemTitleTB.Size = new System.Drawing.Size(226, 20);
        this.SystemTitleTB.TabIndex = 1;
        this.SystemTitleTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ValidateHex);
        //
        // SecurityLbl
        //
        this.SecurityLbl.AutoSize = true;
        this.SecurityLbl.Location = new System.Drawing.Point(5, 9);
        this.SecurityLbl.Name = "SecurityLbl";
        this.SecurityLbl.Size = new System.Drawing.Size(48, 13);
        this.SecurityLbl.TabIndex = 27;
        this.SecurityLbl.Text = "Security:";
        //
        // OKBtn
        //
        this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.OKBtn.Location = new System.Drawing.Point(180, 124);
        this.OKBtn.Name = "OKBtn";
        this.OKBtn.Size = new System.Drawing.Size(75, 23);
        this.OKBtn.TabIndex = 4;
        this.OKBtn.Text = "&OK";
        this.OKBtn.UseVisualStyleBackColor = true;
        this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
        //
        // CancelBtn
        //
        this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.CancelBtn.Location = new System.Drawing.Point(261, 124);
        this.CancelBtn.Name = "CancelBtn";
        this.CancelBtn.Size = new System.Drawing.Size(75, 23);
        this.CancelBtn.TabIndex = 5;
        this.CancelBtn.Text = "&Cancel";
        this.CancelBtn.UseVisualStyleBackColor = true;
        //
        // SystemtitleLbl
        //
        this.SystemtitleLbl.AutoSize = true;
        this.SystemtitleLbl.Location = new System.Drawing.Point(5, 35);
        this.SystemtitleLbl.Name = "SystemtitleLbl";
        this.SystemtitleLbl.Size = new System.Drawing.Size(63, 13);
        this.SystemtitleLbl.TabIndex = 31;
        this.SystemtitleLbl.Text = "System title:";
        //
        // BlockCipherKeyTB
        //
        this.BlockCipherKeyTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                        | System.Windows.Forms.AnchorStyles.Right)));
        this.BlockCipherKeyTB.Location = new System.Drawing.Point(110, 58);
        this.BlockCipherKeyTB.Name = "BlockCipherKeyTB";
        this.BlockCipherKeyTB.Size = new System.Drawing.Size(226, 20);
        this.BlockCipherKeyTB.TabIndex = 2;
        this.BlockCipherKeyTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ValidateHex);
        //
        // BlockCipherKeyLbl
        //
        this.BlockCipherKeyLbl.AutoSize = true;
        this.BlockCipherKeyLbl.Location = new System.Drawing.Point(5, 61);
        this.BlockCipherKeyLbl.Name = "BlockCipherKeyLbl";
        this.BlockCipherKeyLbl.Size = new System.Drawing.Size(91, 13);
        this.BlockCipherKeyLbl.TabIndex = 33;
        this.BlockCipherKeyLbl.Text = "Block Cipher Key:";
        //
        // AuthenticationKeyTB
        //
        this.AuthenticationKeyTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                           | System.Windows.Forms.AnchorStyles.Right)));
        this.AuthenticationKeyTB.Location = new System.Drawing.Point(110, 84);
        this.AuthenticationKeyTB.Name = "AuthenticationKeyTB";
        this.AuthenticationKeyTB.Size = new System.Drawing.Size(226, 20);
        this.AuthenticationKeyTB.TabIndex = 3;
        this.AuthenticationKeyTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ValidateHex);
        //
        // AuthenticationKeyLbl
        //
        this.AuthenticationKeyLbl.AutoSize = true;
        this.AuthenticationKeyLbl.Location = new System.Drawing.Point(5, 87);
        this.AuthenticationKeyLbl.Name = "AuthenticationKeyLbl";
        this.AuthenticationKeyLbl.Size = new System.Drawing.Size(99, 13);
        this.AuthenticationKeyLbl.TabIndex = 35;
        this.AuthenticationKeyLbl.Text = "Authentication Key:";
        //
        // SecurityCB
        //
        this.SecurityCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.SecurityCB.FormattingEnabled = true;
        this.SecurityCB.Location = new System.Drawing.Point(110, 6);
        this.SecurityCB.Name = "SecurityCB";
        this.SecurityCB.Size = new System.Drawing.Size(226, 21);
        this.SecurityCB.TabIndex = 0;
        this.SecurityCB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.SecurityCB_DrawItem);
        //
        // HexRB
        //
        this.HexRB.AutoSize = true;
        this.HexRB.Checked = true;
        this.HexRB.Location = new System.Drawing.Point(12, 113);
        this.HexRB.Name = "HexRB";
        this.HexRB.Size = new System.Drawing.Size(44, 17);
        this.HexRB.TabIndex = 28;
        this.HexRB.TabStop = true;
        this.HexRB.Text = "Hex";
        this.HexRB.UseVisualStyleBackColor = true;
        //
        // AsciiRB
        //
        this.AsciiRB.AutoSize = true;
        this.AsciiRB.Location = new System.Drawing.Point(12, 136);
        this.AsciiRB.Name = "AsciiRB";
        this.AsciiRB.Size = new System.Drawing.Size(52, 17);
        this.AsciiRB.TabIndex = 29;
        this.AsciiRB.Text = "ASCII";
        this.AsciiRB.UseVisualStyleBackColor = true;
        this.AsciiRB.CheckedChanged += new System.EventHandler(this.AsciiRB_CheckedChanged);
        //
        // AuthenticationGmacForm
        //
        this.AcceptButton = this.OKBtn;
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.CancelBtn;
        this.ClientSize = new System.Drawing.Size(348, 159);
        this.Controls.Add(this.SecurityCB);
        this.Controls.Add(this.AuthenticationKeyTB);
        this.Controls.Add(this.AuthenticationKeyLbl);
        this.Controls.Add(this.BlockCipherKeyTB);
        this.Controls.Add(this.BlockCipherKeyLbl);
        this.Controls.Add(this.SystemtitleLbl);
        this.Controls.Add(this.AsciiRB);
        this.Controls.Add(this.HexRB);
        this.Controls.Add(this.SystemTitleTB);
        this.Controls.Add(this.SecurityLbl);
        this.Controls.Add(this.OKBtn);
        this.Controls.Add(this.CancelBtn);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "AuthenticationGmacForm";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "AuthenticationGmacForm";
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox SystemTitleTB;
    private System.Windows.Forms.Label SecurityLbl;
    private System.Windows.Forms.Button OKBtn;
    private System.Windows.Forms.Button CancelBtn;
    private System.Windows.Forms.Label SystemtitleLbl;
    private System.Windows.Forms.TextBox BlockCipherKeyTB;
    private System.Windows.Forms.Label BlockCipherKeyLbl;
    private System.Windows.Forms.TextBox AuthenticationKeyTB;
    private System.Windows.Forms.Label AuthenticationKeyLbl;
    private System.Windows.Forms.ComboBox SecurityCB;
    private System.Windows.Forms.RadioButton HexRB;
    private System.Windows.Forms.RadioButton AsciiRB;
}
}