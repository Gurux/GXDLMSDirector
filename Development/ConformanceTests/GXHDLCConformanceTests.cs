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

using Gurux.DLMS;
using Gurux.DLMS.Conformance.Test;
using Gurux.DLMS.Enums;
using Gurux.DLMS.Objects;
using System;
using System.Threading;

namespace GXDLMSDirector
{
    /// <summary>
    /// This class implements HDLC Conformance tests.
    /// </summary>
    class GXHDLCConformanceTests
    {
        public static void HdlcTests(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            if (dev.InterfaceType != InterfaceType.HDLC &&
                dev.InterfaceType != InterfaceType.HdlcWithModeE)
            {
                return;
            }
            if (!GXConformanceTests.Continue)
            {
                return;
            }
            int resendCount = dev.ResendCount;
            try
            {
                //HDLC tests are not re-send. It will broke some of the tests.
                dev.ResendCount = 1;
                if (!settings.ExcludedHdlcTests.ExcludeTest1)
                {
                    T_HDLC_FRAME_P1(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest2)
                {
                    T_HDLC_FRAME_P2(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }

                if (!settings.ExcludedHdlcTests.ExcludeTest3)
                {
                    T_HDLC_FRAME_P3(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest4)
                {
                    T_HDLC_FRAME_N1(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }

                if (!settings.ExcludedHdlcTests.ExcludeTest5)
                {
                    T_HDLC_FRAME_N2(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest6)
                {
                    T_HDLC_FRAME_N3(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest7)
                {
                    T_HDLC_FRAME_N4(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest8)
                {
                    T_HDLC_FRAME_N5(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest9)
                {
                    T_HDLC_FRAME_N7(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest10)
                {
                    T_HDLC_FRAME_N8(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest11)
                {
                    T_HDLC_ADDRESS_N7(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest12)
                {
                    T_HDLC_NDM2NRM_P1(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest13)
                {
                    T_HDLC_NDM2NRM_P2(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest14)
                {

                    T_HDLC_INFO_P1(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest15)
                {

                    Test15(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest16)
                {

                    T_HDLC_INFO_N1(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest17)
                {

                    T_HDLC_INFO_N2(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest18)
                {

                    T_HDLC_INFO_N3(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest19)
                {
                    T_HDLC_NDMOP_N1(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest20)
                {
                    T_HDLC_ADDRESS_N1(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest21)
                {
                    T_HDLC_ADDRESS_N4(test, settings, dev, output, tryCount);
                    if (!GXConformanceTests.Continue)
                    {
                        return;
                    }
                }
                if (!settings.ExcludedHdlcTests.ExcludeTest101)
                {
                    Test101(test, settings, dev, output, tryCount);
                }
            }
            finally
            {
                dev.ResendCount = resendCount;
            }
        }

        /// <summary>
        /// Send Disc, SNRM and Receiver ready to check that meter is in Normal Response Mode.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_P1
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_FRAME_P1(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #1.\r\n");
            try
            {
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #1. Disconnect request", 1, tryCount, reply);
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
                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format("DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, ex.Message);
                passed = false;
            }
            //SubTest 1: Move the IUT to NRM
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.SNRMRequest(), "HDLC test #1. SNRM request", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
                GXConformanceTests.AddInfo(test, dev, output.Info, "SNRM request succeeded. MaxInfoLengthTransmit: " + dev.Comm.client.HdlcSettings.MaxInfoTX +
                    " MaxInfoLengthReceive: " + dev.Comm.client.HdlcSettings.MaxInfoRX + " WindowSizeTransmit: " +
                    dev.Comm.client.HdlcSettings.WindowSizeTX + " WindowSizeReceive: " + dev.Comm.client.HdlcSettings.WindowSizeRX);
            }
            catch (Exception)
            {
                passed = false;
            }
            //SubTest 2: Check that the IUT is in NRM
            //Check that meter is Normal Response Mode.
            if (passed)
            {
                try
                {
                    reply.Clear();
                    dev.Comm.client.HdlcSettings.SenderFrame = 0x10;
                    dev.Comm.ReadDataBlock(dev.Comm.client.ReceiverReady(RequestTypes.None), "HDLC test #1. Receiver Ready ", 1, tryCount, reply);
                    if ((reply.FrameId & 0xF) != 1)
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc1\">Test #1 failed</a>. Send sequence number is not 0");
                        passed = false;
                    }
                    if ((reply.FrameId & 0x10) != 0x10)
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc1\">Test #1 failed</a>. P/F is 0");
                        passed = false;
                    }
                    if ((reply.FrameId & 0xF0) != 0x10)
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc1\">Test #1 failed</a>. Receive sequence number is not 0");
                        passed = false;
                    }
                }
                catch (Exception)
                {
                    passed = false;
                    GXConformanceTests.AddError(test, dev, output.Errors, "Receiver Ready failed.");
                }
            }
            //SubTest 3: Move the IUT to NDM
            //Send disc.
            if (passed)
            {
                try
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #1. Disconnect request", 1, tryCount, reply);
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
                    passed = false;
                }
                catch (TimeoutException)
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                    passed = false;
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            //SubTest 4: Check that the IUT is in NDM
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #1. Disconnect request", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
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
                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format("DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc1\">Test #1 failed.</a>");
            }
        }

        /// <summary>
        /// Send SNRM and SNRM again where one byte from CRC is removed. Then check that meter is in Normal Response Mode.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_P2
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_FRAME_P2(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #2.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.SNRMRequest(), "HDLC test #2. SNRM request", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
                GXConformanceTests.AddInfo(test, dev, output.Info, "SNRM request succeeded. MaxInfoLengthTransmit: " + dev.Comm.client.HdlcSettings.MaxInfoTX +
                    " MaxInfoLengthReceive: " + dev.Comm.client.HdlcSettings.MaxInfoRX + " WindowSizeTransmit: " +
                    dev.Comm.client.HdlcSettings.WindowSizeTX + " WindowSizeReceive: " + dev.Comm.client.HdlcSettings.WindowSizeRX);
            }
            catch (Exception ex)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "SNRM request failed. " + ex.Message);
                passed = false;
            }
            try
            {
                reply.Clear();
                GXByteBuffer bb = new GXByteBuffer(dev.Comm.DisconnectRequest(true));
                --bb.Size;
                dev.Comm.ReadDataBlock(bb.Array(), "HDLC test #2. Illegal frame.", 1, tryCount, reply);
                GXConformanceTests.AddInfo(test, dev, output.Info, "Illegal frame failed.");
                passed = false;
            }
            catch (TimeoutException)
            {
                passed = true;
            }
            catch (Exception)
            {
                GXConformanceTests.AddInfo(test, dev, output.Info, "Illegal frame succeeded.");
            }
            GXDLMSObjectCollection objs = dev.Comm.client.Objects.GetObjects(ObjectType.IecHdlcSetup);
            GXDLMSHdlcSetup s = (GXDLMSHdlcSetup)objs[0];
            Thread.Sleep(s.InactivityTimeout * 1000);
            //Check that meter is Normal Response Mode.
            try
            {
                reply.Clear();
                dev.Comm.client.HdlcSettings.SenderFrame = 0x10;
                dev.Comm.ReadDataBlock(dev.Comm.client.ReceiverReady(RequestTypes.None), "HDLC test #2. Receiver Ready ", 1, tryCount, reply);
                if (reply.FrameId != 0x11)
                {
                    passed = false;
                    GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc2\">Test #2 failed</a>. Response is not RR.");
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #2. Disconnect request", 1, tryCount, reply);
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
                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format("DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc2\">Test #2 failed.</a>");
            }
        }

        /// <summary>
        /// Send SNRM and then wait to check that inactivity timeout is working.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_P3
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_FRAME_P3(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            test.OnTrace(test, "Starting HDLC tests #3.\r\n");
            GXDLMSObjectCollection objs = dev.Comm.client.Objects.GetObjects(ObjectType.IecHdlcSetup);
            GXReplyData reply = new GXReplyData();
            if (objs.Count == 0)
            {
                output.PreInfo.Add("Inactivity timeout is not tested.");
                test.OnTrace(test, "Ignored.\r\n");
            }
            else
            {
                bool passed = true;
                try
                {
                    dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #1. Disconnect request", 1, tryCount, reply);
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
                        GXConformanceTests.AddError(test, dev, output.Errors, string.Format("DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                        passed = false;
                    }
                }
                catch (TimeoutException)
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                    passed = false;
                }
                catch (Exception)
                {
                    passed = false;
                }
                //SubTest 1: Move the IUT to NRM
                try
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.SNRMRequest(), "HDLC test #1. SNRM request", 1, tryCount, reply);
                    dev.Comm.ParseUAResponse(reply.Data);
                    GXConformanceTests.AddInfo(test, dev, output.Info, "SNRM request succeeded. MaxInfoLengthTransmit: " + dev.Comm.client.HdlcSettings.MaxInfoTX +
                        " MaxInfoLengthReceive: " + dev.Comm.client.HdlcSettings.MaxInfoRX + " WindowSizeTransmit: " +
                        dev.Comm.client.HdlcSettings.WindowSizeTX + " WindowSizeReceive: " + dev.Comm.client.HdlcSettings.WindowSizeRX);
                    dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "HDLC test #14. AARQRequest.", 1, tryCount, reply);
                    dev.Comm.client.ParseAAREResponse(reply.Data);
                }
                catch (Exception)
                {
                    passed = false;
                }
                GXDLMSHdlcSetup s = (GXDLMSHdlcSetup)objs[0];
                if (passed)
                {
                    try
                    {
                        dev.Comm.ReadValue(s, 8);
                    }
                    catch (Exception ex)
                    {
                        GXConformanceTests.AddError(test, dev, output.Errors, "Failed to read inactivity timeout value." + ex.Message);
                    }
                    output.PreInfo.Add("HDLC Setup default inactivity timeout value is " + s.InactivityTimeout + " seconds.");
                }
                if (s.InactivityTimeout == 0)
                {
                    test.OnTrace(test, "Ignored.\r\n");
                }
                else
                {
                    if (passed)
                    {
                        try
                        {
                            reply.Clear();
                            dev.Comm.ReadDataBlock(dev.Comm.SNRMRequest(), "HDLC test #3. SNRM request", 1, tryCount, reply);
                            dev.Comm.ParseUAResponse(reply.Data);
                            GXConformanceTests.AddInfo(test, dev, output.Info, "Disconect SNRM request succeeded. MaxInfoLengthTransmit: " + dev.Comm.client.HdlcSettings.MaxInfoTX +
                                " MaxInfoLengthReceive: " + dev.Comm.client.HdlcSettings.MaxInfoRX + " WindowSizeTransmit: " +
                                dev.Comm.client.HdlcSettings.WindowSizeTX + " WindowSizeReceive: " + dev.Comm.client.HdlcSettings.WindowSizeRX);
                        }
                        catch (Exception ex)
                        {
                            GXConformanceTests.AddError(test, dev, output.Errors, "SNRM request failed. " + ex.Message);
                            passed = false;
                        }
                        test.OnTrace(test, "Testing inactivity timeout and sleeping for " + s.InactivityTimeout + " seconds.\r\n");
                        Thread.Sleep((int)Math.Floor(s.InactivityTimeout * 1000 * 1.1));
                        try
                        {
                            reply.Clear();
                            dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #3. Disconnect request", 1, tryCount, reply);
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
                                GXConformanceTests.AddError(test, dev, output.Errors, string.Format("DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                                passed = false;
                            }
                        }
                        catch (TimeoutException)
                        {
                            GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                            passed = false;
                        }
                        catch (Exception ex)
                        {
                            GXConformanceTests.AddError(test, dev, output.Errors, "SNRM request failed. " + ex.Message);
                            passed = false;
                        }
                    }
                    if (passed)
                    {
                        test.OnTrace(test, "Passed.\r\n");
                    }
                    else
                    {
                        test.OnTrace(test, "Failed.\r\n");
                        GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc3\">Test #3 failed.</a>");
                    }
                }
            }
        }

        /// <summary>
        /// Send Disc and then send invalid frames and check that meter is in Normal Disconnected Mode.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_N1
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_FRAME_N1(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #4.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC Test #4. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "HDLC Test #4. Meter returns DisconnectMode.");
                }
                else if (ex.ErrorCode == (int)ErrorCode.Rejected)
                {
                    passed = false;
                    GXConformanceTests.AddInfo(test, dev, output.Info, "HDLC Test #4. Meter returns Rejected.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "HDLC Test #4. DisconnectRequest failed. " + ex.Message);
            }

            //Opening flag missing
            test.OnTrace(test, "HDLC Illecal frame test (Opening flag missing).\r\n");
            try
            {
                reply.Clear();
                GXByteBuffer bb = new GXByteBuffer(dev.Comm.SNRMRequest());
                bb.Move(1, 0, bb.Size - 1);
                dev.Comm.ReadDataBlock(bb.Array(), "HDLC test #4. Opening flag missing.", 1, tryCount, reply);
                GXConformanceTests.AddError(test, dev, output.Errors, "HDLC Test #4. Opening flag missing failed.");
                passed = false;
            }
            catch (Exception)
            {
                GXConformanceTests.AddInfo(test, dev, output.Info, "HDLC Test #4. Opening flag missing succeeded.");
            }

            //Closing flag missing.
            test.OnTrace(test, "HDLC Illecal frame test (Closing flag missing).\r\n");
            try
            {
                reply.Clear();
                GXByteBuffer bb = new GXByteBuffer(dev.Comm.SNRMRequest());
                --bb.Size;
                dev.Comm.ReadDataBlock(bb.Array(), "HDLC test #4. Closing flag missing.", 1, tryCount, reply);
                GXConformanceTests.AddError(test, dev, output.Errors, "HDLC Test #4. Closing flag missing failed.");
                passed = false;
            }
            catch (Exception)
            {
                GXConformanceTests.AddInfo(test, dev, output.Info, "HDLC Test #4. Closing flag missing succeeded.");
            }

            //Both flags are missing.
            test.OnTrace(test, "HDLC Test #4. HDLC Illecal frame test (Both flags are missing).\r\n");
            try
            {
                reply.Clear();
                GXByteBuffer bb = new GXByteBuffer(dev.Comm.SNRMRequest());
                bb.Move(1, 0, bb.Size - 1);
                --bb.Size;
                dev.Comm.ReadDataBlock(bb.Array(), "HDLC test #4. Both flags missing.", 1, tryCount, reply);
                GXConformanceTests.AddError(test, dev, output.Errors, "HDLC Test #4. Both flags missing failed.");
                passed = false;
            }
            catch (Exception)
            {
                GXConformanceTests.AddInfo(test, dev, output.Info, "HDLC Test #4. Both flags missing succeeded.");
            }
            //Check that the IUT is in NDM.
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #4. Disconnect request", 1, tryCount, reply);
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "HDLC Test #4. Meter returns DisconnectMode.");
                }
                else if (ex.ErrorCode == (int)ErrorCode.Rejected)
                {
                    passed = false;
                    GXConformanceTests.AddInfo(test, dev, output.Info, "HDLC Test #4. Meter returns Rejected.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "HDLC Test #4. Disconnect request failed.");
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc4\">Test #4 failed.</a>");
            }
        }

        /// <summary>
        /// Send Disc and then send invalid frames and SNRM and wait UA.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_N2
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_FRAME_N2(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #5.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #5. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Meter returns DisconnectMode.");
                }
                else if (ex.ErrorCode == (int)ErrorCode.Rejected)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Meter rejects Disconnect Request.");
                }
                else
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format("DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "DisconnectRequest failed. " + ex.Message);
            }

            //Remove content one byte at the time.
            UInt16 rx = dev.Comm.client.HdlcSettings.MaxInfoRX, tx = dev.Comm.client.HdlcSettings.MaxInfoTX;
            dev.Comm.client.HdlcSettings.MaxInfoRX = dev.Comm.client.HdlcSettings.MaxInfoTX = 128;
            try
            {
                GXByteBuffer bb = new GXByteBuffer(dev.Comm.SNRMRequest());
                while (bb.Size - 4 > 0)
                {
                    bb.Move(4, 3, bb.Size - 4);
                    --bb.Data[2];
                    try
                    {
                        reply.Clear();
                        dev.Comm.ReadDataBlock(bb.Array(), "HDLC test #5. Invalid frame.", 1, tryCount, reply);
                        passed = false;
                        break;
                    }
                    catch (TimeoutException)
                    {
                    }
                    catch (Exception)
                    {
                        passed = false;
                    }
                }
            }
            finally
            {
                dev.Comm.client.HdlcSettings.MaxInfoRX = rx;
                dev.Comm.client.HdlcSettings.MaxInfoTX = tx;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.SNRMRequest(), "HDLC test #5. SNRM request", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                GXConformanceTests.AddInfo(test, dev, output.Info, "Disconect SNRM request succeeded. MaxInfoLengthTransmit: " + dev.Comm.client.HdlcSettings.MaxInfoTX +
                    " MaxInfoLengthReceive: " + dev.Comm.client.HdlcSettings.MaxInfoRX + " WindowSizeTransmit: " +
                    dev.Comm.client.HdlcSettings.WindowSizeTX + " WindowSizeReceive: " + dev.Comm.client.HdlcSettings.WindowSizeRX);
            }
            catch (Exception ex)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "SNRM request failed. " + ex.Message);
                passed = false;
            }

            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc5\">Test #5 failed.</a>");
            }
        }

