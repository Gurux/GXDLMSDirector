//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
//
// Version:         $Revision: 10624 $,
//                  $Date: 2019-04-24 13:56:09 +0300 (Wed, 24 Apr 2019) $
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
// More information of Gurux DLMS/COSEM Director: https://www.gurux.org/GXDLMSDirector
//
// This code is licensed under the GNU General Public License v2. 
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------

namespace GXDLMSDirector
{
    partial class ManufacturerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManufacturerForm));
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.UseIEC47CB = new System.Windows.Forms.CheckBox();
            this.UseLNCB = new System.Windows.Forms.CheckBox();
            this.ManufacturerIdTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NameTB = new System.Windows.Forms.TextBox();
            this.NameLbl = new System.Windows.Forms.Label();
            this.StartProtocolLbl = new System.Windows.Forms.Label();
            this.StartProtocolCB = new System.Windows.Forms.ComboBox();
            this.AddressingGB = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ClientAddLbl = new System.Windows.Forms.Label();
            this.ClientAddTB = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SizeCb = new System.Windows.Forms.ComboBox();
            this.SizeLbl = new System.Windows.Forms.Label();
            this.ServerAddressTypeCB = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SerialNumberFormulaTB = new System.Windows.Forms.TextBox();
            this.SerialNumberFormulaLbl = new System.Windows.Forms.Label();
            this.PhysicalServerAddLbl = new System.Windows.Forms.Label();
            this.PhysicalServerAddTB = new System.Windows.Forms.NumericUpDown();
            this.LogicalServerAddTB = new System.Windows.Forms.NumericUpDown();
            this.LogicalServerAddLbl = new System.Windows.Forms.Label();
            this.AuthenticationCB = new System.Windows.Forms.ComboBox();
            this.AuthenticationLbl = new System.Windows.Forms.Label();
            this.WebAddressTB = new System.Windows.Forms.TextBox();
            this.WebAddressLbl = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.BlockCipherKeyAsciiCb = new System.Windows.Forms.CheckBox();
            this.AuthenticationKeyAsciiCb = new System.Windows.Forms.CheckBox();
            this.SystemTitleAsciiCb = new System.Windows.Forms.CheckBox();
            this.SecurityCB = new System.Windows.Forms.ComboBox();
            this.AuthenticationKeyTB = new System.Windows.Forms.TextBox();
            this.BlockCipherKeyTB = new System.Windows.Forms.TextBox();
            this.SystemTitleTB = new System.Windows.Forms.TextBox();
            this.AuthenticationKeyLbl = new System.Windows.Forms.Label();
            this.BlockCipherKeyLbl = new System.Windows.Forms.Label();
            this.SystemtitleLbl = new System.Windows.Forms.Label();
            this.SecurityLbl = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.InfoTB = new System.Windows.Forms.TextBox();
            this.StandardCb = new System.Windows.Forms.ComboBox();
            this.StandardLbl = new System.Windows.Forms.Label();
            this.AddressingGB.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClientAddTB)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PhysicalServerAddTB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogicalServerAddTB)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Location = new System.Drawing.Point(247, 404);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 19;
            this.OKBtn.Text = "&OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(328, 404);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 20;
            this.CancelBtn.Text = "&Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // UseIEC47CB
            // 
            this.UseIEC47CB.AutoSize = true;
            this.UseIEC47CB.Location = new System.Drawing.Point(196, 78);
            this.UseIEC47CB.Name = "UseIEC47CB";
            this.UseIEC47CB.Size = new System.Drawing.Size(103, 17);
            this.UseIEC47CB.TabIndex = 3;
            this.UseIEC47CB.Text = "Use WRAPPER";
            this.UseIEC47CB.UseVisualStyleBackColor = true;
            // 
            // UseLNCB
            // 
            this.UseLNCB.AutoSize = true;
            this.UseLNCB.Location = new System.Drawing.Point(10, 78);
            this.UseLNCB.Name = "UseLNCB";
            this.UseLNCB.Size = new System.Drawing.Size(167, 17);
            this.UseLNCB.TabIndex = 2;
            this.UseLNCB.Text = "Use Logical name referencing";
            this.UseLNCB.UseVisualStyleBackColor = true;
            // 
            // ManufacturerIdTB
            // 
            this.ManufacturerIdTB.Location = new System.Drawing.Point(65, 4);
            this.ManufacturerIdTB.Name = "ManufacturerIdTB";
            this.ManufacturerIdTB.Size = new System.Drawing.Size(58, 20);
            this.ManufacturerIdTB.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Flag ID:";
            // 
            // NameTB
            // 
            this.NameTB.Location = new System.Drawing.Point(183, 2);
            this.NameTB.Name = "NameTB";
            this.NameTB.Size = new System.Drawing.Size(140, 20);
            this.NameTB.TabIndex = 0;
            // 
            // NameLbl
            // 
            this.NameLbl.AutoSize = true;
            this.NameLbl.Location = new System.Drawing.Point(139, 9);
            this.NameLbl.Name = "NameLbl";
            this.NameLbl.Size = new System.Drawing.Size(38, 13);
            this.NameLbl.TabIndex = 15;
            this.NameLbl.Text = "Name:";
            // 
            // StartProtocolLbl
            // 
            this.StartProtocolLbl.AutoSize = true;
            this.StartProtocolLbl.Location = new System.Drawing.Point(10, 103);
            this.StartProtocolLbl.Name = "StartProtocolLbl";
            this.StartProtocolLbl.Size = new System.Drawing.Size(74, 13);
            this.StartProtocolLbl.TabIndex = 20;
            this.StartProtocolLbl.Text = "Start Protocol:";
            // 
            // StartProtocolCB
            // 
            this.StartProtocolCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.StartProtocolCB.FormattingEnabled = true;
            this.StartProtocolCB.Location = new System.Drawing.Point(119, 100);
            this.StartProtocolCB.Name = "StartProtocolCB";
            this.StartProtocolCB.Size = new System.Drawing.Size(194, 21);
            this.StartProtocolCB.TabIndex = 4;
            // 
            // AddressingGB
            // 
            this.AddressingGB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddressingGB.Controls.Add(this.groupBox2);
            this.AddressingGB.Controls.Add(this.groupBox1);
            this.AddressingGB.Controls.Add(this.AuthenticationCB);
            this.AddressingGB.Controls.Add(this.AuthenticationLbl);
            this.AddressingGB.Location = new System.Drawing.Point(3, 124);
            this.AddressingGB.Name = "AddressingGB";
            this.AddressingGB.Size = new System.Drawing.Size(395, 246);
            this.AddressingGB.TabIndex = 5;
            this.AddressingGB.TabStop = false;
            this.AddressingGB.Text = "Addressing";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ClientAddLbl);
            this.groupBox2.Controls.Add(this.ClientAddTB);
            this.groupBox2.Location = new System.Drawing.Point(2, 40);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(331, 43);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Client Address:";
            // 
            // ClientAddLbl
            // 
            this.ClientAddLbl.AutoSize = true;
            this.ClientAddLbl.Location = new System.Drawing.Point(9, 19);
            this.ClientAddLbl.Name = "ClientAddLbl";
            this.ClientAddLbl.Size = new System.Drawing.Size(74, 13);
            this.ClientAddLbl.TabIndex = 42;
            this.ClientAddLbl.Text = "Address: (hex)";
            // 
            // ClientAddTB
            // 
            this.ClientAddTB.Hexadecimal = true;
            this.ClientAddTB.Location = new System.Drawing.Point(100, 16);
            this.ClientAddTB.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.ClientAddTB.Name = "ClientAddTB";
            this.ClientAddTB.Size = new System.Drawing.Size(85, 20);
            this.ClientAddTB.TabIndex = 6;
            this.ClientAddTB.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SizeCb);
            this.groupBox1.Controls.Add(this.SizeLbl);
            this.groupBox1.Controls.Add(this.ServerAddressTypeCB);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.SerialNumberFormulaTB);
            this.groupBox1.Controls.Add(this.SerialNumberFormulaLbl);
            this.groupBox1.Controls.Add(this.PhysicalServerAddLbl);
            this.groupBox1.Controls.Add(this.PhysicalServerAddTB);
            this.groupBox1.Controls.Add(this.LogicalServerAddTB);
            this.groupBox1.Controls.Add(this.LogicalServerAddLbl);
            this.groupBox1.Location = new System.Drawing.Point(2, 81);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(331, 158);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server Address:";
            // 
            // SizeCb
            // 
            this.SizeCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SizeCb.FormattingEnabled = true;
            this.SizeCb.Location = new System.Drawing.Point(63, 100);
            this.SizeCb.Name = "SizeCb";
            this.SizeCb.Size = new System.Drawing.Size(85, 21);
            this.SizeCb.TabIndex = 11;
            // 
            // SizeLbl
            // 
            this.SizeLbl.AutoSize = true;
            this.SizeLbl.Location = new System.Drawing.Point(9, 104);
            this.SizeLbl.Name = "SizeLbl";
            this.SizeLbl.Size = new System.Drawing.Size(30, 13);
            this.SizeLbl.TabIndex = 49;
            this.SizeLbl.Text = "Size:";
            // 
            // ServerAddressTypeCB
            // 
            this.ServerAddressTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ServerAddressTypeCB.FormattingEnabled = true;
            this.ServerAddressTypeCB.Location = new System.Drawing.Point(97, 19);
            this.ServerAddressTypeCB.Name = "ServerAddressTypeCB";
            this.ServerAddressTypeCB.Size = new System.Drawing.Size(214, 21);
            this.ServerAddressTypeCB.TabIndex = 7;
            this.ServerAddressTypeCB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ServerAddressTypeCB_DrawItem);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 39;
            this.label3.Text = "Address Type:";
            // 
            // SerialNumberFormulaTB
            // 
            this.SerialNumberFormulaTB.Location = new System.Drawing.Point(143, 48);
            this.SerialNumberFormulaTB.Name = "SerialNumberFormulaTB";
            this.SerialNumberFormulaTB.Size = new System.Drawing.Size(173, 20);
            this.SerialNumberFormulaTB.TabIndex = 8;
            // 
            // SerialNumberFormulaLbl
            // 
            this.SerialNumberFormulaLbl.AutoSize = true;
            this.SerialNumberFormulaLbl.Location = new System.Drawing.Point(5, 48);
            this.SerialNumberFormulaLbl.Name = "SerialNumberFormulaLbl";
            this.SerialNumberFormulaLbl.Size = new System.Drawing.Size(116, 13);
            this.SerialNumberFormulaLbl.TabIndex = 47;
            this.SerialNumberFormulaLbl.Text = "Serial Number Formula:";
            // 
            // PhysicalServerAddLbl
            // 
            this.PhysicalServerAddLbl.AutoSize = true;
            this.PhysicalServerAddLbl.Location = new System.Drawing.Point(8, 74);
            this.PhysicalServerAddLbl.Name = "PhysicalServerAddLbl";
            this.PhysicalServerAddLbl.Size = new System.Drawing.Size(49, 13);
            this.PhysicalServerAddLbl.TabIndex = 42;
            this.PhysicalServerAddLbl.Text = "Physical:";
            // 
            // PhysicalServerAddTB
            // 
            this.PhysicalServerAddTB.Hexadecimal = true;
            this.PhysicalServerAddTB.Location = new System.Drawing.Point(63, 74);
            this.PhysicalServerAddTB.Maximum = new decimal(new int[] {
            0,
            1,
            0,
            0});
            this.PhysicalServerAddTB.Name = "PhysicalServerAddTB";
            this.PhysicalServerAddTB.Size = new System.Drawing.Size(85, 20);
            this.PhysicalServerAddTB.TabIndex = 9;
            this.PhysicalServerAddTB.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // LogicalServerAddTB
            // 
            this.LogicalServerAddTB.Hexadecimal = true;
            this.LogicalServerAddTB.Location = new System.Drawing.Point(231, 74);
            this.LogicalServerAddTB.Maximum = new decimal(new int[] {
            0,
            1,
            0,
            0});
            this.LogicalServerAddTB.Name = "LogicalServerAddTB";
            this.LogicalServerAddTB.Size = new System.Drawing.Size(85, 20);
            this.LogicalServerAddTB.TabIndex = 10;
            // 
            // LogicalServerAddLbl
            // 
            this.LogicalServerAddLbl.AutoSize = true;
            this.LogicalServerAddLbl.Location = new System.Drawing.Point(168, 74);
            this.LogicalServerAddLbl.Name = "LogicalServerAddLbl";
            this.LogicalServerAddLbl.Size = new System.Drawing.Size(44, 13);
            this.LogicalServerAddLbl.TabIndex = 39;
            this.LogicalServerAddLbl.Text = "Logical:";
            // 
            // AuthenticationCB
            // 
            this.AuthenticationCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AuthenticationCB.FormattingEnabled = true;
            this.AuthenticationCB.Location = new System.Drawing.Point(102, 19);
            this.AuthenticationCB.Name = "AuthenticationCB";
            this.AuthenticationCB.Size = new System.Drawing.Size(85, 21);
            this.AuthenticationCB.TabIndex = 5;
            this.AuthenticationCB.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.AuthenticationCB_DrawItem);
            // 
            // AuthenticationLbl
            // 
            this.AuthenticationLbl.AutoSize = true;
            this.AuthenticationLbl.Location = new System.Drawing.Point(11, 23);
            this.AuthenticationLbl.Name = "AuthenticationLbl";
            this.AuthenticationLbl.Size = new System.Drawing.Size(78, 13);
            this.AuthenticationLbl.TabIndex = 37;
            this.AuthenticationLbl.Text = "Authentication:";
            // 
            // WebAddressTB
            // 
            this.WebAddressTB.Location = new System.Drawing.Point(89, 29);
            this.WebAddressTB.Name = "WebAddressTB";
            this.WebAddressTB.Size = new System.Drawing.Size(233, 20);
            this.WebAddressTB.TabIndex = 1;
            // 
            // WebAddressLbl
            // 
            this.WebAddressLbl.AutoSize = true;
            this.WebAddressLbl.Location = new System.Drawing.Point(9, 32);
            this.WebAddressLbl.Name = "WebAddressLbl";
            this.WebAddressLbl.Size = new System.Drawing.Size(74, 13);
            this.WebAddressLbl.TabIndex = 40;
            this.WebAddressLbl.Text = "Web Address:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(412, 398);
            this.tabControl1.TabIndex = 48;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.StandardCb);
            this.tabPage1.Controls.Add(this.StandardLbl);
            this.tabPage1.Controls.Add(this.AddressingGB);
            this.tabPage1.Controls.Add(this.WebAddressTB);
            this.tabPage1.Controls.Add(this.WebAddressLbl);
            this.tabPage1.Controls.Add(this.NameLbl);
            this.tabPage1.Controls.Add(this.NameTB);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.ManufacturerIdTB);
            this.tabPage1.Controls.Add(this.UseLNCB);
            this.tabPage1.Controls.Add(this.UseIEC47CB);
            this.tabPage1.Controls.Add(this.StartProtocolCB);
            this.tabPage1.Controls.Add(this.StartProtocolLbl);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(404, 372);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.BlockCipherKeyAsciiCb);
            this.tabPage3.Controls.Add(this.AuthenticationKeyAsciiCb);
            this.tabPage3.Controls.Add(this.SystemTitleAsciiCb);
            this.tabPage3.Controls.Add(this.SecurityCB);
            this.tabPage3.Controls.Add(this.AuthenticationKeyTB);
            this.tabPage3.Controls.Add(this.BlockCipherKeyTB);
            this.tabPage3.Controls.Add(this.SystemTitleTB);
            this.tabPage3.Controls.Add(this.AuthenticationKeyLbl);
            this.tabPage3.Controls.Add(this.BlockCipherKeyLbl);
            this.tabPage3.Controls.Add(this.SystemtitleLbl);
            this.tabPage3.Controls.Add(this.SecurityLbl);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(404, 372);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Secured Connections";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // BlockCipherKeyAsciiCb
            // 
            this.BlockCipherKeyAsciiCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BlockCipherKeyAsciiCb.AutoSize = true;
            this.BlockCipherKeyAsciiCb.Location = new System.Drawing.Point(342, 66);
            this.BlockCipherKeyAsciiCb.Name = "BlockCipherKeyAsciiCb";
            this.BlockCipherKeyAsciiCb.Size = new System.Drawing.Size(53, 17);
            this.BlockCipherKeyAsciiCb.TabIndex = 74;
            this.BlockCipherKeyAsciiCb.Text = "ASCII";
            this.BlockCipherKeyAsciiCb.UseVisualStyleBackColor = true;
            // 
            // AuthenticationKeyAsciiCb
            // 
            this.AuthenticationKeyAsciiCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AuthenticationKeyAsciiCb.AutoSize = true;
            this.AuthenticationKeyAsciiCb.Location = new System.Drawing.Point(342, 92);
            this.AuthenticationKeyAsciiCb.Name = "AuthenticationKeyAsciiCb";
            this.AuthenticationKeyAsciiCb.Size = new System.Drawing.Size(53, 17);
            this.AuthenticationKeyAsciiCb.TabIndex = 73;
            this.AuthenticationKeyAsciiCb.Text = "ASCII";
            this.AuthenticationKeyAsciiCb.UseVisualStyleBackColor = true;
            // 
            // SystemTitleAsciiCb
            // 
            this.SystemTitleAsciiCb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SystemTitleAsciiCb.AutoSize = true;
            this.SystemTitleAsciiCb.Location = new System.Drawing.Point(344, 39);
            this.SystemTitleAsciiCb.Name = "SystemTitleAsciiCb";
            this.SystemTitleAsciiCb.Size = new System.Drawing.Size(53, 17);
            this.SystemTitleAsciiCb.TabIndex = 72;
            this.SystemTitleAsciiCb.Text = "ASCII";
            this.SystemTitleAsciiCb.UseVisualStyleBackColor = true;
            // 
            // SecurityCB
            // 
            this.SecurityCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SecurityCB.Location = new System.Drawing.Point(109, 14);
            this.SecurityCB.Name = "SecurityCB";
            this.SecurityCB.Size = new System.Drawing.Size(226, 21);
            this.SecurityCB.TabIndex = 64;
            // 
            // AuthenticationKeyTB
            // 
            this.AuthenticationKeyTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AuthenticationKeyTB.Location = new System.Drawing.Point(109, 92);
            this.AuthenticationKeyTB.Name = "AuthenticationKeyTB";
            this.AuthenticationKeyTB.Size = new System.Drawing.Size(226, 20);
            this.AuthenticationKeyTB.TabIndex = 67;
            // 
            // BlockCipherKeyTB
            // 
            this.BlockCipherKeyTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BlockCipherKeyTB.Location = new System.Drawing.Point(109, 66);
            this.BlockCipherKeyTB.Name = "BlockCipherKeyTB";
            this.BlockCipherKeyTB.Size = new System.Drawing.Size(226, 20);
            this.BlockCipherKeyTB.TabIndex = 66;
            // 
            // SystemTitleTB
            // 
            this.SystemTitleTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SystemTitleTB.Location = new System.Drawing.Point(109, 39);
            this.SystemTitleTB.Name = "SystemTitleTB";
            this.SystemTitleTB.Size = new System.Drawing.Size(226, 20);
            this.SystemTitleTB.TabIndex = 65;
            // 
            // AuthenticationKeyLbl
            // 
            this.AuthenticationKeyLbl.AutoSize = true;
            this.AuthenticationKeyLbl.Location = new System.Drawing.Point(4, 95);
            this.AuthenticationKeyLbl.Name = "AuthenticationKeyLbl";
            this.AuthenticationKeyLbl.Size = new System.Drawing.Size(99, 13);
            this.AuthenticationKeyLbl.TabIndex = 71;
            this.AuthenticationKeyLbl.Text = "Authentication Key:";
            // 
            // BlockCipherKeyLbl
            // 
            this.BlockCipherKeyLbl.AutoSize = true;
            this.BlockCipherKeyLbl.Location = new System.Drawing.Point(4, 69);
            this.BlockCipherKeyLbl.Name = "BlockCipherKeyLbl";
            this.BlockCipherKeyLbl.Size = new System.Drawing.Size(91, 13);
            this.BlockCipherKeyLbl.TabIndex = 70;
            this.BlockCipherKeyLbl.Text = "Block Cipher Key:";
            // 
            // SystemtitleLbl
            // 
            this.SystemtitleLbl.AutoSize = true;
            this.SystemtitleLbl.Location = new System.Drawing.Point(4, 43);
            this.SystemtitleLbl.Name = "SystemtitleLbl";
            this.SystemtitleLbl.Size = new System.Drawing.Size(63, 13);
            this.SystemtitleLbl.TabIndex = 69;
            this.SystemtitleLbl.Text = "System title:";
            // 
            // SecurityLbl
            // 
            this.SecurityLbl.AutoSize = true;
            this.SecurityLbl.Location = new System.Drawing.Point(4, 17);
            this.SecurityLbl.Name = "SecurityLbl";
            this.SecurityLbl.Size = new System.Drawing.Size(48, 13);
            this.SecurityLbl.TabIndex = 68;
            this.SecurityLbl.Text = "Security:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.InfoTB);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(404, 372);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Info";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // InfoTB
            // 
            this.InfoTB.AcceptsReturn = true;
            this.InfoTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoTB.Location = new System.Drawing.Point(3, 3);
            this.InfoTB.Multiline = true;
            this.InfoTB.Name = "InfoTB";
            this.InfoTB.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.InfoTB.Size = new System.Drawing.Size(398, 366);
            this.InfoTB.TabIndex = 1;
            // 
            // StandardCb
            // 
            this.StandardCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.StandardCb.FormattingEnabled = true;
            this.StandardCb.Location = new System.Drawing.Point(119, 55);
            this.StandardCb.Name = "StandardCb";
            this.StandardCb.Size = new System.Drawing.Size(194, 21);
            this.StandardCb.TabIndex = 41;
            // 
            // StandardLbl
            // 
            this.StandardLbl.AutoSize = true;
            this.StandardLbl.Location = new System.Drawing.Point(10, 58);
            this.StandardLbl.Name = "StandardLbl";
            this.StandardLbl.Size = new System.Drawing.Size(53, 13);
            this.StandardLbl.TabIndex = 42;
            this.StandardLbl.Text = "Standard:";
            // 
            // ManufacturerForm
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(416, 439);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.CancelBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ManufacturerForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manufacturer Settings";
            this.AddressingGB.ResumeLayout(false);
            this.AddressingGB.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClientAddTB)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PhysicalServerAddTB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogicalServerAddTB)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.CheckBox UseIEC47CB;
        private System.Windows.Forms.CheckBox UseLNCB;
        private System.Windows.Forms.TextBox ManufacturerIdTB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NameTB;
        private System.Windows.Forms.Label NameLbl;
        private System.Windows.Forms.Label StartProtocolLbl;
        private System.Windows.Forms.ComboBox StartProtocolCB;
        private System.Windows.Forms.GroupBox AddressingGB;
        private System.Windows.Forms.ComboBox AuthenticationCB;
        private System.Windows.Forms.Label AuthenticationLbl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label PhysicalServerAddLbl;
        private System.Windows.Forms.NumericUpDown PhysicalServerAddTB;
        private System.Windows.Forms.NumericUpDown LogicalServerAddTB;
        private System.Windows.Forms.Label LogicalServerAddLbl;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label ClientAddLbl;
        private System.Windows.Forms.NumericUpDown ClientAddTB;
        private System.Windows.Forms.TextBox SerialNumberFormulaTB;
        private System.Windows.Forms.Label SerialNumberFormulaLbl;
        private System.Windows.Forms.ComboBox ServerAddressTypeCB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox WebAddressTB;
        private System.Windows.Forms.Label WebAddressLbl;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox InfoTB;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox BlockCipherKeyAsciiCb;
        private System.Windows.Forms.CheckBox AuthenticationKeyAsciiCb;
        private System.Windows.Forms.CheckBox SystemTitleAsciiCb;
        private System.Windows.Forms.ComboBox SecurityCB;
        private System.Windows.Forms.TextBox AuthenticationKeyTB;
        private System.Windows.Forms.TextBox BlockCipherKeyTB;
        private System.Windows.Forms.TextBox SystemTitleTB;
        private System.Windows.Forms.Label AuthenticationKeyLbl;
        private System.Windows.Forms.Label BlockCipherKeyLbl;
        private System.Windows.Forms.Label SystemtitleLbl;
        private System.Windows.Forms.Label SecurityLbl;
        private System.Windows.Forms.ComboBox SizeCb;
        private System.Windows.Forms.Label SizeLbl;
        private System.Windows.Forms.ComboBox StandardCb;
        private System.Windows.Forms.Label StandardLbl;
    }
}