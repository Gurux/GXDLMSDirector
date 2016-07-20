//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/ManufacturerForm.Designer.cs $
//
// Version:         $Revision: 8655 $,
//                  $Date: 2016-07-20 15:55:25 +0300 (ke, 20 heinä 2016) $
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
            this.InactivityModeCB = new System.Windows.Forms.ComboBox();
            this.InactivityModeLbl = new System.Windows.Forms.Label();
            this.KeepAliveLbl = new System.Windows.Forms.Label();
            this.KeepAliveIntervalTB = new System.Windows.Forms.NumericUpDown();
            this.ForceKeepAliveCB = new System.Windows.Forms.CheckBox();
            this.SecuredConnectionCB = new System.Windows.Forms.CheckBox();
            this.AdvancedBtn = new System.Windows.Forms.Button();
            this.AddressingGB.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClientAddTB)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PhysicalServerAddTB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogicalServerAddTB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.KeepAliveIntervalTB)).BeginInit();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Location = new System.Drawing.Point(168, 410);
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
            this.CancelBtn.Location = new System.Drawing.Point(249, 410);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 20;
            this.CancelBtn.Text = "&Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // UseIEC47CB
            // 
            this.UseIEC47CB.AutoSize = true;
            this.UseIEC47CB.Location = new System.Drawing.Point(198, 79);
            this.UseIEC47CB.Name = "UseIEC47CB";
            this.UseIEC47CB.Size = new System.Drawing.Size(113, 17);
            this.UseIEC47CB.TabIndex = 3;
            this.UseIEC47CB.Text = "Use IEC 62056-47";
            this.UseIEC47CB.UseVisualStyleBackColor = true;
            // 
            // UseLNCB
            // 
            this.UseLNCB.AutoSize = true;
            this.UseLNCB.Location = new System.Drawing.Point(12, 79);
            this.UseLNCB.Name = "UseLNCB";
            this.UseLNCB.Size = new System.Drawing.Size(167, 17);
            this.UseLNCB.TabIndex = 2;
            this.UseLNCB.Text = "Use Logical name referencing";
            this.UseLNCB.UseVisualStyleBackColor = true;
            // 
            // ManufacturerIdTB
            // 
            this.ManufacturerIdTB.Location = new System.Drawing.Point(121, 49);
            this.ManufacturerIdTB.Name = "ManufacturerIdTB";
            this.ManufacturerIdTB.Size = new System.Drawing.Size(95, 20);
            this.ManufacturerIdTB.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Manufacturer ID:";
            // 
            // NameTB
            // 
            this.NameTB.Location = new System.Drawing.Point(12, 24);
            this.NameTB.Name = "NameTB";
            this.NameTB.Size = new System.Drawing.Size(303, 20);
            this.NameTB.TabIndex = 0;
            // 
            // NameLbl
            // 
            this.NameLbl.AutoSize = true;
            this.NameLbl.Location = new System.Drawing.Point(12, 8);
            this.NameLbl.Name = "NameLbl";
            this.NameLbl.Size = new System.Drawing.Size(104, 13);
            this.NameLbl.TabIndex = 15;
            this.NameLbl.Text = "Manufacturer Name:";
            // 
            // StartProtocolLbl
            // 
            this.StartProtocolLbl.AutoSize = true;
            this.StartProtocolLbl.Location = new System.Drawing.Point(12, 185);
            this.StartProtocolLbl.Name = "StartProtocolLbl";
            this.StartProtocolLbl.Size = new System.Drawing.Size(74, 13);
            this.StartProtocolLbl.TabIndex = 20;
            this.StartProtocolLbl.Text = "Start Protocol:";
            // 
            // StartProtocolCB
            // 
            this.StartProtocolCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.StartProtocolCB.FormattingEnabled = true;
            this.StartProtocolCB.Location = new System.Drawing.Point(121, 182);
            this.StartProtocolCB.Name = "StartProtocolCB";
            this.StartProtocolCB.Size = new System.Drawing.Size(194, 21);
            this.StartProtocolCB.TabIndex = 7;
            // 
            // AddressingGB
            // 
            this.AddressingGB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AddressingGB.Controls.Add(this.groupBox2);
            this.AddressingGB.Controls.Add(this.groupBox1);
            this.AddressingGB.Controls.Add(this.AuthenticationCB);
            this.AddressingGB.Controls.Add(this.AuthenticationLbl);
            this.AddressingGB.Location = new System.Drawing.Point(2, 207);
            this.AddressingGB.Name = "AddressingGB";
            this.AddressingGB.Size = new System.Drawing.Size(335, 188);
            this.AddressingGB.TabIndex = 7;
            this.AddressingGB.TabStop = false;
            this.AddressingGB.Text = "Addressing";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ClientAddLbl);
            this.groupBox2.Controls.Add(this.ClientAddTB);
            this.groupBox2.Location = new System.Drawing.Point(2, 40);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(327, 43);
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
            this.ClientAddTB.TabIndex = 10;
            this.ClientAddTB.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
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
            this.groupBox1.Size = new System.Drawing.Size(332, 105);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server Address:";
            // 
            // ServerAddressTypeCB
            // 
            this.ServerAddressTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ServerAddressTypeCB.FormattingEnabled = true;
            this.ServerAddressTypeCB.Location = new System.Drawing.Point(97, 19);
            this.ServerAddressTypeCB.Name = "ServerAddressTypeCB";
            this.ServerAddressTypeCB.Size = new System.Drawing.Size(214, 21);
            this.ServerAddressTypeCB.TabIndex = 14;
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
            this.SerialNumberFormulaTB.TabIndex = 16;
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
            this.PhysicalServerAddTB.TabIndex = 17;
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
            this.LogicalServerAddTB.TabIndex = 18;
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
            this.AuthenticationCB.TabIndex = 8;
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
            // InactivityModeCB
            // 
            this.InactivityModeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InactivityModeCB.FormattingEnabled = true;
            this.InactivityModeCB.Location = new System.Drawing.Point(121, 124);
            this.InactivityModeCB.Name = "InactivityModeCB";
            this.InactivityModeCB.Size = new System.Drawing.Size(194, 21);
            this.InactivityModeCB.TabIndex = 4;
            // 
            // InactivityModeLbl
            // 
            this.InactivityModeLbl.AutoSize = true;
            this.InactivityModeLbl.Location = new System.Drawing.Point(12, 127);
            this.InactivityModeLbl.Name = "InactivityModeLbl";
            this.InactivityModeLbl.Size = new System.Drawing.Size(82, 13);
            this.InactivityModeLbl.TabIndex = 34;
            this.InactivityModeLbl.Text = "Inactivity Mode:";
            // 
            // KeepAliveLbl
            // 
            this.KeepAliveLbl.AutoSize = true;
            this.KeepAliveLbl.Location = new System.Drawing.Point(12, 154);
            this.KeepAliveLbl.Name = "KeepAliveLbl";
            this.KeepAliveLbl.Size = new System.Drawing.Size(95, 13);
            this.KeepAliveLbl.TabIndex = 36;
            this.KeepAliveLbl.Text = "Keepalive Interval:";
            // 
            // KeepAliveIntervalTB
            // 
            this.KeepAliveIntervalTB.Location = new System.Drawing.Point(121, 154);
            this.KeepAliveIntervalTB.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.KeepAliveIntervalTB.Name = "KeepAliveIntervalTB";
            this.KeepAliveIntervalTB.Size = new System.Drawing.Size(68, 20);
            this.KeepAliveIntervalTB.TabIndex = 5;
            this.KeepAliveIntervalTB.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // ForceKeepAliveCB
            // 
            this.ForceKeepAliveCB.AutoSize = true;
            this.ForceKeepAliveCB.Location = new System.Drawing.Point(231, 154);
            this.ForceKeepAliveCB.Name = "ForceKeepAliveCB";
            this.ForceKeepAliveCB.Size = new System.Drawing.Size(53, 17);
            this.ForceKeepAliveCB.TabIndex = 6;
            this.ForceKeepAliveCB.Text = "Force";
            this.ForceKeepAliveCB.UseVisualStyleBackColor = true;
            // 
            // SecuredConnectionCB
            // 
            this.SecuredConnectionCB.AutoSize = true;
            this.SecuredConnectionCB.Location = new System.Drawing.Point(12, 101);
            this.SecuredConnectionCB.Name = "SecuredConnectionCB";
            this.SecuredConnectionCB.Size = new System.Drawing.Size(122, 17);
            this.SecuredConnectionCB.TabIndex = 37;
            this.SecuredConnectionCB.Text = "Secured connection";
            this.SecuredConnectionCB.UseVisualStyleBackColor = true;
            this.SecuredConnectionCB.CheckedChanged += new System.EventHandler(this.SecuredConnectionCB_CheckedChanged);
            // 
            // AdvancedBtn
            // 
            this.AdvancedBtn.Location = new System.Drawing.Point(198, 97);
            this.AdvancedBtn.Name = "AdvancedBtn";
            this.AdvancedBtn.Size = new System.Drawing.Size(75, 23);
            this.AdvancedBtn.TabIndex = 38;
            this.AdvancedBtn.Text = "Advanced...";
            this.AdvancedBtn.UseVisualStyleBackColor = true;
            this.AdvancedBtn.Click += new System.EventHandler(this.AdvancedBtn_Click);
            // 
            // ManufacturerForm
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(337, 445);
            this.Controls.Add(this.AdvancedBtn);
            this.Controls.Add(this.SecuredConnectionCB);
            this.Controls.Add(this.ForceKeepAliveCB);
            this.Controls.Add(this.KeepAliveIntervalTB);
            this.Controls.Add(this.KeepAliveLbl);
            this.Controls.Add(this.InactivityModeLbl);
            this.Controls.Add(this.InactivityModeCB);
            this.Controls.Add(this.AddressingGB);
            this.Controls.Add(this.StartProtocolCB);
            this.Controls.Add(this.StartProtocolLbl);
            this.Controls.Add(this.UseIEC47CB);
            this.Controls.Add(this.UseLNCB);
            this.Controls.Add(this.ManufacturerIdTB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NameTB);
            this.Controls.Add(this.NameLbl);
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
            ((System.ComponentModel.ISupportInitialize)(this.KeepAliveIntervalTB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.ComboBox InactivityModeCB;
        private System.Windows.Forms.Label InactivityModeLbl;
        private System.Windows.Forms.ComboBox AuthenticationCB;
        private System.Windows.Forms.Label AuthenticationLbl;
        private System.Windows.Forms.Label KeepAliveLbl;
        private System.Windows.Forms.NumericUpDown KeepAliveIntervalTB;
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
        private System.Windows.Forms.CheckBox ForceKeepAliveCB;
        private System.Windows.Forms.CheckBox SecuredConnectionCB;
        private System.Windows.Forms.Button AdvancedBtn;
    }
}