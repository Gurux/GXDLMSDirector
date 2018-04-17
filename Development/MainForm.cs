//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
//
// Version:         $Revision: 10042 $,
//                  $Date: 2018-04-17 09:57:56 +0300 (ti, 17 huhti 2018) $
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
using Gurux.Net;
using System.Text;

namespace GXDLMSDirector
{
    public partial class MainForm : Form
    {
        GXNet events;

        /// <summary>
        /// Events translator.
        /// </summary>
        GXDLMSTranslator eventsTranslator = new GXDLMSTranslator(TranslatorOutputType.SimpleXml);
        GXByteBuffer eventsData = new GXByteBuffer();

        /// <summary>
        /// Log translator.
        /// </summary>
        GXDLMSTranslator traceTranslator;

        delegate void CheckUpdatesEventHandler(MainForm form);
        GXAsyncWork TransactionWork;
        Dictionary<Type, List<IGXDLMSView>> Views;
        String path;
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
        private MRUManager mruManager = null;
        /// <summary>
        /// Used trace level.
        /// </summary>
        private byte traceLevel = 0;
        /// <summary>
        /// Used notification level.
        /// </summary>
        private byte notificationLevel = 0;

        /// <summary>
        /// Received trace data.
        /// </summary>
        GXByteBuffer receivedTraceData = new GXByteBuffer();


        private static GXDLMSDevice GetDevice(GXDLMSObject item)
        {
            return item.Parent.Tag as GXDLMSDevice;
        }

        void OnDirty(bool dirty)
        {
            Dirty = dirty;
            this.Text = Properties.Resources.GXDLMSDirectorTxt + " " + path;
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
            if (!Debugger.IsAttached)
            {
                Trace.Listeners.Clear();
            }
            DirectoryInfo di = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName((GXLogWriter.LogPath)));
            if (!di.Exists)
            {
                di.Create();
            }
            GXLogWriter.ClearLog();
            Trace.Listeners.Add(new TextWriterTraceListener(GXLogWriter.LogPath));
            Trace.AutoFlush = true;
            Trace.IndentSize = 4;
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
            reRunToolStripMenuItem.Visible = false;
            CancelBtn.Enabled = false;
            traceTranslator = new GXDLMSTranslator(TranslatorOutputType.SimpleXml);

            Devices = new GXDLMSDeviceCollection();
            ObjectValueView.Visible = DeviceInfoView.Visible = DeviceList.Visible = false;
            ObjectValueView.Dock = ObjectPanelFrame.Dock = DeviceList.Dock = DeviceInfoView.Dock = DockStyle.Fill;
            ProgressBar.Visible = false;
            UpdateDeviceUI(null, DeviceState.None);
            NewMnu_Click(null, null);
            mruManager = new MRUManager(RecentFilesMnu);
            mruManager.OnOpenMRUFile += new OpenMRUFileEventHandler(this.OnOpenMRUFile);
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

        GXDLMSDevice selectedDevice;

