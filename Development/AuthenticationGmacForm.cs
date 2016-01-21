using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gurux.DLMS;
using Gurux.DLMS.ManufacturerSettings;
using Gurux.Common;
using Gurux.DLMS.Enums;

namespace GXDLMSDirector
{
    public partial class AuthenticationGmacForm : Form
    {
        GXAuthentication Target;

        public AuthenticationGmacForm(GXAuthentication target)
        {
            InitializeComponent();
            SecurityCB.Items.AddRange(new object[] { Security.None, Security.Authentication, 
                                                    Security.Encryption, Security.AuthenticationEncryption});
            Target = target;
            this.Text = Target.Type.ToString() + " authentication settings";
            this.SecurityCB.SelectedItem = target.Security;
            this.SystemTitleTB.Text = GXCommon.ToHex(target.SystemTitle, true);
            this.BlockCipherKeyTB.Text = GXCommon.ToHex(target.BlockCipherKey, true);
            this.AuthenticationKeyTB.Text = GXCommon.ToHex(target.AuthenticationKey, true);
        }

        private void SecurityCB_DrawItem(object sender, DrawItemEventArgs e)
        {
            // If the index is invalid then simply exit.
            if (e.Index == -1 || e.Index >= SecurityCB.Items.Count)
            {
                return;
            }

            // Draw the background of the item.
            e.DrawBackground();

            // Should we draw the focus rectangle?
            if ((e.State & DrawItemState.Focus) != 0)
            {
                e.DrawFocusRectangle();
            }

            Font f = new Font(e.Font, FontStyle.Regular);
            // Create a new background brush.
            Brush b = new SolidBrush(e.ForeColor);
            // Draw the item.			
            Security security = (Security)SecurityCB.Items[e.Index];
            string name = security.ToString();
            SizeF s = e.Graphics.MeasureString(name, f);
            e.Graphics.DrawString(name, f, b, e.Bounds);

        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Target.Security = (Security)SecurityCB.SelectedItem;
                Target.SystemTitle = GXCommon.HexToBytes(SystemTitleTB.Text, true);
                Target.BlockCipherKey = GXCommon.HexToBytes(BlockCipherKeyTB.Text, true);
                Target.AuthenticationKey = GXCommon.HexToBytes(AuthenticationKeyTB.Text, true);
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(this, Ex);
                DialogResult = DialogResult.None;
            }
        }
    }
}
