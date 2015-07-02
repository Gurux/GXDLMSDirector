//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/Views/GXDLMSActivityCalendarView.cs $
//
// Version:         $Revision: 6333 $,
//                  $Date: 2013-05-17 12:15:22 +0300 (pe, 17 touko 2013) $
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GXDLMS.Common;
using Gurux.DLMS.Objects;
using Gurux.DLMS;

namespace GXDLMSDirector.Views
{
    [GXDLMSViewAttribute(typeof(GXDLMSActivityCalendar))]
    class GXDLMSActivityCalendarView : Form, IGXDLMSView
    {
        private GroupBox groupBox1;
        private GXValueField LogicalNameTB;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private Label label2;
        private Label label1;
        private TabControl ADayProfileTC;
        private TabPage tabPage3;
        private ListView AWeekProfileLV;
        private ColumnHeader columnHeader1;
        private ColumnHeader MonHeader;
        private ColumnHeader TueHeader;
        private ColumnHeader WedHeader;
        private ColumnHeader ThuHeader;
        private ColumnHeader FriHeader;
        private ColumnHeader SatHeader;
        private ColumnHeader SunHeader;
        private ListView SeasonProfileActiveLV;
        private ColumnHeader NameHeader;
        private ColumnHeader StartHeader;
        private ColumnHeader DayHeader;
        private ColumnHeader MonthHeader;
        private ColumnHeader YearHeader;
        private ColumnHeader WeekdaysHeader;
        private ColumnHeader WeekNameHeader;
        private Label ASeasonProfileLbl;
        private GXValueField ACalendarNameTB;
        private Label ACalendarNameLbl;
        private TabPage tabPage2;
        private Label label3;
        private Label label4;
        private TabControl PDayProfileTC;
        private TabPage tabPage4;
        private ListView PWeekProfileLV;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeader8;
        private ColumnHeader columnHeader9;
        private ListView SeasonProfilePassiveLV;
        private ColumnHeader columnHeader10;
        private ColumnHeader columnHeader11;
        private ColumnHeader columnHeader12;
        private ColumnHeader columnHeader13;
        private ColumnHeader columnHeader14;
        private ColumnHeader columnHeader15;
        private ColumnHeader columnHeader16;
        private Label label5;
        private GXValueField PCalendarNameTB;
        private Label PCalendarNameLbl;
        private System.ComponentModel.IContainer components;
        private ErrorProvider errorProvider1;
        private Label LogicalNameLbl;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSActivityCalendarView()
        {
            InitializeComponent();
        }
        #region IGXDLMSView Members

        public GXDLMSObject Target
        {
            get;
            set;
        }

