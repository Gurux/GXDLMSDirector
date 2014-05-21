//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GuruxClub/GXDLMSDirector/Development/Views/GXDLMSProfileGenericView.cs $
//
// Version:         $Revision: 7358 $,
//                  $Date: 2014-05-05 14:06:51 +0300 (ma, 05 touko 2014) $
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
using Gurux.DLMS;
using System.Data;
using GXDLMS.Common;
using Gurux.DLMS.Objects;

namespace GXDLMSDirector.Views
{
    [GXDLMSViewAttribute(typeof(Gurux.DLMS.Objects.GXDLMSProfileGeneric))]
    class GXDLMSProfileGenericView : Form, IGXDLMSView
    {
        private GroupBox groupBox1;
        private GXValueField LogicalNameTB;
        private Label LogicalNameLbl;
        private GXValueField EntriesTB;
        private GXValueField EntriesInUseTB;
        private Label label1;
        private GroupBox groupBox2;
        private Label DaysLbl;
        private Label ToLbl;
        private DateTimePicker ToPick;
        private DateTimePicker StartPick;
        private RadioButton ReadFromRB;
        private RadioButton ReadLastRB;
        private RadioButton ReadEntryBtn;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private DataGridView ProfileGenericView;
        private System.ComponentModel.IContainer components;
        private TabPage tabPage2;
        private ZedGraph.ZedGraphControl m_MyPane;
        private Label label2;
        private NumericUpDown EndEntry;
        private NumericUpDown StartEntry;
        private NumericUpDown ReadLastTB;
        private GroupBox groupBox3;
        private Label label3;
        private GXValueField SortObjectTB;
        private Label label4;
        private GXValueField SortModeTB;
        private Label label5;
        private GXValueField CapturePeriodTB;
        private ErrorProvider errorProvider1;
        GXDLMSProfileGeneric m_Target;           
        /// <summary>
        /// Constructor.
        /// </summary>
        public GXDLMSProfileGenericView()
        {
            InitializeComponent();
            this.GraphItems = new GXGraphItemCollection();
            m_MyPane.GraphPane.YAxis.Type = ZedGraph.AxisType.Linear;
            m_MyPane.GraphPane.XAxis.Title.Text = "Date";
            m_MyPane.GraphPane.XAxis.Type = ZedGraph.AxisType.Date;
            m_MyPane.GraphPane.XAxis.ScaleFormatEvent += new ZedGraph.Axis.ScaleFormatHandler(XAxis_ScaleFormatEvent);
            this.ReadFromRB.CheckedChanged += new System.EventHandler(this.ReadFromRB_CheckedChanged);
            this.ReadLastRB.CheckedChanged += new System.EventHandler(this.ReadLastRB_CheckedChanged);
            this.ReadEntryBtn.CheckedChanged += new System.EventHandler(this.ReadAllRB_CheckedChanged);            
        }
        
        #region IGXDLMSView Members

        string XAxis_ScaleFormatEvent(ZedGraph.GraphPane pane, ZedGraph.Axis axis, double val, int index)
        {
            return DateTime.FromOADate(val).ToString("dd/MM/yyyy");
        }

        public GXGraphItemCollection GraphItems
        {
            get;
            set;
        }

        delegate void UpdateTargetEventHandler(GXDLMSObject value);