        void UpdateDeviceUI(GXDLMSDevice device, DeviceState state)
        {
            if (selectedDevice != null && selectedDevice != device)
            {
                receivedTraceData.Clear();
                TraceView.ResetText();
                selectedDevice.OnTrace = null;
                selectedDevice.OnEvent = null;
                selectedDevice = null;
            }
            if (device != null)
            {
                selectedDevice = device;
                device.OnTrace = OnTrace;
                device.OnEvent = Events_OnReceived;
            }
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
                    SaveAsTemplateBtn.Enabled = false;
                    DeviceState state = Devices.Count == 0 ? DeviceState.None : DeviceState.Initialized;
                    UpdateDeviceUI(null, state);
                    DeleteMnu.Enabled = DeleteBtn.Enabled = OptionsBtn.Enabled = false;
                    DeviceList.BringToFront();
                }
                else if (DeviceInfoView.Visible)
                {
                    SaveAsTemplateBtn.Enabled = true;
                    GXDLMSDevice dev = (GXDLMSDevice)obj;
                    DeviceGb.Text = dev.Name;
                    StatusValueLbl.Text = dev.Status.ToString();
                    ClientAddressValueLbl.Text = dev.ClientAddress.ToString();
                    LogicalAddressValueLbl.Text = dev.LogicalAddress.ToString();
                    PhysicalAddressValueLbl.Text = dev.PhysicalAddress.ToString();
                    ManufacturerValueLbl.Text = dev.Manufacturers.FindByIdentification(dev.Manufacturer).Name;
                    ConformanceTB.Text = dev.Comm.client.NegotiatedConformance.ToString();
                    UpdateDeviceUI(dev, dev.Status);
                    DeviceInfoView.BringToFront();
                }
                else if (ObjectValueView.Visible)
                {
                    ObjectValueView.BringToFront();
                    SaveAsTemplateBtn.Enabled = true;
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
                    ObjectPanelFrame.BringToFront();
                    SaveAsTemplateBtn.Enabled = true;
                    SelectedView = GXDlmsUi.GetView(Views, (GXDLMSObject)obj);
                    foreach (Control it in ObjectPanelFrame.Controls)
                    {
                        it.Hide();
                    }
                    ObjectPanelFrame.Controls.Clear();
                    SelectedView.Target = (GXDLMSObject)obj;
                    SelectedView.Target.OnChange += new ObjectChangeEventHandler(DLMSItemOnChange);
                    GXDlmsUi.ObjectChanged(SelectedView, obj as GXDLMSObject, (GetDevice((GXDLMSObject)obj).Status & DeviceState.Connected) != 0);
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
                        if (view.ErrorProvider != null)
                        {
                            if (dirty && index != 0)
                            {
                                view.ErrorProvider.SetError(it, GXDLMSDirector.Properties.Resources.ValueChangedTxt);
                            }
                            else
                            {
                                view.ErrorProvider.Clear();
                            }
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

        /// <summary>
        /// User is executing action.
        /// </summary>
        private void OnAction(object sender, GXAsyncWork work, object[] parameters)
        {
            ClearTrace();
            UpdateTransaction(true);
            GXButton btn = parameters[0] as GXButton;
            GXDLMSDevice dev = btn.Target.Parent.Tag as GXDLMSDevice;
            GXActionArgs ve = new GXActionArgs(btn.Target, btn.Index);
            ve.Client = dev.Comm.client;
            ve.Action = btn.Action;

            GXDlmsUi.UpdateAccessRights(btn.View, btn.Target, false);
            dev.KeepAliveStop();
            try
            {
                do
                {
                    btn.View.PreAction(ve);
                    if (ve.Exception != null)
                    {
                        throw ve.Exception;
                    }
                    if (ve.Handled)
                    {
                        break;
                    }
                    else
                    {
                        try
                        {
                            GXReplyData reply = new GXReplyData();
                            if (ve.Value is byte[][])
                            {
                                int pos = 0, cnt = ((byte[][])ve.Value).Length;
                                foreach (byte[] it in (byte[][])ve.Value)
                                {
                                    if (cnt != 1)
                                    {
                                        OnProgress(null, ve.Text, ++pos, cnt);
                                    }
                                    reply.Clear();
                                    dev.Comm.ReadDataBlock(it, ve.Text, 1, reply);
                                }
                            }
                            else if (ve.Action == ActionType.Read)
                            {
                                dev.Comm.ReadValue(ve.Target, ve.Index);
                            }
                            else if (ve.Action == ActionType.Write)
                            {
                                dev.Comm.Write(ve.Target, ve.Index);
                            }
                            else if (ve.Action == ActionType.Action)
                            {
                                dev.Comm.MethodRequest(ve.Target, ve.Index, ve.Value, ve.Text, reply);
                            }
                        }
                        catch (Exception ex)
                        {
                            ve.Exception = ex;
                        }
                        btn.View.PostAction(ve);
                        if (ve.Exception != null)
                        {
                            throw ve.Exception;
                        }
                        ve.Value = null;
                    }
                } while (ve.Action != ActionType.None);
            }
            finally
            {
                dev.KeepAliveStart();
                GXDlmsUi.UpdateAccessRights(btn.View, btn.Target, (dev.Status & DeviceState.Connected) != 0);
                UpdateTransaction(false);
            }
        }

        void OnHandleAction(object sender, EventArgs e)
        {
            try
            {
                if (TransactionWork != null && TransactionWork.IsRunning)
                {
                    throw new Exception("Transaction in progress.");
                }
                TransactionWork = new GXAsyncWork(this, OnAsyncStateChange, OnAction, OnError, null, new object[] { sender });
                TransactionWork.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        delegate void ValueChangedEventHandler(IGXDLMSView view, int index, object value, bool changeByUser, bool connected);

        void OnValueChanged(IGXDLMSView view, int index, object value, bool changeByUser, bool connected)
        {
            try
            {
                view.OnValueChanged(index, value, changeByUser, connected);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        /// <summary>
        /// Create new device list.
        /// </summary>
        private void NewMnu_Click(object sender, EventArgs e)
        {
            try
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
                if (TransactionWork != null)
                {
                    TransactionWork.Cancel();
                    TransactionWork = null;
                }
                path = "";
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
                ConformanceTests.Items.Clear();
                SetDirty(false);
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

        private DialogResult ShowProperties(GXDLMSDevice dev, bool newDevice)
        {
            string man = dev.Manufacturer;
            DevicePropertiesForm dlg = new DevicePropertiesForm(this.Manufacturers, dev);
            DialogResult res = dlg.ShowDialog(this);
            if (res == DialogResult.OK)
            {
                if (!newDevice)
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
            return res;
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
                    ShowProperties(dev, false);
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
                FileVersionInfo info = FileVersionInfo.GetVersionInfo(asm.Location);
                lic.ShowAbout(this, info.FileVersion);
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private static void Save(string path, GXDLMSDeviceCollection devices)
        {
            using (Stream stream = File.Open(path, FileMode.Create))
            {
                GXFileSystemSecurity.UpdateFileSecurity(path);
                List<Type> types = new List<Type>(GXDLMSClient.GetObjectTypes());
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
                    x.Serialize(writer, devices);
                    writer.Close();
                }
                stream.Close();
            }
        }

        private void SaveFile(string path)
        {
            Save(path, Devices);
            SetDirty(false);
            mruManager.Insert(0, path);
        }

        private bool Save()
        {
            if (string.IsNullOrEmpty(path))
            {
                return SaveAs();
            }
            SaveFile(path);
            return true;
        }

        private bool SaveAs()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = Properties.Resources.FilterTxt;
            dlg.DefaultExt = ".gxc";
            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                this.path = dlg.FileName;
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

        delegate void UpdateConformance(GXDLMSDevice device);

        void OnUpdateConformance(GXDLMSDevice device)
        {
            ConformanceTB.Text = device.Comm.client.NegotiatedConformance.ToString();

        }

        void OnError(object sender, Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        /// <summary>
        /// Ask is association view read when connecting first time.
        /// </summary>
        /// <param name="device"></param>
        delegate void FirstConnectionEventHandler(GXDLMSDevice device);

        /// <summary>
        /// Ask is association view read when connecting first time.
        /// </summary>
        /// <param name="device">Connected meter.</param>
        private void OnFirstConnection(GXDLMSDevice device)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new FirstConnectionEventHandler(OnFirstConnection), device);
            }
            else
            {
                if (MessageBox.Show(this, Properties.Resources.ReadAssociationView, Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    TransactionWork = new GXAsyncWork(this, OnAsyncStateChange, Refresh, OnError, null, new object[] { device });
                }
                TransactionWork.Start();
            }
        }

        void Connect(object sender, GXAsyncWork work, object[] parameters)
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
                            this.OnProgress(null, "Connecting", ++pos, cnt);
                            it.InitializeConnection();
                        }
                    }
                }
                else if (obj is GXDLMSDevice)
                {
                    if (!((GXDLMSDevice)obj).Media.IsOpen)
                    {
                        GXDLMSDevice dev = (GXDLMSDevice)obj;
                        this.OnProgress(null, "Connecting", 0, 1);
                        dev.InitializeConnection();
                        if (InvokeRequired)
                        {
                            BeginInvoke(new UpdateConformance(this.OnUpdateConformance), (GXDLMSDevice)obj);
                        }
                        if (dev.Objects.Count == 0)
                        {
                            OnFirstConnection(dev);
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
                    Connect(sender, work, new object[] { devices });
                    if (tmp.Count == 1)
                    {
                        GXDlmsUi.ObjectChanged(SelectedView, tmp[0] as GXDLMSObject, true);
                    }
                }
                else
                {
                    this.OnProgress(null, "Connecting", 0, 1);
                    GXDLMSObject tmp = obj as GXDLMSObject;
                    GXDLMSDevice dev = tmp.Parent.Tag as GXDLMSDevice;
                    if (!dev.Media.IsOpen)
                    {
                        dev.InitializeConnection();
                    }
                    GXDlmsUi.ObjectChanged(SelectedView, tmp as GXDLMSObject, true);
                }
            }
            catch (ThreadAbortException)
            {
                //User has cancel action. Do nothing.
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(sender as Form, Ex);
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
                TransactionWork = new GXAsyncWork(this, OnAsyncStateChange, Connect, OnError, null, new object[] { ObjectTree.SelectedNode.Tag });
            }
            else
            {
                TransactionWork = new GXAsyncWork(this, OnAsyncStateChange, Connect, OnError, null, new object[] { GetDevices() });
            }
            TransactionWork.Start();
        }

        void Disconnect(object sender, GXAsyncWork work, object[] parameters)
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
                        (sender as Form).BeginInvoke(new ProgressEventHandler(this.OnProgress), null, "Disconnecting", ++pos, cnt);
                        it.Disconnect();
                    }
                }
                else if (obj is GXDLMSDevice)
                {
                    (sender as Form).BeginInvoke(new ProgressEventHandler(this.OnProgress), null, "Disconnecting", 0, 1);
                    ((GXDLMSDevice)obj).Disconnect();
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
                    Disconnect(sender, work, new object[] { devices });
                    if (tmp.Count == 1)
                    {
                        GXDlmsUi.ObjectChanged(SelectedView, tmp[0] as GXDLMSObject, false);
                    }
                }
                else
                {
                    (sender as Form).BeginInvoke(new ProgressEventHandler(OnProgress), sender, "Disconnecting", 0, 1);
                    GXDLMSObject tmp = obj as GXDLMSObject;
                    GXDLMSDevice dev = tmp.Parent.Tag as GXDLMSDevice;
                    dev.Disconnect();
                    GXDlmsUi.ObjectChanged(SelectedView, tmp, false);
                }
            }
            catch (ThreadAbortException)
            {
                //User has cancel action. Do nothing.
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError((sender as Form), Ex);
            }
            finally
            {
                (sender as Form).BeginInvoke(new ProgressEventHandler(this.OnProgress), sender, "", 1, 1);
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
                TransactionWork = new GXAsyncWork(this, OnAsyncStateChange, Disconnect, OnError, null, new object[] { ObjectTree.SelectedNode.Tag });
            }
            else
            {
                TransactionWork = new GXAsyncWork(this, OnAsyncStateChange, Disconnect, OnError, null, new object[] { GetDevices() });
            }
            TransactionWork.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                events.Close();
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
        /// Save window position to a xml-file
        /// </summary>
        private void SaveXmlPositioning()
        {
            try
            {
                Properties.Settings.Default.ViewToolbar = ViewToolbarMnu.Checked;
                Properties.Settings.Default.ViewStatusbar = ViewStatusbarMnu.Checked;
                Properties.Settings.Default.ViewTree = ObjectTreeMnu.Checked;
                Properties.Settings.Default.ViewList = ObjectListMnu.Checked;
                Properties.Settings.Default.ViewGroups = GroupsMnu.Checked;
                Properties.Settings.Default.TraceType = traceLevel;
                Properties.Settings.Default.NotificationType = notificationLevel;
                Properties.Settings.Default.ForceRead = ForceReadMnu.Checked;
                Properties.Settings.Default.NotificationAutoReset = AutoReset.Checked;

                Properties.Settings.Default.LogComments = LogCommentsMenu.Checked;
                Properties.Settings.Default.TraceComments = TraceCommentsMenu.Checked;
                Properties.Settings.Default.NotificationsComments = NotificationsCommentsMenu.Checked;

                if (EventsView.Height > 50)
                {
                    Properties.Settings.Default.EventsViewHeight = EventsView.Height;
                }
                if (TraceView.Height > 50)
                {
                    Properties.Settings.Default.TraceViewHeight = TraceView.Height;
                }
                Properties.Settings.Default.Save();
                GXDlmsUi.Save();

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
                    foreach (string it in mruManager.GetNames())
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
                ViewToolbarMnu.Checked = !Properties.Settings.Default.ViewToolbar;
                ViewToolbarMnu_Click(null, null);
                ViewStatusbarMnu.Checked = !Properties.Settings.Default.ViewStatusbar;
                ViewStatusbarMnu_Click(null, null);
                ObjectTreeMnu.Checked = !Properties.Settings.Default.ViewTree;
                ObjectTreeMnu_Click(null, null);
                ObjectListMnu.Checked = !Properties.Settings.Default.ViewList;
                ObjectListMnu_Click(null, null);
                GroupsMnu.Checked = !Properties.Settings.Default.ViewGroups;
                GroupsMnu_Click(null, null);
                if (Properties.Settings.Default.TraceType == 0)
                {
                    noneToolStripMenuItem_Click(null, null);
                }
                else if (Properties.Settings.Default.TraceType == 1)
                {
                    hexToolStripMenuItem_Click(null, null);
                }
                else if (Properties.Settings.Default.TraceType == 2)
                {
                    xmlToolStripMenuItem_Click(null, null);
                }
                else if (Properties.Settings.Default.TraceType == 3)
                {
                    pDUToolStripMenuItem_Click(null, null);
                }

                if (Properties.Settings.Default.NotificationType == 0)
                {
                    NotificationNone_Click(null, null);
                }
                else if (Properties.Settings.Default.NotificationType == 1)
                {
                    NotificationHex_Click(null, null);
                }
                else if (Properties.Settings.Default.NotificationType == 2)
                {
                    NotificationXml_Click(null, null);
                }
                else if (Properties.Settings.Default.NotificationType == 3)
                {
                    NotificationPdu_Click(null, null);
                }

                conformanceTestsToolStripMenuItem1.Checked = !Properties.Settings.Default.CTT;
                conformanceTestsToolStripMenuItem1_Click(null, null);


                TraceView.Height = Properties.Settings.Default.TraceViewHeight;
                EventsView.Height = Properties.Settings.Default.EventsViewHeight;

                ForceReadMnu.Checked = !Properties.Settings.Default.ForceRead;
                ForceReadMnu_Click(null, null);
                AutoReset.Checked = !Properties.Settings.Default.NotificationAutoReset;
                AutoReset_Click(null, null);

                LogCommentsMenu.Checked = !Properties.Settings.Default.LogComments;
                LogCommentsMenu_Click(null, null);
                TraceCommentsMenu.Checked = !Properties.Settings.Default.TraceComments;
                TraceCommentsMenu_Click(null, null);
                NotificationsCommentsMenu.Checked = !Properties.Settings.Default.NotificationsComments;
                NotificationsCommentsMenu_Click(null, null);

                LogHexBtn.Checked = (Properties.Settings.Default.Log & 1) != 0;
                LogXmlBtn.Checked = (Properties.Settings.Default.Log & 2) != 0;
                GXLogWriter.LogLevel = Properties.Settings.Default.Log;

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
                                mruManager.Insert(-1, xtr.ReadElementString());
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
                    dev.Comm.Read(this, it, 0, ForceRefreshBtn.Checked);
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
                dev.KeepAliveStart();
            }
        }

        delegate void UpdateTransactionEventHandler(bool start);

        void OnTrace(GXDLMSDevice sender, string trace, byte[] data, int length, string path)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MessageTraceEventHandler(this.OnTrace), sender, trace, data, length, path);
            }
            else
            {
                //Show data as hex.
                if (traceLevel == 1)
                {
                    TraceView.AppendText(DateTime.Now.ToString() + trace + " " + GXCommon.ToHex(data, true) + Environment.NewLine);
                }
                else if (data != null && (traceLevel == 2 || traceLevel == 3))
                {
                    //Show data as xml or pdu.
                    receivedTraceData.Set(data);
                    try
                    {
                        GXByteBuffer pdu = new GXByteBuffer();
                        InterfaceType type = GXDLMSTranslator.GetDlmsFraming(receivedTraceData);
                        if (traceTranslator.FindNextFrame(receivedTraceData, pdu, type))
                        {
                            TraceView.AppendText(Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine + traceTranslator.MessageToXml(receivedTraceData));
                            receivedTraceData.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        receivedTraceData.Clear();
                        TraceView.ResetText();
                        TraceView.AppendText(Environment.NewLine + DateTime.Now.ToString() + ex.Message + Environment.NewLine);
                        TraceView.AppendText(trace + Environment.NewLine + GXCommon.ToHex(data, true) + Environment.NewLine);
                    }
                }
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

        public void OnBeforeRead(GXDLMSObject sender, int index, Exception ex)
        {
            try
            {
                if ((index == 2 || index == 3) && !this.IsDisposed && sender is GXDLMSProfileGeneric)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new ReadEventHandler(OnBeforeRead), new object[] { sender, index, ex });
                    }
                    else if (SelectedView != null && SelectedView.Target == sender)
                    {
                        GXDLMSProfileGeneric pg = sender as GXDLMSProfileGeneric;
                        pg.Buffer.Clear();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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

        public void OnAfterRead(GXDLMSObject sender, int index, Exception ex)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new ReadEventHandler(OnAfterRead), new object[] { sender, index, ex });
                    return;
                }
                if (SelectedView != null && SelectedView.Target == sender)
                {
                    GXDlmsUi.UpdateProperty(sender as GXDLMSObject, index, SelectedView, true, false);
                }
                ListViewItem lv = ObjectValueItems[sender] as ListViewItem;
                if (lv != null)
                {
                    UpdateValue(sender, index, lv);
                }
                if (ex != null)
                {
                    GXDlmsUi.UpdateError(SelectedView, sender as GXDLMSObject, index, ex);

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        void Read(object sender, GXAsyncWork work, object[] parameters)
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
                        dev.KeepAliveStop();
                        this.OnProgress(dev, "Reading...", 0, 1);
                        try
                        {
                            dev.Comm.OnBeforeRead += new ReadEventHandler(OnBeforeRead);
                            dev.Comm.OnAfterRead += new ReadEventHandler(OnAfterRead);
                            dev.Comm.Read(this, obj, 0, ForceRefreshBtn.Checked);
                        }
                        finally
                        {
                            dev.Comm.OnBeforeRead -= new ReadEventHandler(OnBeforeRead);
                            dev.Comm.OnAfterRead -= new ReadEventHandler(OnAfterRead);
                        }
                        DLMSItemOnChange(obj, false, 0, null);
                        dev.KeepAliveStart();
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
                                dev.Comm.Read(this, obj, 0, ForceRefreshBtn.Checked);
                            }
                            finally
                            {
                                dev.Comm.OnBeforeRead -= new ReadEventHandler(OnBeforeRead);
                                dev.Comm.OnAfterRead -= new ReadEventHandler(OnAfterRead);
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
                    GXDLMS.Common.Error.ShowError(sender as Form, ex);
                }
                else
                {
                    GXDLMS.Common.Error.ShowError(sender as Form, ex);
                    if (dev != null)
                    {
                        dev.Disconnect();
                    }
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(sender as Form, Ex);
                if (dev != null)
                {
                    dev.Disconnect();
                }
            }
            finally
            {
                if ((dev.Status & DeviceState.Connected) != 0)
                {
                    UpdateTransaction(false);
                }
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
                    TransactionWork = new GXAsyncWork(this, OnAsyncStateChange, Read, OnError, null, new object[] { this.ObjectTree.SelectedNode.Tag, SelectedView });
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
                    TransactionWork = new GXAsyncWork(this, OnAsyncStateChange, Read, OnError, null, new object[] { items, SelectedView });
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
                        dev.Comm.OnError += new ReadEventHandler(OnError);
                        try
                        {
                            dev.KeepAliveStop();
                            OnProgress(dev, "Writing...", 0, 1);
                            foreach (GXDLMSObject obj in objects)
                            {
                                dev.Comm.Write(obj, 0);
                                GXDlmsUi.UpdateProperty(obj as GXDLMSObject, 0, SelectedView, true, false);
                            }
                        }
                        finally
                        {
                            dev.Comm.OnError -= new ReadEventHandler(OnError);
                            dev.KeepAliveStart();
                        }
                    }
                    else if (this.ObjectTree.SelectedNode.Tag is GXDLMSObject)
                    {
                        GXDLMSObject obj = (GXDLMSObject)this.ObjectTree.SelectedNode.Tag;
                        GXDLMSDevice dev = obj.Parent.Tag as GXDLMSDevice;
                        try
                        {
                            dev.KeepAliveStop();
                            dev.Comm.OnError += new ReadEventHandler(OnError);
                            OnProgress(dev, "Writing...", 0, 1);
                            dev.Comm.Write(obj, 0);
                            GXDlmsUi.UpdateProperty(obj as GXDLMSObject, 0, SelectedView, true, false);
                        }
                        finally
                        {
                            dev.Comm.OnError -= new ReadEventHandler(OnError);
                            dev.KeepAliveStart();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GXDLMS.Common.Error.ShowError(this, ex);
            }
            finally
            {
                StatusLbl.Text = Properties.Resources.ReadyTxt;
                UpdateTransaction(false);
            }
        }

        public void OnError(GXDLMSObject sender, int index, Exception ex)
        {
            GXDlmsUi.UpdateError(SelectedView, sender as GXDLMSObject, index, ex);
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

        private static void UpdateAttributes(GXDLMSObject obj, GXObisCode it)
        {
            foreach (GXDLMSAttributeSettings a in it.Attributes)
            {
                if (a.UIType != DataType.None)
                {
                    obj.SetUIDataType(a.Index, a.UIType);
                }
                if (a.Type != DataType.None)
                {
                    obj.SetDataType(a.Index, a.UIType);
                }
            }

        }

        TreeNode AddDevice(GXDLMSDevice dev, bool refresh, bool first)
        {
            if (!refresh)
            {
                dev.Media.OnReceived += new ReceivedEventHandler(Events_OnReceived);
                dev.OnProgress += new ProgressEventHandler(this.OnProgress);
                dev.OnStatusChanged += new StatusEventHandler(this.OnStatusChanged);
                GXManufacturer m = Manufacturers.FindByIdentification(dev.Manufacturer);
                if (m.Extension != null)
                {
                    Type t = Type.GetType(m.Extension);
                    dev.Extension = Activator.CreateInstance(t) as IGXManufacturerExtension;
                }
                if (first && m.ObisCodes != null)
                {
                    GXDLMSConverter c = new GXDLMSConverter();
                    foreach (GXObisCode it in m.ObisCodes)
                    {
                        if (it.Append)
                        {
                            GXDLMSObject obj = GXDLMSClient.CreateObject(it.ObjectType);
                            obj.Version = it.Version;
                            obj.LogicalName = it.LogicalName;
                            if (string.IsNullOrEmpty(it.Description))
                            {
                                obj.Description = c.GetDescription(it.LogicalName, it.ObjectType)[0];
                            }
                            else
                            {
                                obj.Description = it.Description;
                            }
                            UpdateAttributes(obj, it);
                            dev.Objects.Add(obj);
                        }
                    }
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
                //Don't show save changes again.
                this.Dirty = false;
            }
            //Clear log every time when new device list is loaded.
            GXLogWriter.ClearLog();
            NewMnu_Click(null, null);
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
                //Conformance is new funtionality. Set default value if not set.
                if (dev.UseLogicalNameReferencing && dev.Conformance == (int)GXDLMSClient.GetInitialConformance(false))
                {
                    dev.Conformance = (int)GXDLMSClient.GetInitialConformance(dev.UseLogicalNameReferencing);
                }
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
                dev.Objects.Tag = dev;
                dev.ObisCodes = m.ObisCodes;
                AddDevice(dev, false, false);
                RefreshDevice(dev, false);

                //Update association views.
                GXDLMSObject it = dev.Objects.FindByLN(ObjectType.AssociationLogicalName, "0.0.40.0.0.255");
                if (it is GXDLMSAssociationLogicalName)
                {
                    (it as GXDLMSAssociationLogicalName).ObjectList.AddRange(dev.Objects);
                }
            }
            GroupItems(GroupsMnu.Checked);
            this.path = path;
            SetDirty(false);
            mruManager.Insert(0, path);
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
                if (string.IsNullOrEmpty(path))
                {
                    dlg.InitialDirectory = Directory.GetCurrentDirectory();
                }
                else
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(path);
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
                    mruManager.Remove(file);
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
            if (item.Attributes.Count != 0)
            {
                foreach (var it in item.Attributes)
                {
                    GXDLMSAttributeSettings a = obj.Attributes.Find(it.Index);
                    if (a != null)
                    {
                        obj.Attributes.Remove(a);
                    }
                    obj.Attributes.Add(it);
                }
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
                TraceView.ResetText();
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
                    OnProgress(dev, "Refresh device", 0, 1);
                    UpdateTransaction(true);
                    RemoveObject(dev);
                    while (dev.Objects.Count != 0)
                    {
                        RemoveObject(dev.Objects[0]);
                    }
                    GXManufacturer m = Manufacturers.FindByIdentification(dev.Manufacturer);
                    dev.ObisCodes = null;
                    //Walk through object tree.
                    dev.UpdateObjects();
                    GroupItems(GroupsMnu.Checked);
                    //Read registers units and scalers.
                    int cnt = dev.Objects.Count;
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

                        //Update association views.
                        if (it is GXDLMSAssociationLogicalName && it.LogicalName == "0.0.40.0.4.255")
                        {
                            (it as GXDLMSAssociationLogicalName).ObjectList.AddRange(dev.Objects);
                        }
                    }
                    //Read inactivity timeout.
                    dev.InactivityTimeout = 120 - 10;
                    foreach (GXDLMSHdlcSetup it in dev.Objects.GetObjects(ObjectType.IecHdlcSetup))
                    {
                        dev.Comm.ReadValue(it, 8);
                        if (dev.InactivityTimeout > it.InactivityTimeout && it.InactivityTimeout > 10)
                        {
                            dev.InactivityTimeout = it.InactivityTimeout - 10;
                        }
                    }
                    this.OnProgress(dev, "Reading scalers and units.", cnt, cnt);
                    GroupItems(GroupsMnu.Checked);
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
                    objects.Tag = deviceNode.Tag;
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

        void Refresh(object sender, GXAsyncWork work, object[] parameters)
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
                        Refresh(sender, work, new object[] { dev });
                    }
                }
                else if (parameters[0] is GXDLMSProfileGeneric)
                {
                    ClearTrace();
                    GXDLMSProfileGeneric pg = (GXDLMSProfileGeneric)parameters[0];
                    GXDLMSDevice dev = pg.Parent.Tag as GXDLMSDevice;
                    dev.UpdateColumns(pg, dev.Manufacturers.FindByIdentification(dev.Manufacturer));
                    ((GXDLMSProfileGenericView)SelectedView).Target = parameters[0] as GXDLMSProfileGeneric;
                    if (InvokeRequired)
                    {
                        this.Invoke(new ValueChangedEventHandler(OnValueChanged), new object[] { SelectedView, 3, null, false, (dev.Status & DeviceState.Connected) != 0 });
                    }
                    else
                    {
                        SelectedView.OnValueChanged(3, null, false, (dev.Status & DeviceState.Connected) != 0);
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
                GXDLMS.Common.Error.ShowError(sender as Form, Ex);
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
                TransactionWork = new GXAsyncWork(this, OnAsyncStateChange, Refresh, OnError, null, new object[] { ObjectTree.SelectedNode.Tag });
            }
            else
            {
                GXDLMSObjectCollection items = new GXDLMSObjectCollection();
                foreach (ListViewItem it in ObjectList.SelectedItems)
                {
                    items.Add(it.Tag as GXDLMSObject);
                }
                TransactionWork = new GXAsyncWork(this, OnAsyncStateChange, Refresh, OnError, null, new object[] { items });
            }

            TransactionWork.Start();
        }

        private void AddDevice(GXDLMSDevice dev)
        {
            DevicePropertiesForm dlg = new DevicePropertiesForm(this.Manufacturers, dev);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                dlg.Device.Manufacturers = this.Manufacturers;
                dlg.Device.Comm.parentForm = this;
                AddDevice(dlg.Device, false, dev == null);
                Devices.Add(dlg.Device);
                GroupItems(GroupsMnu.Checked);
                SetDirty(true);
            }
        }

