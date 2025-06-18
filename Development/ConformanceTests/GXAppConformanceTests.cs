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

using Gurux.Common;
using Gurux.DLMS;
using Gurux.DLMS.Conformance.Test;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace GXDLMSDirector
{
    /// <summary>
    /// This class implements HDLC Conformance tests.
    /// </summary>
    class GXAppConformanceTests
    {
        /// <summary>
        /// Return included tests.
        /// </summary>
        /// <returns></returns>
        public static string ToString(object target)
        {
            StringBuilder sb = new StringBuilder();
            List<string> list = new List<string>();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(target);
            //Are all tests run.
            bool all = true;
            foreach (PropertyDescriptor it in props)
            {
                if (it.Attributes[typeof(ConformanceTestAttribute)] != null)
                {
                    if (it.GetValue(target) is bool value)
                    {
                        if (value)
                        {
                            all = false;
                        }
                        else
                        {
                            list.Add(it.Name);
                        }
                    }
                }
            }

            if (all)
            {
                sb.Append("Running all tests.");
            }
            else if (list.Count == 0)
            {
                sb.Append("All tests are excluded.");
            }
            else
            {
                sb.Append("Running tests: ");
                foreach (string it in list)
                {
                    sb.Append(it);
                    sb.Append(", ");
                }
                sb.Length -= 2;
            }
            return sb.ToString();
        }

        /// <summary>
        /// COSEM application layer tests
        /// </summary>
        public static void CosemApplicationLayerTests(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output)
        {
            int tryCount = 1;
            if (!GXConformanceTests.Continue)
            {
                return;
            }
            if (!settings.ExcludedApplicationTests.T_APPL_IDLE_N1)
            {
                T_APPL_IDLE_N1(test, settings, dev, output, tryCount);
                if (!GXConformanceTests.Continue)
                {
                    return;
                }
            }

            if (!settings.ExcludedApplicationTests.T_APPL_OPEN_1)
            {
                T_APPL_OPEN_1(test, settings, dev, output, tryCount);
                if (!GXConformanceTests.Continue)
                {
                    return;
                }
            }

            if (!settings.ExcludedApplicationTests.T_APPL_OPEN_3)
            {
                T_APPL_OPEN_3(test, settings, dev, output, tryCount);
                if (!GXConformanceTests.Continue)
                {
                    return;
                }
            }
            
            if (!settings.ExcludedApplicationTests.Appl_04)
            {
                APPL_OPEN_4(test, settings, dev, output, tryCount);
                if (!GXConformanceTests.Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.T_APPL_OPEN_5)
            {
                T_APPL_OPEN_5(test, settings, dev, output, tryCount);
                if (!GXConformanceTests.Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.T_APPL_OPEN_6)
            {
                T_APPL_OPEN_6(test, settings, dev, output, tryCount);
                if (!GXConformanceTests.Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.T_APPL_OPEN_7)
            {
                T_APPL_OPEN_7(test, settings, dev, output, tryCount);
                if (!GXConformanceTests.Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.T_APPL_OPEN_9)
            {
                T_APPL_OPEN_9(test, settings, dev, output, tryCount);
                if (!GXConformanceTests.Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.T_APPL_OPEN_11)
            {
                T_APPL_OPEN_11(test, settings, dev, output, tryCount);
                if (!GXConformanceTests.Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.T_APPL_OPEN_12)
            {
                T_APPL_OPEN_12(test, settings, dev, output, tryCount);
                if (!GXConformanceTests.Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.T_APPL_OPEN_14)
            {
                T_APPL_OPEN_14(test, settings, dev, output, tryCount);
                if (!GXConformanceTests.Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.T_APPL_DATA_LN_N1)
            {
                T_APPL_DATA_LN_N1(test, settings, dev, output, tryCount);
                if (!GXConformanceTests.Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.T_APPL_DATA_LN_N3)
            {
                T_APPL_DATA_LN_N3(test, settings, dev, output, tryCount);
                if (!GXConformanceTests.Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.T_APPL_DATA_LN_N4)
            {
                T_APPL_DATA_LN_N4(test, settings, dev, output, tryCount);
                if (!GXConformanceTests.Continue)
                {
                    return;
                }
            }
            if (!settings.ExcludedApplicationTests.T_APPL_REL_P1)
            {
                APPL_REL_P1(test, settings, dev, output, tryCount);
                if (!GXConformanceTests.Continue)
                {
                    return;
                }
            }
        }
    
        private static bool CloseConnection(string name,
            int subTest,
            GXConformanceTest test,
            GXConformanceSettings settings,
            GXDLMSDevice dev,
            GXOutput output)
        {
            string testName = name + " " + Properties.Resources.CTTTest + ". ";
            string failedTestName = name + " " + Properties.Resources.CTTTestFailed + ". ";
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting " + testName + "\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), testName + "Disconnect request", 1, 1, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Meter returns DisconnectMode.");
                }
                else
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format(failedTestName + "DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
            }
            return passed;
        }
        /// <summary>
        /// T_APPL_IDLE_N1: Connection establishment.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_APPL_IDLE_N1.
        /// Try to read association object without establishing the connection to the meter.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_APPL_IDLE_N1(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            string testName = "T_APPL_IDLE_N1 " + Properties.Resources.CTTTest + ". ";
            string failedTestName = "T_APPL_IDLE_N1 " + Properties.Resources.CTTTestFailed + ". ";
            bool passed = CloseConnection("T_APPL_IDLE_N1", 0, test, settings, dev, output);
            GXReplyData reply = new GXReplyData();
            if (passed)
            {
                try
                {
                    byte[] data = dev.Comm.client.SNRMRequest();
                    dev.Comm.ReadDataBlock(data, testName + "SNRM", 1, tryCount, reply);
                    dev.Comm.ParseUAResponse(reply.Data);
                    reply.Clear();
                    GXDLMSObject obj;
                    if (dev.UseLogicalNameReferencing)
                    {
                        obj = new GXDLMSAssociationLogicalName();
                    }
                    else
                    {
                        obj = new GXDLMSAssociationShortName();
                    }
                    (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                    dev.Comm.ReadDataBlock(dev.Comm.Read(obj, 1), testName + "Read logical name", 1, reply);
                    reply.Clear();
                    passed = false;
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode != (int)ErrorCode.UnacceptableFrame)
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, string.Format(failedTestName + "DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                        passed = false;
                    }
                }
                catch (TimeoutException)
                {
                    //It's OK if meter doesn't reply.
                    GXConformanceTests.AddInfo(test, dev, output.Info, test + "Timeout.");
                }
                catch (GXDLMSConfirmedServiceError ex)
                {
                    if (dev.UseLogicalNameReferencing)
                    {
                        GXConformanceTests.AddInfo(test, dev, output.Warnings, string.Format(testName + "Meter returned confirmed service error and exception response was expected."));
                    }
                    else if (ex.ConfirmedServiceError != ConfirmedServiceError.InitiateError ||
                        ex.ServiceError != ServiceError.Service ||
                        ex.ServiceErrorValue != 2)
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, string.Format(failedTestName + "Confirmed service error returned {0}.", ex.Message));
                        passed = false;
                    }
                }
                catch (GXDLMSExceptionResponse ex)
                {
                    if (!dev.UseLogicalNameReferencing)
                    {
                        GXConformanceTests.AddInfo(test, dev, output.Warnings, string.Format(testName + "Meter returned Exception Response and confirmed service error was expected."));
                    }
                    else if (ex.ExceptionServiceError != ExceptionServiceError.ServiceNotSupported)
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, string.Format(failedTestName + "Exception response returned {0}.", ex.Message));
                        passed = false;
                    }
                }
                catch (Exception ex)
                {
                    passed = false;
                    GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "" + ex.Message);
                }
                (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = false;
                try
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), testName + "Disconnect request", 1, tryCount, reply);
                    if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                    {
                        dev.Comm.ParseUAResponse(reply.Data);
                    }
                    Thread.Sleep(1000);
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                    {
                        GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Meter returns DisconnectMode.");
                    }
                    else
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, string.Format(failedTestName + "DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                        passed = false;
                    }
                }
                catch (TimeoutException)
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "Timeout.");
                    passed = false;
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                GXConformanceTests.AddInfo(test, dev, output.Info, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_IDLE_N1\">T_APPL_IDLE_N1 test passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_IDLE_N1\">T_APPL_IDLE_N1 test failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_OPEN_1: Connection establishment.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_APPL_OPEN_1.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_APPL_OPEN_1(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            string testName = "T_APPL_OPEN_1 " + Properties.Resources.CTTTest + ". ";
            string failedTestName = "T_APPL_OPEN_1 " + Properties.Resources.CTTTestFailed + ". ";
            bool passed = CloseConnection("T_APPL_OPEN_1", 0, test, settings, dev, output);
            GXReplyData reply = new GXReplyData();
            if (passed)
            {
                //SubTest 1.Establish an AA using the parameters declared
                try
                {
                    reply.Clear();
                    byte[] data = dev.Comm.client.SNRMRequest();
                    dev.Comm.ReadDataBlock(data, testName + "SNRM", 1, tryCount, reply);
                    dev.Comm.ParseUAResponse(reply.Data);
                    reply.Clear();
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                    {
                        GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Meter returns DisconnectMode.");
                    }
                    else
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, string.Format(failedTestName + "DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                        passed = false;
                    }
                }
                catch (Exception ex)
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "DisconnectMode failed. " + ex.Message);
                    passed = false;
                }
                try
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), testName + "AARQ", 1, tryCount, reply);
                    dev.Comm.ParseAAREResponse(reply.Data);
                    reply.Clear();
                    if (dev.Comm.client.Authentication > Authentication.Low)
                    {
                        reply.Clear();
                        dev.Comm.ReadDataBlock(dev.Comm.client.GetApplicationAssociationRequest(), testName + "Authenticating.", 1, tryCount, reply);
                        dev.Comm.client.ParseApplicationAssociationResponse(reply.Data);
                    }
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                    {
                        GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Meter returns DisconnectMode.");
                    }
                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format(failedTestName + "Meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
                //SubTest 2.Check that the AA has been established
                if (passed)
                {
                    try
                    {
                        reply.Clear();
                        dev.Comm.ReadDataBlock(dev.Comm.client.Read("0.0.40.0.0.255", ObjectType.AssociationLogicalName, 1), testName + "AARQ", 1, reply);
                        string ln = GXDLMSConverter.ToLogicalName(reply.Value);
                        if (ln != "0.0.40.0.0.255")
                        {
                            GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + "Check Associated State: Unexpected Data value. Expected: 0.0.40.0.0.255, actual: " + ln);
                            passed = false;
                        }
                        reply.Clear();
                    }
                    catch (Exception ex)
                    {
                        passed = false;
                        GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "" + ex.Message);
                    }
                    //SubTest 3.Release the AA
                    if (passed)
                    {
                        try
                        {
                            dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), testName + "Disconnect request", 1, tryCount, reply);
                            if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                            {
                                dev.Comm.ParseUAResponse(reply.Data);
                            }
                        }
                        catch (GXDLMSException ex)
                        {
                            if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                            {
                                GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Meter returns DisconnectMode.");
                            }
                            else
                            {
                                GXConformanceTests.AddError(test, dev, output.Errors, string.Format(failedTestName + "DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                                passed = false;
                            }
                        }
                        catch (TimeoutException)
                        {
                            GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "Timeout.");
                            passed = false;
                        }
                        catch (Exception ex)
                        {
                            passed = false;
                            GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + "" + ex.Message);
                        }
                        if (passed)
                        {
                            try
                            {
                                reply.Clear();
                                byte[] data = dev.Comm.client.SNRMRequest();
                                dev.Comm.ReadDataBlock(data, testName + "SNRM", 1, tryCount, reply);
                                dev.Comm.ParseUAResponse(reply.Data);
                                reply.Clear();
                            }
                            catch (GXDLMSException ex)
                            {
                                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                                {
                                    GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Meter returns DisconnectMode.");
                                }
                                else
                                {
                                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format(failedTestName + "DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                                    passed = false;
                                }
                            }
                            catch (Exception)
                            {
                                passed = false;
                            }
                        }
                    }
                }
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                GXConformanceTests.AddInfo(test, dev, output.Info, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_OPEN_1\">T_APPL_OPEN_1 test passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_OPEN_1\">T_APPL_OPEN_1 test failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_OPEN_3: Connection establishment.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_APPL_OPEN_3.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_APPL_OPEN_3(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            string testName = "T_APPL_OPEN_3 " + Properties.Resources.CTTTest + ". ";
            string failedTestName = "T_APPL_OPEN_3 " + Properties.Resources.CTTTestFailed + ". ";
            bool passed = CloseConnection("T_APPL_OPEN_3", 0, test, settings, dev, output);
            if (dev.Authentication < Authentication.High)
            {
            }
            else
            {
                GXReplyData reply = new GXReplyData();
                if (passed)
                {
                    try
                    {
                        reply.Clear();
                        byte[] data = dev.Comm.client.SNRMRequest();
                        dev.Comm.ReadDataBlock(data, testName + "SNRM", 1, tryCount, reply);
                        dev.Comm.ParseUAResponse(reply.Data);
                        reply.Clear();
                    }
                    catch (GXDLMSException ex)
                    {
                        if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                        {
                            GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Meter returns DisconnectMode.");
                        }
                        else
                        {
                            GXConformanceTests.AddError(test, dev, output.Errors, string.Format(failedTestName + "DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                            passed = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "DisconnectMode failed. " + ex.Message);
                        passed = false;
                    }
                    try
                    {
                        reply.Clear();
                        dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), testName + "AARQ", 1, tryCount, reply);
                        dev.Comm.ParseAAREResponse(reply.Data);
                        reply.Clear();
                        if (dev.Comm.client.Authentication > Authentication.Low)
                        {
                            reply.Clear();
                            dev.Comm.ReadDataBlock(dev.Comm.client.GetApplicationAssociationRequest(), testName + "Authenticating.", 1, tryCount, reply);
                            dev.Comm.client.ParseApplicationAssociationResponse(reply.Data);
                        }
                    }
                    catch (GXDLMSException ex)
                    {
                        if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                        {
                            GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Meter returns DisconnectMode.");
                        }
                        GXConformanceTests.AddError(test, dev, output.Errors, string.Format(failedTestName + "Meter returns {0}.", (ErrorCode)ex.ErrorCode));
                        passed = false;
                    }
                }
                if (passed)
                {
                    test.OnTrace(test, "Passed.\r\n");
                    GXConformanceTests.AddInfo(test, dev, output.Info, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_OPEN_1\">T_APPL_OPEN_1 test passed.</a>");
                }
                else
                {
                    test.OnTrace(test, "Failed.\r\n");
                    GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_OPEN_1\">T_APPL_OPEN_1 test failed.</a>");
                }
            }
        }


        /// <summary>
        /// Appl_04: Connection establishment : Protocol-version
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void APPL_OPEN_4(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            string testName = "APPL_OPEN_4 " + Properties.Resources.CTTTest + ". ";
            string failedTestName = "APPL_OPEN_4 " + Properties.Resources.CTTTestFailed + ". ";
            string subTestName = "APPL_OPEN_4 " + Properties.Resources.CTTTest + ". Subtest {0}";
            bool passed = CloseConnection("T_APPL_IDLE_N1", 0, test, settings, dev, output);
            GXReplyData reply = new GXReplyData();
            //Protocol-version present and containing the default value.
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, string.Format(subTestName, 1) + "SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
                try
                {
                    dev.Comm.client.ProtocolVersion = "100001";
                    dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), string.Format(subTestName, 1) + "AARQ", 1, tryCount, reply);
                    dev.Comm.client.ProtocolVersion = null;
                    dev.Comm.ParseAAREResponse(reply.Data);
                    reply.Clear();
                    if (dev.Comm.client.ProtocolVersion != "100001")
                    {
                        throw new Exception(string.Format(Properties.Resources.CTTSubTestFailed, 1));
                    }
                }
                finally
                {
                    dev.Comm.client.ProtocolVersion = null;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, string.Format(Properties.Resources.CTTSubTestFailed, 1) + ex.Message);
            }
            if (passed)
            {
                passed = CloseConnection("APPL_OPEN_4", 1, test, settings, dev, output);
                //SubTest 2: Protocol-version is present but not containing the default value
                try
                {
                    reply.Clear();
                    byte[] data = dev.Comm.client.SNRMRequest();
                    dev.Comm.ReadDataBlock(data, string.Format(subTestName, 2) + "SNRM", 1, tryCount, reply);
                    dev.Comm.ParseUAResponse(reply.Data);
                    reply.Clear();
                    try
                    {
                        dev.Comm.client.ProtocolVersion = "010001";
                        dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), string.Format(subTestName, 2) + "AARQ", 1, tryCount, reply);
                        dev.Comm.ParseAAREResponse(reply.Data);
                        reply.Clear();
                        throw new Exception(string.Format(Properties.Resources.CTTSubTestFailed, 2));
                    }
                    finally
                    {
                        dev.Comm.client.ProtocolVersion = null;
                    }
                }
                catch (GXDLMSException ex)
                {
                    if (ex.Result == AssociationResult.PermanentRejected && ex.Diagnostic == (byte)AcseServiceProvider.NoCommonAcseVersion)
                    {
                        GXConformanceTests.AddInfo(test, dev, output.Info, string.Format(subTestName, 2) + " Invalid Protocol-version succeeded. " + ex.Message);
                    }
                    else
                    {
                        passed = false;
                        GXConformanceTests.AddError(test, dev, output.Errors, string.Format(Properties.Resources.CTTSubTestFailed, 2) + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    passed = false;
                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format(Properties.Resources.CTTSubTestFailed, 2) + ex.Message);
                }
            }
            passed = CloseConnection("APPL_OPEN_4", 1, test, settings, dev, output);
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                GXConformanceTests.AddInfo(test, dev, output.Info, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#APPL_OPEN_4\">APPL_OPEN_4 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#APPL_OPEN_4\">APPL_OPEN_4 failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_OPEN_5: Unknown application context
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_APPL_OPEN_5(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            string testName = "T_APPL_OPEN_5 " + Properties.Resources.CTTTest + ". ";
            string failedTestName = "T_APPL_OPEN_5 " + Properties.Resources.CTTTestFailed + ". ";
            string subTestName = "T_APPL_OPEN_5 " + Properties.Resources.CTTTest + ". Subtest {0}";
            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = CloseConnection("T_APPL_OPEN_5", 1, test, settings, dev, output);
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "T_APPL_OPEN_5 test. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
                GXByteBuffer bb = new GXByteBuffer();
                bb.SetHexString("601DA10906075F857504070203BE10040E01000000065F1F04007C1BA0FFFF");
                dev.Comm.ReadDataBlock(dev.Comm.client.CustomFrameRequest(Command.Aarq, bb), "T_APPL_OPEN_5 test. AARQ", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
                reply.Clear();
                throw new Exception("UNKNOWN ApplicationContextName failed.");
            }
            catch (GXDLMSException ex)
            {
                if (ex.Result == AssociationResult.PermanentRejected && ex.Diagnostic == (byte)SourceDiagnostic.ApplicationContextNameNotSupported)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "T_APPL_OPEN_5 test UNKNOWN ApplicationContextName succeeded. " + ex.Message);
                }
                else
                {
                    passed = false;
                    GXConformanceTests.AddError(test, dev, output.Errors, "T_APPL_OPEN_5 test failed. " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "T_APPL_OPEN_5 test failed. " + ex.Message);
            }
            /*
            //SubTest 1: Unused AARQ fields are present with a dummy value
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #5. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Meter returns DisconnectMode.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, "COSEM Application tests #5 failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #5. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Meter returns DisconnectMode.");
                }
                else
                {
                                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format("DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));

                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, "COSEM Application tests #5 failed. " + ex.Message);
            }
            try
            {
                GXByteBuffer bb = new GXByteBuffer();
                bb.SetHexString("6036A109060760857405080101A203040144A303040144A403020100A503020100A803020100BE10040E01000000065F1F040060FE9FFFFF");
                dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0x10, bb), "COSEM Application test #5. AARQ", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #5. Disconnect request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
            }

            //SubTest 4: AARQ.calling-AE-invocation-id present when client user identification is not supported.
            dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #5. Disconnect request", 1, tryCount, reply);
            dev.Comm.ParseUAResponse(reply.Data);
            reply.Clear();

            data = dev.Comm.client.SNRMRequest();
            dev.Comm.ReadDataBlock(data, "COSEM Application test #5. SNRM", 1, tryCount, reply);
            dev.Comm.ParseUAResponse(reply.Data);
            reply.Clear();
            try
            {
                GXByteBuffer bb = new GXByteBuffer();
                bb.SetHexString("6059A109060760857405080101A203040144A303040144A403020100A503020100A703040144A803020100A9030201018A0207808B0760857405080201AC0A80083132333435363738BE10040E01000000065F1F040060FE9FFFFF");
                dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0x10, bb), "COSEM Application test #5. AARQ", 1, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
            }
            */
            passed = CloseConnection("T_APPL_OPEN_5", 1, test, settings, dev, output);
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                GXConformanceTests.AddInfo(test, dev, output.Info, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_OPEN_5\">T_APPL_OPEN_5 test passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_OPEN_5\">T_APPL_OPEN_5 test failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_OPEN_6:
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_APPL_OPEN_6(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            string testName = "T_APPL_DATA_LN_N6 " + Properties.Resources.CTTTest + ". ";
            string failedTestName = "T_APPL_DATA_LN_N6 " + Properties.Resources.CTTTestFailed + ". ";           
            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = CloseConnection("T_APPL_OPEN_6", 1, test, settings, dev, output);
            //SubTest 1: Unused AARQ fields are present with a dummy value
            try
            {
                reply.Clear();
                if (dev.Comm.client.Ciphering.Security == Security.None)
                {
                    data = dev.Comm.client.SNRMRequest();
                    dev.Comm.ReadDataBlock(data, testName + "SNRM", 1, tryCount, reply);
                    dev.Comm.ParseUAResponse(reply.Data);
                    reply.Clear();
                    GXByteBuffer bb = new GXByteBuffer();
                    string xml = "<AssociationRequest>\n" +
                                  "<ApplicationContextName Value=\"LN\" />\n" +
                                  "<CalledAPTitle Value=\"44\" />\n" +
                                  "<CalledAEQualifier Value=\"44\" />\n" +
                                  "<CalledAPInvocationId Value=\"00\" />\n" +
                                  "<CalledAEInvocationId Value=\"00\" />\n" +
                                  "<CallingApInvocationId Value=\"00\" />\n" +
                                  "<InitiateRequest>\n" +
                                    "<ProposedDlmsVersionNumber Value=\"06\" />\n" +
                                    "<ProposedConformance>\n" +
                                      "<ConformanceBit Name=\"Action\" />\n" +
                                      "<ConformanceBit Name=\"EventNotification\" />\n" +
                                      "<ConformanceBit Name=\"SelectiveAccess\" />\n" +
                                      "<ConformanceBit Name=\"Set\" />\n" +
                                      "<ConformanceBit Name=\"Get\" />\n" +
                                      "<ConformanceBit Name=\"Access\" />\n" +
                                      "<ConformanceBit Name=\"DataNotification\" />\n" +
                                      "<ConformanceBit Name=\"MultipleReferences\" />\n" +
                                      "<ConformanceBit Name=\"BlockTransferWithAction\" />\n" +
                                      "<ConformanceBit Name=\"BlockTransferWithSetOrWrite\" />\n" +
                                      "<ConformanceBit Name=\"BlockTransferWithGetOrRead\" />\n" +
                                      "<ConformanceBit Name=\"Attribute0SupportedWithGet\" />\n" +
                                      "<ConformanceBit Name=\"PriorityMgmtSupported\" />\n" +
                                      "<ConformanceBit Name=\"Attribute0SupportedWithSet\" />\n" +
                                      "<ConformanceBit Name=\"GeneralBlockTransfer\" />\n" +
                                      "<ConformanceBit Name=\"GeneralProtection\" />\n" +
                                    "</ProposedConformance>\n" +
                                    "<ProposedMaxPduSize Value=\"FFFF\" />\n" +
                                  "</InitiateRequest>\n" +
                                "</AssociationRequest>";

                    byte[][] data2 = (dev.Comm.client as GXDLMSXmlClient).PduToMessages((dev.Comm.client as GXDLMSXmlClient).LoadXml(xml)[0]);
                    GXDLMSTranslator t = new GXDLMSTranslator(TranslatorOutputType.SimpleXml);
                    dev.Comm.ReadDataBlock(data2, testName + "AARQ", 1, tryCount, reply);
                    dev.Comm.ParseAAREResponse(reply.Data);
                    reply.Data.Position = 0;
                    string str = t.PduToXml(reply.Data);
                    if (str.Contains("RespondingAeInvocationId"))
                    {
                        throw new Exception("Responding AE Invocation ID present.");
                    }
                    if (str.Contains("RespondingAPTitle"))
                    {
                        throw new Exception("Responding AP title present.");
                    }
                    reply.Clear();
                }
            }
            catch (GXDLMSException ex)
            {
                if (ex.Result == AssociationResult.PermanentRejected && ex.Diagnostic != (byte)SourceDiagnostic.None)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, testName + "UNKNOWN ApplicationContextName succeeded. " + ex.Message);
                }
                else
                {
                    passed = false;
                    GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + ex.Message);
                }
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + ex.Message);
            }
            //SubTest 2: AARQ.calling-AP-title too short
            if (passed && (dev.Comm.client.Authentication == Authentication.HighGMAC ||
                (dev.Comm.client.Ciphering != null && dev.Comm.client.Ciphering.Security != (byte)Security.None)))
            {
                try
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), testName + "Disconnect request", 1, tryCount, reply);
                    if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                    {
                        dev.Comm.ParseUAResponse(reply.Data);
                    }
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                    {
                        GXConformanceTests.AddInfo(test, dev, output.Info, "Meter returns DisconnectMode.");
                    }
                    else
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, string.Format(failedTestName + "DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                        passed = false;
                    }
                }
                catch (Exception ex)
                {
                    passed = false;
                    GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
                }
                byte[] st = dev.Comm.client.Ciphering.SystemTitle;
                try
                {
                    reply.Clear();
                    data = dev.Comm.client.SNRMRequest();
                    dev.Comm.ReadDataBlock(data, testName + "SNRM", 1, tryCount, reply);
                    dev.Comm.ParseUAResponse(reply.Data);
                    reply.Clear();
                    dev.Comm.client.Ciphering.SystemTitle = new byte[] { 0x44 };
                    dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), testName + "AARQ", 1, tryCount, reply);
                    dev.Comm.ParseAAREResponse(reply.Data);
                    reply.Clear();
                    throw new Exception("AARQ.calling-AP-title too short.");
                }
                catch (GXDLMSException ex)
                {
                    if (ex.Result == AssociationResult.PermanentRejected && ex.Diagnostic == (byte)SourceDiagnostic.CallingApTitleNotRecognized)
                    {
                        GXConformanceTests.AddInfo(test, dev, output.Info, "T_APPL_OPEN_6 test AARQ.calling-AP-title too short succeeded. " + ex.Message);
                    }
                    else
                    {
                        passed = false;
                        GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "AARQ.calling-AP-title too short failed. " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    passed = false;
                    GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + ex.Message);
                }
                dev.Comm.client.Ciphering.SystemTitle = st;
            }
            //SubTest 3: AARQ.calling-AP-title too long
            if (passed && (dev.Comm.client.Authentication == Authentication.HighGMAC ||
                (dev.Comm.client.Ciphering != null && dev.Comm.client.Ciphering.Security != (byte)Security.None)))
            {
                try
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), testName + "Disconnect request", 1, tryCount, reply);
                    if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                    {
                        dev.Comm.ParseUAResponse(reply.Data);
                    }
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                    {
                        GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Meter returns DisconnectMode.");
                    }
                    else
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, string.Format(failedTestName + "DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                        passed = false;
                    }
                }
                catch (Exception ex)
                {
                    passed = false;
                    GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
                }
                byte[] st = dev.Comm.client.Ciphering.SystemTitle;
                try
                {
                    reply.Clear();
                    data = dev.Comm.client.SNRMRequest();
                    dev.Comm.ReadDataBlock(data, testName + "SNRM", 1, tryCount, reply);
                    dev.Comm.ParseUAResponse(reply.Data);
                    reply.Clear();
                    dev.Comm.client.Ciphering.SystemTitle = new byte[] { 0x43, 0x54, 0x54, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30 };
                    dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), testName + "AARQ", 1, tryCount, reply);
                    dev.Comm.ParseAAREResponse(reply.Data);
                    reply.Clear();
                    throw new Exception("AARQ.calling-AP-title too long.");
                }
                catch (GXDLMSException ex)
                {
                    if (ex.Result == AssociationResult.PermanentRejected && ex.Diagnostic == (byte)SourceDiagnostic.CallingApTitleNotRecognized)
                    {
                        GXConformanceTests.AddInfo(test, dev, output.Info, "T_APPL_OPEN_6 test AARQ.calling-AP-title too long succeeded. " + ex.Message);
                    }
                    else
                    {
                        passed = false;
                        GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "AARQ.calling-AP-title too long failed. " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    passed = false;
                    GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "AARQ.calling-AP-title too long failed. " + ex.Message);
                }
                dev.Comm.client.Ciphering.SystemTitle = st;
            }
            passed = CloseConnection("T_APPL_OPEN_6", 1, test, settings, dev, output);
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, testName + "SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Meter returns DisconnectMode.");
                }
                else
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format(failedTestName + "DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
            }
            if (passed)
            {
                passed = CloseConnection("T_APPL_OPEN_6", 1, test, settings, dev, output);
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                GXConformanceTests.AddInfo(test, dev, output.Info, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_OPEN_6\">T_APPL_OPEN_6 test passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_OPEN_6\">T_APPL_OPEN_6 test failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_OPEN_7:
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_APPL_OPEN_7(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            string testName = "T_APPL_OPEN_7 " + Properties.Resources.CTTTest + ". ";
            string failedTestName = "T_APPL_OPEN_7 " + Properties.Resources.CTTTestFailed + ". ";
            string subTestName = "T_APPL_OPEN_7 " + Properties.Resources.CTTTest + ". Subtest {0}";
            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = CloseConnection("T_APPL_OPEN_7", 1, test, settings, dev, output);
            //Unused AARQ fields are present with a dummy value
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, testName + "SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
                GXByteBuffer bb = new GXByteBuffer();
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    bb.SetHexString("E6E6006036A109060760857405080101A203040144A303040144A403020100A503020100A803020100BE10040E01000000065F1F040060FE9FFFFF");
                    dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0x10, bb), testName + "AARQ", 1, tryCount, reply);
                }
                else
                {
                    bb.SetHexString("6036A109060760857405080101A203040144A303040144A403020100A503020100A803020100BE10040E01000000065F1F040060FE9FFFFF");
                    dev.Comm.ReadDataBlock(dev.Comm.client.CustomFrameRequest(0, bb), testName + "AARQ", 1, tryCount, reply);
                }
                dev.Comm.ParseAAREResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + ex.Message);
            }
            //SubTest 1: Unused AARQ fields are present with a dummy value. Authentication is not used.
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), testName + "Disconnect request", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, failedTestName +  "Timeout.");
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, testName+  "Meter returns DisconnectMode.");
                }
                else
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + string.Format("DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, testName + "SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Meter returns DisconnectMode.");
                }
                else
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + string.Format("DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
            }
            if (passed)
            {
                passed = CloseConnection("T_APPL_OPEN_7", 1, test, settings, dev, output);
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                GXConformanceTests.AddInfo(test, dev, output.Info, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_OPEN_7\">T_APPL_OPEN_7 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_OPEN_7\">T_APPL_OPEN_7 failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_OPEN_9. Send dedicated key when ciphering is not used.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_APPL_OPEN_9(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = CloseConnection("T_APPL_OPEN_9", 1, test, settings, dev, output);
            Security s = dev.Comm.client.Ciphering.Security;
            try
            {
                reply.Clear();
                dev.Comm.client.Ciphering.Security = 0;
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #9. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                dev.Comm.client.Ciphering.DedicatedKey = GXCommon.HexToBytes("000102030405060708090A0B0C0D0E0F");
                dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "T_APPL_OPEN_9. AARQ", 1, tryCount, reply);
                dev.Comm.client.Ciphering.DedicatedKey = null;
                dev.Comm.ParseAAREResponse(reply.Data);
                reply.Clear();
                passed = false;
            }
            catch (Exception)
            {
                //This test should fail.
            }
            finally
            {
                dev.Comm.client.Ciphering.Security = s;
                dev.Comm.client.Ciphering.DedicatedKey = null;
            }
            CloseConnection("T_APPL_OPEN_9", 1, test, settings, dev, output);
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                GXConformanceTests.AddInfo(test, dev, output.Info, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_OPEN_9\">COSEM Application T_APPL_OPEN_9 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_OPEN_9\">COSEM Application T_APPL_OPEN_9 failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_OPEN_11. Check Quality of service.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_APPL_OPEN_11(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = CloseConnection("T_APPL_OPEN_11", 1, test, settings, dev, output);
            reply.Clear();
            try
            {
                dev.Comm.client.QualityOfService = 1;
                dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "T_APPL_OPEN_11 test. AARQ", 1, tryCount, reply);
                dev.Comm.client.QualityOfService = 0;
                dev.Comm.ParseAAREResponse(reply.Data);
                if (dev.Comm.client.QualityOfService != 1)
                {
                    passed = false;
                    GXConformanceTests.AddInfo(test, dev, output.Info, "T_APPL_OPEN_11 test failed. Quality of service field not found.");
                }
                reply.Clear();
            }
            catch (Exception)
            {
                passed = false;
            }
            dev.Comm.client.QualityOfService = 0;
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "T_APPL_OPEN_11 test. Disconnect request", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "T_APPL_OPEN_11 test failed. Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                GXConformanceTests.AddInfo(test, dev, output.Info, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_OPEN_11\">T_APPL_OPEN_11 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_OPEN_11\">T_APPL_OPEN_11 failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_OPEN_12. Check DLMS version number. Try to use version 5 and 7.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_APPL_OPEN_12(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            string testName = "T_APPL_OPEN_12 " + Properties.Resources.CTTTest + ". ";
            string failedTestName = "T_APPL_OPEN_12 " + Properties.Resources.CTTTestFailed + ". ";
            string subTestName = "T_APPL_OPEN_12 " + Properties.Resources.CTTTest + ". Subtest {0}";
            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = true;
            test.OnTrace(test, "Starting T_APPL_OPEN_12 test.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), testName + "Disconnect request", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "Timeout.");
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Meter returns DisconnectMode.");
                }
                else
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format("DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "COSEM Application test #12. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Meter returns DisconnectMode.");
                }
                else
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + string.Format("DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
            }
            reply.Clear();
            try
            {
                string xml = "<AssociationRequest>\r\n"
                        + "  <ApplicationContextName Value=\"LN\" />\r\n"
                        + "  <InitiateRequest>\r\n"
                        + "    <ProposedDlmsVersionNumber Value=\"05\" />\r\n"
                        + "    <ProposedConformance>\r\n"
                        + "      <ConformanceBit Name=\"GeneralProtection\" />\r\n"
                        + "      <ConformanceBit Name=\"GeneralBlockTransfer\" />\r\n"
                        + "      <ConformanceBit Name=\"Attribute0SupportedWithSet\" />\r\n"
                        + "      <ConformanceBit Name=\"PriorityMgmtSupported\" />\r\n"
                        + "      <ConformanceBit Name=\"Attribute0SupportedWithGet\" />\r\n"
                        + "      <ConformanceBit Name=\"BlockTransferWithGetOrRead\" />\r\n"
                        + "      <ConformanceBit Name=\"BlockTransferWithSetOrWrite\" />\r\n"
                        + "      <ConformanceBit Name=\"BlockTransferWithAction\" />\r\n"
                        + "      <ConformanceBit Name=\"MultipleReferences\" />\r\n"
                        + "      <ConformanceBit Name=\"DataNotification\" />\r\n"
                        + "      <ConformanceBit Name=\"Get\" />\r\n"
                        + "      <ConformanceBit Name=\"Set\" />\r\n"
                        + "      <ConformanceBit Name=\"SelectiveAccess\" />\r\n"
                        + "      <ConformanceBit Name=\"EventNotification\" />\r\n"
                        + "      <ConformanceBit Name=\"Action\" />\r\n"
                        + "    </ProposedConformance>\r\n"
                        + "    <ProposedMaxPduSize Value=\"FFFF\" />\r\n"
                        + "  </InitiateRequest>\r\n"
                        + "</AssociationRequest>";
                byte[][] data2 = (dev.Comm.client as GXDLMSXmlClient).PduToMessages((dev.Comm.client as GXDLMSXmlClient).LoadXml(xml)[0]);
                dev.Comm.ReadDataBlock(data2, testName + "AARQ", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
            }
            catch (GXDLMSConfirmedServiceError ex)
            {
                if (ex.ServiceError == ServiceError.Initiate && ex.ServiceErrorValue == 1)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, testName + " succeeded with DMLS version 5.");
                }
                else
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #12. Disconnect request", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, testName + "SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
            }
            reply.Clear();
            try
            {
                GXByteBuffer bb = new GXByteBuffer();
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    bb.SetHexString("E6E600601DA109060760857405080101BE10040E01000000075F1F040060FE9FFFFF");
                    dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0x10, bb), testName + "AARQ", 1, tryCount, reply);
                }
                else
                {
                    bb.SetHexString("601DA109060760857405080101BE10040E01000000075F1F040060FE9FFFFF");
                    dev.Comm.ReadDataBlock(dev.Comm.client.CustomFrameRequest(0, bb), testName + "AARQ", 1, tryCount, reply);
                }
                dev.Comm.ParseAAREResponse(reply.Data);
            }
            catch (GXDLMSConfirmedServiceError ex)
            {
                if (ex.ServiceError == ServiceError.Initiate && ex.ServiceErrorValue == 1)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, testName + "succeeded with DMLS version 7.");
                }
                else
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), testName + "Disconnect request", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                GXConformanceTests.AddInfo(test, dev, output.Info, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_OPEN_12\">T_APPL_OPEN_12 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_OPEN_12\">T_APPL_OPEN_12 failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_OPEN_14. Try to connect with invalid PDU size.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_APPL_OPEN_14(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            string testName = "T_APPL_OPEN_14 " + Properties.Resources.CTTTest + ". ";
            string failedTestName = "T_APPL_OPEN_14 " + Properties.Resources.CTTTestFailed + ". ";
            string subTestName = "T_APPL_OPEN_14 " + Properties.Resources.CTTTest + ". Subtest {0}";
            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = true;
            test.OnTrace(test, "Starting T_APPL_OPEN_14 test.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), testName + "Disconnect request", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "Timeout.");
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Meter returns DisconnectMode.");
                }
                else
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + string.Format("DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, testName + "SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
            }
            reply.Clear();
            try
            {
                GXByteBuffer bb = new GXByteBuffer();
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    bb.SetHexString("E6E600601DA109060760857405080101BE10040E01000000065F1F040060FE9F000B");
                    dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0x10, bb), testName +  "AARQ", 1, tryCount, reply);
                }
                else
                {
                    bb.SetHexString("601DA109060760857405080101BE10040E01000000065F1F040060FE9F000B");
                    dev.Comm.ReadDataBlock(dev.Comm.client.CustomFrameRequest(0, bb), testName + "AARQ", 1, tryCount, reply);
                }
                dev.Comm.ParseAAREResponse(reply.Data);
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "PDU size 11.");
            }
            catch (GXDLMSConfirmedServiceError ex)
            {
                if (ex.ServiceError == ServiceError.Initiate && ex.ServiceErrorValue == 3)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, testName + "succeeded with PDU size 11.");
                }
                else
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), testName + "Disconnect request", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, testName + "SNRM", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
            }
            reply.Clear();
            try
            {
                GXByteBuffer bb = new GXByteBuffer();
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    bb.SetHexString("E6E600601DA109060760857405080101BE10040E01000000065F1F040060FE9FFFFF");
                    dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0x10, bb), testName + "AARQ", 1, tryCount, reply);
                }
                else
                {
                    bb.SetHexString("601DA109060760857405080101BE10040E01000000065F1F040060FE9FFFFF");
                    dev.Comm.ReadDataBlock(dev.Comm.client.CustomFrameRequest(0, bb), testName + "AARQ", 1, tryCount, reply);
                }
                dev.Comm.ParseAAREResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                GXConformanceTests.AddInfo(test, dev, output.Info, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_OPEN_14\">T_APPL_OPEN_14 test passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_OPEN_14\">T_APPL_OPEN_14 test failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_DATA_LN_N1. Get-Request with unknown tag and Get-Request with missing elements.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_APPL_DATA_LN_N1(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = true;
            test.OnTrace(test, "Starting T_APPL_DATA_LN_N1 tests.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "T_APPL_DATA_LN_N1. Disconnect request", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "T_APPL_DATA_LN_N1 failed. Timeout.");
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Meter returns DisconnectMode.");
                }
                else
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format("T_APPL_DATA_LN_N1 failed. DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, "T_APPL_DATA_LN_N1 failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "T_APPL_DATA_LN_N1. SNRM", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, "T_APPL_DATA_LN_N1 failed. " + ex.Message);
            }
            reply.Clear();

            try
            {
                GXByteBuffer bb = new GXByteBuffer();
                dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "T_APPL_DATA_LN_N1. AARQRequest.", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
                reply.Clear();
                if (dev.Comm.client.Authentication > Authentication.Low)
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.client.GetApplicationAssociationRequest(), "T_APPL_DATA_LN_N1 test. Authenticating.", 1, tryCount, reply);
                    dev.Comm.client.ParseApplicationAssociationResponse(reply.Data);
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            //T_APPL_DATA_LN_N1. Get-Request with unknown tag.
            if (passed)
            {
                try
                {
                    reply.Clear();
                    GXByteBuffer bb = new GXByteBuffer();
                    (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                    if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                    {
                        bb.SetHexString("E6E600C004C1000F0000280000FF0100");
                        dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0, bb), "T_APPL_DATA_LN_N1 subtest 1. Invalid Get request.", 1, tryCount, reply);
                    }
                    else
                    {
                        bb.SetHexString("C004C1000F0000280000FF0100");
                        dev.Comm.ReadDataBlock(dev.Comm.client.CustomFrameRequest(0, bb), "T_APPL_DATA_LN_N1 subtest 1. Invalid Get request.", 1, tryCount, reply);
                    }
                    passed = false;
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.ReadWriteDenied)
                    {
                        GXConformanceTests.AddInfo(test, dev, output.Info, "T_APPL_DATA_LN_N1 subtest 1. Invalid Get request succeeded.");
                    }
                    else
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, "T_APPL_DATA_LN_N1 subtest 1 failed. " + ex.Message);
                        passed = false;
                    }
                    reply.Clear();
                    GXDLMSAssociationLogicalName a = new GXDLMSAssociationLogicalName();
                    dev.Comm.ReadDataBlock(dev.Comm.Read(a, 1), "T_APPL_DATA_LN_N1 subtest 1. Read association view logical name.", 1, reply);
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            //Subtest 2. Get-Request with missing elements
            if (passed)
            {
                try
                {
                    reply.Clear();
                    GXByteBuffer bb = new GXByteBuffer();
                    (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                    if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                    {

                        bb.SetHexString("E6E600C0C1000F0000280000FF0100");
                        //bb.SetHexString("E6E600C001C1000028000100");
                        dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0, bb), "T_APPL_DATA_LN_N1 subtest 2. Invalid Get request.", 1, tryCount, reply);
                    }
                    else
                    {
                        bb.SetHexString("C001C1000028000100");
                        dev.Comm.ReadDataBlock(dev.Comm.client.CustomFrameRequest(0, bb), "T_APPL_DATA_LN_N1 subtest 2. Invalid Get request.", 1, tryCount, reply);
                    }
                    passed = false;
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.ReadWriteDenied)
                    {
                        GXConformanceTests.AddInfo(test, dev, output.Info, "T_APPL_DATA_LN_N1 subtest 2 succeeded. Invalid Get request succeeded.");
                    }
                    else
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, "T_APPL_DATA_LN_N1 subtest 2 failed. " + ex.Message);
                        passed = false;
                    }
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            //Subtest 3: Get-Request for a non-existing object (illegal logical name)
            if (passed)
            {
                try
                {
                    reply.Clear();
                    GXByteBuffer bb = new GXByteBuffer();
                    (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                    GXDLMSAssociationLogicalName a = new GXDLMSAssociationLogicalName("255.255.255.255.255.255");
                    byte[] data2 = dev.Comm.Read(a, 1);
                    dev.Comm.ReadDataBlock(data2, "T_APPL_DATA_LN_N1 subtest 3. Read non-existing object", 1, reply);
                    passed = false;
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.ReadWriteDenied)
                    {
                        GXConformanceTests.AddInfo(test, dev, output.Info, "T_APPL_DATA_LN_N1 subtest 3 succeeded. Invalid Get request succeeded.");
                    }
                    else
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, "T_APPL_DATA_LN_N1 subtest 3 failed. " + ex.Message);
                        passed = false;
                    }
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = false;
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "T_APPL_DATA_LN_N1 subtest 2. Disconnect request", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "T_APPL_DATA_LN_N1 subtest 2 Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            //Send Read request.
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "T_APPL_DATA_LN_N1 subtest 3. SNRM", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
                dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "T_APPL_DATA_LN_N1 subtest 3. AARQRequest.", 1, tryCount, reply);
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, "T_APPL_DATA_LN_N1 subtest 3 failed. " + ex.Message);
            }
            reply.Clear();
            try
            {

                GXByteBuffer bb = new GXByteBuffer();
                (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    bb.SetHexString("E6E600050102FA00");
                    dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0x32, bb), "T_APPL_DATA_LN_N1 subtest 3. Read service", 1, tryCount, reply);
                }
                else
                {
                    bb.SetHexString("050102FA00");
                    dev.Comm.ReadDataBlock(dev.Comm.client.CustomFrameRequest(0, bb), "T_APPL_DATA_LN_N1 subtest 3. Read service", 1, tryCount, reply);
                }
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "T_APPL_DATA_LN_N1 subtest 3 failed using read service.");
            }
            catch (GXDLMSExceptionResponse ex)
            {
                if (ex.ExceptionStateError == ExceptionStateError.ServiceNotAllowed && 
                    ex.ExceptionServiceError == ExceptionServiceError.ServiceNotSupported)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "T_APPL_DATA_LN_N1 subtest 3 succeeded when Short Name referencing is used.");
                }
                else
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "T_APPL_DATA_LN_N1 subtest 3 failed for Short Name referencing. " + ex.Message);
                }
            }
            catch (Exception)
            {
                passed = false;
            }
             (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = false;
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                GXConformanceTests.AddInfo(test, dev, output.Info, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_DATA_LN_N1\">T_APPL_DATA_LN_N1 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_DATA_LN_N1\">T_APPL_DATA_LN_N1 failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_DATA_LN_N3. Set-Request with unknown tag and set-Request with missing elements.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_APPL_DATA_LN_N3(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            string testName = "T_APPL_DATA_LN_N3 " + Properties.Resources.CTTTest + ". ";
            string failedTestName = "T_APPL_DATA_LN_N3 " + Properties.Resources.CTTTestFailed + ". ";

            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = true;
            test.OnTrace(test, "Starting T_APPL_DATA_LN_N3 tests.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), testName + "Disconnect request", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "Timeout.");
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Meter returns DisconnectMode.");
                }
                else
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format(failedTestName + "DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, testName + "SNRM", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
            }
            reply.Clear();

            try
            {
                GXByteBuffer bb = new GXByteBuffer();
                dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), testName + "AARQRequest.", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
                if (dev.Comm.client.Authentication > Authentication.Low)
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.client.GetApplicationAssociationRequest(), testName + "Authenticating.", 1, tryCount, reply);
                    dev.Comm.client.ParseApplicationAssociationResponse(reply.Data);
                }
                reply.Clear();
            }
            catch (Exception)
            {
                passed = false;
            }
            //T_APPL_DATA_LN_N1. Set-Request with unknown tag.
            if (passed)
            {
                try
                {
                    reply.Clear();
                    GXByteBuffer bb = new GXByteBuffer();
                    (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                    if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                    {
                        bb.SetHexString("E6E600C106C1000F0000280000FF010009060000280000FF");
                        dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0, bb), testName + "Invalid Set request.", 1, tryCount, reply);
                    }
                    else
                    {
                        bb.SetHexString("C106C1000F0000280000FF010009060000280000FF");
                        dev.Comm.ReadDataBlock(dev.Comm.client.CustomFrameRequest(0, bb), testName + "Invalid Set request.", 1, tryCount, reply);
                    }
                    passed = false;
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.ReadWriteDenied)
                    {
                        GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Invalid Set request succeeded.");
                    }
                    else
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + ex.Message);
                        passed = false;
                    }
                }
                catch (GXDLMSExceptionResponse ex)
                {
                    if (ex.ExceptionServiceError == ExceptionServiceError.ServiceNotSupported &&
                        ex.ExceptionStateError == ExceptionStateError.ServiceNotAllowed)
                    {
                        GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Invalid Set request succeeded.");
                    }
                    else
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + ex.Message);
                        passed = false;
                    }
                }
                catch (Exception ex)
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + ex.Message);
                    passed = false;
                }
            }
            //Set-Request with missing data
            if (passed)
            {
                try
                {
                    reply.Clear();
                    GXByteBuffer bb = new GXByteBuffer();
                    (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                    if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                    {
                        bb.SetHexString("E6E600C101C1000F0000280000");
                        dev.Comm.ReadDataBlock(dev.Comm.client.CustomHdlcFrameRequest(0, bb), testName + "Invalid Set request.", 1, tryCount, reply);
                    }
                    else
                    {
                        bb.SetHexString("C101C1000F0000280000");
                        dev.Comm.ReadDataBlock(dev.Comm.client.CustomFrameRequest(0, bb), testName + "Invalid Set request.", 1, tryCount, reply);
                    }
                    passed = false;
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.ReadWriteDenied)
                    {
                        GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Invalid Set request succeeded.");
                    }
                    else
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + ex.Message);
                        passed = false;
                    }
                }
                catch (GXDLMSExceptionResponse ex)
                {
                    if (ex.ExceptionServiceError == ExceptionServiceError.ServiceNotSupported &&
                        ex.ExceptionStateError == ExceptionStateError.ServiceNotAllowed)
                    {
                        GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Invalid Set request succeeded.");
                    }
                    else
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + ex.Message);
                        passed = false;
                    }
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = false;
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), testName + "Disconnect request", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                GXConformanceTests.AddInfo(test, dev, output.Info, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_DATA_LN_N3\">T_APPL_DATA_LN_N3 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_DATA_LN_N3\">T_APPL_DATA_LN_N3 failed.</a>");
            }
        }

        /// <summary>
        /// T_APPL_DATA_LN_N4. Connect using to the meter and use unsupported service
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_APPL_DATA_LN_N4(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            string testName = "T_APPL_DATA_LN_N4 " + Properties.Resources.CTTTest + ". ";
            string failedTestName = "T_APPL_DATA_LN_N4 " + Properties.Resources.CTTTestFailed + ". ";

            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = true;
            test.OnTrace(test, "Starting T_APPL_DATA_LN_N4 tests.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), testName + "Disconnect request", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "Timeout.");
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Meter returns DisconnectMode.");
                }
                else
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format(failedTestName + "DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
            }
            try
            {
                reply.Clear();
                data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, testName + "SNRM", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
                reply.Clear();
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
            }
            reply.Clear();

            try
            {
                GXByteBuffer bb = new GXByteBuffer();
                dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), testName + "AARQRequest.", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
                if (dev.Comm.client.Authentication > Authentication.Low)
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.client.GetApplicationAssociationRequest(), testName + "Authenticating.", 1, tryCount, reply);
                    dev.Comm.client.ParseApplicationAssociationResponse(reply.Data);
                }
                reply.Clear();
            }
            catch (Exception)
            {
                passed = false;
            }
            //Try to use Unsupported service.
            bool useLN = dev.Comm.client.UseLogicalNameReferencing;
            if (passed)
            {
                try
                {
                    reply.Clear();
                    (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                    dev.Comm.client.UseLogicalNameReferencing = !useLN;
                    if (useLN)
                    {
                        GXDLMSAssociationShortName sn = new GXDLMSAssociationShortName();
                        dev.Comm.ReadDataBlock(dev.Comm.client.Read(sn, 1), testName + testName + "Read SN.", 1, tryCount, reply);
                    }
                    else
                    {
                        GXDLMSAssociationLogicalName ln = new GXDLMSAssociationLogicalName();
                        dev.Comm.ReadDataBlock(dev.Comm.client.Read(ln, 1), testName + testName + "Read LN.", 1, tryCount, reply);
                    }
                    passed = false;
                }
                catch (GXDLMSExceptionResponse)
                {
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            dev.Comm.client.UseLogicalNameReferencing = useLN;
            (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = false;
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), testName + "Disconnect request", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                GXConformanceTests.AddInfo(test, dev, output.Info, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_DATA_LN_N4\">T_APPL_DATA_LN_N4 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#T_APPL_DATA_LN_N4\">T_APPL_DATA_LN_N4 failed.</a>");
            }
        }

        /// <summary>
        /// APPL_REL_P1. Connect to the meter and graseful release the connection.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void APPL_REL_P1(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            string testName = "APPL_REL_P1 " + Properties.Resources.CTTTest + ". ";
            string failedTestName = "APPL_REL_P1 " + Properties.Resources.CTTTestFailed + ". ";

            GXReplyData reply = new GXReplyData();
            byte[] data;
            bool passed = true;
            test.OnTrace(test, "Starting APPL_REL_P1 tests.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), testName + "Disconnect request", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "Timeout.");
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, testName + "Meter returns DisconnectMode.");
                }
                else
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format(failedTestName + "DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
            }
            //Sub test 1 for HDLC framing.
            if (dev.InterfaceType == InterfaceType.HDLC || dev.InterfaceType == InterfaceType.HdlcWithModeE)
            {
                try
                {
                    reply.Clear();
                    data = dev.Comm.client.SNRMRequest();
                    dev.Comm.ReadDataBlock(data, testName + "SNRM", 1, tryCount, reply);
                    if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                    {
                        dev.Comm.ParseUAResponse(reply.Data);
                    }
                    reply.Clear();
                }
                catch (Exception ex)
                {
                    passed = false;
                    GXConformanceTests.AddInfo(test, dev, output.Info, failedTestName + ex.Message);
                }
                reply.Clear();

                try
                {
                    GXByteBuffer bb = new GXByteBuffer();
                    dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), testName + "AARQRequest.", 1, tryCount, reply);
                    dev.Comm.ParseAAREResponse(reply.Data);
                    if (dev.Comm.client.Authentication > Authentication.Low)
                    {
                        reply.Clear();
                        dev.Comm.ReadDataBlock(dev.Comm.client.GetApplicationAssociationRequest(), testName + "Authenticating.", 1, tryCount, reply);
                        dev.Comm.client.ParseApplicationAssociationResponse(reply.Data);
                    }
                    reply.Clear();
                }
                catch (Exception)
                {
                    passed = false;
                }
                //Try to use Unsupported service.
                bool useLN = dev.Comm.client.UseLogicalNameReferencing;
                if (passed)
                {
                    try
                    {
                        reply.Clear();
                        (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                        dev.Comm.client.UseLogicalNameReferencing = !useLN;
                        if (useLN)
                        {
                            GXDLMSAssociationShortName sn = new GXDLMSAssociationShortName();
                            dev.Comm.ReadDataBlock(dev.Comm.client.Read(sn, 1), testName + testName + "Read SN.", 1, tryCount, reply);
                        }
                        else
                        {
                            GXDLMSAssociationLogicalName ln = new GXDLMSAssociationLogicalName();
                            dev.Comm.ReadDataBlock(dev.Comm.client.Read(ln, 1), testName + testName + "Read LN.", 1, tryCount, reply);
                        }
                        passed = false;
                    }
                    catch (GXDLMSExceptionResponse)
                    {
                    }
                    catch (Exception)
                    {
                        passed = false;
                    }
                }
                dev.Comm.client.UseLogicalNameReferencing = useLN;
                (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = false;
                try
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), testName + "Disconnect request", 1, tryCount, reply);
                    if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                    {
                        dev.Comm.ParseUAResponse(reply.Data);
                    }
                }
                catch (TimeoutException)
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, failedTestName + "Timeout.");
                    passed = false;
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            //Sub test 2 for WRAPPER framing.
            if (passed && dev.InterfaceType == InterfaceType.WRAPPER)
            {
                (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
                try
                {
                    GXByteBuffer bb = new GXByteBuffer();
                    dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), testName + "AARQRequest.", 1, tryCount, reply);
                    dev.Comm.ParseAAREResponse(reply.Data);
                    if (dev.Comm.client.Authentication > Authentication.Low)
                    {
                        reply.Clear();
                        dev.Comm.ReadDataBlock(dev.Comm.client.GetApplicationAssociationRequest(), testName + "Authenticating.", 1, tryCount, reply);
                        dev.Comm.client.ParseApplicationAssociationResponse(reply.Data);
                    }
                    reply.Clear();
                    dev.Comm.media.Close();
                    //Wait inactivity timeout before connect again.
                    Thread.Sleep(dev.InactivityTimeout * 1000);
                    dev.Comm.media.Open();
                    //Try to read association object.
                    GXDLMSObject obj;
                    if (dev.UseLogicalNameReferencing)
                    {
                        obj = new GXDLMSAssociationLogicalName();
                    }
                    else
                    {
                        obj = new GXDLMSAssociationShortName();
                    }
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.Read(obj, 1), testName + "Read logical name of the association object.", 1, reply);
                    reply.Clear();
                    passed = false;
                }
                catch (Exception)
                {
                }
                if (passed)
                {
                    try
                    {
                        GXByteBuffer bb = new GXByteBuffer();
                        dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), testName + "AARQRequest.", 1, tryCount, reply);
                        dev.Comm.ParseAAREResponse(reply.Data);
                        if (dev.Comm.client.Authentication > Authentication.Low)
                        {
                            reply.Clear();
                            dev.Comm.ReadDataBlock(dev.Comm.client.GetApplicationAssociationRequest(), testName + "Authenticating.", 1, tryCount, reply);
                            dev.Comm.client.ParseApplicationAssociationResponse(reply.Data);
                        }
                        reply.Clear();
                        dev.Comm.ReadDataBlock(dev.Comm.client.ReleaseRequest(), testName + "ReleaseRequest.", 1, tryCount, reply);

                        //Try to read association object.
                        GXDLMSObject obj;
                        if (dev.UseLogicalNameReferencing)
                        {
                            obj = new GXDLMSAssociationLogicalName();
                        }
                        else
                        {
                            obj = new GXDLMSAssociationShortName();
                        }
                        reply.Clear();
                        dev.Comm.ReadDataBlock(dev.Comm.Read(obj, 1), testName + "Read logical name of the association object.", 1, reply);
                        reply.Clear();
                        passed = false;
                    }
                    catch (Exception)
                    {
                    }
                }
                (dev.Comm.client as GXDLMSXmlClient).ThrowExceptions = true;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
                GXConformanceTests.AddInfo(test, dev, output.Info, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#APPL_REL_P1\">APPL_REL_P1 passed.</a>");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#APPL_REL_P1\">APPL_REL_P1 failed.</a>");
            }
        }
    }
}
