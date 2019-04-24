//
// --------------------------------------------------------------------------
//  Gurux Ltd
//
//
//
//
// Version:         $Revision: 9442 $,
//                  $Date: 2017-05-23 15:21:03 +0300 (ti, 23 touko 2017) $
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
// More information of Gurux DLMS/COSEM Director: https://www.gurux.org/GXDLMSDirector
//
// This code is licensed under the GNU General Public License v2.
// Full text may be retrieved at http://www.gnu.org/licenses/gpl-2.0.txt
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GXDLMSDirector
{
    public partial class GXDurations : Form
    {
        public GXDurations(List<KeyValuePair<string, string>> values)
        {
            InitializeComponent();
            chart1.GetToolTipText += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs>(GetToolTipText);
            chart1.Series.Clear();
            int result;
            DateTime dt;
            foreach (KeyValuePair<string, string> it in values)
            {
                Series s = new Series(it.Key);
                s.ChartType = SeriesChartType.Line;
                s.XValueType = ChartValueType.DateTime;
                //  s.ToolTip = "#VALX: #VALY";
                foreach (string l in File.ReadAllLines(it.Value))
                {
                    string[] tmp = l.Split(';');
                    if (tmp.Length == 2)
                    {
                        if (int.TryParse(tmp[1], out result))
                        {
                            try
                            {
                                dt = DateTime.Parse(tmp[0], CultureInfo.InvariantCulture);
                                //                            s.Points.AddY(result);
                                s.Points.AddXY(dt, result);
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                    else if (tmp.Length == 1)
                    {
                        s.XValueType = ChartValueType.Auto;
                        if (int.TryParse(l, out result))
                        {
                            s.Points.AddY(result);
                        }
                    }
                }
                if (s.Points.Count != 0)
                {
                    chart1.Series.Add(s);
                }
            }
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss";
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.MouseWheel += chart1_MouseWheel;
        }

        private void GetToolTipText(object sender, ToolTipEventArgs e)
        {

            // Check selevted chart element and set tooltip text
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = e.HitTestResult.PointIndex;
                DataPoint dp = e.HitTestResult.Series.Points[i];
                DateTime dt = DateTime.FromOADate(dp.XValue);
                e.Text = dt.ToShortDateString() + " " + dt.ToLongTimeString() + ": " + dp.YValues[0] + " ms.";
            }
        }

        private void chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var xAxis = chart.ChartAreas[0].AxisX;
            var yAxis = chart.ChartAreas[0].AxisY;

            try
            {
                if (e.Delta < 0) // Scrolled down.
                {
                    xAxis.ScaleView.ZoomReset();
                    yAxis.ScaleView.ZoomReset();
                }
                else if (e.Delta > 0) // Scrolled up.
                {
                    var xMin = xAxis.ScaleView.ViewMinimum;
                    var xMax = xAxis.ScaleView.ViewMaximum;
                    var yMin = yAxis.ScaleView.ViewMinimum;
                    var yMax = yAxis.ScaleView.ViewMaximum;

                    var posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 2;
                    var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 2;
                    var posYStart = yAxis.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / 2;
                    var posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / 2;

                    xAxis.ScaleView.Zoom(posXStart, posXFinish);
                    yAxis.ScaleView.Zoom(posYStart, posYFinish);
                }
            }
            catch
            {
            }
        }

        private void OKBtn_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
