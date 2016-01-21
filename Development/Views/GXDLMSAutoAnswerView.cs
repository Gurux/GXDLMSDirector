//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL:$
//
// Version:         $Revision: $,
//                  $Date:  $
//                  $Author: $
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gurux.DLMS.Objects;
using GXDLMS.Common;
using Gurux.DLMS;
using Gurux.DLMS.Enums;

namespace GXDLMSDirector.Views
{
    [GXDLMSViewAttribute(typeof(GXDLMSAutoAnswer))]
    public partial class GXDLMSAutoAnswerView : Form, IGXDLMSView
    {
        public GXDLMSAutoAnswerView()
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
            GXDLMSAutoAnswer target = Target as GXDLMSAutoAnswer;
            if (attributeID == 3)
            {                
                ListeningWindowLV.Items.Clear();
                if (target.ListeningWindow != null)
                {
                    foreach (var it in target.ListeningWindow)
                    {                        
                        ListViewItem li = ListeningWindowLV.Items.Add(it.Key.ToString());
                        li.SubItems.Add(GXHelpers.ConvertDLMS2String(it.Value.ToString()));
                    }
                }
            }
            else if (attributeID == 5)
            {
                if (target.NumberOfCalls == 0)
                {
                    NumberOfCallsTB.Value = "No limit.";
                }
                else
                {
                    NumberOfCallsTB.Value = target.NumberOfCalls.ToString();
                }

            }
            else if (attributeID == 6)
            {                
                if (target.NumberOfRingsInListeningWindow == 0)
                {
                    this.RingCountInWindowTB.Text = "No connect.";
                }
                else
                {
                    this.RingCountInWindowTB.Text = target.NumberOfRingsInListeningWindow.ToString();
                }
                if (target.NumberOfRingsOutListeningWindow == 0)
                {
                    this.RingCountOutOfWindowTB.Text = "No connect.";
                }
                else
                {
                    this.RingCountOutOfWindowTB.Text = target.NumberOfRingsOutListeningWindow.ToString();
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
    }
}
