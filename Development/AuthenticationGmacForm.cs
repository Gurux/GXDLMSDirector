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
    GXManufacturer Target;

    bool IsPrintable(byte[] str)
    {
        if (str != null)
        {
            foreach (char it in str)
            {
                if (!Char.IsLetterOrDigit(it))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public AuthenticationGmacForm(GXManufacturer target)
    {
        InitializeComponent();
        SecurityCB.Items.AddRange(new object[] { Security.None, Security.Authentication,
                                  Security.Encryption, Security.AuthenticationEncryption
                                               });
        Target = target;
        this.Text = " Security connection settings";
        this.SecurityCB.SelectedItem = target.Security;
        //Update default values.
        if (target.SystemTitle == null &&
                target.BlockCipherKey == null &&
                target.AuthenticationKey == null)
        {
            this.SystemTitleTB.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes("ABCDEFGH"), true);
            this.BlockCipherKeyTB.Text = "00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F";
            this.AuthenticationKeyTB.Text = "D0 D1 D2 D3 D4 D5 D6 D7 D8 D9 DA DB DC DD DE DF";
            AsciiRB.Enabled = false;
        }
        else
        {
            this.SystemTitleTB.Text = GXCommon.ToHex(target.SystemTitle, true);
            this.BlockCipherKeyTB.Text = GXCommon.ToHex(target.BlockCipherKey, true);
            this.AuthenticationKeyTB.Text = GXCommon.ToHex(target.AuthenticationKey, true);
        }
        if (!IsPrintable(target.SystemTitle) ||
                !IsPrintable(target.BlockCipherKey) ||
                !IsPrintable(target.AuthenticationKey))
        {
            AsciiRB.Enabled = false;
        }
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
            if (HexRB.Checked)
            {
                Target.SystemTitle = GXCommon.HexToBytes(SystemTitleTB.Text);
                Target.BlockCipherKey = GXCommon.HexToBytes(BlockCipherKeyTB.Text);
                Target.AuthenticationKey = GXCommon.HexToBytes(AuthenticationKeyTB.Text);
            }
            else
            {
                Target.SystemTitle = ASCIIEncoding.ASCII.GetBytes(SystemTitleTB.Text);
                Target.BlockCipherKey = ASCIIEncoding.ASCII.GetBytes(BlockCipherKeyTB.Text);
                Target.AuthenticationKey = ASCIIEncoding.ASCII.GetBytes(AuthenticationKeyTB.Text);
            }
        }
        catch (Exception Ex)
        {
            GXDLMS.Common.Error.ShowError(this, Ex);
            DialogResult = DialogResult.None;
        }
    }

    private void AsciiRB_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (HexRB.Checked)
            {
                SystemTitleTB.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(SystemTitleTB.Text), true);
                BlockCipherKeyTB.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(BlockCipherKeyTB.Text), true);
                AuthenticationKeyTB.Text = GXCommon.ToHex(ASCIIEncoding.ASCII.GetBytes(AuthenticationKeyTB.Text), true);
            }
            else
            {
                SystemTitleTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(SystemTitleTB.Text));
                BlockCipherKeyTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(BlockCipherKeyTB.Text));
                AuthenticationKeyTB.Text = ASCIIEncoding.ASCII.GetString(GXCommon.HexToBytes(AuthenticationKeyTB.Text));
            }
        }
        catch (Exception Ex)
        {
            GXDLMS.Common.Error.ShowError(this, Ex);
        }
    }

    private void ValidateHex(object sender, KeyPressEventArgs e)
    {
        if (HexRB.Checked)
        {
            if (!Char.IsNumber(e.KeyChar) &&
                    (Char.IsLetter(e.KeyChar) && !(e.KeyChar >= 'a' && e.KeyChar <= 'f' ||
                                                   e.KeyChar >= 'A' && e.KeyChar <= 'F')))
            {
                e.Handled = true;
            }
            if (AsciiRB.Enabled && !Char.IsLetterOrDigit(e.KeyChar))
            {
                AsciiRB.Enabled = false;
            }
        }
    }
}
}
