using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GXDLMSDirector.Macro
{
    public partial class GXMacroDelayDlg : Form
    {
        GXMacro _macro;
        public GXMacroDelayDlg(GXMacro macro)
        {
            InitializeComponent();
            _macro = macro;
            int delay = 1000;
            if (!string.IsNullOrEmpty(macro.Value))
            {
                delay = int.Parse(macro.Value);
            }
            NameTb.Text = macro.Name;
            DelayTb.Value = new DateTime(2000, 1, 1).AddMilliseconds(delay);
            DescriptionTb.Text = macro.Description;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int value = (int)(DelayTb.Value - DelayTb.Value.Date).TotalMilliseconds;
                if (value == 0)
                {
                    throw new Exception("Invalid delay time.");
                }
                _macro.Value = value.ToString();
                _macro.Name = NameTb.Text;
                if (string.IsNullOrEmpty(_macro.Name))
                {
                    _macro.Name = null;
                }
                _macro.Description = DescriptionTb.Text;
                if (string.IsNullOrEmpty(_macro.Description))
                {
                    _macro.Description = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }
    }
}
