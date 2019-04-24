//
// --------------------------------------------------------------------------
//  Gurux Ltd
// 
//
//
// Filename:        $HeadURL$
//
// Version:         $Revision$,
//                  $Date$
//                  $Author$
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
// More information of Gurux products: https://www.gurux.org
//
// This code is licensed under the GNU General Public License v2. 
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------
using System.Collections.Generic;
using System.IO;
using System.Web.UI;

namespace Gurux.DLMS.Conformance.Test
{
    class GXOutput
    {
        public HtmlTextWriter writer;
        public List<string> Errors = new List<string>();
        public List<string> Warnings = new List<string>();
        public List<string> PreInfo = new List<string>();
        public List<string> Info = new List<string>();
        public string file;
        public string GetName()
        {
            return file;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GXOutput(string path, string meter)
        {
            file = path;
            Stream stream = File.Open(file, FileMode.Create);
            writer = new HtmlTextWriter(new StreamWriter(stream));
            writer.WriteLine("<!DOCTYPE html >");
            writer.WriteLine("<html>");
            writer.WriteLine("<style>");
            writer.WriteLine(".tooltip {");
            writer.WriteLine("position: relative;");
            writer.WriteLine("display: inline-block;");
            writer.WriteLine("border-bottom: 1px dotted black;");
            writer.WriteLine("}");
            writer.WriteLine(".tooltip .tooltiptext {");
            writer.WriteLine("visibility: hidden;");
            writer.WriteLine("width: 600px;");
            writer.WriteLine("background-color: Gray;");
            writer.WriteLine("color: #fff;");
            writer.WriteLine("text-align: left;");
            writer.WriteLine("border-radius: 6px;");
            writer.WriteLine("padding: 5px 0;");
            /* Position the tooltip */
            writer.WriteLine("position: absolute;");
            writer.WriteLine("z-index: 1;");
            writer.WriteLine("}");

            writer.WriteLine(".tooltip:hover .tooltiptext {");
            writer.WriteLine("visibility: visible;");
            writer.WriteLine("}");
            writer.WriteLine("</style>");
            writer.WriteLine("<body>");
            writer.Write("<table width=\"100%\">");
            writer.Write("<tr>");
            writer.Write("<td><center><h1>Test Report: " + meter + "</h1></center></td>");
            writer.Write("</tr>");
            writer.Write("</table>");
        }

        public void MakeReport()
        {
            foreach (string it in PreInfo)
            {
                writer.WriteLine(it);
                writer.Write("<br/>");
            }
            // Begin Errors.
            writer.RenderBeginTag("Errors");
            writer.Write("<h2>Errors</h2>");
            if (Errors.Count == 0)
            {
                writer.Write("No errors occurred.<br/>");
            }
            else
            {
                foreach (string it in Errors)
                {
                    writer.Write(it);
                    writer.Write("<br/>");
                }
            }
            writer.RenderEndTag();
            // Begin Warnings.
            writer.RenderBeginTag("Warnings");
            writer.Write("<h2>Warnings</h2>");
            if (Warnings.Count == 0)
            {
                writer.Write("No warnings occurred.<br/>");
            }
            else
            {
                foreach (string it in Warnings)
                {
                    writer.WriteLine(it);
                    writer.Write("<br/>");
                }
            }
            writer.RenderEndTag();
            // Begin Info.
            writer.RenderBeginTag("Info");
            writer.Write("<h2>Info</h2>");
            writer.RenderEndTag();
            foreach (string it in Info)
            {
                writer.WriteLine(it);
                writer.Write("<br/>");
            }
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            PreInfo.Clear();
            Errors.Clear();
            Warnings.Clear();
            Info.Clear();
        }
    }
}
