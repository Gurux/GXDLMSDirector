//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/MainForm.cs $
//
// Version:         $Revision: 9204 $,
//                  $Date: 2017-02-06 12:36:45 +0200 (ma, 06 helmi 2017) $
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


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using System.Xml;
using GXDLMS.ManufacturerSettings;
using System.Diagnostics;
using System.Threading;
using GXDLMS.Common;
using Gurux.Common;
using MRUSample;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Objects;
using Gurux.DLMS;
using System.Linq;
using Gurux.DLMS.Enums;
using Gurux.DLMS.UI;

namespace GXDLMSDirector
{
    public partial class MainForm : Form
    {
        delegate void CheckUpdatesEventHandler(MainForm form);
        GXAsyncWork TransactionWork;
        Dictionary<Type, IGXDLMSView> Views = new Dictionary<Type, IGXDLMSView>();
        String m_Path;
        GXManufacturerCollection Manufacturers;
        System.Collections.Hashtable ObjectTreeItems = new System.Collections.Hashtable();
        SortedList<int, GXDLMSObject> SelectedListItems = new SortedList<int, GXDLMSObject>();
        System.Collections.Hashtable ObjectListItems = new System.Collections.Hashtable();
        System.Collections.Hashtable ObjectValueItems = new System.Collections.Hashtable();
        System.Collections.Hashtable DeviceListViewItems = new System.Collections.Hashtable();
        GXDLMSDeviceCollection Devices;
        IGXDLMSView SelectedView = null;
        bool Dirty = false;
        delegate void DirtyEventHandler(bool dirty);
        private MRUManager m_MruManager = null;

        GXDLMSDevice GetDevice(GXDLMSObject item)
        {
            return item.Parent.Tag as GXDLMSDevice;
        }

        void OnDirty(bool dirty)
        {
            Dirty = dirty;
            this.Text = Properties.Resources.GXDLMSDirectorTxt + " " + m_Path;
            if (Dirty)
            {
                this.Text += " *";
            }
        }