        /// <summary>
        /// Send Disc and then send invalid SNRM frame and Disc and check that meter is in Normal Disconnected Mode.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_N3
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_FRAME_N3(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #6.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #6. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Meter returns DisconnectMode.");
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
                passed = false;
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.SNRMRequest();
                data[1] = 0;
                dev.Comm.ReadDataBlock(data, "HDLC test #6. SNRM request", 1, tryCount, reply);
                GXConformanceTests.AddError(test, dev, output.Errors, "SNRM request failed.");
                passed = false;
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddInfo(test, dev, output.Info, "SNRM request succeeded.");
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #6. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "SNRM request failed. " + ex.Message);
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc6\">Test #6 failed.</a>");
            }
        }

        /// <summary>
        /// Send Disc and then send invalid SNRM frame where length is too long. Send Disc and check that meter is in Normal Disconnected Mode.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_N4
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_FRAME_N4(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #7.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #7. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Meter returns DisconnectMode.");
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.SNRMRequest();
                ++data[2];
                dev.Comm.ReadDataBlock(data, "HDLC test #7. SNRM request", 1, tryCount, reply);
                GXConformanceTests.AddError(test, dev, output.Errors, "SNRM request failed. ");
                passed = false;
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddInfo(test, dev, output.Info, "Illegal frame succeeded.");
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #7. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }

            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc7\">Test #7 failed.</a>");
            }
        }

        /// <summary>
        /// Send SNRM and then send Unknown command identifier.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_N5
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_FRAME_N5(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #8.\r\n");
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #8. SNRM request", 1, tryCount, reply);
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
                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format("DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }
            //SubTest 1: Unknown command identifier
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.CustomHdlcFrameRequest(0xFF, null);
                dev.Comm.ReadDataBlock(data, "HDLC test #8. Unknown command", 1, tryCount, reply);
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.UnacceptableFrame)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Unknown command succeeded. " + ex.Message);
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            // SubTest 2: Check that the HDLC layer can be initialised
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #8. SNRM request", 1, tryCount, reply);
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
                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format("DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #8. Disconnect request", 1, tryCount, reply);
                if (dev.Comm.client.InterfaceType == InterfaceType.HDLC)
                {
                    dev.Comm.ParseUAResponse(reply.Data);
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }

            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc8\">Test #8 failed.</a>");
            }
        }

        /// <summary>
        /// Send Disc and then send illegal frame. Send Disc and check that meter is in Normal Disconnected Mode.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_N7
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_FRAME_N7(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #9.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #9. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                GXByteBuffer bb = new GXByteBuffer(new byte[] { 0x81, 0x80, 0x12, 0x05, 0x01, 0x80, 0x06, 0x01, 0x80, 0x07, 0x04, 0x00, 0x00, 0x00, 0x01, 0x08, 0x04, 0x00, 0x00, 0x00, 0x01 });
                byte[] data = dev.Comm.client.CustomHdlcFrameRequest(0x94, bb);
                dev.Comm.ReadDataBlock(data, "HDLC test #9. Unknown command", 1, tryCount, reply);
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.Rejected)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Unknown command succeeded. (Meter rejects the frame)");
                }
                else if (ex.ErrorCode == (int)ErrorCode.UnacceptableFrame)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Unknown command succeeded. (Unacceptable Frame)");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                GXConformanceTests.AddInfo(test, dev, output.Info, "Unknown command succeeded (timeout).");
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #9. Disconnect request", 1, tryCount, reply);
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.DisconnectMode)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Meter returns DisconnectMode.");
                }
                else if (ex.ErrorCode == (int)ErrorCode.Rejected)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Meter rejects the frame.");
                }
                else
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format("DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                GXConformanceTests.AddInfo(test, dev, output.Info, "Disconnect request failed (timeout).");
                passed = false;
            }

            try
            {
                reply.Clear();
                GXByteBuffer bb = new GXByteBuffer(new byte[] { 0x81, 0x80, 0x12, 0x05, 0x01, 0x80, 0x06, 0x01, 0x80, 0x07, 0x04, 0x00, 0x00, 0x00, 0x01, 0x08, 0x04, 0x00, 0x00, 0x00, 0x01 });
                byte[] data = dev.Comm.client.CustomHdlcFrameRequest(0x94, bb);
                for (byte pos = 0; pos != data.Length; ++pos)
                {
                    if (data[pos] == 0x94)
                    {
                        data[pos] = 0x93;
                        break;
                    }
                }
                dev.Comm.ReadDataBlock(data, "HDLC test #9. Illecal frame.", 1, tryCount, reply);
                passed = false;
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddInfo(test, dev, output.Info, "Unknown command succeeded (timeout).");
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc9\">Test #9 failed.</a>");
            }
        }

        /// <summary>
        /// Send Disc and then send SNRM frame where CRC is wrong. Then send Disc and check that meter is in Normal Disconnected Mode.
        /// </summary>
        /// <remarks>
        /// DLMS CCT: T_HDLC_FRAME_N8
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_FRAME_N8(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #10.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #10. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.SNRMRequest();
                ++data[data.Length - 2];
                dev.Comm.ReadDataBlock(data, "HDLC test #10. Unknown command", 1, tryCount, reply);
                passed = false;
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddInfo(test, dev, output.Info, "Invalid frame succeeded..");
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #10. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc10\">Test #10 failed.</a>");
            }
        }

        /// <summary>
        /// Send frame where server address is three bytes.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_HDLC_ADDRESS_N7.
        /// </remarks>
        private static void T_HDLC_ADDRESS_N7(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #11.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #10. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = Gurux.Common.GXCommon.HexToBytes("7EA0090002052193AFD07E");
                dev.Comm.ReadDataBlock(data, "HDLC test #11. SNRM", 1, tryCount, reply);
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.UnacceptableFrame)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Meter returns Unacceptable Frame.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (TimeoutException)
            {
                //This should happened.
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #11. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc11\">Test #11 failed.</a>");
            }
        }

        /// <summary>
        /// Try to connect using max frame size 2030
        /// </summary>
        /// <remarks>
        /// This is DLMS CCT T_HDLC_NDM2NRM_P1.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_NDM2NRM_P1(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #12.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #12. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                dev.Comm.client.HdlcSettings.MaxInfoRX = dev.Comm.client.HdlcSettings.MaxInfoTX = 2030;
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #12. max frame size.", 1, tryCount, reply);
                dev.Comm.client.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            finally
            {
                dev.Comm.client.HdlcSettings.MaxInfoRX = dev.Comm.client.HdlcSettings.MaxInfoTX = 128;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #12. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc12\">Test #12 failed.</a>");
            }
        }

        /// <summary>
        /// Try to connect using window size 4
        /// </summary>
        /// <remarks>
        /// This is DLMS CCT T_HDLC_NDM2NRM_P2.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_NDM2NRM_P2(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #13.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #13. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                dev.Comm.client.HdlcSettings.WindowSizeRX = dev.Comm.client.HdlcSettings.WindowSizeTX = 4;
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #13. Window size 4.", 1, tryCount, reply);
                dev.Comm.client.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            finally
            {
                dev.Comm.client.HdlcSettings.WindowSizeRX = dev.Comm.client.HdlcSettings.WindowSizeTX = 1;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #13. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc13\">Test #13 failed.</a>");
            }
        }

        /// <summary>
        /// Send AARQ.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_HDLC_INFO_P1.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_INFO_P1(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #14.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #14. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #14. SNRMRequest.", 1, tryCount, reply);
                dev.Comm.client.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                try
                {
                    dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "HDLC test #14. AARQRequest.", 1, tryCount, reply);
                    dev.Comm.client.ParseAAREResponse(reply.Data);
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #14. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc14\">Test #14 failed.</a>");
            }
        }

        /// <summary>
        /// Send AARQ in segments.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_HDLC_INFO_P1.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test15(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #15.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #15. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #15. SNRMRequest.", 1, tryCount, reply);
                dev.Comm.client.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                try
                {
                    dev.Comm.client.HdlcSettings.MaxInfoTX = 4;
                    dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "HDLC test #15. AARQRequest.", 1, tryCount, reply);
                    dev.Comm.client.ParseAAREResponse(reply.Data);
                }
                catch (Exception)
                {
                    passed = false;
                }
                finally
                {
                    dev.Comm.client.HdlcSettings.MaxInfoTX = 128;
                }
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #15. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc15\">Test #15 failed.</a>");
            }
        }

        /// <summary>
        /// Send frame that don't fit to HDLC frame.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_HDLC_INFO_N1.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_INFO_N1(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #16.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #16. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #16. SNRMRequest.", 1, tryCount, reply);
                dev.Comm.client.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                try
                {
                    reply.Clear();
                    GXByteBuffer bb = new GXByteBuffer();
                    ++dev.Comm.client.HdlcSettings.MaxInfoTX;
                    bb.Capacity = dev.Comm.client.HdlcSettings.MaxInfoTX;
                    bb.Size = bb.Capacity;
                    byte[] data = dev.Comm.client.CustomHdlcFrameRequest(0x10, bb);
                    dev.Comm.ReadDataBlock(data, "HDLC test #16. AARQRequest.", 1, tryCount, reply);
                    passed = false;
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode != (int)ErrorCode.UnacceptableFrame)
                    {
                        passed = false;
                    }
                }
                catch (Exception)
                {
                    passed = false;
                }
                finally
                {
                    --dev.Comm.client.HdlcSettings.MaxInfoTX;
                }
            }
            try
            {
                if (passed)
                {
                    reply.Clear();
                    byte[] data = dev.Comm.client.SNRMRequest();
                    dev.Comm.ReadDataBlock(data, "HDLC test #16. SNRMRequest.", 1, tryCount, reply);
                    dev.Comm.client.ParseUAResponse(reply.Data);
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #16. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc16\">Test #16 failed.</a>");
            }
        }

        /// <summary>
        /// Send SNRM and then Receiver ready with wrong sequence number.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_HDLC_INFO_N2.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_INFO_N2(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #17.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #17. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #17. SNRMRequest.", 1, tryCount, reply);
                dev.Comm.client.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                try
                {
                    reply.Clear();
                    byte[] data = dev.Comm.client.CustomHdlcFrameRequest(0x31, null);
                    dev.Comm.ReadDataBlock(data, "HDLC test #17. ReceiverReady.", 1, tryCount, reply);
                    passed = false;
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode != (int)ErrorCode.UnacceptableFrame)
                    {
                        passed = false;
                    }
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            try
            {
                if (passed)
                {
                    reply.Clear();
                    byte[] data = dev.Comm.client.SNRMRequest();
                    dev.Comm.ReadDataBlock(data, "HDLC test #17. SNRMRequest.", 1, tryCount, reply);
                    dev.Comm.client.ParseUAResponse(reply.Data);
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #17. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc17\">Test #17 failed.</a>");
            }
        }

        /// <summary>
        /// Send wrong sequence number.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_HDLC_INFO_N3.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_INFO_N3(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #18.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #18. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #18. SNRMRequest.", 1, tryCount, reply);
                dev.Comm.client.ParseUAResponse(reply.Data);
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                try
                {
                    reply.Clear();
                    GXByteBuffer bb = new GXByteBuffer();
                    bb.SetHexString("E6E600601DA109060760857405080101BE10040E01000000065F1F040060FE9FFFFF");
                    byte[] data = dev.Comm.client.CustomHdlcFrameRequest(0x12, bb);
                    dev.Comm.ReadDataBlock(data, "HDLC test #18. Wrong N(S) sequence number.", 1, tryCount, reply);
                    //Meter should reply RR.
                    if (reply.Data.Size != 0 || reply.FrameId != 0x11)
                    {
                        passed = false;
                    }
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode != (int)ErrorCode.UnacceptableFrame)
                    {
                        passed = false;
                    }
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            try
            {
                if (passed)
                {
                    reply.Clear();
                    byte[] data = dev.Comm.client.SNRMRequest();
                    dev.Comm.ReadDataBlock(data, "HDLC test #18. SNRMRequest.", 1, tryCount, reply);
                    dev.Comm.client.ParseUAResponse(reply.Data);
                }
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #18. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc18\">Test #18 failed.</a>");
            }
        }

        /// <summary>
        /// Start communicating without sending SNRM before AARQ.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_HDLC_NDMOP_N1.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_NDMOP_N1(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #19.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #19. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "HDLC test #19. AARQ request.", 1, tryCount, reply);
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
                    GXConformanceTests.AddError(test, dev, output.Errors, string.Format("DisconnectMode expected, but meter returns {0}.", (ErrorCode)ex.ErrorCode));
                    passed = false;
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddInfo(test, dev, output.Info, "Unknown command succeeded (timeout).");
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #19. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc19\">Test #19 failed.</a>");
            }
        }

        /// <summary>
        /// Send frame where client address is two bytes.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_HDLC_ADDRESS_N1.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_ADDRESS_N1(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #20.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #20. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();

                byte[] data = Gurux.Common.GXCommon.HexToBytes("7EA00B0002040100219361807E");
                dev.Comm.ReadDataBlock(data, "HDLC test #20. SNRM request.", 1, tryCount, reply);
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.UnacceptableFrame)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Meter returns UnacceptableFrame.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddInfo(test, dev, output.Info, "Invalid CRC succeeded (timeout).");
            }
            catch (Exception)
            {
                passed = false;
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #20. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc20\">Test 20 failed.</a>");
            }
        }

        /// <summary>
        /// Send unknown destination.
        /// </summary>
        /// <remarks>
        /// This is DLMS Conformance test: T_HDLC_ADDRESS_N4.
        /// </remarks>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void T_HDLC_ADDRESS_N4(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #21.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #21. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }
            int serverAddress = dev.Comm.client.ServerAddress;
            try
            {
                reply.Clear();
                dev.Comm.client.ServerAddress = GXDLMSClient.GetServerAddress(16, Convert.ToInt32(dev.PhysicalAddress));
                dev.Comm.ReadDataBlock(dev.Comm.SNRMRequest(), "HDLC test #21. SNRM request.", 1, tryCount, reply);
                passed = false;
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.UnacceptableFrame)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Meter returns UnacceptableFrame.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (TimeoutException)
            {
                GXConformanceTests.AddInfo(test, dev, output.Info, "Invalid destination succeeded (timeout).");
            }
            catch (Exception)
            {
                passed = false;
            }
            //SubTest 1. Unknown destination address on one byte
            if (passed)
            {
                try
                {
                    reply.Clear();
                    dev.Comm.client.ServerAddress = 2;
                    dev.Comm.ReadDataBlock(dev.Comm.SNRMRequest(), "HDLC test #21. SNRM request.", 1, tryCount, reply);
                    passed = false;
                }
                catch (GXDLMSException ex)
                {
                    if (ex.ErrorCode == (int)ErrorCode.UnacceptableFrame)
                    {
                        GXConformanceTests.AddInfo(test, dev, output.Info, "Meter returns UnacceptableFrame.");
                    }
                    else
                    {
                        passed = false;
                    }
                }
                catch (TimeoutException)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Invalid destination succeeded (timeout).");
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
            dev.Comm.client.ServerAddress = serverAddress;
            if (!passed)
            {
                try
                {
                    reply.Clear();
                    dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #21. Disconnect request", 1, tryCount, reply);
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
                catch (TimeoutException)
                {
                    GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
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
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc21\">Test 21 failed.</a>");
            }
        }

        /// <summary>
        /// Send same HDLC packet twice and check that meter can hanle this. Then read next data.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="settings"></param>
        /// <param name="dev"></param>
        /// <param name="output"></param>
        private static void Test101(GXConformanceTest test, GXConformanceSettings settings, GXDLMSDevice dev, GXOutput output, int tryCount)
        {
            GXReplyData reply = new GXReplyData();
            bool passed = true;
            test.OnTrace(test, "Starting HDLC tests #101.\r\n");
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #101. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception ex)
            {
                passed = false;
                GXConformanceTests.AddError(test, dev, output.Errors, "Disconnect request failed. " + ex.Message);
            }
            try
            {
                reply.Clear();
                byte[] data = dev.Comm.client.SNRMRequest();
                dev.Comm.ReadDataBlock(data, "HDLC test #101. SNRM", 1, tryCount, reply);
                dev.Comm.ParseUAResponse(reply.Data);
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.client.AARQRequest(), "HDLC test #101. AARQ", 1, tryCount, reply);
                dev.Comm.ParseAAREResponse(reply.Data);
                reply.Clear();
                GXDLMSData ldn = new GXDLMSData("0.0.42.0.0.255");
                data = dev.Comm.Read(ldn, 1);
                dev.Comm.ReadDataBlock(data, "HDLC test #101. Read LDN #1", 1, reply);
                reply.Clear();
                //RR
                dev.Comm.ReadDataBlock(data, "HDLC test #101. Read LDN #2", 1, reply);
                if ((reply.FrameId & 0xC) != 0)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Meter Don't return ReceiveReady.");
                    passed = false;
                }
                reply.Clear();
                //Read value again.
                data = dev.Comm.Read(ldn, 1);
                dev.Comm.ReadDataBlock(data, "HDLC test #101. Read LDN #3", 1, reply);
                reply.Clear();
            }
            catch (GXDLMSException ex)
            {
                if (ex.ErrorCode == (int)ErrorCode.UnacceptableFrame)
                {
                    GXConformanceTests.AddInfo(test, dev, output.Info, "Meter returns Unacceptable Frame.");
                }
                else
                {
                    passed = false;
                }
            }
            catch (Exception)
            {
                //This should happened.
            }
            try
            {
                reply.Clear();
                dev.Comm.ReadDataBlock(dev.Comm.DisconnectRequest(true), "HDLC test #11. Disconnect request", 1, tryCount, reply);
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
            catch (TimeoutException)
            {
                GXConformanceTests.AddError(test, dev, output.Errors, "Timeout.");
                passed = false;
            }
            catch (Exception)
            {
                passed = false;
            }
            if (passed)
            {
                test.OnTrace(test, "Passed.\r\n");
            }
            else
            {
                test.OnTrace(test, "Failed.\r\n");
                GXConformanceTests.AddError(test, dev, output.Errors, "<a href=\"https://www.gurux.fi/gurux.dlms.ctt.tests#hdlc101\">Test #101 failed.</a>");
            }
        }
    }
}
