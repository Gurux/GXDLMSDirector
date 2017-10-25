using Gurux.Common;
using Gurux.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GXDLMSDirector
{
    public partial class GXNotificationDlg : Form
    {
        IGXMedia SelectedMedia;
        Form MediaPropertiesForm;
        public GXNotificationDlg(GXNet media)
        {
            InitializeComponent();
            SelectedMedia = media;
            MediaPropertiesForm = SelectedMedia.PropertiesForm;
            (MediaPropertiesForm as IGXPropertyPage).Initialize();
            while (MediaPropertiesForm.Controls.Count != 0)
            {
                Control ctr = MediaPropertiesForm.Controls[0];
                if (ctr is Panel)
                {
                    if (!ctr.Enabled)
                    {
                        MediaPropertiesForm.Controls.RemoveAt(0);
                        continue;
                    }
                }
                CustomSettings.Controls.Add(ctr);
                ctr.Visible = true;
                if (media.IsOpen)
                {
                    ctr.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Accept changes.
        /// </summary>
        private void OKBtn_Click(object sender, EventArgs e)
        {
            try
            {
                (MediaPropertiesForm as IGXPropertyPage).Apply();
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
                this.DialogResult = DialogResult.None;
            }

        }
    }
}
