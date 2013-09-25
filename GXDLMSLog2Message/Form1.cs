using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GXDLMSLog2Message
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string output = "";
            String message = null;
            bool bMessageContinue = false;
            foreach(string it in InputTB.Lines)
            {
                string line = it.Trim();
                if (bMessageContinue || line.StartsWith("7E") || line.StartsWith("7e"))
                {
                    if (line.EndsWith("7E") || line.EndsWith("7e"))
                    {
                        bMessageContinue = false;
                        line = message + " " + line;
                        output += line.Trim() + Environment.NewLine;
                        message = null;
                    }
                    else
                    {
                        bMessageContinue = true;
                        message += " " + line;
                    }
                }
            }
            OutputTB.Text = output.Replace("  ", " ");
        }
    }
}
