//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL: svn://utopia/projects/GXDirector/Development/MRUManager.cs $
//
// Version:         $Revision: 871 $,
//                  $Date: 2009-09-29 17:22:31 +0300 (ti, 29 syys 2009) $
//                  $Author: airija $
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

using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace MRUSample
{
    delegate void OpenMRUFileEventHandler(string fileName);

	/// <summary>
	/// MRU manager - manages Most Recently Used Files list.
	/// </summary>
	internal class MRUManager
	{
        /// <summary>
        /// Recent Files menu item
        /// </summary>
        ToolStripMenuItem ToolStripMruMenu;
        /// <summary>
        /// Recent Files menu item
        /// </summary>
        private MenuItem menuItemMRU;               
		
        private const int maxNumberOfFiles = 10;    // maximum number of files in MRU list
		private const int maxDisplayLength = 40;    // maximum length of file name for display
		private string currentDirectory;            // current directory
		private List<string> MruItems;               // MRU list (file names)

        public event OpenMRUFileEventHandler OnOpenMRUFile;

		/// <summary>
		/// Constructor.
		/// </summary>
        /// <param name="mruItem">Recent Files menu item</param>
        public MRUManager(MenuItem mruItem)
		{
            MruItems = new List<string>();
            // keep reference to MRU menu item
            menuItemMRU = mruItem;
            MenuItem menuItemParent = (MenuItem)menuItemMRU.Parent;
            menuItemParent.Popup += new EventHandler(this.OnMRUShowItems);
            // keep current directory in the time of initialization
            currentDirectory = Directory.GetCurrentDirectory();
		}		
		
		/// <summary>
		/// Constructor.
		/// </summary>
        /// <param name="mruItem">Recent Files menu item</param>
        public MRUManager(ToolStripMenuItem mruItem)
		{
            MruItems = new List<string>();
            // keep reference to MRU Tool strip menu item.
            ToolStripMruMenu = mruItem;
            ToolStripMenuItem parent = mruItem.OwnerItem as ToolStripMenuItem;
            parent.DropDownOpening += new System.EventHandler(this.OnMRUShowItems);
            // keep current directory in the time of initialization
            currentDirectory = Directory.GetCurrentDirectory();
		}
       
		/// <summary>
		/// Add file name to MRU list.
		/// </summary>
        /// <remarks>
        /// If file already exists in the list, it is moved to the first place.
        /// </remarks>
		/// <param name="file">File Name</param>
		public void Insert(int index, string file)
		{
			Remove(file);
			// if array has maximum length, remove last element
            if (MruItems.Count == maxNumberOfFiles)
            {
                MruItems.RemoveAt(maxNumberOfFiles - 1);
            }
            if (index == -1)
            {
                index = MruItems.Count;
            }
			// add new file name to the start of array
            MruItems.Insert(index, file);
		}

		/// <summary>
		/// Remove file name from MRU list.
		/// </summary>
		/// <param name="file">File Name</param>
		public void Remove(string file)
		{
            if (MruItems.Contains(file))
            {
                MruItems.Remove(file);
            }
		}

        /// <summary>
        /// Returs MRU file names collection.
        /// </summary>
        /// <returns></returns>
        public string[] GetNames()
        {
            return MruItems.ToArray();
        }

		/// <summary>
		/// Update MRU list when MRU menu item parent is opened
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMRUShowItems(object sender, EventArgs e)
		{
			// remove all childs
            if (ToolStripMruMenu != null)
            {
                ToolStripMruMenu.DropDownItems.Clear();
                // Disable menu item if MRU list is empty
                ToolStripMruMenu.Enabled = MruItems.Count != 0;
                if (ToolStripMruMenu.Enabled)
                {
                    ToolStripMenuItem item;
                    foreach (string it in MruItems)
                    {
                        item = new ToolStripMenuItem(GetDisplayName(it));
                        // subscribe to item's Click event
                        item.Click += new EventHandler(this.OnMRUClicked);
                        ToolStripMruMenu.DropDownItems.Add(item);
                    }
                }
            }
            else if (menuItemMRU != null)
            {
                menuItemMRU.MenuItems.Clear();
                // Disable menu item if MRU list is empty
                menuItemMRU.Enabled = MruItems.Count != 0;
                if (menuItemMRU.Enabled)
                {
                    MenuItem item;
                    foreach (string it in MruItems)
                    {
                        item = new MenuItem(GetDisplayName(it));
                        // subscribe to item's Click event
                        item.Click += new EventHandler(this.OnMRUClicked);
                        menuItemMRU.MenuItems.Add(item);
                    }
                }
            }
		}

		/// <summary>
		/// MRU menu item is clicked - call owner's OpenMRUFile function
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnMRUClicked(object sender, EventArgs e)
		{
			// cast sender object to MenuItem
			MenuItem item = sender as MenuItem;
            ToolStripMenuItem item2 = sender as ToolStripMenuItem;
            if (item != null)
            {
                if (OnOpenMRUFile != null)
                {
                    OnOpenMRUFile(MruItems[item.Index]);
                }
            }
            else
            {
                if (OnOpenMRUFile != null)
                {                        
                    OnOpenMRUFile(MruItems[ToolStripMruMenu.DropDownItems.IndexOf(item2)]);
                }
            }            
		}
		
		/// <summary>
		/// Get display file name from full name.
		/// </summary>
		/// <param name="fullName">Full file name</param>
		/// <returns>Short display name</returns>
		private string GetDisplayName(string filePath)
		{
			// if file is in current directory, show only file name
			FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.DirectoryName == currentDirectory)
            {
                return GetShortDisplayName(fileInfo.Name);
            }
			return GetShortDisplayName(filePath);
		}

		/// <summary>
		/// Truncate a path to fit within a certain number of characters 
		/// by replacing path components with ellipses.		
		/// </summary>
        /// <param name="fullPath">Full path to the file.</param>
		/// <returns>Truncated file name</returns>
		private string GetShortDisplayName(string filePath)
		{
			string name = "";		
			if (!string.IsNullOrEmpty(filePath))
			{				
				string[] dirName = filePath.Split(Path.DirectorySeparatorChar);
				string fileName = Path.GetFileName(filePath);
                if (fileName.Length > maxDisplayLength)
				{
					fileName = fileName.Substring(0, 15) + "..." + fileName.Substring(fileName.Length - 5, 5);
				}				
				name = Path.GetPathRoot(filePath) + (dirName.Length > 2 ? dirName[1] + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar : "") + fileName;
			}			
			return name;
		}
	}
}