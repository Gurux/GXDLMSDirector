using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gurux.DLMS.Objects;
using Gurux.Common;
using Gurux.DLMS.Enums;

namespace GXDLMSDirector.Views
{
    public partial class GXDeviceSettingsView : UserControl
    {
        public GXDeviceSettingsView()
        {
            InitializeComponent();
            //this.Dock = DockStyle.Fill;
        }

        public void Update(GXDLMSDevice dev)
        {
            DedicatedKeyTb.Text = GXCommon.ToHex(GXCommon.HexToBytes(dev.DedicatedKey));
            AuthenticationTb.Text = dev.Authentication.ToString();
            if (dev.Security != Security.None || dev.Authentication == Authentication.HighGMAC ||
                dev.Authentication == Authentication.HighECDSA)
            {
                ClientSystemTitleTb.Text = GXCommon.ToHex(GXCommon.HexToBytes(dev.SystemTitle));
                if (dev.PreEstablished)
                {
                    ServerSystemTitleTb.Text = GXCommon.ToHex(GXCommon.HexToBytes(dev.ServerSystemTitle));
                }
                else
                {
                    ServerSystemTitleTb.Text = GXCommon.ToHex(dev.Comm.client.ServerSystemTitle);
                }
                SecurityTb.Text = dev.Security.ToString();
                AuthenticationKeyTb.Text = GXCommon.ToHex(GXCommon.HexToBytes(dev.AuthenticationKey));
                BlockCipherKeyTb.Text = GXCommon.ToHex(GXCommon.HexToBytes(dev.BlockCipherKey));
            }
            else
            {
                AuthenticationKeyTb.Text = BlockCipherKeyTb.Text = SecurityTb.Text = ClientSystemTitleTb.Text = ServerSystemTitleTb.Text = "";
            }
            NetworkIDTb.Text = dev.NetworkId.ToString();
            PhysicalDeviceAddressTb.Text = GXCommon.ToHex(GXCommon.HexToBytes(dev.PhysicalDeviceAddress));


            DeviceGb.Text = dev.Name;
            ClientAddressValueLbl.Text = dev.ClientAddress.ToString();
            LogicalAddressValueLbl.Text = dev.LogicalAddress.ToString();
            PhysicalAddressValueLbl.Text = dev.PhysicalAddress.ToString();
            ManufacturerValueLbl.Text = dev.Manufacturers.FindByIdentification(dev.Manufacturer).Name;
            ProposedConformanceTB.Text = dev.Comm.client.ProposedConformance.ToString();
            NegotiatedConformanceTB.Text = dev.Comm.client.NegotiatedConformance.ToString();
            DeviceInfoView.BringToFront();
            ErrorsView.Items.Clear();
            foreach (GXDLMSObject it in dev.Objects)
            {
                SortedDictionary<int, Exception> errors = it.GetLastErrors();
                if (errors.Count != 0)
                {
                    int count = (it as IGXDLMSBase).GetAttributeCount() + 1;
                    int add = ErrorsView.Columns.Count;
                    count -= add;
                    for (int pos = 0; pos < count; ++pos)
                    {
                        ErrorsView.Columns.Add("Attribute " + (add + pos).ToString());
                    }
                    count = (it as IGXDLMSBase).GetAttributeCount();
                    ListViewItem lv = new ListViewItem(it.LogicalName + " " + it.Description);
                    lv.Tag = it;
                    for (int pos = 0; pos < count; ++pos)
                    {
                        lv.SubItems.Add("");
                    }
                    foreach (var e in errors)
                    {
                        lv.SubItems[e.Key].Text = e.Value.Message;
                    }
                    ErrorsView.Items.Add(lv);
                }
            }
        }
    }
}