        void OnUpdateTarget(GXDLMSObject value)
        {
            m_Target = (GXDLMSProfileGeneric)value;
            m_MyPane.GraphPane.CurveList.Clear();
            GXDLMSObject obj;
            int index = 0;
            if (m_Target != null)
            {                
                m_MyPane.GraphPane.Title.Text = m_Target.Description;
                DataTable table = ProfileGenericView.DataSource as DataTable;
                ProfileGenericView.DataSource = null;
                ProfileGenericView.Columns.Clear();
                DataTable dt = new DataTable();                
                foreach (var it in m_Target.CaptureObjects)
                {
                    DataColumn dc = dt.Columns.Add(index.ToString());
                    dc.Caption = it.Key.Description;
                    int pos = ProfileGenericView.Columns.Add(index.ToString(), it.Key.Description);
                    ProfileGenericView.Columns[pos].DataPropertyName = index.ToString();
                    ++index;
                }
                foreach(object[] it in m_Target.Buffer)
                {
                    dt.LoadDataRow(it, true);                                    
                }

                ProfileGenericView.DataSource = dt;
                if (m_Target.CaptureObjects.Count != 0 && m_Target.CaptureObjects[0].Value.AttributeIndex != 0)
                {
                    //We can show graph only tables that are date based.
                    if (m_Target.CaptureObjects[0].Key.GetUIDataType(m_Target.CaptureObjects[0].Value.AttributeIndex) == DataType.DateTime)
                    {
                        for (int col = 0; col < m_Target.CaptureObjects.Count; ++col)
                        {
                            //Do not shown Status' or Events
                            index = m_Target.CaptureObjects[col].Value.AttributeIndex;
                            if (index > 0 && ((index & 0x8) != 0 || (m_Target.CaptureObjects[col].Value.AttributeIndex & 0x10) != 0))
                            {
                                continue;
                            }
                            obj = m_Target.CaptureObjects[col].Key;
                            GXGraphItem item = GraphItems.Find(obj.LogicalName, index);
                            if (item != null && item.Enabled && GXHelpers.IsNumeric(obj.GetUIDataType(index)))
                            {
                                ZedGraph.DataSourcePointList dspl = new ZedGraph.DataSourcePointList();
                                dspl.DataSource = m_Target.Buffer;
                                dspl.XDataMember = m_Target.CaptureObjects[0].Key.Description;
                                dspl.YDataMember = obj.Description;
                                ZedGraph.LineItem myCurve = m_MyPane.GraphPane.AddCurve(obj.Description, dspl, item.Color);
                            }
                        }
                        m_MyPane.GraphPane.XAxis.Title.Text = m_Target.CaptureObjects[0].Key.LogicalName;
                        // Tell ZedGraph to refigure the axes since the data have changed                
                        m_MyPane.AxisChange();
                    }
                }
            }
            else
            {
                ProfileGenericView.DataSource = null;
            }

            //Set initial values...
            ReadFromRB.Enabled = ReadLastRB.Enabled = ReadEntryBtn.Enabled = m_Target.CaptureObjects.Count != 0;
            ReadFromRB.Checked = ReadLastRB.Checked = ReadEntryBtn.Checked = false;
            StartEntry.Value = 0;
            EndEntry.Value = 1;
            ReadLastTB.Value = 0;
            StartPick.Value = ToPick.Value = DateTime.Now;
            if (!ReadFromRB.Enabled)
            {
                return;
            }
            index = m_Target.CaptureObjects[0].Value.AttributeIndex;
            obj = m_Target.CaptureObjects[0].Key;
            if (index != 0 &&
                obj.GetUIDataType(index) != DataType.DateTime)
            {
                ReadFromRB.Enabled = ReadLastRB.Enabled = false;
                m_Target.AccessSelector = AccessRange.Entry;
                m_Target.From = 0;
                m_Target.To = 1;
            }
            else
            {
                ReadFromRB.Enabled = ReadLastRB.Enabled = true;
            }
            if (m_Target.AccessSelector == AccessRange.Entry)
            {
                StartEntry.Value = Convert.ToInt32(m_Target.From);
                EndEntry.Value = Convert.ToInt32(m_Target.To);
                ReadEntryBtn.Checked = true;
            }
            else if (m_Target.AccessSelector == AccessRange.Last)
            {
                TimeSpan diff = (DateTime)m_Target.To - (DateTime)m_Target.From;
                ReadLastTB.Value = diff.Days - 1;
                ReadLastRB.Checked = true;
            }
            else
            {
                if ((DateTime)m_Target.From == DateTime.MinValue)
                {
                    StartPick.Checked = false;
                }
                else
                {
                    StartPick.Value = (DateTime)m_Target.From;
                }
                if ((DateTime)m_Target.To == DateTime.MaxValue)
                {
                    ToPick.Checked = false;
                }
                else
                {
                    ToPick.Value = (DateTime)m_Target.To;
                }
                ReadFromRB.Checked = true;
            }      
        }

