//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Filename:        $HeadURL: svn://mars/Projects/GuruxClub/GXDLMSDirector/Development/Program.cs $
//
// Version:         $Revision: 8937 $,
//                  $Date: 2016-11-23 14:03:11 +0200 (ke, 23 marras 2016) $
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