        public void OnValueChanged(int attributeID, object value)
        {
            GXDLMSActivityCalendar target = Target as GXDLMSActivityCalendar;
            if (attributeID == 3)
            {                
                SeasonProfileActiveLV.Items.Clear();
                if (target.SeasonProfileActive != null)
                {                    
                    foreach (GXDLMSSeasonProfile it in target.SeasonProfileActive)
                    {
                        ListViewItem li = SeasonProfileActiveLV.Items.Add(it.Name);
                        li.SubItems.AddRange(new string[]{"", "", "", "", "", ""});
                        li.SubItems[this.YearHeader.Index].Text = it.Start.Value.Year.ToString();
                        li.SubItems[this.MonthHeader.Index].Text = it.Start.Value.Month.ToString();
                        li.SubItems[this.DayHeader.Index].Text = it.Start.Value.Day.ToString();
                        li.SubItems[this.StartHeader.Index].Text = it.Start.ToString();                        
                        li.SubItems[this.WeekNameHeader.Index].Text = it.WeekName;
                    }
                }
            }
            else if (attributeID == 4)
            {
                AWeekProfileLV.Items.Clear();
                if (target.WeekProfileTableActive != null)
                {
                    foreach (GXDLMSWeekProfile it in target.WeekProfileTableActive)
                    {
                        ListViewItem li = AWeekProfileLV.Items.Add(it.Name);
                        li.SubItems.AddRange(new string[] { it.Monday.ToString(), it.Tuesday.ToString(), it.Wednesday.ToString(), it.Thursday.ToString(), it.Friday.ToString(), it.Saturday.ToString(), it.Sunday.ToString() });
                    }
                }
            }
            else if (attributeID == 5)
            {
                ADayProfileTC.TabPages.Clear();
                if (target.DayProfileTableActive != null)
                {
                    System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CurrentUICulture;
                    foreach (GXDLMSDayProfile it in target.DayProfileTableActive)
                    {
                        TabPage pg = new TabPage("Day " + it.DayId);
                        ListView lv = new ListView();
                        lv.Dock = DockStyle.Fill;
                        lv.HeaderStyle = ColumnHeaderStyle.Nonclickable;
                        lv.View = View.Details;
                        lv.Columns.Add("Start");
                        lv.Columns.Add("Script table");
                        lv.Columns.Add("Script Selector");
                        pg.Controls.Add(lv);
                        ADayProfileTC.TabPages.Add(pg);
                        foreach (GXDLMSDayProfileAction day in it.DaySchedules)
                        {
                            ListViewItem li = lv.Items.Add(day.StartTime.ToString());
                            li.SubItems.Add(day.ScriptLogicalName);
                            li.SubItems.Add(day.ScriptSelector.ToString());
                        }
                    }
                }                
            }
            else if (attributeID == 7)
            {
                SeasonProfilePassiveLV.Items.Clear();
                if (target.SeasonProfilePassive != null)
                {                    
                    foreach (GXDLMSSeasonProfile it in target.SeasonProfilePassive)
                    {
                        ListViewItem li = SeasonProfilePassiveLV.Items.Add(it.Name);
                        li.SubItems.AddRange(new string[] { "", "", "", "", "", "" });
                        li.SubItems[this.YearHeader.Index].Text = it.Start.Value.Year.ToString();
                        li.SubItems[this.MonthHeader.Index].Text = it.Start.Value.Month.ToString();
                        li.SubItems[this.DayHeader.Index].Text = it.Start.Value.Day.ToString();
                        li.SubItems[this.StartHeader.Index].Text = it.Start.ToString();
                        li.SubItems[this.WeekNameHeader.Index].Text = it.WeekName;
                    }
                }
            }
            else if (attributeID == 8)
            {
                PWeekProfileLV.Items.Clear();
                if (target.WeekProfileTablePassive != null)
                {
                    foreach (GXDLMSWeekProfile it in target.WeekProfileTablePassive)
                    {
                        ListViewItem li = PWeekProfileLV.Items.Add(it.Name);
                        li.SubItems.AddRange(new string[] { it.Monday.ToString(), it.Tuesday.ToString(), it.Wednesday.ToString(), it.Thursday.ToString(), it.Friday.ToString(), it.Saturday.ToString(), it.Sunday.ToString() });
                    }
                }
            }
            else if (attributeID == 9)
            {
                PDayProfileTC.TabPages.Clear();
                if (target.DayProfileTablePassive != null)
                {                    
                    foreach (GXDLMSDayProfile it in target.DayProfileTablePassive)
                    {
                        TabPage pg = new TabPage("Day " + it.DayId);
                        ListView lv = new ListView();
                        lv.Dock = DockStyle.Fill;
                        lv.HeaderStyle = ColumnHeaderStyle.Nonclickable;
                        lv.View = View.Details;
                        lv.Columns.Add("Start");
                        lv.Columns.Add("Script table");
                        lv.Columns.Add("Script Selector");
                        pg.Controls.Add(lv);
                        PDayProfileTC.TabPages.Add(pg);
                        foreach (GXDLMSDayProfileAction day in it.DaySchedules)
                        {
                            ListViewItem li = lv.Items.Add(day.StartTime.ToString());
                            li.SubItems.Add(day.ScriptLogicalName);
                            li.SubItems.Add(day.ScriptSelector.ToString());
                        }
                    }
                }
            }
        }

        public void OnAccessRightsChange(int attributeID, AccessMode access)
        {            
        }

        public System.Windows.Forms.ErrorProvider ErrorProvider
        {
            get
            {
                return errorProvider1;
            }
        }

        public string Description
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public void OnDirtyChange(int attributeID, bool Dirty)
        {

        }

