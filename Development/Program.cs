//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Version:         $Revision: 9607 $,
//                  $Date: 2017-10-13 14:51:16 +0300 (pe, 13 loka 2017) $
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
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                try
                {
                    string initDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GXDLMSDirector");
                    if (!Directory.Exists(initDir))
                    {
                        Directory.CreateDirectory(initDir);
                    }
                    SetAddRemoveProgramsIcon();
                    DirectoryInfo di = new DirectoryInfo("Medias");
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
                                        IGXMedia m = assembly.CreateInstance(type.ToString()) as IGXMedia;
                                    }
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine(ex.Message);
                            }
                        }
                    }
                    Directory.SetCurrentDirectory(initDir);
                }
                catch (Exception)
                {
                }
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                AppDomain.CurrentDomain.TypeResolve += new ResolveEventHandler(CurrentDomain_TypeResolve);
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

        /// <summary>
        /// Resolve Add-In's assemblies. This must add or Director don't work correctly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string[] typeinfo = args.Name.Split(',');
            foreach (Assembly it in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (it.GetName().Name.Equals(typeinfo[0]) ||
                        it.GetName().FullName.Equals(typeinfo[0]))
                {
                    return it;
                }
            }
            string[] tmp = args.Name.Split(',');
            string path = "";
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                path = Path.Combine("/usr", "lib");
                path = Path.Combine(path, tmp[0].ToLower().Replace(".", "-"));
                if (Directory.Exists(path))
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    foreach (FileInfo fi in di.GetFiles(tmp[0] + ".dll"))
                    {
                        System.Diagnostics.Trace.WriteLine("CurrentDomain_AssemblyResolve: Returning assembly from(3):" + fi.FullName);
                        Assembly assembly = Assembly.LoadFile(fi.FullName);
                        return assembly;
                    }
                }
            }
            return null;
        }

        private static Assembly CurrentDomain_TypeResolve(object sender, ResolveEventArgs args)
        {
            string ns = "";
            int pos = args.Name.LastIndexOf('.');
            if (pos != -1)
            {
                ns = args.Name.Substring(0, pos);
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assembly.GetName().Name == ns)
                    {
                        if (assembly.GetType(args.Name, false, true) != null)
                        {
                            return assembly;
                        }
                    }
                }
            }
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Name == args.Name ||
                            type.FullName == args.Name)
                    {
                        return assembly;
                    }
                }
            }
            try
            {
                Assembly asm = Assembly.LoadFrom(ns + ".dll");
                return asm;
            }
            catch
            {
                //Ignore error.
            }
            return null;
        }
    }
}
