//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
//
// Version:         $Revision: 10624 $,
//                  $Date: 2019-04-24 13:56:09 +0300 (ke, 24 huhti 2019) $
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

using Gurux.DLMS;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Objects;
using Gurux.DLMS.UI;
using GXDLMSDirector.Macro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace GXDLMSDirector
{
    public partial class GXMacroView : Form
    {
        /// <summary>
        /// Logical Code to search for.
        /// </summary>
        string searchLn = null;
        /// <summary>
        /// Text to search for.
        /// </summary>
        string searchText = null;
        IGXDLMSView SelectedView = null;
        GXDLMSDevice Target;
        readonly private MRUManager mruManager = null;
        bool dirty = false;
        bool running = false;
        string path = null;
        readonly Dictionary<Type, List<IGXDLMSView>> Views;
        readonly List<GXMacro> Macros2 = new List<GXMacro>();
        Dictionary<GXMacro, GXMacro> Results = new Dictionary<GXMacro, GXMacro>();
        List<GXMacro> Filtered = new List<GXMacro>();

        //When macro is invoked N count this index will describe how many times macro is run.
        int RunIndex = 1;
        int RunCount = 1;

        public delegate void ConnectEventHandler(GXMacro act);
        public delegate void DisconnectEventHandler(GXMacro act);
        public delegate void GetEventHandler(GXMacro act);
        public delegate void SetEventHandler(GXMacro act);
        public delegate void ActionEventHandler(GXMacro act);
        delegate void UpdateTitleEventHandler();

        ConnectEventHandler m_OnConnect;
        DisconnectEventHandler m_OnDisconnect;
        GetEventHandler m_OnGet;
        SetEventHandler m_OnSet;
        ActionEventHandler m_OnAction;
        ExecuteActionEventHandler m_OnExecuted;


        private static void AddMacro(GXMacro it, List<GXMacro> list)
        {
            if (((Properties.Settings.Default.MacroHideFailed && !string.IsNullOrEmpty(it.Exception)) ||
                       (Properties.Settings.Default.MacroHideDisabled && it.Disable) ||
                       (Properties.Settings.Default.MacroIHideVerified && it.Verify) ||
                       (Properties.Settings.Default.MacroHideSucceeded && string.IsNullOrEmpty(it.Exception))))
            {

            }
            else
            {
                list.Add(it);
            }
        }

        private void UpdateFilteredMacros()
        {
            Filtered.Clear();
            foreach (GXMacro it in Macros2)
            {
                AddMacro(it, Filtered);
            }
        }

        private List<GXMacro> GetMacros()
        {
            if (Properties.Settings.Default.MacroHideFailed ||
                       Properties.Settings.Default.MacroHideDisabled ||
                       Properties.Settings.Default.MacroIHideVerified ||
                       Properties.Settings.Default.MacroHideSucceeded)
            {
                return Filtered;
            }
            return Macros2;
        }


        private static GXDLMSDevice GetDevice(object target)
        {
            GXDLMSDevice dev = null;
            if (target is GXDLMSDeviceCollection)
            {
                dev = null;
            }
            else if (target is GXDLMSDevice)
            {
                dev = target as GXDLMSDevice;
            }
            else if (target is GXDLMSObject)
            {
                dev = (target as GXDLMSObject).Parent.Tag as GXDLMSDevice;
            }
            else if (target is GXDLMSObjectCollection)
            {
                dev = (target as GXDLMSObjectCollection).Tag as GXDLMSDevice;
            }
            return dev;
        }


        public bool TargetChanged(object target)
        {
            GXDLMSDevice dev = GetDevice(target);
            if ((running || Record) && Target != dev)
            {
                return false;
            }
            Target = dev;
            return true;
        }

        void UpdateStatus()
        {
            if (Record)
            {
                StatusLbl.Text = "Recording";
            }
            else if (running)
            {
                if (RunCount != 1)
                {
                    StatusLbl.Text = string.Format("Running ({0}/{1}", RunIndex, RunCount);
                }
                else
                {
                    StatusLbl.Text = "Running";
                }
            }
            else
            {
                StatusLbl.Text = "Ready";
            }
        }

        private void UpdateTitle()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new UpdateTitleEventHandler(UpdateTitle));
            }
            else
            {
                string str = "Macro Editor " + path;
                if (dirty)
                {
                    str += " *";
                }
                this.Text = str;
            }
        }

        public bool Record
        {
            get;
            private set;
        }

        public event ConnectEventHandler OnConnect
        {
            add
            {
                m_OnConnect += value;
            }
            remove
            {
                m_OnConnect -= value;
            }
        }
        public event DisconnectEventHandler OnDisconnect
        {
            add
            {
                m_OnDisconnect += value;
            }
            remove
            {
                m_OnDisconnect -= value;
            }
        }
        public event GetEventHandler OnGet
        {
            add
            {
                m_OnGet += value;
            }
            remove
            {
                m_OnGet -= value;
            }
        }
        public event SetEventHandler OnSet
        {
            add
            {
                m_OnSet += value;
            }
            remove
            {
                m_OnSet -= value;
            }
        }
        public event ActionEventHandler OnAction
        {
            add
            {
                m_OnAction += value;
            }
            remove
            {
                m_OnAction -= value;
            }
        }


        public event ExecuteActionEventHandler OnExecuted
        {
            add
            {
                m_OnExecuted += value;
            }
            remove
            {
                m_OnExecuted -= value;
            }
        }

        public GXMacroView(GXDLMSMeterCollection meters)
        {
            InitializeComponent();
            RecordMnu.Text = recordToolStripMenuItem.Text = "Start Recording";
            mruManager = new MRUManager(RecentFilesMnu);
            mruManager.OnOpenMRUFile += new OpenMRUFileEventHandler(this.OnOpenMRUFile);
            if (!string.IsNullOrEmpty(Properties.Settings.Default.MacroFiles))
            {
                int pos = 0;
                string[] tmp = Properties.Settings.Default.MacroFiles.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string it in tmp)
                {
                    mruManager.Insert(pos, it);
                    ++pos;
                }
            }
            if (Properties.Settings.Default.MacroFollowLast)
            {
                FollowLastMnu_Click(null, null);
            }
            if (Properties.Settings.Default.MacroRaw)
            {
                RawMnu_Click(null, null);
            }
            if (Properties.Settings.Default.MacroBreakOnError)
            {
                BreakOnErrorMnu_Click(null, null);
            }

            if (Properties.Settings.Default.MacroHideFailed)
            {
                HideFailedMnu_Click(null, null);
            }
            if (Properties.Settings.Default.MacroHideDisabled)
            {
                HideDisabledMnu_Click(null, null);
            }
            if (Properties.Settings.Default.MacroIHideVerified)
            {
                HideVerifiedMnu_Click(null, null);
            }
            if (Properties.Settings.Default.MacroHideSucceeded)
            {
                HideSucceededMnu_Click(null, null);
            }
            Views = GXDlmsUi.GetViews(ObjectPanelFrame, null);
        }

        void OnOpenMRUFile(string fileName)
        {
            try
            {
                LoadFile(false, fileName);
            }
            catch (Exception Ex)
            {
                mruManager.Remove(fileName);
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        public delegate void AddActionEventHandler(GXMacro macro);



        /// <summary>
        /// Add new user action.
        /// </summary>
        /// <param name="macro"></param>
        public void AddAction(GXMacro macro)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new AddActionEventHandler(AddAction), macro);
            }
            else
            {
                dirty = true;
                macro.Timestamp = DateTime.Now;
                Macros2.Add(macro);
                AddMacro(macro, Filtered);
                //Serialization will remove \r and comparing result will fail...
                if (macro.Value != null)
                {
                    macro.Value = macro.Value.Replace("\r\n", "\n");
                }
                //Serialization will remove \r and comparing result will fail...
                if (macro.Data != null)
                {
                    macro.Data = macro.Data.Replace("\r\n", "\n");
                }
                if (Created)
                {
                    MacrosView.VirtualListSize = GetMacros().Count;
                    if (Properties.Settings.Default.MacroFollowLast)
                    {
                        MacrosView.EnsureVisible(GetMacros().Count - 1);
                    }
                    UpdateTitle();
                }
            }
        }

        private void GXMacroView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!SaveChanges())
            {
                e.Cancel = true;
            }
            else if (Record)
            {
                e.Cancel = true;
                MessageBox.Show(this, "Recording is on progress.", GXDLMSDirector.Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.OK);
            }
            else if (running)
            {
                e.Cancel = true;
                MessageBox.Show(this, "Macro is running.", GXDLMSDirector.Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.OK);
            }
            else
            {

                SelectedView = null;
                //SelectedView must remove from the controls.
                ObjectPanelFrame.Controls.Clear();
                Properties.Settings.Default.MacroFiles = string.Join(";", mruManager.GetNames());
                this.dirty = false;
                clearToolStripMenuItem_Click(null, null);
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    e.Cancel = true;
                    Hide();
                }
                else
                {

                }
            }
        }

        /// <summary>
        /// Show action data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MacrosView.SelectedIndices.Count == 1)
            {
                GXMacro macro = GetMacros()[MacrosView.SelectedIndices[0]];
                MacroDescription.Text = macro.Description;
                if (macro.Type == UserActionType.Get || macro.Type == UserActionType.Set)
                {
                    if (!tabControl1.TabPages.Contains(VisualizedTab))
                    {
                        tabControl1.TabPages.Insert(0, VisualizedTab);
                    }
                }
                else
                {
                    if (tabControl1.TabPages.Contains(VisualizedTab))
                    {
                        tabControl1.TabPages.Remove(VisualizedTab);
                    }
                }
                string actual, expected;
                if (Properties.Settings.Default.MacroRaw)
                {
                    if (macro.Verify && macro.Exception != null)
                    {
                        expected = macro.Exception;
                    }
                    else
                    {
                        expected = macro.Data;
                    }
                }
                else
                {
                    if (macro.Verify && macro.Exception != null)
                    {
                        expected = macro.Exception;
                    }
                    else
                    {
                        expected = macro.Value;
                    }
                }
                OriginalDataTb.Text = expected;
                if (Results.ContainsKey(macro))
                {
                    if (macro.Verify && macro.Exception != Results[macro].Exception)
                    {
                        actual = Results[macro].Exception;
                        expected = macro.Exception;
                    }
                    else if (Properties.Settings.Default.MacroRaw)
                    {
                        actual = Results[macro].Data;
                        expected = macro.Data;
                    }
                    else
                    {
                        actual = Results[macro].Value;
                        expected = macro.Value;
                    }
                    if (actual != expected)
                    {
                        ReplyDataTb.Text = actual;
                        ActualPanel.Visible = true;
                    }
                    else
                    {
                        ActualPanel.Visible = false;
                    }
                }
                else
                {
                    ActualPanel.Visible = false;
                    ReplyDataTb.Text = null;
                }
                try
                {
                    if (macro.Type == UserActionType.Get || macro.Type == UserActionType.Set)
                    {
                        if (macro.ObjectType != 0)
                        {
                            GXDLMSObject obj = GXDLMSClient.CreateObject((ObjectType)macro.ObjectType, macro.ObjectVersion);
                            if (obj != null)
                            {
                                obj.LogicalName = macro.LogicalName;
                                for (int pos = 1; pos != (obj as IGXDLMSBase).GetAttributeCount() + 1; ++pos)
                                {
                                    obj.SetAccess(pos, AccessMode.NoAccess);
                                }
                                obj.SetAccess(macro.Index, AccessMode.ReadWrite);
                                obj.SetDataType(macro.Index, (DataType)macro.DataType);
                                obj.SetUIDataType(macro.Index, (DataType)macro.UIDataType);
                                if (!string.IsNullOrEmpty(macro.Data) && Target != null)
                                {
                                    object value = GXDLMSTranslator.XmlToValue(macro.Data);
                                    if (value is byte[] && macro.UIDataType != 0)
                                    {
                                        value = Target.Comm.client.ChangeType(new GXByteBuffer((byte[])value), (DataType)macro.UIDataType);
                                    }
                                    else if (value is byte[] &&
                                        macro.DataType != (int)DataType.None &&
                                        macro.DataType != (int)DataType.OctetString)
                                    {
                                        if (macro.DataType == (int)DataType.Array ||
                                            macro.DataType == (int)DataType.Structure)
                                        {
                                            GXByteBuffer bb = new GXByteBuffer((byte[])value);
                                            //Skip data type.
                                            bb.Position = 1;
                                            value = Target.Comm.client.ChangeType(bb, (DataType)macro.DataType);
                                        }
                                        else
                                        {
                                            GXByteBuffer bb = new GXByteBuffer((byte[])value);
                                            value = Target.Comm.client.ChangeType(bb, (DataType)macro.DataType);
                                        }
                                    }
                                    if (macro.ObjectType == (int)ObjectType.ProfileGeneric && macro.Index == 2 && !string.IsNullOrEmpty(macro.External))
                                    {
                                        Target.Comm.client.UpdateValue(obj, 3, GXDLMSTranslator.XmlToValue(macro.External));
                                    }
                                    Target.Comm.client.UpdateValue(obj, macro.Index, value);
                                }
                                if (SelectedView == null || SelectedView.Target.ObjectType != obj.ObjectType)
                                {
                                    //SelectedView must remove from the controls.
                                    ObjectPanelFrame.Controls.Clear();
                                    if (Target == null)
                                    {
                                        SelectedView = GXDlmsUi.GetView(Views, obj, Standard.DLMS);
                                    }
                                    else
                                    {
                                        SelectedView = GXDlmsUi.GetView(Views, obj, Target.Comm.client.Standard);
                                    }
                                    SelectedView.Target = obj;
                                    GXDlmsUi.ObjectChanged(SelectedView, Target != null ? Target.Comm.client : null, obj, false);
                                    SelectedView.OnDirtyChange(macro.Index, true);
                                    ObjectPanelFrame.Controls.Add(((Form)SelectedView));
                                    ((Form)SelectedView).Show();
                                }
                                else
                                {
                                    SelectedView.Target = obj;
                                    GXDlmsUi.ObjectChanged(SelectedView, Target != null ? Target.Comm.client : null, obj, false);
                                    SelectedView.OnDirtyChange(macro.Index, true);
                                }
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    GXDLMS.Common.Error.ShowError(this, Ex);
                }
            }
            else
            {
                MacroDescription.Text = "";
                OriginalDataTb.Text = "";
                if (GetMacros().Count == 0 && tabControl1.TabPages.Contains(VisualizedTab))
                {
                    tabControl1.TabPages.Remove(VisualizedTab);
                }
            }
        }

        private bool SaveChanges()
        {
            //Save changes?
            if (this.dirty)
            {
                DialogResult ret = MessageBox.Show(this, Properties.Resources.SaveChangesTxt, Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (ret == DialogResult.Cancel)
                {
                    return false;
                }
                if (ret == DialogResult.Yes)
                {
                    if (!Save(false))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Clear actions.
        /// </summary>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveChanges())
            {
                path = null;
                dirty = false;
                UpdateTitle();
                SelectedView = null;
                //SelectedView must remove from the controls.
                ObjectPanelFrame.Controls.Clear();
                MacrosView.VirtualListSize = 0;
                Macros2.Clear();
                Filtered.Clear();
                MacrosView.SelectedIndices.Clear();
                ActionsList_SelectedIndexChanged(null, null);
            }
        }

        /// <summary>
        /// Remove selected actions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, GXDLMSDirector.Properties.Resources.RemoveObjectConfirmation, Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                //Remove items starting from the last one.
                for (int pos = MacrosView.SelectedIndices.Count - 1; pos > -1; --pos)
                {
                    GetMacros().RemoveAt(MacrosView.SelectedIndices[pos]);
                }
                MacrosView.VirtualListSize = GetMacros().Count;
                dirty = true;
                UpdateTitle();
            }
        }

        private void GXMacroView_Load(object sender, EventArgs e)
        {
            MacrosView.VirtualListSize = GetMacros().Count;
        }

        private void UpdateRunMenu(bool value)
        {
            running = value;
            runSelectedToolStripMenuItem1.Enabled = runSelectedToolStripMenuItem.Enabled = RunMnu.Enabled = !running;
            runToolStripMenuItem.Enabled = RecordMnu.Enabled = recordToolStripMenuItem.Enabled = !running;
            RecordBtn.Enabled = RunBtn.Enabled = !running;
            NewBtn.Enabled = SaveBtn.Enabled = SaveBtn.Enabled = OpenBtn.Enabled = DeleteBtn.Enabled = !running;
            NewMnu.Enabled = LoadMnu.Enabled = SaveMnu.Enabled = SaveAsMnu.Enabled = importToolStripMenuItem1.Enabled = !running;
            RemoveMnu.Enabled = ignoreResultToolStripMenuItem.Enabled = ignoreReplyToolStripMenuItem.Enabled = !running;
            disableToolStripMenuItem.Enabled = enableToolStripMenuItem.Enabled = RunMnu.Enabled = !running;
            runSelectedToolStripMenuItem1.Enabled = RecordMnu.Enabled = !running;
            clearToolStripMenuItem.Enabled = loadToolStripMenuItem.Enabled = saveToolStripMenuItem.Enabled = !running;
            saveAsToolStripMenuItem.Enabled = importToolStripMenuItem.Enabled = RecentFilesMnu.Enabled = closeToolStripMenuItem.Enabled = !running;
            removeToolStripMenuItem.Enabled = VerifyReplyMnu.Enabled = IgnoreReplyMnu.Enabled = DisableMnu.Enabled = !running;
            EnableMnu.Enabled = runToolStripMenuItem.Enabled = runSelectedToolStripMenuItem.Enabled = recordToolStripMenuItem.Enabled = !running;
        }

        private void Run(System.Collections.IList items, List<GXMacro> macros)
        {
            try
            {
                if (Target == null)
                {
                    throw new Exception("Failed to run macro. No device is selected.");
                }
                UpdateRunMenu(true);
                UpdateStatus();
                GXMacro selectedMacro = null;
                if (MacrosView.SelectedIndices.Count == 1)
                {
                    selectedMacro = GetMacros()[MacrosView.SelectedIndices[0]];
                }
                Results.Clear();
                foreach (int it in items)
                {
                    GXMacro orig = macros[it];
                    GXMacro macro = orig.Clone();
                    orig.LastRunMacro = macro;
                    Results.Add(orig, macro);
                    if (Properties.Settings.Default.MacroFollowLast)
                    {
                        MacrosView.EnsureVisible(it);
                    }
                    if (!macro.Disable)
                    {
                        macro.Exception = null;
                        try
                        {
                            orig.Running = true;
                            MacrosView.RedrawItems(it, it, false);
                            switch (macro.Type)
                            {
                                case UserActionType.Connect:
                                    m_OnConnect(macro);
                                    break;
                                case UserActionType.Disconnecting:
                                    m_OnDisconnect(macro);
                                    break;
                                case UserActionType.Get:
                                    macro.Value = macro.Data = macro.Exception = null;
                                    macro.DataType = 0;
                                    m_OnGet(macro);
                                    //Update data if it's updated.
                                    if (macro.Verify)
                                    {
                                        if (macro.Exception != orig.Exception)
                                        {
                                            macro.Exception = "Different exceptions.";
                                        }
                                        else if (macro.DataType != orig.DataType)
                                        {
                                            macro.Exception = "Different data type.";
                                        }
                                        else if (macro.Data != orig.Data)
                                        {
                                            macro.Exception = "Different data.";
                                        }
                                    }
                                    break;
                                case UserActionType.Set:
                                    m_OnSet(macro);
                                    break;
                                case UserActionType.Action:
                                    m_OnAction(macro);
                                    break;
                                case UserActionType.Delay:
                                    Thread.Sleep(int.Parse(macro.Value));
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            //If macro is verified and exception is expected.
                            if (orig.Verify && orig.Exception != null)
                            {
                                macro.Exception = ex.Message;
                            }
                            else
                            {
                                macro.Exception = ex.Message;
                                if (Properties.Settings.Default.MacroBreakOnError)
                                {
                                    throw;
                                }
                            }
                        }
                        finally
                        {
                            //If exception is expected.
                            if (orig.Verify && orig.Exception != macro.Exception)
                            {
                                if (macro.Exception == null)
                                {
                                    macro.Exception = "Error expected but meter didn't return an error.";
                                }
                                else
                                {
                                    macro.Exception = "Meter returned different exception than expected. " + macro.Exception;
                                }
                            }
                            orig.Running = false;
                            MacrosView.RedrawItems(it, it, false);
                        }
                        if (orig == selectedMacro)
                        {
                            ActionsList_SelectedIndexChanged(null, null);
                        }
                    }
                }
            }
            finally
            {
                UpdateRunMenu(false);
                UpdateStatus();
            }
        }

        /// <summary>
        /// Run selected commands.
        /// </summary>
        private void RunMnu_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> list = new List<int>();
                for (int pos = 0; pos != Macros2.Count; ++pos)
                {
                    list.Add(pos);
                }
                Run(list, Macros2);
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Load macro from the file.
        /// </summary>
        /// <param name="import">Is macro imported(append).</param>
        public void LoadFile(bool import, string path)
        {
            dirty = import;
            SelectedView = null;
            //SelectedView must remove from the controls.
            ObjectPanelFrame.Controls.Clear();
            if (!import)
            {
                MacrosView.VirtualListSize = 0;
                Macros2.Clear();
                Filtered.Clear();
                mruManager.Insert(0, path);
                this.path = path;
            }
            UpdateTitle();
            using (XmlReader reader = XmlReader.Create(path))
            {
                XmlSerializer x = new XmlSerializer(typeof(GXMacro[]));
                Macros2.AddRange((GXMacro[])x.Deserialize(reader));
                UpdateFilteredMacros();
                reader.Close();
                MacrosView.VirtualListSize = GetMacros().Count;
            }
            if (!import && GetMacros().Count != 0)
            {
                MacrosView.SelectedIndices.Clear();
                MacrosView.SelectedIndices.Add(0);
            }
        }

        /// <summary>
        /// Load macro from the file.
        /// </summary>
        /// <param name="import">Is macro imported(append).</param>
        private void LoadFile(bool import)
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
            dlg.Filter = Properties.Resources.MacroFilterTxt;
            dlg.DefaultExt = ".gdm";
            dlg.ValidateNames = true;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                path = dlg.FileName;
                if (File.Exists(path))
                {
                    LoadFile(import, path);
                }
            }
        }


        private void LoadMnu_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveChanges())
                {
                    LoadFile(false);
                }
            }
            catch (Exception Ex)
            {
                if (path != null)
                {
                    mruManager.Remove(path);
                }
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Save macros to the file.
        /// </summary>
        /// <param name="saveAs">Is file saveed with a new name.</param>
        /// <returns>Returns false, if user has cancel the action.</returns>
        private bool Save(bool saveAs)
        {
            if (saveAs || string.IsNullOrEmpty(path))
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = Properties.Resources.MacroFilterTxt;
                dlg.DefaultExt = ".gxm";
                dlg.InitialDirectory = Directory.GetCurrentDirectory();
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    return false;
                }
                path = dlg.FileName;
            }
            using (Stream stream = File.Open(path, FileMode.Create))
            {
                using (TextWriter writer = new StreamWriter(stream))
                {
                    writer.NewLine = Environment.NewLine;
                    XmlSerializer x = new XmlSerializer(typeof(GXMacro[]));
                    x.Serialize(writer, Macros2.ToArray());
                    writer.Close();
                }
                stream.Close();
            }
            mruManager.Insert(0, path);
            dirty = false;
            UpdateTitle();
            return true;
        }

        private void SaveMnu_Click(object sender, EventArgs e)
        {
            try
            {
                Save(false);
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
                Save(true);
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void recordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Target == null)
                {
                    throw new Exception("Failed to record macro. No device is selected.");
                }
                Record = !Record;
                RecordBtn.Checked = Record;
                NewBtn.Enabled = OpenBtn.Enabled = SaveBtn.Enabled = !Record;
                NewMnu.Enabled = LoadMnu.Enabled = SaveMnu.Enabled = SaveAsMnu.Enabled = !Record;
                clearToolStripMenuItem.Enabled = loadToolStripMenuItem.Enabled = saveToolStripMenuItem.Enabled = saveAsToolStripMenuItem.Enabled = !Record;
                runToolStripMenuItem.Enabled = RunMnu.Enabled = !Record;
                RunBtn.Enabled = runSelectedToolStripMenuItem.Enabled = runSelectedToolStripMenuItem1.Enabled = !Record;
                DeleteBtn.Enabled = removeToolStripMenuItem.Enabled = RemoveMnu.Enabled = !Record;
                if (Record)
                {
                    RecordMnu.Text = recordToolStripMenuItem.Text = "Stop Recording";
                }
                else
                {
                    RecordMnu.Text = recordToolStripMenuItem.Text = "Start Recording";
                }
                UpdateStatus();
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void ActionMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void runSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MacrosView.SelectedIndices.Count == 0)
                {
                    throw new Exception("Failed to run selected macros. No macro is selected.");
                }
                Run(MacrosView.SelectedIndices, GetMacros());
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                LoadFile(true);
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void ActionsList_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void ActionsList_DragDrop(object sender, DragEventArgs e)
        {
            if (MacrosView.SelectedIndices.Count != 0)
            {
                Point pt = MacrosView.PointToClient(new Point(e.X, e.Y));
                ListViewItem dIt = MacrosView.GetItemAt(pt.X, pt.Y);
                if (dIt != null)
                {
                    List<int> updated = new List<int>();
                    updated.Add(dIt.Index);
                    int index = dIt.Index;
                    foreach (int pos in MacrosView.SelectedIndices)
                    {
                        if (index != pos)
                        {
                            GXMacro old = GetMacros()[pos];
                            Macros2.Remove(old);
                            Macros2.Insert(index, old);
                            updated.Add(pos);
                            ++index;
                        }
                    }
                    foreach (int it in updated)
                    {
                        MacrosView.RedrawItems(it, it, false);
                    }
                    dirty = true;
                    UpdateTitle();
                }
            }
        }

        private void ActionsList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            MacrosView.DoDragDrop(MacrosView.SelectedItems, DragDropEffects.Move);
        }

        /// <summary>
        /// Verify reply for selected macros.
        /// </summary>
        private void VerifyReplyMnu_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (int it in MacrosView.SelectedIndices)
                {
                    GetMacros()[it].Verify = true;
                    MacrosView.RedrawItems(it, it, false);
                }
                dirty = true;
                UpdateTitle();
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Don't verify reply for selected macros.
        /// </summary>
        private void IgnoreReplyMnu_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (int it in MacrosView.SelectedIndices)
                {
                    GetMacros()[it].Verify = false;
                    MacrosView.RedrawItems(it, it, false);
                }
                dirty = true;
                UpdateTitle();
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Mark selected macros to disabled.
        /// </summary>
        private void DisableMnu_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (int it in MacrosView.SelectedIndices)
                {
                    GetMacros()[it].Disable = true;
                    MacrosView.RedrawItems(it, it, false);
                }
                dirty = true;
                UpdateTitle();
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }
        /// <summary>
        /// Mark selected macros to enabled.
        /// </summary>
        private void EnableMnu_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (int it in MacrosView.SelectedIndices)
                {
                    GetMacros()[it].Disable = false;
                    MacrosView.RedrawItems(it, it, false);
                }
                dirty = true;
                UpdateTitle();
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Hide failed items.
        /// </summary>
        private void HideFailedMnu_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.MacroHideFailed = HideFailedMnu.Checked = !HideFailedMnu.Checked;
                UpdateFilteredMacros();
                MacrosView.VirtualListSize = GetMacros().Count;
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Hide disabled.
        /// </summary>
        private void HideDisabledMnu_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.MacroHideDisabled = HideDisabledMnu.Checked = !HideDisabledMnu.Checked;
                UpdateFilteredMacros();
                MacrosView.VirtualListSize = GetMacros().Count;
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Hide verified.
        /// </summary>
        private void HideVerifiedMnu_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.MacroIHideVerified = HideVerifiedMnu.Checked = !HideVerifiedMnu.Checked;
                UpdateFilteredMacros();
                MacrosView.VirtualListSize = GetMacros().Count;
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Hide succeeded.
        /// </summary>
        private void HideSucceededMnu_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.MacroHideSucceeded = HideSucceededMnu.Checked = !HideSucceededMnu.Checked;
                UpdateFilteredMacros();
                MacrosView.VirtualListSize = GetMacros().Count;
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Is reply data shown in Raw (XML) format.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RawMnu_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.MacroRaw = RawMnu.Checked = !RawMnu.Checked;
                ActionsList_SelectedIndexChanged(null, null);
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Show Macro Editor help.
        /// </summary>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://www.gurux.fi/MacroEditor");
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Select all items.
        /// </summary>
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MacrosView.SelectedIndices.Clear();
            for (int pos = 0; pos != GetMacros().Count; ++pos)
            {
                MacrosView.SelectedIndices.Add(pos);
            }
        }

        /// <summary>
        /// Draw macro on Virtual View mode.
        /// </summary>
        private void MacrosView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            lock (Macros2)
            {
                if (e.ItemIndex < GetMacros().Count)
                {
                    GXMacro action = GetMacros()[e.ItemIndex];
                    string value = action.Value;
                    if (value != null && value.Length > 200)
                    {
                        value = value.Substring(0, 200) + "...";
                    }
                    ListViewItem li = new ListViewItem(new string[] { action.Timestamp.ToString(), action.Type.ToString(), action.Name, value });
                    li.Tag = action;
                    //Update image.
                    if (action.Disable)
                    {
                        li.ImageIndex = 5;
                    }
                    else if (!string.IsNullOrEmpty(action.Exception))
                    {
                        if (action.Verify)
                        {
                            li.ImageIndex = 3;
                        }
                        else
                        {
                            li.ImageIndex = 2;
                        }
                        //Exception that has occurred on recording.
                        li.SubItems[3].Text = action.Exception;
                    }
                    else if (action.Verify && action.Type != UserActionType.Connect && action.Type != UserActionType.Disconnecting)
                    {
                        li.ImageIndex = 3;
                    }
                    else
                    {
                        li.ImageIndex = 0;
                    }
                    if (action.Running)
                    {
                        li.BackColor = Color.Green;
                    }
                    else if (action.LastRunMacro != null && action.LastRunMacro.Exception != action.Exception)
                    {
                        //If macro run fails.
                        li.BackColor = Color.Red;
                        li.SubItems[3].Text = action.LastRunMacro.Exception;
                    }
                    e.Item = li;
                }
            }
        }

        /// <summary>
        /// Is new item followed on macro recording.
        /// </summary>
        private void FollowLastMnu_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.MacroFollowLast = FollowLastMnu.Checked = !FollowLastMnu.Checked;
        }

        /// <summary>
        /// Is running macros stop on error.
        /// </summary>
        private void BreakOnErrorMnu_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.MacroBreakOnError = BreakOnErrorMnu.Checked = !BreakOnErrorMnu.Checked;
        }

        /// <summary>
        /// Find macro.
        /// </summary>
        private void FindNextMnu_Click(object sender, EventArgs e)
        {
            try
            {
                GXFindParameters p = new GXFindParameters();
                p.ObisCode = searchLn;
                p.Text = searchText;
                if (GXDlmsUi.Find(this, p))
                {
                    searchLn = p.ObisCode;
                    searchText = p.Text;
                    FindMnu.Enabled = true;
                    FindMnu_Click(null, null);
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Find Macro.
        /// </summary>
        /// <param name="nodes">List of macros to search.</param>
        /// <param name="text">OBIS code to search</param>
        /// <param name="first">If selected item found.</param>
        /// <returns>Is macro found.</returns>
        private bool FindMacro(List<GXMacro> macros, ref bool first)
        {
            int index = 0;
            if (!first && MacrosView.SelectedIndices.Count == 1)
            {
                index = MacrosView.SelectedIndices[0];
                ++index;
            }
            if (searchText != null)
            {
                searchText = searchText.ToLower();
            }
            for (int pos = index; pos < macros.Count; ++pos)
            {
                GXMacro it = macros[pos];
                if ((searchLn != null && it.LogicalName == searchLn) ||
                    (it.Name != null && it.Name.ToLower().Contains(searchText)) ||
                    (it.Value != null && it.Value.ToLower().Contains(searchText)) ||
                    (searchText != null && it.Type.ToString().ToLower().Contains(searchText)))
                {
                    MacrosView.SelectedIndices.Clear();
                    MacrosView.SelectedIndices.Add(pos);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Find next macro.
        /// </summary>
        private void FindMnu_Click(object sender, EventArgs e)
        {
            try
            {
                bool first = false;
                List<GXMacro> macros = GetMacros();
                if (!FindMacro(macros, ref first))
                {
                    first = true;
                    if (!FindMacro(macros, ref first))
                    {
                        if (searchLn != null)
                        {
                            throw new Exception("OBIS code '" + searchLn + "' not found!");
                        }
                        else
                        {
                            throw new Exception("Text '" + searchText + "' not found!");
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// Run macro N times.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void runNTimesMnu_Click(object sender, EventArgs e)
        {
            try
            {
                GXMacroCountDlg dlg = new GXMacroCountDlg();
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    List<int> list = new List<int>();
                    for (int pos = 0; pos != Macros2.Count; ++pos)
                    {
                        list.Add(pos);
                    }
                    RunIndex = 0;
                    RunCount = Properties.Settings.Default.MacroEditorInvokeCount;
                    for (int pos = 0; pos != Properties.Settings.Default.MacroEditorInvokeCount; ++pos)
                    {
                        ++RunIndex;
                        Run(list, Macros2);
                    }
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
            RunIndex = 1;
            RunCount = 1;
        }

        private void InsertDelayMnu_Click(object sender, EventArgs e)
        {
            try
            {
                int index;
                if (MacrosView.SelectedIndices.Count == 0)
                {
                    index = Macros2.Count;
                }
                else
                {
                    index = MacrosView.SelectedIndices[0];
                }
                GXMacro delay = new GXMacro();
                delay.Type = UserActionType.Delay;
                GXMacroDelayDlg dlg = new GXMacroDelayDlg(delay);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    Macros2.Insert(index, delay);
                    MacrosView.VirtualListSize = GetMacros().Count;
                    MacrosView.RedrawItems(index, index, false);
                    dirty = true;
                    UpdateTitle();
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void ShowProperties()
        {
            int index = MacrosView.SelectedIndices[0];
            GXMacro macro = GetMacros()[index];
            Form dlg;
            if (macro.Type == UserActionType.Delay)
            {
                dlg = new GXMacroDelayDlg(macro);
            }
            else
            {
                dlg = new GXMacroEditDlg(macro);
            }
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                MacroDescription.Text = macro.Description;
                MacrosView.RedrawItems(index, index, false);
                dirty = true;
                UpdateTitle();
            }
        }

        private void PropertiesMnu_Click(object sender, EventArgs e)
        {
            try
            {
                if (MacrosView.SelectedIndices.Count == 1)
                {
                    ShowProperties();
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        private void MacrosView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (MacrosView.SelectedIndices.Count == 1)
                {
                    ShowProperties();
                }
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }
    }
}