        #endregion


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDLMSActivityCalendarView));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ADayProfileTC = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.AWeekProfileLV = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.MonHeader = new System.Windows.Forms.ColumnHeader();
            this.TueHeader = new System.Windows.Forms.ColumnHeader();
            this.WedHeader = new System.Windows.Forms.ColumnHeader();
            this.ThuHeader = new System.Windows.Forms.ColumnHeader();
            this.FriHeader = new System.Windows.Forms.ColumnHeader();
            this.SatHeader = new System.Windows.Forms.ColumnHeader();
            this.SunHeader = new System.Windows.Forms.ColumnHeader();
            this.SeasonProfileActiveLV = new System.Windows.Forms.ListView();
            this.NameHeader = new System.Windows.Forms.ColumnHeader();
            this.StartHeader = new System.Windows.Forms.ColumnHeader();
            this.DayHeader = new System.Windows.Forms.ColumnHeader();
            this.MonthHeader = new System.Windows.Forms.ColumnHeader();
            this.YearHeader = new System.Windows.Forms.ColumnHeader();
            this.WeekdaysHeader = new System.Windows.Forms.ColumnHeader();
            this.WeekNameHeader = new System.Windows.Forms.ColumnHeader();
            this.ASeasonProfileLbl = new System.Windows.Forms.Label();
            this.ACalendarNameTB = new GXDLMSDirector.Views.GXValueField();
            this.ACalendarNameLbl = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.PDayProfileTC = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.PWeekProfileLV = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.SeasonProfilePassiveLV = new System.Windows.Forms.ListView();
            this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader13 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader14 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader15 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader16 = new System.Windows.Forms.ColumnHeader();
            this.label5 = new System.Windows.Forms.Label();
            this.PCalendarNameTB = new GXDLMSDirector.Views.GXValueField();
            this.PCalendarNameLbl = new System.Windows.Forms.Label();
            this.LogicalNameTB = new GXDLMSDirector.Views.GXValueField();
            this.LogicalNameLbl = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.ADayProfileTC.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.PDayProfileTC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Controls.Add(this.LogicalNameTB);
            this.groupBox1.Controls.Add(this.LogicalNameLbl);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(507, 618);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Activity Calendar Object";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(6, 47);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(495, 558);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.ADayProfileTC);
            this.tabPage1.Controls.Add(this.AWeekProfileLV);
            this.tabPage1.Controls.Add(this.SeasonProfileActiveLV);
            this.tabPage1.Controls.Add(this.ASeasonProfileLbl);
            this.tabPage1.Controls.Add(this.ACalendarNameTB);
            this.tabPage1.Controls.Add(this.ACalendarNameLbl);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(487, 532);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Active";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 326);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Day Profile:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Week Profile:";
            // 
            // ADayProfileTC
            // 
            this.ADayProfileTC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ADayProfileTC.Controls.Add(this.tabPage3);
            this.ADayProfileTC.Location = new System.Drawing.Point(11, 342);
            this.ADayProfileTC.Name = "ADayProfileTC";
            this.ADayProfileTC.SelectedIndex = 0;
            this.ADayProfileTC.Size = new System.Drawing.Size(470, 176);
            this.ADayProfileTC.TabIndex = 17;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(462, 150);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // AWeekProfileLV
            // 
            this.AWeekProfileLV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AWeekProfileLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.MonHeader,
            this.TueHeader,
            this.WedHeader,
            this.ThuHeader,
            this.FriHeader,
            this.SatHeader,
            this.SunHeader});
            this.AWeekProfileLV.FullRowSelect = true;
            this.AWeekProfileLV.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.AWeekProfileLV.HideSelection = false;
            this.AWeekProfileLV.Location = new System.Drawing.Point(11, 174);
            this.AWeekProfileLV.Name = "AWeekProfileLV";
            this.AWeekProfileLV.Size = new System.Drawing.Size(470, 147);
            this.AWeekProfileLV.TabIndex = 16;
            this.AWeekProfileLV.UseCompatibleStateImageBehavior = false;
            this.AWeekProfileLV.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 49;
            // 
            // MonHeader
            // 
            this.MonHeader.Text = "Mon";
            this.MonHeader.Width = 50;
            // 
            // TueHeader
            // 
            this.TueHeader.Text = "Tue";
            // 
            // WedHeader
            // 
            this.WedHeader.Text = "Wed";
            // 
            // ThuHeader
            // 
            this.ThuHeader.Text = "Thu";
            // 
            // FriHeader
            // 
            this.FriHeader.Text = "Fri";
            this.FriHeader.Width = 73;
            // 
            // SatHeader
            // 
            this.SatHeader.Text = "Sat";
            this.SatHeader.Width = 78;
            // 
            // SunHeader
            // 
            this.SunHeader.Text = "Sun";
            // 
            // SeasonProfileActiveLV
            // 
            this.SeasonProfileActiveLV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SeasonProfileActiveLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameHeader,
            this.StartHeader,
            this.DayHeader,
            this.MonthHeader,
            this.YearHeader,
            this.WeekdaysHeader,
            this.WeekNameHeader});
            this.SeasonProfileActiveLV.FullRowSelect = true;
            this.SeasonProfileActiveLV.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.SeasonProfileActiveLV.HideSelection = false;
            this.SeasonProfileActiveLV.Location = new System.Drawing.Point(11, 54);
            this.SeasonProfileActiveLV.Name = "SeasonProfileActiveLV";
            this.SeasonProfileActiveLV.Size = new System.Drawing.Size(470, 99);
            this.SeasonProfileActiveLV.TabIndex = 15;
            this.SeasonProfileActiveLV.UseCompatibleStateImageBehavior = false;
            this.SeasonProfileActiveLV.View = System.Windows.Forms.View.Details;
            // 
            // NameHeader
            // 
            this.NameHeader.Text = "Name";
            this.NameHeader.Width = 49;
            // 
            // StartHeader
            // 
            this.StartHeader.Text = "Start";
            this.StartHeader.Width = 50;
            // 
            // DayHeader
            // 
            this.DayHeader.Text = "Day";
            // 
            // MonthHeader
            // 
            this.MonthHeader.Text = "Month";
            // 
            // YearHeader
            // 
            this.YearHeader.Text = "Year";
            // 
            // WeekdaysHeader
            // 
            this.WeekdaysHeader.Text = "Weekdays";
            this.WeekdaysHeader.Width = 73;
            // 
            // WeekNameHeader
            // 
            this.WeekNameHeader.Text = "Week Name";
            this.WeekNameHeader.Width = 78;
            // 
            // ASeasonProfileLbl
            // 
            this.ASeasonProfileLbl.AutoSize = true;
            this.ASeasonProfileLbl.Location = new System.Drawing.Point(11, 38);
            this.ASeasonProfileLbl.Name = "ASeasonProfileLbl";
            this.ASeasonProfileLbl.Size = new System.Drawing.Size(78, 13);
            this.ASeasonProfileLbl.TabIndex = 14;
            this.ASeasonProfileLbl.Text = "Season Profile:";
            // 
            // ACalendarNameTB
            // 
            this.ACalendarNameTB.AttributeID = 2;
            this.ACalendarNameTB.Location = new System.Drawing.Point(107, 9);
            this.ACalendarNameTB.Name = "ACalendarNameTB";
            this.ACalendarNameTB.ReadOnly = true;
            this.ACalendarNameTB.Size = new System.Drawing.Size(208, 20);
            this.ACalendarNameTB.TabIndex = 12;
            this.ACalendarNameTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // ACalendarNameLbl
            // 
            this.ACalendarNameLbl.AutoSize = true;
            this.ACalendarNameLbl.Location = new System.Drawing.Point(11, 12);
            this.ACalendarNameLbl.Name = "ACalendarNameLbl";
            this.ACalendarNameLbl.Size = new System.Drawing.Size(83, 13);
            this.ACalendarNameLbl.TabIndex = 13;
            this.ACalendarNameLbl.Text = "Calendar Name:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.PDayProfileTC);
            this.tabPage2.Controls.Add(this.PWeekProfileLV);
            this.tabPage2.Controls.Add(this.SeasonProfilePassiveLV);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.PCalendarNameTB);
            this.tabPage2.Controls.Add(this.PCalendarNameLbl);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(487, 532);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Passive";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 329);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Day Profile:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Week Profile:";
            // 
            // PDayProfileTC
            // 
            this.PDayProfileTC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PDayProfileTC.Controls.Add(this.tabPage4);
            this.PDayProfileTC.Location = new System.Drawing.Point(8, 345);
            this.PDayProfileTC.Name = "PDayProfileTC";
            this.PDayProfileTC.SelectedIndex = 0;
            this.PDayProfileTC.Size = new System.Drawing.Size(470, 176);
            this.PDayProfileTC.TabIndex = 25;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(462, 150);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // PWeekProfileLV
            // 
            this.PWeekProfileLV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PWeekProfileLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9});
            this.PWeekProfileLV.FullRowSelect = true;
            this.PWeekProfileLV.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.PWeekProfileLV.HideSelection = false;
            this.PWeekProfileLV.Location = new System.Drawing.Point(8, 177);
            this.PWeekProfileLV.Name = "PWeekProfileLV";
            this.PWeekProfileLV.Size = new System.Drawing.Size(470, 147);
            this.PWeekProfileLV.TabIndex = 24;
            this.PWeekProfileLV.UseCompatibleStateImageBehavior = false;
            this.PWeekProfileLV.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 49;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Mon";
            this.columnHeader3.Width = 50;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Tue";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Wed";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Thu";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Fri";
            this.columnHeader7.Width = 73;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Sat";
            this.columnHeader8.Width = 78;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Sun";
            // 
            // SeasonProfilePassiveLV
            // 
            this.SeasonProfilePassiveLV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SeasonProfilePassiveLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader15,
            this.columnHeader16});
            this.SeasonProfilePassiveLV.FullRowSelect = true;
            this.SeasonProfilePassiveLV.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.SeasonProfilePassiveLV.HideSelection = false;
            this.SeasonProfilePassiveLV.Location = new System.Drawing.Point(8, 57);
            this.SeasonProfilePassiveLV.Name = "SeasonProfilePassiveLV";
            this.SeasonProfilePassiveLV.Size = new System.Drawing.Size(470, 99);
            this.SeasonProfilePassiveLV.TabIndex = 23;
            this.SeasonProfilePassiveLV.UseCompatibleStateImageBehavior = false;
            this.SeasonProfilePassiveLV.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Name";
            this.columnHeader10.Width = 49;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Start";
            this.columnHeader11.Width = 50;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Day";
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Month";
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "Year";
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "Weekdays";
            this.columnHeader15.Width = 73;
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "Week Name";
            this.columnHeader16.Width = 78;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Season Profile:";
            // 
            // PCalendarNameTB
            // 
            this.PCalendarNameTB.AttributeID = 6;
            this.PCalendarNameTB.Location = new System.Drawing.Point(104, 12);
            this.PCalendarNameTB.Name = "PCalendarNameTB";
            this.PCalendarNameTB.ReadOnly = true;
            this.PCalendarNameTB.Size = new System.Drawing.Size(208, 20);
            this.PCalendarNameTB.TabIndex = 20;
            this.PCalendarNameTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // PCalendarNameLbl
            // 
            this.PCalendarNameLbl.AutoSize = true;
            this.PCalendarNameLbl.Location = new System.Drawing.Point(8, 15);
            this.PCalendarNameLbl.Name = "PCalendarNameLbl";
            this.PCalendarNameLbl.Size = new System.Drawing.Size(83, 13);
            this.PCalendarNameLbl.TabIndex = 21;
            this.PCalendarNameLbl.Text = "Calendar Name:";
            // 
            // LogicalNameTB
            // 
            this.LogicalNameTB.AttributeID = 1;
            this.LogicalNameTB.Location = new System.Drawing.Point(102, 21);
            this.LogicalNameTB.Name = "LogicalNameTB";
            this.LogicalNameTB.ReadOnly = true;
            this.LogicalNameTB.Size = new System.Drawing.Size(208, 20);
            this.LogicalNameTB.TabIndex = 1;
            this.LogicalNameTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // LogicalNameLbl
            // 
            this.LogicalNameLbl.AutoSize = true;
            this.LogicalNameLbl.Location = new System.Drawing.Point(6, 24);
            this.LogicalNameLbl.Name = "LogicalNameLbl";
            this.LogicalNameLbl.Size = new System.Drawing.Size(75, 13);
            this.LogicalNameLbl.TabIndex = 0;
            this.LogicalNameLbl.Text = "Logical Name:";
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.Icon = ((System.Drawing.Icon)(resources.GetObject("errorProvider1.Icon")));
            // 
            // GXDLMSActivityCalendarView
            // 
            this.ClientSize = new System.Drawing.Size(531, 635);
            this.Controls.Add(this.groupBox1);
            this.Name = "GXDLMSActivityCalendarView";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ADayProfileTC.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.PDayProfileTC.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

    }
}
