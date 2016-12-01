//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/GXObisValueItemForm.Designer.cs $
//
// Version:         $Revision: 8937 $,
//                  $Date: 2016-11-23 14:03:11 +0200 (ke, 23 marras 2016) $
//                  $Author: gurux01 $
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
partial class GXObisValueItemForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXObisValueItemForm));
        this.OKBtn = new System.Windows.Forms.Button();
        this.CancelBtn = new System.Windows.Forms.Button();
        this.DeviceValueTB = new System.Windows.Forms.TextBox();
        this.NameLbl = new System.Windows.Forms.Label();
        this.UIValueTB = new System.Windows.Forms.TextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.SuspendLayout();
        //
        // OKBtn
        //
        this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.OKBtn.Location = new System.Drawing.Point(111, 69);
        this.OKBtn.Name = "OKBtn";
        this.OKBtn.Size = new System.Drawing.Size(75, 23);
        this.OKBtn.TabIndex = 2;
        this.OKBtn.Text = "&OK";
        this.OKBtn.UseVisualStyleBackColor = true;
        this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
        //
        // CancelBtn
        //
        this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.CancelBtn.Location = new System.Drawing.Point(192, 69);
        this.CancelBtn.Name = "CancelBtn";
        this.CancelBtn.Size = new System.Drawing.Size(75, 23);
        this.CancelBtn.TabIndex = 3;
        this.CancelBtn.Text = "&Cancel";
        this.CancelBtn.UseVisualStyleBackColor = true;
        //
        // DeviceValueTB
        //
        this.DeviceValueTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                     | System.Windows.Forms.AnchorStyles.Right)));
        this.DeviceValueTB.Location = new System.Drawing.Point(92, 6);
        this.DeviceValueTB.Name = "DeviceValueTB";
        this.DeviceValueTB.Size = new System.Drawing.Size(175, 20);
        this.DeviceValueTB.TabIndex = 0;
        //
        // NameLbl
        //
        this.NameLbl.AutoSize = true;
        this.NameLbl.Location = new System.Drawing.Point(12, 9);
        this.NameLbl.Name = "NameLbl";
        this.NameLbl.Size = new System.Drawing.Size(74, 13);
        this.NameLbl.TabIndex = 17;
        this.NameLbl.Text = "Device Value:";
        //
        // UIValueTB
        //
        this.UIValueTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                 | System.Windows.Forms.AnchorStyles.Right)));
        this.UIValueTB.Location = new System.Drawing.Point(92, 32);
        this.UIValueTB.Name = "UIValueTB";
        this.UIValueTB.Size = new System.Drawing.Size(175, 20);
        this.UIValueTB.TabIndex = 1;
        //
        // label1
        //
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(12, 35);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(51, 13);
        this.label1.TabIndex = 19;
        this.label1.Text = "UI Value:";
        //
        // GXObisValueItemForm
        //
        this.AcceptButton = this.OKBtn;
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.CancelBtn;
        this.ClientSize = new System.Drawing.Size(279, 104);
        this.Controls.Add(this.UIValueTB);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.DeviceValueTB);
        this.Controls.Add(this.NameLbl);
        this.Controls.Add(this.OKBtn);
        this.Controls.Add(this.CancelBtn);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "GXObisValueItemForm";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "GXObisValueItemForm";
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button OKBtn;
    private System.Windows.Forms.Button CancelBtn;
    private System.Windows.Forms.TextBox DeviceValueTB;
    private System.Windows.Forms.Label NameLbl;
    private System.Windows.Forms.TextBox UIValueTB;
    private System.Windows.Forms.Label label1;
}
}