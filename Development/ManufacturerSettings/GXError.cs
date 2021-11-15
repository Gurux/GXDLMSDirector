//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
//
// Version:         $Revision: 12719 $,
//                  $Date: 2021-11-15 15:18:25 +0200 (ma, 15 marras 2021) $
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
// This code is licensed under the GNU General Public License v2.
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace GXDLMS.Common
{
    class Error
    {
        delegate void ShowErrorEventHandler(System.Windows.Forms.IWin32Window owner, Exception Ex);

        static void OnShowError(System.Windows.Forms.IWin32Window owner, Exception Ex)
        {
            if (string.IsNullOrEmpty(Ex.HelpLink))
            {
                System.Windows.Forms.MessageBox.Show(owner, Ex.Message, "GXDLMSDirector", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.MessageBoxDefaultButton.Button1, 0);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(owner, Ex.Message, "GXDLMSDirector", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.MessageBoxDefaultButton.Button1, 0, Ex.HelpLink);
            }
        }

        static public void ShowError(System.Windows.Forms.IWin32Window owner, Exception Ex)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(Ex.ToString());
            }
            catch
            {
            }
            //Save error to the last error log.
            //Vista: C:\ProgramData
            //XP: c:\Program Files\Common Files
            string path = string.Empty;
            //XP = 5.1 & Vista = 6.0
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                path = System.IO.Path.Combine(path, ".Gurux");
            }
            else
            {
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                }
                else
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
                }
                path = System.IO.Path.Combine(path, "Gurux");
            }
            path = System.IO.Path.Combine(path, "GXDLMSDirector");
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            path = System.IO.Path.Combine(path, "LastError.log");
            try
            {
                using (System.IO.TextWriter tw = System.IO.File.CreateText(path))
                {
                    tw.Write(Ex.ToString());
                    tw.Close();
                }
            }
            catch (Exception)
            {
                //Skip all errors.
            }
            System.Windows.Forms.Control ctrl = owner as System.Windows.Forms.Control;
            if (ctrl != null && !ctrl.IsDisposed && ctrl.InvokeRequired)
            {
                ctrl.Invoke(new ShowErrorEventHandler(OnShowError), owner, Ex);
            }
            else
            {
                if (ctrl != null && ctrl.IsDisposed)
                {
                    if (Ex.HelpLink != null)
                    {
                        System.Windows.Forms.MessageBox.Show(null, Ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.MessageBoxDefaultButton.Button1, 0, Ex.HelpLink);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(null, Ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (Ex.HelpLink != null)
                    {
                        System.Windows.Forms.MessageBox.Show(owner, Ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.MessageBoxDefaultButton.Button1, 0, Ex.HelpLink);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(owner, Ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