        private void AddDeviceMnu_Click(object sender, EventArgs e)
        {
            try
            {
                AddDevice(null);
                //Add OBIS codes.

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
                Process.Start("http://www.gurux.fi/index.php?q=GXDLMSDirectorHelp");
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
                    if (code != null)
                    {
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
                events = new GXNet(NetworkType.Tcp, 4059);
                events.ConfigurableSettings = (AvailableMediaSettings.Port | AvailableMediaSettings.Protocol);
                events.Settings = Properties.Settings.Default.EventsSettings;
                events.OnMediaStateChange += Events_OnMediaStateChange;
                events.OnReceived += Events_OnReceived;
                events.OnClientConnected += Events_OnClientConnected;
                events.OnClientDisconnected += Events_OnClientDisconnected;
                Application.EnableVisualStyles();
                ObjectValueView.Columns.Clear();
                ObjectValueView.Columns.Add("Name");
                ObjectValueView.Columns.Add("Object Type");
                ObjectValueView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                LoadXmlPositioning();

                Views = GXDlmsUi.GetViews(ObjectPanelFrame, OnHandleAction);

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

        delegate void AddNotificationEventHanded(string data);

        /// <summary>
        /// Add new notification string.
        /// </summary>
        /// <param name="data">Data to add.</param>
        void OnAddNotification(string data)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new AddNotificationEventHanded(OnAddNotification), data);
            }
            else
            {
                try
                {
                    EventsView.AppendText(data);
                    EventsView.AppendText(Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


        /// <summary>
        /// Client disconnects from sending events.
        /// </summary>
        private void Events_OnClientDisconnected(object sender, ConnectionEventArgs e)
        {
            OnAddNotification("Client disconnected: " + e.Info);
        }

        /// <summary>
        /// New client connects to send events.
        /// </summary>
        private void Events_OnClientConnected(object sender, ConnectionEventArgs e)
        {
            OnAddNotification("Client connected: " + e.Info);
        }

        private void ConverToString(StringBuilder sb, object value)
        {
            if (value is byte[])
            {
                sb.AppendLine(GXCommon.ToHex((byte[])value, true));
            }
            else if (value is object[])
            {
                sb.AppendLine("{");
                foreach (object it in (object[])value)
                {
                    ConverToString(sb, it);
                    if (sb.Length != 0)
                    {
                        sb.Length -= 2;
                    }
                    sb.Append(", ");
                }
                if (sb.Length != 0)
                {
                    sb.Length -= 2;
                }
                sb.AppendLine("}");
            }
            else
            {
                sb.AppendLine(Convert.ToString(value));
            }
        }

        /// <summary>
        /// Meter sends event notification.
        /// </summary>
        private void Events_OnReceived(object sender, ReceiveEventArgs e)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new ReceivedEventHandler(Events_OnReceived), sender, e);
            }
            else
            {
                if (e.Data is string)
                {
                    OnAddNotification(DateTime.Now.ToString() + Environment.NewLine + e.Data);
                }
                else if (NotificationHex.Checked)
                {
                    //Show as hex.
                    OnAddNotification(DateTime.Now.ToString() + Environment.NewLine + GXCommon.ToHex((byte[])e.Data, true));
                }
                else
                {
                    try
                    {
                        //Show as PDU.
                        eventsTranslator.PduOnly = NotificationPdu.Checked;
                        eventsData.Set((byte[])e.Data);
                        GXByteBuffer pdu = new GXByteBuffer();
                        InterfaceType type = GXDLMSTranslator.GetDlmsFraming(eventsData);
                        StringBuilder sb = new StringBuilder();
                        while (eventsTranslator.FindNextFrame(eventsData, pdu, type))
                        {
                            sb.Append(eventsTranslator.MessageToXml(eventsData));
                            pdu.Clear();
                        }
                        if (AutoReset.Checked)
                        {
                            EventsView.ResetText();
                        }
                        OnAddNotification(DateTime.Now.ToString() + Environment.NewLine + sb.ToString());
                    }
                    catch (Exception ex)
                    {
                        eventsData.Clear();
                        OnAddNotification(ex.Message);
                    }
                }
            }
        }

        private void Events_OnMediaStateChange(object sender, MediaStateEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MediaStateChangeEventHandler(Events_OnMediaStateChange), e);
            }
            else
            {
                if (e.State == MediaState.Open)
                {
                    OnAddNotification("Notifications listen stated.");
                    StartNotifications.Visible = false;
                    StopNotifications.Visible = true;
                    NotificationsBtn.Checked = true;
                }
                else if (e.State == MediaState.Closed)
                {
                    OnAddNotification("Notifications listen closed.");
                    StartNotifications.Visible = true;
                    StopNotifications.Visible = false;
                    NotificationsBtn.Checked = false;
                }
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

        void OnAsyncStateChange(object sender, GXAsyncWork work, object[] parameters, AsyncState state, string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AsyncStateChangeEventHandler(OnAsyncStateChange), sender, work, parameters, state, text);
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
                    StatusLbl.Text = Properties.Resources.ReadyTxt;
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
                mruManager.Remove(fileName);
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
                    TreeNode deviceNode = AddDevice(dev, true, false);
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
                Debug.WriteLine(ex.Message);
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
                Debug.WriteLine(ex.Message);
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
        /// Force read.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ForceReadMnu_Click(object sender, EventArgs e)
        {
            ForceRefreshBtn.Checked = ForceReadMnu.Checked = !ForceReadMnu.Checked;
        }

        /// <summary>
        /// Start listen notifications.
        /// </summary>
        private void StartNotifications_Click(object sender, EventArgs e)
        {
            try
            {
                events.Open();
            }
            catch (Exception Ex)
            {
                GXCommon.ShowError(Ex);
            }
        }

        /// <summary>
        /// Stop listen notifications.
        /// </summary>
        private void StopNotifications_Click(object sender, EventArgs e)
        {
            try
            {
                events.Close();
            }
            catch (Exception Ex)
            {
                GXCommon.ShowError(Ex);
            }
        }

        private void NotificationsBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (events.IsOpen)
                {
                    events.Close();
                }
                else
                {
                    events.Open();
                }
            }
            catch (Exception Ex)
            {
                GXCommon.ShowError(Ex);
            }
        }

        private void ClearNotifications_Click(object sender, EventArgs e)
        {
            EventsView.ResetText();
        }

        private void AutoReset_Click(object sender, EventArgs e)
        {
            AutoReset.Checked = !AutoReset.Checked;
        }

        private void UpdateTrace(byte level)
        {
            traceLevel = level;
            panel1.Visible = level != 0 || EventsView.Visible || ConformanceTests.Visible;
            TraceView.Visible = level != 0;
            noneToolStripMenuItem.Checked = level == 0;
            hexToolStripMenuItem.Checked = level == 1;
            xmlToolStripMenuItem.Checked = level == 2;
            pDUToolStripMenuItem.Checked = level == 3;
            //Show data as pdu.
            traceTranslator.PduOnly = level == 3;

            TraceCommentsMenu.Enabled = level > 1;
        }

        /// <summary>
        /// Hide trace.
        /// </summary>
        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateTrace(0);
        }

        /// <summary>
        /// Show trace in Hex Mode.
        /// </summary>
        private void hexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateTrace(1);
        }

        /// <summary>
        /// Show trace in PDU Mode.
        /// </summary>
        private void pDUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateTrace(3);
        }

        /// <summary>
        /// Show trace in Xml Mode.
        /// </summary>
        private void xmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateTrace(2);
        }

        /// <summary>
        /// Show settings.
        /// </summary>
        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                GXSettingsDlg dlg = new GXSettingsDlg(events);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    Properties.Settings.Default.EventsSettings = events.Settings;
                }
            }
            catch (Exception Ex)
            {
                GXCommon.ShowError(Ex);
            }

        }

        private static GXDLMSDevice GetDevice(TreeNode node)
        {
            GXDLMSDevice dev = null;
            if (node != null)
            {
                if (node.Tag is GXDLMSDeviceCollection)
                {
                    dev = null;
                }
                if (node.Tag is GXDLMSDevice)
                {
                    dev = node.Tag as GXDLMSDevice;
                }
                else if (node.Tag is GXDLMSObject)
                {
                    dev = GetDevice(node.Tag as GXDLMSObject);
                }
                else if (node.Parent != null)
                {
                    dev = (GXDLMSDevice)node.Parent.Tag;
                }
            }
            return dev;
        }

        private void ObjectTree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            GXDLMSDevice oldDev = GetDevice(ObjectTree.SelectedNode);
            GXDLMSDevice newDev = GetDevice(e.Node);
            if (oldDev != newDev)
            {
                eventsTranslator.Clear();
                if (newDev != null)
                {
                    traceTranslator.Security = newDev.Security;
                    traceTranslator.SystemTitle = GXCommon.HexToBytes(newDev.SystemTitle);
                    traceTranslator.BlockCipherKey = GXCommon.HexToBytes(newDev.BlockCipherKey);
                    traceTranslator.AuthenticationKey = GXCommon.HexToBytes(newDev.AuthenticationKey);
                    traceTranslator.InvocationCounter = newDev.InvocationCounter;
                    traceTranslator.ServerSystemTitle = GXCommon.HexToBytes(newDev.ServerSystemTitle);
                }
            }
        }

        private void UpdateNotification(byte level)
        {
            notificationLevel = level;
            panel1.Visible = level != 0 || TraceView.Visible || ConformanceTests.Visible;
            splitter3.Visible = EventsView.Visible = level != 0;
            NotificationNone.Checked = level == 0;
            NotificationHex.Checked = level == 1;
            NotificationXml.Checked = level == 2;
            NotificationPdu.Checked = level == 3;
            //Show data as pdu.
            eventsTranslator.PduOnly = level == 3;
            if (level == 0 && !ConformanceTests.Visible)
            {
                TraceView.Dock = DockStyle.Fill;
            }
            else
            {
                TraceView.Dock = DockStyle.Left;
            }
            NotificationsCommentsMenu.Enabled = level > 1;
        }


        private void NotificationNone_Click(object sender, EventArgs e)
        {
            UpdateNotification(0);
        }

        private void NotificationHex_Click(object sender, EventArgs e)
        {
            UpdateNotification(1);
        }

        private void NotificationXml_Click(object sender, EventArgs e)
        {
            UpdateNotification(2);

        }

        private void NotificationPdu_Click(object sender, EventArgs e)
        {
            UpdateNotification(3);
        }

        /// <summary>
        /// OBIS code to search.
        /// </summary>
        string search = null;

        private bool FindTreeNode(TreeNodeCollection nodes, string text, ref TreeNode first)
        {
            foreach (TreeNode it in nodes)
            {
                if (first == null)
                {
                    if (it == ObjectTree.SelectedNode)
                    {
                        first = it;
                    }
                }
                else if (it.Tag is GXDLMSObject && (it.Tag as GXDLMSObject).LogicalName == search)
                {
                    ObjectTree.SelectedNode = it;
                    return true;
                }
                if (it.Nodes.Count != 0)
                {
                    if (FindTreeNode(it.Nodes, text, ref first))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Find OBIS code from the tree view.
        /// </summary>
        /// <param name="nodes">List of OBIS codes to search.</param>
        /// <param name="text">OBIS code to search</param>
        /// <param name="first">If selected item found.</param>
        /// <returns>Is OBIS code found.</returns>
        private bool FindTreeNode(TreeNodeCollection nodes, string text, ref bool first)
        {
            TreeNode node = ObjectTree.SelectedNode;
            foreach (TreeNode it in nodes)
            {
                if (!first)
                {
                    first = it == node;
                }
                else if (it.Tag is GXDLMSObject && (it.Tag as GXDLMSObject).LogicalName == search)
                {
                    ObjectTree.SelectedNode = it;
                    return true;
                }
                if (it.Nodes.Count != 0)
                {
                    if (FindTreeNode(it.Nodes, text, ref first))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Find next OBIS code from the tree view.
        /// </summary>
        private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                bool first = false;
                if (!FindTreeNode(ObjectTree.Nodes, search, ref first))
                {
                    first = true;
                    if (!FindTreeNode(ObjectTree.Nodes, search, ref first))
                    {
                        throw new Exception("OBIS code '" + search + "' not found!");
                    }
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Find OBIS code from the tree view.
        /// </summary>
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GXFindParameters p = new GXFindParameters();
                if (GXDlmsUi.Find(this, p))
                {
                    search = p.ObisCode;
                    bool tmp = string.IsNullOrEmpty(search);
                    findNextToolStripMenuItem.Enabled = !tmp;
                    if (!tmp)
                    {
                        findNextToolStripMenuItem_Click(null, null);
                    }
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Clone selected device.
        /// </summary>
        private void CloneBtn_Click(object sender, EventArgs e)
        {
            try
            {
                object tag = ObjectTree.SelectedNode.Tag;
                GXDLMSDevice dev = null;
                if (tag is GXDLMSDevice)
                {
                    dev = tag as GXDLMSDevice;
                }
                else if (tag is GXDLMSObject)
                {
                    dev = GetDevice(tag as GXDLMSObject);
                }
                if (dev != null)
                {
                    //Create clone from original items.
                    using (MemoryStream ms = new MemoryStream())
                    {
                        List<Type> types = new List<Type>(GXDLMSClient.GetObjectTypes());
                        types.Add(typeof(GXDLMSAttributeSettings));
                        types.Add(typeof(GXDLMSAttribute));
                        XmlAttributeOverrides overrides = new XmlAttributeOverrides();
                        XmlAttributes attribs = new XmlAttributes();
                        attribs.XmlIgnore = true;
                        overrides.Add(typeof(GXDLMSDevice), "ObsoleteObjects", attribs);
                        overrides.Add(typeof(GXDLMSAttributeSettings), attribs);
                        XmlSerializer x = new XmlSerializer(typeof(GXDLMSDevice), overrides, types.ToArray(), null, "Gurux1");
                        using (TextWriter writer = new StreamWriter(ms))
                        {
                            x.Serialize(writer, dev);
                            ms.Position = 0;
                            using (XmlReader reader = XmlReader.Create(ms))
                            {
                                dev = (GXDLMSDevice)x.Deserialize(reader);
                            }
                        }
                        ms.Close();
                        dev.Name = "";
                        AddDevice(dev);
                    }
                }
            }
            catch (Exception Ex)
            {
                Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Log as hex.
        /// </summary>
        private void LogHexBtn_Click(object sender, EventArgs e)
        {
            LogHexBtn.Checked = !LogHexBtn.Checked;
            Properties.Settings.Default.Log = LogHexBtn.Checked ? 1 : 0;
            if (LogXmlBtn.Checked)
            {
                Properties.Settings.Default.Log |= 2;
            }
            GXLogWriter.LogLevel = Properties.Settings.Default.Log;
            LogCommentsMenu.Enabled = LogXmlBtn.Checked;
        }

        /// <summary>
        /// Log as XML.
        /// </summary>
        private void LogXmlBtn_Click(object sender, EventArgs e)
        {
            LogXmlBtn.Checked = !LogXmlBtn.Checked;
            Properties.Settings.Default.Log = LogHexBtn.Checked ? 1 : 0;
            if (LogXmlBtn.Checked)
            {
                Properties.Settings.Default.Log |= 2;
            }
            GXLogWriter.LogLevel = Properties.Settings.Default.Log;
            LogCommentsMenu.Enabled = LogXmlBtn.Checked;
        }

        /// <summary>
        /// Save device as template.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsTemplateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                object tag = ObjectTree.SelectedNode.Tag;
                if (tag != null)
                {
                    GXDLMSDevice dev = null;
                    if (tag is GXDLMSDevice)
                    {
                        dev = tag as GXDLMSDevice;
                    }
                    else if (tag is GXDLMSObject)
                    {
                        dev = GetDevice(tag as GXDLMSObject);
                    }
                    else if (tag is GXDLMSObjectCollection)
                    {
                        dev = (tag as GXDLMSObjectCollection).Tag as GXDLMSDevice;
                    }
                    if (dev != null)
                    {
                        GXManufacturer man = Manufacturers.FindByIdentification(dev.Manufacturer);
                        man.ObisCodes.Clear();
                        foreach (GXDLMSObject it in dev.Objects)
                        {
                            GXObisCode c = new GXObisCode(it.LogicalName, it.ObjectType, null);
                            c.Append = true;
                            man.ObisCodes.Add(c);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Show help not available message.
        /// </summary>
        /// <param name="hevent">A HelpEventArgs that contains the event data.</param>
        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            try
            {
                // Get the control where the user clicked
                Point lpe = menuStrip1.PointToClient(hevent.MousePos);
                Control ctl = this.GetChildAtPoint(this.PointToClient(hevent.MousePos));
                string str = "http://www.gurux.fi/index.php?q=GXDLMSDirectorHelp";
                if (ctl == toolStrip1 || ctl == menuStrip1)
                {
                    str = "http://www.gurux.fi/GXDLMSDirector.Menu";
                }
                else if (ctl == ObjectPanelFrame && SelectedView != null)
                {
                    GXDLMSViewAttribute[] att = (GXDLMSViewAttribute[])SelectedView.GetType().GetCustomAttributes(typeof(GXDLMSViewAttribute), true);
                    str = "http://www.gurux.fi/index.php?q=" + att[0].DLMSType.ToString();
                }
                else if (ctl == ObjectValueView && ObjectValueView.Items.Count != 0)
                {
                    str = "http://www.gurux.fi/index.php?q=" + ObjectValueView.Items[0].Tag.GetType();
                }
                else if (ctl == panel1)
                {
                    ctl = panel1.GetChildAtPoint(panel1.PointToClient(hevent.MousePos));
                    if (ctl == ConformanceTests)
                    {
                        str = "http://www.gurux.fi/index.php?q=GXDLMSDirector.ConformanceTest";
                    }
                }
                // Show online help.
                Process.Start(str);
                // Set flag to show that the Help event as been handled
                hevent.Handled = true;
            }
            catch (Exception Ex)
            {
                Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Show result of conformance test.
        /// </summary>
        private void ShowReportBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConformanceTests.SelectedItems.Count == 1)
                {
                    ListViewItem li = ConformanceTests.SelectedItems[0];
                    string path = Path.Combine((li.Tag as GXConformanceTest).Results, "Results.html");
                    Process.Start(path);
                }
            }
            catch (Exception Ex)
            {
                Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Show log of conformance test.
        /// </summary>
        private void ShowLogBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConformanceTests.SelectedItems.Count == 1)
                {
                    ListViewItem li = ConformanceTests.SelectedItems[0];
                    string path = Path.Combine((li.Tag as GXConformanceTest).Results, "Trace.txt");
                    Process.Start(path);
                }
            }
            catch (Exception Ex)
            {
                Error.ShowError(this, Ex);
            }
        }

        void ConformanceExecute(object sender, GXAsyncWork work, object[] parameters)
        {
            GXConformanceSettings p = (GXConformanceSettings)parameters[1];
            if (p.ConcurrentTesting)
            {
                List<GXConformanceTest> tests = (List<GXConformanceTest>)parameters[0];
                ManualResetEvent[] threads = new ManualResetEvent[tests.Count];
                int pos = 0;
                foreach (GXConformanceTest it in tests)
                {
                    Thread t = new Thread(() =>
                    {
                        List<GXConformanceTest> tmp = new List<GXConformanceTest>();
                        tmp.Add(it);
                        GXConformanceTests.ReadXmlMeter(new object[] { tmp, p });
                    }
                    );
                    t.IsBackground = true;
                    it.Done = threads[pos] = new ManualResetEvent(false);
                    ++pos;
                    t.Start();
                }
                WaitHandle.WaitAll(threads);
            }
            else
            {
                GXConformanceTests.ReadXmlMeter(parameters);
            }
        }

        void ConformanceError(object sender, Exception ex)
        {
            MessageBox.Show(this, ex.Message);
        }

        void ConformanceStateChange(object sender, GXAsyncWork work, object[] parameters, AsyncState state, string text)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AsyncStateChangeEventHandler(ConformanceStateChange), sender, work, parameters, state, text);
            }
            else
            {
                CancelBtn.Enabled = state == AsyncState.Start;
                ProgressBar.Visible = state == AsyncState.Start;
                GXConformanceTests.Continue = state != AsyncState.Cancel;
                if (state == AsyncState.Finish ||
                        state == AsyncState.Cancel)
                {
                    StatusLbl.Text = Properties.Resources.ReadyTxt;
                    if (state == AsyncState.Finish)
                    {
                        MessageBox.Show(this, "Gurux Conformance Tests end.");
                    }
                }
                else
                {
                    StatusLbl.Text = "Running Gurux Conformance Tests.";
                }
            }
        }


        /// <summary>
        /// Conformance test is finnished.
        /// </summary>
        /// <param name="sender">Executed conformance test.</param>
        void OnConformanceReady(GXConformanceTest sender)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ConformanceReadyEvent(OnConformanceReady), sender);
            }
            else
            {
                foreach (ListViewItem li in ConformanceTests.Items)
                {
                    if (li.Tag == sender)
                    {
                        string path = Path.Combine((li.Tag as GXConformanceTest).Results, "Results.html");
                        li.SubItems[1].Text = path;
                        li.ImageIndex = sender.ErrorLevel;
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// Show failed conformance test.
        /// </summary>
        /// <param name="sender">Executed conformance test.</param>
        /// <param name="e">Occurred exception.</param>
        public void OnConformanceError(GXConformanceTest sender, Exception e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ConformanceErrorEvent(OnConformanceError), sender, e);
            }
            else
            {
                try
                {
                    if (traceLevel != 0)
                    {
                        TraceView.AppendText(e.ToString());
                    }
                    foreach (ListViewItem li in ConformanceTests.Items)
                    {
                        if (li.Tag == sender)
                        {
                            li.SubItems[1].Text = e.Message;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Show conformance test trace.
        /// </summary>
        /// <param name="sender">Executed conformance test.</param>
        /// <param name="data"></param>
        public void OnConformanceTrace(GXConformanceTest sender, string data)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ConformanceTraceEvent(OnConformanceTrace), sender, data);
            }
            else
            {
                try
                {
                    if (traceLevel != 0)
                    {
                        TraceView.AppendText(data);
                    }
                    using (FileStream fs = File.Open(Path.Combine(sender.Results, "Values.txt"), FileMode.Append))
                    {
                        using (TextWriter writer = new StreamWriter(fs))
                        {
                            writer.Write(data);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Run basic conformance tests.
        /// </summary>
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Devices.Count == 0)
                {
                    throw new Exception("No devices to test.");
                }
                GXDLMSDeviceCollection devices = new GXDLMSDeviceCollection();
                devices.AddRange(Devices);
                GXConformanceSettings settings;
                XmlSerializer x = new XmlSerializer(typeof(GXConformanceSettings));
                if (Properties.Settings.Default.ConformanceSettings == "")
                {
                    settings = new GXConformanceSettings();
                }
                else
                {
                    try
                    {
                        using (StringReader reader = new StringReader(Properties.Settings.Default.ConformanceSettings))
                        {
                            settings = (GXConformanceSettings)x.Deserialize(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        settings = new GXConformanceSettings();
                    }
                }
                if (RunConformanceTest(settings, devices))
                {
                    try
                    {
                        using (StringWriter writer = new StringWriter())
                        {
                            x.Serialize(writer, settings);
                            Properties.Settings.Default.ConformanceSettings = writer.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        Properties.Settings.Default.ConformanceSettings = "";
                    }
                }
            }
            catch (Exception Ex)
            {
                Error.ShowError(this, Ex);
            }
        }

        private bool RunConformanceTest(GXConformanceSettings settings, GXDLMSDeviceCollection devices)
        {
            GXConformanceDlg dlg = new GXConformanceDlg(settings);
            if (dlg.ShowDialog(this) != DialogResult.OK)
            {
                return false;
            }
            TraceView.ResetText();
            ConformanceTests.Items.Clear();
            GXConformanceTests.ValidateTests(settings);

            List<GXConformanceTest> tests = new List<GXConformanceTest>();
            string path2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GXDLMSDirector");
            string testResults = Path.Combine(path2, "TestResults");
            if (!Directory.Exists(path2))
            {
                Directory.CreateDirectory(path2);
            }
            if (!Directory.Exists(testResults))
            {
                Directory.CreateDirectory(testResults);
            }
            string[] list = Directory.GetDirectories(testResults);
            int testcount = 0;
            foreach (GXDLMSDevice it in devices)
            {
                testcount += it.Objects.Count;
                GXConformanceTest t = new GXConformanceTest() { Device = it };
                t.OnReady = OnConformanceReady;
                t.OnError = OnConformanceError;
                t.OnTrace = OnConformanceTrace;
                t.OnProgress = OnProgress;
                t.Results = Path.Combine(testResults, it.Name + "_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss"));
                Directory.CreateDirectory(t.Results);
                using (Stream stream = File.Open(Path.Combine(t.Results, "Results.html"), FileMode.Create))
                {

                }
                using (Stream stream = File.Open(Path.Combine(t.Results, "Trace.txt"), FileMode.Create))
                {

                }
                using (Stream stream = File.Open(Path.Combine(t.Results, "Values.txt"), FileMode.Create))
                {

                }
                using (Stream stream = File.Open(Path.Combine(t.Results, "settings.xml"), FileMode.Create))
                {
                    XmlSerializer x = new XmlSerializer(typeof(GXConformanceSettings));
                    using (TextWriter writer = new StreamWriter(stream))
                    {
                        x.Serialize(writer, settings);
                        writer.Flush();
                        writer.Close();
                    }
                }
                GXDLMSDeviceCollection devs = new GXDLMSDeviceCollection();
                devs.Add(GXConformanceTests.CloneDevice(it));
                Save(Path.Combine(t.Results, "device.gxc"), devs);
                tests.Add(t);
                ListViewItem li = ConformanceTests.Items.Add(it.Name);
                li.SubItems.Add("");
                li.Tag = t;
            }
            TraceView.ResetText();
            ProgressBar.Value = 0;
            ProgressBar.Maximum = testcount;
            GXConformanceTests.Continue = true;
            TransactionWork = new GXAsyncWork(this, ConformanceStateChange, ConformanceExecute, ConformanceError, null, new object[] { tests, settings });
            if (settings.WarningBeforeStart)
            {
                DialogResult ret = MessageBox.Show(this, Properties.Resources.CTTWarning, Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (ret != DialogResult.Yes)
                {
                    return false;
                }
            }
            TransactionWork.Start();
            return true;
        }

        /// <summary>
        /// Show or hide conformance tests.
        /// </summary>
        private void conformanceTestsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            splitter4.Visible = conformanceTestsToolStripMenuItem1.Checked = !conformanceTestsToolStripMenuItem1.Checked;
            panel1.Visible = EventsView.Visible || TraceView.Visible || conformanceTestsToolStripMenuItem1.Checked;
            ConformanceTests.Visible = conformanceTestsToolStripMenuItem1.Checked;
            if (!conformanceTestsToolStripMenuItem1.Checked)
            {
                if (EventsView.Visible)
                {
                    EventsView.Dock = DockStyle.Fill;
                }
                else if (TraceView.Visible)
                {
                    TraceView.Dock = DockStyle.Fill;
                }
            }
            else
            {
                ConformanceTests.Dock = DockStyle.Fill;
                TraceView.Dock = EventsView.Dock = DockStyle.Left;
            }
        }

        private void contextMenuStrip2_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            runToolStripMenuItem.Enabled = Devices.Count != 0 && (TransactionWork == null || !TransactionWork.IsRunning);
            OpenContainingFolderBtn.Enabled = showValuesBtn.Enabled = ShowLogBtn.Enabled = ShowReportBtn.Enabled = ConformanceTests.SelectedItems.Count != 0;
            reRunToolStripMenuItem.Enabled = ConformanceTests.SelectedItems.Count != 0 && (TransactionWork == null || !TransactionWork.IsRunning);
        }

        /// <summary>
        /// Show read values in Conformance Test Tool.
        /// </summary>
        private void showValuesBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConformanceTests.SelectedItems.Count == 1)
                {
                    ListViewItem li = ConformanceTests.SelectedItems[0];
                    string path = Path.Combine((li.Tag as GXConformanceTest).Results, "Values.txt");
                    Process.Start(path);
                }
            }
            catch (Exception Ex)
            {
                Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Open containing folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenContainingFolderBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConformanceTests.SelectedItems.Count == 1)
                {
                    ListViewItem li = ConformanceTests.SelectedItems[0];
                    Process.Start("explorer.exe", string.Format("/select, \"{0}\"", (li.Tag as GXConformanceTest).Results));
                }
            }
            catch (Exception Ex)
            {
                Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Re-Run selected conformance test.
        /// </summary>
        private void reRunToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Clear trace.
        /// </summary>
        private void clearTraceMnu_Click(object sender, EventArgs e)
        {
            TraceView.ResetText();
        }

        private void CopyTrace_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(TraceView.Text);
            }
            catch (Exception ex)
            {
                Error.ShowError(this, ex);
            }
        }

        private void NotificationCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(EventsView.Text);
            }
            catch (Exception ex)
            {
                Error.ShowError(this, ex);
            }
        }

        /// <summary>
        /// Are Log comments shown.
        /// </summary>
        private void LogCommentsMenu_Click(object sender, EventArgs e)
        {
            LogCommentsMenu.Checked = !LogCommentsMenu.Checked;
            eventsTranslator.Comments = LogCommentsMenu.Checked;
        }

        /// <summary>
        /// Are Trace comments shown.
        /// </summary>
        private void TraceCommentsMenu_Click(object sender, EventArgs e)
        {
            TraceCommentsMenu.Checked = !TraceCommentsMenu.Checked;
            traceTranslator.Comments = TraceCommentsMenu.Checked;
        }

        /// <summary>
        /// Are Notification comments shown.
        /// </summary>
        private void NotificationsCommentsMenu_Click(object sender, EventArgs e)
        {
            NotificationsCommentsMenu.Checked = !NotificationsCommentsMenu.Checked;
            eventsTranslator.Comments = NotificationsCommentsMenu.Checked;
        }

        /// <summary>
        /// Add new object to the device.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddObjectMenu_Click(object sender, EventArgs e)
        {
            try
            {
                ObjectType ot = ObjectType.None;
                object tag = ObjectTree.SelectedNode.Tag;
                GXDLMSDevice dev = null;
                if (tag is GXDLMSDevice)
                {
                    dev = tag as GXDLMSDevice;
                }
                else if (tag is GXDLMSObject)
                {
                    ot = (tag as GXDLMSObject).ObjectType;
                    dev = GetDevice(tag as GXDLMSObject);
                }
                else
                {
                    dev = (GXDLMSDevice)ObjectTree.SelectedNode.Parent.Tag;
                }
                if (dev != null)
                {
                    GXObisCodeCollection collection = new GXObisCodeCollection();
                    GXObisCode item = new GXObisCode();
                    item.ObjectType = ot;
                    OBISCodeForm dlg = new OBISCodeForm(collection, item);
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        GXDLMSObject obj = GXDLMSClient.CreateObject(item.ObjectType);
                        obj.LogicalName = item.LogicalName;
                        obj.Version = item.Version;
                        obj.Description = item.Description;
                        UpdateAttributes(obj, item);
                        dev.Objects.Add(obj);
                        TreeNode deviceNode = (TreeNode)ObjectTreeItems[dev];
                        ListViewGroup group = null;
                        foreach (ListViewGroup g in ObjectList.Groups)
                        {
                            if (g.ToString() == obj.ObjectType.ToString())
                            {
                                group = g;
                                break;
                            }
                        }
                        if (group == null)
                        {
                            group = new ListViewGroup(obj.ObjectType.ToString(), obj.ObjectType.ToString());
                            ObjectList.Groups.Add(group);
                        }
                        ListViewItem li = AddNode(obj, deviceNode, group);
                        ObjectList.Items.Add(li);
                    }
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }
    }
}