        public GXDLMSObject Target
        {
            get
            {
                return m_Target;
            }
            set
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new UpdateTargetEventHandler(OnUpdateTarget), value);
                }
                else
                {
                    OnUpdateTarget(value);                    
                }                          
            }
        }

        public void OnValueChanged(int attributeID, object value)
        {
            if (attributeID == 2)
            {     
                DataTable dt = ProfileGenericView.DataSource as DataTable;
                if (m_Target.Buffer.Count < dt.Rows.Count)
                {
                    dt.Rows.Clear();
                }
                for (int pos = dt.Rows.Count; pos < m_Target.Buffer.Count; ++pos)
                {
                    object[] row = m_Target.Buffer[pos];
                    dt.LoadDataRow(row, true);
                }        
                ProfileGenericView.Refresh();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GXDLMSProfileGenericView));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.CapturePeriodTB = new GXDLMSDirector.Views.GXValueField();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SortObjectTB = new GXDLMSDirector.Views.GXValueField();
            this.label4 = new System.Windows.Forms.Label();
            this.SortModeTB = new GXDLMSDirector.Views.GXValueField();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ProfileGenericView = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.m_MyPane = new ZedGraph.ZedGraphControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ReadLastTB = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.EndEntry = new System.Windows.Forms.NumericUpDown();
            this.StartEntry = new System.Windows.Forms.NumericUpDown();
            this.DaysLbl = new System.Windows.Forms.Label();
            this.ToLbl = new System.Windows.Forms.Label();
            this.ToPick = new System.Windows.Forms.DateTimePicker();
            this.StartPick = new System.Windows.Forms.DateTimePicker();
            this.ReadFromRB = new System.Windows.Forms.RadioButton();
            this.ReadLastRB = new System.Windows.Forms.RadioButton();
            this.ReadEntryBtn = new System.Windows.Forms.RadioButton();
            this.EntriesTB = new GXDLMSDirector.Views.GXValueField();
            this.EntriesInUseTB = new GXDLMSDirector.Views.GXValueField();
            this.LogicalNameLbl = new System.Windows.Forms.Label();
            this.LogicalNameTB = new GXDLMSDirector.Views.GXValueField();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProfileGenericView)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadLastTB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndEntry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartEntry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.CapturePeriodTB);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.EntriesTB);
            this.groupBox1.Controls.Add(this.EntriesInUseTB);
            this.groupBox1.Controls.Add(this.LogicalNameLbl);
            this.groupBox1.Controls.Add(this.LogicalNameTB);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(501, 572);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Profile Generic";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(245, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Period:";
            // 
            // CapturePeriodTB
            // 
            this.CapturePeriodTB.AttributeID = 4;
            this.CapturePeriodTB.Location = new System.Drawing.Point(297, 120);
            this.CapturePeriodTB.Name = "CapturePeriodTB";
            this.CapturePeriodTB.Size = new System.Drawing.Size(185, 20);
            this.CapturePeriodTB.TabIndex = 15;
            this.CapturePeriodTB.TabStop = false;
            this.CapturePeriodTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.SortObjectTB);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.SortModeTB);
            this.groupBox3.Location = new System.Drawing.Point(3, 169);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(486, 49);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sort:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(243, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Object:";
            // 
            // SortObjectTB
            // 
            this.SortObjectTB.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SortObjectTB.AttributeID = 6;
            this.SortObjectTB.Location = new System.Drawing.Point(294, 18);
            this.SortObjectTB.Name = "SortObjectTB";
            this.SortObjectTB.Size = new System.Drawing.Size(185, 20);
            this.SortObjectTB.TabIndex = 15;
            this.SortObjectTB.TabStop = false;
            this.SortObjectTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Mode:";
            // 
            // SortModeTB
            // 
            this.SortModeTB.AttributeID = 5;
            this.SortModeTB.Location = new System.Drawing.Point(94, 18);
            this.SortModeTB.Name = "SortModeTB";
            this.SortModeTB.ReadOnly = true;
            this.SortModeTB.Size = new System.Drawing.Size(134, 20);
            this.SortModeTB.TabIndex = 13;
            this.SortModeTB.TabStop = false;
            this.SortModeTB.Type = GXDLMSDirector.Views.GXValueFieldType.CompoBox;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(5, 224);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(490, 342);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ProfileGenericView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(482, 316);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Data";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ProfileGenericView
            // 
            this.ProfileGenericView.AllowUserToAddRows = false;
            this.ProfileGenericView.AllowUserToDeleteRows = false;
            this.ProfileGenericView.AllowUserToOrderColumns = true;
            this.ProfileGenericView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProfileGenericView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProfileGenericView.Location = new System.Drawing.Point(3, 3);
            this.ProfileGenericView.Name = "ProfileGenericView";
            this.ProfileGenericView.ReadOnly = true;
            this.ProfileGenericView.ShowCellErrors = false;
            this.ProfileGenericView.ShowRowErrors = false;
            this.ProfileGenericView.Size = new System.Drawing.Size(476, 310);
            this.ProfileGenericView.TabIndex = 4;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.m_MyPane);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(482, 316);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Graph";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // m_MyPane
            // 
            this.m_MyPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_MyPane.Location = new System.Drawing.Point(3, 3);
            this.m_MyPane.Name = "m_MyPane";
            this.m_MyPane.ScrollGrace = 0;
            this.m_MyPane.ScrollMaxX = 0;
            this.m_MyPane.ScrollMaxY = 0;
            this.m_MyPane.ScrollMaxY2 = 0;
            this.m_MyPane.ScrollMinX = 0;
            this.m_MyPane.ScrollMinY = 0;
            this.m_MyPane.ScrollMinY2 = 0;
            this.m_MyPane.Size = new System.Drawing.Size(476, 310);
            this.m_MyPane.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ReadLastTB);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.EndEntry);
            this.groupBox2.Controls.Add(this.StartEntry);
            this.groupBox2.Controls.Add(this.DaysLbl);
            this.groupBox2.Controls.Add(this.ToLbl);
            this.groupBox2.Controls.Add(this.ToPick);
            this.groupBox2.Controls.Add(this.StartPick);
            this.groupBox2.Controls.Add(this.ReadFromRB);
            this.groupBox2.Controls.Add(this.ReadLastRB);
            this.groupBox2.Controls.Add(this.ReadEntryBtn);
            this.groupBox2.Location = new System.Drawing.Point(5, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(367, 97);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Reading";
            // 
            // ReadLastTB
            // 
            this.ReadLastTB.Location = new System.Drawing.Point(94, 45);
            this.ReadLastTB.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ReadLastTB.Name = "ReadLastTB";
            this.ReadLastTB.Size = new System.Drawing.Size(114, 20);
            this.ReadLastTB.TabIndex = 12;
            this.ReadLastTB.ValueChanged += new System.EventHandler(this.ReadLastTB_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(192, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Count:";
            // 
            // EndEntry
            // 
            this.EndEntry.Location = new System.Drawing.Point(238, 20);
            this.EndEntry.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.EndEntry.Name = "EndEntry";
            this.EndEntry.Size = new System.Drawing.Size(112, 20);
            this.EndEntry.TabIndex = 11;
            this.EndEntry.ValueChanged += new System.EventHandler(this.StartEntry_ValueChanged);
            // 
            // StartEntry
            // 
            this.StartEntry.Location = new System.Drawing.Point(94, 19);
            this.StartEntry.Name = "StartEntry";
            this.StartEntry.Size = new System.Drawing.Size(86, 20);
            this.StartEntry.TabIndex = 10;
            this.StartEntry.ValueChanged += new System.EventHandler(this.StartEntry_ValueChanged);
            // 
            // DaysLbl
            // 
            this.DaysLbl.Location = new System.Drawing.Point(214, 43);
            this.DaysLbl.Name = "DaysLbl";
            this.DaysLbl.Size = new System.Drawing.Size(72, 16);
            this.DaysLbl.TabIndex = 16;
            this.DaysLbl.Text = "Days";
            // 
            // ToLbl
            // 
            this.ToLbl.AutoSize = true;
            this.ToLbl.Location = new System.Drawing.Point(214, 70);
            this.ToLbl.Name = "ToLbl";
            this.ToLbl.Size = new System.Drawing.Size(20, 13);
            this.ToLbl.TabIndex = 15;
            this.ToLbl.Text = "To";
            // 
            // ToPick
            // 
            this.ToPick.Checked = false;
            this.ToPick.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.ToPick.Location = new System.Drawing.Point(238, 67);
            this.ToPick.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.ToPick.Name = "ToPick";
            this.ToPick.ShowCheckBox = true;
            this.ToPick.Size = new System.Drawing.Size(112, 20);
            this.ToPick.TabIndex = 14;
            this.ToPick.ValueChanged += new System.EventHandler(this.StartPick_ValueChanged);
            // 
            // StartPick
            // 
            this.StartPick.Checked = false;
            this.StartPick.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.StartPick.Location = new System.Drawing.Point(94, 67);
            this.StartPick.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.StartPick.Name = "StartPick";
            this.StartPick.ShowCheckBox = true;
            this.StartPick.Size = new System.Drawing.Size(112, 20);
            this.StartPick.TabIndex = 13;
            this.StartPick.ValueChanged += new System.EventHandler(this.StartPick_ValueChanged);
            // 
            // ReadFromRB
            // 
            this.ReadFromRB.Location = new System.Drawing.Point(6, 67);
            this.ReadFromRB.Name = "ReadFromRB";
            this.ReadFromRB.Size = new System.Drawing.Size(80, 16);
            this.ReadFromRB.TabIndex = 11;
            this.ReadFromRB.Text = "Read From";
            // 
            // ReadLastRB
            // 
            this.ReadLastRB.Checked = true;
            this.ReadLastRB.Location = new System.Drawing.Point(6, 43);
            this.ReadLastRB.Name = "ReadLastRB";
            this.ReadLastRB.Size = new System.Drawing.Size(80, 16);
            this.ReadLastRB.TabIndex = 10;
            this.ReadLastRB.TabStop = true;
            this.ReadLastRB.Text = "Read last";
            // 
            // ReadEntryBtn
            // 
            this.ReadEntryBtn.Location = new System.Drawing.Point(6, 19);
            this.ReadEntryBtn.Name = "ReadEntryBtn";
            this.ReadEntryBtn.Size = new System.Drawing.Size(80, 16);
            this.ReadEntryBtn.TabIndex = 9;
            this.ReadEntryBtn.Text = "Read Entry:";
            // 
            // EntriesTB
            // 
            this.EntriesTB.AttributeID = 8;
            this.EntriesTB.Location = new System.Drawing.Point(186, 143);
            this.EntriesTB.Name = "EntriesTB";
            this.EntriesTB.Size = new System.Drawing.Size(57, 20);
            this.EntriesTB.TabIndex = 7;
            this.EntriesTB.TabStop = false;
            this.EntriesTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // EntriesInUseTB
            // 
            this.EntriesInUseTB.AttributeID = 7;
            this.EntriesInUseTB.Location = new System.Drawing.Point(94, 143);
            this.EntriesInUseTB.Name = "EntriesInUseTB";
            this.EntriesInUseTB.Size = new System.Drawing.Size(66, 20);
            this.EntriesInUseTB.TabIndex = 5;
            this.EntriesInUseTB.TabStop = false;
            this.EntriesInUseTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // LogicalNameLbl
            // 
            this.LogicalNameLbl.AutoSize = true;
            this.LogicalNameLbl.Location = new System.Drawing.Point(10, 120);
            this.LogicalNameLbl.Name = "LogicalNameLbl";
            this.LogicalNameLbl.Size = new System.Drawing.Size(75, 13);
            this.LogicalNameLbl.TabIndex = 0;
            this.LogicalNameLbl.Text = "Logical Name:";
            // 
            // LogicalNameTB
            // 
            this.LogicalNameTB.AttributeID = 1;
            this.LogicalNameTB.Location = new System.Drawing.Point(94, 117);
            this.LogicalNameTB.Name = "LogicalNameTB";
            this.LogicalNameTB.Size = new System.Drawing.Size(140, 20);
            this.LogicalNameTB.TabIndex = 1;
            this.LogicalNameTB.TabStop = false;
            this.LogicalNameTB.Type = GXDLMSDirector.Views.GXValueFieldType.TextBox;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Entries:";
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.Icon = ((System.Drawing.Icon)(resources.GetObject("errorProvider1.Icon")));
            // 
            // GXDLMSProfileGenericView
            // 
            this.ClientSize = new System.Drawing.Size(517, 582);
            this.Controls.Add(this.groupBox1);
            this.Name = "GXDLMSProfileGenericView";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ProfileGenericView)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReadLastTB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndEntry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartEntry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        public GXGroupBy GroupBy
        {
            get;
            set;
        }

        void UpdateView()
        {
            ReadLastTB.ReadOnly = !ReadLastRB.Checked;
            StartPick.Enabled = ToPick.Enabled = ReadFromRB.Checked;
            StartEntry.ReadOnly = EndEntry.ReadOnly = !ReadEntryBtn.Checked;
            if (ReadEntryBtn.Checked)
            {
                m_Target.AccessSelector = AccessRange.Entry;
                StartEntry_ValueChanged(null, null);
            }
            else if (ReadLastRB.Checked)
            {
                m_Target.AccessSelector = AccessRange.Last;
                ReadLastTB_ValueChanged(null, null);                
            }
            else if (ReadFromRB.Checked)
            {
                m_Target.AccessSelector = AccessRange.Range;
                StartPick_ValueChanged(null, null);
            }
        }

        private void ReadAllRB_CheckedChanged(object sender, EventArgs e)
        {
            if (ReadEntryBtn.Checked)
            {
                UpdateView();                
            }
        }

        private void ReadLastRB_CheckedChanged(object sender, EventArgs e)
        {
            if (ReadLastRB.Checked)
            {
                UpdateView();                
            }
        }

        private void ReadFromRB_CheckedChanged(object sender, EventArgs e)
        {
            if (ReadFromRB.Checked)
            {
                UpdateView();                
            }
        }

        private void StartPick_ValueChanged(object sender, EventArgs e)
        {
            if (ReadFromRB.Checked)
            {
                m_Target.From = Convert.ToInt32(StartEntry.Value);
                m_Target.To = Convert.ToInt32(EndEntry.Value);
                if (StartPick.Checked)
                {
                    m_Target.From = StartPick.Value.Date;
                }
                else
                {
                    m_Target.From = DateTime.MinValue;
                }

                if (ToPick.Checked)
                {
                    if (m_Target.CapturePeriod != 0)
                    {
                        m_Target.To = ToPick.Value.Date.AddDays(1).AddSeconds(-m_Target.CapturePeriod);
                    }
                    else
                    {
                        m_Target.To = ToPick.Value.Date.AddDays(1).AddMinutes(-1);
                    }
                }
                else
                {
                    m_Target.To = DateTime.MaxValue;
                }
            }
        }

        private void StartEntry_ValueChanged(object sender, EventArgs e)
        {
            if (ReadEntryBtn.Checked)
            {
                m_Target.From = StartEntry.Value;
                m_Target.To = EndEntry.Value;
            }
        }        

        private void ReadLastTB_ValueChanged(object sender, EventArgs e)
        {
            if (ReadLastRB.Checked)
            {
                m_Target.From = DateTime.Now.Date.AddDays(-Convert.ToInt32(ReadLastTB.Value)).Date;
                m_Target.To = DateTime.Now.AddDays(1).Date;
            }
        }       
    }
}
