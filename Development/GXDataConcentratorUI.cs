using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gurux.DLMS;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.DLMS.Objects;

namespace GXDLMSDirector
{
    public partial class GXDataConcentratorUI : UserControl
    {
        IGXDataConcentrator target;
        GXManufacturerCollection Manufacturers;
        Dictionary<GXDLMSMeter, TreeNode> nodes = new Dictionary<GXDLMSMeter, TreeNode>();
        public GXDataConcentratorUI(GXManufacturerCollection manufacturers, IGXDataConcentrator dc)
        {
            Manufacturers = manufacturers;
            target = dc;
            InitializeComponent();
            target.OnMeterAdd += Target_OnMeterAdd;
            target.OnMeterEdit += Target_OnMeterEdit;
            target.OnMeterRemove += Target_OnMeterRemove;
            this.Dock = DockStyle.Fill;
        }

        private void Target_OnMeterRemove(object sender, GXDLMSMeter[] meter)
        {
            try
            {
                foreach (GXDLMSMeter it in meter)
                {
                    if (nodes.ContainsKey(it))
                    {
                        nodes[it].Remove();
                        nodes.Remove(it);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void Target_OnMeterEdit(object sender, GXDLMSMeter[] meter)
        {
            try
            {
                foreach (GXDLMSMeter it in meter)
                {
                    if (nodes.ContainsKey(it))
                    {
                        nodes[it].Text = it.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void Target_OnMeterAdd(object sender, GXDLMSMeter[] meter)
        {
            try
            {
                List<TreeNode> list = new List<TreeNode>();
                foreach (GXDLMSMeter it in meter)
                {
                    TreeNode n = new TreeNode() { Text = it.Name, Tag = it };
                    list.Add(n);
                    nodes.Add(it, n);
                    n.SelectedImageIndex = n.ImageIndex = 2;
                }
                Devices.Nodes.AddRange(list.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            try
            {
                List<TreeNode> list = new List<TreeNode>();
                nodes.Clear();
                foreach (GXDLMSMeter it in target.GetDevices(SearchTb.Text))
                {
                    TreeNode n = new TreeNode() { Text = it.Name, Tag = it };
                    list.Add(n);
                    nodes.Add(it, n);
                    n.SelectedImageIndex = n.ImageIndex = 2;
                }
                Devices.Nodes.Clear();
                Devices.Nodes.AddRange(list.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void AddDeviceCMnu_Click(object sender, EventArgs e)
        {
            try
            {
                DevicePropertiesForm dlg = new DevicePropertiesForm(Manufacturers, new GXDLMSMeter());
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    target.AddDevices(new GXDLMSMeter[] { dlg.Device });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void PropertiesCMnu_Click(object sender, EventArgs e)
        {
            try
            {
                if (Devices.SelectedNode != null)
                {
                    GXDLMSMeter m = Devices.SelectedNode.Tag as GXDLMSMeter;
                    DevicePropertiesForm dlg = new DevicePropertiesForm(Manufacturers, m);
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        target.EditDevices(new GXDLMSMeter[] { dlg.Device });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void DeleteCMnu_Click(object sender, EventArgs e)
        {
            try
            {
                if (Devices.SelectedNode != null)
                {
                    GXDLMSMeter m = Devices.SelectedNode.Tag as GXDLMSMeter;
                    if (MessageBox.Show(this, GXDLMSDirector.Properties.Resources.RemoveDeviceConfirmation, GXDLMSDirector.Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                    {
                        return;
                    }
                    target.RemoveDevices(new GXDLMSMeter[] { m });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void ReadCMnu_Click(object sender, EventArgs e)
        {
            try
            {
                if (Devices.SelectedNode != null)
                {
                    GXDLMSMeter dev = Devices.SelectedNode.Tag as GXDLMSMeter;
                    List<KeyValuePair<GXDLMSObject, byte>> objects = new List<KeyValuePair<GXDLMSObject, byte>>();
                    //Read association view if there are no objects on the meter.
                    if (dev.Objects.Count == 0)
                    {
                        objects.Add(new KeyValuePair<GXDLMSObject, byte>(new GXDLMSAssociationLogicalName(), 2));
                    }
                    target.ReadObjects(new GXDLMSMeter[] { dev }, objects);
                    if (dev.Objects.Count == 0)
                    {
                        //Add all objects.
                        TreeNode n = nodes[dev];
                        List<TreeNode> list = new List<TreeNode>();
                        foreach (GXDLMSObject it in (objects[0].Key as GXDLMSAssociationLogicalName).ObjectList)
                        {
                            GXManufacturer m = Manufacturers.FindByIdentification(dev.Manufacturer);
                            if (m.ObisCodes != null)
                            {
                                GXDLMSConverter c = new GXDLMSConverter();
                                GXObisCode oc = m.ObisCodes.FindByLN(it.ObjectType, it.LogicalName, null);
                                if (string.IsNullOrEmpty(it.Description))
                                {
                                    it.Description = c.GetDescription(it.LogicalName, it.ObjectType)[0];
                                }
                                if (oc != null)
                                {
                                    MainForm.UpdateAttributes(it, oc);
                                }
                            }
                            TreeNode t = new TreeNode(it.LogicalName + " " + it.Description);
                            t.Tag = it;
                            list.Add(t);
                        }
                        n.Nodes.AddRange(list.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
        }
    }
}
}
