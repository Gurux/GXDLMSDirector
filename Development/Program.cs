//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Version:         $Revision: 9689 $,
//                  $Date: 2017-11-16 11:51:42 +0200 (to, 16 marras 2017) $
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
// More information of Gurux DLMS/COSEM Director: http://www.gurux.org/GXDLMSDirector
//
// This code is licensed under the GNU General Public License v2.
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GXDLMS.ManufacturerSettings;
using System.IO;
using System.Threading;
using System.Reflection;
using Gurux.Common;
using System.Deployment.Application;
using Microsoft.Win32;

namespace GXDLMSDirector
{
    static class Program
    {
        /// <summary>
        /// Load medias from the directory.
        /// </summary>
        /// <param name="di">Directory info.</param>
        static void LoadMedias(DirectoryInfo di)
        {
            if (di.Exists)
            {
                foreach (FileInfo file in di.GetFiles("*.dll"))
                {
                    try
                    {
                        if (string.Compare(file.Name, "Gurux.Common.dll", true) == 0)
                        {
                            continue;
                        }
                        Assembly assembly = Assembly.LoadFile(file.FullName);
                        foreach (Type type in assembly.GetTypes())
                        {
                            if (!type.IsAbstract && type.IsClass && typeof(IGXMedia).IsAssignableFrom(type))
                            {
                                assembly.CreateInstance(type.ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }
            }
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                string initDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GXDLMSDirector");
                try
                {
                    if (!Directory.Exists(initDir))
                    {
                        Directory.CreateDirectory(initDir);
                    }
                    SetAddRemoveProgramsIcon();
                    Directory.SetCurrentDirectory(initDir);
                    LoadMedias(new DirectoryInfo(Path.Combine(initDir, "Medias")));
                    LoadMedias(new DirectoryInfo("Medias"));
                }
                catch (Exception)
                {
                }
                MainForm.InitMain();
            }
            catch (Exception Ex)
            {
                GXDLMS.Common.Error.ShowError(null, Ex);
            }
        }

        /// <summary>
        /// Set the icon in add/remove programs.
        /// </summary>
        private static void SetAddRemoveProgramsIcon()
        {
            // only run if clickonce deployed, on first run only
            if (!System.Diagnostics.Debugger.IsAttached && ApplicationDeployment.IsNetworkDeployed
            && ApplicationDeployment.CurrentDeployment.IsFirstRun)
            {
                try
                {
                    string icon = string.Format("{0},0", System.Reflection.Assembly.GetExecutingAssembly().Location);
                    RegistryKey myUninstallKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");
                    string[] mySubKeyNames = myUninstallKey.GetSubKeyNames();
                    for (int i = 0; i < mySubKeyNames.Length; i++)
                    {
                        RegistryKey myKey = myUninstallKey.OpenSubKey(mySubKeyNames[i], true);
                        object myValue = myKey.GetValue("DisplayName");
                        if (myValue != null && myValue.ToString() == "GXDLMSDirector")
                        {
                            myKey.SetValue("DisplayIcon", icon);
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
