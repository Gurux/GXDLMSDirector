//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
// Version:         $Revision: 10764 $,
//                  $Date: 2019-06-04 11:01:51 +0300 (ti, 04 kesä 2019) $
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
using System.IO;
using System.Threading;
using System.Reflection;
using Gurux.Common;
using System.Deployment.Application;
using Microsoft.Win32;
using System.Diagnostics;
using Gurux.DLMS;

namespace GXDLMSDirector
{
    static class Program
    {
        static void OnAsyncStateChange(object sender, GXAsyncWork work, object[] parameters, AsyncState state, string text)
        {
        }

        /// <summary>
        /// Download missing medias.
        /// </summary>
        static void DownloadMedias(object sender, GXAsyncWork work, object[] parameters)
        {
            string[] list = (string[])parameters[0];
            string initDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GXDLMSDirector");
            string medias = Path.Combine(initDir, "Medias");
            foreach (string it in list)
            {
                IGXUpdater updater = null;
                string path = Path.Combine(medias, Path.GetFileName(it));
                Assembly asm = null;
                //Check is there new version from the media.
                if (File.Exists(path))
                {
                    asm = Assembly.LoadFile(path);
                    foreach (Type type in asm.GetTypes())
                    {
                        if (!type.IsAbstract && type.IsClass && typeof(IGXUpdater).IsAssignableFrom(type))
                        {
                            updater = Activator.CreateInstance(type) as IGXUpdater;
                            break;
                        }
                    }
                    if (updater != null && GXExternalMediaForm.CheckUpdates(updater, asm))
                    {
                        //TODO: Show that there are new updates.
                    }
                }
                else
                {
                    //If external media is missing.
                    GXExternalMediaForm.DownLoadMedia(it);
                }
            }
        }

        /// <summary>
        /// Download media updates.
        /// </summary>
        static void DownloadMediaUpdates(object sender, GXAsyncWork work, object[] parameters)
        {
            IGXUpdater updater = (IGXUpdater)parameters[0];
            Assembly asm = (Assembly)parameters[1];
            GXExternalMediaForm.CheckUpdates(updater, asm);
        }

        /// <summary>
        /// Show occurred errors.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ex"></param>
        static private void OnError(object sender, Exception ex)
        {
            MessageBox.Show(ex.Message, Properties.Resources.GXDLMSDirectorTxt, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static Mutex mutex = new Mutex(true, "{e78762b5-4e85-45c8-a9fa-e95786f77684}");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Allow only one instance.
            bool first;
            try
            {
                first = mutex.WaitOne(TimeSpan.Zero, true);
            }
            catch (AbandonedMutexException)
            {
                first = true;
            }
            if (first)
            {
                try
                {
                    //Update previous installed settings.
                    //If file is corrupted it's found from:
                    //%USERPROFILE%\AppData\Local\Gurux_Ltd\
                    //This might happen if Windows is not closed correctly.
                    if (Properties.Settings.Default.UpdateSettings)
                    {
                        Properties.Settings.Default.Upgrade();
                        Properties.Settings.Default.UpdateSettings = false;
                        Properties.Settings.Default.Save();
                        Gurux.DLMS.UI.GXDlmsUi.Upgrade();
                    }
#if (NET46)
                    //This is needed to make Gurux.MQTT visible.
                    try
                    {
                        new Gurux.MQTT.GXMqtt();
                    }
                    catch(Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
#endif

                    string initDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GXDLMSDirector");
                    if (!Directory.Exists(initDir))
                    {
                        Directory.CreateDirectory(initDir);
                    }
                    string updates = Path.Combine(initDir, "Updates");
                    string medias = Path.Combine(initDir, "Medias");
                    if (Directory.Exists(updates))
                    {
                        if (!Directory.Exists(medias))
                        {
                            Directory.CreateDirectory(medias);
                        }
                        DirectoryInfo di = new DirectoryInfo(updates);
                        foreach (string it in Properties.Settings.Default.ExternalMedias.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            FileInfo fi = new FileInfo(Path.Combine(updates, Path.GetFileName(it)));
                            if (fi.Exists)
                            {
                                try
                                {
                                    File.Copy(fi.FullName, Path.Combine(medias, fi.Name), true);
                                    File.Delete(fi.FullName);
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex.Message);
                                }
                            }
                        }
                    }

                    try
                    {
                        SetAddRemoveProgramsIcon();
                        Directory.SetCurrentDirectory(initDir);
                        //Load external medias.
                        List<string> missingMedias = new List<string>();
                        List<string> downloadedMedias = new List<string>();
                        foreach (string it in Properties.Settings.Default.ExternalMedias.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (GXExternalMediaForm.IsDownloaded(it))
                            {
                                FileInfo fi = new FileInfo(Path.Combine(medias, Path.GetFileName(it)));
                                if (!fi.Exists)
                                {
                                    missingMedias.Add(it);
                                }
                                else
                                {
                                    downloadedMedias.Add(it);
                                }
                            }
                            else if (File.Exists(it))
                            {
                                Assembly assembly = Assembly.LoadFile(it);
                            }
                        }
                        if (missingMedias.Count != 0)
                        {
                            //Download media again if not found.
                            GXAsyncWork checkUpdates = new GXAsyncWork(null, OnAsyncStateChange, DownloadMedias, OnError, null, new object[] { missingMedias.ToArray() });
                            checkUpdates.Start();
                        }
                        if (downloadedMedias.Count != 0)
                        {
                            //Download media again if not found.
                            GXAsyncWork checkUpdates = new GXAsyncWork(null, OnAsyncStateChange, DownloadMedias, OnError, null, new object[] { downloadedMedias.ToArray() });
                            checkUpdates.Start();
                        }
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
                mutex.ReleaseMutex();
            }
            else
            {
                foreach(Process p in Process.GetProcessesByName("GXDLMSDirector"))
                {
                    Gurux.Win32.SetForegroundWindow(p.MainWindowHandle);
                }
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
