//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
//
// Version:         $Revision: 9442 $,
//                  $Date: 2017-05-23 15:21:03 +0300 (ti, 23 touko 2017) $
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

using Gurux.DLMS.Objects;
using Gurux.DLMS.UI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GXDLMSDirector
{
    public partial class GXValuesDlg : Form
    {
        Dictionary<Type, List<IGXDLMSView>> Views;
        public GXValuesDlg(Dictionary<Type, List<IGXDLMSView>> views, string path)
        {
            InitializeComponent();
            Views = views;
            foreach (GXDLMSObject it in GXDLMSObjectCollection.Load(path))
            {
                ListViewItem li = new ListViewItem(it.LogicalName);
                li.Tag = it;
                ObjectList.Items.Add(li);
            }
            if (ObjectList.Items.Count != 0)
            {
                ObjectList.Items[0].Selected = true;
            }
        }

        private void OKBtn_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// User has selected new OBIS code.
        /// </summary>
        private void ObjectList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            try
            {
                GXDLMSObject obj = (GXDLMSObject)e.Item.Tag;
                IGXDLMSView SelectedView = GXDlmsUi.GetView(Views, obj);
                SelectedView.Target = obj;
                GXDlmsUi.ObjectChanged(SelectedView, null, obj, false);
                ObjectPanelFrame.Controls.Add(((Form)SelectedView));
                ((Form)SelectedView).Show();
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
            }
        }

        /// <summary>
        /// SelectedView must remove from the controls.
        /// </summary>
        private void GXValuesDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            ObjectPanelFrame.Controls.Clear();
        }
    }
}