        internal void SetDirty(bool dirty)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new DirtyEventHandler(OnDirty), dirty);
            }
            else
            {
                OnDirty(dirty);
            }
        }

        public static void InitMain()
        {
            //Debug traces are written only log file.
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Trace.Listeners.Clear();
            }
            DirectoryInfo di = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName((GXLogWriter.LogPath)));
            if (!di.Exists)
            {
                di.Create();
            }
            GXLogWriter.ClearLog();
            System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(GXLogWriter.LogPath));
            System.Diagnostics.Trace.AutoFlush = true;
            System.Diagnostics.Trace.IndentSize = 4;
            if (Environment.OSVersion.Platform != PlatformID.Unix)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
            }
            MainForm form = new MainForm();
            Application.Run(form);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// Do not catch errors here.
        /// If error is occurred exception is shown and application is closed.
        /// </remarks>
        public MainForm()
        {
            InitializeComponent();
            CancelBtn.Enabled = false;

            Devices = new GXDLMSDeviceCollection();
            ObjectValueView.Visible = DeviceInfoView.Visible = DeviceList.Visible = false;
            ObjectValueView.Dock = ObjectPanelFrame.Dock = DeviceList.Dock = DeviceInfoView.Dock = DockStyle.Fill;
            ProgressBar.Visible = false;
            UpdateDeviceUI(null, DeviceState.None);
            Initialize();
            m_MruManager = new MRUManager(RecentFilesMnu);
            m_MruManager.OnOpenMRUFile += new OpenMRUFileEventHandler(this.OnOpenMRUFile);
        }

        public void OnProgress(object sender, string description, int current, int maximium)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ProgressEventHandler(OnProgress), sender, description, current, maximium);
            }
            else
            {
                if (this.Created)
                {
                    if (current > maximium)
                    {
                        maximium = current;
                    }
                    StatusLbl.Text = description;
                    ProgressBar.Maximum = maximium;
                    ProgressBar.Value = current;
                }
            }
        }

        public void StatusChanged(object sender, DeviceState state)
        {
            UpdateDeviceUI((GXDLMSDevice)sender, state);
        }

        public void OnStatusChanged(object sender, DeviceState state)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new StatusEventHandler(StatusChanged), sender, state);
            }
            else
            {
                StatusChanged(sender, state);
            }
        }

        /// <summary>
        /// Is write enabled.
        /// </summary>
        /// <remarks>
        /// Write is enabled for only some interfaces.
        /// </remarks>
        /// <returns></returns>
        void UpdateWriteEnabled()
        {
            if (SelectedView != null && SelectedView.Target.GetDirtyAttributeIndexes().Length != 0 && ReadCMnu.Enabled)
            {
                WriteBtn.Enabled = WriteMnu.Enabled = true;
            }
            else
            {
                WriteBtn.Enabled = WriteMnu.Enabled = false;
            }
        }

        void UpdateDeviceUI(GXDLMSDevice device, DeviceState state)
        {
            ConnectCMnu.Enabled = ConnectBtn.Enabled = ConnectMnu.Enabled = (state & DeviceState.Initialized) != 0;
            ReadCMnu.Enabled = ReadBtn.Enabled = RefreshMnu.Enabled = ReadMnu.Enabled = (state & DeviceState.Connected) == DeviceState.Connected;
            DisconnectCMnu.Enabled = DisconnectMnu.Enabled = ConnectBtn.Checked = (state & DeviceState.Connected) == DeviceState.Connected;
            DeleteCMnu.Enabled = DeleteMnu.Enabled = DeleteBtn.Enabled = OptionsBtn.Enabled = state == DeviceState.Initialized;
            UpdateWriteEnabled();
        }


        private void SelectItem(object obj)
        {
            try
            {
                if (SelectedView != null && SelectedView.Target != null)
                {
                    SelectedView.Target.OnChange -= new ObjectChangeEventHandler(DLMSItemOnChange);
                }
                //Do not shown list if there is only one item in the list.
                if (obj is GXDLMSObjectCollection)
                {
                    GXDLMSObjectCollection items = obj as GXDLMSObjectCollection;
                    if (items.Count == 1)
                    {
                        obj = items[0];
                    }
                }

                DeviceList.Visible = obj is GXDLMSDeviceCollection;
                //If device is selected.
                DeviceInfoView.Visible = obj is GXDLMSDevice;
                ObjectPanelFrame.Visible = obj is GXDLMSObject;
                ObjectValueView.Visible = obj is GXDLMSObjectCollection;
                if (DeviceList.Visible)
                {
                    DeviceState state = Devices.Count == 0 ? DeviceState.None : DeviceState.Initialized;
                    UpdateDeviceUI(null, state);
                    DeleteMnu.Enabled = DeleteBtn.Enabled = OptionsBtn.Enabled = false;
                }
                else if (DeviceInfoView.Visible)
                {
                    GXDLMSDevice dev = (GXDLMSDevice)obj;
                    DeviceGb.Text = dev.Name;
                    StatusValueLbl.Text = dev.Status.ToString();
                    ClientAddressValueLbl.Text = dev.ClientAddress.ToString();
                    LogicalAddressValueLbl.Text = dev.LogicalAddress.ToString();
                    PhysicalAddressValueLbl.Text = dev.PhysicalAddress.ToString();
                    ManufacturerValueLbl.Text = dev.Manufacturers.FindByIdentification(dev.Manufacturer).Name;
                    UpdateDeviceUI(dev, dev.Status);
                }
                else if (ObjectValueView.Visible)
                {
                    try
                    {
                        ObjectValueView.BeginUpdate();
                        ObjectValueItems.Clear();
                        ObjectValueView.Items.Clear();
                        int maxAttributeCount = ObjectValueView.Columns.Count - 2;
                        int originalCount = maxAttributeCount;
                        int maxColCount = 0;
                        int len = 0;
                        GXDLMSObjectCollection items = obj as GXDLMSObjectCollection;
                        List<ListViewItem> rows = new List<ListViewItem>(items.Count);
                        foreach (GXDLMSObject it in items)
                        {
                            object[] values = it.GetValues();
                            len = values.Length - 1;
                            if (len > maxColCount)
                            {
                                maxColCount = len;
                            }
                            if (len > maxAttributeCount)
                            {
                                for (int pos = maxAttributeCount; pos != len; ++pos)
                                {
                                    ObjectValueView.Columns.Add("Attribute " + (pos + 2).ToString());
                                    ++maxAttributeCount;
                                }
                            }
                            ListViewItem lv = new ListViewItem(it.LogicalName + " " + it.Description);
                            lv.SubItems.Add(it.ObjectType.ToString());
                            string[] texts = new string[len];
                            for (int pos = 0; pos != len; ++pos)
                            {
                                texts[pos] = "";
                            }
                            lv.SubItems.AddRange(texts);
                            ObjectValueItems[it] = lv;
                            lv.Tag = it;
                            for (int pos = 0; pos != len; ++pos)
                            {
                                UpdateValue(it, pos + 2, lv);
                            }
                            rows.Add(lv);
                        }
                        ObjectValueView.Items.AddRange(rows.ToArray());
                        int cnt = ObjectValueView.Columns.Count - maxColCount - 2;
                        for (int pos = 0; pos < cnt; ++pos)
                        {
                            ObjectValueView.Columns.RemoveAt(ObjectValueView.Columns.Count - 1);
                        }
                        for (int pos = originalCount + 1; pos < ObjectValueView.Columns.Count; ++pos)
                        {
                            ObjectValueView.Columns[pos].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                        }
                        if (items.Count != 0)
                        {
                            GXDLMSObject obj2 = items[0] as GXDLMSObject;
                            GXDLMSDevice dev = obj2.Parent.Tag as GXDLMSDevice;
                            UpdateDeviceUI(dev, dev.Status);
                        }
                    }
                    finally
                    {
                        ObjectValueView.EndUpdate();
                    }
                }
                else
                {
                    SelectedView = Views[obj.GetType()];
                    foreach (Control it in ObjectPanelFrame.Controls)
                    {
                        it.Hide();
                    }
                    ObjectPanelFrame.Controls.Clear();
                    SelectedView.Target = (GXDLMSObject)obj;
                    SelectedView.Target.OnChange += new ObjectChangeEventHandler(DLMSItemOnChange);
                    UpdateProperties(obj, SelectedView, new List<object>(), 0);
                    ObjectPanelFrame.Controls.Add((Form)SelectedView);
                    ((Form)SelectedView).Show();
                    UpdateDeviceUI(GetDevice(SelectedView.Target), GetDevice((GXDLMSObject)obj).Status);
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void ObjectTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            while (SelectedListItems.Count != 0)
            {
                GXDLMSObject it = SelectedListItems.Values[0];
                ListViewItem item = ObjectListItems[it] as ListViewItem;
                item.Selected = false;
            }
            SelectedListItems.Clear();
            SelectItem(e.Node.Tag);
            ObjectTree.Focus();
            try
            {
                this.ObjectList.ItemSelectionChanged -= new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ObjectList_ItemSelectionChanged);
                GXDLMSObject obj = e.Node.Tag as GXDLMSObject;
                if (obj != null)
                {
                    ListViewItem item = ObjectListItems[e.Node.Tag] as ListViewItem;
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }
            }
            finally
            {
                this.ObjectList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ObjectList_ItemSelectionChanged);
            }
        }

        private static bool UpdateDirty(IGXDLMSView view, System.Windows.Forms.Control.ControlCollection controls, GXDLMSObject target, int index, bool dirty)
        {
            bool found = false;
            foreach (Control it in controls)
            {
                if (it is GXValueField)
                {
                    GXValueField obj = it as GXValueField;
                    if (obj.Index == index)
                    {
                        if (dirty && index != 0)
                        {
                            view.ErrorProvider.SetError(it, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                        }
                        else
                        {
                            view.ErrorProvider.Clear();
                        }
                        found = true;
                    }
                }
                else if (it.Controls.Count != 0)
                {
                    found = UpdateDirty(view, it.Controls, target, index, dirty);
                }
                if (found)
                {
                    break;
                }
            }
            return found;
        }


        void DLMSItemOnChange(GXDLMSObject sender, bool dirty, int attributeIndex, object value)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new ObjectChangeEventHandler(DLMSItemOnChange), sender, dirty, attributeIndex, value);
            }
            else
            {
                if (SelectedView != null && SelectedView.Target == sender)
                {
                    UpdateDirty(SelectedView, ((Form)SelectedView).Controls, sender, attributeIndex, dirty);
                }
                TreeNode node = ObjectTreeItems[sender] as TreeNode;
                if (node != null)
                {
                    if (sender is GXDLMSProfileGeneric)
                    {
                        node.ImageIndex = node.SelectedImageIndex = 8;
                    }
                    else
                    {
                        node.ImageIndex = node.SelectedImageIndex = dirty ? 10 : 7;
                    }
                }
                UpdateWriteEnabled();
            }
        }

        static List<GXButton> ActionList = new List<GXButton>();

        private static bool UpdateProperties(IGXDLMSView view, System.Windows.Forms.Control.ControlCollection controls, GXDLMSObject target, int index, object value)
        {
            foreach (GXButton it in ActionList)
            {
                it.Click -= new EventHandler(OnAction);
            }
            ActionList.Clear();
            bool found = false;
            foreach (Control it in controls)
            {
                if (it is GXValueField)
                {
                    GXValueField obj = it as GXValueField;
                    if (obj.Index == index)
                    {
                        obj.Target = target;
                        obj.UpdateValueItems(target, index, value);
                        obj.Value = value;
                        found = true;
                    }
                }
                else if (it is GXButton)
                {
                    GXButton obj = it as GXButton;
                    bool enabled = target.GetMethodAccess(obj.Index) != MethodAccessMode.NoAccess;
                    obj.Enabled = enabled;
                    if (enabled)
                    {
                        obj.Target = target;
                        it.Click += new EventHandler(OnAction);
                        ActionList.Add(obj);
                    }
                }
                else if (it.Controls.Count != 0)
                {
                    found = UpdateProperties(view, it.Controls, target, index, value);
                }
                if (found)
                {
                    break;
                }
            }
            return found;
        }

        static void OnAction(object sender, EventArgs e)
        {
            GXButton obj = sender as GXButton;
            try
            {
                GXReplyData reply = new GXReplyData();
                GXDLMSDevice dev = obj.Target.Parent.Tag as GXDLMSDevice;
                dev.Comm.MethodRequest(obj.Target, obj.Index, null, reply);
            }
            catch (Exception ex)
            {
                MessageBox.Show(obj, ex.Message);
            }
        }

        delegate void ValueChangedEventHandler(IGXDLMSView view, int index, object value, bool changeByUser);

        static void OnValueChanged(IGXDLMSView view, int index, object value, bool changeByUser)
        {
            view.OnValueChanged(index, value, changeByUser);
        }

        private void UpdateProperties(object obj, IGXDLMSView view, List<object> UpdatedObjects, int index)
        {
            if (obj == null)
            {
                return;
            }
            UpdateWriteEnabled();
            GXDLMSObject tmp = view.Target;
            view.Description = tmp.Description;
            if (view.ErrorProvider != null)
            {
                view.ErrorProvider.Clear();
                foreach (int it in tmp.GetDirtyAttributeIndexes())
                {
                    UpdateDirty(view, ((Form)view).Controls, tmp, it, true);
                }
            }
            bool InvokeRequired = ((Form)view).InvokeRequired;
            for (int it = 1; it != (obj as IGXDLMSBase).GetAttributeCount() + 1; ++it)
            {
                if (index == 0 || index == it)
                {
                    object value = null;
                    bool dirty = view.Target.GetDirty(it, out value);
                    value = view.Target.GetValues()[it - 1];
                    bool bFound = UpdateProperties(view, ((Form)view).Controls, view.Target, it, value);
                    if (!bFound)
                    {
                        view.OnAccessRightsChange(it, view.Target.GetAccess(it));
                    }
                    if (!bFound)
                    {
                        if (InvokeRequired)
                        {
                            ((Form)view).Invoke(new ValueChangedEventHandler(OnValueChanged), new object[] { view, it, value, dirty });
                        }
                        else
                        {
                            view.OnValueChanged(it, value, false);
                        }
                    }
                }
            }
        }

        void Initialize()
        {
            m_Path = "";
            ObjectTreeItems.Clear();
            SelectedListItems.Clear();
            ObjectListItems.Clear();
            ObjectValueItems.Clear();
            foreach (GXDLMSDevice it in Devices)
            {
                it.Disconnect();
            }
            Devices.Clear();
            ObjectTree.Nodes.Clear();
            DeviceListViewItems.Clear();
            DeviceList.Items.Clear();
            ObjectList.Items.Clear();
            ObjectList.Groups.Clear();
            ObjectValueView.Items.Clear();
            TreeNode node = ObjectTree.Nodes.Add("Devices");
            node.Tag = Devices;
            node.SelectedImageIndex = node.ImageIndex = 0;
            ObjectTree.SelectedNode = node;
            SetDirty(false);
        }

        /// <summary>
        /// Create new device list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewMnu_Click(object sender, EventArgs e)
        {
            try
            {
                Initialize();
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        GXDLMSDevice GetSelectedDevice()
        {
            if (this.ObjectTree.SelectedNode != null)
            {
                if (this.ObjectTree.SelectedNode.Tag is GXDLMSDeviceCollection)
                {
                    //If device list is selected.
                    return null;
                }
                else if (this.ObjectTree.SelectedNode.Tag is GXDLMSDevice)
                {
                    return this.ObjectTree.SelectedNode.Tag as GXDLMSDevice;
                }
                else if (this.ObjectTree.SelectedNode.Tag is GXDLMSObject)
                {
                    return ((GXDLMSObject)this.ObjectTree.SelectedNode.Tag).Parent.Tag as GXDLMSDevice;
                }
                else
                {
                    return (GXDLMSDevice)this.ObjectTree.SelectedNode.Parent.Tag;
                }
            }
            return null;
        }

        /// <summary>
        /// Show properties of the selected media.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PropertiesMnu_Click(object sender, EventArgs e)
        {
            try
            {
                GXDLMSDevice dev = GetSelectedDevice();
                if (dev != null)
                {
                    string man = dev.Manufacturer;
                    DevicePropertiesForm dlg = new DevicePropertiesForm(this.Manufacturers, dev);
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        //If user has change meter manufacturer.
                        if (man != dev.Manufacturer)
                        {
                            while (dev.Objects.Count != 0)
                            {
                                RemoveObject(dev.Objects[0]);
                            }
                            SelectItem(dev);
                        }
                        ((TreeNode)ObjectTreeItems[dev]).Text = dev.Name;
                        SetDirty(true);
                    }
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Toggle toolbar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewToolbarMnu_Click(object sender, EventArgs e)
        {
            try
            {
                ViewToolbarMnu.Checked = !ViewToolbarMnu.Checked;
                toolStrip1.Visible = ViewToolbarMnu.Checked;
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Toggle statusbar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewStatusbarMnu_Click(object sender, EventArgs e)
        {
            try
            {
                ViewStatusbarMnu.Checked = !ViewStatusbarMnu.Checked;
                statusStrip1.Visible = ViewStatusbarMnu.Checked;
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Show About dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutMnu_Click(object sender, EventArgs e)
        {
            try
            {
                GXAboutForm lic = new GXAboutForm();
                lic.Title = GXDLMSDirector.Properties.Resources.AboutGXDLMSDirectorTxt;
                lic.Application = GXDLMSDirector.Properties.Resources.GXDLMSDirectorTxt;
                lic.AboutText = GXDLMSDirector.Properties.Resources.ForMoreInfoTxt;
                lic.CopyrightText = GXDLMSDirector.Properties.Resources.CopyrightTxt;
                //Get version info
                System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
                System.Diagnostics.FileVersionInfo info = System.Diagnostics.FileVersionInfo.GetVersionInfo(asm.Location);
                lic.ShowAbout(this, info.FileVersion);
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void SaveFile(string path)
        {
            Stream stream = File.Open(path, FileMode.Create);
            GXFileSystemSecurity.UpdateFileSecurity(path);
            List<Type> types = new List<Type>(Gurux.DLMS.GXDLMSClient.GetObjectTypes());
            types.Add(typeof(GXDLMSAttributeSettings));
            types.Add(typeof(GXDLMSAttribute));
            using (TextWriter writer = new StreamWriter(stream))
            {
                XmlAttributeOverrides overrides = new XmlAttributeOverrides();
                XmlAttributes attribs = new XmlAttributes();
                attribs.XmlIgnore = true;
                overrides.Add(typeof(GXDLMSDevice), "ObsoleteObjects", attribs);
                overrides.Add(typeof(GXDLMSAttributeSettings), attribs);
                XmlSerializer x = new XmlSerializer(typeof(GXDLMSDeviceCollection), overrides, types.ToArray(), null, "Gurux1");
                x.Serialize(writer, Devices);
                writer.Close();
            }
            stream.Close();
            SetDirty(false);
            m_MruManager.Insert(0, path);
        }

        private bool Save()
        {
            if (string.IsNullOrEmpty(m_Path))
            {
                return SaveAs();
            }
            SaveFile(m_Path);
            return true;
        }

        private bool SaveAs()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = Properties.Resources.FilterTxt;
            dlg.DefaultExt = ".gxc";
            dlg.InitialDirectory = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                this.m_Path = dlg.FileName;
                SaveFile(dlg.FileName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Save media settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveMnu_Click(object sender, EventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void SaveAsMnu_Click(object sender, EventArgs e)
        {
            try
            {
                SaveAs();
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        void Connect(System.Windows.Forms.Control sender, object[] parameters)
        {
            try
            {
                object obj = parameters[0];
                int pos = 0;
                int cnt;
                if (obj is GXDLMSDeviceCollection)
                {
                    cnt = ((GXDLMSDeviceCollection)obj).Count;
                    foreach (GXDLMSDevice it in (GXDLMSDeviceCollection)obj)
                    {
                        if (!it.Media.IsOpen)
                        {
                            try
                            {
                                it.OnTrace += new MessageTraceEventHandler(OnTrace);
                                this.OnProgress(null, "Connecting", ++pos, cnt);
                                it.InitializeConnection();
                            }
                            finally
                            {
                                it.OnTrace -= new MessageTraceEventHandler(OnTrace);
                            }
                        }
                    }
                }
                else if (obj is GXDLMSDevice)
                {
                    if (!((GXDLMSDevice)obj).Media.IsOpen)
                    {
                        try
                        {
                            ((GXDLMSDevice)obj).OnTrace += new MessageTraceEventHandler(OnTrace);
                            this.OnProgress(null, "Connecting", 0, 1);
                            ((GXDLMSDevice)obj).InitializeConnection();
                        }
                        finally
                        {
                            ((GXDLMSDevice)obj).OnTrace -= new MessageTraceEventHandler(OnTrace);
                        }
                    }
                }
                else if (obj is GXDLMSObjectCollection)
                {
                    GXDLMSObjectCollection tmp = obj as GXDLMSObjectCollection;
                    GXDLMSDeviceCollection devices = new GXDLMSDeviceCollection();
                    foreach (GXDLMSObject it in tmp)
                    {
                        GXDLMSDevice dev = it.Parent.Tag as GXDLMSDevice;
                        if (!devices.Contains(dev))
                        {
                            devices.Add(dev);
                        }
                    }
                    Connect(sender, new object[] { devices });
                }
                else
                {
                    this.OnProgress(null, "Connecting", 0, 1);
                    GXDLMSObject tmp = obj as GXDLMSObject;
                    GXDLMSDevice dev = tmp.Parent.Tag as GXDLMSDevice;
                    if (!dev.Media.IsOpen)
                    {
                        try
                        {
                            dev.OnTrace += new MessageTraceEventHandler(OnTrace);
                            dev.InitializeConnection();
                        }
                        finally
                        {
                            dev.OnTrace -= new MessageTraceEventHandler(OnTrace);
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            {
                //User has cancel action. Do nothing.
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(sender, Ex);
            }
            finally
            {
                this.OnProgress(null, "Connecting", 1, 1);
            }
        }

        /// <summary>
        /// Connect to the selected device.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectMnu_Click(object sender, EventArgs e)
        {
            ClearTrace();
            if (tabControl1.SelectedIndex == 0)
            {
                TransactionWork = new GXAsyncWork(this, OnAsyncStateChange, Connect, new object[] { ObjectTree.SelectedNode.Tag });
            }
            else
            {
                TransactionWork = new GXAsyncWork(this, OnAsyncStateChange, Connect, new object[] { GetDevices() });
            }
            TransactionWork.Start();
        }

        void Disconnect(System.Windows.Forms.Control sender, object[] parameters)
        {
            try
            {
                ClearTrace();
                int pos = 0;
                int cnt;
                object obj = parameters[0];
                if (obj is GXDLMSDeviceCollection)
                {
                    cnt = ((GXDLMSDeviceCollection)obj).Count;
                    foreach (GXDLMSDevice it in (GXDLMSDeviceCollection)obj)
                    {
                        sender.BeginInvoke(new ProgressEventHandler(this.OnProgress), null, "Disconnecting", ++pos, cnt);
                        try
                        {
                            it.OnTrace += new MessageTraceEventHandler(OnTrace);
                            it.Disconnect();
                        }
                        finally
                        {
                            it.OnTrace -= new MessageTraceEventHandler(OnTrace);
                        }
                    }
                }
                else if (obj is GXDLMSDevice)
                {
                    sender.BeginInvoke(new ProgressEventHandler(this.OnProgress), null, "Disconnecting", 0, 1);
                    try
                    {
                        ((GXDLMSDevice)obj).OnTrace += new MessageTraceEventHandler(OnTrace);
                        ((GXDLMSDevice)obj).Disconnect();
                    }
                    finally
                    {
                        ((GXDLMSDevice)obj).OnTrace -= new MessageTraceEventHandler(OnTrace);
                    }
                }
                else if (obj is GXDLMSObjectCollection)
                {
                    GXDLMSObjectCollection tmp = obj as GXDLMSObjectCollection;
                    GXDLMSDeviceCollection devices = new GXDLMSDeviceCollection();
                    foreach (GXDLMSObject it in tmp)
                    {
                        GXDLMSDevice dev = it.Parent.Tag as GXDLMSDevice;
                        if (!devices.Contains(dev))
                        {
                            devices.Add(dev);
                        }
                    }
                    Disconnect(sender, new object[] { devices });
                }
                else
                {
                    sender.BeginInvoke(new ProgressEventHandler(this.OnProgress), null, "Disconnecting", 0, 1);
                    GXDLMSObject tmp = obj as GXDLMSObject;
                    GXDLMSDevice dev = tmp.Parent.Tag as GXDLMSDevice;
                    try
                    {
                        dev.OnTrace += new MessageTraceEventHandler(OnTrace);
                        dev.Disconnect();
                    }
                    finally
                    {
                        dev.OnTrace -= new MessageTraceEventHandler(OnTrace);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                //User has cancel action. Do nothing.
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(sender, Ex);
            }
            finally
            {
                sender.BeginInvoke(new ProgressEventHandler(this.OnProgress), null, "", 1, 1);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        GXDLMSDeviceCollection GetDevices()
        {
            GXDLMSDeviceCollection devices = new GXDLMSDeviceCollection();
            foreach (GXDLMSObject it in SelectedListItems.Values)
            {
                GXDLMSDevice dev = it.Parent.Tag as GXDLMSDevice;
                if (!devices.Contains(dev))
                {
                    devices.Add(dev);
                }
            }
            //If no device item is selected return all devices.
            if (devices.Count == 0)
            {
                return this.Devices;
            }
            return devices;
        }

        /// <summary>
        /// Disconnect to the selected device.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisconnectMnu_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                TransactionWork = new GXAsyncWork(this, OnAsyncStateChange, Disconnect, new object[] { ObjectTree.SelectedNode.Tag });
            }
            else
            {
                TransactionWork = new GXAsyncWork(this, OnAsyncStateChange, Disconnect, new object[] { GetDevices() });
            }
            TransactionWork.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //Save changes?
                if (this.Dirty)
                {
                    DialogResult ret = MessageBox.Show(this, Properties.Resources.SaveChangesTxt, Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (ret == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                    if (ret == DialogResult.Yes)
                    {
                        if (!Save())
                        {
                            e.Cancel = true;
                        }
                    }
                }
                if (e.Cancel)
                {
                    return;
                }
                SaveXmlPositioning();
                SetDirty(false);
                Manufacturers.WriteManufacturerSettings();
                foreach (GXDLMSDevice it in Devices)
                {
                    it.Disconnect();
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        #region XML positioning

        /// <summary>
        /// Retrieves application data path from environment variables.
        /// </summary>
        public static string UserDataPath
        {
            get
            {
                string path;
                if (System.Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                }
                else
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                }
                return path;
            }
        }

        /// <summary>
        /// Saves window positioning to a xml-file
        /// </summary>
        private void SaveXmlPositioning()
        {
            try
            {
                GXDLMSDirector.Properties.Settings.Default.ViewToolbar = ViewToolbarMnu.Checked;
                GXDLMSDirector.Properties.Settings.Default.ViewStatusbar = ViewStatusbarMnu.Checked;
                GXDLMSDirector.Properties.Settings.Default.ViewTree = ObjectTreeMnu.Checked;
                GXDLMSDirector.Properties.Settings.Default.ViewList = ObjectListMnu.Checked;
                GXDLMSDirector.Properties.Settings.Default.ViewGroups = GroupsMnu.Checked;
                GXDLMSDirector.Properties.Settings.Default.ViewTrace = TraceMnu.Checked;
                GXDLMSDirector.Properties.Settings.Default.Save();

                string path = UserDataPath;
                if (System.Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    path = Path.Combine(path, ".Gurux");
                }
                else
                {
                    path = Path.Combine(path, "Gurux");
                }
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, "GXDLMSDirector.xml");
                using (XmlTextWriter xtw = new XmlTextWriter(File.CreateText(path)))
                {
                    xtw.WriteStartDocument();
                    xtw.WriteStartElement("GXDLMSDirector");
                    xtw.WriteStartElement("MRU");
                    foreach (string it in m_MruManager.GetNames())
                    {
                        xtw.WriteElementString("item", it);
                    }
                    xtw.WriteEndElement();
                    xtw.WriteStartElement("MainWindow");
                    xtw.WriteAttributeString("State", ((int)this.WindowState).ToString());
                    if (this.WindowState == FormWindowState.Normal)
                    {
                        xtw.WriteAttributeString("X", this.Bounds.X.ToString());
                        xtw.WriteAttributeString("Y", this.Bounds.Y.ToString());
                        xtw.WriteAttributeString("Width", this.Bounds.Width.ToString());
                        xtw.WriteAttributeString("Height", this.Bounds.Height.ToString());
                    }
                    xtw.WriteEndElement();
                    xtw.WriteStartElement("Views");
                    Rectangle rc = this.tabControl1.Bounds;
                    xtw.WriteAttributeString("X", rc.X.ToString());
                    xtw.WriteAttributeString("Y", rc.Y.ToString());
                    xtw.WriteAttributeString("Width", rc.Width.ToString());
                    xtw.WriteAttributeString("Height", rc.Height.ToString());
                    xtw.WriteEndElement();
                    xtw.WriteEndDocument();
                    xtw.Close();
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Loads window positioning from a xml-file.
        /// </summary>
        private void LoadXmlPositioning()
        {
            try
            {
                ViewToolbarMnu.Checked = !GXDLMSDirector.Properties.Settings.Default.ViewToolbar;
                ViewToolbarMnu_Click(null, null);
                ViewStatusbarMnu.Checked = !GXDLMSDirector.Properties.Settings.Default.ViewStatusbar;
                ViewStatusbarMnu_Click(null, null);
                ObjectTreeMnu.Checked = !GXDLMSDirector.Properties.Settings.Default.ViewTree;
                ObjectTreeMnu_Click(null, null);
                ObjectListMnu.Checked = !GXDLMSDirector.Properties.Settings.Default.ViewList;
                ObjectListMnu_Click(null, null);
                GroupsMnu.Checked = !GXDLMSDirector.Properties.Settings.Default.ViewGroups;
                GroupsMnu_Click(null, null);
                TraceMnu.Checked = !GXDLMSDirector.Properties.Settings.Default.ViewTrace;
                TraceMenu_Click(null, null);

                string path = UserDataPath;
                if (System.Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    path = Path.Combine(path, ".Gurux");
                }
                else
                {
                    path = Path.Combine(path, "Gurux");
                }
                path = Path.Combine(path, "GXDLMSDirector.xml");
                if (!File.Exists(path))
                {
                    return;
                }
                using (XmlTextReader xtr = new XmlTextReader(File.OpenText(path)))
                {
                    while (xtr.Read())
                    {
                        if (xtr.Name == "General")
                        {
                        }
                        else if (xtr.Name == "MRU" && !xtr.IsEmptyElement)
                        {
                            xtr.Read();
                            while (xtr.Name != "MRU" && xtr.NodeType != XmlNodeType.EndElement)
                            {
                                m_MruManager.Insert(-1, xtr.ReadElementString());
                            }
                        }
                        else if (xtr.Name == "MainWindow")
                        {
                            string val = xtr.GetAttribute("State");
                            if (val != null)
                            {
                                FormWindowState state = (FormWindowState)int.Parse(val);
                                if (state == FormWindowState.Normal)
                                {
                                    this.SetBoundsCore(int.Parse(xtr.GetAttribute("X")),
                                                       int.Parse(xtr.GetAttribute("Y")),
                                                       int.Parse(xtr.GetAttribute("Width")),
                                                       int.Parse(xtr.GetAttribute("Height")), BoundsSpecified.All);
                                }
                                //If dual display and other display is removed.
                                Rectangle rc = Screen.GetWorkingArea(this);
                                if (this.Left > rc.Width)
                                {
                                    this.Left = 0;
                                }
                                if (this.Top > rc.Height)
                                {
                                    this.Top = 0;
                                }
                            }
                        }
                        else if (xtr.Name == "Views")
                        {
                            tabControl1.SetBounds(int.Parse(xtr.GetAttribute("X")),
                                                  int.Parse(xtr.GetAttribute("Y")),
                                                  int.Parse(xtr.GetAttribute("Width")),
                                                  int.Parse(xtr.GetAttribute("Height")));
                        }
                    }
                    xtr.Close();
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        #endregion

        void ReadDevices()
        {
            foreach (GXDLMSDevice it in Devices)
            {
                ReadDevice(it);
            }
        }

        void ReadDevice(GXDLMSDevice dev)
        {
            try
            {
                dev.Comm.OnBeforeRead += new ReadEventHandler(OnBeforeRead);
                dev.Comm.OnAfterRead += new ReadEventHandler(OnAfterRead);
                dev.OnTrace += new MessageTraceEventHandler(OnTrace);
                dev.KeepAliveStop();
                int cnt = dev.Objects.Count;
                if (cnt == 0)
                {
                    RefreshDevice(dev, true);
                    //Do not try to refresh and read values at the same time.
                    return;
                }
                int pos = 0;
                foreach (GXDLMSObject it in dev.Objects)
                {
                    OnProgress(dev, "Reading " + it.LogicalName + "...", ++pos, cnt);
                    dev.Comm.Read(this, it, 0);
                    DLMSItemOnChange(it, false, 0, null);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                dev.Comm.OnBeforeRead -= new ReadEventHandler(OnBeforeRead);
                dev.Comm.OnAfterRead -= new ReadEventHandler(OnAfterRead);
                dev.OnTrace -= new MessageTraceEventHandler(OnTrace);
                dev.KeepAliveStart();
            }
        }

        delegate void UpdateTransactionEventHandler(bool start);

        void OnTrace(GXDLMSDevice sender, string trace)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MessageTraceEventHandler(this.OnTrace), sender, trace);
            }
            else
            {
                TraceView.AppendText(trace + Environment.NewLine);
            }
        }

        void UpdateTransaction(bool start)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateTransactionEventHandler(this.UpdateTransaction), start);
            }
            else
            {
                ReadBtn.Enabled = ReadMnu.Enabled = !start;
                if (!start)
                {
                    UpdateWriteEnabled();
                }
                else
                {
                    WriteBtn.Enabled = WriteMnu.Enabled = !start;
                }
            }
        }

        public void OnBeforeRead(GXDLMSObject sender, int index)
        {
            try
            {
                if ((index == 2 || index == 3) && !this.IsDisposed && sender is GXDLMSProfileGeneric)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new ReadEventHandler(OnBeforeRead), new object[] { sender, index });
                    }
                    else if (SelectedView != null && SelectedView.Target == sender)
                    {
                        GXDLMSProfileGeneric pg = sender as GXDLMSProfileGeneric;
                        pg.Buffer.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void UpdateValue(GXDLMSObject sender, int index, ListViewItem lv)
        {
            if (index != 1)
            {
                object value = sender.GetValues()[index - 1];
                string str;
                if (value != null && value.GetType().IsArray)
                {
                    str = Convert.ToString(GXDLMS.Common.GXHelpers.ConvertFromDLMS(value, DataType.None, DataType.None, true));
                }
                else
                {
                    str = GXHelpers.ConvertDLMS2String(value);
                }
                lv.SubItems[index].Text = str;
            }
        }

        public void OnAfterRead(GXDLMSObject sender, int index)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new ReadEventHandler(OnAfterRead), new object[] { sender, index });
                    return;
                }
                if (SelectedView != null && SelectedView.Target == sender)
                {
                    UpdateProperties(sender, SelectedView, new List<object>(), index);
                }
                ListViewItem lv = ObjectValueItems[sender] as ListViewItem;
                if (lv != null)
                {
                    UpdateValue(sender, index, lv);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Read(System.Windows.Forms.Control sender, object[] parameters)
        {
            GXDLMSDevice dev = null;
            try
            {
                ClearTrace();
                UpdateTransaction(true);
                object item = parameters[0];
                IGXDLMSView SelectedView = parameters[1] as IGXDLMSView;
                if (item != null)
                {
                    //Read all objects if device is selected.
                    if (item is GXDLMSDeviceCollection)
                    {
                        ReadDevices();
                    }
                    else if (item is GXDLMSDevice)
                    {
                        dev = item as GXDLMSDevice;
                        ReadDevice(dev);
                    }
                    else if (item is GXDLMSObject)
                    {
                        GXDLMSObject obj = item as GXDLMSObject;
                        dev = obj.Parent.Tag as GXDLMSDevice;
                        IGXDLMSView view = Views[obj.GetType()];
                        dev.KeepAliveStop();
                        this.OnProgress(dev, "Reading...", 0, 1);
                        try
                        {
                            dev.Comm.OnBeforeRead += new ReadEventHandler(OnBeforeRead);
                            dev.Comm.OnAfterRead += new ReadEventHandler(OnAfterRead);
                            dev.OnTrace += new MessageTraceEventHandler(OnTrace);
                            dev.Comm.Read(this, obj, 0);
                        }
                        finally
                        {
                            dev.Comm.OnBeforeRead -= new ReadEventHandler(OnBeforeRead);
                            dev.Comm.OnAfterRead -= new ReadEventHandler(OnAfterRead);
                            dev.OnTrace -= new MessageTraceEventHandler(OnTrace);
                        }
                        DLMSItemOnChange(obj, false, 0, null);
                        dev.KeepAliveStart();
                        //Draw graph again...
                        if (view is GXDLMSProfileGenericView)
                        {
                            view.Target = obj;
                        }
                    }
                    else if (item is GXDLMSObjectCollection)
                    {
                        GXDLMSObjectCollection items = item as GXDLMSObjectCollection;
                        dev = items[0].Parent.Tag as GXDLMSDevice;
                        dev.KeepAliveStop();
                        int pos = 0;
                        foreach (GXDLMSObject obj in items)
                        {
                            this.OnProgress(dev, "Reading...", pos++, items.Count);
                            try
                            {
                                dev.Comm.OnBeforeRead += new ReadEventHandler(OnBeforeRead);
                                dev.Comm.OnAfterRead += new ReadEventHandler(OnAfterRead);
                                dev.OnTrace += new MessageTraceEventHandler(OnTrace);
                                dev.Comm.Read(this, obj, 0);
                            }
                            finally
                            {
                                dev.Comm.OnBeforeRead -= new ReadEventHandler(OnBeforeRead);
                                dev.Comm.OnAfterRead -= new ReadEventHandler(OnAfterRead);
                                dev.OnTrace -= new MessageTraceEventHandler(OnTrace);
                            }
                            DLMSItemOnChange(obj, false, 0, null);
                        }
                        dev.KeepAliveStart();
                    }
                }
            }
            catch (ThreadAbortException)
            {
                //User has cancel action. Do nothing.
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.ReadWriteDenied ||
                        ex.ErrorCode == (int)ErrorCode.UndefinedObject ||
                        ex.ErrorCode == (int)ErrorCode.UnavailableObject ||
                        //Actaris returns access violation error.
                        ex.ErrorCode == (int)ErrorCode.AccessViolated)
                {
                    GXDLMS.Common.Error.ShowError(sender, ex);
                }
                else
                {
                    GXDLMS.Common.Error.ShowError(sender, ex);
                    if (dev != null)
                    {
                        dev.Disconnect();
                    }
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(sender, Ex);
                if (dev != null)
                {
                    dev.Disconnect();
                }
            }
            finally
            {
                UpdateTransaction(false);
            }
        }

        /// <summary>
        /// Read selected DLMS object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadMnu_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                //Set focus to the tree or Read count do not updated.
                this.ObjectTree.Focus();
                if (this.ObjectTree.SelectedNode != null)
                {
                    TransactionWork = new GXAsyncWork(this, OnAsyncStateChange, Read, new object[] { this.ObjectTree.SelectedNode.Tag, SelectedView });
                    TransactionWork.Start();
                }
            }
            else
            {
                if (this.ObjectList.SelectedItems.Count != 0)
                {
                    GXDLMSObjectCollection items = new GXDLMSObjectCollection();
                    foreach (ListViewItem it in this.ObjectList.SelectedItems)
                    {
                        items.Add(it.Tag as GXDLMSObject);
                    }
                    TransactionWork = new GXAsyncWork(this, OnAsyncStateChange, Read, new object[] { items, SelectedView });
                    TransactionWork.Start();
                }
            }
        }

        private void WriteMnu_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateTransaction(true);
                if (this.ObjectTree.SelectedNode != null)
                {
                    //Read all objects if device is selected.
                    if (this.ObjectTree.SelectedNode.Tag is GXDLMSDeviceCollection)
                    {
                        throw new Exception("Only objects can be write, not whole devices.");
                    }
                    else if (this.ObjectTree.SelectedNode.Tag is GXDLMSDevice)
                    {
                        throw new Exception("Only objects can be write, not whole devices.");
                    }
                    else if (this.ObjectTree.SelectedNode.Tag is GXDLMSObjectCollection)
                    {
                        GXDLMSObjectCollection objects = (GXDLMSObjectCollection)this.ObjectTree.SelectedNode.Tag;
                        GXDLMSDevice dev = objects.Tag as GXDLMSDevice;
                        dev.KeepAliveStop();
                        OnProgress(dev, "Writing...", 0, 1);
                        foreach (GXDLMSObject obj in objects)
                        {
                            dev.Comm.Write(obj, obj, 0, new List<object>());
                        }
                        dev.KeepAliveStart();
                    }
                    else if (this.ObjectTree.SelectedNode.Tag is GXDLMSObject)
                    {
                        GXDLMSObject obj = (GXDLMSObject)this.ObjectTree.SelectedNode.Tag;
                        GXDLMSDevice dev = obj.Parent.Tag as GXDLMSDevice;
                        dev.KeepAliveStop();
                        OnProgress(dev, "Writing...", 0, 1);
                        dev.Comm.Write(obj, obj, 0, new List<object>());
                        dev.KeepAliveStart();
                    }
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
            finally
            {
                UpdateTransaction(false);
            }
        }

        /// <summary>
        /// Close application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitMnu_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void RemoveDevice(GXDLMSDevice dev)
        {
            dev.Disconnect();
            dev.OnProgress -= new ProgressEventHandler(this.OnProgress);
            dev.OnStatusChanged -= new StatusEventHandler(this.OnStatusChanged);
            Devices.Remove(dev);
            TreeNode node = (TreeNode)ObjectTreeItems[dev];
            node.Remove();
            ObjectTreeItems.Remove(dev);
            ListViewItem item = (ListViewItem)DeviceListViewItems[dev];
            DeviceList.Items.Remove(item);
            DeviceListViewItems.Remove(dev);
            while (dev.Objects.Count != 0)
            {
                RemoveObject(dev.Objects[0]);
            }
        }

        delegate void RemoveObjectEventHandler(object target);

        void RemoveObject(object target)
        {
            if (InvokeRequired)
            {
                Invoke(new RemoveObjectEventHandler(RemoveObject), target);
            }
            else
            {
                OnDirty(true);
                if (target is GXDLMSObject)
                {
                    GXDLMSObject obj = target as GXDLMSObject;
                    int pos = SelectedListItems.IndexOfValue(obj);
                    if (pos != -1)
                    {
                        SelectedListItems.RemoveAt(pos);
                    }
                    ListViewItem lv = ObjectValueItems[obj] as ListViewItem;
                    if (lv != null)
                    {
                        ObjectValueItems.Remove(obj);
                        lv.Remove();
                    }
                    (obj.Parent.Tag as GXDLMSDevice).Objects.Remove(obj);
                    obj.Parent.Remove(obj);
                    TreeNode node = ObjectTreeItems[obj] as TreeNode;
                    if (node != null)
                    {
                        //If this is the last node to remove.
                        if (node.Parent.Nodes.Count == 1 && !(node.Parent.Tag is GXDLMSDevice))
                        {
                            object type = (obj.Parent.Tag.GetHashCode() << 16) + obj.ObjectType;
                            TreeNode node2 = ObjectTreeItems[type] as TreeNode;
                            if (node2 != null)
                            {
                                node2.Remove();
                            }
                            ObjectTreeItems.Remove(type);
                        }
                        else
                        {
                            node.Remove();
                        }
                        ObjectTreeItems.Remove(obj);
                    }
                    ListViewItem item = ObjectListItems[obj] as ListViewItem;
                    if (item != null)
                    {
                        if (SelectedListItems.Values.Contains(obj))
                        {
                            SelectedListItems.RemoveAt(SelectedListItems.IndexOfValue(obj));
                        }
                        item.Remove();
                        ObjectListItems.Remove(obj);
                    }
                }
                else if (target is GXDLMSObjectCollection)
                {
                    GXDLMSObjectCollection items = target as GXDLMSObjectCollection;
                    if (items.Count != 0)
                    {
                        foreach (GXDLMSObject it in items)
                        {
                            RemoveObject(it);
                        }
                    }
                }
                else if (target is GXDLMSDevice)
                {
                    GXDLMSDevice dev = target as GXDLMSDevice;
                    while (dev.Objects.Count != 0)
                    {
                        RemoveObject(dev.Objects[0]);
                    }
                }
            }
        }

        /// <summary>
        /// Delete selected object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteMnu_Click(object sender, EventArgs e)
        {
            try
            {
                if (ObjectTree.Focused)
                {
                    TreeNode node = ObjectTree.SelectedNode;
                    if (node != null)
                    {
                        object obj = node.Tag;
                        if (obj is GXDLMSDevice)
                        {
                            if (MessageBox.Show(this, GXDLMSDirector.Properties.Resources.RemoveDeviceConfirmation, GXDLMSDirector.Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                            {
                                return;
                            }
                            RemoveDevice(obj as GXDLMSDevice);
                        }
                        else if (obj is GXDLMSObject)
                        {
                            if (MessageBox.Show(this, GXDLMSDirector.Properties.Resources.RemoveObjectConfirmation, GXDLMSDirector.Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                            {
                                return;
                            }
                            RemoveObject(obj);
                        }
                        else if (obj is GXDLMSObjectCollection)
                        {
                            if (MessageBox.Show(this, GXDLMSDirector.Properties.Resources.RemoveObjectConfirmation, GXDLMSDirector.Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                            {
                                return;
                            }
                            RemoveObject(obj);
                        }
                        //Find selected device from the device list.
                        else if (obj is GXDLMSDeviceCollection)
                        {
                            if (MessageBox.Show(this, GXDLMSDirector.Properties.Resources.RemoveDeviceConfirmation, GXDLMSDirector.Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                            {
                                return;
                            }
                            foreach (ListViewItem it in DeviceList.SelectedItems)
                            {
                                RemoveDevice(it.Tag as GXDLMSDevice);
                            }
                        }
                    }
                }
                else if (ObjectList.Focused && ObjectList.SelectedItems.Count != 0)
                {
                    if (MessageBox.Show(this, GXDLMSDirector.Properties.Resources.RemoveObjectConfirmation, GXDLMSDirector.Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                    {
                        return;
                    }

                    GXDLMSObjectCollection items = new GXDLMSObjectCollection();
                    foreach (ListViewItem it in ObjectList.SelectedItems)
                    {
                        items.Add(it.Tag as GXDLMSObject);
                    }
                    foreach (GXDLMSObject it in items)
                    {
                        RemoveObject(it);
                    }
                }
                SetDirty(true);
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ConnectBtn.Checked)
                {
                    ConnectMnu_Click(null, null);
                }
                else
                {
                    DisconnectMnu_Click(null, null);
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        TreeNode AddDevice(GXDLMSDevice dev, bool refresh)
        {
            if (!refresh)
            {
                dev.OnProgress += new ProgressEventHandler(this.OnProgress);
                dev.OnStatusChanged += new StatusEventHandler(this.OnStatusChanged);
                GXManufacturer m = Manufacturers.FindByIdentification(dev.Manufacturer);
                if (m.Extension != null)
                {
                    Type t = Type.GetType(m.Extension);
                    dev.Extension = Activator.CreateInstance(t) as IGXManufacturerExtension;
                }
            }
            //Add device to device tree.
            TreeNode node = this.ObjectTree.Nodes[0].Nodes.Add(dev.Name);
            node.SelectedImageIndex = node.ImageIndex = 2;
            node.Tag = dev;
            ObjectTreeItems[dev] = node;
            //Add device to device list.
            if (!refresh)
            {
                ListViewItem item = DeviceList.Items.Add(dev.Name);
                item.Tag = dev;
                DeviceListViewItems[dev] = item;
            }
            return node;
        }

        /// <summary>
        /// Load settings from the file.
        /// </summary>
        /// <param name="path"></param>
        void LoadFile(string path)
        {
            //Save changes?
            if (this.Dirty)
            {
                DialogResult ret = MessageBox.Show(this, Properties.Resources.SaveChangesTxt, Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (ret == DialogResult.Cancel)
                {
                    return;
                }
                if (ret == DialogResult.Yes)
                {
                    if (!Save())
                    {
                        return;
                    }
                }
            }
            //Clear log every time when new device list is loaded.
            GXLogWriter.ClearLog();
            Initialize();
            int version = 1;
            using (XmlReader reader = XmlReader.Create(path))
            {
                List<Type> types = new List<Type>(Gurux.DLMS.GXDLMSClient.GetObjectTypes());
                types.Add(typeof(GXDLMSAttributeSettings));
                types.Add(typeof(GXDLMSAttribute));
                //Version is added to namespace.
                XmlSerializer x = new XmlSerializer(typeof(GXDLMSDeviceCollection), null, types.ToArray(), null, "Gurux1");
                if (!x.CanDeserialize(reader))
                {
                    version = 0;
                    x = new XmlSerializer(typeof(GXDLMSDeviceCollection), types.ToArray());
                }
                Devices = (GXDLMSDeviceCollection)x.Deserialize(reader);
                reader.Close();
                TreeNode node = ObjectTree.Nodes[0];
                node.Tag = Devices;
            }
            //Add devices to the device tree and update parser.
            foreach (GXDLMSDevice dev in Devices)
            {
                dev.Comm.parentForm = this;
                dev.Manufacturers = this.Manufacturers;
                GXManufacturer m = Manufacturers.FindByIdentification(dev.Manufacturer);
                if (m == null)
                {
                    throw new Exception("Load failed. Invalid manufacturer: " + dev.Manufacturer);
                }
                if (version == 0)
                {
                    dev.UseLogicalNameReferencing = m.UseLogicalNameReferencing;
                }
                dev.ObisCodes = m.ObisCodes;
                this.AddDevice(dev, false);
                RefreshDevice(dev, false);
            }
            GroupItems(GroupsMnu.Checked);
            m_Path = path;
            SetDirty(false);
            m_MruManager.Insert(0, path);
        }

        /// <summary>
        /// Open media settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenMnu_Click(object sender, EventArgs e)
        {
            string file = null;
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Multiselect = false;
                if (string.IsNullOrEmpty(m_Path))
                {
                    dlg.InitialDirectory = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
                }
                else
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(m_Path);
                    dlg.InitialDirectory = fi.DirectoryName;
                    dlg.FileName = fi.Name;
                }
                dlg.Filter = Properties.Resources.FilterTxt;
                dlg.DefaultExt = ".gdr";
                dlg.ValidateNames = true;
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    file = dlg.FileName;
                    if (File.Exists(file))
                    {
                        LoadFile(file);
                    }
                }
            }
            catch (Exception Ex)
            {
                if (file != null)
                {
                    m_MruManager.Remove(file);
                }
                GXLogWriter.WriteLog(Ex.ToString());
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        void UpdateFromObisCode(GXDLMSObject obj, GXObisCode item)
        {
            if (!String.IsNullOrEmpty(item.Description))
            {
                obj.Description = item.Description;
            }
            foreach (GXDLMSAttributeSettings it in item.Attributes)
            {
                if (obj.Attributes.Find(it.Index) == null)
                {
                    obj.Attributes.Add(it);
                }
            }
        }

        delegate void ClearTraceEventHandler();

        void ClearTrace()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ClearTraceEventHandler(this.ClearTrace));
            }
            else
            {
                TraceView.Text = "";
            }
        }

        void RefreshDevice(GXDLMSDevice dev, bool bRefresh)
        {
            try
            {
                ClearTrace();
                if (bRefresh)
                {
                    dev.KeepAliveStop();
                }
                TreeNode deviceNode = (TreeNode)ObjectTreeItems[dev];
                if (bRefresh)
                {
                    try
                    {
                        dev.OnTrace += new MessageTraceEventHandler(OnTrace);
                        OnProgress(dev, "Refresh device", 0, 1);
                        UpdateTransaction(true);
                        RemoveObject(dev);
                        while (dev.Objects.Count != 0)
                        {
                            RemoveObject(dev.Objects[0]);
                        }
                        GXManufacturer m = Manufacturers.FindByIdentification(dev.Manufacturer);
                        dev.ObisCodes = m.ObisCodes;
                        //Walk through object tree.
                        dev.UpdateObjects();
                        GroupItems(GroupsMnu.Checked);
                        //Read registers units and scalers.
                        int cnt = dev.Objects.Count;
                        GXLogWriter.WriteLog("--- Reading scalers and units. ---");
                        for (int pos = 0; pos != cnt; ++pos)
                        {
                            GXDLMSObject it = dev.Objects[pos];
                            GXObisCode obj = m.ObisCodes.FindByLN(it.ObjectType, it.LogicalName, null);
                            if (obj != null)
                            {
                                UpdateFromObisCode(it, obj);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        GXLogWriter.WriteLog("--- Reading scalers and units end. ---");
                        /* TODO:
                        if (!m.UseLogicalNameReferencing)
                        {
                            GXLogWriter.WriteLog("--- Reading Access rights. ---");
                            try
                            {
                                foreach (GXDLMSAssociationShortName sn in dev.Objects.GetObjects(ObjectType.AssociationShortName))
                                {
                                    dev.Comm.Read(sn, 3);
                                }
                            }
                            catch (Exception ex)
                            {
                                GXLogWriter.WriteLog(ex.Message);
                            }
                            GXLogWriter.WriteLog("--- Reading Access rights end. ---");
                        }
                         * */
                        this.OnProgress(dev, "Reading scalers and units.", cnt, cnt);
                        GroupItems(GroupsMnu.Checked);
                    }
                    finally
                    {
                        dev.OnTrace -= new MessageTraceEventHandler(OnTrace);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                //User has cancel action. Do nothing.
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (bRefresh)
                {
                    dev.KeepAliveStart();
                    UpdateTransaction(false);
                }
            }
        }

        delegate ListViewItem AddNodeEventHandler(GXDLMSObject it, TreeNode deviceNode, ListViewGroup group);

        ListViewItem AddNode(GXDLMSObject it, TreeNode deviceNode, ListViewGroup group)
        {
            if (this.InvokeRequired)
            {
                return this.Invoke(new AddNodeEventHandler(AddNode), new object[] { it, deviceNode, group }) as ListViewItem;
            }
            TreeNode node;
            GXDLMSObjectCollection objects = null;
            if (group != null)
            {
                node = ObjectTreeItems[(it.Parent.Tag.GetHashCode() << 16) + it.ObjectType] as TreeNode;
                if (node == null)
                {
                    node = deviceNode.Nodes.Add(it.ObjectType.ToString());
                    node.SelectedImageIndex = node.ImageIndex = 11;
                    ObjectTreeItems[(it.Parent.Tag.GetHashCode() << 16) + it.ObjectType] = node;
                    objects = new GXDLMSObjectCollection();
                    node.Tag = objects;
                }
                else
                {
                    objects = node.Tag as GXDLMSObjectCollection;
                }
                objects.Add(it);
                node = node.Nodes.Add(it.LogicalName + " " + it.Description);
            }
            else
            {
                node = deviceNode.Nodes.Add(it.LogicalName + " " + it.Description);
            }
            node.Tag = it;
            ObjectTreeItems[it] = node;
            ListViewItem item = new ListViewItem(it.LogicalName + " " + it.Description);
            item.Tag = it;
            item.Group = group;
            ObjectListItems.Add(it, item);
            if (it is GXDLMSProfileGeneric)
            {
                node.SelectedImageIndex = node.ImageIndex = 8;
            }
            else
            {
                node.SelectedImageIndex = node.ImageIndex = 7;
            }
            return item;
        }

        delegate void RefreshEventHandler(System.Windows.Forms.Control sender, object[] parameters);

        void Refresh(System.Windows.Forms.Control sender, object[] parameters)
        {
            try
            {
                if (parameters[0] is GXDLMSDeviceCollection)
                {
                    foreach (GXDLMSDevice dev in (GXDLMSDeviceCollection)parameters[0])
                    {
                        RefreshDevice(dev, true);
                    }
                }
                else if (parameters[0] is GXDLMSDevice)
                {
                    GXDLMSDevice dev = (GXDLMSDevice)parameters[0];
                    RefreshDevice(dev, true);
                }
                else if (parameters[0] is GXDLMSObjectCollection)
                {
                    bool oneProfileGeneric = false;
                    bool allProfileGeneric = true;
                    //If only profile generics are selected update only them not whole device.
                    GXDLMSObjectCollection items = parameters[0] as GXDLMSObjectCollection;
                    foreach (GXDLMSObject it in items)
                    {
                        if (!(it is GXDLMSProfileGeneric))
                        {
                            allProfileGeneric = false;
                            if (oneProfileGeneric)
                            {
                                break;
                            }
                        }
                        else
                        {
                            oneProfileGeneric = true;
                        }
                    }
                    if (oneProfileGeneric && allProfileGeneric)
                    {
                        foreach (GXDLMSProfileGeneric pg in items)
                        {
                            GXDLMSDevice dev = pg.Parent.Tag as GXDLMSDevice;
                            dev.UpdateColumns(pg, dev.Manufacturers.FindByIdentification(dev.Manufacturer));
                            if (items.Count != 1)
                            {
                                ListViewItem it = ObjectListItems[pg] as ListViewItem;
                                it.Selected = false;
                            }
                        }
                    }
                    else if (items.Count != 0)//If other than profile generics are selected read meter.
                    {
                        GXDLMSDevice dev = items[0].Parent.Tag as GXDLMSDevice;
                        Refresh(sender, new object[] { dev });
                    }
                }
                else if (parameters[0] is GXDLMSProfileGeneric)
                {
                    ClearTrace();
                    GXDLMSProfileGeneric pg = (GXDLMSProfileGeneric)parameters[0];
                    GXDLMSDevice dev = pg.Parent.Tag as GXDLMSDevice;
                    dev.OnTrace += new MessageTraceEventHandler(OnTrace);
                    try
                    {
                        dev.UpdateColumns(pg, dev.Manufacturers.FindByIdentification(dev.Manufacturer));
                        ((GXDLMSProfileGenericView)SelectedView).Target = parameters[0] as GXDLMSProfileGeneric;
                    }
                    finally
                    {
                        dev.OnTrace -= new MessageTraceEventHandler(OnTrace);
                    }
                }
                else
                {
                    GXDLMSObject obj = parameters[0] as GXDLMSObject;
                    GXDLMSDevice dev = obj.Parent.Tag as GXDLMSDevice;
                    if (!this.InvokeRequired)
                    {
                        ObjectTree.SelectedNode = ObjectTreeItems[dev] as TreeNode;
                    }
                    RefreshDevice(dev, true);
                }
                SetDirty(true);
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(sender, Ex);
            }
        }

        /// <summary>
        /// Update object tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                TransactionWork = new GXAsyncWork(this, OnAsyncStateChange, Refresh, new object[] { ObjectTree.SelectedNode.Tag });
            }
            else
            {
                GXDLMSObjectCollection items = new GXDLMSObjectCollection();
                foreach (ListViewItem it in ObjectList.SelectedItems)
                {
                    items.Add(it.Tag as GXDLMSObject);
                }
                TransactionWork = new GXAsyncWork(this, OnAsyncStateChange, Refresh, new object[] { items });
            }

            TransactionWork.Start();
        }

        private void AddDeviceMnu_Click(object sender, EventArgs e)
        {
            try
            {
                DevicePropertiesForm dlg = new DevicePropertiesForm(this.Manufacturers, null);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    dlg.Device.Manufacturers = this.Manufacturers;
                    dlg.Device.Comm.parentForm = this;
                    AddDevice(dlg.Device, false);
                    Devices.Add(dlg.Device);
                    SetDirty(true);
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void ContentsMnu_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.gurux.fi/index.php?q=GXDLMSDirectorHelp");
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Edit manufacturers settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManufacturersMnu_Click(object sender, EventArgs e)
        {
            string selectedManufacturer = null;
            GXDLMSDevice dev = GetSelectedDevice();
            if (dev != null)
            {
                selectedManufacturer = dev.Manufacturer;
            }
            ManufacturersForm dlg = new ManufacturersForm(this.Manufacturers, selectedManufacturer);
            dlg.ShowDialog(this);
        }

        private void OBISCodesMnu_Click(object sender, EventArgs e)
        {
            string ln = null;
            string selectedManufacturer = null;
            TreeNode node = this.ObjectTree.SelectedNode;
            ObjectType Interface = ObjectType.None;
            GXDLMSDevice dev = null;
            if (node != null)
            {
                if (node.Tag is GXDLMSDevice)
                {
                    dev = (GXDLMSDevice)node.Tag;
                    selectedManufacturer = dev.Manufacturer;
                }
                else if (this.ObjectTree.SelectedNode.Tag is GXDLMSObject)
                {
                    GXDLMSObject obj = this.ObjectTree.SelectedNode.Tag as GXDLMSObject;
                    dev = obj.Parent.Tag as GXDLMSDevice;
                    selectedManufacturer = dev.Manufacturer;
                    ln = obj.LogicalName;
                    Interface = obj.ObjectType;
                }
            }
            OBISCodesForm dlg = new OBISCodesForm(this.Manufacturers, selectedManufacturer, Interface, ln);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                if (this.ObjectTree.SelectedNode.Tag is GXDLMSObject)
                {
                    GXDLMSObject obj = this.ObjectTree.SelectedNode.Tag as GXDLMSObject;
                    GXObisCode code = this.Manufacturers.FindByIdentification(selectedManufacturer).ObisCodes.FindByLN(obj.ObjectType, obj.LogicalName, null);
                    obj.Description = code.Description;
                    foreach (GXDLMSAttributeSettings it in code.Attributes)
                    {
                        GXDLMSAttributeSettings att = obj.Attributes.Find(it.Index);
                        if (att == null)
                        {
                            obj.Attributes.Add(it);
                        }
                        else if (att.GetType() == typeof(GXDLMSAttribute))
                        {
                            obj.Attributes.Remove(it);
                            GXDLMSAttributeSettings tmp = new GXDLMSAttributeSettings();
                            it.CopyTo(tmp);
                            obj.Attributes.Add(tmp);
                        }
                        else
                        {
                            it.CopyTo(att);
                        }
                    }
                }
            }
        }

        private void LogMnu_Click(object sender, EventArgs e)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = "notepad";
                p.StartInfo.FileName = GXLogWriter.LogPath;
                p.Start();
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void ClearLogMnu_Click(object sender, EventArgs e)
        {
            try
            {
                GXLogWriter.ClearLog();
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void ObjectPanelFrame_Resize(object sender, EventArgs e)
        {
        }

        private void ObjectTree_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = new Point(e.X, e.Y);
                ObjectTree.PointToClient(pt);

                TreeNode Node = ObjectTree.GetNodeAt(pt);
                if (Node != null)
                {
                    if (Node.Bounds.Contains(pt))
                    {
                        ObjectTree.SelectedNode = Node;
                        ConfigureContextMenu();
                        contextMenuStrip1.Show(ObjectTree, pt);
                    }
                }
            }

        }

        private void ConfigureContextMenu()
        {
            ConnectCMnu.Visible = false;
            DisconnectCMnu.Visible = false;
            ReadCMnu.Visible = false;
            AddDeviceCMnu.Visible = false;
            DeleteCMnu.Visible = false;
            Line1CMnu.Visible = false;

            if (this.ObjectTree.SelectedNode != null)
            {
                if (this.ObjectTree.SelectedNode.Tag is GXDLMSDeviceCollection)
                {
                    AddDeviceCMnu.Visible = true;
                    return;
                }
                ConnectCMnu.Visible = true;
                DisconnectCMnu.Visible = true;
                ReadCMnu.Visible = true;
                DeleteCMnu.Visible = true;
                Line1CMnu.Visible = true;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                Application.EnableVisualStyles();
                ObjectValueView.Columns.Clear();
                ObjectValueView.Columns.Add("Name");
                ObjectValueView.Columns.Add("Object Type");
                ObjectValueView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                LoadXmlPositioning();
                foreach (Type type in typeof(IGXDLMSView).Assembly.GetTypes())
                {
                    GXDLMSViewAttribute[] att = (GXDLMSViewAttribute[])type.GetCustomAttributes(typeof(GXDLMSViewAttribute), true);
                    if (!type.IsInterface && typeof(IGXDLMSView).IsAssignableFrom(type))
                    {
                        IGXDLMSView view = Activator.CreateInstance(type) as IGXDLMSView;
                        Form f = view as Form;
                        f.TopLevel = false;
                        f.TopMost = false;
                        f.FormBorderStyle = FormBorderStyle.None;
                        f.Dock = DockStyle.Fill;
                        f.Width = ObjectPanelFrame.Width;
                        f.Height = ObjectPanelFrame.Height;
                        Views.Add(att[0].DLMSType, view);
                    }
                }

                if (GXManufacturerCollection.IsFirstRun())
                {
                    if (MessageBox.Show(this, GXDLMSDirector.Properties.Resources.InstallManufacturersOnlineTxt, GXDLMSDirector.Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                    {
                        GXManufacturerCollection.UpdateManufactureSettings();
                    }
                }
                Manufacturers = new GXManufacturerCollection();
                GXManufacturerCollection.ReadManufacturerSettings(Manufacturers);
                ThreadPool.QueueUserWorkItem(new WaitCallback(CheckUpdates), this);
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        static void CheckUpdates(object data)
        {
            try
            {
                DateTime LastUpdateCheck = DateTime.MinValue;
                MainForm main = (MainForm)data;
                //Wait for a while before check updates.
                //Check new updates once a day.
                while (true)
                {
                    if (LastUpdateCheck.AddDays(1) < DateTime.Now)
                    {
                        LastUpdateCheck = DateTime.Now;
                        bool isConnected = true;
                        if (Environment.OSVersion.Platform != PlatformID.Unix)
                        {
                            Gurux.Win32.InternetConnectionState flags = Gurux.Win32.InternetConnectionState.INTERNET_CONNECTION_LAN | Gurux.Win32.InternetConnectionState.INTERNET_CONNECTION_CONFIGURED;
                            isConnected = Gurux.Win32.InternetGetConnectedState(ref flags, 0);
                        }
                        //If there are updates available.
                        if (isConnected && GXManufacturerCollection.IsUpdatesAvailable())
                        {
                            main.BeginInvoke(new CheckUpdatesEventHandler(OnNewObisCodes), new object[] { main });
                            break;
                        }
                    }
                    //Wait for a day before next check.
                    System.Threading.Thread.Sleep(DateTime.Now.AddDays(1) - DateTime.Now);
                }
            }
            catch
            {
                //It's OK if this fails.
                //Wait for a day before next check.
                System.Threading.Thread.Sleep(DateTime.Now.AddDays(1) - DateTime.Now);
            }
        }

        static void OnNewObisCodes(MainForm form)
        {
            form.updateManufactureSettingsToolStripMenuItem.Visible = true;
        }


        private void updateManufactureSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(this, GXDLMSDirector.Properties.Resources.UpdateManufacturersOnlineTxt, GXDLMSDirector.Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {
                    GXManufacturerCollection.UpdateManufactureSettings();
                    Manufacturers = new GXManufacturerCollection();
                    GXManufacturerCollection.ReadManufacturerSettings(Manufacturers);
                }
                updateManufactureSettingsToolStripMenuItem.Visible = false;
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        void OnAsyncStateChange(System.Windows.Forms.Control sender, AsyncState state)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AsyncStateChangeEventHandler(this.OnAsyncStateChange), sender, state);
            }
            else
            {
                CancelBtn.Enabled = state == AsyncState.Start;
                if (state == AsyncState.Cancel)
                {
                    DisconnectMnu_Click(this, null);
                }
                ProgressBar.Visible = state == AsyncState.Start;
                if (state == AsyncState.Finish ||
                        state == AsyncState.Cancel)
                {
                    StatusLbl.Text = "Ready";
                }
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            try
            {
                TransactionWork.Cancel();
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        void OnOpenMRUFile(string fileName)
        {
            try
            {
                LoadFile(fileName);
            }
            catch (Exception Ex)
            {
                m_MruManager.Remove(fileName);
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        delegate void GroupItemsEventHandler(bool bGroup);

        void GroupItems(bool bGroup)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new GroupItemsEventHandler(GroupItems), bGroup);
            }
            else
            {
                ObjectList.Groups.Clear();
                ObjectList.Items.Clear();
                SelectedListItems.Clear();
                ObjectListItems.Clear();
                ObjectTreeItems.Clear();
                ObjectTreeItems[Devices] = this.ObjectTree.Nodes[0];
                this.ObjectTree.Nodes[0].Nodes.Clear();
                Dictionary<ObjectType, ListViewGroup> groups = new Dictionary<ObjectType, ListViewGroup>();
                List<ListViewItem> items = new List<ListViewItem>();
                foreach (GXDLMSDevice dev in Devices)
                {
                    TreeNode deviceNode = AddDevice(dev, true);
                    ListViewGroup group = null;
                    foreach (GXDLMSObject it in dev.Objects)
                    {
                        if (bGroup)
                        {
                            if (!groups.ContainsKey(it.ObjectType))
                            {
                                group = new ListViewGroup(it.ObjectType.ToString(), it.ObjectType.ToString());
                                groups.Add(it.ObjectType, group);
                            }
                            else
                            {
                                group = groups[it.ObjectType];
                            }
                        }
                        items.Add(AddNode(it, deviceNode, group));
                    }
                }
                ObjectList.Groups.AddRange(groups.Values.ToArray());
                ObjectList.Items.AddRange(items.ToArray());
            }
        }

        private void GroupsMnu_Click(object sender, EventArgs e)
        {
            GroupsMnu.Checked = !GroupsMnu.Checked;
            GroupItems(GroupsMnu.Checked);
        }

        private void ObjectList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                foreach (ListViewItem it in ObjectList.SelectedItems)
                {
                    RemoveObject(it.Tag);
                }
            }
        }

        private void ObjectList_Resize(object sender, EventArgs e)
        {
            try
            {
                DescriptionColumnHeader.Width = ObjectList.ClientSize.Width;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Show or hide object tree.
        /// </summary>
        private void ObjectTreeMnu_Click(object sender, EventArgs e)
        {
            ObjectTreeMnu.Checked = !ObjectTreeMnu.Checked;
            if (ObjectTreeMnu.Checked)
            {
                if (!this.tabControl1.TabPages.Contains(this.TreeView))
                {
                    this.tabControl1.TabPages.Add(this.TreeView);
                }
            }
            else if (this.tabControl1.TabPages.Contains(this.TreeView))
            {
                this.tabControl1.TabPages.Remove(this.TreeView);
            }

        }
        /// <summary>
        /// Show or hide object list.
        /// </summary>
        private void ObjectListMnu_Click(object sender, EventArgs e)
        {
            ObjectListMnu.Checked = !ObjectListMnu.Checked;
            if (ObjectListMnu.Checked)
            {
                if (!this.tabControl1.TabPages.Contains(this.ListView))
                {
                    this.tabControl1.TabPages.Add(this.ListView);
                }
            }
            else if (this.tabControl1.TabPages.Contains(this.ListView))
            {
                this.tabControl1.TabPages.Remove(this.ListView);
            }
        }

        private void ObjectList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            try
            {
                this.ObjectTree.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.ObjectTree_AfterSelect);
                if (e.IsSelected)
                {
                    SelectedListItems.Add(e.ItemIndex, e.Item.Tag as GXDLMSObject);
                    if (SelectedListItems.Count == 1)
                    {
                        SelectItem(e.Item.Tag);
                    }
                    else
                    {
                        GXDLMSObjectCollection items = new GXDLMSObjectCollection();
                        items.AddRange(SelectedListItems.Values.ToArray());
                        SelectItem(items);
                    }
                    TreeNode node = ObjectTreeItems[e.Item.Tag] as TreeNode;
                    if (node != null)
                    {
                        ObjectTree.SelectedNode = node;
                    }
                }
                else
                {
                    int pos = SelectedListItems.IndexOfValue(e.Item.Tag as GXDLMSObject);
                    if (pos != -1)
                    {
                        SelectedListItems.RemoveAt(pos);
                    }
                    else
                    {
                        return;
                    }
                    if (SelectedListItems.Count == 0)
                    {
                        SelectItem(((GXDLMSObject)e.Item.Tag).Parent.Tag);
                    }
                    else if (SelectedListItems.Count == 1)
                    {
                        SelectItem(SelectedListItems.Values[0]);
                    }
                    else
                    {
                        GXDLMSObjectCollection items = new GXDLMSObjectCollection();
                        items.AddRange(SelectedListItems.Values.ToArray());
                        SelectItem(items);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                this.ObjectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ObjectTree_AfterSelect);
            }
        }

        private void ObjectValueView_DoubleClick(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                object target = ObjectValueView.SelectedItems[0].Tag;
                TreeNode node = ObjectTreeItems[target] as TreeNode;
                ObjectTree.SelectedNode = node;
            }
            else
            {
                if (ObjectValueView.SelectedItems.Count == 1)
                {
                    object target = ObjectValueView.SelectedItems[0].Tag;
                    ObjectList.SelectedItems.Clear();
                    ListViewItem item = ObjectListItems[target] as ListViewItem;
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Show library versions.
        /// </summary>
        private void LibraryVersionsMenu_Click(object sender, EventArgs e)
        {
            try
            {
                LibraryVersionsDlg dlg = new LibraryVersionsDlg();
                dlg.ShowDialog(this);
            }
            catch (Exception Ex)
            {
                GXCommon.ShowError(Ex);
            }
        }

        /// <summary>
        /// Show or hide trace.
        /// </summary>
        private void TraceMenu_Click(object sender, EventArgs e)
        {
            TraceMnu.Checked = !TraceMnu.Checked;
            TraceView.Visible = TraceMnu.Checked;
        }
    }

}
