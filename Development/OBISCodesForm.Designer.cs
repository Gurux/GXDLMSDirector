//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/OBISCodesForm.Designer.cs $
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
partial class OBISCodesForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OBISCodesForm));
        this.EditBtn = new System.Windows.Forms.Button();
        this.RemoveBtn = new System.Windows.Forms.Button();
        this.NewBtn = new System.Windows.Forms.Button();
        this.panel2 = new System.Windows.Forms.Panel();
        this.OBISCodesList = new System.Windows.Forms.ListView();
        this.NameCH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
        this.splitter1 = new System.Windows.Forms.Splitter();
        this.ManufacturersList = new System.Windows.Forms.ListView();
        this.ManufacturerNameCH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
        this.OKBtn = new System.Windows.Forms.Button();
        this.panel1 = new System.Windows.Forms.Panel();
        this.CancelBtn = new System.Windows.Forms.Button();
        this.panel2.SuspendLayout();
        this.panel1.SuspendLayout();
        this.SuspendLayout();
        //
        // EditBtn
        //
        this.EditBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.EditBtn.Location = new System.Drawing.Point(91, 287);
        this.EditBtn.Name = "EditBtn";
        this.EditBtn.Size = new System.Drawing.Size(75, 23);
        this.EditBtn.TabIndex = 9;
        this.EditBtn.Text = "Edit";
        this.EditBtn.UseVisualStyleBackColor = true;
        this.EditBtn.Click += new System.EventHandler(this.EditBtn_Click);
        //
        // RemoveBtn
        //
        this.RemoveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.RemoveBtn.Location = new System.Drawing.Point(170, 287);
        this.RemoveBtn.Name = "RemoveBtn";
        this.RemoveBtn.Size = new System.Drawing.Size(75, 23);
        this.RemoveBtn.TabIndex = 2;
        this.RemoveBtn.Text = "Remove";
        this.RemoveBtn.UseVisualStyleBackColor = true;
        this.RemoveBtn.Click += new System.EventHandler(this.RemoveBtn_Click);
        //
        // NewBtn
        //
        this.NewBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.NewBtn.Location = new System.Drawing.Point(10, 287);
        this.NewBtn.Name = "NewBtn";
        this.NewBtn.Size = new System.Drawing.Size(75, 23);
        this.NewBtn.TabIndex = 1;
        this.NewBtn.Text = "New";
        this.NewBtn.UseVisualStyleBackColor = true;
        this.NewBtn.Click += new System.EventHandler(this.NewBtn_Click);
        //
        // panel2
        //
        this.panel2.Controls.Add(this.OBISCodesList);
        this.panel2.Controls.Add(this.EditBtn);
        this.panel2.Controls.Add(this.RemoveBtn);
        this.panel2.Controls.Add(this.NewBtn);
        this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panel2.Location = new System.Drawing.Point(188, 0);
        this.panel2.Name = "panel2";
        this.panel2.Size = new System.Drawing.Size(257, 316);
        this.panel2.TabIndex = 18;
        //
        // OBISCodesList
        //
        this.OBISCodesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                     | System.Windows.Forms.AnchorStyles.Left)
                                     | System.Windows.Forms.AnchorStyles.Right)));
        this.OBISCodesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameCH
        });
        this.OBISCodesList.FullRowSelect = true;
        this.OBISCodesList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
        this.OBISCodesList.HideSelection = false;
        this.OBISCodesList.Location = new System.Drawing.Point(0, 0);
        this.OBISCodesList.MultiSelect = false;
        this.OBISCodesList.Name = "OBISCodesList";
        this.OBISCodesList.Size = new System.Drawing.Size(257, 281);
        this.OBISCodesList.TabIndex = 16;
        this.OBISCodesList.UseCompatibleStateImageBehavior = false;
        this.OBISCodesList.View = System.Windows.Forms.View.Details;
        this.OBISCodesList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.OBISCodesList_ItemSelectionChanged);
        this.OBISCodesList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OBISCodesList_MouseDoubleClick);
        //
        // NameCH
        //
        this.NameCH.Text = "Name";
        this.NameCH.Width = 253;
        //
        // splitter1
        //
        this.splitter1.Location = new System.Drawing.Point(185, 0);
        this.splitter1.Name = "splitter1";
        this.splitter1.Size = new System.Drawing.Size(3, 316);
        this.splitter1.TabIndex = 17;
        this.splitter1.TabStop = false;
        //
        // ManufacturersList
        //
        this.ManufacturersList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ManufacturerNameCH
        });
        this.ManufacturersList.Dock = System.Windows.Forms.DockStyle.Left;
        this.ManufacturersList.FullRowSelect = true;
        this.ManufacturersList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
        this.ManufacturersList.HideSelection = false;
        this.ManufacturersList.Location = new System.Drawing.Point(0, 0);
        this.ManufacturersList.MultiSelect = false;
        this.ManufacturersList.Name = "ManufacturersList";
        this.ManufacturersList.Size = new System.Drawing.Size(185, 316);
        this.ManufacturersList.TabIndex = 15;
        this.ManufacturersList.UseCompatibleStateImageBehavior = false;
        this.ManufacturersList.View = System.Windows.Forms.View.Details;
        //
        // ManufacturerNameCH
        //
        this.ManufacturerNameCH.Text = "Name";
        this.ManufacturerNameCH.Width = 50;
        //
        // OKBtn
        //
        this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.OKBtn.Location = new System.Drawing.Point(277, 11);
        this.OKBtn.Name = "OKBtn";
        this.OKBtn.Size = new System.Drawing.Size(75, 23);
        this.OKBtn.TabIndex = 7;
        this.OKBtn.Text = "&OK";
        this.OKBtn.UseVisualStyleBackColor = true;
        this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
        //
        // panel1
        //
        this.panel1.Controls.Add(this.OKBtn);
        this.panel1.Controls.Add(this.CancelBtn);
        this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panel1.Location = new System.Drawing.Point(0, 316);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(445, 46);
        this.panel1.TabIndex = 16;
        //
        // CancelBtn
        //
        this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.CancelBtn.Location = new System.Drawing.Point(358, 11);
        this.CancelBtn.Name = "CancelBtn";
        this.CancelBtn.Size = new System.Drawing.Size(75, 23);
        this.CancelBtn.TabIndex = 8;
        this.CancelBtn.Text = "&Cancel";
        this.CancelBtn.UseVisualStyleBackColor = true;
        //
        // OBISCodesForm
        //
        this.AcceptButton = this.OKBtn;
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.CancelBtn;
        this.ClientSize = new System.Drawing.Size(445, 362);
        this.Controls.Add(this.panel2);
        this.Controls.Add(this.splitter1);
        this.Controls.Add(this.ManufacturersList);
        this.Controls.Add(this.panel1);
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "OBISCodesForm";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "OBIS Codes";
        this.panel2.ResumeLayout(false);
        this.panel1.ResumeLayout(false);
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button EditBtn;
    private System.Windows.Forms.Button RemoveBtn;
    private System.Windows.Forms.Button NewBtn;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.ListView OBISCodesList;
    private System.Windows.Forms.Splitter splitter1;
    private System.Windows.Forms.ListView ManufacturersList;
    private System.Windows.Forms.Button OKBtn;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button CancelBtn;
    private System.Windows.Forms.ColumnHeader NameCH;
    private System.Windows.Forms.ColumnHeader ManufacturerNameCH;

}
}