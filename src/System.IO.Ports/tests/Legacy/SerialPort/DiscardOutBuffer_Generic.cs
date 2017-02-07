// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO.Ports;
using System.Diagnostics;
using System.IO.PortsTests;
using Legacy.Support;
using Xunit;

public class DiscardOutBuffer_Generic : PortsTest
{
    #region Test Cases

    [ConditionalFact(nameof(HasOneSerialPort))]
    public void DiscardWithoutOpen()
    {
        using (SerialPort com = new SerialPort(TCSupport.LocalMachineSerialInfo.FirstAvailablePortName))
        {
            Debug.WriteLine("Verifying Discard method throws exception without a call to Open()");
            VerifyDiscardException(com, typeof(InvalidOperationException));
        }
    }


    [ConditionalFact(nameof(HasOneSerialPort))]
    public void DiscardAfterFailedOpen()
    {
        using (SerialPort com = new SerialPort("BAD_PORT_NAME"))
        {
            Debug.WriteLine("Verifying read Discard throws exception with a failed call to Open()");

            //Since the PortName is set to a bad port name Open will thrown an exception
            //however we don't care what it is since we are verifying a read method
            Assert.ThrowsAny<Exception>(() => com.Open());
            VerifyDiscardException(com, typeof(InvalidOperationException));
        }
    }


    [ConditionalFact(nameof(HasOneSerialPort))]
    public void DiscardAfterClose()
    {
        using (SerialPort com = new SerialPort(TCSupport.LocalMachineSerialInfo.FirstAvailablePortName))
        {
            Debug.WriteLine("Verifying Discard method throws exception after a call to Cloes()");
            com.Open();
            com.Close();

            VerifyDiscardException(com, typeof(InvalidOperationException));
        }
    }


    [ConditionalFact(nameof(HasOneSerialPort))]
    public void DiscardAfterOpen()
    {
        using (SerialPort com = new SerialPort(TCSupport.LocalMachineSerialInfo.FirstAvailablePortName))
        {
            Debug.WriteLine("Verifying Discard method does not throw an exception after a call to Open()");
            com.Open();
            VerifyDiscardException(com, null);

            if (0 != com.BytesToRead)
            {
                Debug.WriteLine("Error!!! BytesToRead is not 0");
            }
        }
    }
    #endregion

    #region Verification for Test Cases
    private void VerifyDiscardException(SerialPort com, Type expectedException)
    {
        try
        {
            com.DiscardOutBuffer();
            if (null != expectedException)
            {
                Fail("ERROR!!!: No Excpetion was thrown");
            }
        }
        catch (Exception e)
        {
            if (null == expectedException)
            {
                Fail("ERROR!!!: No Excpetion was expected and {0} was thrown", e.GetType());
            }

            if (e.GetType() != expectedException)
            {
                Fail("ERROR!!!: {0} exception was thrown expected {1}", e.GetType(), expectedException);
            }
        }
    }
    #endregion
}