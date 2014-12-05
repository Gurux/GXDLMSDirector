//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GuruxClub/GXDLMSDirector/Development/OBISCodeForm.Designer.cs $
//
// Version:         $Revision: 5059 $,
//                  $Date: 2012-05-09 15:19:43 +0300 (ke, 09 touko 2012) $
//                  $Author: kurumi $
//
// Copyright (c) Gurux Ltd
//
//---------------------------------------------------------------------------
//
//  DESCRIPTION
//
// This file is a part of Gurux Device Framework.
//
// Gurux Device Framework is Open Source software; you can redistribute it
// and/or modify it under the terms of the GNU General Public License 
// as published by the Free Software Foundation; version 2 of the License.
// Gurux Device Framework is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details.
//
// More information of Gurux DLMS/COSEM Director: http://www.gurux.org/GXDLMSDirector
//
// This code is licensed under the GNU General Public License v2. 
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------

namespace GXDLMSDirector
{
    partial class AuthenticationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuthenticationForm));
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SecretTB = new System.Windows.Forms.TextBox();
            this.SecretLbl = new System.Windows.Forms.Label();
            this.HexRB = new System.Windows.Forms.RadioButton();
            this.AsciiRB = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Location = new System.Drawing.Point(154, 70);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 1;
            this.OKBtn.Text = "&OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(235, 70);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "&Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // SecretTB
            // 
            this.SecretTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SecretTB.Location = new System.Drawing.Point(92, 6);
            this.SecretTB.Name = "SecretTB";
            this.SecretTB.Size = new System.Drawing.Size(218, 20);
            this.SecretTB.TabIndex = 0;
            // 
            // SecretLbl
            // 
            this.SecretLbl.AutoSize = true;
            this.SecretLbl.Location = new System.Drawing.Point(12, 9);
            this.SecretLbl.Name = "SecretLbl";
            this.SecretLbl.Size = new System.Drawing.Size(41, 13);
            this.SecretLbl.TabIndex = 21;
            this.SecretLbl.Text = "Secret:";
            // 
            // HexRB
            // 
            this.HexRB.AutoSize = true;
            this.HexRB.Checked = true;
            this.HexRB.Location = new System.Drawing.Point(24, 32);
            this.HexRB.Name = "HexRB";
            this.HexRB.Size = new System.Drawing.Size(44, 17);
            this.HexRB.TabIndex = 22;
            this.HexRB.TabStop = true;
            this.HexRB.Text = "Hex";
            this.HexRB.UseVisualStyleBackColor = true;
            this.HexRB.CheckedChanged += new System.EventHandler(this.OnChange);
            // 
            // AsciiRB
            // 
            this.AsciiRB.AutoSize = true;
            this.AsciiRB.Location = new System.Drawing.Point(24, 55);
            this.AsciiRB.Name = "AsciiRB";
            this.AsciiRB.Size = new System.Drawing.Size(52, 17);
            this.AsciiRB.TabIndex = 23;
            this.AsciiRB.Text = "ASCII";
            this.AsciiRB.UseVisualStyleBackColor = true;
            // 
            // AuthenticationSHAForm
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(322, 105);
            this.Controls.Add(this.AsciiRB);
            this.Controls.Add(this.HexRB);
            this.Controls.Add(this.SecretTB);
            this.Controls.Add(this.SecretLbl);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.CancelBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AuthenticationSHAForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SHA Authentication Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.TextBox SecretTB;
        private System.Windows.Forms.Label SecretLbl;
        private System.Windows.Forms.RadioButton HexRB;
        private System.Windows.Forms.RadioButton AsciiRB;
    }
}